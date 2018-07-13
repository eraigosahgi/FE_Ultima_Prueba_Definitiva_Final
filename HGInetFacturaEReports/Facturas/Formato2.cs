namespace HGInetFacturaEReports.Facturas
{
    using HGInetMiFacturaElectonicaData.ModeloServicio;
    using LibreriaGlobalHGInet.Funciones;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for Formato2.
    /// </summary>
    public partial class Formato2 : Telerik.Reporting.Report
    {
        public Formato2()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void Formato2_ItemDataBinding(object sender, EventArgs e)
        {
            try
            {
                //toma los valores que se carga de la pagina principal
                Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;

                Factura datos_factura = (Factura)report.DataSource;

                List<DocumentoDetalle> detalles_factura = datos_factura.DocumentoDetalles;

                string tipo_moneda = LibreriaGlobalHGInet.Formato.ConfiguracionRegional.TipoMoneda(datos_factura.Moneda);

                //Convierte el valor total de la factura  en letras
                string valor_letras = Numero.EnLetras((Convert.ToDouble(datos_factura.Total)), tipo_moneda.ToUpper());

                if (datos_factura.DocumentoFormato.CamposPredeterminados != null)
                {
                    List<FormatoCampo> campos = datos_factura.DocumentoFormato.CamposPredeterminados;

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
                                    this.campo1_v.Visible = true;
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
                                HtmlTextBox campo_descripcion = report_item_descripcion[0] as HtmlTextBox;

                                if (campo_descripcion != null)
                                {
                                    campo_descripcion.Value = item.Descripcion.ToUpper();
                                    campo_descripcion.Visible = true;
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
                                    campo_valor.Visible = true;
                                }
                            }
                        }
                    }
                }

                //Asigna al SubReporte los detalles de la factura
                Formato2Detalles reporte = new Formato2Detalles();
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