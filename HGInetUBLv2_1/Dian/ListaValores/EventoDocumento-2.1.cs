using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaEventoDocumento
	{
		public string ShortName = "EventoDocumento";
		public string LongName = "Eventos de un Documento Electrónico";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:EventoDocumento-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:EventoDocumento-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/EventoDocumento-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("001", "Uso Autorizado por PA", "Uso Autorizado por PA"),
new ListaItem("002", "Uso Autorizado por la DIAN", "Uso Autorizado por la DIAN"),
new ListaItem("003", "Documento Fiscal Electrónico Validado por PA, y que Debería Haber Sido Rechazado", "Documento Fiscal Electrónico Validado por PA, y que Debería Haber Sido Rechazado"),
new ListaItem("010", "Documento Fical Electrónico Referenciado por Otro Documento Fiscal Electrónico", "Documento Fical Electrónico Referenciado por Otro Documento Fiscal Electrónico"),
new ListaItem("011", "Documento Referenciado no Existe en la Base de Datos de la DIAN", "Documento Referenciado no Existe en la Base de Datos de la DIAN"),
new ListaItem("012", "Anulación de Efecto de Evento", "Anulación de Efecto de Evento"),
new ListaItem("015", "Anotación de Oficio por la DIAN", "Anotación de Oficio por la DIAN"),
new ListaItem("020", "Anulación de Negocio", "Anulación de Negocio"),
new ListaItem("021", "Anulación de Documento", "Anulación de Documento"),
new ListaItem("030", "Solicitación de Corrección en Documento", "Solicitación de Corrección en Documento"),
new ListaItem("031", "Rechazo de Documento", "Rechazo de Documento"),
new ListaItem("032", "Recibimiento de los Bienes", "Recibimiento de los Bienes"),
new ListaItem("033", "Aceptación de Documento", "Aceptación de Documento"),
new ListaItem("040", "Factura Ofrecida para Negociación como Título Valor", "Factura Ofrecida para Negociación como Título Valor"),
new ListaItem("041", "Factura Negociada como Título Valor", "Factura Negociada como Título Valor"),

		};

	}
}
