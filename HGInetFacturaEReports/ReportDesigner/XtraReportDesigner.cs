using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System.Collections.Generic;

namespace HGInetFacturaEReports.ReportDesigner
{
	public partial class XtraReportDesigner : DevExpress.XtraReports.UI.XtraReport
	{
		public XtraReportDesigner()
		{
			InitializeComponent();
		}

		private void XtraReportDesigner_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{

			XtraReport rep = (XtraReport)sender;

			Factura datos_documento = (Factura)rep.DataSource;


			if (datos_documento != null)
			{
				//Asignar los datos al reporte.
				rep.DataSource = datos_documento;

				List<FormatoCampo> campos = datos_documento.DocumentoFormato.CamposPredeterminados;

				//Recorre los campos adicionales del objeto
				foreach (FormatoCampo item in campos)
				{
					//Recorre las secciones del reporte para obtener los controles y asignarle los valores.
					foreach (Band reportBand in rep.Bands)
					{
						XRLabel control_descripcion = (XRLabel)FindBandControl(reportBand, string.Format("{0}_d", item.Ubicacion.ToLowerInvariant()));
						if (control_descripcion != null)
						{
							control_descripcion.Text = item.Descripcion;
						}

						XRLabel control_valor = (XRLabel)FindBandControl(reportBand, string.Format("{0}_v", item.Ubicacion.ToLowerInvariant()));
						if (control_valor != null)
						{
							control_valor.Text = item.Valor;
						}
					}
				}
			}
		}


		private XRControl FindBandControl(Band reportBand, string controlName)
		{
			XRControl foundControl = reportBand.FindControl(controlName, false);

			if (foundControl != null)
			{
				return foundControl;
			}

			foreach (Band subBand in reportBand.SubBands)
			{
				FindBandControl(subBand, controlName);
			}

			return null;
		}
	}
}
