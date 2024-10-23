using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
    #region TipoSalario
    public enum TipoSalario
    {

        [Description("Falso")]
        F = 0,

        [Description("Verdadero")]
        V = 1,

    }
    #endregion

    #region SubTipoCotizante
    public enum SubTipoCotizante
    {
        [Description("No Aplica")]
        NoAplica = 0,

        [Description("Dependiente pensionado por vejez activo")]
        DependienteVejezActivo = 1,

        [Description("Independiente pensionado por vejez activo")]
        IndependienteVejezActivo = 2,

        [Description("Cotizante no obligados a cotización a pensiones por edad")]
        CotizantePensionesEdad = 3,

        [Description("Cotizante requisitos cumplidos por pensión ")]
        CotizanteCumplidosPension = 4,

        [Description("Cotizante a quien se ha reconocido indemnización")]
        CotizanteReconocidoIndemnizacion = 5,

        [Description("Cotizante perteneciente a un régimen exceptuado de pensiones")]
        CotizanteRegimenPensiones = 6,

        [Description("Afiliado al ahorro programado de largo plazo y cotizante al régimen contributivo de salud")]
        AfiliadoRegimenContributivoSalud = 7,

        [Description("Afiliado al ahorro programado de largo plazo y no cotizante al régimen contributivo de salud")]
        AfiliadoRegimenNoContributivoSalud = 8,

        [Description("Cotizante pensionado con mesada superior a 25 SMMLV ")]
        CotizanteMesadaSuperiorSMMLV = 9,

        [Description("Residente en el exterior afiliado voluntario al sistema general de pensiones ")]
        ResidenteExterior = 10,

        [Description("Conductores de servicio publico de transporte terrestre automotor individual de pasajeros")]
        ConductoresServicioPublicoTransporte = 11,

        [Description("Conductores de servicio publico de transporte terrestre automotor individual de pasajeros no obligados a cotizar pensión ")]
        ConductoresServicioPublicoTransporteNo = 12,
    }

    #endregion

    #region TipoCuenta
    public enum TipoCuenta
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





