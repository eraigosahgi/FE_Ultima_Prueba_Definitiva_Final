using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaCodigoPrecioReferencia
	{
		public string ShortName = "CodigoPrecioReferencia";
		public string LongName = "Lista de códigos para precios de referencia";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:CodigoPrecioReferencia-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:CodigoPrecioReferencia-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/CodigoPrecioReferencia-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("01", "Valor comercial", "Valor comercial"),
new ListaItem("02", "Valor en inventarios", "Valor en inventarios"),
new ListaItem("03", "Otro valor", "Otro valor"),

		};

	}
}
