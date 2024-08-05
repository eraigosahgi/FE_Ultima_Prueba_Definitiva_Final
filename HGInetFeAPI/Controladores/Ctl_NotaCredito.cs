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
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo NotaCredito</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioNotaCredito.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioNotaCredito.NotaCredito> documentos_envio)
		{
			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			//Url Api
			UrlWs = string.Format("{0}Api/Notacredito/Recepcion", Ctl_Utilidades.ValidarUrl(UrlWs));

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			foreach (ServicioNotaCredito.NotaCredito item in documentos_envio)
			{
				if (item == null)
					throw new ApplicationException("No se encontró informacion en el ServicioNotaCredito.NotaCredito");
				//throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, documentos_envio, "ServicioNotaCredito.NotaCredito"));

				if (item.DocumentoDetalles == null || !item.DocumentoDetalles.Any())
					throw new Exception("El detalle del documento es inválido.");

				item.DataKey = dataKey;
			}

			List<ServicioNotaCredito.NotaCredito> datos = new List<ServicioNotaCredito.NotaCredito>();

			string vcData = JsonConvert.SerializeObject(documentos_envio);
			byte[] vtDataStream = Encoding.UTF8.GetBytes(vcData);

			List<ServicioNotaCredito.DocumentoRespuesta> respuesta = new List<ServicioNotaCredito.DocumentoRespuesta>();

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
						respuesta = JsonConvert.DeserializeObject<List<ServicioNotaCredito.DocumentoRespuesta>>(jsonResponse);
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


			//ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			//	// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			//	string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			//	foreach (ServicioNotaCredito.NotaCredito item in documentos_envio)
			//	{
			//		if (item == null)
			//			throw new ApplicationException("No se encontró informacion en el ServicioNotaCredito.NotaCredito");
			//		//throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, documentos_envio, "ServicioNotaCredito.NotaCredito"));

			//		if (item.DocumentoDetalles == null || !item.DocumentoDetalles.Any())
			//			throw new Exception("El detalle del documento es inválido.");

			//		item.DataKey = dataKey;
			//	}

			//	// datos para la petición
			//	ServicioNotaCredito.RecepcionRequest peticion = new ServicioNotaCredito.RecepcionRequest()
			//	{
			//		documentos = documentos_envio
			//	};

			//	// ejecución del servicio web
			//	ServicioNotaCredito.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

			//	// resultado del servicio web
			//	List<ServicioNotaCredito.DocumentoRespuesta> result = respuesta.RecepcionResult;

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
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="FechaInicio">fecha inicial de consulta</param>
		/// <param name="FechaFin">fecha final de consulta</param>
		/// <returns>Una lista de las Nota Credito generadas a nombre del adquiriente</returns>
		public static List<ServicioNotaCredito.NotaCreditoConsulta> ObtenerNotaCreditoPorAdquiriente(string UrlWs, string Serial, string Identificacion, DateTime FechaInicio, DateTime FechaFin)
		{
			// valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
			if (!UrlWs.Contains("hgi"))
			{
				UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
			}

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			UrlWs = string.Format("{0}/Api/NotaCredito/ObtenerPorFechasAdquiriente", Ctl_Utilidades.ValidarUrl(UrlWs));

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

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			// Construir la URL de la API con los parámetros
			UrlWs += $"?DataKey={dataKey}&Identificacion={Identificacion}&FechaInicio={FechaInicio}&FechaFinal={FechaFin}";

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
							datos = JsonConvert.DeserializeObject<List<ServicioNotaCredito.NotaCreditoConsulta>>(responseData);
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

			//ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			//	// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			//	string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			//	// datos para la petición
			//	ServicioNotaCredito.ObtenerPorFechasAdquirienteRequest peticion = new ServicioNotaCredito.ObtenerPorFechasAdquirienteRequest()
			//	{
			//		DataKey = dataKey,
			//		Identificacion = Identificacion,
			//		FechaInicio = FechaInicio,
			//		FechaFinal = FechaFin,
			//	};

			//	// ejecución del servicio web
			//	ServicioNotaCredito.ObtenerPorFechasAdquirienteResponse respuesta = cliente_ws.ObtenerPorFechasAdquiriente(peticion);

			//	// resultado del servicio web
			//	List<ServicioNotaCredito.NotaCreditoConsulta> result = respuesta.ObtenerPorFechasAdquirienteResult;

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
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGI Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGI Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="CodigosDocumentos">còdigos de documentos del Facturador Electrónico para consulta separados por el caracter coma (,)</param>
		/// <returns>Una lista de las Notas Credito generadas a nombre del adquiriente</returns>
		public static List<ServicioNotaCredito.NotaCreditoConsulta> ObtenerPorIdSeguridadAdquiriente(string UrlWs, string Serial, string Identificacion, string CodigosDocumentos, bool Facturador = false)
        {
	        // valida si es un integrador o son pruebas para que obtenga la ruta que le corresponda
	        if (!UrlWs.Contains("hgi"))
	        {
		        UrlWs = Ctl_Utilidades.ObtenerUrl(UrlWs, Identificacion);
	        }

			// valida la URL del servicio web
			//UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			UrlWs = string.Format("{0}Api/NotaCredito/ObtenerPorIdSeguridadAdquiriente", Ctl_Utilidades.ValidarUrl(UrlWs));

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

			// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

			// Construir la URL de la API con los parámetros
			UrlWs += $"?DataKey={dataKey}&Identificacion={Identificacion}&CodigosRegistros={CodigosDocumentos}&Facturador={Facturador}";

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
							datos = JsonConvert.DeserializeObject<List<ServicioNotaCredito.NotaCreditoConsulta>>(responseData);
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

            

   //         List<ServicioNotaCredito.NotaCreditoConsulta> datos = new List<ServicioNotaCredito.NotaCreditoConsulta>();

			//ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = null;

			//try
			//{
			//	// conexión cliente para el servicio web
			//	EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
			//	cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
			//	cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			//	// configura la cadena de autenticación para la ejecución del servicio web en SHA1
			//	string dataKey = Ctl_Utilidades.Encriptar_SHA512(string.Format("{0}{1}", Serial, Identificacion));

   //             // datos para la petición
   //             ServicioNotaCredito.ObtenerPorIdSeguridadAdquirienteRequest peticion = new ServicioNotaCredito.ObtenerPorIdSeguridadAdquirienteRequest()
   //             {
   //                 DataKey = dataKey,
   //                 Identificacion = Identificacion,
   //                 CodigosRegistros = CodigosDocumentos,
			//		Facturador = Facturador,
   //             };

   //             // ejecución del servicio web
   //             ServicioNotaCredito.ObtenerPorIdSeguridadAdquirienteResponse respuesta = cliente_ws.ObtenerPorIdSeguridadAdquiriente(peticion);

   //             // resultado del servicio web
   //             List<ServicioNotaCredito.NotaCreditoConsulta> result = respuesta.ObtenerPorIdSeguridadAdquirienteResult;

   //             if (respuesta != null)
   //                 return result;
   //             else
   //                 throw new Exception("Error al obtener los datos con los parámetros indicados.");

   //         }
   //         catch (FaultException excepcion)
   //         {
   //             throw new ApplicationException(excepcion.Message, excepcion);
   //         }
   //         catch (CommunicationException excepcion)
   //         {
   //             throw new Exception(string.Format("Error de comunicación: {0}", excepcion.Message), excepcion);
   //         }
   //         catch (Exception excepcion)
   //         {
   //             throw excepcion;
   //         }
   //         finally
   //         {
   //             if (cliente_ws != null)
   //                 cliente_ws.Abort();
   //         }
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

			ServicioNotaCredito.ServicioNotaCreditoClient cliente_ws = null;

			try
			{
				// conexión cliente para el servicio web
				EndpointAddress endpoint_address = new System.ServiceModel.EndpointAddress(UrlWs);
				cliente_ws = new ServicioNotaCredito.ServicioNotaCreditoClient(Ctl_Utilidades.ObtenerBinding(UrlWs), endpoint_address);
				cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

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
		//public static string CalcularCUFE(string clave_tecnica, string cufe_factura,string prefijo , string numero_nota_credito, DateTime fecha_nota_credito, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		//{
		//	try
		//	{
		//		string cufe_encriptado = Ctl_CalculoCufe.CufeNotaCredito(clave_tecnica, cufe_factura, prefijo, numero_nota_credito, fecha_nota_credito, nit_facturador, tipo_identificacion_adquiriente, nit_adquiriente, total, subtotal, iva, impto_consumo, rte_ica, true);
		//		return cufe_encriptado;
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}

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
		//public static string CalcularCUFEV2(string pin_software, string prefijo, string numero_nota_credito, DateTime fecha_nota_credito, string nit_facturador, string ambiente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		//{
		//	try
		//	{
		//		if (string.IsNullOrEmpty(ambiente))
		//			throw new Exception("Ambiente de Envío del documento no valido");

		//		if (string.IsNullOrEmpty(pin_software))
		//			throw new Exception("Pin del Software no valido");

		//		//string fecha = fecha_nota_credito.AddHours(5).ToString(Fecha.formato_fecha_hora_zona);
		//		string fecha = fecha_nota_credito.ToString(Fecha.formato_fecha_hginet);
		//		string hora = fecha_nota_credito.AddHours(5).ToString(Fecha.formato_hora_zona);
		//		string fec_hor = string.Format("{0}{1}", fecha, hora);
		//		string cufe_encriptado = Ctl_CalculoCufe.CufeNotaCreditoV2(pin_software, prefijo, numero_nota_credito, fec_hor, nit_facturador, ambiente, nit_adquiriente, total, subtotal, iva, impto_consumo, rte_ica, true);
		//		return cufe_encriptado;
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}



	}
}
