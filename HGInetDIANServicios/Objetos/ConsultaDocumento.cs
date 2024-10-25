using HGInetDIANServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
    public class ConsultaDocumento
    {
        /// <summary>
        /// Indica el estado del documento en la DIAN 
        /// </summary>
        public ValidacionRespuestaDian RecepcionDocumento { get; set; }

        public int ConsultaEstado { get; set; }

        public string ConsultaEstadoDescripcion { get; set; }

        public string CodigoEstadoDian { get; set; }

        public string EstadoDianDescripcion { get; set; }
	
		public EstadoDocumentoDian Estado { get; set; }

		public string Mensaje { get; set; }
    }
}
