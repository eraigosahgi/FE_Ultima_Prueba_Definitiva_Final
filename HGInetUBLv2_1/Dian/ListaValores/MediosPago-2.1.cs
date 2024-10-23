using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaMediosPago
	{
		public string ShortName = "MediosPago";
		public string LongName = "Medios de Pago";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:MediosPago-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:MediosPago-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/MediosPago-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("1", "Instrumento no definido", "Instrumento no definido"),
			new ListaItem("2", "Crédito ACH", "Crédito ACH"),
			new ListaItem("3", "Débito ACH", "Débito ACH"),
			new ListaItem("4", "Reversión débito de demanda ACH", "Reversión débito de demanda ACH"),
			new ListaItem("5", "Reversión crédito de demanda ACH ", "Reversión crédito de demanda ACH "),
			new ListaItem("6", "Crédito de demanda ACH", "Crédito de demanda ACH"),
			new ListaItem("7", "Débito de demanda ACH", "Débito de demanda ACH"),
			new ListaItem("8", "Mantener", "Mantener"),
			new ListaItem("9", "Clearing Nacional o Regional", "Clearing Nacional o Regional"),
			new ListaItem("10", "Efectivo", "Efectivo"),
			new ListaItem("11", "Reversión Crédito Ahorro", "Reversión Crédito Ahorro"),
			new ListaItem("12", "Reversión Débito Ahorro", "Reversión Débito Ahorro"),
			new ListaItem("13", "Crédito Ahorro", "Crédito Ahorro"),
			new ListaItem("14", "Débito Ahorro", "Débito Ahorro"),
			new ListaItem("15", "Bookentry Crédito", "Bookentry Crédito"),
			new ListaItem("16", "Bookentry Débito", "Bookentry Débito"),
			new ListaItem("17", "Concentración de la demanda en efectivo /Desembolso Crédito (CCD)", "Concentración de la demanda en efectivo /Desembolso Crédito (CCD)"),
			new ListaItem("18", "Concentración de la demanda en efectivo / Desembolso (CCD) débito", "Concentración de la demanda en efectivo / Desembolso (CCD) débito"),
			new ListaItem("19", "Crédito Pago negocio corporativo (CTP)", "Crédito Pago negocio corporativo (CTP)"),
			new ListaItem("20", "Cheque", "Cheque"),
			new ListaItem("21", "Poyecto bancario", "Poyecto bancario"),
			new ListaItem("22", "Proyecto bancario certificado", "Proyecto bancario certificado"),
			new ListaItem("23", "Cheque bancario", "Cheque bancario"),
			new ListaItem("24", "Nota cambiaria esperando aceptación", "Nota cambiaria esperando aceptación"),
			new ListaItem("25", "Cheque certificado", "Cheque certificado"),
			new ListaItem("26", "Cheque Local", "Cheque Local"),
			new ListaItem("27", "Débito Pago Neogcio Corporativo (CTP)", "Débito Pago Neogcio Corporativo (CTP)"),
			new ListaItem("28", "Crédito Negocio Intercambio Corporativo (CTX)", "Crédito Negocio Intercambio Corporativo (CTX)"),
			new ListaItem("29", "Débito Negocio Intercambio Corporativo (CTX)", "Débito Negocio Intercambio Corporativo (CTX)"),
			new ListaItem("30", "Transferecia Crédito", "Transferecia Crédito"),
			new ListaItem("31", "Transferencia Débito", "Transferencia Débito"),
			new ListaItem("32", "Concentración Efectivo / Desembolso Crédito plus (CCD+)", "Concentración Efectivo / Desembolso Crédito plus (CCD+)"),
			new ListaItem("33", "Concentración Efectivo / Desembolso Débito plus (CCD+)", "Concentración Efectivo / Desembolso Débito plus (CCD+)"),
			new ListaItem("34", "Pago y depósito pre acordado (PPD)", "Pago y depósito pre acordado (PPD)"),
			new ListaItem("35", "Concentración efectivo ahorros / Desembolso Crédito (CCD)", "Concentración efectivo ahorros / Desembolso Crédito (CCD)"),
			new ListaItem("36", "Concentración efectivo ahorros / Desembolso Drédito (CCD)", "Concentración efectivo ahorros / Desembolso Drédito (CCD)"),
			new ListaItem("37", "Pago Negocio Corporativo Ahorros Crédito (CTP)", "Pago Negocio Corporativo Ahorros Crédito (CTP)"),
			new ListaItem("38", "Pago Neogcio Corporativo Ahorros Débito (CTP)", "Pago Neogcio Corporativo Ahorros Débito (CTP)"),
			new ListaItem("39", "Crédito Negocio Intercambio Corporativo (CTX)", "Crédito Negocio Intercambio Corporativo (CTX)"),
			new ListaItem("40", "Débito Negocio Intercambio Corporativo (CTX)", "Débito Negocio Intercambio Corporativo (CTX)"),
			new ListaItem("41", "Concentración efectivo/Desembolso Crédito plus (CCD+) ", "Concentración efectivo/Desembolso Crédito plus (CCD+) "),
			new ListaItem("42", "Consignación bancaria", "Consignación bancaria"),
			new ListaItem("43", "Concentración efectivo / Desembolso Débito plus (CCD+)", "Concentración efectivo / Desembolso Débito plus (CCD+)"),
			new ListaItem("44", "Nota cambiaria", "Nota cambiaria"),
			new ListaItem("45", "Transferencia Crédito Bancario", "Transferencia Crédito Bancario"),
			new ListaItem("46", "Transferencia Débito Interbancario", "Transferencia Débito Interbancario"),
			new ListaItem("47", "Transferencia Débito Bancaria", "Transferencia Débito Bancaria"),
			new ListaItem("48", "Tarjeta Crédito", "Tarjeta Crédito"),
			new ListaItem("49", "Tarjeta Débito", "Tarjeta Débito"),
			new ListaItem("50", "Postgiro", "Postgiro"),
			new ListaItem("51", "Telex estándar bancario francés", "Telex estándar bancario francés"),
			new ListaItem("52", "Pago comercial urgente", "Pago comercial urgente"),
			new ListaItem("53", "Pago Tesorería Urgente", "Pago Tesorería Urgente"),
			new ListaItem("60", "Nota promisoria", "Nota promisoria"),
			new ListaItem("61", "Nota promisoria firmada por el acreedor", "Nota promisoria firmada por el acreedor"),
			new ListaItem("62", "Nota promisoria firmada por el acreedor, avalada por el banco", "Nota promisoria firmada por el acreedor, avalada por el banco"),
			new ListaItem("63", "Nota promisoria firmada por el acreedor, avalada por un tercero", "Nota promisoria firmada por el acreedor, avalada por un tercero"),
			new ListaItem("64", "Nota promisoria firmada pro el banco", "Nota promisoria firmada pro el banco"),
			new ListaItem("65", "Nota promisoria firmada por un banco avalada por otro banco", "Nota promisoria firmada por un banco avalada por otro banco"),
			new ListaItem("66", "Nota promisoria firmada ", "Nota promisoria firmada "),
			new ListaItem("67", "Nota promisoria firmada por un tercero avalada por un banco", "Nota promisoria firmada por un tercero avalada por un banco"),
			new ListaItem("70", "Retiro de nota por el por el acreedor", "Retiro de nota por el por el acreedor"),
			new ListaItem("71", "Bonos", "Bonos"),
			new ListaItem("72", "Vales", "Vales"),
			new ListaItem("74", "Retiro de nota por el por el acreedor sobre un banco", "Retiro de nota por el por el acreedor sobre un banco"),
			new ListaItem("75", "Retiro de nota por el acreedor, avalada por otro banco", "Retiro de nota por el acreedor, avalada por otro banco"),
			new ListaItem("76", "Retiro de nota por el acreedor, sobre un banco avalada por un tercero", "Retiro de nota por el acreedor, sobre un banco avalada por un tercero"),
			new ListaItem("77", "Retiro de una nota por el acreedor sobre un tercero", "Retiro de una nota por el acreedor sobre un tercero"),
			new ListaItem("78", "Retiro de una nota por el acreedor sobre un tercero avalada por un banco", "Retiro de una nota por el acreedor sobre un tercero avalada por un banco"),
			new ListaItem("91", "Nota bancaria tranferible", "Nota bancaria tranferible"),
			new ListaItem("92", "Cheque local traferible", "Cheque local traferible"),
			new ListaItem("93", "Giro referenciado", "Giro referenciado"),
			new ListaItem("94", "Giro urgente", "Giro urgente"),
			new ListaItem("95", "Giro formato abierto", "Giro formato abierto"),
			new ListaItem("96", "Método de pago solicitado no usuado", "Método de pago solicitado no usuado"),
			new ListaItem("97", "Clearing entre partners", "Clearing entre partners"),
			new ListaItem("0", "Acuerdo mutuo", "Acuerdo mutuo"),

		};

	}
}
