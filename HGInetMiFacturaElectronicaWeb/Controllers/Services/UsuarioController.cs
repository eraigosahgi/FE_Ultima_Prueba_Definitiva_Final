using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectonicaController;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_Usuario;
using HGInetMiFacturaElectonicaData.Objetos;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class UsuarioController : ApiController
	{
		/// <summary>
		/// Obtiene el usuario de la sesión
		/// </summary>
		/// <returns></returns>
		public IHttpActionResult Get()
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblUsuarios> datos = new List<TblUsuarios>();

				datos.Add(Sesion.DatosUsuario);

				var retorno = datos.Select(d => new
				{
					FechaActualizacion = d.DatFechaActualizacion,
					FechaIngreso = d.DatFechaIngreso,
					FechaCambioClave = d.DatFechaCambioClave,
					Estado = d.IntIdEstado,
					Apellidos = d.StrApellidos,
					Cargo = d.StrCargo,
					Celular = d.StrCelular,
					Clave = d.StrClave,
					Empresa = d.StrEmpresa,
					Extension = d.StrExtension,
					IdCambioClave = d.StrIdCambioClave,
					IdSeguridad = d.StrIdSeguridad,
					Mail = d.StrMail,
					Nombres = d.StrNombres,
					Telefono = d.StrTelefono,
					CodigoUsuario = d.StrUsuario
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Valida la existencia de un usuario
		/// </summary>
		/// <param name="codigo_empresa"></param>
		/// <param name="codigo_usuario"></param>
		/// <param name="clave"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(string codigo_empresa, string codigo_usuario, string clave)
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();
				List<TblUsuarios> datos = ctl_usuario.ValidarExistencia(codigo_empresa, codigo_usuario, clave);

				if (datos == null || datos.Count == 0)
				{
					throw new ApplicationException("Datos de autenticación inválidos.");
				}

				var retorno = datos.Select(d => new
				{
					Token = d.StrIdSeguridad
				});


				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los usuarios por codigo de usuario y empresa.
		/// </summary>
		/// <param name="codigo_usuario"></param>
		/// <returns></returns>
		//public IHttpActionResult Get(string codigo_usuario, string codigo_empresa)
		//{
		//	try
		//	{
		//		//Se coloca esta valiacion para que no puedan enviar *,* por la webapi, se puede enviar el usuario*, pero no la empresa
		//		if (codigo_empresa == "*")
		//			throw new ApplicationException("Error al consultar los datos de los usuarios *");

		//		Sesion.ValidarSesion();

		//		List<TblEmpresas> datosSesion = new List<TblEmpresas>();

		//		datosSesion.Add(Sesion.DatosEmpresa);

		//		TblEmpresas datosempresa = datosSesion.FirstOrDefault();


		//		List<TblUsuarios> datos = new List<TblUsuarios>();
		//		Ctl_Usuario ctl_usuario = new Ctl_Usuario();

		//		if (datosempresa.IntAdministrador)
		//		{
		//			codigo_empresa = "*";

		//			datos = ctl_usuario.ObtenerUsuarios(codigo_usuario, codigo_empresa);
		//		}
		//		else
		//		{
		//			if (datosempresa.IntIntegrador)
		//			{
		//				datos = ctl_usuario.ObtenerListaUsuarios(codigo_usuario, codigo_empresa);
		//			}
		//			else
		//			{
		//				datos = ctl_usuario.ObtenerUsuarios(codigo_usuario, codigo_empresa);
		//			}
		//		}


		//		//ObtenerUsuarios
		//		//List<TblUsuarios> datos = ctl_usuario.ObtenerUsuarios(codigo_usuario, codigo_empresa);


		//		if (datos == null)
		//		{
		//			return NotFound();
		//		}

		//		var retorno = datos.Select(d => new
		//		{
		//			FechaActualizacion = d.DatFechaActualizacion,
		//			FechaCambioClave = d.DatFechaCambioClave,
		//			FechaIngreso = d.DatFechaIngreso,
		//			Estado = d.IntIdEstado,
		//			Apellidos = d.StrApellidos,
		//			Cargo = d.StrCargo,
		//			Celular = d.StrCelular,
		//			Clave = d.StrClave,
		//			Empresa = d.StrEmpresa,
		//			Extension = d.StrExtension,
		//			IdCambioClave = d.StrIdCambioClave,
		//			IdSeguridad = d.StrIdSeguridad,
		//			Mail = d.StrMail,
		//			Nombres = d.StrNombres,
		//			NombreCompleto = string.Format("{0} {1}", d.StrNombres, d.StrApellidos),
		//			Telefono = d.StrTelefono,
		//			Usuario = d.StrUsuario,
		//			RazonSocial = d.TblEmpresas.StrRazonSocial
		//		});

		//		return Ok(retorno);
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}



		///// <summary>
		///// Obtiene los usuarios por codigo de usuario y empresa.
		///// </summary>
		///// <param name="codigo_usuario"></param>
		///// <returns></returns>
		public IHttpActionResult Get(string codigo_usuario, string codigo_empresa,int Desde, int Hasta,string nombre_usuario)
		{
			try
			{
				//Se coloca esta valiacion para que no puedan enviar *,* por la webapi, se puede enviar el usuario*, pero no la empresa
				if (codigo_empresa == "*")
					throw new ApplicationException("Error al consultar los datos de los usuarios *");

				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				List<ObjUsuario> datos = new List<ObjUsuario>();

				Ctl_Usuario ctl_usuario = new Ctl_Usuario();

				if (datosempresa.IntAdministrador)
				{
					//Si viene vacio entonces le colocamos *, de lo contratio dejamos el filtro
					if (string.IsNullOrEmpty(codigo_empresa))
					{
						codigo_empresa = "*";
					}

					datos = ctl_usuario.ConsultaUsuarios(codigo_usuario, codigo_empresa, nombre_usuario, Desde, Hasta);
				}

				else
				{
					if (datosempresa.IntIntegrador)
					{
						if (string.IsNullOrEmpty(codigo_empresa))
						{
							codigo_empresa = datosempresa.StrIdentificacion;
						}
						datos = ctl_usuario.ObtenerListaUsuarios(codigo_usuario, codigo_empresa, nombre_usuario, Desde, Hasta);
					}
					else
					{
						if (string.IsNullOrEmpty(codigo_empresa))
						{
							codigo_empresa = datosempresa.StrIdentificacion;
						}

						datos = ctl_usuario.ConsultaUsuarios(codigo_usuario, codigo_empresa, nombre_usuario,Desde, Hasta);
					}
				}
			
				if (datos == null)
				{
					return NotFound();
				}


				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		public IHttpActionResult Get(string codigo_usuario)
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();
				TblUsuarios datos_usuario = ctl_usuario.ObtenerUsuarios(codigo_usuario, "*").FirstOrDefault();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();
				TblEmpresas datos_empresa = ctl_empresa.Obtener(datos_usuario.StrEmpresa);

				if (datos_usuario == null && datos_empresa == null)
				{
					return NotFound();
				}

				Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
				List<MensajeEnvio> respuesta = clase_email.Bienvenida(datos_empresa, datos_usuario);

				return Ok(respuesta);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Valida la existencia de un usuario para restablecer la Contraseña
		/// </summary>
		/// <param name="codigo_empresa"></param>
		/// <param name="codigo_usuario"></param>        
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]string codigo_empresa, [FromUri]string codigo_usuario)
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();

				string datos = ctl_usuario.ValidarExistenciaRestablecer(codigo_empresa, codigo_usuario);

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// Valida si el codigo de seguridad existe y si tiene vigencia
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri]string clave)
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();

				bool datos = ctl_usuario.RestablecerClave(id_seguridad, clave);

				if (datos)
				{
					return Ok(datos);
				}
				return NotFound();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Valida si el codigo de seguridad existe y si tiene vigencia
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(System.Guid id_seguridad)
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();

				bool datos = ctl_usuario.ValidarIdSeguridad(id_seguridad);

				if (datos)
				{
					return Ok(datos);
				}
				return NotFound();

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Consulta un usuario por el id de Seguridad (StrIdSeguridad)
		/// </summary>
		/// <param name="StrIdSeguridad"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(System.Guid StrIdSeguridad, int Consulta)
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();

				TblUsuarios datos = ctl_usuario.ObtenerIdSeguridad(StrIdSeguridad);
				List<TblUsuarios> resultado = new List<TblUsuarios>();

				resultado.Add(datos);

				var retorno = resultado.Select(d => new
				{
					StrEmpresa = d.StrEmpresa,
					StrUsuario = d.StrUsuario,
					StrClave = d.StrClave,
					StrNombres = d.StrNombres,
					StrApellidos = d.StrApellidos,
					StrCargo = d.StrCargo,
					StrTelefono = d.StrTelefono,
					StrExtension = d.StrExtension,
					StrCelular = d.StrCelular,
					StrMail = d.StrMail,
					IntIdEstado = d.IntIdEstado
				});

				return Ok(retorno);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// Crea el Usuario
		/// </summary>
		/// <param name="StrEmpresa"></param>        
		/// <param name="StrUsuario"></param>                
		///  <param name="StrClave"></param>        
		///  <param name="StrNombres"></param>        
		///  <param name="StrApellidos"></param>        
		/// <param name="StrMail"></param>        
		/// <param name="StrTelefono"></param>         
		/// <param name="StrExtension"></param> 
		/// <param name="StrCelular"></param> 
		/// <param name="StrCargo"></param> 
		/// <param name="IntIdEstado"></param> 
		/// <param name="Tipo"></param>                                
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]string StrEmpresa, [FromUri]string StrUsuario, [FromUri] string StrNombres, [FromUri]string StrApellidos, [FromUri]string StrMail, [FromUri]string StrTelefono, [FromUri]string StrExtension, [FromUri]string StrCelular, [FromUri]string StrCargo, [FromUri] short IntIdEstado, [FromUri]int Tipo)//1 Nuevo -- 2 Actualizar
		{
			try
			{
				Ctl_Usuario ctl_usuario = new Ctl_Usuario();
				//Quitamos espacios
				StrEmpresa = StrEmpresa.Trim();

				if (Tipo == 1)
				{
					TblUsuarios usuario = new TblUsuarios();
					usuario.StrEmpresa = StrEmpresa;
					usuario.StrUsuario = StrUsuario;
					usuario.StrNombres = StrNombres;
					usuario.StrApellidos = (StrApellidos != null) ? StrApellidos : "";
					usuario.StrMail = StrMail;
					usuario.StrTelefono = StrTelefono;
					usuario.StrExtension = StrExtension;
					usuario.StrCelular = StrCelular;
					usuario.StrCargo = StrCargo;
					usuario.IntIdEstado = IntIdEstado;

					bool datos = ctl_usuario.Crear(usuario, null);
				}
				if (Tipo == 2)
				{
					TblUsuarios usuario = new TblUsuarios();
					usuario.StrEmpresa = StrEmpresa;
					usuario.StrUsuario = StrUsuario;
					usuario.StrNombres = StrNombres;
					usuario.StrApellidos = (StrApellidos != null) ? StrApellidos : "";
					usuario.StrMail = StrMail;
					usuario.StrTelefono = StrTelefono;
					usuario.StrExtension = StrExtension;
					usuario.StrCelular = StrCelular;
					usuario.StrCargo = StrCargo;
					usuario.IntIdEstado = IntIdEstado;

					bool datos = ctl_usuario.Actualizar(usuario);
				}

				//if (!datos)
				//{
				//    return NotFound();
				//}

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
