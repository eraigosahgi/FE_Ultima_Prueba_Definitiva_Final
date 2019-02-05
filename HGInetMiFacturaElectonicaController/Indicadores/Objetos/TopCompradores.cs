using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
	public class TopCompradores
	{
		public int Posicion { get; set; }
		public string Identificacion { get; set; }
		public string RazonSocial { get; set; }
		public decimal CantidadTransacciones { get; set; }
		public decimal ValorCompras { get; set; }
	}

	public class ResumenCompradores
	{
		public decimal TotalTop { get; set; }
		public decimal TotalOtros { get; set; }
		public decimal Total { get; set; }
		public List<TopCompradores> Detalles { get; set; }
	}

}
