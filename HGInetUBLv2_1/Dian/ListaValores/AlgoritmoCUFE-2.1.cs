using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaAlgoritmoCUFE
	{
		public string ShortName = "AlgoritmoCUFE";
		public string LongName = "Algoritmo de encriptado del CUFE";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:AlgoritmoCUFE-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:AlgoritmoCUFE-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/AlgoritmoCUFE-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("CUFE-SHA384", "Algoritmo SHA-384", "Algoritmo SHA-384"),
			new ListaItem("CUDE-SHA384", "Algoritmo SHA-384", "Algoritmo SHA-384"),
		};

	}
}
