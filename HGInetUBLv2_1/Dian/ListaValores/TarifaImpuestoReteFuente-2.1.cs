using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaTarifaImpuestoReteFuente
	{
		public string ShortName = "TarifaImpuestoReteFuente";
		public string LongName = "Tarifas por Impuesto";
		public string Version = "1";
		public string CanonicalUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TarifaImpuestos-2.1";
		public string CanonicalVersionUri = "urn:dian:names:especificacion:ubl:listacodigos:gc:TarifaImpuestos-2.1";
		public string LocationUri = "http://dian.gov.co/ubl/os-ubl-2.0/cl/gc/default/TarifaImpuestos-2.1.gc";
		public string AgencyName = "DIAN (Dirección de Impuestos y Aduanas Nacionales)";
		public string AgencyIdentifier = "195";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			new ListaItem("2.50", "ReteFuente", "Compras generales (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Compras generales (no declarantes)"),
			new ListaItem("1.50", "ReteFuente", "Compras con tarjeta débito o crédito"),
			new ListaItem("1.50", "ReteFuente", "Compras de bienes o productos agrícolas o pecuarios sin procesamiento industrial"),
			new ListaItem("2.50", "ReteFuente", "Compras de bienes o productos agrícolas o pecuarios con procesamiento industrial (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Compras de bienes o productos agrícolas o pecuarios con procesamiento industrial declarantes (no declarantes)"),
			new ListaItem("0.50", "ReteFuente", "Compras de café pergamino o cereza"),
			new ListaItem("0.10", "ReteFuente", "Compras de combustibles derivados del petróleo"),
			new ListaItem("1.00", "ReteFuente", "Enajenación de activos fijos de personas naturales (notarías y tránsito son agentes retenedores)"),
			new ListaItem("1.00", "ReteFuente", "Compras de vehículos"),
			new ListaItem("1.00", "ReteFuente", "Compras de bienes raíces cuya destinación y uso sea vivienda de habitación (por las primeras 20.000 UVT, es decir hasta $637.780.000)"),
			new ListaItem("2.50", "ReteFuente", "Compras  de bienes raíces cuya destinación y uso sea vivienda de habitación (exceso de las primeras 20.000 UVT, es decir superior a $637.780.000)"),
			new ListaItem("2.50", "ReteFuente", "Compras  de bienes raíces cuya destinación y uso sea distinto a vivienda de habitación"),
			new ListaItem("4.00", "ReteFuente", "Servicios generales (declarantes)"),
			new ListaItem("6.00", "ReteFuente", "Servicios generales (no declarantes)"),
			new ListaItem("4.00", "ReteFuente", "Por emolumentos eclesiásticos (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Por emolumentos eclesiásticos (no declarantes)"),
			new ListaItem("1.00", "ReteFuente", "Servicios de transporte de carga"),
			new ListaItem("3.50", "ReteFuente", "Servicios de  transporte nacional de pasajeros por vía terrestre (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Servicios de  transporte nacional de pasajeros por vía terrestre (no declarantes)"),
			new ListaItem("1.00", "ReteFuente", "Servicios de  transporte nacional de pasajeros por vía aérea o marítima"),
			new ListaItem("1.00", "ReteFuente", "Servicios prestados por empresas de servicios temporales (sobre AIU)"),
			new ListaItem("2.00", "ReteFuente", "Servicios prestados por empresas de vigilancia y aseo (sobre AIU)"),
			new ListaItem("2.00", "ReteFuente", "Servicios integrales de salud prestados por IPS"),
			new ListaItem("3.50", "ReteFuente", "Servicios de hoteles y restaurantes (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Servicios de hoteles y restaurantes (no declarantes)"),
			new ListaItem("4.00", "ReteFuente", "Arrendamiento de bienes muebles"),
			new ListaItem("3.50", "ReteFuente", "Arrendamiento de bienes inmuebles (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Arrendamiento de bienes inmuebles (no declarantes)"),
			new ListaItem("2.50", "ReteFuente", "Otros ingresos tributarios (declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Otros ingresos tributarios (no declarantes)"),
			new ListaItem("11.00", "ReteFuente", "Honorarios y comisiones (personas jurídicas)"),
			new ListaItem("11.00", "ReteFuente", "Honorarios y comisiones personas naturales que suscriban contrato o cuya sumatoria de los pagos o abonos en cuenta superen las 3.300 UVT ($105.135.000)"),
			new ListaItem("10.00", "ReteFuente", "Honorarios y comisiones (no declarantes)"),
			new ListaItem("3.50", "ReteFuente", "Servicios de licenciamiento o derecho de uso de software"),
			new ListaItem("7.00", "ReteFuente", "Intereses o rendimientos financieros"),
			new ListaItem("4.00", "ReteFuente", "Rendimientos financieros provenientes de títulos de renta fija"),
			new ListaItem("20.00", "ReteFuente", "Loterías, rifas, apuestas y similares"),
			new ListaItem("3.00", "ReteFuente", "Retención en colocación independiente de juegos de suerte y azar"),
			new ListaItem("2.00", "ReteFuente", "Contratos de construcción  y urbanización"),

		};

	}
}
