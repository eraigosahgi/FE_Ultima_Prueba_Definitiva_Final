//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetMiFacturaElectonicaData.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblAlmacenamientoDocs
    {
        public System.Guid StrIdSeguridadDoc { get; set; }
        public int IntConsecutivo { get; set; }
        public string StrUrlAnterior { get; set; }
        public string StrUrlActual { get; set; }
        public System.DateTime DatFechaRegistroDoc { get; set; }
        public System.DateTime DatFechaSincronizacion { get; set; }
        public bool IntBorrado { get; set; }
        public System.DateTime DatFechaBorrado { get; set; }
    
        public virtual TblDocumentos TblDocumentos { get; set; }
    }
}