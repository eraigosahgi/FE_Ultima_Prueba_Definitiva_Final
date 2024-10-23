using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaConceptoNotaDebito
	{
		public string ShortName = "ConceptoNotaDebito";
		public string LongName = "Concepto de Correción para Notas débito";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:ConceptoNotaDebito-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:ConceptoNotaDebito-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/ConceptoNotaDebito-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("1", "Intereses ", "Intereses "),
new ListaItem("2", "Gastos por cobrar ", "Gastos por cobrar "),
new ListaItem("3", "Cambio del valor ", "Cambio del valor "),
new ListaItem("4", "Otro", "Otro"),

		};

	}
}
