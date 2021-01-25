using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFeAPI
{
	public class Ctl_PagosElectronicos
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/pagoselectronicos.svc";

		/// <summary>
		/// Permite consultar el resultado de los documentos enviados a la plataforma por el Facturador Electrónico
		/// Manual Técnico: 5.2.2 Metodo Web: Consulta por Código(s) de Registro
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="CodigosDocumentos">còdigos de documentos del Facturador Electrónico para consulta separados por el caracter coma (,)</param>
		/// <param name="DocumentoTipo">indica el tipo de documento de consulta 1: Factura - 2: Nota Débito - 3: Nota Crédito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioPagosElectronicos.PagoElectronicoRespuesta> ObtenerPorCodigoRegistro(string UrlWs, string Serial, string Identificacion, string CodigosDocumentos)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioPagosElectronicos.PagoElectronicoRespuesta> datos = new List<ServicioPagosElectronicos.PagoElectronicoRespuesta>();

			ServicioPagosElectronicos.ServicioPagosElectronicosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioPagosElectronicos.ServicioPagosElectronicosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioPagosElectronicos.ConsultaPorCodigoRegistroRequest peticion = new ServicioPagosElectronicos.ConsultaPorCodigoRegistroRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					CodigosRegistros = CodigosDocumentos
				};

				// ejecución del servicio web
				ServicioPagosElectronicos.ConsultaPorCodigoRegistroResponse respuesta = cliente_ws.ConsultaPorCodigoRegistro(peticion);

				// resultado del servicio web
				List<ServicioPagosElectronicos.PagoElectronicoRespuesta> result = respuesta.ConsultaPorCodigoRegistroResult;

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
		/// Permite consultar el resultado de los pagos en la plataforma por el Facturador Electrónico
		/// Manual Técnico: 5.2.2 Metodo Web: Consulta por Fecha de Elboración
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="FechaInicial">Fecha Inicial</param>
		/// <param name="FechaFinal">Fecha Final</param>
		/// <param name="Procesados"></param>
		/// <returns>indica el Pago fue procesado o no: 0 Todos Los Pagos, 1 Pagos Procesados, 2 Pagos que aun no se han Procesado</returns>
		public static List<ServicioPagosElectronicos.PagoElectronicoRespuesta> ConsultaPorFechaElaboracion(string UrlWs, string Serial, string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioPagosElectronicos.PagoElectronicoRespuesta> datos = new List<ServicioPagosElectronicos.PagoElectronicoRespuesta>();

			ServicioPagosElectronicos.ServicioPagosElectronicosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioPagosElectronicos.ServicioPagosElectronicosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioPagosElectronicos.ConsultaPorFechaElaboracionRequest peticion = new ServicioPagosElectronicos.ConsultaPorFechaElaboracionRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					FechaInicial = FechaInicial,
					FechaFinal = FechaFinal,
					Procesados=Procesados
				};

				// ejecución del servicio web
				ServicioPagosElectronicos.ConsultaPorFechaElaboracionResponse respuesta = cliente_ws.ConsultaPorFechaElaboracion(peticion);

				// resultado del servicio web
				List<ServicioPagosElectronicos.PagoElectronicoRespuesta> result = respuesta.ConsultaPorFechaElaboracionResult;

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
		/// Permite actualizar el estado de los pagos a procesado y retornar el detalle del pago con el nuevo estado
		/// Manual Técnico: 5.2.2 Metodo Web: Consulta por Fecha de Elboración
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="CodigosPagos">còdigos de pagos separados por el caracter coma (,), campo IdRegistro del objeto PagoElectronicoRespuestaDetalle</param>		
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioPagosElectronicos.PagoElectronicoRespuestaDetalle> ActualizarEstadoPago(string UrlWs, string Serial, string Identificacion, string CodigosPagos)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioPagosElectronicos.PagoElectronicoRespuesta> datos = new List<ServicioPagosElectronicos.PagoElectronicoRespuesta>();

			ServicioPagosElectronicos.ServicioPagosElectronicosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioPagosElectronicos.ServicioPagosElectronicosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioPagosElectronicos.ActualizarEstadoPagoRequest peticion = new ServicioPagosElectronicos.ActualizarEstadoPagoRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					CodigosRegistros = CodigosPagos
				};

				// ejecución del servicio web
				ServicioPagosElectronicos.ActualizarEstadoPagoResponse respuesta = cliente_ws.ActualizarEstadoPago(peticion);

				// resultado del servicio web
				List<ServicioPagosElectronicos.PagoElectronicoRespuestaDetalle> result = respuesta.ActualizarEstadoPagoResult;

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

	}
}
