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
		public decimal TotalDocumentos { get; set; }
		public decimal ValorTotalDocumentos { get; set; }
	}

	public class ResumenTopTransaccional
	{
		public decimal TotalTop { get; set; }
		public decimal TotalOtros { get; set; }
		public decimal Total { get; set; }
		public List<TopTransaccional> Detalles { get; set; }
	}
}
