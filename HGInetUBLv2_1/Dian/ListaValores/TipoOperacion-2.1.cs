using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoOperacion
	{
		public string ShortName = "TipoOperacion";
		public string LongName = "Tipo de operacion del documento";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoOperacion-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoOperacion-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoOperacion-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("1", "Estandar", "Estandar"),
			new ListaItem("2", "Mandato Servicio", "Mandato Servicio"),
			new ListaItem("3", "Mandato Bienes", "Mandato Bienes"),

		};

	}
}
