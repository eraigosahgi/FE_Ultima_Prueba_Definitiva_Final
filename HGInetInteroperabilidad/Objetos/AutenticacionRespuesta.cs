using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    /// <summary>
    /// Objeto para el manejo de la respuesta de autenticación, por parte del proveedor receptor
    /// </summary>
    public class AutenticacionRespuesta
    {
        public string jwtToken { get; set; }
        public string passwordExpiration { get; set; }
    }
}
