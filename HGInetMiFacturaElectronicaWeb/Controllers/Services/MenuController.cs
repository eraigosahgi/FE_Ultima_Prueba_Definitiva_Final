using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class MenuController : ApiController
	{
		//Menu Dinamico
		public string ListaNodos = "";
		public int NivelPrincipal = 0;
		public int Orden = 1;
		List<ObjMenuGlobal> ListaGlobal = new List<ObjMenuGlobal>();
		//Menu Dinamico

		[HttpGet]
		[Route("api/ObtenerMenu")]
		public IHttpActionResult ObtenerMenu(string Facturador)
		{
			string Resultado = ActivarMenu("jflores", Facturador);
			return Ok(Resultado);
		}			

		public string ActivarMenu(string Usuario, string Empresa)
		{

			Ctl_Usuario ctlUsuario = new Ctl_Usuario();

			TblOpciones opciones = new TblOpciones();

			//Obtengo los datos de la tabla para crear el Menu            
			var DatosMenu = ctlUsuario.ObtenerPermisos(Usuario, Empresa);

			List<ObjMenu> ListMenu = new List<ObjMenu>();
			int i = 0;
			foreach (var item in DatosMenu)
			{


				if (i > 0)
				{
					///Valido la existencia de padres en la lista
					var Padre = from LMenu in ListMenu
								where LMenu.Id.Equals(item.IntIdDependencia)
								select LMenu;

					//////Validar si el registro tiene un padre ya que si se ingresar un hijo sin padre, este genera un error al convertir la lista de Nodos
					if (Padre.Count() > 0)
						ListMenu.Add(new ObjMenu(item.IntId, item.IntIdDependencia, item.StrDescripcion, item.StrClaseMenu, item.StrRutaMenu));
					i++;
				}
				else
				{
					ListMenu.Add(new ObjMenu(item.IntId, item.IntIdDependencia, item.StrDescripcion, item.StrClaseMenu, item.StrRutaMenu));
					i++;
				}
			}
			///Convierte la lista en un Nodo         
			var rootArbols = Arbol<ObjMenu>.CreateTree(ListMenu, l => l.Id, l => l.ParentId);

			var rootArbol = rootArbols.Single();

			NivelPrincipal = 1;
			//ListaNodos += "-- Orden: " + Orden + " Nivel: " + NivelPrincipal + " Padre: " + rootArbol.Value.Descripcion;
			ListaGlobal.Add(new ObjMenuGlobal(rootArbol.Value.Id, rootArbol.Value.ParentId, rootArbol.Value.Descripcion, rootArbol.Value.Clase, rootArbol.Value.Ruta, Orden, NivelPrincipal));
			NivelPrincipal = 2;
			foreach (var Arbol in rootArbol)
			{
				NodosHijosPadres(Arbol, NivelPrincipal);
			}

			/////////
			int nivelActual = 0;
			int nivelAnterior = 0;


			//Se define cabecera del menú principal
			string Menu = @"<ul class='navigation navigation-main navigation-accordion' id='MenuPrincipal' >
                             <li class='navigation-header'><span style = 'margin: -10px'> Menú </span> <i class='icon-menu' title='Menú'></i></li>";

			//Se itero la lista de permisos para ir creado los tag del menú
			foreach (var item in ListaGlobal)
			{


				//Cierra del tag cuando es submenu
				if (nivelAnterior > 0)
					// if (string.IsNullOrEmpty(item.Ruta))
					if (item.Nivel < nivelAnterior)
						for (int j = item.Nivel; j <= nivelAnterior - 1; j++)
						{
							Menu += CerrarTagMenu();
						}

				//////////////////////////////////////////////////////////////////////////////////////////////                                                                                                                
				//opción para  submenu
				//si ruta es vacia, creo un tag de menu
				//Se abre un tag de opciones
				if (string.IsNullOrEmpty(item.Ruta))
					Menu += GenerarTagMenu(item.Id, item.Descripcion, item.Clase, (nivelAnterior == 0) ? "class='active'" : "");

				//si tengo ruta, entonces creo una opcion
				//Opción de Menú con opción a redirección
				if (!string.IsNullOrEmpty(item.Ruta))
					Menu += GenerarTagMenuopt(item.Id, item.Ruta, item.Clase, item.Descripcion);

				nivelAnterior = item.Nivel;
				//////////////////////////////////////////////////////////////////////////////////////////////

			}

			var Resultado = Menu + "</ul> </li> </ul>";
			////Guardo el Menu en un Cookies para no tener que crear nuevamente el Menu
			//string NuevoCokkies = Menu + "</ul> </li> </ul>";
			//NuevoCokkies = NuevoCokkies.Replace("<", "xxx").Replace(">", "zzz");
			return Resultado;

		}

		#region Menu Dinamico
		/// <summary>
		/// Crea un tag de menú para incluir items dentro 
		/// </summary>
		/// <param Descripcion="IdMenu"></param>
		/// <param Descripcion="DescripcionTag"></param>
		/// <param Descripcion="IconoTag"></param>
		/// <returns></returns>
		public string GenerarTagMenu(int IdMenu, string DescripcionTag, string IconoTag, string Activo)
		{

			return "<li  " + Activo + "  id='menu_" + IdMenu + "'> " +
						"<a href = '#' class='dropdown-item rounded'><i class='" + IconoTag + "'></i><span style = 'margin: -10px'> " + DescripcionTag + " </span></a>  " +
							"<ul  id='ul_" + IdMenu + "'>";

		}
		/// <summary>
		/// //Cierra un tag cuando es un submenu
		/// </summary>
		/// <returns></returns>
		public string CerrarTagMenu()
		{
			return @"</ul> </li>";
		}
		/// <summary>
		/// Crea una opción en el menú con el parametro url para redirección
		/// </summary>
		/// <param Descripcion="IdMenu"></param>
		/// <param Descripcion="url"></param>
		/// <param Descripcion="Icono"></param>
		/// <param Descripcion="DescripcionMenu"></param>
		/// <returns></returns>
		public string GenerarTagMenuopt(int IdMenu, string url, string Icono, string DescripcionMenu)
		{
			return @"<li  id='menu_" + IdMenu + "'>" +
						"<a id='link_" + IdMenu + "' class='dropdown-item rounded' href='" + url + "'><i class='" + Icono + "'></i><span style = 'margin: -10px'>" + DescripcionMenu + "</span></a>" +
					"</li>";
		}


		private void NodosHijosPadres(Arbol<ObjMenu> rootArbol, int nivel)
		{
			int NodoInterno = 1;
			NivelPrincipal = nivel;
			foreach (var Arbol in rootArbol)
			{

				//Valido si estoy en el mismo nodo Hijo para no crear nuevamente el padre en la nueva lista
				if (NodoInterno == 1)
				{
					foreach (var Arbol2 in Arbol.Ancestors)
					{
						Orden += 1;
						//ListaNodos += "-- Orden: " + Orden + " Nivel: " + NivelPrincipal + " Padre: " + Arbol2.Value.Descripcion;
						ListaGlobal.Add(new ObjMenuGlobal(Arbol2.Value.Id, Arbol2.Value.ParentId, Arbol2.Value.Descripcion, Arbol2.Value.Clase, Arbol2.Value.Ruta, Orden, NivelPrincipal));
						break;
					}
				}
				Orden += 1;
				//Anexo el items a la lista como hijo
				//ListaNodos += "-- Orden: " + Orden + " Nivel: " + (NivelPrincipal + 1) + " Hijo: " + Arbol.Value.Descripcion;
				ListaGlobal.Add(new ObjMenuGlobal(Arbol.Value.Id, Arbol.Value.ParentId, Arbol.Value.Descripcion, Arbol.Value.Clase, Arbol.Value.Ruta, Orden, NivelPrincipal + 1));
				//Valido si este hijo tiene mas hijos
				foreach (var Arbol2 in Arbol)
				{
					Orden += 1;
					if (Arbol2.Children.Count() == 0)
					{
						//ListaNodos += "-- Orden: " + Orden + " Nivel: " + (NivelPrincipal + 2) + " Hijo: " + Arbol2.Value.Descripcion;
						ListaGlobal.Add(new ObjMenuGlobal(Arbol2.Value.Id, Arbol2.Value.ParentId, Arbol2.Value.Descripcion, Arbol2.Value.Clase, Arbol2.Value.Ruta, Orden, NivelPrincipal + 2));
					}
					//Contar si el segundo hijo tiene otro hijo

					NodosHijosPadres(Arbol2, NivelPrincipal + 2);

					NivelPrincipal = NivelPrincipal - 2;
				}

				NodoInterno += 1;

			}

		}

	}

	/// <summary>
	/// Tipo de Objeto en el que deseo converir mi consulta de permisos
	/// </summary>
	public class ObjMenu
	{
		public int Id;
		public int? ParentId;
		public string Descripcion;
		public string Clase;
		public string Ruta;

		public ObjMenu(int Id, int? ParentId, string Descripcion, string Clase, string Ruta)
		{
			this.Id = Id;
			this.ParentId = ParentId;
			this.Descripcion = Descripcion;
			this.Clase = Clase;
			this.Ruta = Ruta;
		}
	}

	/// <summary>
	/// Tipo de Objeto en el que deseo convertir mi objeto de permisos a mi objeto que llenara el Menu
	/// </summary>

	public class ObjMenuGlobal
	{
		public int Id;
		public int? ParentId;
		public string Descripcion;
		public string Clase;
		public string Ruta;
		public int Orden;
		public int Nivel;

		public ObjMenuGlobal(int Id, int? ParentId, string Descripcion, string Clase, string Ruta, int Orden, int Nivel)
		{
			this.Id = Id;
			this.ParentId = ParentId;
			this.Descripcion = Descripcion;
			this.Clase = Clase;
			this.Ruta = Ruta;
			this.Orden = Orden;
			this.Nivel = Nivel;
		}
	}


	#endregion

}



