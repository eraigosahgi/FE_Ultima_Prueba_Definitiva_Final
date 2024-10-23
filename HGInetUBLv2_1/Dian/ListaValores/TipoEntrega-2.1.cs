using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoEntrega
	{
		public string ShortName = "TipoEntrega";
		public string LongName = "Condiciones de Entrega";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoEntrega-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoEntrega-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoEntrega-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("CFR", "Costo y flete", "Costo y flete"),
			new ListaItem("CIF", "Costo, flete y seguro", "Costo, flete y seguro"),
			new ListaItem("CIP", "Transporte y Seguro Pagados hasta", "Transporte y Seguro Pagados hasta"),
			new ListaItem("CPT", "Transporte Pagado Hasta", "Transporte Pagado Hasta"),
			new ListaItem("DAP", "Entregado en un Lugar", "Entregado en un Lugar"),
			new ListaItem("DAT", "Entregado en Terminal", "Entregado en Terminal"),
			new ListaItem("DDP", "Entregado con Pago de Derechos", "Entregado con Pago de Derechos"),
			new ListaItem("EXW", "En Fábrica", "En Fábrica"),
			new ListaItem("FAS", "Franco al costado del buque", "Franco al costado del buque"),
			new ListaItem("FCA", "Franco transportista", "Franco transportista"),
			new ListaItem("FOB", "Franco a bordo", "Franco a bordo"),

		};

	}
}
