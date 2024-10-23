using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{

    #region TiposBancos
    public enum TiposBancos
    {

        [Description("Occidente")]
        Occidente = 0,

        [Description("BancolombiaPAB")]
        BancolombiaPAB = 1,

        [Description("Bogotá")]
        Bogota = 2,

        [Description("Bancafé")]
        Bancafe = 3,

        [Description("BancolombiaSAP")]
        BancolombiaSAP = 4,

        [Description("Caja Social")]
        CajaSocial = 5,

        [Description("Davivienda")]
        Davivienda = 6,

        [Description("Santander")]
        Santander = 7,

        [Description("AvVillas")]
        AvVillas = 8,

        [Description("Colpatria")]
        Colpatria = 9,

        [Description("BBVA")]
        BBVA = 10,

        [Description("GNB Sudameris")]
        GNBSudameris = 11,

        [Description("CorpBanca")]
        CorpBanca = 12,

    }

    #endregion

    #region TipoBancos
    public enum TipoBancos
    {
        [Description("Corriente")]
        Corriente = 0,

        [Description("Ahorros")]
        Ahorros = 1,

        [Description("Virtual")]
        Virtual = 2,
    }

    #endregion

}


