using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    public class Resolucion
    {
        #region Propiedades

        /// <summary>
        /// Número de Resolución asignado por la DIAN.
        /// </summary>
        public string NumeroResolucion { get; set; }

        /// <summary>
        /// Fecha de la Resolución
        /// </summary>
        public DateTime FechaResolucion { get; set; }

        /// <summary>
        /// Prefijo de la Factura
        /// </summary>
        public string Prefijo { get; set; }

        /// <summary>
        /// Número inicial del rango
        /// </summary>
        public int RangoInicial { get; set; }

        /// <summary>
        /// Número final del rango
        /// </summary>
        public int RangoFinal { get; set; }

        /// <summary>
        /// Fecha inicial de la vigencia de la Resolución
        /// </summary>
        public DateTime FechaVigenciaInicial { get; set; }

        /// <summary>
        /// Fecha final de la vigencia de la Resolución
        /// </summary>
        public DateTime FechaVigenciaFinal { get; set; }

        /// <summary>
        /// Clave técnica de la Resolución asignada por la DIAN
        /// </summary>
        public string ClaveTecnica { get; set; }

        #endregion

    }
}
