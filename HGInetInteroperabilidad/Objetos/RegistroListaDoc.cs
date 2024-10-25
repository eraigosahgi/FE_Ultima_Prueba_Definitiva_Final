using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
   
    /// <summary>
    /// objeto para el registro de los documentos (Recepción y Envio)
    /// </summary>
    public class RegistroListaDoc
    {
        public string nombre { get; set; }
        public string uuid { get; set; }
        public Extensiones extensiones { get; set; }
        public List<Documentos> documentos { get; set; }
    }
}
