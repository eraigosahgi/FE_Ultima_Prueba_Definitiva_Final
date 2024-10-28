namespace HGInetFacturaEReports.Facturas
{
	using HGInetMiFacturaElectonicaData.ModeloServicio;
	using LibreriaGlobalHGInet.Funciones;
	using LibreriaGlobalHGInet.Objetos;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;
	using Telerik.Reporting;
	using Telerik.Reporting.Drawing;

	/// <summary>
	/// Summary description for Formato4.
	/// </summary>
	public partial class Formato4 : Telerik.Reporting.Report
	{
		public Formato4()
		{
			//
			// Required for telerik Reporting designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void Formato4_ItemDataBinding(object sender, EventArgs e)
		{
			try
			{
				//toma los valores que se carga de la pagina principal
				Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;

				dynamic datos_documento = (dynamic)null;
				datos_documento = report.DataSource;

				//Obtiene el dato del parametro TipoDocumento para obtener el título del documento.
				int tipo = Convert.ToInt32(report.Parameters["TipoDocumento"].Value);

				TipoDocumento tipo_doc = Enumeracion.GetEnumObjectByValue<TipoDocumento>(tipo);

				TextBoxTituloDocumento.Value = Enumeracion.GetDescription(tipo_doc).ToUpper();

				switch (tipo_doc)
				{
					case TipoDocumento.Factura:
						TextBoxTituloVencimiento.Value = "Vence";
						TextBoxValorVencimiento.Value = Convert.ToDateTime(datos_documento.FechaVence).ToString("dd MM yyyy");
						break;
					case TipoDocumento.NotaCredito:
						TextBoxTituloVencimiento.Value = "Fecha Factura";
						TextBoxValorVencimiento.Value = Convert.ToDateTime(datos_documento.FechaFactura).ToString("dd MM yyyy");
						break;
					case TipoDocumento.NotaDebito:
						TextBoxTituloVencimiento.Value = "Fecha Factura";
						TextBoxValorVencimiento.Value = Convert.ToDateTime(datos_documento.FechaFactura).ToString("dd MM yyyy");
						break;
				}

				string cod_qr =

				   cod_qr = string.Format("NumFac:{0} {1}\r\nFecFac:{2}\r\nNitFac:{3}\r\nDocAdq:{4}\r\n", datos_documento.Prefijo, datos_documento.Documento, Convert.ToDateTime(datos_documento.Fecha).ToString("yyyyMMddHHm"), datos_documento.DatosObligado.Identificacion, datos_documento.DatosAdquiriente.Identificacion);
				cod_qr = cod_qr + string.Format("ValFac:{0}\r\nValIva:{1}\r\nValOtroImp:{2}\r\n", datos_documento.ValorSubtotal.ToString().Replace(",", "."), datos_documento.ValorIva.ToString().Replace(",", "."), datos_documento.ValorImpuestoConsumo.ToString().Replace(",", "."));
				cod_qr = cod_qr + string.Format("ValFacIm:{0}\r\nCUFE:{1}", datos_documento.Total.ToString().Replace(",", "."), datos_documento.Cufe);
				CodigoQR.Value = cod_qr;

				List<DocumentoDetalle> detalles_factura = datos_documento.DocumentoDetalles;


				if (datos_documento.DocumentoFormato.CamposPredeterminados != null)
				{
					List<FormatoCampo> campos = datos_documento.DocumentoFormato.CamposPredeterminados;

					foreach (FormatoCampo item in campos)
					{
						if (item.Ubicacion.ToLowerInvariant().Equals("campo1"))
						{
							MemoryStream ms = null;

							try
							{
								byte[] bytes = Convert.FromBase64String(item.Valor);
								using (ms = new MemoryStream(bytes))
								{
									Image logo = Image.FromStream(ms);
									this.campo1_v.Value = logo;
								}
							}
							catch (Exception excepcion)
							{
								if (ms != null)
									ms.Close();
							}
						}
						else if (item.Ubicacion.ToLowerInvariant().Equals("campo3"))
						{
							MemoryStream ms = null;

							try
							{
								byte[] bytes = Convert.FromBase64String(item.Valor);
								using (ms = new MemoryStream(bytes))
								{
									Image logo = Image.FromStream(ms);
									this.campo3_v.Value = logo;
								}
							}
							catch (Exception excepcion)
							{
								if (ms != null)
									ms.Close();
							}
						}
						else
						{
							//Obtiene el control en el reporte, teniendo en cuenta la ubicacion
							ReportItemBase[] report_item_descripcion = this.Items.Find(string.Format("{0}_d", item.Ubicacion.ToLowerInvariant()), true);

							if (report_item_descripcion.Length > 0)
							{
								Telerik.Reporting.TextBox campo_descripcion = report_item_descripcion[0] as Telerik.Reporting.TextBox;

								if (campo_descripcion != null)
								{
									campo_descripcion.Value = item.Descripcion.Replace("<br />", "\n").Replace("<br/>", "\n").Replace("<br>", "\n").Replace("</br>", "\n").ToUpper();
									campo_descripcion.Visible = true;
								}
							}

							//Obtiene el control en el reporte, teniendo en cuenta la ubicacion
							ReportItemBase[] report_item_valor = this.Items.Find(string.Format("{0}_v", item.Ubicacion.ToLowerInvariant()), true);

							if (report_item_valor.Length > 0)
							{
								Telerik.Reporting.TextBox campo_valor = report_item_valor[0] as Telerik.Reporting.TextBox;

								if (campo_valor != null)
								{
									campo_valor.Value = item.Valor.Replace("<br />", "\n").Replace("<br/>", "\n").Replace("<br>", "\n").Replace("</br>", "\n");
									campo_valor.Visible = true;
								}
							}
						}
					}
				}

				//Asigna al SubReporte los detalles de la factura
				Formato4Detalles reporte = new Formato4Detalles();
				reporte.DataSource = detalles_factura;

				subReportDetalles.ReportSource = reporte;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}