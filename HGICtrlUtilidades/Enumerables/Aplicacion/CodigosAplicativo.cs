using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
    public enum CodigosAplicativo
    {
        [Description("HGInet Administrativo")]
        HGInetAdministrativ = 1,

        [Description("HGInet Contable")]
        HGInetContable = 2,

        [Description("HGInet Nómina")]
        HGInetNomina = 3,

        [Description("HGInet POS")]
        HGInetPOS = 4,

        [Description("HGInet Ganadero")]
        HGInetGanadero = 6,

        [Description("HGInet Móvil")]
        HGInetMovil = 10,

        [Description("HGInet Servicios Web")]
        HGInetServiciosWeb = 11,

        [Description("HGInet Smart")]
        HGInetSmart = 12,

        [Description("HGInet Facturación Electrónica")]
        HGInetFacturacionElectronica = 13,

        [Description("HGInet Backup")]
        HGInetBackup = 18,

        [Description("HGInet Email")]
        HGInetEmail = 19,

        [Description("HGInet SMS")]
        HGInetSMS = 20,

        [Description("HGInet Pagos Electrónicos")]
        HGInetPagosElectronicos = 21,

        [Description("Happgi")]
        Happgi = 23,

        [Description("Happgi Contable")]
        HappgiContable = 24,

        [Description("Happgi Nómina")]
        HappgiNomina = 25,

        [Description("Happgi Pos")]
        HappgiPos = 26,

        [Description("CRM")]
		HappgiCRM = 27,

		[Description("WMS")]
		HappgiWMS = 28,

		[Description("HGI Store")]
		HGIEcommerce = 30,

		[Description("ERP Web")]
        ErpWebAdmin = 33,

		[Description("ERP Web Contable")]
		ErpWebContable = 34,

		[Description("ERP Web Nómina")]
		ErpWebNomina = 35,

		[Description("ERP Web Pos")]
		ErpWebPos = 36,

	}
}
