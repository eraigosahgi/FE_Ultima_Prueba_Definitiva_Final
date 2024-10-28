﻿using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Enumerables;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.HgiNet;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Xml;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_Empresa;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EmpresasController : ApiController
	{
		/// <summary>
		/// Recibe certificado digital para almacenarlo
		/// </summary>
		/// <param name="StrIdSeguridad">Id de seguridad de la empresa</param>
		/// <param name="Clave">Clave del certificado</param>
		/// <returns>Indica si se guardo con exito</returns>
		[HttpPost]
		[Route("api/SubirArchivo")]
		public IHttpActionResult SubirArchivo(System.Guid StrIdSeguridad, string Clave, int Certificadora)
		{
			try
			{
				if (string.IsNullOrEmpty(Clave) || Clave.Equals("null"))
				{
					throw new ApplicationException("Debe ingresar la clave del certificado Digital");
				}

				var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
				CertificadoDigital datos = new CertificadoDigital();
				if (file != null && file.ContentLength > 0)
				{
					Ctl_Empresa Controlador = new Ctl_Empresa();
					datos = Controlador.GuardarCertificadoDigital(file, StrIdSeguridad, Clave, Certificadora);
				}

				var Resultado = new { Descripcion = datos.Propietario, FechaVencimiento = Convert.ToDateTime(datos.Fechavenc).ToString(Fecha.formato_fecha_hginet), Serial = datos.Serial, Emisor = datos.Certificadora };

				return Ok(Resultado);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

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
		public IHttpActionResult ObtenerEmpresas(string IdentificacionEmpresa, int Desde, int Hasta, int Tipo, string Nit, string razon_social)
		{
			Sesion.ValidarSesion();

			Ctl_Empresa CtEmpresa = new Ctl_Empresa();

			TblEmpresas empresa = new TblEmpresas();

			List<ObjEmpresa> datos = new List<ObjEmpresa>();

			empresa = CtEmpresa.Obtener(IdentificacionEmpresa);


			if (empresa.IntAdministrador)
			{
				datos = CtEmpresa.Pag_ObtenerEmpresas(Desde, Hasta, Tipo, Nit, razon_social);
			}
			else
			{
				if (empresa.IntIntegrador)
				{
					datos = CtEmpresa.Pag_ObtenerAsociadas(IdentificacionEmpresa, Desde, Hasta, Nit, razon_social);
				}
			}

			if (datos == null)
			{
				return NotFound();
			}

			return Ok(datos);
		}

		[HttpGet]
		[Route("api/ObtenerEmpresasCertificados")]
		public IHttpActionResult ObtenerEmpresasCertificados(string IdentificacionEmpresa, DateTime Desde, DateTime Hasta, string responsable = "*", string proveedor = "*")
		{
			Sesion.ValidarSesion();

			Ctl_Empresa CtEmpresa = new Ctl_Empresa();
			List<TblEmpresas> empresas = new List<TblEmpresas>();

			empresas = CtEmpresa.ObtenerEmpresasCertificados(IdentificacionEmpresa, Desde, Hasta, responsable, proveedor);

			var retorno = empresas.Select(d => new
			{
				Identificacion = d.StrIdentificacion,
				RazonSocial = d.StrRazonSocial,
				Email = d.StrMailAdmin,
				d.DatCertVence,
				d.IntCertProveedor,
				d.IntCertFirma,
				d.IntCertResponsableHGI
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
			bool ConsultaAdmin = false;
			if (empresa.IntAdministrador)
			{
				datos = ctl_empresa.ObtenerFacturadores();
				ConsultaAdmin = true;
			}
			else
			{
				ConsultaAdmin = false;
				if (empresa.IntIntegrador)
				{
					datos = ctl_empresa.ObtenerAsociadas(empresa.StrIdentificacion);

				}
				else
				{
					datos = ctl_empresa.Obtener(empresa.StrIdSeguridad, false);
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
				Resolucion = d.StrResolucionDian,
				ConsultaAdmin = ConsultaAdmin,
				d.IntPdfCampoDianPosX,
				d.IntPdfCampoDianPosY
			});

			return Ok(retorno);
		}



		/// <summary>
		/// Obtiene de Facturadores y perfil 99 de Habilitacion 
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerMisFacturadores")]
		public IHttpActionResult ObtenerMisFacturadores()
		{

			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();

			List<TblEmpresas> datosSesion = new List<TblEmpresas>();

			datosSesion.Add(Sesion.DatosEmpresa);

			TblEmpresas empresa = datosSesion.FirstOrDefault();



			var datos = ctl_empresa.ObtenerMisFacturadores(empresa.StrIdentificacion);

			if (datos == null)
			{
				return NotFound();
			}


			return Ok(datos);

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
				TipoIdentificacion = (d.StrTipoIdentificacion == "0") ? "31" : d.StrTipoIdentificacion,
				Identificacion = d.StrIdentificacion,
				RazonSocial = d.StrRazonSocial,
				Email = d.StrMailAdmin,
				Serial = d.StrSerial,
				Perfil = d.IntAdquiriente,
				Habilitacion = d.IntHabilitacion,
				Habilitacion_NominaE = d.IntHabilitacionNomina,
				IntEnvioNominaMail = d.IntEnvioNominaMail,
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
				d.IntManejaPagoE,
				d.IntPagoEParcial,
				IntEmailRecepcion = d.IntEnvioMailRecepcion,
				Radian = d.IntRadian,
				SMS = d.IntEnvioSms,
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
				d.IntTipoPlan,
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
				DatCertVence = Convert.ToDateTime(d.DatCertVence).ToString(Fecha.formato_fecha_hginet),
				SerialCloudServices = d.StrSerialCloudServices,
				Debug = d.IntDebug,
				InterOp = d.IntInteroperabilidad,
				d.IntPagosPermiteConsTodos,
				d.ComercioConfigId,
				d.ComercioConfigDescrip,

				d.StrIdentificacionRep,
				d.StrTipoIdentificacionRep,
				d.StrNombresRep,
				d.StrApellidosRep,
				d.StrCargo

			});

			return Ok(retorno);
		}

		/// <summary>
		/// Obtiene Empresa Operador(Factoring - Sistema de Negociacion)
		/// </summary>
		/// <param name="identificacion">Identficacion del Operador(Endosatario)</param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerOperador")]
		public IHttpActionResult ObtenerOperador(string identificacion)
		{
			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();
			TblEmpresas datos = ctl_empresa.Obtener(identificacion);

			if (datos == null)
			{
				return NotFound();
			}

			var retorno = new
			{
				Idseguridad = datos.StrIdSeguridad,
				RazonSocial = datos.StrRazonSocial,
				TipoIdentificacion = datos.StrTipoIdentificacion,
				Identificacion = datos.StrIdentificacion,
				Email = datos.StrMailAdmin,
				TipoOperador = datos.IntTipoOperador
			};

			return Ok(retorno);
		}

		/// <summary>
		/// Crea o edita Empresa Operador(Factoring - Sistema de Negociacion)
		/// </summary>
		/// <param name="identificacion">Identficacion del Operador(Endosatario)</param>
		/// <returns></returns>
		[HttpPost]
		[Route("api/CrearEditarOperador")]
		public IHttpActionResult CrearEditarOperador(Guid? Idseguridad, string RazonSocial, int TipoIdentificacion, string Identificacion, string Email, int TipoOperador)
		{
			Sesion.ValidarSesion();

			Ctl_Empresa ctl_empresa = new Ctl_Empresa();
			TblEmpresas datos = null;

			///Proceso si va a editar o crear y hacer el llenado de la informacion necesaria para la creacion como empresa.
			if (Idseguridad == null)
			{
				Tercero empresa_operador = new Tercero();
				empresa_operador.Identificacion = Identificacion;
				empresa_operador.IdentificacionDv = FuncionesIdentificacion.Dv(Identificacion); ;
				empresa_operador.TipoIdentificacion = TipoIdentificacion;
				empresa_operador.RazonSocial = RazonSocial;
				empresa_operador.NombreComercial = RazonSocial;
				empresa_operador.Email = Email;

				TblEmpresas empresa = ctl_empresa.Crear(empresa_operador, false);

				if (empresa.IntTipoOperador != TipoOperador)
				{
					empresa.IntTipoOperador = TipoOperador;
					empresa = ctl_empresa.Actualizar(empresa);
				}

				datos = empresa;

			}
			else
			{
				datos = ctl_empresa.Obtener(Idseguridad.Value).FirstOrDefault();
				datos.IntTipoOperador = TipoOperador;
				ctl_empresa.Actualizar(datos);
			}


			if (datos == null)
			{
				return NotFound();
			}

			var retorno = new
			{
				Idseguridad = datos.StrIdSeguridad,
				RazonSocial = datos.StrRazonSocial,
				TipoIdentificacion = datos.StrTipoIdentificacion,
				Identificacion = datos.StrIdentificacion,
				Email = datos.StrMailAdmin,
				TipoOperador = datos.IntTipoOperador
			};

			return Ok(retorno);
		}


		/// <summary>
		/// Obtiene la lista 
		/// </summary>
		/// /// <param name="IdSeguridad">id de seguridad</param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerInfCert")]
		public IHttpActionResult ObtenerInfCert(System.Guid IdSeguridad, string Clave, int Certificadora)// "Shh4DshyVN"
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				var datos = ctl_empresa.ObtenerInfCert(IdSeguridad, Clave, Certificadora);

				if (datos == null)
				{
					return NotFound();
				}
				//Generamos formato al resultado
				var Resultado = new { Descripcion = datos.Propietario, FechaVencimiento = Convert.ToDateTime(datos.Fechavenc).ToString(Fecha.formato_fecha_hora), Serial = datos.Serial, Emisor = datos.Certificadora };

				return Ok(Resultado);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Crea o actualiza un empresa
		/// </summary>
		/// <param name="ObjEmpresa">Tabla empresas</param>
		/// <returns>Ok() O error indicando el Error</returns>
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
				Empresa.IntHabilitacionNomina = ObjEmpresa.IntHabilitacionNomina;
				Empresa.IntObligado = ObjEmpresa.IntObligado;
				Empresa.StrEmpresaAsociada = (string.IsNullOrEmpty(ObjEmpresa.StrEmpresaAsociada) ? ObjEmpresa.StrIdentificacion.Trim() : ObjEmpresa.StrEmpresaAsociada.Trim());
				Empresa.StrObservaciones = ObjEmpresa.StrObservaciones;
				Empresa.StrSerial = ObjEmpresa.StrSerial;
				Empresa.IntIntegrador = ObjEmpresa.IntIntegrador;
				Empresa.IntNumUsuarios = ObjEmpresa.IntNumUsuarios;
				Empresa.IntIdEstado = ObjEmpresa.IntIdEstado;
				Empresa.IntCobroPostPago = ObjEmpresa.IntCobroPostPago;
				Empresa.IntManejaAnexos = ObjEmpresa.IntManejaAnexos;
				Empresa.IntManejaPagoE = ObjEmpresa.IntManejaPagoE;
				Empresa.IntPagoEParcial = ObjEmpresa.IntPagoEParcial;
				Empresa.IntPagosPermiteConsTodos = ObjEmpresa.IntPagosPermiteConsTodos;
				Empresa.IntEnvioMailRecepcion = ObjEmpresa.IntEnvioMailRecepcion;
				Empresa.IntEnvioNominaMail = ObjEmpresa.IntEnvioNominaMail;
				Empresa.IntVersionDian = ObjEmpresa.IntVersionDian;
				Empresa.StrEmpresaDescuento = (string.IsNullOrEmpty(ObjEmpresa.StrEmpresaDescuento) ? ObjEmpresa.StrIdentificacion.Trim() : ObjEmpresa.StrEmpresaDescuento.Trim());
				Empresa.StrSerialCloudServices = ObjEmpresa.StrSerialCloudServices;
				Empresa.IntDebug = ObjEmpresa.IntDebug;
				Empresa.IntInteroperabilidad = ObjEmpresa.IntInteroperabilidad;
				Empresa.IntRadian = ObjEmpresa.IntRadian;
				Empresa.IntEnvioSms = ObjEmpresa.IntEnvioSms;
				Empresa.IntTipoPlan = ObjEmpresa.IntTipoPlan;
				Empresa.IntCompraPlan = ObjEmpresa.IntTipoPlan == 0 ? true : false;

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

				//Lista de correos 
				List<ObjVerificacionEmail> ListaEmailRegistro = new List<ObjVerificacionEmail>();

				#region Correo administrativo
				//Validamos si esta en registro o verificado y el email esta diferente de string.IsNullOrEmpty entonces lo coloco en estado de verificación
				if (ObjEmpresa.IntMailAdminVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailAdmin))
				{
					//Si esta en esta de registro, entonces guardo el email en la lista de email de registro para enviar los emails de verificación
					if (ObjEmpresa.IntMailAdminVerificado == EstadoVerificacionEmail.Registro.GetHashCode())
					{
						ListaEmailRegistro.Add(new ObjVerificacionEmail { email = Empresa.StrMailAdmin });
					}
					//Aqui asigno el estado de verificación
					Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}

				//Si el email esta en blanco, entonces dejamos el estatus de verificado en registro
				if (string.IsNullOrEmpty(Empresa.StrMailAdmin))
				{
					Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				#endregion

				#region Recepción
				//Recepción
				if (ObjEmpresa.IntMailRecepcionVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailRecepcion))
				{
					if (ObjEmpresa.IntMailRecepcionVerificado == EstadoVerificacionEmail.Registro.GetHashCode())
					{
						ListaEmailRegistro.Add(new ObjVerificacionEmail { email = Empresa.StrMailRecepcion });
					}
					Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}
				//Si el email esta en blanco, entonces dejamos el estatus de verificado en registro
				if (string.IsNullOrEmpty(Empresa.StrMailRecepcion))
				{
					Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				#endregion

				#region Correo de Envio
				//Envio
				if (ObjEmpresa.IntMailEnvioVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailEnvio))
				{
					//Si esta en esta de registro, entonces guardo el email en la lista de email de registro para enviar los emails de verificación
					if (ObjEmpresa.IntMailEnvioVerificado == EstadoVerificacionEmail.Registro.GetHashCode())
					{
						ListaEmailRegistro.Add(new ObjVerificacionEmail { email = Empresa.StrMailEnvio });
					}
					//Aqui asigno el estado de verificación
					Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}
				//Si el email esta en blanco, entonces dejamos el estatus de verificado en registro
				if (string.IsNullOrEmpty(Empresa.StrMailEnvio))
				{
					Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				#endregion

				#region Correo de Acuse
				//Acuse
				if (ObjEmpresa.IntMailAcuseVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailAcuse))
				{
					//Si esta en esta de registro, entonces guardo el email en la lista de email de registro para enviar los emails de verificación
					if (ObjEmpresa.IntMailAcuseVerificado == EstadoVerificacionEmail.Registro.GetHashCode())
					{
						ListaEmailRegistro.Add(new ObjVerificacionEmail { email = Empresa.StrMailAcuse });
					}
					//Aqui asigno el estado de verificación
					Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}
				//Si el email esta en blanco, entonces dejamos el estatus de verificado en registro
				if (string.IsNullOrEmpty(Empresa.StrMailAcuse))
				{
					Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				#endregion

				#region Correo de pagos
				//Pagos
				if (ObjEmpresa.IntMailPagosVerificado <= EstadoVerificacionEmail.Verificacion.GetHashCode() && !string.IsNullOrEmpty(Empresa.StrMailPagos))
				{
					//Si esta en esta de registro, entonces guardo el email en la lista de email de registro para enviar los emails de verificación
					if (ObjEmpresa.IntMailPagosVerificado == EstadoVerificacionEmail.Registro.GetHashCode())
					{
						ListaEmailRegistro.Add(new ObjVerificacionEmail { email = Empresa.StrMailPagos });
					}
					//Aqui asigno el estado de verificación
					Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Verificacion.GetHashCode();
				}
				//Si el email esta en blanco, entonces dejamos el estatus de verificado en registro
				if (string.IsNullOrEmpty(Empresa.StrMailPagos))
				{
					Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Registro.GetHashCode();
				}
				#endregion

				#endregion

				Empresa.StrIdentificacionRep = ObjEmpresa.StrIdentificacionRep;
				Empresa.StrTipoIdentificacionRep = ObjEmpresa.StrTipoIdentificacionRep;
				Empresa.StrNombresRep = ObjEmpresa.StrNombresRep;
				Empresa.StrApellidosRep = ObjEmpresa.StrApellidosRep;
				Empresa.StrCargo = ObjEmpresa.StrCargo;

				if (ObjEmpresa.StrIdSeguridad == Guid.Empty)//Nuevo
				{

					List<TblEmpresas> datosSesion = new List<TblEmpresas>();

					datosSesion.Add(Sesion.DatosEmpresa);

					TblEmpresas empresaSession = datosSesion.FirstOrDefault();

					if (empresaSession.IntAdministrador)
					{
						var datos = ctl_empresa.Guardar(Empresa, ListaEmailRegistro);

						//try
						//{
						//	if (ObjEmpresa.IntIntegrador == true)
						//	{
						//		Ctl_Integradores ctl_int = new Ctl_Integradores();
						//		TblIntegradores integrador = ctl_int.Obtener(ObjEmpresa.StrTipoIdentificacion);
						//		if (integrador == null)
						//		{
						//			integrador = new TblIntegradores();
						//			integrador.StrIdentificacion = ObjEmpresa.StrTipoIdentificacion;
						//			integrador.StrRazonSocial = ObjEmpresa.StrRazonSocial;
						//			integrador.TblEmpresas = datos;
						//			ctl_int.Crear(integrador);
						//		}
						//	}
						//}
						//catch (Exception)
						//{

						//}
					}
					else
					{
						throw new ApplicationException("No tiene permisos para crear Empresas");
					}
				}
				else
				{
					{
						///Se agrega el objeto session del usuario para almacenar en la auditoria
						TblUsuarios usuario_sesion = null;
						try
						{
							usuario_sesion = Sesion.DatosUsuario;
						}
						catch (Exception)
						{ }
						var datos = ctl_empresa.Editar(Empresa, Sesion.DatosEmpresa.IntAdministrador, ListaEmailRegistro, usuario_sesion);

						//try
						//{
						//	if (ObjEmpresa.IntIntegrador == true)
						//	{
						//		Ctl_Integradores ctl_int = new Ctl_Integradores();
						//		TblIntegradores integrador = ctl_int.Obtener(ObjEmpresa.StrTipoIdentificacion);
						//		if (integrador == null)
						//		{
						//			integrador = new TblIntegradores();
						//			integrador.StrIdentificacion = ObjEmpresa.StrTipoIdentificacion;
						//			integrador.StrRazonSocial = ObjEmpresa.StrRazonSocial;
						//			integrador.TblEmpresas = datos;
						//			ctl_int.Crear(integrador);
						//		}
						//	}
						//}
						//catch (Exception)
						//{

						//}
					}
				}

				//try
				//{
				//	if (ObjEmpresa.IntIntegrador == true)
				//	{
				//		Ctl_Integradores ctl_int = new Ctl_Integradores();
				//		TblIntegradores integrador = ctl_int.Obtener(ObjEmpresa.StrTipoIdentificacion);
				//		if (integrador == null)
				//		{
				//			integrador = new TblIntegradores();
				//			integrador.StrIdentificacion = ObjEmpresa.StrTipoIdentificacion;
				//			integrador.StrRazonSocial = ObjEmpresa.StrRazonSocial;
				//			//integrador.TblEmpresas = Empresa;
				//			ctl_int.Crear(integrador);
				//		}
				//	}
				//}
				//catch (Exception)
				//{

				//}

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
		/// Obtiene los adquirientes del facturador
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

		/// <summary>
		/// Obtiene los empleados del emisor
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerEmpleados")]
		public IHttpActionResult ObtenerEmpleados()
		{
			try
			{
				Sesion.ValidarSesion();

				string codigo_emisor = Sesion.DatosEmpresa.StrIdentificacion;

				Ctl_Empresa controlador = new Ctl_Empresa();
				var lista = controlador.ObtenerEmpleados(codigo_emisor);


				return Ok(lista);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("api/ObtenerAdquirientesPagos")]
		public IHttpActionResult ObtenerAdquirientesPagos()
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				Ctl_Empresa controlador = new Ctl_Empresa();
				var lista = controlador.ObtenerAdquirientes(datosempresa.StrIdentificacion);

				return Ok(lista);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene Todos los adquirientes de la tabla tblempresas
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerTodosAdquirientes")]
		public IHttpActionResult ObtenerTodosAdquirientes()
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa controlador = new Ctl_Empresa();
				var lista = controlador.ObtenerTodosAdquirientes();


				return Ok(lista);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		#endregion



		[HttpGet]
		[Route("Api/EmpresaEditarConfigPago")]
		public IHttpActionResult EditarConfigPago(string Stridseguridad, bool Permitepagosparciales, string IdComercio, string DescripcionComercio)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa Controlador = new Ctl_Empresa();

				if (!string.IsNullOrEmpty(DescripcionComercio) || !string.IsNullOrEmpty(IdComercio))
				{
					if (string.IsNullOrEmpty(DescripcionComercio))
					{
						throw new ApplicationException("Debe indicar una descripción para la configuración de comercio");
					}
				}
				var Datos = Controlador.EditarConfigPago(Guid.Parse(Stridseguridad), Permitepagosparciales, IdComercio, DescripcionComercio);

				//Resultado
				var resultado = new
				{
					Datos.ComercioConfigDescrip,
					Datos.ComercioConfigId,
					Datos.IntPagoEParcial
				};

				return Ok(resultado);

			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/EmpresaEditarCamposDian")]
		public IHttpActionResult EditarCamposDian(string StrIdseguridad, decimal posicion_x, decimal posicion_y)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa Controlador = new Ctl_Empresa();
				var Datos = Controlador.EditarCamposDian(Guid.Parse(StrIdseguridad), posicion_x, posicion_y);

				//Resultado
				var resultado = new
				{
					Datos.IntPdfCampoDian,
					Datos.IntPdfCampoDianPosX,
					Datos.IntPdfCampoDianPosY
				};

				return Ok(resultado);

			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/ObtenerSerialCloud")]
		public IHttpActionResult ObtenerSerialCloud(string codigo_facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Empresa Controlador = new Ctl_Empresa();

				var Datos = Controlador.ObtenerSerialCloud(codigo_facturador);

				return Ok(Datos);

			}
			catch (Exception e)
			{
				//Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				return InternalServerError(new ApplicationException("No se encontraron datos, por favor valide que tenga configuración asignada en CloudServices"));
			}


		}


	}

}
