using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{

	public enum DocumentTypeV2
	{
		[Description("01")]
		FacturaNacional = 1,

		[Description("92")]
		NotaDebito = 2,

		[Description("91")]
		NotaCredito = 3,

		[Description("03")]
		FacturaContingencia = 4,

		[Description("02")]
		FacturaExportacion = 5,

	}

	public enum TipoOperacion
	{

		[Description("03")]
		[AmbientValue("Factura por Contingencia Facturador")]
		FacturaContingencia = 1,

		[Description("02")]
		[AmbientValue("Factura de Exportación")]
		FacturaExportacion = 2,

		[Description("04")]
		[AmbientValue("Factura por Contingencia  DIAN")]
		FacturaContingenciaDian = 3,

		[Description("20")]
		[AmbientValue("Nota Crédito que referencia una factura electrónica.")]
		NCV2 = 20,

		[Description("22")]
		[AmbientValue("Nota Crédito sin referencia a facturas")]
		NCSinRef = 22,

		[Description("23")]
		[AmbientValue("Nota Crédito para facturación electrónica V1 (Decreto 2242).")]
		NC2242 = 23,

		[Description("30")]
		[AmbientValue("Nota Débito que referencia una factura electrónica.")]
		NDV2 = 30,

		[Description("32")]
		[AmbientValue("Nota Débito sin referencia a facturas")]
		NDSinRef = 32,

		[Description("33")]
		[AmbientValue("Nota Débito para facturación electrónica V1 (Decreto 2242).")]
		ND2242 = 33,


	}
}
