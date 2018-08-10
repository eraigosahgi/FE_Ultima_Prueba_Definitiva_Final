using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
    public class TopTransaccional
    {
        public string Identificacion { get; set; }
        public decimal CantidadMesAnterior { get; set; }
        public decimal CantidadMesActual { get; set; }
        public string RazonSocial { get; set; }

    }
}
