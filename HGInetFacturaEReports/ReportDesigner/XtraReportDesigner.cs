using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System.Collections.Generic;
using System.IO;

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
			
			dynamic datos_documento = rep.DataSource;

			bool continua_proceso = false;

			try
			{
				// valida la existencia del DataSource
				datos_documento.DataSetName = "Validador";
			}
			catch (Exception)
			{
				continua_proceso = true;
			}

			if (datos_documento != null && continua_proceso)
			{
				//Asignar los datos al reporte.
				rep.DataSource = datos_documento;
				
				List<FormatoCampo> campos = datos_documento.DocumentoFormato.CamposPredeterminados;
				bool lleno_titulo = false;

				if (campos != null)
				{
					//Recorre los campos adicionales del objeto
					foreach (FormatoCampo item in campos)
					{
						//Recorre las secciones del reporte para obtener los controles y asignarle los valores.
						foreach (Band reportBand in rep.Bands)
						{
							XRLabel control_titulo = (XRLabel)FindBandControl(reportBand, "LblTituloReporte");
							if (control_titulo != null && !lleno_titulo)
							{
								lleno_titulo = true;
								string titulo_formato = string.Empty;
								titulo_formato = datos_documento.DocumentoFormato.Titulo;

								control_titulo.Text = titulo_formato;
							}

							try
							{
								//Obtiene los controles de tipo picture y carga las imagenes 
								XRPictureBox control_imagen = (XRPictureBox)FindBandControl(reportBand,
									string.Format("{0}_v", item.Ubicacion.ToLowerInvariant()));
								if (control_imagen != null)
								{
									MemoryStream ms = null;
									try
									{
										byte[] bytes = Convert.FromBase64String(item.Valor);
										using (ms = new MemoryStream(bytes))
										{
											Image logo = Image.FromStream(ms);
											control_imagen.Image = logo;
											control_imagen.Visible = true;
										}

										break;
									}
									catch (Exception excepcion)
									{
										if (ms != null)
											ms.Close();
									}
								}
							}
							catch (Exception)
							{
							}

							//Obtiene los campos de descripciones
							XRLabel control_descripcion = (XRLabel)FindBandControl(reportBand,
								string.Format("{0}_d", item.Ubicacion.ToLowerInvariant()));
							if (control_descripcion != null)
							{
								control_descripcion.Text = item.Descripcion.Replace("<br />", "\n")
									.Replace("<br/>", "\n").Replace("<br>", "\n").Replace("</br>", "\n").ToUpper();
								;
							}

							//Obtiene los campos de valores.
							XRLabel control_valor = (XRLabel)FindBandControl(reportBand,
								string.Format("{0}_v", item.Ubicacion.ToLowerInvariant()));
							if (control_valor != null)
							{
								control_valor.Text = item.Valor.Replace("<br />", "\n").Replace("<br/>", "\n")
									.Replace("<br>", "\n").Replace("</br>", "\n");
								;
							}
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
