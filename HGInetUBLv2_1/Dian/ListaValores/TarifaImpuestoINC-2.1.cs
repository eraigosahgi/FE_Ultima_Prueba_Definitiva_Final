using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTarifaImpuestoINC
	{
		public string ShortName = "TarifaImpuestoINC";
		public string LongName = "Tarifas por Impuesto";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TarifaImpuestoINC-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TarifaImpuestoINC-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TarifaImpuestoINC-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("2.00", "INC", "Tarifa especial"),
			new ListaItem("4.00", "INC", "Tarifa especial"),
			new ListaItem("8.00", "INC", "Tarifa general"),
			new ListaItem("16.00", "INC", "Tarifa especial"),

		};

	}
}
