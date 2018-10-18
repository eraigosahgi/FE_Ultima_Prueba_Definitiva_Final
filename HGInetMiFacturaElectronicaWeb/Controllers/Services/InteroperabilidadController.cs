using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using static HGInetMiFacturaElectronicaWeb.Controllers.Services.DocumentosController;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class InteroperabilidadController : ApiController
	{
		/// <summary>
		/// Obtiene los formatos
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerPendientesProceso")]
		public IHttpActionResult ObtenerPendientesProceso(string identificacion_proveedor)
		{
			try
			{
				Sesion.ValidarSesion();
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<TblDocumentos> docs_pendientes = new List<TblDocumentos>();

				Ctl_Documento clase_documentos = new Ctl_Documento();

				docs_pendientes = clase_documentos.ObtenerDocumentosProveedores(identificacion_proveedor);

				if (docs_pendientes == null)
				{
					return NotFound();
				}

				var datos_retorno = docs_pendientes.Select(d => new
				{
					IdFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					Facturador = d.TblEmpresasFacturador.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					d.DatFechaDocumento,
					d.DatFechaIngreso,
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == 3) ? -d.IntVlrTotal : d.IntVlrTotal,
					EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
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
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo))
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/ObtenerAcusePendienteRecepcion")]
		public IHttpActionResult ObtenerAcusePendienteRecepcion(string identificacion_proveedor)
		{
			try
			{
				Sesion.ValidarSesion();
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<TblDocumentos> docs_pendientes = new List<TblDocumentos>();

				Ctl_Documento clase_documentos = new Ctl_Documento();

				docs_pendientes = clase_documentos.ObtenerAcusePendienteRecepcion(identificacion_proveedor);

				if (docs_pendientes == null)
				{
					return NotFound();
				}

				var datos_retorno = docs_pendientes.Select(d => new
				{
					IdFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					Facturador = d.TblEmpresasFacturador.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					d.DatFechaDocumento,
					d.DatFechaIngreso,
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == 3) ? -d.IntVlrTotal : d.IntVlrTotal,
					EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
					d.StrIdSeguridad,
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo))
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los formatos
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerProveedores")]
		public IHttpActionResult ObtenerProveedores()
		{
			try
			{
				Sesion.ValidarSesion();
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<TblConfiguracionInteroperabilidad> proveedores = new List<TblConfiguracionInteroperabilidad>();

				Ctl_ConfiguracionInteroperabilidad clase_inter = new Ctl_ConfiguracionInteroperabilidad();

				proveedores = clase_inter.ObtenerProveedores("*");

				if (proveedores == null)
				{
					return NotFound();
				}


                Ctl_Documento clase_documentos = new Ctl_Documento();

                

                var datos_retorno = proveedores.Select(d => new
				{
					Identificacion = d.StrIdentificacion,
                    RazonSocial = d.StrRazonSocial,
                    Dp =clase_documentos.ObtenerDocumentosProveedores(d.StrIdentificacion).Count,
                    Ap =clase_documentos.ObtenerAcusePendienteRecepcion(d.StrIdentificacion).Count,
                    BitActivo=d.BitActivo,
                    DatFechaActualizacion=d.DatFechaActualizacion,
                    DatFechaExpiracion=d.DatFechaExpiracion,
                    DatFechaIngreso=d.DatFechaIngreso,
                    DatHgiFechaToken=d.DatHgiFechaToken,
                    StrClave=d.StrClave,
                    StrHgiClave=d.StrHgiClave,
                    StrHgiUsuario=d.StrHgiUsuario,
                    StrIdSeguridad=d.StrIdSeguridad,
                    StrObservaciones=d.StrObservaciones,
                    StrTelefono=d.StrTelefono,
                    StrUrlApi=d.StrUrlApi,
                    StrUrlFtp=d.StrUrlFtp,
                    StrUsuario=d.StrUsuario,
                    StrMail=d.StrMail                 

                });

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}




        /// <summary>
		/// Obtiene un proveedor especifico
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
        [Route("Api/ObtenerProveedores")]
        public IHttpActionResult ObtenerProveedores(Guid identificacion)
        {
            try
            {
                Sesion.ValidarSesion();
                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                //List<TblConfiguracionInteroperabilidad> proveedores = new List<TblConfiguracionInteroperabilidad>();

                Ctl_ConfiguracionInteroperabilidad clase_inter = new Ctl_ConfiguracionInteroperabilidad();

                var proveedores = clase_inter.ObtenerProveedores(identificacion);

                if (proveedores == null)
                {
                    return NotFound();
                }


                Ctl_Documento clase_documentos = new Ctl_Documento();



                var datos_retorno = proveedores.Select(d => new
                {
                    Identificacion = d.StrIdentificacion,
                    RazonSocial = d.StrRazonSocial,
                    Dp = clase_documentos.ObtenerDocumentosProveedores(d.StrIdentificacion).Count,
                    Ap = clase_documentos.ObtenerAcusePendienteRecepcion(d.StrIdentificacion).Count,
                    BitActivo = d.BitActivo,
                    DatFechaActualizacion = d.DatFechaActualizacion,
                    DatFechaExpiracion = d.DatFechaExpiracion,
                    DatFechaIngreso = d.DatFechaIngreso,
                    DatHgiFechaToken = d.DatHgiFechaToken,
                    StrClave = d.StrClave,
                    StrHgiClave = d.StrHgiClave,
                    StrHgiUsuario = d.StrHgiUsuario,
                    StrIdSeguridad = d.StrIdSeguridad,
                    StrObservaciones = d.StrObservaciones,
                    StrTelefono = d.StrTelefono,
                    StrUrlApi = d.StrUrlApi,
                    StrUrlFtp = d.StrUrlFtp,
                    StrUsuario = d.StrUsuario,
                    StrMail = d.StrMail

                });

                return Ok(datos_retorno);
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

		private string DescripcionTipodDoc(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		/// <summary>
		/// Recibe lista de Documentos 
		/// </summary>
		/// <param name="objeto"></param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post(Object objeto)
		{
			try
			{
				Ctl_Documento clase_doc = new Ctl_Documento();

				var jobjeto = (dynamic)objeto;

				string ListaDoc = jobjeto.Documentos;

				List<DocumentosJSON> ListadeDocumentos = new JavaScriptSerializer().Deserialize<List<DocumentosJSON>>(ListaDoc);

				List<TblDocumentos> ListaDocumentos = new List<TblDocumentos>();

				foreach (var item in ListadeDocumentos)
				{
					TblDocumentos doc = clase_doc.ObtenerPorIdSeguridad(item.Documentos).First();
					if (doc != null)
						ListaDocumentos.Add(doc);
				}

				List<RespuestaRegistro> Respuesta = Ctl_Envio.Procesar(ListaDocumentos);


                var datos = Respuesta.Select(d => new
                {
                    MensajeZip= d.MensajeZip,              
                    Documento=d.Documento.IntNumero,
                    Mensaje=d.Respuesta.mensaje,
                    uuid =d.Respuesta.uuid,
                    codigoError= d.Respuesta.codigoError,                    
                    tipodoc = (d.Documento.IntIdEstado==0)? "": (d.Documento.IntIdEstado == (Int16) ProcesoEstado.Finalizacion.GetHashCode())? Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(TipoDocumento.AcuseRecibo.GetHashCode())) :  Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.Documento.IntDocTipo)),
                    FechaUltimoProceso = d.Documento.DatFechaActualizaEstado,
                    EstadoFactura = (d.Documento.IntIdEstado == 0) ? "" :  DescripcionEstadoFactura(d.Documento.IntIdEstado),
                    Nombre = d.Respuesta.nombreDocumento
                });
                    
               
                return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

        [HttpPost]
        [Route("Api/ProcesarAcusesPRecepcion")]
        public IHttpActionResult ProcesarAcusesPRecepcion(Object objeto)
        {
            try
            {
                Ctl_Documento clase_doc = new Ctl_Documento();

                var jobjeto = (dynamic)objeto;

                string ListaDoc = jobjeto.Documentos;

                List<DocumentosJSON> ListadeDocumentos = new JavaScriptSerializer().Deserialize<List<DocumentosJSON>>(ListaDoc);

                List<TblDocumentos> ListaDocumentos = new List<TblDocumentos>();

                foreach (var item in ListadeDocumentos)
                {
                    TblDocumentos doc = clase_doc.ObtenerPorIdSeguridad(item.Documentos).First();
                    if (doc != null)
                        ListaDocumentos.Add(doc);
                }

                List<RespuestaAcuseProceso> respuesta = Ctl_Envio.ProcesarAcuses(ListaDocumentos);

               // List<RegistroDocRespuesta> respuesta = Ctl_Envio.ProcesarAcuse(ListaDocumentos);

                var datos = respuesta.Select(d => new
                {
                    NumeroDocumento = d.Numero,
                    IdSeguridad = d.IdSeguridad.ToString(),
                    FechaProceso = d.FechaProceso,
                    DocumentoTipo = DescripcionTipodDoc(d.DocumentoTipo),
                    Mensaje = d.Mensaje,                    
                    Facturador = d.Facturador,
                    Estado = (d.Error==null)?0: (d.Error.Mensaje.Contains("encolado"))?1:0,
                    MensajeFinal = d.MensajeFinal
                });

                return Ok(datos);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

    }
}
