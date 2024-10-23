using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{

    #region ValidacionF3

    public enum ValidacionF3
    {
        [Description("No valida cartera")]
        No_Valida_Cartera = 0,

        [Description("Valida cartera")]
        Valida_Cartera = 1,

        [Description("Ninguna acción")]
        Ninguna_Accion = 2,
    }

    #endregion


    //#region Pedido

    //public enum Pedido
    //{
    //    [Description("No Aplica")]
    //    NoAplica = 0,

    //    [Description("Pedido/O.C")]
    //    PedidoOC = 1,

    //    [Description("Cancelación")]
    //    Cancelacion = 2,

    //    [Description("Descarga ")]
    //    Descarga = 3,

    //}

    //#endregion

    #region Or.Concepto

    public enum OrConcepto
    {
        [Description("Clase")]
        Clase = 0,

        [Description("Tipo")]
        Tipo = 1,

        [Description("Grupo ")]
        Grupo = 2,

    }
    #endregion

    #region Doc.Ref.F9

    public enum DocRefF9
    {
        [Description("Ninguno.")]
        Ninguno = 0,

        [Description("Doc. Ref.")]
        DocRef = 1,

        [Description("Documento. ")]
        Documento = 2,

        [Description("Recibo. ")]
        Recibo = 3,



    }
    #endregion

    #region Asume Cantidad

    public enum AsumeCantidad
    {
        [Description("No asume cantidad.")]
        NoAsumeCantidad = 0,

        [Description("Asume cantidad.")]
        AsumeCantidad = 1,

        [Description("Sugiere cantidad. ")]
        SugiereCantidad = 2,

    }
    #endregion




}
