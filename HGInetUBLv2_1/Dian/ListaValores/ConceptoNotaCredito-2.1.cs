using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaConceptoNotaCredito
	{
		public string ShortName = "ConceptoNotaCredito";
		public string LongName = "Concepto de Correción para Notas crédito";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:ConceptoNotaCredito-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:ConceptoNotaCredito-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/ConceptoNotaCredito-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("1", "Devolución parcial de los bienes y/o no aceptación parcial del servicio", "Devolución parcial de los bienes y/o no aceptación parcial del servicio"),
new ListaItem("2", "Anulación de factura electrónica", "Anulación de factura electrónica"),
new ListaItem("3", "Rebaja o descuento parcial o total","Rebaja o descuento parcial o total"),
new ListaItem("4", "Ajuste de precio", "Ajuste de precio"),
new ListaItem("5", "Otros", "Otros"),
new ListaItem("6", "Otros ", "Otros "),

		};

	}
}
