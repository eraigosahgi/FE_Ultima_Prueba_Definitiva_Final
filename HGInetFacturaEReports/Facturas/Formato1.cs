namespace HGInetFacturaEReports.Facturas
{
    using Org.BouncyCastle.Utilities.Encoders;
    using HGInetMiFacturaElectonicaData.ModeloServicio;
    using LibreriaGlobalHGInet.Funciones;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for ReportDocumento.
    /// </summary>
    public partial class Formato1 : Telerik.Reporting.Report
    {
        public Formato1()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void pictureBox1_ItemDataBound(object sender, EventArgs e)
        {

        }

        private void Formato1_ItemDataBinding(object sender, EventArgs e)
        {
            try
            {
                //toma los valores que se carga de la pagina principal
                Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;

                Factura datos_factura = (Factura)report.DataSource;

                List<DocumentoDetalle> detalles_factura = datos_factura.DocumentoDetalles;

                //Convierte el valor total de la factura  en letras
                string valor_letras = Numero.EnLetras((Convert.ToDouble(datos_factura.Total)), "PESOS");
                this.TextBoxValorLetras.Value = valor_letras;

                if (datos_factura.DocumentoFormato.CamposPredeterminados != null)
                {
                    List<FormatoCampo> campos = datos_factura.DocumentoFormato.CamposPredeterminados;

                    foreach (FormatoCampo item in campos)
                    {
                        if (item.Ubicacion.ToLowerInvariant().Equals("campo1"))
                        {
                            try
                            {
                                byte[] bytes = Convert.FromBase64String(item.Valor);
                                using (MemoryStream ms = new MemoryStream(bytes))
                                {
                                    Image logo = Image.FromStream(ms);
                                    this.campo1_v.Value = logo;
                                    this.campo1_v.Visible = true;
                                }
                            }
                            catch (Exception)
                            {
                            }

                        }
                        else
                        {

                            //Obtiene el control en el reporte, teniendo en cuenta la ubicacion
                            ReportItemBase[] report_item_descripcion = this.Items.Find(string.Format("{0}_d", item.Ubicacion.ToLowerInvariant()), true);

                            if (report_item_descripcion.Length > 0)
                            {
                                HtmlTextBox campo_descripcion = report_item_descripcion[0] as HtmlTextBox;

                                if (campo_descripcion != null)
                                {
                                    campo_descripcion.Value = item.Descripcion.ToUpper();
                                }
                            }

                            //Obtiene el control en el reporte, teniendo en cuenta la ubicacion
                            ReportItemBase[] report_item_valor = this.Items.Find(string.Format("{0}_v", item.Ubicacion.ToLowerInvariant()), true);

                            if (report_item_valor.Length > 0)
                            {
                                HtmlTextBox campo_valor = report_item_valor[0] as HtmlTextBox;

                                if (campo_valor != null)
                                {
                                    campo_valor.Value = item.Valor;
                                }
                            }
                        }
                    }
                }

                //Asigna al SubReporte los detalles de la factura
                Formato1Detalles reporte = new Formato1Detalles();
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
