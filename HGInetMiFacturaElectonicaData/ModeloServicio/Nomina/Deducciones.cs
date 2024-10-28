using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Deducciones
	{
		/// <summary>
		/// Reporte de la deduccion de Salud que corresponde al 4% según la ley al trabajador. Ocurrencia 1-1
		/// </summary>
		public NovedadDeduccion Salud { get; set; }

		/// <summary>
		/// Reporte de la deduccion de Pension que corresponde al 4% según la ley al trabajador. Ocurrencia 1-1
		/// </summary>
		public NovedadDeduccion Pension { get; set; }

		/// <summary>
		/// Reporte de la deduccion de Fondo de Seguridad Pensional del Documento. Ocurrencia 0-1
		/// </summary>
		public FondoSP DatosFondoSP { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadDeduccion para informar deducciones de pago a sindicatos. Ocurrencia 0-N
		/// </summary>
		public List<NovedadDeduccion> DatosSindicatos { get; set; }

		/// <summary>
		/// Lista de objeto tipo Sancion para informar deducciones de pago a Sanciones. Ocurrencia 0-N
		/// </summary>
		public List<Sancion> DatosSanciones { get; set; }

		/// <summary>
		/// Lista de objeto tipo Libranza para informar deducciones de pago a Libranzas. Ocurrencia 0-N
		/// </summary>
		public List<Libranza> DatosLibranzas { get; set; }

		/// <summary>
		/// Lista de objeto tipo decimal para informar PagosTerceros. Ocurrencia 0-N
		/// </summary>
		public List<decimal> PagosTerceros { get; set; }

		/// <summary>
		/// Lista de objeto tipo decimal para informar Anticipos. Ocurrencia 0-N
		/// </summary>
		public List<decimal> Anticipos { get; set; }

		/// <summary>
		/// Lista de objeto tipo decimal para informar Valor Pagado por Otra Deducción.. Ocurrencia 0-N
		/// </summary>
		public List<decimal> OtrasDeducciones { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente al ahorro que hace el trabajador para complementar su pensión obligatoria o cumplir metas específicas. Ocurrencia 0-1
		/// </summary>
		public decimal PensionVoluntaria { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Retención en la Fuente por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal RetencionFuente { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a ICA por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal ICA { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a AFC por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal AFC { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Cooperativas por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal Cooperativa { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Embargos Fiscales por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal EmbargoFiscal { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Planes Complementarios por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal PlanComplementarios { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Conceptos Educativos por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal Educacion { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Reintegro por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal Reintegro { get; set; }

		/// <summary>
		/// Valor Pagado correspondiente a Deuda con la Empresa por parte del trabajador. Ocurrencia 0-1
		/// </summary>
		public decimal Deuda { get; set; }

	}
}
