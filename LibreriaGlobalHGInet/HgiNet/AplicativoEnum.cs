using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.HgiNet
{
	public enum AplicativoEnum
	{
		[Description("HGI Administrativo")]
		HGInetAdministrativ = 1,

		[Description("HGI Contable")]
		HGInetContable = 2,

		[Description("HGI Nómina")]
		HGInetNomina = 3,

		[Description("HGI POS")]
		HGInetPOS = 4,

		[Description("HGI Ganadero")]
		HGInetGanadero = 6,

		[Description("HGI Móvil")]
		HGInetMovil = 10,

		[Description("HGI Servicios Web")]
		HGInetServiciosWeb = 11,

		[Description("HGI CRM")]
		HGInetSmart = 12,

		[Description("HGI Facturación Electrónica")]
		HGInetFacturacionElectronica = 13,

		[Description("HGI Backup")]
		HGInetBackup = 18,

		[Description("HGI Email")]
		HGInetEmail = 19,

		[Description("HGI SMS")]
		HGInetSMS = 20,

		[Description("Administración")]
		Happgi = 23,		

		[Description("Contabilidad")]
		HappgiContable = 24,

		[Description("Nómina")]
		HappgiNomina = 25,

		[Description("Pos")]
		HappgiPos = 26,

		[Description("CRM")]
		HappgiCRM = 27,

		[Description("WMS")]
		HappgiWMS = 28,

		[Description("HGI Store")]
		Ecommerce = 30,

		[Description("Administración")]
		ErpWeb = 33,

		[Description("Contabilidad")]
		ErpWebContable = 34,

		[Description("Nómina")]
		ErpWebNomina = 35,

		[Description("Pos")]
		ErpWebPos = 36,

		[Description("Impresión")]
		Impresion = 99,
	}


	public enum AplicativoEdicion
	{
		[Description("Edición Factura")]
		EdicionFactura = -1,

		[Description("Edición Express")]
		EdicionExpress = 0,

		[Description("Edición Básica")]
		EdicionBasica = 1,

		[Description("Edición Estándar")]
		EdicionEstandar = 2,

		[Description("Edición Avanzada")]
		EdicionAvanzada = 3,

		[Description("Edición Contador")]
		EdicionContador = 4,

	}


	public enum AmbientesUrl
	{
		[Description("HGI Facturación Electrónica Producción")]
		HGInetFacturaEProduccion = 1,

		[Description("HGI Facturación Electrónica Habilitación")]
		HGInetFacturaEHabilitacion = 2,

		[Description("HGI Facturación Electrónica QA")]
		HGInetFacturaEQa = 3,

		[Description("HGI Licencia")]
		HGInetLicencia = 4,

		[Description("MiPaquete")]
		MiPaquete = 5,
	}

}
