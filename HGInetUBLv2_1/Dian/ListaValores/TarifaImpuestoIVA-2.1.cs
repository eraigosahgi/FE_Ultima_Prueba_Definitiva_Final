using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTarifaImpuestoIVA
	{
		public string ShortName = "TarifaImpuestoIVA";
		public string LongName = "Tarifas por Impuesto IVA";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TarifaImpuestoIVA-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TarifaImpuestoIVA-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TarifaImpuestoIVA-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("0.00", "IVA", "Exento"),
new ListaItem("5.00", "IVA", "Bienes / Servicios al 5"),
new ListaItem("16.00", "IVA", "Contratos firmados con el estado antes de ley 1819"),
new ListaItem("19.00", "IVA", "Tarifa general"),

		};

	}
}
