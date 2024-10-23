using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class RespuestaHGIPagos
	{
		public string FechaRegistro { get; set; }
		public string FechaDocumento { get; set; }
		public string FechaConfirmacion { get; set; }
		public string IdSeguridad { get; set; }
		public string TicketId { get; set; }
		public string TransaccionCUS { get; set; }
		public string Estado { get; set; }
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
		public string Valor { get; set; }
		public string MetodoPago { get; set; }
		public string ValorIva { get; set; }
		public string Documento { get; set; }
		public string Ciclo { get; set; }
		public string CodAprobacion { get; set; }

		public string StrClienteIdentificacion { get; set; }
		public string StrClienteNombre { get; set; }
		public string StrClienteEmail { get; set; }
		public string StrClienteTelefono { get; set; }

	}
}
