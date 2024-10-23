using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{

    /// <summary>
    /// Objeto de respuesta al momento de cambiar la contraseña
    /// De esta forma vamos a recibir la respuesta del cambio de contraseña, al igual que vamos a enviar nosotros cuando
    /// otro proveedor cambie la contraseña en nuestra plataforma
    /// </summary>
    class CambioContrasenaRespuesta
    {
        public string mensajeGlobal { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
