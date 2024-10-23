using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
    /// <summary>
    /// Objeto Tpado de Pago de plataforma intermedia
    /// </summary>
    public class TblPasarelaPagosPI
    {
        public Nullable<Guid> StrIdSeguridadComercio { get; set; }
        public string StrIdSeguridad { get; set; }
        public System.Guid StrIdSeguridadRegistro { get; set; }
        public System.DateTime DatFechaRegistro { get; set; }
        public string StrComercioData { get; set; }

        public string StrPagoIdPlataforma { get; set; }
        public string StrIdDocumento { get; set; }
        public int IntPagoEstado { get; set; }
        public decimal IntValor { get; set; }
        public decimal IntValorIva { get; set; }
        public string StrDescripcionPago { get; set; }
        public string StrCampo1 { get; set; }
        public string StrCampo2 { get; set; }
        public string StrCampo3 { get; set; }
        public string StrClienteTipoId { get; set; }
        public string StrClienteIdentificacion { get; set; }
        public string StrClienteNombre { get; set; }
        public string StrClienteEmail { get; set; }
        public string StrClienteTelefono { get; set; }
        public string StrPagoTicketID { get; set; }
        public string StrPagoTransaccionCUS { get; set; }
        public int IntPagoClicloTransaccion { get; set; }
        public string StrPagoCodBanco { get; set; }
        public string StrPagoDesBanco { get; set; }
        public int IntPagoCodServicio { get; set; }
        public int IntPagoFormaPago { get; set; }
        public string StrPagoCodFranquicia { get; set; }
        public System.DateTime DatFechaVerificacion { get; set; }
        public string StrMensajeVerificacion { get; set; }
        public int IntIdAplicativo { get; set; }
        public string StrRutaProcedencia { get; set; }
        public string StrRutaDestino { get; set; }
        public string StrRutaSync { get; set; }
        public string StrAuthToken { get; set; }
        public string StrAuthIdEmpresa { get; set; }
        public int IntAuthCompania { get; set; }
        public int IntAuthEmpresa { get; set; }
        public bool IntSincronizacion { get; set; }
        public System.DateTime DatFechaSync { get; set; }
        public string StrIdSeguridadPago { get; set; }
        public string StrIdPlataforma { get; set; }
        public System.Guid StrIdSeguridadDoc { get; set; }
        public DateTime DatFechaDocumento { get; set; }
        public DateTime DatFechaVencDocumento { get; set; }
        public string StrPrefijoDocumento { get; set; }
        public long IntNumeroDocumento { get; set; }
        public decimal IntVlrTotalDocumento { get; set; }
        public short IntPasarela { get; set; }
        public string StrRespuesta { get; set; }
        public string StrIdTercero { get; set; }
        public RespuestaLicenciaHappgi Respuesta_Licencia_Happgi { get; set; }
        public RecargaDocsFE recarga_docs_FE { get; set; }
    }

    public class RespuestaTblPasarelaPagosPI
    {
        public string CodigoSeguridad { get; set; }
        public TblPasarelaPagosPI TblPasarelaPagos { get; set; }
    }

}
