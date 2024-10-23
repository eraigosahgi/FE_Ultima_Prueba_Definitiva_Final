using HGInetZonaPagos.ZonaVirtualServicioPagos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetZonaPagos.Clases
{
	public class ProcesoPago
	{
		/// <summary>
		/// Identificador único del comercio. 
		/// Este valor es suministrado por Zona Pagos.
		/// </summary>
		private int id_tienda;

		/// <summary>
		/// Clave de validación entre el comercio y Zona Pagos.
		/// </summary>
		private string clave;

		/// <summary>
		/// Código del comercio para la ruta web de pago en Zona Pagos.
		/// </summary>
		private string codigo_ruta;

		/// <summary>
		/// Ruta web para re direccionar al cliente que desea realizar el pago
		/// </summary>
		/// <param name="id_pago"></param>
		/// <returns></returns>
		private string ObtenerRutaPago(string id_pago, bool demo = false)
		{
            if(demo == false)
			    return string.Format("https://www.zonapagos.com/{0}/pago.asp?estado_pago=iniciar_pago&identificador={1}", this.codigo_ruta, id_pago);
            else
                return string.Format("https://www.zonapagosdemo.com/{0}/pago.asp?estado_pago=iniciar_pago&identificador={1}", this.codigo_ruta, id_pago);
        }
        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="id_tienda">Identificador único del comercio. Este valor es suministrado por Zona Virtual.</param>
        /// <param name="clave">Clave de validación entre el comercio y ZonaPAGOS.</param>
        public ProcesoPago(int id_tienda, string clave, string codigo_ruta)
		{
			this.id_tienda = id_tienda;
			this.clave = clave;
			this.codigo_ruta = codigo_ruta;
		}


        /// <summary>
        /// Reporta el pago en Zona Pagos
        /// </summary>
        /// <param name="datos_cliente">información del cliente para Zona Pagos</param>
        /// <param name="datos_pago">información del pago para Zona Pagos</param>
        /// <param name="demo">indica si el proceso de pago es de demostración</param>
        /// <returns>Ruta para redireccionar y realizar el pago en Zona Pagos</returns>
        public string ReportarPago(Cliente datos_cliente, Pago datos_pago, bool demo = false)
		{
			try
			{
				// Información de envio
				inicio_pagoV2RequestBody datos_peticion = new inicio_pagoV2RequestBody();

				// datos de la tienda
				datos_peticion.id_tienda = this.id_tienda;
				datos_peticion.clave = this.clave;

				// datos del cliente
				datos_peticion.id_cliente = datos_cliente.id_cliente;
				datos_peticion.tipo_id = datos_cliente.tipo_id;
				datos_peticion.nombre_cliente = datos_cliente.nombre;
				datos_peticion.apellido_cliente = datos_cliente.apellido;
				datos_peticion.telefono_cliente = datos_cliente.telefono;
				datos_peticion.email = datos_cliente.email;

				// datos del pago
				datos_peticion.id_pago = datos_pago.id_pago;
				datos_peticion.descripcion_pago = datos_pago.descripcion_pago;
				datos_peticion.total_con_iva = datos_pago.total_con_iva;
				datos_peticion.valor_iva = datos_pago.valor_iva;
				datos_peticion.info_opcional1 = datos_pago.info_opcional1;
				datos_peticion.info_opcional2 = datos_pago.info_opcional2;
				datos_peticion.info_opcional3 = datos_pago.info_opcional3;
				datos_peticion.codigo_servicio_principal = datos_pago.codigo_servicio_principal;
				datos_peticion.lista_codigos_servicio_multicredito = Pago.ConvertirArrayString(datos_pago.lista_codigos_servicio_multicredito);
				datos_peticion.lista_nit_codigos_servicio_multicredito = Pago.ConvertirArrayString(datos_pago.lista_nit_codigos_servicio_multicredito);
				datos_peticion.lista_valores_con_iva = Pago.ConvertirArrayDouble(datos_pago.lista_valores_con_iva);
				datos_peticion.lista_valores_iva = Pago.ConvertirArrayDouble(datos_pago.lista_valores_iva);
				datos_peticion.total_codigos_servicio = datos_pago.total_codigos_servicio;

                //Obtiene o establece el protocolo de seguridad usado por los objetos ServicePoint administrados por el objeto ServicePointManager.
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

                // crear petición
                inicio_pagoV2Request peticion = new inicio_pagoV2Request(datos_peticion);

				ZPagosSoapClient _cliente = new ZPagosSoapClient();

				/*
					Si el consumo y el inicio de la transacción fue exitosa, se devuelve un cadena que contiene un
					número entero mayor 0, el cual es el identificador de la transacción el cual se debe utilizar para
					que el comercio proceda a re direccionar al cliente que desea realizar el pago a la ruta: 
				 */

				// ejecutar petición y obtener respuesta
				inicio_pagoV2Response respuesta = _cliente.inicio_pagoV2(peticion);

				//if(respuesta.Body.inicio_pagoV2Result)

				// retorna la ruta para realizar el pago
                return ObtenerRutaPago(respuesta.Body.inicio_pagoV2Result.ToString(), demo);

            }
			catch (Exception excepcion)
			{
				throw;
			}

		}

	}
}
