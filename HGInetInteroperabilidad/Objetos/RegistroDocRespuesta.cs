using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    ///6.2. Consultar estado de procesamiento 
    //GET ​ ​https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/consultar/{UUID} 
    // Parametros UUID 
    //201 El documento electrónico existe en el sistema se procede a dar el estado de procesamiento
    //409 El documento electrónico asociado al UUID consultado no existe en el sistema
    //406 El documento electrónico asociado al UUID consultado existe en el sistema pero fue registrado por un proveedor de factura diferente  
    //500 Error interno del receptor del documento electrónico  
    //400 Usuario no autenticado

    /// <summary>
    /// Objeto que gestiona la cabesera de la respuesta cuando se consulta un documento especificamente por UUID
    /// </summary>
    public class RegistroDocRespuesta
    {
        public List<DetalleDocRespuesta> historial { get; set; }
        public string mensajeGlobal { get; set; }
        public string timeStamp { get; set; }
    }

    /// Objeto que contiene el detalle de la respuesta de la consulta de un documento en especifico por UUID        
    public class DetalleDocRespuesta
    {
        public string nombre { get; set; }
        public string timeStamp { get; set; }
        public string mensaje { get; set; }
    }
}
