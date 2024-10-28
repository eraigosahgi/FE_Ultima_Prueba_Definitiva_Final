namespace HGInetFacturaEReports.Facturas
{
	partial class Formato5Detalles
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
			this.htmlTextBox29 = new Telerik.Reporting.TextBox();
			this.htmlTextBox39 = new Telerik.Reporting.TextBox();
			this.htmlTextBox38 = new Telerik.Reporting.TextBox();
			this.htmlTextBox36 = new Telerik.Reporting.TextBox();
			this.htmlTextBox37 = new Telerik.Reporting.TextBox();
			this.htmlTextBox34 = new Telerik.Reporting.TextBox();
			this.htmlTextBox33 = new Telerik.Reporting.TextBox();
			this.htmlTextBox32 = new Telerik.Reporting.TextBox();
			this.campo29_v = new Telerik.Reporting.TextBox();
			this.htmlTextBox30 = new Telerik.Reporting.TextBox();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// detail
			// 
			this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.25D);
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
			this.htmlTextBox29.CanGrow = false;
			this.htmlTextBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010004286741605029D));
			this.htmlTextBox29.Name = "htmlTextBox29";
			this.htmlTextBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1099998950958252D), Telerik.Reporting.Drawing.Unit.Cm(0.24980023503303528D));
			this.htmlTextBox29.Style.Font.Bold = false;
			this.htmlTextBox29.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox29.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.htmlTextBox29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox29.Value = "{Fields.ProductoCodigo}";
			// 
			// htmlTextBox39
			// 
			this.htmlTextBox39.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox39.CanGrow = false;
			this.htmlTextBox39.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.115848541259766D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.htmlTextBox39.Name = "htmlTextBox39";
			this.htmlTextBox39.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8540518283843994D), Telerik.Reporting.Drawing.Unit.Cm(0.2498001903295517D));
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
			this.htmlTextBox38.CanGrow = false;
			this.htmlTextBox38.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.915647506713867D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.htmlTextBox38.Name = "htmlTextBox38";
			this.htmlTextBox38.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.htmlTextBox38.Style.Font.Bold = false;
			this.htmlTextBox38.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox38.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.htmlTextBox38.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox38.Value = "{Fields.ReteFuentePorcentaje.ToString().Replace(\",\" , \".\")}";
			// 
			// htmlTextBox36
			// 
			this.htmlTextBox36.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox36.CanGrow = false;
			this.htmlTextBox36.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.715452194213867D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.htmlTextBox36.Name = "htmlTextBox36";
			this.htmlTextBox36.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.htmlTextBox36.Style.Font.Bold = false;
			this.htmlTextBox36.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox36.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.htmlTextBox36.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox36.Value = "{Fields.IvaPorcentaje.ToString().Replace(\",\" , \".\")}";
			// 
			// htmlTextBox37
			// 
			this.htmlTextBox37.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox37.CanGrow = false;
			this.htmlTextBox37.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.09999942779541D), Telerik.Reporting.Drawing.Unit.Cm(9.9227028840687126E-05D));
			this.htmlTextBox37.Name = "htmlTextBox37";
			this.htmlTextBox37.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6110544204711914D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.htmlTextBox37.Style.Font.Bold = false;
			this.htmlTextBox37.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox37.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.htmlTextBox37.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox37.Value = "=FORMAT(\'{0:###,##0.}\', Fields.ValorSubtotal)";
			// 
			// htmlTextBox34
			// 
			this.htmlTextBox34.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox34.CanGrow = false;
			this.htmlTextBox34.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.61424446105957D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.htmlTextBox34.Name = "htmlTextBox34";
			this.htmlTextBox34.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.48555588722229D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.htmlTextBox34.Style.Font.Bold = false;
			this.htmlTextBox34.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox34.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.htmlTextBox34.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox34.Value = "{Fields.DescuentoPorcentaje.ToString().Replace(\",\" , \".\")}";
			// 
			// htmlTextBox33
			// 
			this.htmlTextBox33.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox33.CanGrow = false;
			this.htmlTextBox33.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.714044570922852D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.htmlTextBox33.Name = "htmlTextBox33";
			this.htmlTextBox33.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8999999761581421D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.htmlTextBox33.Style.Font.Bold = false;
			this.htmlTextBox33.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox33.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.htmlTextBox33.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox33.Value = "=FORMAT(\'{0:###,##0.}\', Fields.ValorUnitario)";
			// 
			// htmlTextBox32
			// 
			this.htmlTextBox32.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox32.CanGrow = false;
			this.htmlTextBox32.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.5291471481323242D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.htmlTextBox32.Name = "htmlTextBox32";
			this.htmlTextBox32.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1846973896026611D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.htmlTextBox32.Style.Font.Bold = false;
			this.htmlTextBox32.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox32.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.htmlTextBox32.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox32.Value = "{Fields.Cantidad.ToString().Replace(\",\" , \".\")}";
			// 
			// campo29_v
			// 
			this.campo29_v.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.campo29_v.CanGrow = false;
			this.campo29_v.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.3136434555053711D), Telerik.Reporting.Drawing.Unit.Cm(0.0001000316915451549D));
			this.campo29_v.Name = "campo29_v";
			this.campo29_v.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2153027057647705D), Telerik.Reporting.Drawing.Unit.Cm(0.24980016052722931D));
			this.campo29_v.Style.Font.Bold = false;
			this.campo29_v.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.campo29_v.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
			this.campo29_v.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.campo29_v.Value = "{Fields.Bodega}";
			// 
			// htmlTextBox30
			// 
			this.htmlTextBox30.Anchoring = Telerik.Reporting.AnchoringStyles.Bottom;
			this.htmlTextBox30.CanGrow = false;
			this.htmlTextBox30.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.1102004051208496D), Telerik.Reporting.Drawing.Unit.Cm(9.9361139291431755E-05D));
			this.htmlTextBox30.Name = "htmlTextBox30";
			this.htmlTextBox30.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.2032427787780762D), Telerik.Reporting.Drawing.Unit.Cm(0.2498001903295517D));
			this.htmlTextBox30.Style.Font.Bold = false;
			this.htmlTextBox30.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
			this.htmlTextBox30.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
			this.htmlTextBox30.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
			this.htmlTextBox30.Value = "{Fields.ProductoNombre}";
			// 
			// Formato5Detalles
			// 
			this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
			this.Name = "Formato5Detalles";
			this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(6D), Telerik.Reporting.Drawing.Unit.Mm(4D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(2D));
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
			styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
			styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
			styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
			this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
			this.Width = Telerik.Reporting.Drawing.Unit.Cm(19.969999313354492D);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		#endregion
		private Telerik.Reporting.DetailSection detail;
		private Telerik.Reporting.TextBox htmlTextBox29;
		private Telerik.Reporting.TextBox htmlTextBox39;
		private Telerik.Reporting.TextBox htmlTextBox38;
		private Telerik.Reporting.TextBox htmlTextBox36;
		private Telerik.Reporting.TextBox htmlTextBox37;
		private Telerik.Reporting.TextBox htmlTextBox34;
		private Telerik.Reporting.TextBox htmlTextBox33;
		private Telerik.Reporting.TextBox htmlTextBox32;
		private Telerik.Reporting.TextBox campo29_v;
		private Telerik.Reporting.TextBox htmlTextBox30;
	}
}