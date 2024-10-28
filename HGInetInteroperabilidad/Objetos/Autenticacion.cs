using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    //POST ​ https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/login 
    /// <summary>
    /// Objeto para el manejo de autenticación por parte del proveedor emisor
    /// </summary>
    public class Autenticacion
    {
        public string u { get; set; }
        public string p { get; set; }
    }
}
