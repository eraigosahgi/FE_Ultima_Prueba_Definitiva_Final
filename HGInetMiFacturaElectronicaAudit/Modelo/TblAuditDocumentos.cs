//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetMiFacturaElectronicaAudit.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblAuditDocumentos
    {
        public System.Guid Id { get; set; }
        public System.Guid StrIdSeguridad { get; set; }
        public string StrIdPeticion { get; set; }
        public System.DateTime DatFecha { get; set; }
        public string StrObligado { get; set; }
        public int IntIdEstado { get; set; }
        public int IntIdProceso { get; set; }
        public int IntTipoRegistro { get; set; }
        public int IntIdProcesadoPor { get; set; }
        public string StrRealizadoPor { get; set; }
        public string StrMensaje { get; set; }
        public string StrResultadoProceso { get; set; }
        public string StrPrefijo { get; set; }
        public string StrNumero { get; set; }
    }
}