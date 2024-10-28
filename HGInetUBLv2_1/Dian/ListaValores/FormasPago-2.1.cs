using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaFormasPago
	{
		public string ShortName = "FormasPago";
		public string LongName = "Tipos de Formas de Pago";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:FormasPago-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:FormasPago-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/FormasPago-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("1", "Contado", "Contado"),
new ListaItem("2", "Crédito", "Crédito"),

		};

	}
}
