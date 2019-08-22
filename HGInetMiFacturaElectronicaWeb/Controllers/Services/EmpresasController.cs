using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Enumerables;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Xml;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EmpresasController : ApiController
	{

		/// <summary>
		/// Obtiene la lista de empresas
		/// 
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get()
		{
			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();
			List<TblEmpresas> datos = ctl_empresa.ObtenerTodas();

			if (datos == null)
			{
				return NotFound();
			}

			var retorno = datos.Select(d => new
			{
				Identificacion = d.StrIdentificacion,
				RazonSocial = d.StrRazonSocial,
				Email = d.StrMailAdmin,
				Serial = d.StrSerial,
				Perfil = d.IntAdquiriente && d.IntObligado ? "Facturador y Adquiriente" : d.IntAdquiriente ? "Adquiriente" : d.IntObligado ? "Facturador" : "",
				Habilitacion = d.IntHabilitacion,
				IdSeguridad = d.StrIdSeguridad
			});

			return Ok(retorno);
		}

		/// <summary>
		/// Obtiene la empresa o lista de empresas
		/// Si es Admin en la tabla de empresas, muestra todas las empresas
		/// 
		/// Si es Adquiriente, solo muestra esa empresa
		/// 
		/// Si es Facturador y no administrador, muestra su empresa y las empresas donde fue seleccionado como asociado
		/// 
		/// (El campo integrador no afectaria aqui ya que la empresa seleccionada como asociado no solo son integradores, si no todos los facturadores)
		/// </summary>
		/// <param name="IdentificacionEmpresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerEmpresas")]
		public IHttpActionResult ObtenerEmpresas(string IdentificacionEmpresa)
		{
			Sesion.ValidarSesion();

			Ctl_Empresa CtEmpresa = new Ctl_Empresa();

			TblEmpresas empresa = new TblEmpresas();

			List<TblEmpresas> datos = new List<TblEmpresas>();

			empresa = CtEmpresa.Obtener(IdentificacionEmpresa);


			if (empresa.IntAdministrador)
			{
				datos = CtEmpresa.ObtenerTodas();
			}
			else
			{
				if (empresa.IntIntegrador)
				{
					datos = CtEmpresa.ObtenerAsociadas(IdentificacionEmpresa);
				}
				else
				{
					datos = CtEmpresa.Obtener(empresa.StrIdSeguridad);
				}
			}

			if (datos == null)
			{
				return NotFound();
			}

			var retorno = datos.Select(d => new
			{
				Identificacion = d.StrIdentificacion,
				RazonSocial = d.StrRazonSocial,
				Email = d.StrMailAdmin,
				Serial = d.StrSerial,
				Perfil = d.IntAdquiriente && d.IntObligado ? "Facturador y Adquiriente" : d.IntAdquiriente ? "Adquiriente" : d.IntObligado ? "Facturador" : "",
				Habilitacion = d.IntHabilitacion,
				IdSeguridad = d.StrIdSeguridad,
				Asociado = d.StrEmpresaAsociada,
				EmpresaDescuento = d.StrEmpresaDescuento,
				Estado = d.IntIdEstado,
				Postpago = (d.IntCobroPostPago == 1) ? "SI" : "NO",
				Nusuaurios = d.IntNumUsuarios,
				HorasAcuse = (d.IntAcuseTacito > 0) ? d.IntAcuseTacito.ToString() : "NO",
				NotificacionMail = (d.IntEnvioMailRecepcion) ? "SI" : "NO",
				d.StrMailEnvio,
				d.StrMailPagos,
				d.StrMailRecepcion,
				d.StrMailAcuse
			});

			return Ok(retorno);
		}

		/// <summary>
		/// Obtiene de Facturadores y perfil 99 de Habilitacion 
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(bool Facturador)
		{

			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();

			List<TblEmpresas> datosSesion = new List<TblEmpresas>();

			datosSesion.Add(Sesion.DatosEmpresa);

			TblEmpresas empresa = datosSesion.FirstOrDefault();


			List<TblEmpresas> datos = new List<TblEmpresas>();

			if (empresa.IntAdministrador)
			{
				datos = ctl_empresa.ObtenerFacturadores();
			}
			else
			{
				if (empresa.IntIntegrador)
				{
					datos = ctl_empresa.ObtenerAsociadas(empresa.StrIdentificacion);
				}
				else
				{
					datos = ctl_empresa.Obtener(empresa.StrIdSeguridad);
				}
			}
			if (datos == null)
			{
				return NotFound();
			}

			var retorno = datos.Select(d => new
			{
				Identificacion = d.StrIdentificacion,
				RazonSocial = d.StrRazonSocial,
				Email = d.StrMailAdmin,
				Serial = d.StrSerial,
				datakey = Encriptar.Encriptar_SHA1(string.Format("{0}{1}", d.StrSerial, d.StrIdentificacion)),
				Perfil = d.IntAdquiriente && d.IntObligado ? "Facturador y Adquiriente" : d.IntAdquiriente ? "Adquiriente" : d.IntObligado ? "Facturador" : "",
				Habilitacion = d.IntHabilitacion,
				IdSeguridad = d.StrIdSeguridad,
				Resolucion = d.StrResolucionDian
			});

			return Ok(retorno);
		}




		/// <summary>
		/// Obtiene la lista 
		/// </summary>
		/// /// <param name="IdSeguridad">id de seguridad</param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(System.Guid IdSeguridad)
		{
			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();
			List<TblEmpresas> datos = ctl_empresa.Obtener(IdSeguridad);

			if (datos == null)
			{
				return NotFound();
			}

			var retorno = datos.Select(d => new
			{
				TipoIdentificacion = d.StrTipoIdentificacion,
				Identificacion = d.StrIdentificacion,
				RazonSocial = d.StrRazonSocial,
				Email = d.StrMailAdmin,
				Serial = d.StrSerial,
				Perfil = d.IntAdquiriente,
				Habilitacion = d.IntHabilitacion,
				IdSeguridad = d.StrIdSeguridad,
				Intadquiriente = d.IntAdquiriente,
				intObligado = d.IntObligado,
				IntIdentificacionDv = d.IntIdentificacionDv,
				StrResolucionDian = d.StrResolucionDian,
				StrEmpresaAsociada = ctl_empresa.ObtenerRazonSocial(d.StrEmpresaAsociada),
				StrObservaciones = d.StrObservaciones,
				IntIntegrador = d.IntIntegrador,
				IntNumUsuarios = d.IntNumUsuarios,
				IntAnexo = d.IntManejaAnexos,
				IntEmailRecepcion = d.IntEnvioMailRecepcion,
				IntAcuseTacito = d.IntAcuseTacito,
				StrEmpresaDescuenta = ctl_empresa.ObtenerRazonSocial(d.StrEmpresaDescuento),
				Estado = d.IntIdEstado,
				Postpago = d.IntCobroPostPago,
				d.StrMailEnvio,
				d.StrMailPagos,
				d.StrMailRecepcion,
				d.StrMailAcuse,
				Admin = d.IntAdministrador,
				telefono = d.StrTelefono,
				VersionDIAN = d.IntVersionDian,
				Proc_Email = d.IntMailAdminVerificado,
				Proc_MailEnvio = d.IntMailEnvioVerificado,
				Proc_MailRecepcion = d.IntMailRecepcionVerificado,
				Proc_MailAcuse = d.IntMailAcuseVerificado,
				Proc_MailPagos = d.IntMailPagosVerificado,

				IntCertFirma = d.IntCertFirma,
				IntCertProveedor = d.IntCertProveedor,
				IntCertResponsableHGI = d.IntCertResponsableHGI,
				IntCertNotificar = d.IntCertNotificar,
				StrCertClave = d.StrCertClave,
				DatCertVence = Convert.ToDateTime(d.DatCertVence).ToString(Fecha.formato_fecha_hginet)			

			});

			return Ok(retorno);
		}



		/// <summary>
		/// Obtiene la lista 
		/// </summary>
		/// /// <param name="IdSeguridad">id de seguridad</param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerInfCert")]
		public IHttpActionResult ObtenerInfCert(System.Guid IdSeguridad,string Clave)// "Shh4DshyVN"
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				var datos = ctl_empresa.ObtenerInfCert(IdSeguridad, Clave);
			
				if (datos == null)
				{
					return NotFound();
				}
				//Generamos formato al resultado
				var Resultado = new { Descripcion = datos.Propietario, FechaVencimiento =  Convert.ToDateTime(datos.Fechavenc ).ToString(Fecha.formato_fecha_hginet), Serial = datos.Serial };

				return Ok(Resultado);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Crea o Modifica una empresa una  Empresa
		/// </summary>
		/// <param name="TipoIdentificacion">Tipo de Identificación de la empresa</param>
		/// <param name="Identificacion">Identificación de la empresa</param>
		/// <param name="RazonSocial">Razon Social</param>
		/// <param name="Email">Email</param>
		/// <param name="Intadquiriente">Si es adquiriente</param>
		/// <param name="IntObligado">Si es facturador</param>
		/// <param name="IntHabilitacion">Si esta habilitado</param>
		/// <param name="StrEmpresaAsociada">Si tiene una empresa asociada</param>
		/// <param name="StrObservaciones">Observaciones</param>
		/// <param name="IntIntegrador">Si es integrador</param>
		/// <param name="IntNumUsuarios">Numero de usuarios permitidos</param>
		/// <param name="IntAcuseTacito">Numero de horas para el acuse tacito</param>
		/// <param name="IntAnexo">Si permite anexos</param>
		/// <param name="IntEmailRecepcion"></param>
		/// <param name="StrEmpresaDescuento">Empresa para el descuento de los planes</param>
		/// <param name="tipo">1.- Nuevo -- 2.- Editar</param>
		/// <returns></returns>
		//[HttpPost]
		//public IHttpActionResult Post([FromUri] string TipoIdentificacion, [FromUri]string Identificacion, [FromUri]string RazonSocial, [FromUri]string Email, [FromUri]bool Intadquiriente, [FromUri]bool IntObligado, [FromUri]Byte IntHabilitacion, [FromUri] string StrEmpresaAsociada, [FromUri]string StrObservaciones, [FromUri] bool IntIntegrador, [FromUri] int IntNumUsuarios, [FromUri] short IntAcuseTacito, [FromUri]bool IntAnexo, [FromUri]bool IntEmailRecepcion, [FromUri] string StrEmpresaDescuento, [FromUri]short intestado, [FromUri]short intpostpago, [FromUri]string StrMailEnvio, [FromUri]string StrMailRecepcion, [FromUri]string StrMailAcuse, [FromUri]string StrMailPagos, [FromUri]string telefono, [FromUri]short version, [FromUri] short IntMailAdminVerificado, [FromUri] short IntMailEnvioVerificado, [FromUri] short IntMailRecepcionVerificado, [FromUri] short IntMailAcuseVerificado, [FromUri] short IntMailPagosVerificado, [FromUri]short IntCertFirma, [FromUri]short IntCertProveedor, [FromUri]bool IntCertResponsableHGI, [FromUri] bool IntCertNotificar, [FromUri]string StrCertClave, [FromUri]DateTime DatCertVence, [FromUri]int tipo)//1.- Nuevo -- 2.- Editar
		//{
		//	Sesion.ValidarSesion();


		//	Sesion.ValidarSesion();

		//	Ctl_Empresa ctl_empresa = new Ctl_Empresa();
		//	TblEmpresas Empresa = new TblEmpresas();
		//	try
		//	{

		//		Empresa.StrTipoIdentificacion = TipoIdentificacion;
		//		Empresa.StrIdentificacion = Identificacion;
		//		Empresa.StrRazonSocial = RazonSocial;
		//		Empresa.IntAdquiriente = Intadquiriente;
		//		Empresa.IntHabilitacion = IntHabilitacion;
		//		Empresa.IntObligado = IntObligado;
		//		Empresa.StrEmpresaAsociada = (string.IsNullOrEmpty(StrEmpresaAsociada) ? Identificacion.Trim() : StrEmpresaAsociada.Trim());
		//		Empresa.StrObservaciones = StrObservaciones;
		//		Empresa.IntIntegrador = IntIntegrador;
		//		Empresa.IntNumUsuarios = IntNumUsuarios;
		//		Empresa.IntIdEstado = intestado;
		//		Empresa.IntCobroPostPago = intpostpago;
		//		Empresa.IntManejaAnexos = IntAnexo;
		//		Empresa.IntEnvioMailRecepcion = IntEmailRecepcion;
		//		Empresa.IntVersionDian = version;
		//		Empresa.StrEmpresaDescuento = (string.IsNullOrEmpty(StrEmpresaDescuento) ? Identificacion.Trim() : StrEmpresaDescuento.Trim());

		//		#region Certificado
		//		Empresa.IntCertFirma = IntCertFirma;
		//		Empresa.IntCertProveedor = IntCertProveedor;
		//		Empresa.IntCertResponsableHGI = IntCertResponsableHGI;
		//		Empresa.IntCertNotificar = IntCertNotificar;
		//		Empresa.StrCertClave = StrCertClave;
		//		Empresa.DatCertVence = DatCertVence;
		//		#endregion

		//		Empresa.StrMailAdmin = Email;
		//		Empresa.IntAcuseTacito = IntAcuseTacito;
		//		Empresa.StrMailAcuse = StrMailAcuse;
		//		Empresa.StrMailEnvio = StrMailEnvio;
		//		Empresa.StrMailRecepcion = StrMailRecepcion;
		//		Empresa.StrMailPagos = StrMailPagos;
		//		Empresa.StrTelefono = telefono;

		//		#region Verificación de Email

		//		Empresa.IntMailAdminVerificado = IntMailAdminVerificado;
		//		Empresa.IntMailEnvioVerificado = IntMailEnvioVerificado;
		//		Empresa.IntMailRecepcionVerificado = IntMailRecepcionVerificado;
		//		Empresa.IntMailAcuseVerificado = IntMailAcuseVerificado;
		//		Empresa.IntMailPagosVerificado = IntMailPagosVerificado;

		//		//Admin
		//		if (IntMailAdminVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailAdmin))
		//		{
		//			Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
		//		}

		//		if (IntMailAdminVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailAdmin))
		//		{
		//			Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
		//		}
		//		//Recepción
		//		if (IntMailRecepcionVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailRecepcion))
		//		{
		//			Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
		//		}

		//		if (IntMailRecepcionVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailRecepcion))
		//		{
		//			Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
		//		}
		//		//Envio
		//		if (IntMailEnvioVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailEnvio))
		//		{
		//			Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
		//		}

		//		if (IntMailEnvioVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailEnvio))
		//		{
		//			Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
		//		}
		//		//Acuse
		//		if (IntMailAcuseVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailAcuse))
		//		{
		//			Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
		//		}

		//		if (IntMailAcuseVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailAcuse))
		//		{
		//			Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
		//		}
		//		//Pagos
		//		if (IntMailPagosVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailPagos))
		//		{
		//			Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
		//		}

		//		if (IntMailPagosVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailPagos))
		//		{
		//			Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
		//		}
		//		#endregion


		//		if (tipo == 1)//Nuevo
		//		{

		//			List<TblEmpresas> datosSesion = new List<TblEmpresas>();

		//			datosSesion.Add(Sesion.DatosEmpresa);

		//			TblEmpresas empresaSession = datosSesion.FirstOrDefault();

		//			if (empresaSession.IntAdministrador)
		//			{
		//				var datos = ctl_empresa.Guardar(Empresa);
		//			}
		//			else
		//			{
		//				throw new ApplicationException("No tiene permisos para crear Empresas");
		//			}

		//		}

		//		if (tipo == 2)//Editar
		//		{
		//			var datos = ctl_empresa.Editar(Empresa, Sesion.DatosEmpresa.IntAdministrador);
		//		}

		//		return Ok();
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}





		[HttpPost]
		[Route("api/GuardarEmpresa")]		
		public IHttpActionResult GuardarEmpresa(TblEmpresas ObjEmpresa)
		{
			Sesion.ValidarSesion();						

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();
			TblEmpresas Empresa = new TblEmpresas();
			try
			{

				Empresa.StrTipoIdentificacion = ObjEmpresa.StrTipoIdentificacion;
				Empresa.StrIdentificacion = ObjEmpresa.StrIdentificacion;
				Empresa.StrRazonSocial = ObjEmpresa.StrRazonSocial;
				Empresa.IntAdquiriente = ObjEmpresa.IntAdquiriente;
				Empresa.IntHabilitacion = ObjEmpresa.IntHabilitacion;
				Empresa.IntObligado = ObjEmpresa.IntObligado;
				Empresa.StrEmpresaAsociada = (string.IsNullOrEmpty(ObjEmpresa.StrEmpresaAsociada) ? ObjEmpresa.StrIdentificacion.Trim() : ObjEmpresa.StrEmpresaAsociada.Trim());
				Empresa.StrObservaciones = ObjEmpresa.StrObservaciones;
				Empresa.IntIntegrador = ObjEmpresa.IntIntegrador;
				Empresa.IntNumUsuarios = ObjEmpresa.IntNumUsuarios;
				Empresa.IntIdEstado = ObjEmpresa.IntIdEstado;
				Empresa.IntCobroPostPago = ObjEmpresa.IntCobroPostPago;
				Empresa.IntManejaAnexos = ObjEmpresa.IntManejaAnexos;
				Empresa.IntEnvioMailRecepcion = ObjEmpresa.IntEnvioMailRecepcion;
				Empresa.IntVersionDian = ObjEmpresa.IntVersionDian;
				Empresa.StrEmpresaDescuento = (string.IsNullOrEmpty(ObjEmpresa.StrEmpresaDescuento) ? ObjEmpresa.StrIdentificacion.Trim() : ObjEmpresa.StrEmpresaDescuento.Trim());

				#region Certificado
				Empresa.IntCertFirma = ObjEmpresa.IntCertFirma;
				Empresa.IntCertProveedor = ObjEmpresa.IntCertProveedor;
				Empresa.IntCertResponsableHGI = ObjEmpresa.IntCertResponsableHGI;
				Empresa.IntCertNotificar = ObjEmpresa.IntCertNotificar;
				Empresa.StrCertClave = ObjEmpresa.StrCertClave;
				Empresa.DatCertVence = ObjEmpresa.DatCertVence;
				#endregion

				Empresa.StrMailAdmin = ObjEmpresa.StrMailAdmin;
				Empresa.IntAcuseTacito = ObjEmpresa.IntAcuseTacito;
				Empresa.StrMailAcuse = ObjEmpresa.StrMailAcuse;
				Empresa.StrMailEnvio = ObjEmpresa.StrMailEnvio;
				Empresa.StrMailRecepcion = ObjEmpresa.StrMailRecepcion;
				Empresa.StrMailPagos = ObjEmpresa.StrMailPagos;
				Empresa.StrTelefono = ObjEmpresa.StrTelefono;

				#region Verificación de Email

				Empresa.IntMailAdminVerificado = ObjEmpresa.IntMailAdminVerificado;
				Empresa.IntMailEnvioVerificado = ObjEmpresa.IntMailEnvioVerificado;
				Empresa.IntMailRecepcionVerificado = ObjEmpresa.IntMailRecepcionVerificado;
				Empresa.IntMailAcuseVerificado = ObjEmpresa.IntMailAcuseVerificado;
				Empresa.IntMailPagosVerificado = ObjEmpresa.IntMailPagosVerificado;

				//Admin
				if (ObjEmpresa.IntMailAdminVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailAdmin))
				{
					Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}

				if (ObjEmpresa.IntMailAdminVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailAdmin))
				{
					Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				//Recepción
				if (ObjEmpresa.IntMailRecepcionVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailRecepcion))
				{
					Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}

				if (ObjEmpresa.IntMailRecepcionVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailRecepcion))
				{
					Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				//Envio
				if (ObjEmpresa.IntMailEnvioVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailEnvio))
				{
					Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}

				if (ObjEmpresa.IntMailEnvioVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailEnvio))
				{
					Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				//Acuse
				if (ObjEmpresa.IntMailAcuseVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailAcuse))
				{
					Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}

				if (ObjEmpresa.IntMailAcuseVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailAcuse))
				{
					Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				//Pagos
				if (ObjEmpresa.IntMailPagosVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailPagos))
				{
					Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}

				if (ObjEmpresa.IntMailPagosVerificado > EstadoVerificacionEmail.Registro.GetHashCode() && string.IsNullOrEmpty(Empresa.StrMailPagos))
				{
					Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				#endregion


				if (ObjEmpresa.StrIdSeguridad == Guid.Empty)//Nuevo
				{

					List<TblEmpresas> datosSesion = new List<TblEmpresas>();

					datosSesion.Add(Sesion.DatosEmpresa);

					TblEmpresas empresaSession = datosSesion.FirstOrDefault();

					if (empresaSession.IntAdministrador)
					{
						var datos = ctl_empresa.Guardar(Empresa);
					}
					else
					{
						throw new ApplicationException("No tiene permisos para crear Empresas");
					}
				}
				else
				{
					{
						var datos = ctl_empresa.Editar(Empresa, Sesion.DatosEmpresa.IntAdministrador);
					}
				}
					return Ok();
				}

			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// Crear la activacion para la Empresa ingresando el serial y la resolución
		/// </summary>
		/// <param name="codigo_empresa"></param>
		/// <param name="Codigo_Serial"></param>        
		/// <param name="Codigo_Resolucion"></param>        
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]string Identificacion, [FromUri]string Serial, [FromUri]string Resolucion)
		{
			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();
			TblEmpresas Empresa = new TblEmpresas();
			try
			{
				Empresa.StrIdentificacion = Identificacion;
				Empresa.StrResolucionDian = Resolucion;
				Empresa.StrSerial = Serial;

				var datos = ctl_empresa.Editar(Identificacion, Serial, Resolucion);

				return Ok();
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);

			}
		}

		/// <summary>
		/// Envio de Email con Serial 
		/// </summary>
		/// <param name="codigo_empresa"></param>        
		/// <param name="Email"></param>        
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]string Identificacion, [FromUri]string Mail)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();
				List<MensajeEnvio> Enviarmail = Email.EnviaSerial(Identificacion, Mail);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene la lista de Habilitacion según el ambiente 
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get([FromUri] string tipo)
		{
			try
			{
				Sesion.ValidarSesion();

				List<ClsHabilitacion> lista = new List<ClsHabilitacion>();

				string Tipoambiente = (tipo == "99") ? "99" : "0";

				foreach (var value in Enum.GetValues(typeof(Habilitacion)))
				{
					ClsHabilitacion habi = new ClsHabilitacion();

					FieldInfo fi = value.GetType().GetField(value.ToString());

					DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
					AmbientValueAttribute[] ambiente = (AmbientValueAttribute[])fi.GetCustomAttributes(typeof(AmbientValueAttribute), false);

					if (ambiente[0].Value.ToString() == Tipoambiente)
					{
						habi.ID = (int)value;
						habi.Texto = attributes[0].Description;

						lista.Add(habi);
					}
				}
				return Ok(lista);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public class ClsHabilitacion
		{
			public int ID { get; set; }
			public string Texto { get; set; }

		}


		#region Filtros
		/// <summary>
		/// Obtiene la lista de Habilitacion según el ambiente 
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerAdquirientes")]
		public IHttpActionResult ObtenerAdquirientes(string Facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa controlador = new Ctl_Empresa();
				var lista = controlador.ObtenerAdquirientes(Facturador);


				return Ok(lista);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		#endregion



	}

}
