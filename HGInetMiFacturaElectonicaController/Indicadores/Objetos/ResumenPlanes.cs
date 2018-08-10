using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
    public class ResumenPlanes
    {
        public decimal TransaccionesAdquiridas { get; set; }
        public decimal TransaccionesProcesadas { get; set; }
        public decimal TransaccionesDisponibles { get; set; }
        public object PlanesAdquiridos { get; set; }

    }
}
