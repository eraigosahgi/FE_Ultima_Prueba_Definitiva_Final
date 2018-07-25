namespace HGInetFacturaEReports.Facturas
{
    partial class Formato2Detalles
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
            this.htmlTextBox29 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox39 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox38 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox36 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox37 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox34 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox33 = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox32 = new Telerik.Reporting.HtmlTextBox();
            this.campo29_v = new Telerik.Reporting.HtmlTextBox();
            this.htmlTextBox30 = new Telerik.Reporting.HtmlTextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.50010019540786743D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.htmlTextBox29,
            this.htmlTextBox39,
            this.htmlTextBox38,
            this.htmlTextBox36,
            this.htmlTextBox37,
            this.htmlTextBox34,
            this.htmlTextBox33,
            this.htmlTextBox32,
            this.campo29_v,
            this.htmlTextBox30});
            this.detail.Name = "detail";
            // 
            // htmlTextBox29
            // 
            this.htmlTextBox29.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(4.7298024696829088E-07D));
            this.htmlTextBox29.Name = "htmlTextBox29";
            this.htmlTextBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1000001430511475D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox29.Style.Font.Bold = false;
            this.htmlTextBox29.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox29.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox29.Value = "{Fields.ProductoCodigo}";
            // 
            // htmlTextBox39
            // 
            this.htmlTextBox39.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox39.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.399393081665039D), Telerik.Reporting.Drawing.Unit.Cm(2.514932759822841E-07D));
            this.htmlTextBox39.Name = "htmlTextBox39";
            this.htmlTextBox39.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8923429250717163D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox39.Style.Font.Bold = false;
            this.htmlTextBox39.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox39.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox39.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox39.Value = "=FORMAT(\'{0:###,##0.}\', SUM(Fields.ValorSubtotal + Fields.IvaValor - Fields.ReteF" +
    "uenteValor + Fields.ValorImpuestoConsumo))";
            // 
            // htmlTextBox38
            // 
            this.htmlTextBox38.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox38.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.199193954467773D), Telerik.Reporting.Drawing.Unit.Cm(2.514932759822841E-07D));
            this.htmlTextBox38.Name = "htmlTextBox38";
            this.htmlTextBox38.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox38.Style.Font.Bold = false;
            this.htmlTextBox38.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox38.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox38.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox38.Value = "{Fields.ReteFuentePorcentaje.ToString().Replace(\",\" , \".\")}";
            // 
            // htmlTextBox36
            // 
            this.htmlTextBox36.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox36.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.998993873596191D), Telerik.Reporting.Drawing.Unit.Cm(1.0649084458691505E-07D));
            this.htmlTextBox36.Name = "htmlTextBox36";
            this.htmlTextBox36.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox36.Style.Font.Bold = false;
            this.htmlTextBox36.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox36.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox36.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox36.Value = "{Fields.IvaPorcentaje.ToString().Replace(\",\" , \".\")}";
            // 
            // htmlTextBox37
            // 
            this.htmlTextBox37.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox37.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.09898853302002D), Telerik.Reporting.Drawing.Unit.Cm(3.7746822556528059E-08D));
            this.htmlTextBox37.Name = "htmlTextBox37";
            this.htmlTextBox37.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8998047113418579D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox37.Style.Font.Bold = false;
            this.htmlTextBox37.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox37.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox37.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox37.Value = "=FORMAT(\'{0:###,##0.}\', Fields.ValorSubtotal)";
            // 
            // htmlTextBox34
            // 
            this.htmlTextBox34.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox34.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.898785591125488D), Telerik.Reporting.Drawing.Unit.Cm(3.3643509311787057E-08D));
            this.htmlTextBox34.Name = "htmlTextBox34";
            this.htmlTextBox34.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox34.Style.Font.Bold = false;
            this.htmlTextBox34.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox34.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox34.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox34.Value = "{Fields.DescuentoPorcentaje.ToString().Replace(\",\" , \".\")}";
            // 
            // htmlTextBox33
            // 
            this.htmlTextBox33.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox33.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.998781204223633D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.htmlTextBox33.Name = "htmlTextBox33";
            this.htmlTextBox33.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8998047113418579D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox33.Style.Font.Bold = false;
            this.htmlTextBox33.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox33.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox33.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox33.Value = "=FORMAT(\'{0:###,##0.}\', Fields.ValorUnitario)";
            // 
            // htmlTextBox32
            // 
            this.htmlTextBox32.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox32.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.7985801696777344D), Telerik.Reporting.Drawing.Unit.Cm(1.0093052793536117E-07D));
            this.htmlTextBox32.Name = "htmlTextBox32";
            this.htmlTextBox32.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox32.Style.Font.Bold = false;
            this.htmlTextBox32.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox32.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.htmlTextBox32.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox32.Value = "{Fields.Cantidad.ToString().Replace(\",\" , \".\")}";
            // 
            // campo29_v
            // 
            this.campo29_v.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.campo29_v.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.5983800888061523D), Telerik.Reporting.Drawing.Unit.Cm(-2.2204460492503131E-16D));
            this.campo29_v.Name = "campo29_v";
            this.campo29_v.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.campo29_v.Style.Font.Bold = false;
            this.campo29_v.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.campo29_v.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.campo29_v.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.campo29_v.Value = "{Fields.Bodega}";
            // 
            // htmlTextBox30
            // 
            this.htmlTextBox30.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
            this.htmlTextBox30.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.1002004146575928D), Telerik.Reporting.Drawing.Unit.Cm(-2.2204460492503131E-16D));
            this.htmlTextBox30.Name = "htmlTextBox30";
            this.htmlTextBox30.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.4979791641235352D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.htmlTextBox30.Style.Font.Bold = false;
            this.htmlTextBox30.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.htmlTextBox30.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.htmlTextBox30.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.htmlTextBox30.Value = "{Fields.ProductoNombre}";
            // 
            // Formato2Detalles
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Formato2Detalles";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(3.7999999523162842D), Telerik.Reporting.Drawing.Unit.Mm(1D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(2D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(20.299999237060547D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.HtmlTextBox htmlTextBox29;
        private Telerik.Reporting.HtmlTextBox htmlTextBox39;
        private Telerik.Reporting.HtmlTextBox htmlTextBox38;
        private Telerik.Reporting.HtmlTextBox htmlTextBox36;
        private Telerik.Reporting.HtmlTextBox htmlTextBox37;
        private Telerik.Reporting.HtmlTextBox htmlTextBox34;
        private Telerik.Reporting.HtmlTextBox htmlTextBox33;
        private Telerik.Reporting.HtmlTextBox htmlTextBox32;
        private Telerik.Reporting.HtmlTextBox campo29_v;
        private Telerik.Reporting.HtmlTextBox htmlTextBox30;
    }
}