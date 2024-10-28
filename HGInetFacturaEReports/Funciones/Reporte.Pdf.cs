

using DevExpress.XtraReports.UI;
using Telerik.Reporting;

namespace HGInetFacturaEReports
{
	/// <summary>
	/// Funciones para la gestión de PDF
	/// </summary>
	public partial class Reporte
	{

		/// <summary>
		/// Genera y almacena el reporte en formato PDF
		/// </summary>
		/// <param name="report">reporte Telerik</param>
		/// <returns>indica si creó el formato pdf</returns>
		public bool GenerarPdf(IReportDocument report)
		{
			return this.Generar(report, Extensiones.Pdf);
		}

		public void GenerarPdfDev(XtraReport report, string nit_facturador)
		{
			this.GenerarPdf(report, nit_facturador);
		}
	}
}
