using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoCodigoProducto
	{
		public string ShortName = "TipoCodigoProducto";
		public string LongName = "Eventos de un documento electronico";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoCodigoProducto-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoCodigoProducto-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoCodigoProducto-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("001", "UNSPSC", "UNSPSC"),
			new ListaItem("010", "GTIN", "GTIN"),
			new ListaItem("999", "Estándar de adopción del contribuyente", "Estándar de adopción del contribuyente"),

		};

	}
}
