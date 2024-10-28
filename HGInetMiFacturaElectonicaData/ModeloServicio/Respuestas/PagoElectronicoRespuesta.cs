using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas
{

	public class PagoElectronicoRespuestaDetalle
	{
		/// <summary>
		/// StrIdRegistro id de seguridad unico de cada pago
		/// </summary>
		public string IdRegistro { get; set; }
		/// <summary>
		/// Fecha de pago
		/// </summary>
		public System.DateTime Fecha { get; set; }
		/// <summary>
		/// Id registrado en plataforma intermedia 9 digitos
		/// </summary>
		public string IdPago { get; set; }
		/// <summary>
		/// Codigo de Referencia de la pasarela de pago
		/// </summary>
		public string ReferenciaCUS { get; set; }
		/// <summary>
		/// Codigo de Ticket de la pasarela de pago
		/// </summary>
		public string TicketID { get; set; }
		/// <summary>
		/// Descripción del estado del pago de la pasarela de pago
		/// </summary>
		public string PagoEstadoDescripcion { get; set; }
		/// <summary>
		/// Código del estado del pago
		/// </summary>
		public int PagoEstado { get; set; }
		/// <summary>
		/// Valor del pago
		/// </summary>
		public decimal Valor { get; set; }
		/// <summary>
		/// Forma de pago
		/// </summary>
		public string FormaPago { get; set; }
		/// <summary>
		/// Franquicia
		/// </summary>
		public string Franquicia { get; set; }
	}


	public class PagoElectronicoRespuesta
	{
		/// <summary>
		/// Id único del documento generado por la Plataforma
		/// </summary>
		public string IdDocumento { get; set; }

		/// <summary>
		/// Identificación adquiriente.
		/// </summary>
		public string Identificacion { get; set; }

		/// <summary>
		/// Número de Resolución asignado por la DIAN.(Aplica para Documento tipo Factura)
		/// </summary>
		public string NumeroResolucion { get; set; }

		/// <summary>
		/// Prefijo de la Factura. (Aplica para Documento tipo Factura)
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Indica el tipo de Documento(1: factura - 2: nota débito - 3: nota crédito)
		/// </summary>
		public int DocumentoTipo { get; set; }

		/// <summary>
		/// Número de Documento
		/// </summary>
		public long Documento { get; set; }

		/// <summary>
		/// Fecha del documento
		/// </summary>
		public DateTime FechaDocumento { get; set; }

		/// <summary>
		/// Código identificador del documento ante la DIAN
		/// </summary>
		public string Cufe { get; set; }

		/// <summary>
		/// Lista de detalles de los pagos electrónicos
		/// </summary>
		public List<PagoElectronicoRespuestaDetalle> DetallesPagos { get; set; }

		/// <summary>
		/// Objeto de tipo Error 
		/// </summary>
		public Error Error { get; set; }

	}

	public class PagoElectronicoRespuestaPorFecha
	{
		/// <summary>
		/// Id único del documento generado por la Plataforma
		/// </summary>
		public string IdDocumento { get; set; }

		/// <summary>
		/// Identificación adquiriente.
		/// </summary>
		public string Identificacion { get; set; }

		/// <summary>
		/// Indica el tipo de Documento(1: factura - 2: nota débito - 3: nota crédito)
		/// </summary>
		public int DocumentoTipo { get; set; }

		/// <summary>
		/// Número de Documento
		/// </summary>
		public long Documento { get; set; }	

		/// <summary>
		/// Código identificador del documento ante la DIAN
		/// </summary>
		public string Cufe { get; set; }

		/// <summary>
		/// StrIdRegistro id de seguridad unico de cada pago
		/// </summary>
		public string IdRegistro { get; set; }
		/// <summary>
		/// Fecha de pago
		/// </summary>
		public System.DateTime Fecha { get; set; }
		/// <summary>
		/// Id registrado en plataforma intermedia 9 digitos
		/// </summary>
		public string IdPago { get; set; }
		/// <summary>
		/// Codigo de Referencia de la pasarela de pago
		/// </summary>
		public string ReferenciaCUS { get; set; }
		/// <summary>
		/// Codigo de Ticket de la pasarela de pago
		/// </summary>
		public string TicketID { get; set; }
		/// <summary>
		/// Descripción del estado del pago de la pasarela de pago
		/// </summary>
		public string PagoEstadoDescripcion { get; set; }
		/// <summary>
		/// Código del estado del pago
		/// </summary>
		public int PagoEstado { get; set; }
		/// <summary>
		/// Valor del pago
		/// </summary>
		public decimal Valor { get; set; }
		/// <summary>
		/// Forma de pago
		/// </summary>
		public string FormaPago { get; set; }
		/// <summary>
		/// Franquicia
		/// </summary>
		public string Franquicia { get; set; }


		/// <summary>
		/// Objeto de tipo Error 
		/// </summary>
		public Error Error { get; set; }

	}


	public class PagoElectronicoRespuestaAgrupadoPorFecha
	{
		/// <summary>
		/// Fecha de pago
		/// </summary>
		public DateTime Fecha { get; set; }
		/// <summary>
		/// Ciclo de la transacción (PSE)
		/// </summary>
		public int Ciclo { get; set; }
		/// <summary>
		/// Forma de pago
		/// </summary>
		public int FormaPago { get; set; }
		/// <summary>
		/// Valor del pago
		/// </summary>
		public decimal Valor { get; set; }
		public List<PagoElectronicoRespuestaPorFecha> Pago_Electronico_Respuesta_Por_Fecha { get; set; }

	}
}
