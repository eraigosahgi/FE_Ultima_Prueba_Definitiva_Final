using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoImpuesto
	{
		public string ShortName = "TipoImpuesto";
		public string LongName = "Tipo de Tributos";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoImpuesto-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoImpuesto-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoImpuesto-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("01", "IVA", "IVA"),
			new ListaItem("02", "IC", "IC"),
			new ListaItem("03", "ICA", "ICA"),
			new ListaItem("04", "INC", "INC"),
			new ListaItem("05", "ReteIVA", "ReteIVA"),
			new ListaItem("06", "ReteFuente", "ReteFuente"),
			new ListaItem("07", "ReteICA", "ReteICA"),
			new ListaItem("08", "ReteCREE", "ReteCREE"),
			new ListaItem("20", "FtoHorticultura", "FtoHorticultura"),
			new ListaItem("21", "Timbre", "Timbre"),
			new ListaItem("22", "INC Bolsas", "INC Bolsas"),
			new ListaItem("23", "INCarbono", "INCarbono"),
			new ListaItem("24", "INCombustibles", "INCombustibles"),
			new ListaItem("25", "Sobretasa Combustibles", "Sobretasa Combustibles"),
			new ListaItem("26", "Sordicom", "Sordicom"),
			new ListaItem("35", "ICUI", "ICUI"),
			new ListaItem("ZZ", "No aplica", "No aplica"),

		};

	}
}
