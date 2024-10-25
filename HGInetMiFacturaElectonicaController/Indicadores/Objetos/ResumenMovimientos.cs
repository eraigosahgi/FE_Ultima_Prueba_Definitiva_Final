using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
    public class ResumenMovimientos
    {
        public decimal NumeroDocumentos { get; set; }

        public decimal CantidadAdquiriente { get; set; }

        public decimal CantidadObligado { get; set; }

        public decimal total { get; set; }
    }
}
