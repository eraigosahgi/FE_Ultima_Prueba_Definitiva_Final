using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
    public class VentasMensuales
    {
        public int Anyo { get; set; }
        public int Mes { get; set; }
        public string DescripcionMes { get; set; }
        public byte TipoProceso { get; set; }
        public decimal CantidadTransaccionesCortesias { get; set; }
        public decimal CantidadTransaccionesPostVenta { get; set; }
        public decimal CantidadTransaccionesVentas { get; set; }
        public decimal ValorVentas { get; set; }

    }
}
