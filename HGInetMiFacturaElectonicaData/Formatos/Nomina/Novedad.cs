using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Formatos
{
	public class Novedad
	{
		public string Concepto { get; set; }
		public string ConceptoDes { get; set; }
		public decimal Cantidad { get; set; }
		public decimal Porcentaje { get; set; }
		public decimal ValorBase { get; set; }
		public decimal Ded { get; set; }
		public decimal Dev { get; set; }
		public DateTime FechaIni { get; set; }
		public DateTime FechaFin { get; set; }
		public short orden { get; set; }
	}
}
