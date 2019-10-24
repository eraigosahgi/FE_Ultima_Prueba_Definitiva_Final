using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HGInetFeAPI
{
	/// <summary>
	/// Controlador para la gestión de Documentos
	/// </summary>
	public class Ctl_Documentos
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/documentos.svc";

		/// <summary>
		/// Permite consultar el resultado de los documentos enviados a la plataforma por el Facturador Electrónico
		/// Manual Técnico: 5.2.1 Metodo Web: Consulta por Número(s) de Documento 
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="NumerosDocumentos">números de documentos para consulta separados por el caracter coma (,)</param>
		/// <param name="DocumentoTipo">indica el tipo de documento de consulta 1: Factura - 2: Nota Débito - 3: Nota Crédito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> ObtenerPorNumeros(string UrlWs, string Serial, string Identificacion, string NumerosDocumentos, int DocumentoTipo)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();

			ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioDocumento.ConsultaPorNumerosRequest peticion = new ServicioDocumento.ConsultaPorNumerosRequest()
				{	DataKey = dataKey,
					Identificacion = Identificacion,
					Numeros = NumerosDocumentos,
					TipoDocumento = DocumentoTipo
				};

				// ejecución del servicio web
				ServicioDocumento.ConsultaPorNumerosResponse respuesta = cliente_ws.ConsultaPorNumeros(peticion);

				// resultado del servicio web
				List<ServicioDocumento.DocumentoRespuesta> result = respuesta.ConsultaPorNumerosResult;

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
		/// Permite consultar el resultado de los documentos enviados a la plataforma por el Facturador Electrónico
		/// Manual Técnico: 5.2.2 Metodo Web: Consulta por Código(s) de Registro
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="CodigosDocumentos">còdigos de documentos del Facturador Electrónico para consulta separados por el caracter coma (,)</param>
		/// <param name="DocumentoTipo">indica el tipo de documento de consulta 1: Factura - 2: Nota Débito - 3: Nota Crédito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> ObtenerPorCodigoRegistro(string UrlWs, string Serial, string Identificacion, string CodigosDocumentos, int DocumentoTipo)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();
			
			ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioDocumento.ConsultaPorCodigoRegistroRequest peticion = new ServicioDocumento.ConsultaPorCodigoRegistroRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					CodigosRegistros = CodigosDocumentos,
					TipoDocumento = DocumentoTipo
				};

				// ejecución del servicio web
				ServicioDocumento.ConsultaPorCodigoRegistroResponse respuesta = cliente_ws.ConsultaPorCodigoRegistro(peticion);

				// resultado del servicio web
				List<ServicioDocumento.DocumentoRespuesta> result = respuesta.ConsultaPorCodigoRegistroResult;

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
		/// Permite consultar el resultado de los documentos enviados a la plataforma por el Facturador Electrónico
		/// Manual Técnico: 5.2.3 Metodo Web: Consulta por Fecha de Elaboracion
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="FechaInicio">fecha inicial de consulta</param>
		/// <param name="FechaFin">fecha final de consulta</param>
		/// <param name="DocumentoTipo">indica el tipo de documento de consulta 1: Factura - 2: Nota Débito - 3: Nota Crédito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> ObtenerPorFechaElaboracion(string UrlWs, string Serial, string Identificacion, DateTime FechaInicio, DateTime FechaFin, int DocumentoTipo)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();
			
			ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioDocumento.ConsultaPorFechaElaboracionRequest peticion = new ServicioDocumento.ConsultaPorFechaElaboracionRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					FechaInicial = FechaInicio,
					FechaFinal = FechaFin,
					TipoDocumento = DocumentoTipo
				};

				// ejecución del servicio web
				ServicioDocumento.ConsultaPorFechaElaboracionResponse respuesta = cliente_ws.ConsultaPorFechaElaboracion(peticion);

				// resultado del servicio web
				List<ServicioDocumento.DocumentoRespuesta> result = respuesta.ConsultaPorFechaElaboracionResult;

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
		/// Permite enviar los documentos de tipo Factura por el Facturador Electrónico
		/// Manual Técnico: 6.1.4 Metodo Web: Crear Documento Archivo
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos como archivos XML en estandar UBL</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioDocumento.DocumentoArchivo> documentos_envio)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();
			
			ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

				foreach (ServicioDocumento.DocumentoArchivo item in documentos_envio)
				{	item.DataKey = dataKey;
				}
				
				// datos para la petición
				ServicioDocumento.RecepcionRequest peticion = new ServicioDocumento.RecepcionRequest()
				{
					documentos = documentos_envio
				};

				// ejecución del servicio web
				ServicioDocumento.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				List<ServicioDocumento.DocumentoRespuesta> result = respuesta.RecepcionResult;

				if (respuesta != null)
					return result.ToList();
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
			
			ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				// datos para la petición
				ServicioDocumento.TestRequest peticion = new ServicioDocumento.TestRequest();

				// ejecución del servicio web
				ServicioDocumento.TestResponse respuesta = cliente_ws.Test(peticion);

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

	}
}
