namespace HGInetFacturaEReports.Facturas
{
    partial class Formato1Detalles
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.htmlTextBox18 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox31 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox17 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox6 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox5 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox4 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox3 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox8 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox1 = new Telerik.Reporting.HtmlTextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.57010006904602051D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.htmlTextBox18,
            this.htmlTextBox31,
            this.htmlTextBox17,
            this.htmlTextBox8,
            this.htmlTextBox1});
            this.detail.Name = "detail";
            // 
            // htmlTextBox18
            // 
            this.htmlTextBox18.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox18.CanShrink = false;
            this.htmlTextBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0597743988037109D), Telerik.Reporting.Drawing.Unit.Cm(2.7991390538772976E-07D));
            this.htmlTextBox18.Name = "htmlTextBox18";
            this.htmlTextBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.6005268096923828D), Telerik.Reporting.Drawing.Unit.Cm(0.56999999284744263D));
            this.htmlTextBox18.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.htmlTextBox18.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox18.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox18.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox18.Value = "{Fields.ProductoNombre}";
            // 
            // htmlTextBox31
            // 
            this.htmlTextBox31.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox31.CanShrink = false;
            this.htmlTextBox31.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.660500526428223D), Telerik.Reporting.Drawing.Unit.Cm(2.7991390538772976E-07D));
            this.htmlTextBox31.Name = "htmlTextBox31";
            this.htmlTextBox31.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.5002003908157349D), Telerik.Reporting.Drawing.Unit.Cm(0.56999999284744263D));
            this.htmlTextBox31.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.htmlTextBox31.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox31.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox31.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox31.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox31.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox31.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox31.Value = "{Fields.Cantidad.ToString().Replace(\",\" , \".\")}";
            // 
            // htmlTextBox17
            // 
            this.htmlTextBox17.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox17.CanShrink = false;
            this.htmlTextBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.7991390538772976E-07D));
            this.htmlTextBox17.Name = "htmlTextBox17";
            this.htmlTextBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0596733093261719D), Telerik.Reporting.Drawing.Unit.Cm(0.56999999284744263D));
            this.htmlTextBox17.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.htmlTextBox17.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox17.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox17.Style.Font.Strikeout = false;
            this.htmlTextBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox17.Value = "{Fields.ProductoCodigo}";
            // 
            // htmlTextBox6
            // 
            this.htmlTextBox6.Name = "htmlTextBox6";
            this.htmlTextBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox6.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox6.Style.Font.Bold = true;
            this.htmlTextBox6.Style.Font.Italic = false;
            this.htmlTextBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // htmlTextBox5
            // 
            this.htmlTextBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.72229194641113281D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.htmlTextBox5.Name = "htmlTextBox15";
            this.htmlTextBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.6601979732513428D), Telerik.Reporting.Drawing.Unit.Cm(0.56999999284744263D));
            this.htmlTextBox5.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox5.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox5.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox5.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox5.Style.Font.Bold = true;
            this.htmlTextBox5.Style.Font.Italic = false;
            this.htmlTextBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox5.Value = "CÓDIGO";
            // 
            // htmlTextBox4
            // 
            this.htmlTextBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.877498626708984D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.htmlTextBox4.Name = "htmlTextBox25";
            this.htmlTextBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.199800968170166D), Telerik.Reporting.Drawing.Unit.Cm(0.56999999284744263D));
            this.htmlTextBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox4.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox4.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox4.Style.Font.Bold = true;
            this.htmlTextBox4.Style.Font.Italic = false;
            this.htmlTextBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox4.Value = "V.UNITARIO";
            // 
            // htmlTextBox3
            // 
            this.htmlTextBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.4000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.htmlTextBox3.Name = "htmlTextBox20";
            this.htmlTextBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9D), Telerik.Reporting.Drawing.Unit.Cm(0.56999999284744263D));
            this.htmlTextBox3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox3.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox3.Style.Font.Bold = true;
            this.htmlTextBox3.Style.Font.Italic = false;
            this.htmlTextBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.htmlTextBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox3.Value = "DESCRIPCIÓN";
            // 
            // htmlTextBox8
            // 
            this.htmlTextBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.160900115966797D), Telerik.Reporting.Drawing.Unit.Cm(0.00010002215276472271D));
            this.htmlTextBox8.Name = "htmlTextBox8";
            this.htmlTextBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1997010707855225D), Telerik.Reporting.Drawing.Unit.Cm(0.56999409198760986D));
            this.htmlTextBox8.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.htmlTextBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox8.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.htmlTextBox8.Style.Font.Bold = false;
            this.htmlTextBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox8.Value = "=FORMAT(\'{0:###,##0.}\',Fields.ValorUnitario)";
            // 
            // htmlTextBox1
            // 
            this.htmlTextBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.360801696777344D), Telerik.Reporting.Drawing.Unit.Cm(9.9988508736714721E-05D));
            this.htmlTextBox1.Name = "htmlTextBox1";
            this.htmlTextBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2391958236694336D), Telerik.Reporting.Drawing.Unit.Cm(0.56999409198760986D));
            this.htmlTextBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Dotted;
            this.htmlTextBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox1.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.htmlTextBox1.Style.Font.Bold = false;
            this.htmlTextBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.htmlTextBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox1.Value = "=FORMAT(\'{0:###,##0.}\',Fields.ValorSubtotal)";
            // 
            // Formato1Detalles
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Formato1Detalles";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(12D), Telerik.Reporting.Drawing.Unit.Mm(12D), Telerik.Reporting.Drawing.Unit.Mm(12D), Telerik.Reporting.Drawing.Unit.Mm(12D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(18.599998474121094D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.HtmlTextBox htmlTextBox18;
        private Telerik.Reporting.HtmlTextBox htmlTextBox31;
        private Telerik.Reporting.HtmlTextBox htmlTextBox17;
        private Telerik.Reporting.HtmlTextBox htmlTextBox6;
        private Telerik.Reporting.HtmlTextBox htmlTextBox5;
        private Telerik.Reporting.HtmlTextBox htmlTextBox4;
        private Telerik.Reporting.HtmlTextBox htmlTextBox3;
        private Telerik.Reporting.HtmlTextBox htmlTextBox8;
        private Telerik.Reporting.HtmlTextBox htmlTextBox1;
    }
}