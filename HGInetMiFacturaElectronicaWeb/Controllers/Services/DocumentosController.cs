using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class DocumentosController : ApiController
    {

        public IHttpActionResult Get(string codigo_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            Ctl_Documento ctl_documento = new Ctl_Documento();

            List<TblDocumentos> datos = ctl_documento.ObtenerPorFechasAdquiriente(codigo_adquiente, numero_documento, estado_recibo, fecha_inicio.Date, fecha_fin.Date);

            var retorno = datos.Select(d => new
            {
                NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
                d.DatFechaDocumento,
                d.DatFechaVencDocumento,
                d.IntVlrTotal,
                EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
                EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
                MotivoRechazo = (d.StrAdquirienteMvoRechazo != null) ? d.StrAdquirienteMvoRechazo : "N/A",
                d.StrAdquirienteMvoRechazo,
                IdentificacionFacturador = d.TblEmpresas.StrIdentificacion,
                NombreFacturador = d.TblEmpresas.StrRazonSocial,
                d.StrUrlArchivoPdf,
                d.StrUrlArchivoUbl,
                d.StrIdSeguridad
            });

            if (datos == null)
            {
                return NotFound();
            }

            return Ok(retorno);
        }


        public IHttpActionResult Get(System.Guid id_seguridad)
        {
            Ctl_Documento ctl_documento = new Ctl_Documento();

            List<TblDocumentos> datos = ctl_documento.ObtenerPorIdSeguridad(id_seguridad);

            var retorno = datos.Select(d => new
            {
                NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
                IdAdquiriente = d.TblEmpresas1.StrIdentificacion,
                NombreAdquiriente = d.TblEmpresas1.StrRazonSocial,
                Cufe = d.StrCufe,
                IdSeguridad = d.StrIdSeguridad,
                EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
                MotivoRechazo = (d.StrAdquirienteMvoRechazo != null) ? d.StrAdquirienteMvoRechazo : "N/A",
                FechaRespuesta = d.DatAdquirienteFechaRecibo,
                Xml = d.StrUrlArchivoUbl,
                Pdf = d.StrUrlArchivoPdf,
                RespuestaVisible = (d.IntAdquirienteRecibo == 1 || d.IntAdquirienteRecibo == 2) ? true : false,
                CamposVisibles = (d.IntAdquirienteRecibo == 0) ? true : false
            });

            if (datos == null)
            {
                return NotFound();
            }

            return Ok(retorno);
        }

        /// <summary>
        /// Actualiza la respuesta de acuse del documento.
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <param name="estado"></param>
        /// <param name="motivo_rechazo"></param>
        /// <returns></returns>
        public IHttpActionResult Put(System.Guid id_seguridad, short estado, string motivo_rechazo)
        {
            Ctl_Documento ctl_documento = new Ctl_Documento();

            List<TblDocumentos> datos = ctl_documento.ActualizarRespuestaAcuse(id_seguridad, estado, motivo_rechazo);

            var retorno = datos.Select(d => new
            {
                NumeroDocumento = string.Format("{0}{1}", d.StrPrefijo, d.IntNumero),
                IdAdquiriente = d.TblEmpresas1.StrIdentificacion,
                NombreAdquiriente = d.TblEmpresas1.StrRazonSocial,
                Cufe = d.StrCufe,
                IdSeguridad = d.StrIdSeguridad,
                EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
                MotivoRechazo = (d.StrAdquirienteMvoRechazo != null) ? d.StrAdquirienteMvoRechazo : "N/A",
                Xml = d.StrUrlArchivoUbl,
                Pdf = d.StrUrlArchivoPdf,
                RespuestaVisible = (d.IntAdquirienteRecibo == 1 || d.IntAdquirienteRecibo == 2) ? true : false,
                CamposVisibles = (d.IntAdquirienteRecibo == 0) ? true : false
            });

            if (datos == null)
            {
                return NotFound();
            }

            return Ok(retorno);
        }


        /// <summary>
        /// Retorna la descripción del estado de la factura.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string DescripcionEstadoFactura(short e)
        {
            return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>(e));
        }

        /// <summary>
        /// Retorna la descripción del estado del acuse.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string DescripcionEstadoAcuse(short e)
        {
            return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>(e));
        }
    }
}
