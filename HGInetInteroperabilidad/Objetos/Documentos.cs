using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    public class Documentos
    {
        public string nombre { get; set; }
        public string sha256 { get; set; }
        public string tipo { get; set; }
        public string notaDeEntrega { get; set; }
        public bool adjuntos { get; set; }
        public bool representacionGraficas { get; set; }
        public string identificacionDestinatario { get; set; }
        public Extensiones extensiones { get; set; }
    }
}
