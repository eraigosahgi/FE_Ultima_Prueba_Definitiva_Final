using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.PagosElectronicos;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea;
using LibreriaGlobalHGInet.Peticiones;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
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
		public IHttpActionResult Get(string codigo_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasAdquiriente(codigo_adquiente, numero_documento, estado_recibo, fecha_inicio.Date, fecha_fin.Date, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),					
					DatFechaDocumento = d.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet),
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode()) ? -d.IntVlrTotal : d.IntVlrTotal,                    
                    EstadoFactura = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
                    EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					NombreFacturador = d.TblEmpresasFacturador.StrRazonSocial,
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					XmlAcuse = d.StrUrlAcuseUbl,
					d.StrIdSeguridad,
					RutaPublica = plataforma.RutaPublica,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian):"",
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					poseeIdComercio = (d.TblEmpresasResoluciones.IntComercioId != null) ? (d.IntIdEstado != 90) ? 1 : 0 : 0,
					FacturaCenlada = d.IntIdEstado,
					PagosParciales = (d.TblEmpresasResoluciones.IntPermiteParciales == null) ? 0 : (d.TblEmpresasResoluciones?.IntPermiteParciales == true) ? 1 : 0,
					Telefono = d.TblEmpresasFacturador.StrTelefono,
					Email = d.TblEmpresasFacturador.StrMail,
					zip = d.StrUrlAnexo,
                    Estado = d.IdCategoriaEstado,
                    permiteenvio = ((Int16)d.IdCategoriaEstado == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false
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
		public IHttpActionResult Get(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, string Resolucion, int tipo_filtro_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasObligado(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Resolucion, tipo_filtro_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),					
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode()) ? -d.IntVlrTotal : d.IntVlrTotal,
					EstadoFactura = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					XmlAcuse = d.StrUrlAcuseUbl,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian):"",
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					poseeIdComercio = (d.TblEmpresasResoluciones.IntComercioId == null) ? false : (d.TblEmpresasResoluciones.IntComercioId > 0) ? true : false,
					zip = d.StrUrlAnexo,
                    permiteenvio=((Int16)d.IdCategoriaEstado == CategoriaEstado.ValidadoDian.GetHashCode())?true:false,
                    Estado = d.IdCategoriaEstado
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
					RespuestaVisible = (d.IntAdquirienteRecibo != 0) ? true : false,
					CamposVisibles = (d.IntAdquirienteRecibo == 0) ? true : false,
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					poseeIdComercio = (d.TblEmpresasResoluciones.IntComercioId == null) ? false : (d.TblEmpresasResoluciones.IntComercioId > 0) ? true : false,
					Estatus = Pago.VerificarSaldo(id_seguridad),
					XmlAcuse = d.StrUrlAcuseUbl,
                    EstadoCat = d.IdCategoriaEstado,
                    EstadoFactura = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
                    pago = d.TblPagosElectronicos.Select(p => new
					{
						p.StrIdRegistro,
						p.IntEstadoPago
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


		public IHttpActionResult Get(System.Guid id_seguridad, string email)
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

				Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
				bool respuesta = false;

				//Valida el estado del documento para saber que tipo de notificacion enviar
				if (datos.FirstOrDefault().IdCategoriaEstado < CategoriaEstado.ValidadoDian.GetHashCode())
				{
					respuesta = clase_email.NotificacionBasica(datos.FirstOrDefault(), "", email);
				}
				else
				{
					respuesta = clase_email.NotificacionDocumento(datos.FirstOrDefault(), "", email);
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
                    MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
                    RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
                    Xml = d.StrUrlArchivoUbl,
                    Pdf = d.StrUrlArchivoPdf,                                                           
                    zip = d.StrUrlAnexo,
                    RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
                    XmlAcuse = d.StrUrlAcuseUbl,
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
				List<TblDocumentos> datos = ctl_documento.ObtenerAcuseTacito(codigo_facturador, numero_documento, codigo_adquiriente);

				var retorno = datos.Select(d => new
				{
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					RazonSocial = d.TblEmpresasAdquiriente.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
					DocSinPrefijo =  d.IntNumero,
					Fecha = d.DatFechaDocumento,
					FechaIngreso = d.DatFechaIngreso,
					Estado = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					IdentificacionFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					NombreFacturador = d.TblEmpresasFacturador.StrRazonSocial,
					d.StrIdSeguridad,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
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

				List<TblDocumentos> ListaDocumentos = new List<TblDocumentos>();

				List<System.Guid> List_id_seguridad = new List<Guid>();

				foreach (var item in ListadeDocumentos)
				{
					List_id_seguridad.Add(item.Documentos);
				}

				Ctl_Documento documento = new Ctl_Documento();

				var lista = documento.ProcesarDocumentos(List_id_seguridad);

				List<TblDocumentos> datos = new List<TblDocumentos>();

				foreach (var item in lista)
				{
					try
					{
						documento = new Ctl_Documento();
						datos = documento.ActualizarRespuestaAcuse(item.StrIdSeguridad, (short)AdquirienteRecibo.AprobadoTacito.GetHashCode(), Enumeracion.GetDescription(AdquirienteRecibo.AprobadoTacito));

					}
					catch (Exception ex)
					{
						LogExcepcion.Guardar(ex);
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
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri]short estado, [FromUri]string motivo_rechazo)
		{
			try
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ActualizarRespuestaAcuse(id_seguridad, estado, motivo_rechazo);

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
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					RespuestaVisible = (d.IntAdquirienteRecibo == 1 || d.IntAdquirienteRecibo == 2) ? true : false,
					CamposVisibles = (d.IntAdquirienteRecibo == 0) ? true : false
				});
				return Ok(retorno);

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
					Documento = string.Format("{0}{1}", (d.Prefijo != null) ? d.Prefijo:string.Empty, d.Documento),
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
                    RutaServDian = (d.IntIdEstado<7)?"": (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
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
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri] string mail)
		{
			try
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();
				bool email = ctl_documento.ReenviarRespuestaAcuse(id_seguridad, mail);

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
					IntVlrTotal = (d.IntDocTipo == 3) ? -d.IntVlrTotal : d.IntVlrTotal,
					EstadoFactura = string.Format("{0} - {1}", DescripcionEstadoFactura(d.IntIdEstado), DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado)),
                    EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
                    zip = d.StrUrlAnexo,
                    RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
                    XmlAcuse = d.StrUrlAcuseUbl,
                    permiteenvio = ((Int16)d.IdCategoriaEstado == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false
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
		[HttpGet]
		public IHttpActionResult Get(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin, int TipoDocumento,int tipo_fecha)
		{
			try
			{
				Sesion.ValidarSesion();

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				Ctl_Documento ctl_documento = new Ctl_Documento();
				List<TblDocumentos> datos = ctl_documento.ObtenerAdmin(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, TipoDocumento, tipo_fecha);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					IdFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					Facturador = d.TblEmpresasFacturador.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					d.DatFechaDocumento,
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == 3) ? -d.IntVlrTotal : d.IntVlrTotal,
					EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
                    EstadoCategoria = DescripcionCategoriaFactura((Int16)d.IdCategoriaEstado),
                    EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
                    zip = d.StrUrlAnexo,
                    RutaServDian = (d.StrUrlArchivoUbl != null) ? d.StrUrlArchivoUbl.Replace("FacturaEDian", LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian) : "",
                    XmlAcuse = d.StrUrlAcuseUbl,
                    permiteenvio = ((Int16)d.IdCategoriaEstado == CategoriaEstado.ValidadoDian.GetHashCode()) ? true : false,
                    Estado = d.IdCategoriaEstado
                });

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}
		#endregion

		#region Zonas de pago
		/// <summary>
		/// Obtiene link para el pago de factura
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>

		public IHttpActionResult Get(System.Guid strIdSeguridad, int tipo_pago = 0, bool registrar_pago = true, double valor_pago = 0)
		{
			Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

			var datos = Pago.ReportePagoElectronicoPI(strIdSeguridad, tipo_pago, registrar_pago, valor_pago);
			return Ok(datos);

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
					Mail = d.TblEmpresasFacturador.StrMail,
					DocTipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo)),
					IntNumero = d.IntNumero,
					FechaDocumento = d.DatFechaDocumento.ToString(Fecha.formato_fecha_hginet),

					//Detalle del pago
					Pagos = d.TblPagosElectronicos.Select(p => new
					{
						Monto = p.IntValorPago,
						FechaRegistro = p.DatFechaRegistro,
						FechaVerificacion = p.DatFechaVerificacion,
						StrIdSeguridadPago = p.StrIdSeguridadPago,
						IdRegistro = p.StrIdRegistro,
						Estado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(p.IntEstadoPago)),
					})

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
		[Route("Api/ObtenerPagosFacturador")]
		public IHttpActionResult ObtenerPagosFacturador(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string resolucion, int tipo_fecha)
		{

			try
			{
				Ctl_PagosElectronicos Pago = new Ctl_PagosElectronicos();

				var datos = Pago.ObtenerPagosFacturador(codigo_facturador, numero_documento, codigo_adquiriente, fecha_inicio, fecha_fin, estado_recibo, resolucion, tipo_fecha);


				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					NumeroDocumento = string.Format("{0}{1}", (!d.TblDocumentos.StrPrefijo.Equals("0")) ? d.TblDocumentos.StrPrefijo : "", d.TblDocumentos.IntNumero),
					StrEmpresaAdquiriente = d.TblDocumentos.StrEmpresaAdquiriente,
					NombreAdquiriente = d.TblDocumentos.TblEmpresasAdquiriente.StrRazonSocial,
					DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
					DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
					PagoFactura = (d.IntValorPago == null) ? 0 : d.IntValorPago,
					//EstadoFactura = (d.IntEstadoPago == 0) ? "Rechazado" : (d.IntEstadoPago == 1) ? "Aprobado" : (d.IntEstadoPago == 999) ? "Pendiente" : "",
					EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
					CodEstado = d.IntEstadoPago,
					idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
					StrIdRegistro = d.StrIdRegistro,
					StrIdSeguridadDoc = d.StrIdSeguridadDoc


				});

				return Ok(retorno);
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

				var datos = Pago.ObtenerPagosAdquiriente((string.IsNullOrEmpty(codigo_facturador)) ? "*" : codigo_facturador, numero_documento, codigo_adquiriente, fecha_inicio, fecha_fin, estado_recibo, tipo_fecha);


				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{

					NumeroDocumento = string.Format("{0}{1}", (!d.TblDocumentos.StrPrefijo.Equals("0")) ? d.TblDocumentos.StrPrefijo : "", d.TblDocumentos.IntNumero),
					StrEmpresaAdquiriente = d.TblDocumentos.StrEmpresaFacturador,
					NombreAdquiriente = d.TblDocumentos.TblEmpresasFacturador.StrRazonSocial,
					DatAdquirienteFechaRecibo = (d.DatFechaRegistro != null) ? d.DatFechaRegistro.ToString(Fecha.formato_fecha_hora) : "",
					DatFechaVencDocumento = (d.DatFechaVerificacion != null) ? d.DatFechaVerificacion?.ToString(Fecha.formato_fecha_hora) : "",
					PagoFactura = (d.IntValorPago == null) ? 0 : d.IntValorPago,
					EstadoFactura = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPago>(d.IntEstadoPago)),
					CodEstado = d.IntEstadoPago,
					idseguridadpago = (d.StrIdSeguridadPago == null) ? "" : d.StrIdSeguridadPago,
					StrIdRegistro = d.StrIdRegistro,
					StrIdSeguridadDoc = d.StrIdSeguridadDoc
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{

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
                //Ruta de consulta de estado de pago en la plataforma intermedia(Pagos electronicos)
                PasarelaPagos Ruta_servicio_pago = HgiConfiguracion.GetConfiguration().PasarelaPagos;
                //Aqui consulto el estado del pago en la plataforma intermedia de pagos
                ClienteRest<TblPasarelaPagosPI> cliente = new ClienteRest<TblPasarelaPagosPI>(string.Format("{0}?IdSeguridadPago={1}&StrIdSeguridadRegistro={2}", Ruta_servicio_pago.RutaServicio.ToString(), IdSeguridad, StrIdSeguridadRegistro),TipoContenido.Applicationjson.GetHashCode() , "");
                TblPasarelaPagosPI ConfigPago = cliente.GET();
                //Como el objeto puede venir null de la plataforma de pago, se valida y se coloca los id de seguridad de la consulta
                if (ConfigPago.StrIdSeguridadDoc == Guid.Empty)
                {
                    ConfigPago.StrIdSeguridadDoc = IdSeguridad;
                    ConfigPago.StrIdSeguridadRegistro = StrIdSeguridadRegistro;
                }
                else
                {
                    //Si vienes datos para actualizar se hace un cifrado para validar la llave de encriptacion
                    string CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.IntComercioId + "-" + ConfigPago.IntValor.ToString("0.##"));
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

                string CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.IntComercioId + "-" + ConfigPago.IntValor.ToString("0.##"));

                if (CodValidacion!=CifradoSecundario)
                {
                    return Ok(false);
                }

                var ObjetoPago = JsonConvert.DeserializeObject<TblPasarelaPagosPI>(Pago);

                Ctl_PagosElectronicos pago = new Ctl_PagosElectronicos();
                
                var Detalle = pago.ActualizarPago(ObjetoPago);

                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
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
                Ctl_Documento Controlador = new Ctl_Documento();
                TblDocumentos datos = Controlador.ObtenerPorIdSeguridad(IdSeguridad).FirstOrDefault();
                var objeto = (dynamic)null;
                objeto = Ctl_Documento.ConvertirServicio(datos,true);
                string correo = objeto.DatosFactura.DatosAdquiriente.Email;
                return Request.CreateResponse(HttpStatusCode.OK, correo);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
                //throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }
    }
}
