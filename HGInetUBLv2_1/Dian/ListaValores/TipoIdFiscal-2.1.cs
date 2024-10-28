using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTipoIdFiscal
	{
		public string ShortName = "TipoIdFiscal";
		public string LongName = "Tipo de Identificador Fiscal";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoIdFiscal-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoIdFiscal-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoIdFiscal-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("11", "Registro civil ", "Registro civil "),
			new ListaItem("12", "Tarjeta de identidad ", "Tarjeta de identidad "),
			new ListaItem("13", "Cédula de ciudadanía ", "Cédula de ciudadanía "),
			new ListaItem("21", "Tarjeta de extranjería ", "Tarjeta de extranjería "),
			new ListaItem("22", "Cédula de extranjería ", "Cédula de extranjería "),
			new ListaItem("31", "NIT", "NIT"),
			new ListaItem("41", "Pasaporte ", "Pasaporte "),
			new ListaItem("42", "Documento de identificación extranjero ", "Documento de identificación extranjero "),
			new ListaItem("47", "PEP", "PEP"),
			new ListaItem("48", "PPT (Permiso Protección Temporal)", "PPT (Permiso Protección Temporal)"),
			new ListaItem("50", "NIT de otro país", "NIT de otro país"),
			new ListaItem("91", "NUIP * ", "NUIP * "),

		};

	}
}
