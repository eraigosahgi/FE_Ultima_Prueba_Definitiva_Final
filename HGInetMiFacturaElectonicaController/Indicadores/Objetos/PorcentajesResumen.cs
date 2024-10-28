using HGInetMiFacturaElectonicaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
    public class PorcentajesResumen
    {
        public string IdControl { get; set; }
        public int Estado { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Cantidad { get; set; }
        public string Titulo { get; set; }
        public string Observaciones { get; set; }
        public string Color { get; set; }
        public string Icono { get; set; }

    }
}
