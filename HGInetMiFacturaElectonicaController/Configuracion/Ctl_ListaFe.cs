using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HGInetUBLv2_1.DianListas.ListaItem;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_ListaFe
	{
		/// <summary>
		/// Obtiene las listas que se usan para FE
		/// </summary>
		/// <param name="codigo_lista">números de codigos para consulta separados por el caracter coma (,) ó enviando el caracter (*) para obtenerlas todas</param>
		/// <param name="todas">True si son todas las liestas</param>
		/// <returns>Objeto con las Lista de FE</returns>
		public List<ListaFE> Obtener(string codigo_lista, bool todas)
		{
			try
			{

				//Convierte Numeros en una lista.
				List<string> lista_codigo = Coleccion.ConvertirLista(codigo_lista);

				List<ListaFE> retorno = new List<ListaFE>();

				if (todas == true || lista_codigo.Contains("1"))
				{
					ListaCodigoPostal lista = new ListaCodigoPostal();
					string cod_lista = "1";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("2"))
				{
					ListaMediosPago lista = new ListaMediosPago();
					string cod_lista = "2";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("3"))
				{
					ListaPaises lista = new ListaPaises();
					string cod_lista = "3";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("4"))
				{
					ListaProducto lista = new ListaProducto();
					string cod_lista = "4";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("5"))
				{
					ListaTipoCodigoProducto lista = new ListaTipoCodigoProducto();
					string cod_lista = "5";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("6"))
				{
					ListaTipoEntrega lista = new ListaTipoEntrega();
					string cod_lista = "6";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("7"))
				{
					ListaTipoMoneda lista = new ListaTipoMoneda();
					string cod_lista = "7";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("8"))
				{
					ListaUnidadesMedida lista = new ListaUnidadesMedida();
					string cod_lista = "8";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("9"))
				{
					ListaMunicipio lista = new ListaMunicipio();
					string cod_lista = "9";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}

				if (todas == true || lista_codigo.Contains("10"))
				{
					ListaDepartamentos lista = new ListaDepartamentos();
					string cod_lista = "10";
					string Descripcion_lista = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ListasFE>(Convert.ToInt16(cod_lista)));

					Llenar_lista(retorno, lista.Items, cod_lista, Descripcion_lista);

				}


				return retorno;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public void Llenar_lista(List<ListaFE> retorno, object lista, string codigo_lista, string descripcion_lista)
		{

			var lista_obj = (dynamic)null;
			lista_obj = lista;

			foreach (ListaItem item in lista_obj)
			{
				ListaFE item_lista = new ListaFE();
				item_lista.CodigoItem = item.Codigo;
				item_lista.DescripcionItem = item.Descripcion;
				item_lista.CodigoLista = codigo_lista;
				item_lista.DescripcionLista = descripcion_lista;
				retorno.Add(item_lista);
			}
		}

	}
}
