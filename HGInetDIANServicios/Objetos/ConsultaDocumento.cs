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
        /// Valida si el documento se encuentra correctamente (true) en la DIAN
        /// </summary>
        public bool DocumentoCorrecto { get; set; }

        public int ConsultaEstado { get; set; }

        public string ConsultaEstadoDescripcion { get; set; }

        public string EstadoDian { get; set; }

        public string EstadoDianDescripcion { get; set; }

        public string Mensaje { get; set; }
    }
}
