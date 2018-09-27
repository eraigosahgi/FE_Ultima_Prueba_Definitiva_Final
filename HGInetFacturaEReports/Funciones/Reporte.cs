using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HGInetFacturaEReports.ReportDesigner;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace HGInetFacturaEReports
{
	/// <summary>
	/// Funciones para los reportes de Telerik
	/// </summary>
	public partial class Reporte
	{
		/// <summary>
		/// Flujo de datos para almacenamiento
		/// </summary>
		private Stream FlujoDatos;

		/// <summary>
		/// Nombre del archivo para almacenamiento
		/// </summary>
		private string NombreArchivo { get; set; }

		/// <summary>
		/// Ruta física del archivo para almacenamiento
		/// </summary>
		private string Ruta { get; set; }

		/// <summary>
		/// Ruta física de almacenamiento incluyendo el nombre del archivo
		/// </summary>
		public string RutaArchivo { get; set; }

		/// <summary>
		/// Modo de creación del archivo para almacenamiento
		/// </summary>
		private FileMode ModoArchivo { get; set; }

		/// <summary>
		/// Constructor del objeto reporte para almacenamiento
		/// </summary>
		/// <param name="nombre_archivo">nombre del archivo que se va a crear sin extensión</param>
		/// <param name="ruta_fisica">ruta física donde se almacenará el archivo</param>
		/// <param name="modo_archivo">modo de almacenamiento del archivo</param>
		public Reporte(string nombre_archivo, string ruta_fisica, FileMode modo_archivo = FileMode.Create)
		{
			this.NombreArchivo = nombre_archivo;
			this.Ruta = ruta_fisica;
			this.ModoArchivo = modo_archivo;
		}

		/// <summary>
		/// Genera y almacena el reporte en formato especificado de acuerdo con la extensión
		/// </summary>
		/// <param name="report">reporte Telerik</param>
		/// <param name="extension">extensión del reporte para exportación</param>
		/// <returns>indica si creó el formato pdf</returns>
		private bool Generar(IReportDocument report, Extensiones extension)
		{
			try
			{   // nombre del archivo necesario para renderizar
				string nombre = this.NombreArchivo;

				// tipo de formato para exportación, ejemplo: "PDF"
				// https://docs.telerik.com/reporting/configuring-rendering-extensions
				string formato = Enum.GetName(typeof(Extensiones), extension);

				ReportProcessor reportProcessor = new ReportProcessor();

				Hashtable deviceInfo = new Hashtable();
				deviceInfo["OutputFormat"] = "";

				InstanceReportSource instanceReportSource = new InstanceReportSource();
				instanceReportSource.ReportDocument = report;

				bool result = reportProcessor.RenderReport(formato, instanceReportSource, deviceInfo, Guardar, out nombre);

				// cierra el flujo de escritura del archivo
				CerrarFlujo();

				return result;
			}
			catch (Exception exception)
			{
				throw new ApplicationException("Error creando el archivo: " + this.RutaArchivo, exception);
			}
		}

		/// <summary>
		/// Almacena el reporte en formato PDF 
		/// </summary>
		/// <param name="report">Reporte</param>
		/// <param name="nit_facturador">nit del facturador del documento</param>
		private void GenerarPdf(XtraReport report, string nit_facturador)
		{
			try
			{
				// Especifica las opciones de exportación específicas de PDF.
				PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

				// Especifique las páginas que se exportarán. 
				//pdfOptions.PageRange = "1, 3-5";

				// Especifica la calidad de las imágenes exportadas. 
				pdfOptions.ConvertImagesToJpeg = false;
				pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;

				// Especifica la compatibilidad PDF/A-3b.
				pdfOptions.PdfACompatibility = PdfACompatibility.PdfA3b;

				// Especifica las opciones del documento. 
				pdfOptions.DocumentOptions.Application = "HGInet Factura Electrónica";
				pdfOptions.DocumentOptions.Author = "HGI SAS";
				pdfOptions.DocumentOptions.Producer = Environment.UserName.ToString();
				pdfOptions.DocumentOptions.Subject = "Representación Gráfica";
				pdfOptions.DocumentOptions.Title = this.NombreArchivo;

				this.RutaArchivo = Path.Combine(this.Ruta, string.Format("{0}.{1}", this.NombreArchivo, Extensiones.Pdf));

				report.ExportToPdf(this.RutaArchivo, pdfOptions);
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}


		/// <summary>
		/// Almacena físicamente el formato PDF
		/// </summary>
		/// <param name="name">nombre del archivo</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="encoding">codificación del archivo</param>
		/// <param name="mimeType">tipo de archivo</param>
		/// <returns>flujo de datos del archivo</returns>
		private Stream Guardar(string name, string extension, Encoding encoding, string mimeType)
		{
			try
			{
				this.RutaArchivo = Path.Combine(this.Ruta, string.Format("{0}.{1}", this.NombreArchivo, extension));
				this.FlujoDatos = new FileStream(this.RutaArchivo, this.ModoArchivo);
				return this.FlujoDatos;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(string.Format(@"Error creando el archivo: {0}\{1}.{1}", this.Ruta, this.NombreArchivo, extension), exception);
			}
		}


		/// <summary>
		/// Cierra el flujo de datos (Stream)
		/// </summary>
		private void CerrarFlujo()
		{
			if (this.FlujoDatos != null)
			{
				this.FlujoDatos.Close();
				this.FlujoDatos.Dispose();
			}
		}

	}
}
