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
