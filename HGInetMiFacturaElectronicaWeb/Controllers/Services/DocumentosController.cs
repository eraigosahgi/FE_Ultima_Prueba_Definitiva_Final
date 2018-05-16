using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
            PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

            Ctl_Documento ctl_documento = new Ctl_Documento();
            List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasAdquiriente(codigo_adquiente, numero_documento, estado_recibo, fecha_inicio.Date, fecha_fin.Date);

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
                NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
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
            PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

            Ctl_Documento ctl_documento = new Ctl_Documento();
            List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasObligado(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin);

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
                NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
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

        /// <summary>
        /// Obtiene el documento por ID de seguridad.
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(System.Guid id_seguridad)
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


        public IHttpActionResult Get(System.Guid id_seguridad, string email)
        {
            try
            {
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
        /// Actualiza la respuesta de acuse del documento.
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <param name="estado"></param>
        /// <param name="motivo_rechazo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri]short estado, [FromUri]string motivo_rechazo)
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
    }
}
