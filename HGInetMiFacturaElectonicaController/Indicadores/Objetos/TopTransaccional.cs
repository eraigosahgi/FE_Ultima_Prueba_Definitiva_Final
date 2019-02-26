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
		public decimal CantidadAnterior { get; set; }
		public decimal CantidadActual { get; set; }
		public string RazonSocial { get; set; }
		public decimal TotalDocumentos { get; set; }
		public decimal ValorTotalDocumentos { get; set; }
	}

}
