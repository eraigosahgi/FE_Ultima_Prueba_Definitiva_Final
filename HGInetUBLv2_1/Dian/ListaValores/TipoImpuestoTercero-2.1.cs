﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{

	/// <summary>
	/// Se agrega Lista de impuesto solo para Tercero segun Anexo V1.8 Seccion 13.2.6.2. Para el grupo PartyTaxScheme
	/// </summary>
	public class ListaTipoImpuestoTercero
	{
		public string ShortName = "TipoImpuestoTercero";
		public string LongName = "Tipo de Tributos";
		public string Version = "1";
		//public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoImpuesto-2.1";
		//public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TipoImpuesto-2.1";
		//public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TipoImpuesto-2.1.gc";
		//public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		//public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("01", "IVA", "IVA"),
			new ListaItem("04", "INC", "INC"),
			new ListaItem("22", "Bolsas", "Bolsas"),
			new ListaItem("ZA", "IVA e INC", "IVA e INC"),
			new ListaItem("ZZ", "No aplica", "No aplica"),

		};

	}
}
