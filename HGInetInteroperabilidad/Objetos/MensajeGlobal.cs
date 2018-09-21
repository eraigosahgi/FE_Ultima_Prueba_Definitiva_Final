using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    /// <summary>
    /// Objeto generico que comparte propiedades con muchos de los objetos de respuesta 
    /// del proceso de interoperabilidad. (informacion de envío y recepción)
    /// 
    /// Adicionalmente es el objeto que retorna la respuesta del acuse
    /// 
    /// GET ​ ​https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/application/{UUID}
    /// </summary>
    public class MensajeGlobal
    {
        public string mensajeGlobal { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
