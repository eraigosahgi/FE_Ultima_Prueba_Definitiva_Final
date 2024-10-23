using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoOrganizacion
	{
		public string ShortName = "TipoOrganizacion";
		public string LongName = "Tipo de organización jurídica (Personas)";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoOrganizacion-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoOrganizacion-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoOrganizacion-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("1", "Persona Jurídica", "Persona Jurídica"),
			new ListaItem("2", "Persona Natural", "Persona Natural"),

		};

	}
}
