using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.PagosElectronicos;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea;
using LibreriaGlobalHGInet.Peticiones;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Xml;
using static HGInetMiFacturaElectonicaController.PagosElectronicos.Ctl_PagosElectronicos;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{

	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class DocumentosController : ApiController
	{
		/// <summary>
		/// Obtiene los documentos por adquiriente
		/// </summary>
		/// <param name="codigo_adquiente"></param>
		/// <param name="numero_documento"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerDocumentosAdquirientes")]
		public IHttpActionResult ObtenerDocumentosAdquirientes(string codigo_facturador, string codigo_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();


				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.ObtenerPorFechasAdquiriente(codigo_facturador, codigo_adquiente, numero_documento, estado_recibo, fecha_inicio.Date, fecha_fin.Date, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					d.IdFacturador,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,// DescripcionEstadoFactura(d.EstadoFactura),
					EstadoAcuse = d.EstadoAcuse,// DescripcionEstadoAcuse(d.EstadoAcuse),
					d.MotivoRechazo,
					StrAdquirienteMvoRechazo = d.StrAdquirienteMvoRechazo,
					d.Facturador,
					d.NumeroDocumento,
					d.DatFechaIngreso,
					EstadoCategoria = d.EstadoCategoria,//DescripcionCategoriaFactura(d.EstadoCategoria),
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.tipodoc)),
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,// DescripcionEstadoEmail(Convert.ToInt16(d.EstadoEnvioMail)),
					MensajeEnvio = d.MensajeEnvio,// DescripcionMensajeEmail(Convert.ToInt16(d.MensajeEnvio)),
					d.EnvioMail,
					poseeIdComercio = d.poseeIdComercio,
					FacturaCancelada = d.FacturaCancelada,
					PagosParciales = (d.poseeIdComercio == 1) ? Resolucion.ManejaPagosParciales(d.NumResolucion, d.IdFacturador) : 0, // d.PagosParciales,
																																	  //Telefono = d.TblEmpresasFacturador.StrTelefono,
					d.poseeIdComercioPSE,
					d.poseeIdComercioTC,
					Saldo = (d.poseeIdComercio == 1) ? Pago.ConsultaSaldoDocumentoPM(d.StrIdSeguridad, d.IntValorPagar) : 0
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		#region HgiPay
		/// <summary>
		/// Obtiene los documentos por adquiriente
		/// </summary>
		/// <param name="codigo_adquiente"></param>
		/// <param name="numero_documento"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/HGIpayConsultaDocumentos")]
		public IHttpActionResult HGIpayConsultaDocumentos(string IdSeguridad, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesionPagos();


				if (string.IsNullOrEmpty(IdSeguridad))
					throw new ApplicationException("Error de identificación del Facturador. no se encontro Serial");

				string codigo_adquiente = "";

				codigo_adquiente = Sesion.DatosUsuarioPagos.StrEmpresaAdquiriente;

				if (string.IsNullOrEmpty(codigo_adquiente))
					throw new ApplicationException("No se encontro información de los datos del Adquiriente");


				Ctl_Empresa _controlador_empresa = new Ctl_Empresa();
				TblEmpresas empresa = new TblEmpresas();
				empresa = _controlador_empresa.Obtener(Guid.Parse(IdSeguridad), false).FirstOrDefault();

				if (empresa == null)
					throw new ApplicationException("No se encontro información de los datos del Facturador");


				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.HGIpayObtenerPorFechasAdquiriente(codigo_adquiente, empresa.StrIdentificacion, numero_documento, estado_recibo, fecha_inicio.Date, fecha_fin.Date, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();


				var retorno = datos.Select(d => new
				{
					d.IdFacturador,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,
					EstadoAcuse = d.EstadoAcuse,
					d.MotivoRechazo,
					StrAdquirienteMvoRechazo = d.StrAdquirienteMvoRechazo,
					d.Facturador,
					d.NumeroDocumento,
					d.DatFechaIngreso,
					EstadoCategoria = d.EstadoCategoria,
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.tipodoc)),
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,
					MensajeEnvio = d.MensajeEnvio,
					d.EnvioMail,
					poseeIdComercio = d.poseeIdComercio,
					FacturaCancelada = d.FacturaCancelada,
					PagosParciales = d.PagosParciales,
					d.poseeIdComercioPSE,
					d.poseeIdComercioTC,
					d.IdComercio,
					d.DescripComercio,
					Saldo = Pago.ConsultaSaldoDocumentoPM(d.StrIdSeguridad, d.IntValorPagar)
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}





		#endregion
		/// <summary>
		/// Obtiene los documentos por obligado
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.ObtenerPorFechasObligado(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Resolucion, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				List<string> doc_consulta_evento = datos.Where(x => x.tipodoc == 1 && ((x.IntAdquirienteRecibo >= 0 && x.IntAdquirienteRecibo < 3) || (x.IntAdquirienteRecibo == 4)) && x.FormaPago == 2 && x.EstadoCategoria == 300).Select(x => x.StrIdSeguridad.ToString()).ToList();

				if (doc_consulta_evento != null)
				{
					string docs_consulta = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(doc_consulta_evento, ",");
					var Tarea1 = ctl_documento.SondaConsultareventos(false, docs_consulta);
				}

				var retorno = datos.Select(d => new
				{
					d.IdFacturador,
					d.Facturador,
					d.NumeroDocumento,
					d.DatFechaIngreso,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,// DescripcionEstadoFactura(d.EstadoFactura),
					EstadoCategoria = d.EstadoCategoria,//DescripcionCategoriaFactura(d.EstadoCategoria),
					EstadoAcuse = DescripcionEstadoAcuse(d.EstadoAcuse),
					d.MotivoRechazo,
					AdquirienteMvoRechazo = d.StrAdquirienteMvoRechazo,
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.tipodoc)),
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,// DescripcionEstadoEmail(Convert.ToInt16(d.EstadoEnvioMail)),
					MensajeEnvio = d.MensajeEnvio,// DescripcionMensajeEmail(Convert.ToInt16(d.MensajeEnvio)),
					d.EnvioMail,
					d.Radian,
					TituloValor = (d.IntAdquirienteRecibo > 5) ? "Titulo Valor" : (d.IntAdquirienteRecibo == 5) || (d.IntAdquirienteRecibo == 3) ? "Aceptado" : "Documento Electrónico"
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// Obtiene los documentos por obligado
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ObtenerDocumentosSoporte")]
		public IHttpActionResult ObtenerDocumentosSoporte(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;


				codigo_facturador = Sesion.DatosUsuario.StrEmpresa;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.ObtenerDocumentosSoporte(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Resolucion, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				//List<string> doc_consulta_evento = datos.Where(x => x.tipodoc == 1 && ((x.IntAdquirienteRecibo >= 0 && x.IntAdquirienteRecibo < 3) || (x.IntAdquirienteRecibo == 4)) && x.FormaPago == 2 && x.EstadoCategoria == 300).Select(x => x.StrIdSeguridad.ToString()).ToList();

				//if (doc_consulta_evento != null)
				//{
				//	string docs_consulta = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(doc_consulta_evento, ",");
				//	var Tarea1 = ctl_documento.SondaConsultareventos(false, docs_consulta);
				//}

				var retorno = datos.Select(d => new
				{
					d.IdFacturador,
					d.Facturador,
					d.NumeroDocumento,
					d.DatFechaIngreso,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,
					EstadoCategoria = d.EstadoCategoria,
					EstadoAcuse = DescripcionEstadoAcuse(d.EstadoAcuse),
					d.MotivoRechazo,
					AdquirienteMvoRechazo = d.StrAdquirienteMvoRechazo,
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = (d.tipodoc == 1) ? "Documento de adquisiciones" : "Nota de ajuste",
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,
					MensajeEnvio = d.MensajeEnvio,
					d.EnvioMail,
					d.Radian,
					TituloValor = (d.IntAdquirienteRecibo > 5) ? "Titulo Valor" : (d.IntAdquirienteRecibo == 5) || (d.IntAdquirienteRecibo == 3) ? "Aceptado" : "Documento Electrónico"
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// Obtiene los documentos por obligado
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>		
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		/// 
		[HttpGet]
		[Route("api/ObtenerDocumentosRadian")]
		public IHttpActionResult ObtenerDocumentosRadian(string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				string codigo_facturador = Sesion.DatosUsuario.StrEmpresa;

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.ObtenerDocumentosRadian(codigo_facturador, numero_documento, codigo_adquiriente, fecha_inicio, fecha_fin, Resolucion, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				List<string> doc_consulta_evento = datos.Where(x => x.tipodoc == 1 && ((x.IntAdquirienteRecibo >= 0 && x.IntAdquirienteRecibo < 3) || (x.IntAdquirienteRecibo == 4)) && x.FormaPago == 2 && x.EstadoCategoria == 300).Select(x => x.StrIdSeguridad.ToString()).ToList();

				if (doc_consulta_evento != null)
				{
					string docs_consulta = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(doc_consulta_evento, ",");
					var Tarea1 = ctl_documento.SondaConsultareventos(false, docs_consulta);
				}

				var retorno = datos.Select(d => new
				{
					d.IdFacturador,
					d.Facturador,
					d.NumeroDocumento,
					d.DatFechaIngreso,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,// DescripcionEstadoFactura(d.EstadoFactura),
					EstadoCategoria = d.EstadoCategoria,//DescripcionCategoriaFactura(d.EstadoCategoria),
					EstadoAcuse = DescripcionEstadoAcuse(d.EstadoAcuse),
					d.MotivoRechazo,
					AdquirienteMvoRechazo = d.StrAdquirienteMvoRechazo,
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.tipodoc)),
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,// DescripcionEstadoEmail(Convert.ToInt16(d.EstadoEnvioMail)),
					MensajeEnvio = d.MensajeEnvio,// DescripcionMensajeEmail(Convert.ToInt16(d.MensajeEnvio)),
					d.EnvioMail,
					d.Radian,
					TituloValor = (d.IntAdquirienteRecibo > 5) ? "Titulo Valor" : (d.IntAdquirienteRecibo == 5) || (d.IntAdquirienteRecibo == 3) ? "Aceptado" : "Documento Electrónico"
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los documentos por obligado
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/DocumentosEmisor")]
		public IHttpActionResult DocumentosEmisor(string codigo_emisor, string numero_documento, string codigo_empleado, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_documento, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				codigo_emisor = Sesion.DatosEmpresa.StrIdentificacion;

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.ObtenerPorFechasEmisor(codigo_emisor, numero_documento, codigo_empleado, estado_dian, estado_recibo, fecha_inicio, fecha_fin, tipo_documento, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					d.IdFacturador,
					d.Facturador,
					d.NumeroDocumento,
					d.DatFechaIngreso,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,// DescripcionEstadoFactura(d.EstadoFactura),
					EstadoCategoria = d.EstadoCategoria,//DescripcionCategoriaFactura(d.EstadoCategoria),
					EstadoAcuse = d.EstadoAcuse,// DescripcionEstadoAcuse(d.EstadoAcuse),
					d.MotivoRechazo,
					AdquirienteMvoRechazo = d.StrAdquirienteMvoRechazo,
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.tipodoc)),
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,// DescripcionEstadoEmail(Convert.ToInt16(d.EstadoEnvioMail)),
					MensajeEnvio = d.MensajeEnvio,// DescripcionMensajeEmail(Convert.ToInt16(d.MensajeEnvio)),
					d.EnvioMail
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		/// <summary>
		/// Retorna los documentos del plan solicitado
		/// </summary>
		/// <param name="IdPlan"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/DocumentosPlanes")]
		public IHttpActionResult DocumentosPlanes(Guid IdPlan)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerPorPlan(IdPlan);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					d.DatFechaIngreso,
					Cod_Facturador = d.StrEmpresaFacturador,
					NombreFacturador = d.TblEmpresasFacturador.StrRazonSocial
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el documento por ID de seguridad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(System.Guid id_seguridad)
		{
			try
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerPorIdSeguridad(id_seguridad);

				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();
				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
					IdAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					Cufe = d.StrCufe,
					IdSeguridad = d.StrIdSeguridad,
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					FechaRespuesta = d.DatAdquirienteFechaRecibo,
					Xml = d.StrUrlArchivoUbl,
					d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					RespuestaVisible = (d.IntAdquirienteRecibo == CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == CodigoResponseV2.Expresa.GetHashCode() || d.IntAdquirienteRecibo == CodigoResponseV2.Inscripcion.GetHashCode()) ? true : false,
					CamposVisibles = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() && d.IntAdquirienteRecibo != (short)CodigoResponseV2.AprobadoTacito.GetHashCode()) ? true : false,
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					//poseeIdComercio = (d.TblEmpresasResoluciones.IntComercioId == null) ? false : (d.TblEmpresasResoluciones.IntComercioId > 0) ? true : false,
					Estatus = Pago.VerificarSaldo(id_seguridad),
					XmlAcuse = d.StrUrlAcuseUbl,
					EstadoCat = d.IdCategoriaEstado,
					EstadoFactura = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
					pago = d.TblPagosDetalles.Select(p => new
					{
						p.TblPagosElectronicos.StrIdRegistro,
						p.TblPagosElectronicos.IntEstadoPago
					}
					//Se coloca este codigo adicional, para validar si el documento tiene un pago pendiente, y si es asi, este debe retornar un segundo objeto
					//con el codigo unico de pago en la plataforma de FE, para poder hacer la consulta desde acuse y validar en la plataforma intermedia el estado del documento
					).Where(x => x.IntEstadoPago.Equals(EstadoPago.Pendiente.GetHashCode()) || x.IntEstadoPago.Equals(EstadoPago.Pendiente2.GetHashCode()))
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el documento por ID de seguridad en la vista de acuse de recibo
		/// </summary>
		/// <param name="id_seguridad">Id de seguridad del documento</param>
		/// <param name="usuario">Usuario activo en la session</param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ConsultarAcuse")]
		public IHttpActionResult ConsultarAcuse(System.Guid id_seguridad, string usuario)
		{
			try
			{

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerPorIdSeguridad(id_seguridad, false);

				//Actualiza el estado del Acuse como leido
				ctl_documento = new Ctl_Documento();
				TblDocumentos documento = new TblDocumentos();
				documento = datos.FirstOrDefault();

				bool actualizar_doc = false;
				bool cliente_hgi = (documento.TblEmpresasAdquiriente.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode() && documento.TblEmpresasAdquiriente.IntObligado == true && documento.TblEmpresasAdquiriente.IntAdquiriente == true && documento.TblEmpresasAdquiriente.IntIdEstado == EstadoEmpresa.ACTIVA.GetHashCode()) ? true : false;

				Ctl_EventosRadian ctl_evento = new Ctl_EventosRadian();

				if (((documento.IntAdquirienteRecibo < (short)CodigoResponseV2.Recibido.GetHashCode()) || (documento.IntAdquirienteRecibo >= (short)CodigoResponseV2.Aceptado.GetHashCode())))
				{

					if (documento.IntEstadoEnvio != (short)EstadoEnvio.Leido.GetHashCode())
					{
						documento.IntEstadoEnvio = (short)EstadoEnvio.Leido.GetHashCode();
						//documento.IntAdquirienteRecibo = (short)AdquirienteRecibo.Pendiente.GetHashCode();
						documento.DatFechaActualizaEstado = Fecha.GetFecha();
						actualizar_doc = true;
					}
				}

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				if (cliente_hgi == true && documento.IntAdquirienteRecibo < (short)CodigoResponseV2.Recibido.GetHashCode())
				{
					//Ctl_EventosRadian evento = new Ctl_EventosRadian();
					Task envio_acuse = ctl_evento.ProcesoCrearAcuseRecibo(string.Empty, documento.StrIdSeguridad);
					//documento.IntAdquirienteRecibo = (short)CodigoResponseV2.Recibido.GetHashCode();
					//documento.DatAdquirienteFechaRecibo = Fecha.GetFecha();
					//actualizar_doc = true;

				}

				TblEventosRadian evento = ctl_evento.Obtener(documento.StrIdSeguridad).OrderByDescending(x => x.IntEstadoEvento).FirstOrDefault();

				if (evento != null && documento.IntAdquirienteRecibo != evento.IntEstadoEvento)
				{
					documento.IntAdquirienteRecibo = evento.IntEstadoEvento;
					documento.DatAdquirienteFechaRecibo = evento.DatFechaEvento;
					actualizar_doc = true;
				}

				if (actualizar_doc == true)
					ctl_documento.Actualizar(documento);


				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();
				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					IdAdquiriente = d.StrEmpresaAdquiriente,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					Cufe = d.StrCufe,
					IdSeguridad = d.StrIdSeguridad,
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					FechaRespuesta = d.DatAdquirienteFechaRecibo,
					Xml = d.StrUrlArchivoUbl,
					d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					RespuestaVisible = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode()) ? false : true,
					CamposVisibles = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode()) ? ((cliente_hgi == false) ? false : true) : false,
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					Estatus = Pago.VerificarSaldo(id_seguridad),
					XmlAcuse = d.StrUrlAcuseUbl,
					EstadoCat = d.IdCategoriaEstado,
					EstadoFactura = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
					d.IntAdquirienteRecibo,
					obligado = d.StrEmpresaFacturador,
					prefijo = d.StrPrefijo,
					documento = d.IntNumero,
					FechaDocumento = d.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet),
					ValorDoc = d.IntVlrTotal,
					poseeIdComercio = (d.TblEmpresasFacturador.IntManejaPagoE) ? true : false,
					PermiteParciales = (d.TblEmpresasFacturador.IntPagoEParcial) ? true : false,
					pago = d.TblPagosDetalles.Select(p => new
					{
						//p.TblPagosElectronicos.StrIdRegistro,
						//p.TblPagosElectronicos.IntEstadoPago
						StrIdRegistro = Pago.ObtenerPorRegistroPrincipal(p.StrIdPagoPrincipal),
						//p.TblPagosElectronicos.IntEstadoPago
					}
					//Se coloca este codigo adicional, para validar si el documento tiene un pago pendiente, y si es asi, este debe retornar un segundo objeto
					//con el codigo unico de pago en la plataforma de FE, para poder hacer la consulta desde acuse y validar en la plataforma intermedia el estado del documento
					)//.Where(x => x.IntEstadoPago.Equals(EstadoPago.Pendiente.GetHashCode()) || x.IntEstadoPago.Equals(EstadoPago.Pendiente2.GetHashCode()))
				});

				//Si el documento ya tiene acuse de recibo, no guarda en auditoria que el documento esta visto
				if (((documento.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode()) || (documento.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode())))
				{

					try
					{
						Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();
						int estado_doc = Ctl_Documento.ObtenerCategoria(documento.IntIdEstado);
						clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, documento.StrEmpresaFacturador, ProcesoEstado.AcuseVisto, TipoRegistro.Actualizacion, Procedencia.Usuario, (!string.IsNullOrEmpty(usuario) ? usuario : string.Empty), Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.AcuseVisto.GetHashCode())), string.Empty, documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
					}
					catch (Exception)
					{ }
				}


				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		public IHttpActionResult Get(System.Guid id_seguridad, string email, string Usuario)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerPorIdSeguridad(id_seguridad);

				if (datos == null)
				{
					return NotFound();
				}

				if (datos.FirstOrDefault().IdCategoriaEstado < 200)
				{
					return NotFound();
				}

				Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
				List<MensajeEnvio> respuesta = null;

				respuesta = clase_email.NotificacionDocumento(datos.FirstOrDefault(), "", email, "", Procedencia.Usuario, (!string.IsNullOrEmpty(Usuario)) ? Usuario : "", ProcesoEstado.EnvioEmailAcuse, "", true);

				if (datos.FirstOrDefault().IntEstadoEnvio == (short)EstadoEnvio.NoEntregado.GetHashCode() || datos.FirstOrDefault().IntEstadoEnvio == (short)EstadoEnvio.Desconocido.GetHashCode())
				{
					ctl_documento = new Ctl_Documento();
					datos.FirstOrDefault().IntEstadoEnvio = (short)EstadoEnvio.Enviado.GetHashCode();
					ctl_documento.Actualizar(datos.FirstOrDefault());
				}
				return Ok(respuesta);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los datos de los acuse de recibo
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="numero_documento"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		public IHttpActionResult Get(string codigo_facturador, string codigo_adquiriente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerAcuseRecibo(codigo_facturador, numero_documento, codigo_adquiriente, null, estado_recibo, fecha_inicio, fecha_fin, "*", tipo_fecha).Where(x => x.IntAdquirienteRecibo != 0).ToList();

				var retorno = datos.Select(d => new
				{
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					RazonSocial = d.TblEmpresasAdquiriente.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
					FechaRespuesta = d.DatAdquirienteFechaRecibo,
					Estado = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrIdSeguridad,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMailAdmin,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					zip = d.StrUrlAnexo,
					RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					XmlAcuse = d.StrUrlAcuseUbl,
					d.IntAdquirienteRecibo,
					EstadoCat = d.IdCategoriaEstado
				});

				return Ok(retorno);
			}
			catch (Exception)
			{

				throw;
			}
		}


		#region Proceso para acuse de documentos (Tacito)
		/// <summary>
		/// Obtiene los datos de los acuse vista Tacito
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="numero_documento"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("api/ConsultaAcuseTacito")]
		public IHttpActionResult ConsultaAcuseTacito(string codigo_facturador, string codigo_adquiriente, string numero_documento)
		{
			try
			{
				Sesion.ValidarSesion();
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				//PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<Guid> guids = ctl_documento.ObtenerAcuseTacito(codigo_facturador, numero_documento, codigo_adquiriente);

				List<TblDocumentos> datos = new List<TblDocumentos>();

				foreach (var item in guids)
				{
					try
					{
						datos.AddRange(ctl_documento.ObtenerPorIdSeguridad(item));

					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						throw;
					}

				}


				var retorno = datos.Select(d => new
				{
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					RazonSocial = d.TblEmpresasAdquiriente.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
					DocSinPrefijo = d.IntNumero,
					Fecha = d.DatFechaDocumento,
					FechaIngreso = d.DatFechaIngreso,
					Estado = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					IdentificacionFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					NombreFacturador = d.TblEmpresasFacturador.StrRazonSocial,
					d.StrIdSeguridad,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMailAdmin,
					dias = Math.Truncate(DateTime.Now.Subtract(d.DatFechaIngreso).TotalHours),
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					zip = d.StrUrlAnexo,
					RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					XmlAcuse = d.StrUrlAcuseUbl
				});



				return Ok(retorno);
			}
			catch (Exception)
			{

				throw;
			}
		}




		/// <summary>
		/// Generar Acuse Tacito a Documentos
		/// </summary>
		/// <param name="objeto"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("api/GenerarAcuseTacito")]
		public IHttpActionResult GenerarAcuseTacito(Object objeto)
		{
			try
			{
				Ctl_Documento clase_doc = new Ctl_Documento();

				var jobjeto = (dynamic)objeto;

				string ListaDoc = jobjeto.Documentos;

				List<DocumentosJSON> ListadeDocumentos = new JavaScriptSerializer().Deserialize<List<DocumentosJSON>>(ListaDoc);

				List<System.Guid> List_id_seguridad = new List<Guid>();

				foreach (var item in ListadeDocumentos)
				{
					List_id_seguridad.Add(item.Documentos);
				}

				foreach (var item in List_id_seguridad)
				{
					try
					{
						Ctl_Documento documento = new Ctl_Documento();
						string respuesta_error_dian = string.Empty;
						documento.ActualizarRespuestaAcuse(item, (short)AdquirienteRecibo.AprobadoTacito.GetHashCode(), Enumeracion.GetDescription(AdquirienteRecibo.AprobadoTacito),ref respuesta_error_dian);

					}
					catch (Exception ex)
					{
						RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
					}
				}

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}






		#endregion

		/// <summary>
		/// Actualiza la respuesta de acuse del documento.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="estado"></param>
		/// <param name="motivo_rechazo"></param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri]short estado, [FromUri]string motivo_rechazo, [FromUri]string usuario)
		{
			try
			{

				try
				{

					if (string.IsNullOrEmpty(usuario))
					{
						usuario = Sesion.DatosUsuario.StrUsuario;
					}
				}
				catch (Exception)
				{
				}



				Ctl_Documento ctl_documento = new Ctl_Documento();

				List<TblDocumentos> datos = new List<TblDocumentos>();

				string respuesta_error_dian = string.Empty;

				TblDocumentos doc = ctl_documento.ActualizarRespuestaAcuse(id_seguridad, estado, motivo_rechazo, ref respuesta_error_dian, (!string.IsNullOrEmpty(usuario)) ? usuario : "");

				if (doc == null)
				{
					return NotFound();
				}

				if (string.IsNullOrEmpty(respuesta_error_dian))
				{
					datos.Add(doc);

					var retorno = datos.Select(d => new
					{
						NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
						IdAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
						NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
						Cufe = d.StrCufe,
						IdSeguridad = d.StrIdSeguridad,
						EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
						MotivoRechazo = d.StrAdquirienteMvoRechazo,
						Xml = d.StrUrlArchivoUbl,
						Pdf = d.StrUrlArchivoPdf,
						//RespuestaVisible = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode()) ? false : true,
						//CamposVisibles = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode()) ? ((cliente_hgi == false) ? false : true) : false,
						RespuestaVisible = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode()) ? true : false,
						CamposVisibles = (d.IntAdquirienteRecibo < (short)CodigoResponseV2.Rechazado.GetHashCode() || d.IntAdquirienteRecibo == (short)CodigoResponseV2.Aceptado.GetHashCode()) ? true : false
					});
					return Ok(retorno);
				}
				else
				{
					throw new ArgumentException(respuesta_error_dian);
				}
				

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Retorna la descripción del estado de la factura.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionEstadoFactura(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		/// <summary>
		/// Retorna la descripción del estado de la factura.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionCategoriaFactura(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CategoriaEstado>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		/// <summary>
		/// Retorna la descripción del estado del acuse.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionEstadoAcuse(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		/// <summary>
		/// Retorna la descripción del estado del envio del correo.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionEstadoEmail(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.EstadoEnvio>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		/// <summary>
		/// Retorna la descripción del estado del envio del correo.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionMensajeEmail(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		#region Procesar Documentos

		/// <summary>
		/// Recibe lista de Documentos 
		/// </summary>
		/// <param name="objeto">Lista de documentos a procesar</param>
		/// <param name="Consultar_Documento">Indica si debe consultar el documentos en la dian, antes de enviarlo</param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post(Object objeto)
		{
			try
			{
				Ctl_Documento clase_doc = new Ctl_Documento();

				var jobjeto = (dynamic)objeto;

				string ListaDoc = jobjeto.Documentos;
				bool Consultar_Documento = Convert.ToBoolean(jobjeto.Consultar_Documento);

				List<DocumentosJSON> ListadeDocumentos = new JavaScriptSerializer().Deserialize<List<DocumentosJSON>>(ListaDoc);

				List<TblDocumentos> ListaDocumentos = new List<TblDocumentos>();

				List<System.Guid> List_id_seguridad = new List<Guid>();

				foreach (var item in ListadeDocumentos)
				{
					List_id_seguridad.Add(item.Documentos);
				}

				Ctl_Documento documento = new Ctl_Documento();

				var lista = documento.ProcesarDocumentos(List_id_seguridad);

				List<DocumentoRespuesta> datos = Ctl_Documentos.Procesar(lista, Consultar_Documento);

				var retorno = datos.Select(d => new
				{
					Aceptacion = d.Aceptacion,
					CodigoRegistro = d.CodigoRegistro,
					Cufe = d.Cufe,
					DescripcionProceso = (d.Error != null) ? d.Error.Mensaje : d.DescripcionProceso,
					DocumentoTipo = d.DocumentoTipo,
					Documento = string.Format("{0}{1}", (d.Prefijo != null) ? d.Prefijo : string.Empty, d.Documento),
					EstadoDianCodigoRespuesta = (d.EstadoDian != null) ? d.EstadoDian.CodigoRespuesta : "",
					EstadoDianDescripcion = (d.EstadoDian != null) ? d.EstadoDian.Descripcion : "",
					EstadoDianEstadoDocumento = (d.EstadoDian != null) ? d.EstadoDian.EstadoDocumento : 0,
					EstadoDianUrlXmlRespuesta = (d.EstadoDian != null) ? d.EstadoDian.UrlXmlRespuesta : "",
					FechaRecepcion = d.FechaRecepcion,
					FechaUltimoProceso = d.FechaUltimoProceso,
					IdDocumento = d.IdDocumento,
					Identificacion = d.Identificacion,
					IdProceso = d.IdProceso,
					MotivoRechazo = d.MotivoRechazo,
					NumeroResolucion = d.NumeroResolucion,
					Prefijo = d.Prefijo,
					ProcesoFinalizado = d.ProcesoFinalizado,
					CodigoError = (d.Error != null) ? d.Error.Mensaje : "",
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.DocumentoTipo))
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		/// <summary>
		/// Obtiene los documentos para ser procesados
		/// </summary>
		/// <param name="codigo_adquiente"></param>
		/// <param name="numero_documento"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/DocumentosAProcesar")]
		//public IHttpActionResult Get(System.Guid? IdSeguridad, string estado_recibo)
		public IHttpActionResult Get()
		{
			try
			{

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerDocumentosaProcesar();

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo != null) ? (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "" : "", d.IntNumero),
					IdSeguridad = d.StrIdSeguridad,
					d.DatFechaIngreso,
					EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = (d.StrAdquirienteMvoRechazo != null) ? d.StrAdquirienteMvoRechazo : "",
					IdentificacionFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					Facturador = d.TblEmpresasFacturador.StrIdentificacion + " -- " + d.TblEmpresasFacturador.StrRazonSocial,
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					zip = d.StrUrlAnexo,
					RutaServDian = (d.IntIdEstado < 7) ? "" : (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					XmlAcuse = d.StrUrlAcuseUbl,
					Estado = d.IdCategoriaEstado
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}





		public class DocumentosJSON
		{

			public System.Guid Documentos { get; set; }

		}
		public class DocumentosTacito
		{
			public long Documentos { get; set; }

		}
		#endregion

		#region Reeviar Acuse
		/// <summary>
		/// Reevia respuesta de acuse del documento.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="mail"></param>        
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri] string mail, [FromUri]string Usuario)
		{
			try
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();
				bool email = ctl_documento.ReenviarRespuestaAcuse(id_seguridad, mail, Usuario);

				if (email)
				{
					return Ok();
				}

				return NotFound();

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion

		#region Consulta de Documentos por Cliente (Soporte)
		/// <summary>
		/// Obtiene un documento por empresa y los primeros 8 digitos del idseguridad o por la empresa-- Resolución y Numero del documento
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="IdSeguridad"></param>
		/// <param name="numero_resolucion"></param>        
		/// <param name="numero_documento"></param>
		/// <returns></returns>
		public IHttpActionResult Get(string codigo_facturador, int? numero_documento, string IdSeguridad = "*", string numero_resolucion = "*")
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				Ctl_Documento ctl_documento = new Ctl_Documento();

				if (string.IsNullOrEmpty(IdSeguridad))
				{
					IdSeguridad = "*";
				}

				// comparadores de string con Equals...
				if (codigo_facturador.Equals(""))
					throw new ApplicationException("Debe Indicar el Facturador");

				if (IdSeguridad.Equals("*") && (numero_resolucion.Equals("*") || numero_documento == 0 || numero_documento == null))
					throw new ApplicationException("No se han especificado los criterios de búsqueda");


				//  !string.IsNullOrEmpty(IdSeguridad)

				if ((!IdSeguridad.Equals("*") && !string.IsNullOrEmpty(IdSeguridad)) && (!numero_resolucion.Equals("*") || numero_documento != null))
					throw new ApplicationException("No se han especificado los criterios de búsqueda");

				if (!IdSeguridad.Equals("*") && !string.IsNullOrEmpty(IdSeguridad))
					if ((IdSeguridad.Length != 8))
						throw new ApplicationException("El codigo de plataforma debe ser de 8 digitos");

				var datos = ctl_documento.ObtenerDocumentoCliente(codigo_facturador, numero_documento, (!string.IsNullOrEmpty(IdSeguridad)) ? IdSeguridad : "*", (numero_resolucion != null) ? numero_resolucion : "*");

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode()) ? -d.IntVlrTotal : d.IntVlrTotal,
					IntSubTotal = (d.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode()) ? -d.IntValorSubtotal : d.IntValorSubtotal,
					IntNeto = (d.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode()) ? -d.IntValorNeto : d.IntValorNeto,
					EstadoFactura = string.Format("{0} - {1}", DescripcionEstadoFactura(d.IntIdEstado), DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado)),
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMailAdmin,
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					zip = d.StrUrlAnexo,
					RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					XmlAcuse = d.StrUrlAcuseUbl,
					permiteenvio = ((Int16)d.IdCategoriaEstado == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.StrEmpresaFacturador,
					Estado = d.IdCategoriaEstado,
					EstadoEnvioMail = DescripcionEstadoEmail((Int16)d.IntEstadoEnvio),
					MensajeEnvio = DescripcionMensajeEmail((Int16)d.IntMensajeEnvio),
					EnvioMail = d.IntEstadoEnvio,
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion

		#region Docuemntos Admin
		/// <summary>
		/// Obtiene los documentos para el administrador (Solo Gerencia)
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		//[HttpGet]
		//public IHttpActionResult Get(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int TipoDocumento, int tipo_fecha, int Desde, int Hasta)
		//{
		//	try
		//	{
		//		Sesion.ValidarSesion();

		//		PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

		//		Ctl_Documento ctl_documento = new Ctl_Documento();
		//		List<TblDocumentos> datos = ctl_documento.ObtenerAdmin(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, TipoDocumento, tipo_fecha, Desde, Hasta);

		//		if (datos == null)
		//		{
		//			return NotFound();
		//		}

		//		var retorno = datos.Select(d => new
		//		{
		//			IdFacturador = d.TblEmpresasFacturador.StrIdentificacion,
		//			Facturador = d.TblEmpresasFacturador.StrRazonSocial,
		//			NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
		//			d.DatFechaIngreso,
		//			d.DatFechaDocumento,
		//			d.DatFechaVencDocumento,
		//			IntVlrTotal = (d.IntDocTipo == 3) ? -d.IntVlrTotal : d.IntVlrTotal,
		//			IntSubTotal = (d.IntDocTipo == 3) ? -d.IntValorSubtotal : d.IntValorSubtotal,
		//			IntNeto = (d.IntDocTipo == 3) ? -d.IntValorNeto : d.IntValorNeto,
		//			EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
		//			EstadoCategoria = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
		//			EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
		//			MotivoRechazo = d.StrAdquirienteMvoRechazo,
		//			d.StrAdquirienteMvoRechazo,
		//			IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
		//			NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
		//			MailAdquiriente = d.TblEmpresasAdquiriente.StrMailAdmin,
		//			Xml = d.StrUrlArchivoUbl,
		//			Pdf = d.StrUrlArchivoPdf,
		//			d.StrIdSeguridad,
		//			RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
		//			tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
		//			zip = d.StrUrlAnexo,
		//			RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
		//			XmlAcuse = d.StrUrlAcuseUbl,
		//			permiteenvio = ((Int16)d.IdCategoriaEstado == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
		//			d.IntAdquirienteRecibo,
		//			Estado = d.IdCategoriaEstado,
		//			EstadoEnvioMail = DescripcionEstadoEmail((Int16)d.IntEstadoEnvio),
		//			MensajeEnvio = DescripcionMensajeEmail((Int16)d.IntMensajeEnvio),
		//			EnvioMail = d.IntEstadoEnvio
		//		});

		//		return Ok(retorno);
		//	}
		//	catch (Exception excepcion)
		//	{

		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}

		//}

		/// <summary>
		/// Obtiene los documentos para el administrador (Solo Gerencia)
		/// </summary>
		/// <param name="codigo_facturador"></param>
		/// <param name="numero_documento"></param>
		/// <param name="codigo_adquiriente"></param>
		/// <param name="estado_dian"></param>
		/// <param name="estado_recibo"></param>
		/// <param name="fecha_inicio"></param>
		/// <param name="fecha_fin"></param>
		/// <returns></returns>
		[HttpGet]
		//[Route("api/ObtenerDocumentosAdmin")]
		public IHttpActionResult Get(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int TipoDocumento, int tipo_fecha, int Desde, int Hasta)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<ObjDocumentos> datos = ctl_documento.ObtenerAdministrador(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, TipoDocumento, tipo_fecha, Desde, Hasta);

				if (datos == null)
				{
					return NotFound();
				}

				var resultado = datos.Select(d => new
				{
					d.IdFacturador,
					d.Facturador,
					//NumeroDocumento = string.Format("{0}{1}", (d.Prefijo == null) ? "" : (!d.Prefijo.Equals("0")) ? d.Prefijo : "", d.NumeroDocumento),
					d.NumeroDocumento,
					d.DatFechaIngreso,
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					d.IntVlrTotal,
					d.IntSubTotal,
					d.IntNeto,
					EstadoFactura = d.EstadoFactura,// DescripcionEstadoFactura(d.EstadoFactura),
					EstadoCategoria = d.EstadoCategoria,//DescripcionCategoriaFactura(d.EstadoCategoria),
					EstadoAcuse = d.EstadoAcuse,// DescripcionEstadoAcuse(d.EstadoAcuse),
					d.MotivoRechazo,
					d.StrAdquirienteMvoRechazo,
					d.IdentificacionAdquiriente,
					d.NombreAdquiriente,
					d.MailAdquiriente,
					d.Xml,
					d.Pdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = (d.tipoOperacion != 3) ? Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.tipodoc)) : (d.tipodoc == 1) ? "Documento de adquisiciones" : "Nota de ajuste adquisiciones",
					d.zip,
					RutaServDian = (d.RutaServDian != null) ? d.RutaServDian.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
					d.XmlAcuse,
					permiteenvio = (d.EstadoCategoria == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
					d.IntAdquirienteRecibo,
					d.Estado,
					EstadoEnvioMail = d.EstadoEnvioMail,// DescripcionEstadoEmail(Convert.ToInt16(d.EstadoEnvioMail)),
					MensajeEnvio = d.MensajeEnvio,// DescripcionMensajeEmail(Convert.ToInt16(d.MensajeEnvio)),
					d.EnvioMail
				});

				return Ok(resultado);
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		#endregion

		/// <summary>
		/// Retorna el correo del adquiriente que se encuentra en el ubl
		/// </summary>
		/// <param name="IdSeguridad">Guid de seguridad del documento</param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/ConsultarEmailUbl")]
		public HttpResponseMessage ConsultarEmailUbl(Guid IdSeguridad)
		{
			try
			{
				string correo = string.Empty;
				Ctl_Documento Controlador = new Ctl_Documento();
				TblDocumentos datos = Controlador.ObtenerPorIdSeguridad(IdSeguridad).FirstOrDefault();

				if (datos.IntDocTipo < TipoDocumento.AcuseRecibo.GetHashCode())
				{
					var objeto = (dynamic)null;
					objeto = Ctl_Documento.ConvertirServicio(datos, true);

					if (datos.IntDocTipo == TipoDocumento.Factura.GetHashCode())
					{
						correo = objeto.DatosFactura.DatosAdquiriente.Email;
					}

					if (datos.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
					{
						correo = objeto.DatosNotaCredito.DatosAdquiriente.Email;
					}

					if (datos.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
					{
						correo = objeto.DatosNotaDebito.DatosAdquiriente.Email;
					}
				}
				else
				{
					correo = datos.TblEmpresasFacturador.StrMailAdmin;
				}





				return Request.CreateResponse(HttpStatusCode.OK, correo);

			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
				//throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}


		#region Zonas de pago
		/// <summary>
		/// Obtiene link para el pago de factura
		/// </summary>
		/// <param name="strIdSeguridad">Id de seguridad del documento</param>
		/// <param name="tipo_pago">Tipo de pago, por defecto esta en 0</param>
		/// <param name="registrar_pago">Indica si debe registrar el pago en base de datos</param>
		/// <param name="valor_pago">Valor del pago</param>
		/// <param name="usuario">Usuario que guarda el pago</param>
		/// <param name="Metodo">Metodo de Pago, 0 = No Definida, 29 = PSE, 31 = Tarjeta de Crédito</param>
		/// <returns>Ruta de Pago</returns>
		public IHttpActionResult Get(System.Guid strIdSeguridad, int tipo_pago = 0, bool registrar_pago = true, double valor_pago = 0, string usuario = "", int IntPagoFormaPago = 0)
		{
			Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

			string monto = valor_pago.ToString().Replace(",", ".");

			string lista_documentos = string.Concat("[{Documento:", "'", strIdSeguridad, "',Valor:", monto, "}]");

			//var datos = Pago.ReportePagoElectronicoPI(strIdSeguridad, tipo_pago, registrar_pago, valor_pago, usuario, IntPagoFormaPago);
			var datos = Pago.ReportePagoElectronicoPIMultiple(lista_documentos, valor_pago);
			return Ok(datos);

		}


		[HttpGet]
		[Route("Api/PagoMultiple")]
		public IHttpActionResult PagoMultiple(string lista_documentos, double valor_pago = 0)
		{
			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				var datos = Pago.ReportePagoElectronicoPIMultiple(lista_documentos, valor_pago);
				return Ok(datos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
		public class PagosMultiples
		{
			public Guid Documento { get; set; }
			public decimal Valor { get; set; }
		}

		/// <summary>
		/// Obtiene la lista de pagos de un documento en especifico
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>

		[HttpGet]
		[Route("Api/ConsultarPagos")]
		public IHttpActionResult ConsultarPagos(System.Guid StrIdSeguridadDoc)
		{

			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				var datos = Pago.Obtener(StrIdSeguridadDoc);


				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					//Encabezado del pago
					RazonSocialFacturador = d.TblEmpresasFacturador.StrRazonSocial,
					NitFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					Telefono = d.TblEmpresasFacturador.StrTelefono,
					Mail = d.TblEmpresasFacturador.StrMailAdmin,
					DocTipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					IntNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					FechaDocumento = d.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet),
					Monto = d.IntValorPagar,//d.IntVlrTotal,
											//Validamos si la resolucion tiene configuracion para entonces validar si maneja pagos parciales, si no maneja pagos parciales, entonces se busca la configuración de la empresa
					PagosParciales = (string.IsNullOrEmpty(d.TblEmpresasResoluciones.ComercioConfigId.ToString())) ? d.TblEmpresasFacturador.IntManejaPagoE : (d.TblEmpresasResoluciones.PermiteParciales == 1) ? true : false,
					//Detalle del pago
					Pagos = d.TblPagosDetalles.Select(p => new
					{
						Monto = p.IntValorPago,
						Franquicia = (!string.IsNullOrEmpty(p.TblPagosElectronicos.StrCodigoFranquicia)) ? p.TblPagosElectronicos.StrCodigoFranquicia : "",
						FechaRegistro = p.TblPagosElectronicos.DatFechaRegistro,
						FechaVerificacion = p.TblPagosElectronicos.DatFechaVerificacion,
						StrIdSeguridadPago = p.TblPagosElectronicos.StrIdSeguridadPago,
						IdRegistro = p.TblPagosElectronicos.StrIdRegistro,
						Estado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(p.TblPagosElectronicos.IntEstadoPago)),
					}).OrderByDescending(x => x.FechaRegistro)

				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}



		/// <summary>
		/// Obtiene el saldo de un documento en especifico
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>

		[HttpGet]
		[Route("Api/ConsultaSaldoDocumento")]
		public IHttpActionResult ConsultaSaldoDocumento(System.Guid StrIdSeguridadDoc)
		{

			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				var datos = Pago.ConsultaSaldoDocumento(StrIdSeguridadDoc);


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

		/// <summary>
		/// Obtiene la lista de pagos realizadas a un facturador
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>

		[HttpGet]
		[Route("Api/ObtenerPagosAdministracion")]
		public IHttpActionResult ObtenerPagosAdministracion(string codigo_facturador, string numero_documento, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string resolucion, int tipo_fecha)
		{

			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				if (Sesion.DatosEmpresa.IntAdministrador)
				{
					var datos = Pago.ObtenerPagos(codigo_facturador, numero_documento, "*", fecha_inicio, fecha_fin, estado_recibo, resolucion, tipo_fecha);


					if (datos == null)
					{
						return NotFound();
					}

					var resultado = ConvertirPagos(datos);
					//var retorno = datos.Select(d => new
					//{
					//	//NumeroDocumento = string.Format("{0}{1}", (!d.TblDocumentos.StrPrefijo.Equals("0")) ? d.TblDocumentos.StrPrefijo : "", d.TblDocumentos.IntNumero),
					//	StrEmpresaFacturador = d.StrEmpresaFacturador,
					//	NombreFacturador = d.TblEmpresas.StrRazonSocial,
					//	DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
					//	DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
					//	PagoFactura = d.IntValorPago,
					//	EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
					//	CodEstado = d.IntEstadoPago,
					//	idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
					//	StrIdRegistro = d.StrIdRegistro,
					//	StrIdSeguridadDoc = d.StrIdRegistro2,
					//	Ciclo = d.IntClicloTransaccion,
					//	Ticket = d.StrTicketID,
					//	Cus = (d.IntFormaPago == 31) ? d.StrCampo1 : d.StrTransaccionCUS,//Si es tarjeta de credito, entonces el valor del campo Strcampo1, corresponde al cogido de aprobacion
					//	Franquicia = (string.IsNullOrEmpty(d.StrCodigoFranquicia)) ? "" : d.StrCodigoFranquicia.ToUpper(),
					//	Pagos = d.TblPagosDetalles.Select(p => new
					//	{
					//		Prefijo = p.TblDocumentos.StrPrefijo,
					//		Documento = p.TblDocumentos.IntNumero,
					//		Monto = p.IntValorPago,
					//	})


					//});

					return Ok(resultado);
				}
				else
				{
					throw new ApplicationException("No tiene permisos para ejecutar esta opción");
				}
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}

		[HttpGet]
		[Route("Api/ObtenerPagosFacturador")]
		public IHttpActionResult ObtenerPagosFacturador(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string resolucion, int tipo_fecha, int tipo_consulta = 1)
		{

			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();


				try
				{
					codigo_facturador = Sesion.DatosUsuario.StrEmpresa;
				}
				catch (Exception)
				{
				}

				try
				{

					if (tipo_consulta == 1)
					{
						var datos = Pago.ObtenerPagos(codigo_facturador, numero_documento, codigo_adquiriente, fecha_inicio, fecha_fin, estado_recibo, resolucion, tipo_fecha);
						var resultado = ConvertirPagos(datos);
						return Ok(resultado);
					}
					else
					{
						var datos_detalle = Pago.ObtenerPagosDetalle(codigo_facturador, numero_documento, codigo_adquiriente, fecha_inicio, fecha_fin, estado_recibo, resolucion, tipo_fecha);
						var resultado = ConvertirDetalles(datos_detalle);
						return Ok(resultado);
					}

					//if (datos == null)
					//{
					//	return NotFound();
					//}

					//var retorno = datos.Select(d => new
					//{
					//	//NumeroDocumento = string.Format("{0}{1}", (!d.TblDocumentos.StrPrefijo.Equals("0")) ? d.TblDocumentos.StrPrefijo : "", d.TblDocumentos.IntNumero),
					//	StrEmpresaFacturador = d.StrEmpresaFacturador,
					//	NombreFacturador = d.TblEmpresas.StrRazonSocial,
					//	StrEmpresaAdquiriente = d.StrEmpresaAdquiriente,
					//	NombreAdquiriente = d.TblEmpresas1.StrRazonSocial,
					//	DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
					//	DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
					//	PagoFactura = d.IntValorPago,
					//	EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
					//	CodEstado = d.IntEstadoPago,
					//	idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
					//	StrIdRegistro = d.StrIdRegistro,
					//	StrIdSeguridadDoc = d.StrIdRegistro2,
					//	Ciclo = d.IntClicloTransaccion,
					//	Ticket = d.StrTicketID,
					//	Cus = (d.IntFormaPago == 31) ? d.StrCampo1 : d.StrTransaccionCUS,//Si es tarjeta de credito, entonces el valor del campo Strcampo1, corresponde al cogido de aprobacion
					//	Franquicia = (string.IsNullOrEmpty(d.StrCodigoFranquicia)) ? "" : d.StrCodigoFranquicia.ToUpper(),
					//	Pagos = d.TblPagosDetalles.Select(p => new
					//	{
					//		Prefijo = p.TblDocumentos.StrPrefijo,
					//		Documento = p.TblDocumentos.IntNumero,
					//		Monto = p.IntValorPago,
					//	})


					//});

					return Ok();

				}
				catch (Exception excepcion)
				{

					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}


		/// <summary>
		/// Obtiene la lista de pagos realizadas a un adquiriente
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerPagosAdquiriente")]
		public IHttpActionResult ObtenerPagosAdquiriente(string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string codigo_facturador, int tipo_fecha)
		{

			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				try
				{
					codigo_adquiriente = Sesion.DatosUsuario.StrEmpresa;
				}
				catch (Exception)
				{
				}

				try
				{

					var datos = Pago.ObtenerPagos(codigo_facturador, numero_documento, codigo_adquiriente, fecha_inicio, fecha_fin, estado_recibo, "*", tipo_fecha);

					if (datos == null)
					{
						return NotFound();
					}

					var resultado = ConvertirPagos(datos);
					return Ok(resultado);
					//var retorno = datos.Select(d => new
					//{
					//	//NumeroDocumento = string.Format("{0}{1}", (!d.TblDocumentos.StrPrefijo.Equals("0")) ? d.TblDocumentos.StrPrefijo : "", d.TblDocumentos.IntNumero),
					//	StrEmpresaFacturador = d.StrEmpresaFacturador,
					//	NombreFacturador = d.TblEmpresas.StrRazonSocial,
					//	StrEmpresaAdquiriente = d.StrEmpresaAdquiriente,
					//	NombreAdquiriente = d.TblEmpresas1.StrRazonSocial,
					//	DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
					//	DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
					//	PagoFactura = d.IntValorPago,
					//	EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
					//	CodEstado = d.IntEstadoPago,
					//	idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
					//	StrIdRegistro = d.StrIdRegistro,
					//	StrIdSeguridadDoc = d.StrIdRegistro2,
					//	Ciclo = d.IntClicloTransaccion,
					//	Ticket = d.StrTicketID,
					//	Cus = (d.IntFormaPago == 31) ? d.StrCampo1 : d.StrTransaccionCUS,//Si es tarjeta de credito, entonces el valor del campo Strcampo1, corresponde al cogido de aprobacion
					//	Franquicia = (string.IsNullOrEmpty(d.StrCodigoFranquicia)) ? "" : d.StrCodigoFranquicia.ToUpper(),
					//	Pagos = d.TblPagosDetalles.Select(p => new
					//	{
					//		Prefijo = p.TblDocumentos.StrPrefijo,
					//		Documento = p.TblDocumentos.IntNumero,
					//		Monto = p.IntValorPago,
					//	})

					//});

					//return Ok(retorno);

				}
				catch (Exception excepcion)
				{
					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene la lista de pagos realizadas a un adquiriente
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>
		[HttpGet]
		[Route("Api/HGIpayObtenerPagosAdquiriente")]
		public IHttpActionResult HGIpayObtenerPagosAdquiriente(string IdSeguridad, string numero_documento, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, int tipo_fecha)
		{

			try
			{

				Sesion.ValidarSesionPagos();

				if (string.IsNullOrEmpty(IdSeguridad))
					throw new ApplicationException("Error de identificación del Facturador. no se encontro Serial");

				string codigo_adquiente = "";

				codigo_adquiente = Sesion.DatosUsuarioPagos.StrEmpresaAdquiriente;

				if (string.IsNullOrEmpty(codigo_adquiente))
					throw new ApplicationException("No se encontro información de los datos del Adquiriente");


				Ctl_Empresa _controlador_empresa = new Ctl_Empresa();
				TblEmpresas empresa = new TblEmpresas();
				empresa = _controlador_empresa.Obtener(Guid.Parse(IdSeguridad), false).FirstOrDefault();

				if (empresa == null)
					throw new ApplicationException("No se encontro información de los datos del Facturador");


				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();
				var datos = Pago.ObtenerPagos(empresa.StrIdentificacion, numero_documento, codigo_adquiente, fecha_inicio, fecha_fin, estado_recibo, "*", tipo_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				var resultado = ConvertirPagos(datos);
				return Ok(resultado);

				//Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();
				//var datos = Pago.HGIpayObtenerPagosAdquiriente(empresa.StrIdentificacion, numero_documento, codigo_adquiente, fecha_inicio, fecha_fin, estado_recibo, tipo_fecha);
				//if (datos == null)
				//{
				//	return NotFound();
				//}
				//var retorno = datos.Select(d => new
				//{
				//	//NumeroDocumento = string.Format("{0}{1}", (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.TblDocumentos.IntNumero),
				//	//StrEmpresaAdquiriente = d.TblDocumentos.StrEmpresaFacturador,
				//	//NombreAdquiriente = d.TblDocumentos.TblEmpresasFacturador.StrRazonSocial,
				//	DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
				//	DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
				//	PagoFactura = d.IntValorPago,
				//	EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
				//	CodEstado = d.IntEstadoPago,
				//	idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
				//	StrIdRegistro = d.StrIdRegistro,
				//	StrIdSeguridadDoc = d.StrIdRegistro2,
				//	Franquicia = (string.IsNullOrEmpty(d.StrCodigoFranquicia)) ? "" : d.StrCodigoFranquicia.ToUpper(),
				//	ticket = d.StrTicketID,
				//	cus = d.StrTransaccionCUS,
				//	ciclo = d.IntClicloTransaccion,
				//	detalle = d.TblPagosDetalles.Select(p => new
				//	{
				//		p.TblDocumentos.TblEmpresasFacturador.StrIdentificacion,
				//		p.TblDocumentos.TblEmpresasFacturador.StrRazonSocial,
				//		p.TblDocumentos.StrPrefijo,
				//		p.TblDocumentos.IntNumero,
				//		p.IntValorPago
				//	})
				//});

				//return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}


		}

		/// <summary>
		/// Actualiza el estado del pago consultando antes en la plataforma intermedia y luego con los datos de dicha plataforma, estos son actualziados en FE
		/// </summary>
		/// <param name="IdSeguridad">Id New Guid con el que se crea el pago</param>
		/// <param name="StrIdSeguridadRegistro">Id de seguridad del Documento</param>
		/// <param name="Pago">Objeto de pago que retorna la plataforma de zona de pagos</param>
		/// Aqui se debe colocar un parametro adicional con un cifrado para validar que quien 
		/// esta solicitando la actualizacion es una de las plataformas autorizadas
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ActualizarEstado")]

		public IHttpActionResult ActualizarEstado(Guid IdSeguridad, Guid StrIdSeguridadRegistro)
		{
			try
			{

				Ctl_PagosElectronicos _detalle = new Ctl_PagosElectronicos();
				var Id_Documento = _detalle.ObtenerPorRegistroPrincipal(StrIdSeguridadRegistro);

				Ctl_PagosElectronicos _pagos = new Ctl_PagosElectronicos();

				var pago_electronico = _pagos.Obtener(IdSeguridad, StrIdSeguridadRegistro);
				if (pago_electronico == null)
				{
					pago_electronico = _pagos.Obtener(Guid.Parse(Id_Documento), StrIdSeguridadRegistro);
				}


				//if (datos != null)
				//{

				//Ruta de consulta de estado de pago en la plataforma intermedia(Pagos electronicos)
				PasarelaPagos Ruta_servicio_pago = HgiConfiguracion.GetConfiguration().PasarelaPagos;
				//Aqui consulto el estado del pago en la plataforma intermedia de pagos
				ClienteRest<TblPasarelaPagosPI> cliente = new ClienteRest<TblPasarelaPagosPI>(string.Format("{0}?IdSeguridadPago={1}&StrIdSeguridadRegistro={2}", Ruta_servicio_pago.RutaServicio.ToString(), Id_Documento, StrIdSeguridadRegistro), TipoContenido.Applicationjson.GetHashCode(), "");
				TblPasarelaPagosPI ConfigPago = cliente.GET();
				//Como el objeto puede venir null de la plataforma de pago, se valida y se coloca los id de seguridad de la consulta
				if (ConfigPago.StrIdSeguridadDoc == Guid.Empty)
				{
					ConfigPago.StrIdSeguridadDoc = IdSeguridad;
					ConfigPago.StrIdSeguridadRegistro = StrIdSeguridadRegistro;
					ConfigPago.IntPagoEstado = EstadoPago.Pendiente.GetHashCode();

					pago_electronico = _pagos.Obtener(IdSeguridad, StrIdSeguridadRegistro);
					if (pago_electronico == null)
					{
						pago_electronico = _pagos.Obtener(Guid.Parse(Id_Documento), StrIdSeguridadRegistro);
						ConfigPago.StrIdSeguridadDoc = Guid.Parse(Id_Documento);
					}

					var fecha_pago = pago_electronico.DatFechaRegistro;
					var fecha_actual = Fecha.GetFecha();

					var diferencia = (fecha_actual - fecha_pago);

					//Si despues de este tiempo no tenemos datos del pago en la plataforma intermedia
					//Colocamos el estado de Pago no Iniciado
					if (diferencia.TotalMinutes > 15)
					{
						ConfigPago.IntPagoEstado = EstadoPago.NoIniciado.GetHashCode();
					}

				}
				else
				{
					//Si vienes datos para actualizar se hace un cifrado para validar la llave de encriptacion
					string CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.StrIdSeguridadComercio + "-" + ConfigPago.IntValor.ToString("0.##"));
					if (ConfigPago.StrAuthIdEmpresa != CifradoSecundario)
					{
						return Conflict();
					}
				}
				Ctl_PagosElectronicos pago = new Ctl_PagosElectronicos();
				//Luego aqui, actualizo el resultado que me retorno la plataforma de pagos
				var Detalle = pago.ActualizarPago(ConfigPago);
				return Ok();
				//}
				//return Ok();
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				return Conflict();
			}
		}



		[HttpGet]
		[Route("Api/ActualizarEstadoPI")]

		public IHttpActionResult ActualizarEstadoPI(Guid IdSeguridad, Guid StrIdSeguridadRegistro)
		{
			try
			{

				//Ruta de consulta de estado de pago en la plataforma intermedia(Pagos electronicos)
				PasarelaPagos Ruta_servicio_pago = HgiConfiguracion.GetConfiguration().PasarelaPagos;
				//Aqui consulto el estado del pago en la plataforma intermedia de pagos
				ClienteRest<TblPasarelaPagosPI> cliente = new ClienteRest<TblPasarelaPagosPI>(string.Format("{0}?IdSeguridadPago={1}&StrIdSeguridadRegistro={2}", Ruta_servicio_pago.RutaServicio.ToString(), IdSeguridad, StrIdSeguridadRegistro), TipoContenido.Applicationjson.GetHashCode(), "");
				TblPasarelaPagosPI ConfigPago = cliente.GET();
				//Como el objeto puede venir null de la plataforma de pago, se valida y se coloca los id de seguridad de la consulta
				if (ConfigPago.StrIdSeguridadDoc == Guid.Empty)
				{
					ConfigPago.StrIdSeguridadDoc = IdSeguridad;
					ConfigPago.StrIdSeguridadRegistro = StrIdSeguridadRegistro;
					ConfigPago.IntPagoEstado = EstadoPago.Pendiente.GetHashCode();

					Ctl_PagosElectronicos _pagos = new Ctl_PagosElectronicos();

					var pago_electronico = _pagos.Obtener(IdSeguridad, StrIdSeguridadRegistro);

					var fecha_pago = pago_electronico.DatFechaRegistro;
					var fecha_actual = Fecha.GetFecha();

					var diferencia = (fecha_actual - fecha_pago);

					//Si despues de este tiempo no tenemos datos del pago en la plataforma intermedia
					//Colocamos el estado de Pago no Iniciado
					if (diferencia.TotalMinutes > 15)
					{
						ConfigPago.IntPagoEstado = EstadoPago.NoIniciado.GetHashCode();
					}

				}
				else
				{
					//Si vienes datos para actualizar se hace un cifrado para validar la llave de encriptacion
					string CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.StrIdSeguridadComercio + "-" + ConfigPago.IntValor.ToString("0.##"));
					if (ConfigPago.StrAuthIdEmpresa != CifradoSecundario)
					{
						return Conflict();
					}
				}
				Ctl_PagosElectronicos pago = new Ctl_PagosElectronicos();
				//Luego aqui, actualizo el resultado que me retorno la plataforma de pagos
				var Detalle = pago.ActualizarPago(ConfigPago);
				return Ok();

			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				return Conflict();
			}
		}

		/// <summary>
		/// Este metodo es invocado desde la plataforma de pago, se usa para actualziar el estado de un pago
		/// Recibe en la cabezera el objeto de pago y la llave de encripción necesaria para validar que el pago es seguro
		/// /// </summary>
		/// <returns></returns>
		[HttpPut]
		[Route("Api/SrcActualizaEstado")]
		public IHttpActionResult SrcActualizaEstado()
		{
			try
			{
				System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
				string Pago = string.Empty;
				string CodValidacion = string.Empty;
				string ContrasenaActual = string.Empty;

				if (headers.Contains("Pago"))
				{
					Pago = headers.GetValues("Pago").First();
				}

				if (headers.Contains("CodValidacion"))
				{
					CodValidacion = headers.GetValues("CodValidacion").First();
				}

				var ConfigPago = JsonConvert.DeserializeObject<TblPasarelaPagosPI>(Pago);

				string CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.StrIdSeguridadComercio + "-" + ConfigPago.IntValor.ToString("0.##"));

				if (CodValidacion != CifradoSecundario)
				{
					return Ok(false);
				}

				var ObjetoPago = JsonConvert.DeserializeObject<TblPasarelaPagosPI>(Pago);

				Ctl_PagosElectronicos pago = new Ctl_PagosElectronicos();

				var Detalle = pago.ActualizarPago(ObjetoPago);

				return Ok(true);
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				return Ok(false);
			}
		}


		[HttpPost]
		[Route("Api/SrcActualizaEstadoP")]
		public IHttpActionResult SrcActualizaEstadoP()
		{
			try
			{
				System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
				string Pago = string.Empty;
				string CodValidacion = string.Empty;
				string ContrasenaActual = string.Empty;

				if (headers.Contains("Pago"))
				{
					Pago = headers.GetValues("Pago").First();
				}

				if (headers.Contains("CodValidacion"))
				{
					CodValidacion = headers.GetValues("CodValidacion").First();
				}

				var ConfigPago = JsonConvert.DeserializeObject<TblPasarelaPagosPI>(Pago);

				string CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.StrIdSeguridadComercio + "-" + ConfigPago.IntValor.ToString("0.##"));

				if (CodValidacion != CifradoSecundario)
				{
					return Ok(false);
				}

				var ObjetoPago = JsonConvert.DeserializeObject<TblPasarelaPagosPI>(Pago);

				Ctl_PagosElectronicos pago = new Ctl_PagosElectronicos();

				var Detalle = pago.ActualizarPago(ObjetoPago);

				return Ok(true);
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				return Ok(false);
			}
		}
		#endregion



		////Consulta del Webpart
		//[HttpGet]
		//[Route("api/ConsultarPagosFueraPlataforma")]
		//public HttpResponseMessage ConsultarPagosFueraPlataforma(System.Guid IdSeguridad, string identificacion, string documento, string prefijo)
		//{

		//	try
		//	{
		//		if (string.IsNullOrEmpty(IdSeguridad.ToString()))
		//		{
		//			return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro serial en la petición");
		//		}

		//		if (string.IsNullOrEmpty(identificacion))
		//		{
		//			return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro identificación en la petición");
		//		}



		//		Ctl_Empresa _empresa = new Ctl_Empresa();
		//		TblEmpresas empresa = _empresa.Obtener(IdSeguridad, false).FirstOrDefault();


		//		if (empresa == null)
		//		{
		//			return Request.CreateResponse(HttpStatusCode.InternalServerError, "Empresa no existe o serial invalido");
		//		}

		//		bool empresa_maneja_lista_documentos = true;


		//		if (!empresa.IntManejaPagoE)
		//		{
		//			return Request.CreateResponse(HttpStatusCode.InternalServerError, "Empresa no maneja pagos");
		//		}

		//		if (!empresa.IntPagosPermiteConsTodos)
		//		{
		//			if (string.IsNullOrEmpty(documento))
		//			{
		//				return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro documento en la petición");
		//			}
		//		}


		//		Ctl_Documento Controlador = new Ctl_Documento();
		//		List<QryDocumentosSaldo> datos = Controlador.ConsultarPagosFueraPlataforma(empresa.StrIdentificacion, documento, identificacion, empresa_maneja_lista_documentos);

		//		if (datos.Count() == 0)
		//		{
		//			return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro ningún documento con los criterios de busqueda");
		//		}


		//		var retorno = datos.Select(d => new
		//		{
		//			PagosParciales = empresa.IntPagoEParcial,
		//			d.StrIdSeguridad,
		//			d.StrPrefijo,
		//			FechaDocumento = d.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet),
		//			d.IntNumero,
		//			d.IntVlrTotal
		//		});

		//		return Request.CreateResponse(HttpStatusCode.OK, retorno);

		//	}
		//	catch (Exception ex)
		//	{
		//		return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
		//	}

		//}


		//Consulta del Webpart
		[HttpGet]
		[Route("api/ConsultarPagosFueraPlataforma")]
		public HttpResponseMessage ConsultarPagosFueraPlataforma(System.Guid IdSeguridad, string identificacion, string documento, string prefijo)
		{

			try
			{
				///Validamos si viene serial en la petición
				if (string.IsNullOrEmpty(IdSeguridad.ToString()))
				{
					return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro serial en la petición");
				}

				///Validamos si viene identificación en la petición
				if (string.IsNullOrEmpty(identificacion))
				{
					return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro identificación en la petición");
				}

				Ctl_Empresa _empresa = new Ctl_Empresa();
				///Consultar la empresa
				TblEmpresas empresa = _empresa.Obtener(IdSeguridad, false).FirstOrDefault();

				///Validamos si la empresa existe
				if (empresa == null)
				{
					return Request.CreateResponse(HttpStatusCode.InternalServerError, "Empresa no existe o serial invalido");
				}

				///Validamos si la empresa maneja pagos
				if (!empresa.IntManejaPagoE)
				{
					return Request.CreateResponse(HttpStatusCode.InternalServerError, "Empresa no maneja pagos");
				}

				///Validamos si la empresa maneja la modalidad de consultar la lista de documentos
				if (!empresa.IntPagosPermiteConsTodos)
				{
					if (string.IsNullOrEmpty(documento))
					{
						return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro documento en la petición");
					}
				}


				Ctl_Documento Controlador = new Ctl_Documento();
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();
				///Consultamos los documentos
				List<ObjDocumentos> datos2 = Controlador.ConsultarPagosFueraPlataforma(identificacion, empresa.StrIdentificacion, documento);
				///Validamos si la consulta retorna documentos
				if (datos2.Count() == 0)
				{
					return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se encontro ningún documento con los criterios de busqueda");
				}

				///Filtramos que los documentos tengan saldo pendiente por pago.
				var retorno = datos2.Select(d => new
				{
					d.PagosParciales,
					d.IdComercio,
					d.StrIdSeguridad,
					StrPrefijo = d.Prefijo,
					FechaDocumento = d.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet),
					IntNumero = d.NumeroDocumento,
					d.IntVlrTotal,
					d.FacturaCancelada,
					Saldo = Pago.ConsultaSaldoDocumentoPM(d.StrIdSeguridad, d.IntValorPagar),
					IdsPago = Pago.ConsultaIDSeguridadPagos(d.StrIdSeguridad)
				}).Where(x => x.FacturaCancelada != 1000);
				//}).Where(x => x.Saldo > 0).Where(x => x.FacturaCancelada != 1000);

				return Request.CreateResponse(HttpStatusCode.OK, retorno);

			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

		}


		[HttpGet]
		[Route("api/ValidarTextoXYPDF")]
		public HttpResponseMessage ValidarTextoXYPDF(Guid guid_facturador, string identificacion_facturador, int tipo_documento, string numero_documento, string numero_resolucion, decimal posicion_x, decimal posicion_y)
		{
			try
			{
				Ctl_Documento Controlador = new Ctl_Documento();
				string retorno = Controlador.ValidarTextoXYPDF(guid_facturador, identificacion_facturador, tipo_documento, numero_documento, numero_resolucion, posicion_x, posicion_y);

				return Request.CreateResponse(HttpStatusCode.OK, retorno);

			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
				//throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}

		/// <summary>
		/// Objeto de respuesta para todas las consultas de pagos
		/// Consulta de Pagos Recibidos
		/// Consulta de Pagos Realizados
		/// COnsulta de Pagos Administracion
		/// Consulta de Pagos Usuarios de Pagos
		/// </summary>
		/// <param name="datos">List<TblPagosElectronicos></param>
		/// <returns>object con la Respuesta</returns>
		public static object ConvertirPagos(List<TblPagosElectronicos> datos)
		{
			var resultado = datos.Select(d => new
			{
				StrEmpresaFacturador = d.StrEmpresaFacturador,
				NombreFacturador = d.TblEmpresas.StrRazonSocial,
				StrEmpresaAdquiriente = d.StrEmpresaAdquiriente,
				NombreAdquiriente = d.TblEmpresas1.StrRazonSocial,
				DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
				DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
				PagoFactura = d.IntValorPago,
				EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
				CodEstado = d.IntEstadoPago,
				idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
				StrIdRegistro = d.StrIdRegistro,
				StrIdSeguridadDoc = d.StrIdRegistro2,
				Ciclo = d.IntClicloTransaccion,
				Ticket = d.StrTicketID,
				Cus = (d.IntFormaPago == 31) ? d.StrCampo1 : d.StrTransaccionCUS,//Si es tarjeta de credito, entonces el valor del campo Strcampo1, corresponde al cogido de aprobacion
				Franquicia = (string.IsNullOrEmpty(d.StrCodigoFranquicia)) ? "" : d.StrCodigoFranquicia.ToUpper(),
				Pagos = d.TblPagosDetalles.Select(p => new
				{
					Prefijo = p.TblDocumentos.StrPrefijo,
					Documento = p.TblDocumentos.IntNumero,
					Monto = p.IntValorPago,
				})

			});
			return resultado;
		}

		public static object ConvertirDetalles(List<TblPagosDetalles> datos)
		{
			var resultado = datos.Select(d => new
			{
				StrEmpresaFacturador = d.TblPagosElectronicos.StrEmpresaFacturador,
				NombreFacturador = d.TblPagosElectronicos.TblEmpresas.StrRazonSocial,
				StrEmpresaAdquiriente = d.TblPagosElectronicos.StrEmpresaAdquiriente,
				NombreAdquiriente = d.TblPagosElectronicos.TblEmpresas1.StrRazonSocial,
				DatAdquirienteFechaRecibo = (d.TblPagosElectronicos.DatFechaRegistro != null) ? d.TblPagosElectronicos.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
				DatFechaVencDocumento = (d.TblPagosElectronicos.DatFechaVerificacion != null) ? d.TblPagosElectronicos.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
				PagoFactura = d.IntValorPago,
				EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.TblPagosElectronicos.IntEstadoPago)),
				CodEstado = d.TblPagosElectronicos.IntEstadoPago,
				idseguridadpago = (d.TblPagosElectronicos.StrIdSeguridadPago == null) ? "" : d.TblPagosElectronicos.StrIdSeguridadPago,
				StrIdRegistro = d.TblPagosElectronicos.StrIdRegistro,
				StrIdSeguridadDoc = d.TblPagosElectronicos.StrIdRegistro2,
				Ciclo = d.TblPagosElectronicos.IntClicloTransaccion,
				Ticket = d.TblPagosElectronicos.StrTicketID,
				Cus = (d.TblPagosElectronicos.IntFormaPago == 31) ? d.TblPagosElectronicos.StrCampo1 : d.TblPagosElectronicos.StrTransaccionCUS,//Si es tarjeta de credito, entonces el valor del campo Strcampo1, corresponde al cogido de aprobacion
				Franquicia = (string.IsNullOrEmpty(d.TblPagosElectronicos.StrCodigoFranquicia)) ? "" : d.TblPagosElectronicos.StrCodigoFranquicia.ToUpper(),
				Prefijo = d.TblDocumentos.StrPrefijo,
				Documento = d.TblDocumentos.IntNumero,
			});
			return resultado;
		}


		[HttpGet]
		[Route("Api/ConsultarEventosRadian")]

		public IHttpActionResult ConsultarEventosRadian(string List_IdSeguridad)
		{
			try
			{
				Ctl_Documento Controlador = new Ctl_Documento();

				Controlador.ConsultarEventosRadian(false, List_IdSeguridad);

				return Ok();
				//}
				//return Ok();
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				return Conflict();
			}
		}

	}
}
