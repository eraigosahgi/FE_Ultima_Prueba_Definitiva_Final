using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    /// <summary>
    /// Objeto para la petición de cambio de clave a otro proveedor tecnologico
    /// </summary>
    public class CambioClave
    {
        public string NITProveedor { get; set; }
        public string ContrasenaActual { get; set; }
        public string ContrasenaNueva { get; set; }
    }
}
