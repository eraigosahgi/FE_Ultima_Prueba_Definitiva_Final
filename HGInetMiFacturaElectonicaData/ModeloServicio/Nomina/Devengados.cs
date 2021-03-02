using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{

	/// <summary>
	/// Utilizado para Todos los Devengos del Documento
	/// </summary>
	public class Devengados
	{

		/// <summary>
		/// Cantidad de días laborados durante el Periodo de Pago.
		/// </summary>
		public int DiasTrabajados { get; set; }

		/// <summary>
		/// Cantidad de días laborados durante el Periodo de Pago.
		/// </summary>
		public decimal SueldoTrabajado { get; set; }

		/// <summary>
		/// Lista de objeto tipo Transporte para informar Auxilio y viaticos. Ocurrencia 0-N
		/// </summary>
		public List<Transporte> DatosTransporte { get; set; }

		/// <summary>
		/// Lista de objeto tipo Hora para informar extras y recargos. Ocurrencia 0-N
		/// </summary>
		public List<Hora> DatosHoras { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadGeneral para informar vacaciones comunes(disfrutadas en tiempo) o compensadas(disfrutadas en dinero). Ocurrencia 0-N
		/// </summary>
		public List<NovedadGeneral> Vacaciones { get; set; }

		/// <summary>
		/// Reporte del pago de la Prima. Ocurrencia 0-1
		/// </summary>
		public Prima PagoPrima { get; set; }

		/// <summary>
		/// Reporte del pago de las Cesantias e intereses. Ocurrencia 0-1
		/// </summary>
		public Cesantias PagoCesantias { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadGeneral para informar Incapacidades. Ocurrencia 0-N
		/// </summary>
		public List<NovedadGeneral> Incapacidades { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadGeneral para informar Licencias maternidad, paternidad, remunerada y no remunerada. Ocurrencia 0-N
		/// </summary>
		public List<NovedadGeneral> Licencias { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadSalNoSal para informar Bonificaciones salariales y no salariales. Ocurrencia 0-N
		/// </summary>
		public List<NovedadSalNoSal> Bonificaciones { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadSalNoSal para informar Auxilios salariales y no salariales. Ocurrencia 0-N
		/// </summary>
		public List<NovedadSalNoSal> Auxilios { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadGeneral para informar HuelgaLegal, no se informa pago solo fechas y cantidad. Ocurrencia 0-N
		/// </summary>
		public List<NovedadGeneral> HuelgaLegal { get; set; }

		/// <summary>
		/// Lista de objeto tipo OtroConcepto para informar OtrosConceptos salariales y no salariales. Ocurrencia 0-N
		/// </summary>
		public List<OtroConcepto> OtrosConceptos { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadSalNoSal para informar Compensaciones salariales y no salariales. Ocurrencia 0-N
		/// </summary>
		public List<NovedadSalNoSal> Compensaciones { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadSalNoSal para informar BonoEPCTV salariales y no salariales. Ocurrencia 0-N
		/// </summary>
		public List<NovedadSalNoSal> BonoEPCTV { get; set; }

		/// <summary>
		/// Lista de objeto tipo NovedadSalNoSal para informar BonoAlimentacion salariales y no salariales. Ocurrencia 0-N
		/// </summary>
		public List<NovedadSalNoSal> BonoAlimentacion { get; set; }

		/// <summary>
		/// Lista de objeto tipo decimal para informar Comisiones. Ocurrencia 0-N
		/// </summary>
		public List<decimal> Comisiones { get; set; }

		/// <summary>
		/// Lista de objeto tipo decimal para informar PagosTerceros. Ocurrencia 0-N
		/// </summary>
		public List<decimal> PagosTerceros { get; set; }

		/// <summary>
		/// Lista de objeto tipo decimal para informar Anticipos. Ocurrencia 0-N
		/// </summary>
		public List<decimal> Anticipos { get; set; }

		/// <summary>
		/// Valor Pagado por Dotación. Ocurrencia 0-1
		/// </summary>
		public decimal Dotacion { get; set; }

		/// <summary>
		/// Valor Pagado por trabajo en Teletrabajo. Ocurrencia 0-1
		/// </summary>
		public decimal Teletrabajo { get; set; }

		/// <summary>
		/// Valor Pagado por Retiro de la empresa. Ocurrencia 0-1
		/// </summary>
		public decimal BonifRetiro { get; set; }

		/// <summary>
		/// Valor Pagado por Indemnizacion de la empresa. Ocurrencia 0-1
		/// </summary>
		public decimal Indemnizacion { get; set; }

	}
}
