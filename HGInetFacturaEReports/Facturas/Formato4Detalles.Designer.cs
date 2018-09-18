namespace HGInetFacturaEReports.Facturas
{
    partial class Formato4Detalles
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.htmlTextBox25 = new Telerik.Reporting.TextBox();
            this.htmlTextBox24 = new Telerik.Reporting.TextBox();
            this.htmlTextBox26 = new Telerik.Reporting.TextBox();
            this.htmlTextBox1 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            formattingRule1.Filters.Add(new Telerik.Reporting.Filter("=Fields.OcultarItem", Telerik.Reporting.FilterOperator.Equal, "1"));
            formattingRule1.Style.Visible = false;
            this.detail.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] {
            formattingRule1});
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.48000001907348633D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.htmlTextBox25,
            this.htmlTextBox24,
            this.htmlTextBox26,
            this.htmlTextBox1});
            this.detail.Name = "detail";
            // 
            // htmlTextBox25
            // 
            this.htmlTextBox25.CanGrow = false;
            this.htmlTextBox25.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.8001999855041504D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.htmlTextBox25.Name = "htmlTextBox25";
            this.htmlTextBox25.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.059800148010254D), Telerik.Reporting.Drawing.Unit.Cm(0.47990003228187561D));
            this.htmlTextBox25.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox25.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox25.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox25.Style.Font.Bold = true;
            this.htmlTextBox25.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.htmlTextBox25.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox25.Value = "=Fields.ProductoNombre";
            // 
            // htmlTextBox24
            // 
            this.htmlTextBox24.CanGrow = false;
            this.htmlTextBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.860199928283691D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.htmlTextBox24.Name = "htmlTextBox24";
            this.htmlTextBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9180757999420166D), Telerik.Reporting.Drawing.Unit.Cm(0.47990009188652039D));
            this.htmlTextBox24.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox24.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox24.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox24.Style.Font.Bold = true;
            this.htmlTextBox24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.htmlTextBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox24.Value = "=FORMAT(\'{0:###,##0.}\',Fields.ValorUnitario)";
            // 
            // htmlTextBox26
            // 
            this.htmlTextBox26.CanGrow = false;
            this.htmlTextBox26.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.778375625610352D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.htmlTextBox26.Name = "htmlTextBox26";
            this.htmlTextBox26.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9245450496673584D), Telerik.Reporting.Drawing.Unit.Cm(0.48000001907348633D));
            this.htmlTextBox26.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox26.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox26.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox26.Style.Font.Bold = true;
            this.htmlTextBox26.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.htmlTextBox26.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox26.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox26.Value = "=FORMAT(\'{0:###,##0.}\',Fields.ValorSubtotal)";
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.CanGrow = false;
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010002215276472271D));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.47990003228187561D));
            this.htmlTextBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox1.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox1.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox1.Style.Font.Bold = true;
            this.htmlTextBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.htmlTextBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox1.Value = "=Fields.ProductoCodigo";
            // 
            // Formato4Detalles
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Formato4Detalles";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(8D), Telerik.Reporting.Drawing.Unit.Mm(2D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(2D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(19.703022003173828D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox htmlTextBox25;
        private Telerik.Reporting.TextBox htmlTextBox26;
        private Telerik.Reporting.TextBox htmlTextBox24;
        private Telerik.Reporting.TextBox htmlTextBox1;
    }
}