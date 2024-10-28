using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaCodigoDescuento
	{
		public string ShortName = "CodigoDescuento";
		public string LongName = "Codigos de descuentos";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:CodigoDescuento-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:CodigoDescuento-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/CodigoDescuento-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("00", "Descuento por impuesto asumido", "Descuento por impuesto asumido"),
			new ListaItem("01", "Pague uno lleve otro", "Pague uno lleve otro"),
			new ListaItem("02", "Descuentos contractulales", "Descuentos contractulales"),
			new ListaItem("03", "Descuento por pronto pago", "Descuento por pronto pago"),
			new ListaItem("04", "Envío gratis", "Envío gratis"),
			new ListaItem("05", "Descuentos escpecíficos por inventarios", "Descuentos escpecíficos por inventarios"),
			new ListaItem("06", "Descuento por monto de compras", "Descuento por monto de compras"),
			new ListaItem("07", "Descuento de temporada", "Descuento de temporada"),
			new ListaItem("08", "Descuento por acturalización de productos / servicios", "Descuento por acturalización de productos / servicios"),
			new ListaItem("09", "Descuento general", "Descuento general"),
			new ListaItem("10", "Descuento por volumen", "Descuento por volumen"),
			new ListaItem("11", "Otro descuento", "Otro descuento"),

		};

	}
}
