using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	/*
	{
    "sender": { // información básica del remitente
    "name": "Jesus",
    "surname": "Ramirez",
    "cellPhone": "3013528220",
    "prefix": "+57",
    "email": "alejandraruizsoto51@gmail.com",
    "pickupAddress": "Calle 20 #32b 143", // dirección de recogida
    "nit": "1152456886",
    "nitType": "CC"
  },
  "receiver": {  // información básica del destinatario
    "name": "Maria",
    "surname": "Ruiz",
    "email": "alejandraruizsoto51@gmail.com",
    "prefix": "+57",
    "cellPhone": "3013528220",
    "destinationAddress": "Carrera 70 #32N 143", // dirección destino 
    "nit": "1152456886", // Si el destinatario recoge en punto u oficina de la transportadora, este campo es necesario
    "nitType": "CC" // Si el destinatario recoge en punto u oficina de la transportadora, este campo es necesario
  },
  "productInformation": { // información del producto
    "quantity": 1, // cantidad de unidades con las mismas dimensiones y peso
    "width": 20, // ancho del paquete en cm (número entero)
    "large": 20, // largo del paquete  en cm (número entero)
    "height": 10, // alto del paquete  en cm (número entero)
    "weight": 1, // peso del paquete  en kg (número entero)
    "forbiddenProduct": true, // si no se envían articulos prohibidos el parametro se configura como true
    "productReference": "JEAN A17", //referencia del producto, si tiene
    "declaredValue": 10000 // valor al cuál se quiere asegurar el paquete o producto a enviar
  },
  "locate": {
    "originDaneCode": "11001000", // código DANE de ciudad o municipio origen
    "destinyDaneCode": "11001000" // código DANE de ciudad o municipio destino
  },
  "channel": "Tienda de don pacho", // nombre de la tienda que está consumiendo este servicio 
  "user": "601d508940c88f51340eb765", // id del usuario (no es obligatorio)
  "deliveryCompany": "5ca22d9587981510092322f6", // id de la transportadora
  "description": "JEANS", // descripción del producto 
  "comments": "comentarios adicionales", // comentarios adicionales
  "paymentType": 101, // tipo de pago 101- pago con saldo de mipaquete o 102 - Descontando el envío del recaudo realizado(aplica para pagocontraentrega)
  "valueCollection": 0, // valor a recaudar si el envío es con modalidad de pago contraentrega, si no, se coloca 0
  "requestPickup": false, // si desea solicitar servicio de recolección del paquete en dirección origen
  "adminTransactionData": {
    "saleValue": 110000 //valor de la venta del producto a enviar (aplica para servicio de pago contraentrega, si no se coloca 0)
  }
}*/
	public class createSending
	{
		/// <summary>
		/// Remitente
		/// </summary>
		public sender sender { get; set; }

		/// <summary>
		/// Receptor
		/// </summary>
		public receiver receiver { get; set; }

		/// <summary>
		/// Información de producto
		/// </summary>
		public productInformation productInformation { get; set; }
		public locate locate { get; set; }
		public string channel { get; set; }
		public string deliveryCompany { get; set; }
		public string description { get; set; }
		public string comments { get; set; }
		public decimal paymentType { get; set; }
		public decimal valueCollection { get; set; }
		public bool requestPickup { get; set; }
		public adminTransactionData adminTransactionData { get; set; }
	}

	/// <summary>
	/// Remitente
	/// </summary>
	public class sender
	{
		public string name { get; set; }
		public string surname { get; set; }
		public string cellPhone { get; set; }
		public string prefix { get; set; }
		public string email { get; set; }
		public string pickupAddress { get; set; }
		public string nit { get; set; }
		public string nitType { get; set; }
	}

	/// <summary>
	/// Receptor
	/// </summary>
	public class receiver
	{
		public string name { get; set; }
		public string surname { get; set; }
		public string email { get; set; }
		public string prefix { get; set; }
		public string cellPhone { get; set; }
		public string destinationAddress { get; set; }
		public string nit { get; set; }
		public string nitType { get; set; }
	}

	/// <summary>
	/// Información de producto
	/// </summary>
	public class productInformation
	{
		public decimal quantity { get; set; }
		public decimal width { get; set; }
		public decimal large { get; set; }
		public decimal height { get; set; }
		public decimal weight { get; set; }
		public bool forbiddenProduct { get; set; }

		/// <summary>
		/// Referencia del producto
		/// </summary>
		public string productReference { get; set; }

		/// <summary>
		/// Valor declarado de producto
		/// </summary>
		public decimal declaredValue { get; set; }
	}
	public class locate
	{
		/// <summary>
		/// Código dame origen
		/// </summary>
		public string originDaneCode { get; set; }

		/// <summary>
		/// Código dane destino
		/// </summary>
		public string destinyDaneCode { get; set; }
	}
	public class adminTransactionData
	{
		public decimal saleValue { get; set; }
	}

	public class createSendingResponse
	{
		/*		 {
    "mpCode": 1086244,
    "message": "Envio generado correctamente"
	} */

		/// <summary>
		/// Código de envio
		/// </summary>
		public int mpCode { get; set; }

		/// <summary>
		/// Mensaje respuesta de envío
		/// </summary>
		public string message { get; set; }
	}
}
