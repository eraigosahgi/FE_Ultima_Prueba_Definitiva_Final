using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoResponsabilidad
	{
		public string ShortName = "TipoResponsabilidad";
		public string LongName = "Responsabilidades fiscales; Régimen fiscal";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoResponsabilidad-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoResponsabilidad-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoResponsabilidad-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("O-13", "Gran contribuyente", "Gran contribuyente"),
			new ListaItem("O-15", "Autorretenedor", "Autorretenedor"),
			new ListaItem("O-23", "Agente de retención en el impuesto sobre las ventas", "Agente de retención en el impuesto sobre las ventas"),
			new ListaItem("O-47", "Régimen Simple de Tributación – SIMPLE", "Régimen Simple de Tributación – SIMPLE"),
			new ListaItem("R-99-PN", "Otro tipo de responsable PJ", "Otro tipo de responsable PJ"),

		};

	}
}
