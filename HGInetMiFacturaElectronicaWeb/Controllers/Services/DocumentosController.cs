using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
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
        public IHttpActionResult Get(string codigo_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            try
            {
                Sesion.ValidarSesion();

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

                Ctl_Documento ctl_documento = new Ctl_Documento();
                List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasAdquiriente(codigo_adquiente, numero_documento, estado_recibo, fecha_inicio.Date, fecha_fin.Date);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo != "0") ? d.StrPrefijo : "", d.IntNumero),
                    d.DatFechaDocumento,
                    d.DatFechaVencDocumento,
                    d.IntVlrTotal,
                    EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
                    EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
                    MotivoRechazo = d.StrAdquirienteMvoRechazo,
                    d.StrAdquirienteMvoRechazo,
                    IdentificacionFacturador = d.TblEmpresasFacturador.StrIdentificacion,
                    NombreFacturador = d.TblEmpresasFacturador.StrRazonSocial,
                    Xml = d.StrUrlArchivoUbl,
                    Pdf = d.StrUrlArchivoPdf,
                    d.StrIdSeguridad,
                    RutaPublica = plataforma.RutaPublica,
                    RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString()))
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
        public IHttpActionResult Get(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            try
            {
                Sesion.ValidarSesion();

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

                Ctl_Documento ctl_documento = new Ctl_Documento();
                List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasObligado(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo != "0") ? d.StrPrefijo : "", d.IntNumero),
                    d.DatFechaDocumento,
                    d.DatFechaVencDocumento,
                    d.IntVlrTotal,
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
                    RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString()))
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
                bool respuesta = clase_email.NotificacionDocumento(datos.FirstOrDefault(), "", email);

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
        public IHttpActionResult Get(string codigo_facturador, string codigo_adquiriente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            try
            {
                Sesion.ValidarSesion();

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                Ctl_Documento ctl_documento = new Ctl_Documento();
                List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasObligado(codigo_facturador, numero_documento, codigo_adquiriente, "*", estado_recibo, fecha_inicio, fecha_fin).Where(x => x.IntAdquirienteRecibo != 0).ToList();

                var retorno = datos.Select(d => new
                {
                    IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
                    RazonSocial = d.TblEmpresasAdquiriente.StrRazonSocial,
                    NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
                    FechaRespuesta = d.DatAdquirienteFechaRecibo,
                    Estado = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
                    MotivoRechazo = d.StrAdquirienteMvoRechazo,
                    Xml = d.StrUrlArchivoUbl,
                    Pdf = d.StrUrlArchivoPdf,
                    d.StrIdSeguridad,
                    RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
                    MailAdquiriente = d.TblEmpresasAdquiriente.StrMail

                });

                return Ok(retorno);
            }
            catch (Exception)
            {

                throw;
            }
        }



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

                List<System.Guid> List_id_seguridad = new List<Guid>();

                foreach (var item in ListadeDocumentos)
                {
                    List_id_seguridad.Add(item.Documentos);
                }

                Ctl_Documento documento = new Ctl_Documento();

                var lista = documento.ProcesarDocumentos(List_id_seguridad);
                
                return Ok();
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
        public IHttpActionResult Get(System.Guid? IdSeguridad, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            try
            {

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

                Ctl_Documento ctl_documento = new Ctl_Documento();
                List<TblDocumentos> datos = ctl_documento.ObtenerDocumentosaProcesar(IdSeguridad, estado_recibo, fecha_inicio, fecha_fin);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    IdSeguridad = d.StrIdSeguridad,
                    //NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo != "0") ? d.StrPrefijo : "", d.IntNumero),
                    d.DatFechaIngreso,
                    //d.DatFechaVencDocumento,
                    //d.IntVlrTotal,
                    EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
                    EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
                    MotivoRechazo = d.StrAdquirienteMvoRechazo,
                    //d.StrAdquirienteMvoRechazo,
                    IdentificacionFacturador = d.TblEmpresasFacturador.StrIdentificacion,
                    Facturador = d.TblEmpresasFacturador.StrIdentificacion + " -- " + d.TblEmpresasFacturador.StrRazonSocial
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
        public IHttpActionResult Get(string codigo_facturador, int? numero_documento,  string IdSeguridad="*", string numero_resolucion="*")
        {
            try
            {
                Sesion.ValidarSesion();

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                Ctl_Documento ctl_documento = new Ctl_Documento();
                var  datos = ctl_documento.ObtenerDocumentoCliente(codigo_facturador, numero_documento, (IdSeguridad!=null)?IdSeguridad:"*", (numero_resolucion!=null)?numero_resolucion:"*");

                var retorno = datos.Select(d => new
                {
                    NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo != "0") ? d.StrPrefijo : "", d.IntNumero),
                    d.DatFechaDocumento,
                    d.DatFechaVencDocumento,
                    d.IntVlrTotal,
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
                    RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString()))
                });

                return Ok(retorno);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
