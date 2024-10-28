﻿using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Seguridad
{
	public class Sesion
	{
		#region Propiedades

		/// <summary>
		/// Datos de la empresa 
		/// </summary>
		public static TblEmpresas DatosEmpresa
		{
			get
			{
				ValidarSesion();
				if (HttpContext.Current.Session["datos_empresa"] == null)
				{
					TblEmpresas empresa = new TblEmpresas();
					HttpContext.Current.Session["datos_empresa"] = empresa;
				}
				return (TblEmpresas)HttpContext.Current.Session["datos_empresa"];
			}
		}

		/// <summary>
		/// Datos del usuario
		/// </summary>
		public static TblUsuarios DatosUsuario
		{
			get
			{
				ValidarSesion();
				if (HttpContext.Current.Session["datos_usuario"] == null)
				{
					TblUsuarios usuario = new TblUsuarios();
					HttpContext.Current.Session["datos_usuario"] = usuario;
				}
				return (TblUsuarios)HttpContext.Current.Session["datos_usuario"];
			}
		}

		/// <summary>
		/// Datos del usuario
		/// </summary>
		public static TblUsuariosPagos DatosUsuarioPagos
		{
			get
			{
				ValidarSesionPagos();
				if (HttpContext.Current.Session["datos_usuario_pagos"] == null)
				{
					TblUsuariosPagos usuario = new TblUsuariosPagos();
					HttpContext.Current.Session["datos_usuario_pagos"] = usuario;
				}
				return (TblUsuariosPagos)HttpContext.Current.Session["datos_usuario_pagos"];
			}
		}


		/// <summary>
		/// Lista de opciones de permiso del usuario.
		/// </summary>
		public static string PermisosUsuario
		{
			get
			{
				ValidarSesion();
				if (HttpContext.Current.Session["PermisosUsuario"] == null)
				{
					HttpContext.Current.Session["PermisosUsuario"] = string.Empty;
				}
				return HttpContext.Current.Session["PermisosUsuario"].ToString();
			}
		}

		#endregion

		/// <summary>
		/// Valida que se encuentren los datos necesarios en la sesión
		/// </summary>
		public static void ValidarSesion()
		{
			HttpContext context = HttpContext.Current;

			if (context.Session == null)
				throw new ApplicationException("No se encontraron los datos de autenticación en la sesión; ingrese nuevamente.");


			if (context.Session["datos_empresa"] == null || context.Session["datos_usuario"] == null)
				throw new ApplicationException("No se encontraron los datos de autenticación en la sesión; ingrese nuevamente.");


			dynamic usuario = context.Session["datos_usuario"];

			Ctl_Usuario controlador = new Ctl_Usuario();
			if (!controlador.ValidarToken(usuario.StrEmpresa, usuario.StrIdSeguridad, usuario.StrToken))
				throw new ApplicationException("Se ha iniciado sesión desde otra ubicación.");
		}


		public static void ValidarSesionPagos()
		{
			HttpContext context = HttpContext.Current;

			if (context.Session == null)
				throw new ApplicationException("No se encontraron los datos de autenticación en la sesión; ingrese nuevamente.");


			if (context.Session["datos_empresa"] == null || context.Session["datos_usuario_pagos"] == null)
				throw new ApplicationException("No se encontraron los datos de autenticación en la sesión; ingrese nuevamente.");


			dynamic usuario = context.Session["datos_usuario_pagos"];

			Ctl_UsuarioPagos controlador = new Ctl_UsuarioPagos();
			if (!controlador.ValidarToken(usuario.StrEmpresaFacturador, usuario.StrIdSeguridad, usuario.StrToken))
				throw new ApplicationException("Se ha iniciado sesión desde otra ubicación.");
		}


		/// <summary>
		/// Limpiar Session
		/// </summary>
		public static void LimpiarSesion()
		{
			HttpContext context = HttpContext.Current;

			context.Session["datos_empresa"] = null;
		}




		/// <summary>
		/// Guarda los datos del usuario en la sesión web
		/// </summary>
		/// <returns>datos de autenticacion</returns>
		public void GuardarSesionWeb(System.Guid id_seguridad)
		{
			try
			{
				//Obtiene los datos del usuario autenticado
				Ctl_Usuario clase_usuario = new Ctl_Usuario();
				TblUsuarios datos_usuario = clase_usuario.ObtenerIdSeguridad(id_seguridad);

				if (datos_usuario != null)
					HttpContext.Current.Session.Add("datos_usuario", datos_usuario);

				//Obtiene la información de la empresa autenticada.
				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				TblEmpresas datos_empresa = clase_empresa.Obtener(datos_usuario.StrEmpresa, false);

				if (datos_empresa != null)
					HttpContext.Current.Session.Add("datos_empresa", datos_empresa);

				CargarPermisos();

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("{0}{1}{2}", "Error al construir la sesíón del usuario con los datos de autenticación. ", excepcion.Message, excepcion.InnerException));
			}
		}


		public void GuardarSesionWebPagos(System.Guid id_seguridad)
		{
			try
			{
				//Obtiene los datos del usuario autenticado
				Ctl_UsuarioPagos clase_usuario = new Ctl_UsuarioPagos();
				TblUsuariosPagos datos_usuario = clase_usuario.ObtenerIdSeguridad(id_seguridad);

				if (datos_usuario != null)
					HttpContext.Current.Session.Add("datos_usuario_pagos", datos_usuario);

				//Obtiene la información de la empresa autenticada.
				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				TblEmpresas datos_empresa = clase_empresa.Obtener(datos_usuario.StrEmpresaFacturador, false);

				if (datos_empresa != null)
					HttpContext.Current.Session.Add("datos_empresa", datos_empresa);


			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("{0}{1}{2}", "Error al construir la sesíón del usuario con los datos de autenticación. ", excepcion.Message, excepcion.InnerException));
			}
		}

		/// <summary>
		/// Obtiene los permisos del usuario y los convierte en lista string.
		/// </summary>
		public static void CargarPermisos()
		{
			try
			{
				Ctl_OpcionesUsuario ctl_opc_usuario = new Ctl_OpcionesUsuario();

				// crea los parámetros para el servicio web
				List<TblOpcionesUsuario> permisos_usuario = ctl_opc_usuario.ObtenerOpcionesUsuarios(DatosUsuario.StrUsuario, DatosEmpresa.StrIdentificacion);

				if (permisos_usuario == null)
					throw new ApplicationException("No se encontraron permisos asignados del usuario.");

				string lista_permisos = string.Empty;

				foreach (TblOpcionesUsuario item in permisos_usuario)
				{
					lista_permisos = string.Format("{0}{1},", lista_permisos, item.IntIdOpcion);
				}

				HttpContext context = HttpContext.Current;
				context.Session["PermisosUsuario"] = lista_permisos;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al obtener los permisos del usuario - {0}.", excepcion.Message), excepcion);
			}
		}

		/// <summary>
		/// Valida que el usuario tenga permiso para el código.
		/// </summary>
		/// <param name="codigo_opcion"></param>
		/// <returns></returns>
		public static bool ValidarPermiso(string codigo_opcion, OperacionesBD operacion_pagina)
		{

			bool respuesta = false;

			if (operacion_pagina == 0)
				return false;

			// valida que el usuario tenga el permiso en la sesión
			if (string.IsNullOrEmpty(PermisosUsuario.Split(',').Where(_permiso => _permiso.Equals(codigo_opcion)).FirstOrDefault()))
				respuesta = false;
			else
			{
				Ctl_OpcionesUsuario clase_permisos = new Ctl_OpcionesUsuario();

				TblOpcionesUsuario datos_permiso = clase_permisos.ObtenerOpcionesUsuarios(DatosUsuario.StrUsuario, DatosUsuario.StrEmpresa, codigo_opcion, true).FirstOrDefault();

				if (datos_permiso != null)
				{
					PropertyDescriptorCollection cols = TypeDescriptor.GetProperties((datos_permiso));

					PropertyDescriptor campo = cols.Find(operacion_pagina.ToString(), true);

					if (campo.Name.Equals(operacion_pagina.ToString()))
					{
						if (Convert.ToBoolean(campo.GetValue(datos_permiso)))
							respuesta = true;
						else
							respuesta = false;
					}
				}
			}

			return respuesta;
		}


	}
}