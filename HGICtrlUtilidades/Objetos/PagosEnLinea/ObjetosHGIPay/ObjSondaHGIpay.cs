using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
    public class ObjSondaHGIpay
    {
        public string client { get; set; }
        public string payment_identifier { get; set; }
        public int status { get; set; }
        public string checksum { get; set; }
    }

    public class ObjResumenPago
    {
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaDocumento { get; set; }
        DateTime FechaConfirmacion { get; set; }
        public Guid IdSeguridad { get; set; }
        public Int32 TicketId { get; set; }
        public string TransaccionCUS { get; set; }
        public Int32 Estado { get; set; }
        public string CodigoBanco { get; set; }
        public string Banco { get; set; }
        public string Descripcion { get; set; }
        public string Franquicia { get; set; }
        public string MensajeVerificacion { get; set; }
        public string Referencia1 { get; set; }
        public string Referencia2 { get; set; }
        public string Referencia3 { get; set; }
        public string RutaDestino { get; set; }
        public string RutaSync { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPago { get; set; }
        public decimal ValorIva { get; set; }
        public int Documento { get; set; }
        public Int32 Ciclo { get; set; }
        public string CodAprobacion { get; set; }
        public Guid payment_identifier { get; set; }
        public string StrClienteIdentificacion { get; set; }
        public string StrClienteNombre { get; set; }
        public string StrClienteEmail { get; set; }
        public string StrClienteTelefono { get; set; }

    }



}
