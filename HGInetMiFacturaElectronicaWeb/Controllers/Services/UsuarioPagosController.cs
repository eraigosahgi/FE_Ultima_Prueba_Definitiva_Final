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
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Registros;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class UsuarioPagosController : ApiController
	{


		/// <summary>
		///Autenticación al modulo de pagos
		/// </summary>
		/// <param name="codigo_empresa"></param>
		/// <param name="codigo_usuario"></param>
		/// <param name="clave"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/AutenticarPagos")]
		public IHttpActionResult AutenticarPagos(string IdSeguridadFacturador, string codigo_usuario, string clave)
		{
			try
			{

				if (string.IsNullOrEmpty(IdSeguridadFacturador))
				{
					throw new ApplicationException("No se encontro Serial del Facturador en la petición");
				}

				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();
				List<TblUsuariosPagos> datos = ctl_usuario.ValidarExistencia(Guid.Parse(IdSeguridadFacturador), codigo_usuario, clave);

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
		/// Obtiene todos los usuarios
		/// </summary>
		/// <param name="codigo_usuario"></param>
		/// <param name="codigo_empresa"></param>
		/// <param name="Desde"></param>
		/// <param name="Hasta"></param>
		/// <param name="nombre_usuario"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerMisUsuarios")]
		public IHttpActionResult ObtenerMisUsuarios(string codigo_usuario, string nombre_usuario)
		{
			try
			{

				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				List<ObjUsuario> datos = new List<ObjUsuario>();

				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();
				datos = ctl_usuario.ConsultaUsuarios(codigo_usuario, datosempresa.StrIdentificacion, nombre_usuario);


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


		[HttpGet]
		[Route("api/ObtenerUnUsuario")]
		public IHttpActionResult ObtenerUnUsuario(string id_seguridad_usuario)
		{
			try
			{

				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				ObjUsuario datos = new ObjUsuario();

				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();
				datos = ctl_usuario.ConsultaPorIdSeguridad(datosempresa.StrIdentificacion, Guid.Parse(id_seguridad_usuario));


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


		[HttpGet]
		[Route("api/EnviarNotificacion")]
		public IHttpActionResult EnviarNotificacion(string codigo_usuario)
		{
			try
			{

				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();
				TblUsuariosPagos datos_usuario = ctl_usuario.ObtenerUsuarios(codigo_usuario, datosempresa.StrIdentificacion).FirstOrDefault();


				if (datos_usuario == null && datosempresa == null)
				{
					return NotFound();
				}

				Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
				List<MensajeEnvio> respuesta = clase_email.BienvenidaPagos(datosempresa, datos_usuario);

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
		[Route("api/RestablecerClavePagos")]
		public IHttpActionResult Post([FromUri]string IdSeguridadFacturador, [FromUri]string codigo_usuario)
		{
			try
			{

				if (string.IsNullOrEmpty(IdSeguridadFacturador))
				{
					throw new ApplicationException("Serial invalido o empresa no existe");
				}

				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();

				string datos = ctl_usuario.ValidarExistenciaRestablecer(Guid.Parse(IdSeguridadFacturador), codigo_usuario);

				var resultado = datos.Split('@');

				try
				{
					datos = string.Format("{0}****@{1}", resultado[0].Substring(0, 1), resultado[1].ToString());
				}
				catch (Exception)
				{
				}

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// Valida si el codigo de seguridad existe y si tiene vigencia para restablecer la contraseña
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <param name="clave"></param>
		/// <returns>Resultado de restablecer</returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri]string clave)
		{
			try
			{
				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();

				string empresa = ctl_usuario.RestablecerClave(id_seguridad, clave);

				TblEmpresas datos_empresa = null;
				if (!string.IsNullOrEmpty(empresa))
				{

					Ctl_Empresa ctl_empresa = new Ctl_Empresa();
					datos_empresa = ctl_empresa.Obtener(empresa);
				}

				if (!string.IsNullOrEmpty(datos_empresa.StrIdSeguridad.ToString()))
				{
					return Ok(datos_empresa.StrIdSeguridad.ToString());
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
				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();

				string empresa = ctl_usuario.ValidarIdSeguridad(id_seguridad);

				TblEmpresas datos_empresa = null;
				if (!string.IsNullOrEmpty(empresa))
				{

					Ctl_Empresa ctl_empresa = new Ctl_Empresa();
					datos_empresa = ctl_empresa.Obtener(empresa);
				}

				if (!string.IsNullOrEmpty(datos_empresa.StrIdSeguridad.ToString()))
				{
					return Ok(datos_empresa.StrIdSeguridad.ToString());
				}
				if (!string.IsNullOrEmpty(datos_empresa.StrIdSeguridad.ToString()))
				{
					return Ok(datos_empresa.StrIdSeguridad.ToString());
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
				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();

				TblUsuariosPagos datos = ctl_usuario.ObtenerIdSeguridad(StrIdSeguridad);
				List<TblUsuariosPagos> resultado = new List<TblUsuariosPagos>();

				resultado.Add(datos);

				var retorno = resultado.Select(d => new
				{
					StrEmpresa = d.StrEmpresaFacturador,
					StrUsuario = d.StrUsuario,
					StrClave = d.StrClave,
					StrNombres = d.StrNombres,
					StrApellidos = d.StrApellidos,
					StrTelefono = d.StrTelefono,
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
		[Route("api/GuardarUsuario")]
		public IHttpActionResult GuardarUsuario([FromUri]string StrEmpresaAdquiriente, [FromUri]string StrUsuario, [FromUri] string StrNombres, [FromUri]string StrApellidos, [FromUri]string StrMail, [FromUri]string StrTelefono, [FromUri] short IntIdEstado, [FromUri]int Tipo)//1 Nuevo -- 2 Actualizar
		{
			try
			{
				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();

				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				string StrEmpresa = datosempresa.StrIdentificacion;

				TblUsuariosPagos usuario = new TblUsuariosPagos();
				usuario.StrEmpresaFacturador = StrEmpresa;
				usuario.StrEmpresaAdquiriente = StrEmpresaAdquiriente;
				usuario.StrUsuario = StrUsuario;
				usuario.StrNombres = StrNombres;
				usuario.StrApellidos = (StrApellidos != null) ? StrApellidos : "";
				usuario.StrMail = StrMail;
				usuario.StrTelefono = StrTelefono;
				usuario.IntIdEstado = IntIdEstado;

				if (Tipo == 1)
				{

					bool datos = ctl_usuario.Crear(usuario, null);
				}
				if (Tipo == 2)
				{
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

		[HttpPost]
		[Route("api/RegistroUsuarioPagos")]
		public IHttpActionResult RegistroUsuarioPagos([FromUri]string IdSeguridadFacturador, [FromUri]string StrEmpresaAdquiriente, [FromUri]string StrUsuario, [FromUri] string StrNombres, [FromUri]string StrApellidos, [FromUri]string StrMail, [FromUri]string StrTelefono)//1 Nuevo -- 2 Actualizar
		{
			try
			{

				//Validamos que llegue el guid de seguridad
				if (string.IsNullOrEmpty(IdSeguridadFacturador))
				{
					throw new ApplicationException("No se encontro Serial del Facturador en la petición");
				}


				Ctl_Empresa _controlador_empresa = new Ctl_Empresa();
				TblEmpresas emp = new TblEmpresas();

				emp = _controlador_empresa.Obtener(Guid.Parse(IdSeguridadFacturador), false).FirstOrDefault();

				if (emp == null)
				{
					throw new ApplicationException("Serial invalido o empresa no existe");
				}

				Ctl_Documento _doc = new Ctl_Documento();
				var documento = _doc.ObtenerDocumentodeFacturadorAdquiriente(emp.StrIdentificacion, StrEmpresaAdquiriente);

				if (documento == null)
				{
					throw new ApplicationException("No fue posible realizar el registro ya que usted no posee ningún documento con el Facturador");
				}

				Ctl_UsuarioPagos ctl_usuario = new Ctl_UsuarioPagos();

				TblUsuariosPagos usuario = new TblUsuariosPagos();
				usuario.StrEmpresaFacturador = emp.StrIdentificacion;
				usuario.StrEmpresaAdquiriente = StrEmpresaAdquiriente;
				usuario.StrUsuario = StrUsuario;
				usuario.StrNombres = StrNombres;
				usuario.StrApellidos = (StrApellidos != null) ? StrApellidos : "";
				usuario.StrMail = StrMail;
				usuario.StrTelefono = StrTelefono;
				usuario.IntIdEstado = 2;//Inactivo

				bool datos = ctl_usuario.Crear(usuario, null, true);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
