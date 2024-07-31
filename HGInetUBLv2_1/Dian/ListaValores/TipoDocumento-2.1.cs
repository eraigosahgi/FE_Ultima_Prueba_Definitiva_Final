using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoDocumento
	{
		public string ShortName = "TipoDocumento";
		public string LongName = "Tipo de Documento";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoDocumento-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoDocumento-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoDocumento-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("01", "Factura de Venta Nacional", "Factura de Venta Nacional"),
			new ListaItem("02", "Factura de Exportación ", "Factura de Exportación "),
			new ListaItem("03", "Factura por Contingencia Facturador", "Factura por Contingencia Facturador"),
			new ListaItem("04", "Factura por Contingencia DIAN", "Factura por Contingencia DIAN"),
			new ListaItem("05", "documento soporte en adquisiciones efectuadas a sujetos no obligados(equivalente)", "documento soporte en adquisiciones efectuadas a sujetos no obligados(equivalente)"),
			new ListaItem("20", "documento soporte equivalente POS", "documento soporte equivalente POS"),
			new ListaItem("35", "documento soporte equivalente POS Pasajeros", "documento soporte equivalente POS Pasajeros"),
			new ListaItem("91", "Nota Crédito", "Nota Crédito"),
			new ListaItem("92", "Nota Débito", "Nota Débito"),

		};

	}
}
