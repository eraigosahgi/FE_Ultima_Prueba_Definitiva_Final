using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea
{
	public class VerificaPago
	{
		/// <summary>
		/// Indica si el pago fué exitoso (true)
		/// </summary>
		public bool pago_generado;

		/// <summary>
		/// estado del pago (0: en proceso; 1: proceso)
		/// </summary>
		public bool estado_verificacion;

		/// <summary>
		/// Mensaje
		/// </summary>
		public string mensaje;

		/// <summary>
		/// Pagos
		/// </summary>				
		public List<VerificaPagoDetalle> detalle_pago;

		/// <summary>
		/// Almacena la respuesta en formato Json
		/// </summary>
		public string respuesta;
	}
}
