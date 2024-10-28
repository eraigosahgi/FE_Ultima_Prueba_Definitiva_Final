using HGInetMiFacturaElectonicaData.ModeloServicio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    public class EstadoDocumento
    {

        #region Propiedades

        /// <summary>
        /// Id único del documento generado por la Plataforma
        /// </summary>
        public string IdDocumento { get; set; }

        /// <summary>
        /// Fecha de recepción del documento por la Plataforma
        /// </summary>
        public DateTime FechaRecepcion { get; set; }

        /// <summary>
        /// Id único de Registro del Obligado a Facturar
        /// </summary>
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// Número de Resolución asignado por la DIAN.(Aplica solo para Factura)
        /// </summary>
        public string NumeroResolucion { get; set; }

        /// <summary>
        /// Prefijo de la Factura
        /// </summary>
        public string Prefijo { get; set; }

        /// <summary>
        /// Número de Documento
        /// </summary>
        public int Documento { get; set; }

        /// <summary>
        /// Fecha del último proceso del documento realizado por la Plataforma
        /// </summary>
        public DateTime FechaUltimoProceso { get; set; }

        /// <summary>
        /// Indica el id del último proceso realizado por la Plataforma
        /// </summary>
        public int IdUltimoProceso { get; set; }

        /// <summary>
        /// Nombre del último proceso realizado por la Plataforma
        /// </summary>
        public string UltimoProceso { get; set; }

        /// <summary>
        /// Indica si el documento ha finalizado todos los procesos en la Plataforma 
        /// (0: Procesos pendientes, 1: Procesos finalizados)
        /// </summary>
        public int ProcesoFinalizado { get; set; }

        /// <summary>
        /// Mensajes de la Plataforma
        /// </summary>
        public string Observacion { get; set; }

        /// <summary>
        /// Información de la trazabilidad del documento. Objeto de tipo Auditoria 
        /// </summary>
        public List<Auditoria> Auditorias {get; set;}



        #endregion

    }
}
