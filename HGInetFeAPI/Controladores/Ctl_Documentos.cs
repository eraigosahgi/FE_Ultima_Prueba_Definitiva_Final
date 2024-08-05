using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="NumerosDocumentos">números de documentos para consulta separados por el caracter coma (,)</param>
		/// <param name="DocumentoTipo">indica el tipo de documento de consulta 1: Factura - 2: Nota Débito - 3: Nota Crédito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> ObtenerPorNumeros(string UrlWs, string Serial, string Identificacion, string NumerosDocumentos, int DocumentoTipo)
		{
			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			//Url Api
			UrlWs = string.Format("{0}Api/DocumentosApi/ConsultaPorNumeros", Ctl_Utilidades.ValidarUrl(UrlWs));

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(NumerosDocumentos))
				throw new ApplicationException("Parámetro CodigosDocumentos de tipo string inválido.");

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			// Construir la URL de la API con los parámetros
			UrlWs += $"?DataKey={dataKey}&Identificacion={Identificacion}&Numeros={NumerosDocumentos}&TipoDocumento={DocumentoTipo}";

			// Crear una solicitud HTTP utilizando la URL de la API
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlWs);
			request.Method = "GET";

			// Enviar la solicitud y obtener la respuesta
			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					// Verificar el código de estado de la respuesta
					if (response.StatusCode == HttpStatusCode.OK)
					{
						// Leer la respuesta
						using (StreamReader reader = new StreamReader(response.GetResponseStream()))
						{
							string responseData = reader.ReadToEnd();

							// Deserializar la respuesta JSON en un objeto MiObjeto
							datos = JsonConvert.DeserializeObject<List<ServicioDocumento.DocumentoRespuesta>>(responseData);
							return datos;
						}
					}
					else
					{
						//Console.WriteLine("Error al llamar a la API. Código de estado: " + response.StatusCode);
						throw new Exception("Error al obtener los datos con los parámetros indicados. Código de estado:" + response.StatusCode);
					}
				}
			}
			catch (WebException ex)
			{
				string ex_message = string.Empty;
				// Manejar excepciones de WebException
				if (ex.Response != null)
				{
					using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
					{
						ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
						using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
						{
							string errorText = reader.ReadToEnd();
							ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
						}
					}
				}
				else
				{
					ex_message = ("Error: " + ex.Message);
				}

				throw new Exception(ex_message, ex);
			}

			//ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

				

			//	// datos para la petición
			//	ServicioDocumento.ConsultaPorNumerosRequest peticion = new ServicioDocumento.ConsultaPorNumerosRequest()
			//	{	DataKey = dataKey,
			//		Identificacion = Identificacion,
			//		Numeros = NumerosDocumentos,
			//		TipoDocumento = DocumentoTipo
			//	};

			//	// ejecución del servicio web
			//	ServicioDocumento.ConsultaPorNumerosResponse respuesta = cliente_ws.ConsultaPorNumeros(peticion);

			//	// resultado del servicio web
			//	List<ServicioDocumento.DocumentoRespuesta> result = respuesta.ConsultaPorNumerosResult;

			//	if (respuesta != null)
			//		return result;
			//	else
			//		throw new Exception("Error al obtener los datos con los parámetros indicados.");
			//}
			//catch (FaultException excepcion)
			//{
			//	throw new ApplicationException(excepcion.Message, excepcion);
			//}
			//catch (CommunicationException excepcion)
			//{
			//	throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			//}
			//catch (Exception excepcion)
			//{
			//	throw excepcion;
			//}
			//finally
			//{
			//	if (cliente_ws != null)
			//		cliente_ws.Abort();
			//}
		}

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
		public static List<ServicioDocumento.DocumentoRespuesta> ObtenerPorCodigoRegistro(string UrlWs, string Serial, string Identificacion, string CodigosDocumentos, int DocumentoTipo)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			UrlWs = string.Format("{0}Api/DocumentosApi/ConsultaPorCodigoRegistro", Ctl_Utilidades.ValidarUrl(UrlWs));

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(CodigosDocumentos))
				throw new ApplicationException("Parámetro CodigosDocumentos de tipo string inválido.");

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			// Construir la URL de la API con los parámetros
			UrlWs += $"?DataKey={dataKey}&Identificacion={Identificacion}&CodigosRegistros={CodigosDocumentos}&TipoDocumento={DocumentoTipo}";

			// Crear una solicitud HTTP utilizando la URL de la API
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlWs);
			request.Method = "GET";

			// Enviar la solicitud y obtener la respuesta
			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					// Verificar el código de estado de la respuesta
					if (response.StatusCode == HttpStatusCode.OK)
					{
						// Leer la respuesta
						using (StreamReader reader = new StreamReader(response.GetResponseStream()))
						{
							string responseData = reader.ReadToEnd();

							// Deserializar la respuesta JSON en un objeto MiObjeto
							datos = JsonConvert.DeserializeObject<List<ServicioDocumento.DocumentoRespuesta>>(responseData);
							return datos;
						}
					}
					else
					{
						//Console.WriteLine("Error al llamar a la API. Código de estado: " + response.StatusCode);
						throw new Exception("Error al obtener los datos con los parámetros indicados. Código de estado:" + response.StatusCode);
					}
				}
			}
			catch (WebException ex)
			{
				string ex_message = string.Empty;
				// Manejar excepciones de WebException
				if (ex.Response != null)
				{
					using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
					{
						ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
						using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
						{
							string errorText = reader.ReadToEnd();
							ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
						}
					}
				}
				else
				{
					ex_message = ("Error: " + ex.Message);
				}

				throw new Exception(ex_message, ex);
			}

			//ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			//	// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			//	string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			//	// datos para la petición
			//	ServicioDocumento.ConsultaPorCodigoRegistroRequest peticion = new ServicioDocumento.ConsultaPorCodigoRegistroRequest()
			//	{
			//		DataKey = dataKey,
			//		Identificacion = Identificacion,
			//		CodigosRegistros = CodigosDocumentos,
			//		TipoDocumento = DocumentoTipo
			//	};

			//	// ejecución del servicio web
			//	ServicioDocumento.ConsultaPorCodigoRegistroResponse respuesta = cliente_ws.ConsultaPorCodigoRegistro(peticion);

			//	// resultado del servicio web
			//	List<ServicioDocumento.DocumentoRespuesta> result = respuesta.ConsultaPorCodigoRegistroResult;

			//	if (respuesta != null)
			//		return result;
			//	else
			//		throw new Exception("Error al obtener los datos con los parámetros indicados.");

			//}
			//catch (FaultException excepcion)
			//{
			//	throw new ApplicationException(excepcion.Message, excepcion);
			//}
			//catch (CommunicationException excepcion)
			//{
			//	throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			//}
			//catch (Exception excepcion)
			//{
			//	throw excepcion;
			//}
			//finally
			//{
			//	if (cliente_ws != null)
			//		cliente_ws.Abort();
			//}
		}

		/// <summary>
		/// Permite consultar el resultado de los documentos enviados a la plataforma por el Facturador Electrónico
		/// Manual Técnico: 5.2.3 Metodo Web: Consulta por Fecha de Elaboracion
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="FechaInicio">fecha inicial de consulta</param>
		/// <param name="FechaFin">fecha final de consulta</param>
		/// <param name="DocumentoTipo">indica el tipo de documento de consulta 1: Factura - 2: Nota Débito - 3: Nota Crédito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> ObtenerPorFechaElaboracion(string UrlWs, string Serial, string Identificacion, DateTime FechaInicio, DateTime FechaFin, int DocumentoTipo)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			UrlWs = string.Format("{0}Api/DocumentosApi/ConsultaPorFechaElaboracion", Ctl_Utilidades.ValidarUrl(UrlWs));

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

			List<ServicioDocumento.DocumentoRespuesta> datos = new List<ServicioDocumento.DocumentoRespuesta>();

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			// Construir la URL de la API con los parámetros
			UrlWs += $"?DataKey={dataKey}&Identificacion={Identificacion}&FechaInicio={FechaInicio}&FechaFinal={FechaFin}&TipoDocumento={DocumentoTipo}";

			// Crear una solicitud HTTP utilizando la URL de la API
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlWs);
			request.Method = "GET";

			// Enviar la solicitud y obtener la respuesta
			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					// Verificar el código de estado de la respuesta
					if (response.StatusCode == HttpStatusCode.OK)
					{
						// Leer la respuesta
						using (StreamReader reader = new StreamReader(response.GetResponseStream()))
						{
							string responseData = reader.ReadToEnd();

							// Deserializar la respuesta JSON en un objeto MiObjeto
							datos = JsonConvert.DeserializeObject<List<ServicioDocumento.DocumentoRespuesta>>(responseData);
							return datos;
						}
					}
					else
					{
						//Console.WriteLine("Error al llamar a la API. Código de estado: " + response.StatusCode);
						throw new Exception("Error al obtener los datos con los parámetros indicados. Código de estado:" + response.StatusCode);
					}
				}
			}
			catch (WebException ex)
			{
				string ex_message = string.Empty;
				// Manejar excepciones de WebException
				if (ex.Response != null)
				{
					using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
					{
						ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
						using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
						{
							string errorText = reader.ReadToEnd();
							ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
						}
					}
				}
				else
				{
					ex_message = ("Error: " + ex.Message);
				}

				throw new Exception(ex_message, ex);
			}

			//ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			//	// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			//	string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			//	// datos para la petición
			//	ServicioDocumento.ConsultaPorFechaElaboracionRequest peticion = new ServicioDocumento.ConsultaPorFechaElaboracionRequest()
			//	{
			//		DataKey = dataKey,
			//		Identificacion = Identificacion,
			//		FechaInicial = FechaInicio,
			//		FechaFinal = FechaFin,
			//		TipoDocumento = DocumentoTipo
			//	};

			//	// ejecución del servicio web
			//	ServicioDocumento.ConsultaPorFechaElaboracionResponse respuesta = cliente_ws.ConsultaPorFechaElaboracion(peticion);

			//	// resultado del servicio web
			//	List<ServicioDocumento.DocumentoRespuesta> result = respuesta.ConsultaPorFechaElaboracionResult;

			//	if (respuesta != null)
			//		return result;
			//	else
			//		throw new Exception("Error al obtener los datos con los parámetros indicados.");

			//}
			//catch (FaultException excepcion)
			//{
			//	throw new ApplicationException(excepcion.Message, excepcion);
			//}
			//catch (CommunicationException excepcion)
			//{
			//	throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			//}
			//catch (Exception excepcion)
			//{
			//	throw excepcion;
			//}
			//finally
			//{
			//	if (cliente_ws != null)
			//		cliente_ws.Abort();
			//}
		}

		/// <summary>
		/// Permite enviar los documentos de tipo Factura por el Facturador Electrónico
		/// Manual Técnico: 6.1.4 Metodo Web: Crear Documento Archivo
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos como archivos XML en estandar UBL</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioDocumento.DocumentoArchivo> documentos_envio)
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
		/// Obtiene el CUFE o CUDE y QR del documento electrónico
		/// Manual Técnico: 6.1.4 Metodo Web: Obtener Cufe
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_cufe">información de documentos electrónicos para el cálculo del CUFE</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioDocumento.DocumentoCufe> CalcularCufe(string UrlWs, string Serial, string Identificacion, List<ServicioDocumento.DocumentoCufe> documentos_cufe)
		{

			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			UrlWs = string.Format("{0}/Api/Notadebito/ObtenerPorFechasAdquiriente", Ctl_Utilidades.ValidarUrl(UrlWs));

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			// valida el parámetro Identificacion
			if (documentos_cufe == null)
				throw new ApplicationException("Parámetro documentos_cufe no puede estar vacío.");

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			foreach (ServicioDocumento.DocumentoCufe item in documentos_cufe)
			{
				item.DataKey = dataKey;
			}

			string vcData = JsonConvert.SerializeObject(documentos_cufe);
			byte[] vtDataStream = Encoding.UTF8.GetBytes(vcData);

			List<ServicioDocumento.DocumentoCufe> respuesta = new List<ServicioDocumento.DocumentoCufe>();

			try
			{
				HttpWebRequest vtRequest = (HttpWebRequest)WebRequest.Create(UrlWs);

				vtRequest.Method = "POST";
				vtRequest.ContentType = "application/json";
				vtRequest.Accept = "application/json";
				vtRequest.ContentLength = vtDataStream.Length;

				//Se agrega instruccion para habilitar la seguridad en el envio
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

				Stream newStream = vtRequest.GetRequestStream();

				// Enviamos los datos
				newStream.Write(vtDataStream, 0, vtDataStream.Length);
				newStream.Close();

				// ejecución del servicio web
				HttpWebResponse vtHttpResponse = (HttpWebResponse)vtRequest.GetResponse();

				if (vtHttpResponse.StatusCode == HttpStatusCode.OK)
				{
					using (StreamReader vtStreamReader = new StreamReader(vtHttpResponse.GetResponseStream()))
					{
						// Leer el contenido de la respuesta como una cadena JSON
						string jsonResponse = vtStreamReader.ReadToEnd();

						// Deserializar la respuesta JSON en un objeto MiObjeto
						respuesta = JsonConvert.DeserializeObject<List<ServicioDocumento.DocumentoCufe>>(jsonResponse);
					}

				}
				vtHttpResponse.Close();

				return respuesta;
			}
			catch (WebException ex)
			{
				string ex_message = string.Empty;
				// Manejar excepciones de WebException
				if (ex.Response != null)
				{
					using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
					{
						ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
						using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
						{
							string errorText = reader.ReadToEnd();
							ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
						}
					}
				}
				else
				{
					ex_message = ("Error: " + ex.Message);
				}

				throw new Exception(ex_message, ex);
			}

			//ServicioDocumento.ServicioDocumentosClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioDocumento.ServicioDocumentosClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			//	// datos para la petición
			//	ServicioDocumento.ObtenerCufeRequest peticion = new ServicioDocumento.ObtenerCufeRequest()
			//	{
			//		documentos_cufe = documentos_cufe
			//	};

			//	// ejecución del servicio web
			//	ServicioDocumento.ObtenerCufeResponse respuesta = cliente_ws.ObtenerCufe(peticion);

			//	// resultado del servicio web
			//	List<ServicioDocumento.DocumentoCufe> result = respuesta.ObtenerCufeResult;

			//	if (respuesta != null)
			//		return result.ToList();
			//	else
			//		throw new Exception("Error al obtener los datos con los parámetros indicados.");

			//}
			//catch (FaultException excepcion)
			//{
			//	throw new ApplicationException(excepcion.Message, excepcion);
			//}
			//catch (CommunicationException excepcion)
			//{
			//	throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
			//}
			//catch (Exception excepcion)
			//{
			//	throw excepcion;
			//}
			//finally
			//{
			//	if (cliente_ws != null)
			//		cliente_ws.Abort();
			//}
		}


		/// <summary>
		/// Prueba del servicio web de la plataforma de Facturación Electrónica
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <returns></returns>
		public static string Test(string UrlWs)
		{
			string Identificacion = "811021438";
			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
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
