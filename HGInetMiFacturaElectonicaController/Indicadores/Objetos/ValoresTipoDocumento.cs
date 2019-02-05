using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
    public class ValoresTipoDocumento
    {
        public int Mes { get; set; }
        public string DescripcionMes { get; set; }
        public int Anyo { get; set; }
        public int CantidadDocumentos { get; set; }
        public decimal ValorFacturas { get; set; }
        public decimal ValorNotasCredito { get; set; }
        public decimal ValorNotasDebito { get; set; }

    }
}
