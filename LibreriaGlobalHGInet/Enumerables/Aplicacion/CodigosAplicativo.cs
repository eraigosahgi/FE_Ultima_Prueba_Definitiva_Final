using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables.Aplicacion
{
	public enum CodigosAplicativo
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

		[Description("HGI Pagos Electrónicos")]
		HGInetPagosElectronicos = 21,

		[Description("Happgi")]
		Happgi = 23,
	}
}
