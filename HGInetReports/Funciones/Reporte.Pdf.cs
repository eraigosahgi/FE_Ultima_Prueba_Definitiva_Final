

using Telerik.Reporting;

namespace HGInetReports
{
	/// <summary>
	/// Funciones para la gestión de PDF
	/// </summary>
	partial class Reporte
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
	}
}
