using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace HGInetFacturaEServicios
{
	/// <summary>
	/// Controlador para el envío de objetos de Nota Crédito
	/// </summary>
	public class Ctl_NotaCredito
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/notacredito.svc";

		/// <summary>
		/// Permite enviar los documentos de tipo NotaCredito por el Facturador Electrónico
		/// Manual Técnico: 5.1.2 Metodo Web: Crear Nota Crédito
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo NotaCredito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioNotaCredito.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioNotaCredito.NotaCredito> documentos_envio)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioNotaCredito.NotaCredito> datos = new List<ServicioNotaCredito.NotaCredito>();

			// conexión cliente para el servicio web
			ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				foreach (ServicioNotaCredito.NotaCredito item in documentos_envio)
				{
					if (item == null)
						throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, documentos_envio, "ServicioNotaCredito.NotaCredito"));

					if (item.DocumentoDetalles == null || !item.DocumentoDetalles.Any())
						throw new Exception("El detalle del documento es inválido.");

					item.DataKey = dataKey;
				}

				// datos para la petición
				ServicioNotaCredito.RecepcionRequest peticion = new ServicioNotaCredito.RecepcionRequest()
				{
					documentos = documentos_envio
				};

				// ejecución del servicio web
				ServicioNotaCredito.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				List<ServicioNotaCredito.DocumentoRespuesta> result = respuesta.RecepcionResult;

				if (respuesta != null)
					return result;
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");

			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}

		}

		/// <summary>
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="FechaInicio">fecha inicial de consulta</param>
		/// <param name="FechaFin">fecha final de consulta</param>
		/// <returns>Una lista de las Nota Credito generadas a nombre del adquiriente</returns>
		public static List<ServicioNotaCredito.NotaCreditoConsulta> ObtenerNotaCreditoPorAdquiriente(string UrlWs, string Serial, string Identificacion, DateTime FechaInicio, DateTime FechaFin)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			if (FechaInicio == null)
				throw new ApplicationException("Fecha inicial inválida.");
			if (FechaFin == null)
				throw new ApplicationException("Fecha final inválida.");

			if (FechaFin < FechaInicio)
				throw new ApplicationException("Fecha final debe ser mayor o igual que la fecha inicial.");

			List<ServicioNotaCredito.NotaCreditoConsulta> datos = new List<ServicioNotaCredito.NotaCreditoConsulta>();

			// conexión cliente para el servicio web
			ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioNotaCredito.ObtenerPorFechasAdquirienteRequest peticion = new ServicioNotaCredito.ObtenerPorFechasAdquirienteRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					FechaInicio = FechaInicio,
					FechaFinal = FechaFin,
				};

				// ejecución del servicio web
				ServicioNotaCredito.ObtenerPorFechasAdquirienteResponse respuesta = cliente_ws.ObtenerPorFechasAdquiriente(peticion);

				// resultado del servicio web
				List<ServicioNotaCredito.NotaCreditoConsulta> result = respuesta.ObtenerPorFechasAdquirienteResult;

				if (respuesta != null)
					return result;
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");

			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}
		}


        /// <summary>
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="CodigosDocumentos">còdigos de documentos del Facturador Electrónico para consulta separados por el caracter coma (,)</param>
		/// <returns>Una lista de las Notas Credito generadas a nombre del adquiriente</returns>
		public static List<ServicioNotaCredito.NotaCreditoConsulta> ObtenerPorIdSeguridadAdquiriente(string UrlWs, string Serial, string Identificacion, string CodigosDocumentos)
        {
            // valida la URL del servicio web
            UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

            // valida el parámetro Serial
            if (string.IsNullOrEmpty(Serial))
                throw new ApplicationException("Parámetro Serial de tipo string inválido.");

            // valida el parámetro Identificacion
            if (string.IsNullOrEmpty(Identificacion))
                throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

            // valida el parámetro Identificacion
            if (string.IsNullOrEmpty(CodigosDocumentos))
                throw new ApplicationException("Parámetro CodigosDocumentos de tipo string inválido.");

            List<ServicioNotaCredito.NotaCreditoConsulta> datos = new List<ServicioNotaCredito.NotaCreditoConsulta>();

            // conexión cliente para el servicio web
            ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient();
            cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

            try
            {
                // configura la cadena de autenticación para la ejecución del servicio web en SHA1
                string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

                // datos para la petición
                ServicioNotaCredito.ObtenerPorIdSeguridadAdquirienteRequest peticion = new ServicioNotaCredito.ObtenerPorIdSeguridadAdquirienteRequest()
                {
                    DataKey = dataKey,
                    Identificacion = Identificacion,
                    CodigosRegistros = CodigosDocumentos,
                };

                // ejecución del servicio web
                ServicioNotaCredito.ObtenerPorIdSeguridadAdquirienteResponse respuesta = cliente_ws.ObtenerPorIdSeguridadAdquiriente(peticion);

                // resultado del servicio web
                List<ServicioNotaCredito.NotaCreditoConsulta> result = respuesta.ObtenerPorIdSeguridadAdquirienteResult;

                if (respuesta != null)
                    return result;
                else
                    throw new Exception("Error al obtener los datos con los parámetros indicados.");

            }
            catch (FaultException excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion);
            }
            catch (CommunicationException excepcion)
            {
                throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
            }
            catch (Exception excepcion)
            {
                throw excepcion;
            }
            finally
            {
                if (cliente_ws != null)
                    cliente_ws.Abort();
            }
        }


        /// <summary>
        /// Prueba del servicio web de la plataforma de Facturación Electrónica
        /// </summary>
        /// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
        /// <returns></returns>
        public static string Test(string UrlWs)
		{   // valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// conexión cliente para el servicio web
			ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// datos para la petición
				ServicioNotaCredito.TestRequest peticion = new ServicioNotaCredito.TestRequest();

				// ejecución del servicio web
				ServicioNotaCredito.TestResponse respuesta = cliente_ws.Test(peticion);

				// resultado del servicio web
				string result = respuesta.TestResult;

				if (respuesta != null)
					return result;
				else
					throw new Exception("Error al obtener los datos con los parámetros indicados.");
			}
			catch (FaultException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion);
			}
			catch (CommunicationException excepcion)
			{
				throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (cliente_ws != null)
					cliente_ws.Abort();
			}
		}

		/// <summary>
		/// Calcula el código CUFE de la nota crédito
		/// </summary>
		/// <param name="clave_tecnica">Clave técnica de la resolución de la factura</param>
		/// <param name="cufe_factura">Cufe de la factura</param>
		/// <param name="numero_nota_credito">Número de la nota crédito</param>
		/// <param name="fecha_nota_credito">Fecha de la nota crédito</param>
		/// <param name="nit_facturador">Número de identificación del facturador electrónico</param>
		/// <param name="tipo_identificacion_adquiriente">Tipo de identificación del facturador electrónico</param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la nota crédito</param>
		/// <param name="subtotal">Subtotal de la nota crédito</param>
		/// <param name="iva">Iva de la nota crédito</param>
		/// <param name="impto_consumo">Impuesto al consumo de la nota crédito</param>
		/// <param name="rte_ica">Retención del ICA de la nota crédito</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CalcularCUFE(string clave_tecnica, string cufe_factura,string prefijo , string numero_nota_credito, DateTime fecha_nota_credito, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		{
			try
			{
				string cufe_encriptado = Ctl_CalculoCufe.CufeNotaCredito(clave_tecnica, cufe_factura, prefijo, numero_nota_credito, fecha_nota_credito, nit_facturador, tipo_identificacion_adquiriente, nit_adquiriente, total, subtotal, iva, impto_consumo, rte_ica, true);
				return cufe_encriptado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Calcula el código CUFE de la nota crédito
		/// </summary>
		/// <param name="pin_software">Pin del software registrado en el catalogo del participante, el cual no esta expresado en el XML</param>
		/// <param name="numero_nota_credito">Número de la nota crédito</param>
		/// <param name="fecha_nota_credito">Fecha y Hora de la nota crédito en hora Colombiana</param>
		/// <param name="nit_facturador">Número de identificación del facturador electrónico</param>
		/// <param name="ambiente">Ambiente a donde se va enviar el documento </param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la nota crédito</param>
		/// <param name="subtotal">Subtotal de la nota crédito</param>
		/// <param name="iva">Iva de la nota crédito</param>
		/// <param name="impto_consumo">Impuesto al consumo de la nota crédito</param>
		/// <param name="rte_ica">Retención del ICA de la nota crédito</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CalcularCUFEV2(string pin_software, string prefijo, string numero_nota_credito, DateTime fecha_nota_credito, string nit_facturador, string ambiente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		{
			try
			{
				if (string.IsNullOrEmpty(ambiente))
					throw new Exception("Ambiente de Envío del documento no valido");

				if (string.IsNullOrEmpty(pin_software))
					throw new Exception("Pin del Software no valido");

				string fecha = fecha_nota_credito.AddHours(5).ToString(Fecha.formato_fecha_hora_zona);
				string cufe_encriptado = Ctl_CalculoCufe.CufeNotaCreditoV2(pin_software, prefijo, numero_nota_credito, fecha, nit_facturador, ambiente, nit_adquiriente, total, subtotal, iva, impto_consumo, rte_ica, true);
				return cufe_encriptado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



	}
}
