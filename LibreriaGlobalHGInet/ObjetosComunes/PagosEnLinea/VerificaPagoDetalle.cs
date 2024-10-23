using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea
{
	public class VerificaPagoDetalle
	{
		/// <summary>
		/// identificador único del comercio
		/// </summary>
		public int id_comercio;

		/// <summary>
		/// fecha en la cual se realiza le pago
		/// </summary>
		public DateTime fecha;

		/// <summary>
		/// identificador de pago
		/// </summary>
		public string id_pago;

		/// <summary>
		/// Mensaje del estado del pago
		/// </summary>
		public string estado_pago_nombre;

		/// <summary>
		/// se envía 1 si el pago fue exitoso, 0 si el pago no se pudo realizar
		/// </summary>
		public int estado_pago;

		/// <summary>
		/// Forma de pago
		/// </summary>
		public string forma_pago_nombre;

		private int _forma_pago_codigo;
		/// <summary>
		/// se envía 29 si el pago se realizó por banco ACH PSE, 31 y 32 si el pago se realizó por tarjeta de crédito
		/// </summary>
		public int forma_pago_codigo
		{
			get { return _forma_pago_codigo; }
			set
			{
				_forma_pago_codigo = value;

				switch (value)
				{
					case 29: forma_pago_nombre = "Pago PSE, débito desde su cuenta corriente o de ahorro."; break;

					case 31: forma_pago_nombre = "Pago realizado por tarjeta de crédito en línea."; break;

					case 32: forma_pago_nombre = "Pago realizado por tarjeta de crédito en línea."; break;

					default: forma_pago_nombre = "No definida."; break;
				}
			}
		}

		/// <summary>
		/// valor que se pagó en el banco
		/// </summary>
		public double valor_pagado;

		/// <summary>
		/// odentifcador de pago ante el banco
		/// </summary>
		public string ticketID;

		/// <summary>
		/// clave que se utiliza para verificar que el llamado sea desde el servidor indicado.
		/// </summary>
		public string id_clave;

		/// <summary>
		/// cliente que inició el pago
		/// </summary>
		public string id_cliente;

		/// <summary>
		/// Franquicia
		/// </summary>
		public string franquicia_nombre;


		private string _franquicia_codigo;
		/// <summary>
		/// Si el pago se realizó con tarjeta de crédito en línea, se envía el código de la franquicia usada por el cliente, de la siguiente manera:
		/// CR_CR : Credencial
		/// CR_DN : Dinners
		/// CR_AM : American Express
		/// CR_VS : Visa
		/// RM_MC : Master Card
		/// </summary>
		public string franquicia_codigo
		{
			get { return _franquicia_codigo; }
			set
			{
				_franquicia_codigo = value;

				switch (value)
				{
					case "CR_CR": franquicia_nombre = "Credencial"; break;

					case "CR_DN": franquicia_nombre = "Dinners"; break;

					case "CR_AM": franquicia_nombre = "American Express"; break;

					case "CR_VS": franquicia_nombre = "Visa"; break;

					case "RM_MC": franquicia_nombre = "Master Card"; break;

					default: franquicia_nombre = "No definida."; break;
				}
			}
		}

		/// <summary>
		/// retorna el código del servicio con el que se inició la transacción
		/// aplica solo para medio de pago PSE
		/// </summary>
		public int codigo_servicio;

		/// <summary>
		/// retorna el código del banco donde se inició el pago
		/// </summary>
		public int codigo_banco;

		/// <summary>
		/// retorna el nombre del banco donde se inició el pago
		/// </summary>
		public string nombre_banco;

		/// <summary>
		/// retorna el código de transacción CUS
		/// </summary>
		public string codigo_transaccion;

		/// <summary>
		/// retorna el ciclo de la transacción
		/// </summary>
		public int ciclo_transaccion;

		/// <summary>
		/// campo opcional 1
		/// </summary>
		public string campo1;

		/// <summary>
		/// campo opcional 2
		/// </summary>
		public string campo2;

		/// <summary>
		/// capo opciona 3
		/// </summary>
		public string campo3;

	}
}
