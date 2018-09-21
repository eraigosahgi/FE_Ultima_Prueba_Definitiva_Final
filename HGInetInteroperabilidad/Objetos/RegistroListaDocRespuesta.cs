using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    /// <summary>
    /// 6.1.4.2.1. Respuesta exitosa 
    /// Objeto de respuesta del resultado del registro de los documentos
    /// este seria el objeto que va a contener la cabesera de la respuesta del registro de la lista de los documentos
    /// </summary>        
    public class RegistroListaDocRespuesta
    {
        public DateTime timeStamp { get; set; }
        public List<RegistroListaDetalleDocRespuesta> trackingIds { get; set; }
        public string mensajeGlobal { get; set; }
    }

    /// <summary>
    /// Objeto que contiene el detalle de la lista de respuesta al momento de registrar una lista de documentos en el otro proveedor 
    /// </summary>
    public class RegistroListaDetalleDocRespuesta
    {
        public string nombreDocumento { get; set; }
        public string uuid { get; set; }
        public string codigoError { get; set; }
        public string mensaje { get; set; }
    }
    
}
