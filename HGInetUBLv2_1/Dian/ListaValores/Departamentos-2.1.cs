using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaDepartamentos
	{
		public string ShortName = "Departamentos";
		public string LongName = "Departamentos";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:Departamentos-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:Departamentos-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/Departamentos-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("91", "Amazonas", "Amazonas"),
			new ListaItem("05", "Antioquia", "Antioquia"),
			new ListaItem("81", "Arauca", "Arauca"),
			new ListaItem("08", "Atlántico", "Atlántico"),
			new ListaItem("11", "Bogotá", "Bogotá"),
			new ListaItem("13", "Bolívar", "Bolívar"),
			new ListaItem("15", "Boyacá", "Boyacá"),
			new ListaItem("17", "Caldas", "Caldas"),
			new ListaItem("18", "Caquetá", "Caquetá"),
			new ListaItem("85", "Casanare", "Casanare"),
			new ListaItem("19", "Cauca", "Cauca"),
			new ListaItem("20", "Cesar", "Cesar"),
			new ListaItem("27", "Chocó", "Chocó"),
			new ListaItem("23", "Córdoba", "Córdoba"),
			new ListaItem("25", "Cundinamarca", "Cundinamarca"),
			new ListaItem("94", "Guainía", "Guainía"),
			new ListaItem("95", "Guaviare", "Guaviare"),
			new ListaItem("41", "Huila", "Huila"),
			new ListaItem("44", "La Guajira", "La Guajira"),
			new ListaItem("47", "Magdalena", "Magdalena"),
			new ListaItem("50", "Meta", "Meta"),
			new ListaItem("52", "Nariño", "Nariño"),
			new ListaItem("54", "Norte de Santander", "Norte de Santander"),
			new ListaItem("86", "Putumayo", "Putumayo"),
			new ListaItem("63", "Quindío", "Quindío"),
			new ListaItem("66", "Risaralda", "Risaralda"),
			new ListaItem("88", "San Andrés y Providencia", "San Andrés y Providencia"),
			new ListaItem("68", "Santander", "Santander"),
			new ListaItem("70", "Sucre", "Sucre"),
			new ListaItem("73", "Tolima", "Tolima"),
			new ListaItem("76", "Valle del Cauca", "Valle del Cauca"),
			new ListaItem("97", "Vaupés", "Vaupés"),
			new ListaItem("99", "Vichada", "Vichada"),

		};

	}
}
