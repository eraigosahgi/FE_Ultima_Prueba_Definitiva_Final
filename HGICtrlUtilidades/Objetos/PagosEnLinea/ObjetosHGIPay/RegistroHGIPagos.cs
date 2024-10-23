using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{	
	public class RegistroHGIPagos
	{
		//Identificación del cliente.
		public string client { get; set; }
		public string ComercioHGIpay { get; set; }
		public Guid payment_identifier { get; set; }
		//Codigo del comercio 
		public string datakey { get; set; }
		//Numero de documento o Factura del Tercero
		public int document { get; set; }
		//Correo
		public string email { get; set; }
		//Metodo de pago, por ahora solo es pse
		public string method { get; set; }
		//Pais
		public int country { get; set; }
		//Telefono del cliente
		public string mobile_phone { get; set; }
		//Moneda
		public string money { get; set; }
		//Monto de la orden de compra. 
		public string amount { get; set; }
		//Descripción del pago
		public string description { get; set; }
		//public string expiration { get; set; }
		public string iva { get; set; }
		//Nombre de Usuario
		public string name { get; set; }
		//Tipo de documento de identidad del comprador. Máx. 3 caracteres
		public string user_di { get; set; }
		//Identificacion del cliente
		public string di { get; set; }
		//Tipo de persona que realiza la compra. "N" natural o "J" jurídica
		public string type_person { get; set; }
		//Referencias
		public string reference_number1 { get; set; }
		//Referencias 2
		public string reference_number2 { get; set; }
		//Referencias 3
		public string reference_number3 { get; set; }
		//Url de actualizacion de Pago
		public string url_confirmation { get; set; }
		//Url de regreso al comercio
		public string url_response { get; set; }
	}

	public class RegistroHGIPagos_Respuesta
	{
		public string IdSeguridad { get; set; }
		public int TicketId { get; set; }
		//public string TransaccionCUS { get; set; }
		public string Estado { get; set; }
		//public string Referencia1 { get; set; }
		public string url_response { get; set; }
	}

	public class ResultadoPrevioRegistroHGIPasarelaPagos
	{
		public string CODE { get; set; }
		public string DESC { get; set; }
		public RegistroHGIPagos_Respuesta DATA { get; set; }
	}
}
