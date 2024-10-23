using HGInetZonaPagos.Objetos;
using HGInetZonaPagos.ZonaVirtualServicioVerfica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetZonaPagos.Clases
{
    public class VerificarPago
    {
        /// <summary>
        /// Identificador único del comercio. 
        /// Este valor es suministrado por Zona Pagos.
        /// </summary>
        private int id_tienda;

        /// <summary>
        /// Clave de validación entre el comercio E:\Desarrollo\jflores\Proyectos\HGINetMiFacturaElectronica\Codigo\HGInetZonaPagos\Clases\VerificarPago.csy Zona Pagos.
        /// </summary>
        private string clave;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="id_tienda">Identificador único del comercio. Este valor es suministrado por Zona Virtual.</param>
        /// <param name="clave">Clave de validación entre el comercio y ZonaPAGOS.</param>
        public VerificarPago(int id_tienda, string clave)
        {
            this.id_tienda = id_tienda;
            this.clave = clave;
        }

        /// <summary>
        /// Verifica el pago en Zona Pagos
        /// </summary>
        /// <param name="id_pago">id de pago en Zona Virtual</param>
        /// <returns>datos de respuesta</returns>
        public VerificaPago Verificar(string id_pago)
        {
            try
            {
                // Información de envio
                verificar_pago_v3Request datos_peticion = new verificar_pago_v3Request();

                // datos de la tienda
                datos_peticion.int_id_tienda = this.id_tienda;
                datos_peticion.str_id_clave = this.clave;

                // datos del pago
                datos_peticion.str_id_pago = id_pago;

                // Obtiene o establece el protocolo de seguridad usado por los objetos ServicePoint administrados por el objeto ServicePointManager.
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;

                ServiceSoapClient _cliente = new ServiceSoapClient();

                // ejecutar petición y obtener respuesta
                verificar_pago_v3Response respuesta = _cliente.verificar_pago_v3(datos_peticion);

                /*
				  respuesta.verificar_pago_v3Result
				  1: Se encontraron pagos
				  0: Pago Rechazado o nunca iniciado en Entidad Financiera.
				 -1: Error de Clave o Comercio Inválido. - Error no identificado.
				 */

                VerificaPago verificacion = new VerificaPago();
                verificacion.pago_generado = false;

                List<VerificaPagoDetalle> lista = new List<VerificaPagoDetalle>();

                string mensaje_respuesta = string.Empty;
                string forma_pago = string.Empty;

                if (respuesta.res_pagos_v3.Count() > 0)
                {
                    foreach (var item in respuesta.res_pagos_v3)
                    {
                        VerificaPagoDetalle detalle = new VerificaPagoDetalle();
                        detalle.campo1 = item.str_campo1;
                        detalle.campo2 = item.str_campo2;
                        detalle.campo3 = item.str_campo3;
                        detalle.ciclo_transaccion = item.int_ciclo_transaccion;
                        detalle.codigo_banco = item.int_codigo_banco;
                        detalle.codigo_servicio = item.int_codigo_servico;
                        detalle.codigo_transaccion = item.str_codigo_transaccion;

                        /*
						 Pago Exitoso = 1
						 Pago Pendiente en Entidad Financiera = 999
						 Pago Pendiente por iniciar en Entidad Financiera = 888
						 */
                        detalle.estado_pago = item.int_estado_pago;

                        detalle.fecha = item.dat_fecha;

                        detalle.franquicia_codigo = item.str_franquicia;

                        detalle.id_clave = item.str_id_clave;
                        detalle.id_cliente = item.str_id_cliente;

                        detalle.forma_pago_codigo = item.int_id_forma_pago;

                        detalle.id_pago = item.str_id_pago;
                        detalle.nombre_banco = item.str_nombre_banco;
                        detalle.ticketID = item.str_ticketID;
                        detalle.valor_pagado = item.dbl_valor_pagado;

                        // Pago Exitoso
                        if (respuesta.int_error == 0 && item.int_estado_pago == 1)
                        {
                            mensaje_respuesta = string.Format("Pago Exitoso.");
                            verificacion.pago_generado = true;
                            verificacion.estado_verificacion = true;
                        }
                        // Pago Pendiente en Entidad Financiera
                        else if (respuesta.int_error == 0 && item.int_estado_pago == 999)
                        {
                            mensaje_respuesta = string.Format("Pago Pendiente por Finalizar en Entidad Financiera.");
                        }
                        // Pago Pendiente por Iniciar en Entidad Financiera
                        else if (respuesta.int_error == 0 && item.int_estado_pago == 888)
                        {
                            mensaje_respuesta = string.Format("Pago Pendiente por Iniciar en Entidad Financiera.");
                        }

                        detalle.estado_pago_nombre = mensaje_respuesta;

                        lista.Add(detalle);
                    }
                }
                else
                {
                    // Pago Rechazado o Nunca Iniciado en la Entidad Financiera
                    if (respuesta.verificar_pago_v3Result == 0 && respuesta.int_error == 1)
                    {
                        mensaje_respuesta = string.Format("Pago Rechazado o Nunca Iniciado en la Entidad Financiera.");
                        verificacion.estado_verificacion = true;
                    }
                    // Error de Clave o Comercio Inválido
                    else if (respuesta.verificar_pago_v3Result == -1 && respuesta.int_error == 2)
                    {
                        mensaje_respuesta = string.Format("Error de Clave o Comercio Inválido.");
                        verificacion.estado_verificacion = true;
                    }
                    // Error no identificado
                    else if (respuesta.verificar_pago_v3Result == -1 && respuesta.int_error == -1)
                    {
                        mensaje_respuesta = string.Format("Error no identificado.");
                        verificacion.estado_verificacion = true;
                    }
                }

                verificacion.mensaje = mensaje_respuesta;
                verificacion.detalle_pago = lista;

                return verificacion;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException("Error al procesar el pago en Zona de Pagos", excepcion);
            }

        }


    }
}
