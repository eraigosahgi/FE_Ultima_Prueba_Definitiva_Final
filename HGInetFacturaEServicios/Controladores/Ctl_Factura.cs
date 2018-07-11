using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace HGInetFacturaEServicios
{
	/// <summary>
	/// Controlador para el envío de objetos de Factura
	/// </summary>
	public class Ctl_Factura
	{
		// ruta donde se encuentra el servicio web
		private static string UrlWcf = "wcf/factura.svc";

		/// <summary>
		/// Permite enviar los documentos de tipo Factura por el Facturador Electrónico
		/// Manual Técnico: 5.1.1 Metodo Web: Crear Factura
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="documentos_envio">documentos de tipo Factura</param>
		/// <returns>respuesta del proceso de los documentos</returns>
		public static List<ServicioFactura.DocumentoRespuesta> Enviar(string UrlWs, string Serial, string Identificacion, List<ServicioFactura.Factura> documentos_envio)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioFactura.Factura> datos = new List<ServicioFactura.Factura>();

			// conexión cliente para el servicio web
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				foreach (ServicioFactura.Factura item in documentos_envio)
				{
					if (item == null)
						throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, documentos_envio, "ServicioFactura.Factura"));

					if (item.DocumentoDetalles == null || !item.DocumentoDetalles.Any())
						throw new Exception("El detalle del documento es inválido.");

					if (item.DocumentoFormato != null)
					{
						if (!string.IsNullOrEmpty(item.DocumentoFormato.ArchivoPdf))
						{
							byte[] pdf = Convert.FromBase64String(item.DocumentoFormato.ArchivoPdf);
							//valida el peso del formato
							if (pdf.Length < 5120)
								throw new Exception("El Formato de impresion es inválido.");
						}
					}
					item.DataKey = dataKey;
				}

				// datos para la petición
				ServicioFactura.RecepcionRequest peticion = new ServicioFactura.RecepcionRequest()
				{
					documentos = documentos_envio
				};

				// ejecución del servicio web
				ServicioFactura.RecepcionResponse respuesta = cliente_ws.Recepcion(peticion);

				// resultado del servicio web
				List<ServicioFactura.DocumentoRespuesta> result = respuesta.RecepcionResult;

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
		/// Permite obtener los documentos enviados a la plataforma a nombre del Adquiriente
		/// </summary>
		/// <param name="UrlWs">ruta principal de ejecución del servicio web HGInet Facturación Electrónica (http)</param>
		/// <param name="Serial">serial de licenciamiento para HGInet Facturación Electrónica</param>
		/// <param name="Identificacion">número de identificación del Adquiriente</param>
		/// <param name="FechaInicio">fecha inicial de consulta</param>
		/// <param name="FechaFin">fecha final de consulta</param>
		/// <returns>Una lista de las Facturas generadas a nombre del adquiriente</returns>
		public static List<ServicioFactura.FacturaConsulta> ObtenerFacturaPorAdquiriente(string UrlWs, string Serial, string Identificacion, DateTime FechaInicio, DateTime FechaFin)
		{
			// valida la URL del servicio web
			UrlWs = string.Format("{0}{1}", Ctl_Utilidades.ValidarUrl(UrlWs), UrlWcf);

			// valida el parámetro Serial
			if (string.IsNullOrEmpty(Serial))
				throw new ApplicationException("Parámetro Serial de tipo string inválido.");

			// valida el parámetro Identificacion
			if (string.IsNullOrEmpty(Identificacion))
				throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

			List<ServicioFactura.FacturaConsulta> datos = new List<ServicioFactura.FacturaConsulta>();

			// conexión cliente para el servicio web
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// configura la cadena de autenticación para la ejecución del servicio web en SHA1
				string dataKey = Ctl_Utilidades.Encriptar_SHA1(string.Format("{0}{1}", Serial, Identificacion));

				// datos para la petición
				ServicioFactura.ObtenerPorFechasAdquirienteRequest peticion = new ServicioFactura.ObtenerPorFechasAdquirienteRequest()
				{
					DataKey = dataKey,
					Identificacion = Identificacion,
					FechaInicio = FechaInicio,
					FechaFinal = FechaFin,
				};

				// ejecución del servicio web
				ServicioFactura.ObtenerPorFechasAdquirienteResponse respuesta = cliente_ws.ObtenerPorFechasAdquiriente(peticion);

				// resultado del servicio web
				List<ServicioFactura.FacturaConsulta> result = respuesta.ObtenerPorFechasAdquirienteResult;

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
			ServicioFactura.ServicioFacturaClient cliente_ws = new ServicioFactura.ServicioFacturaClient();
			cliente_ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(UrlWs);

			try
			{
				// datos para la petición
				ServicioFactura.TestRequest peticion = new ServicioFactura.TestRequest();

				// ejecución del servicio web
				ServicioFactura.TestResponse respuesta = cliente_ws.Test(peticion);

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
		/// Calcula el código CUFE de la factura
		/// </summary>     
		/// <param name="clave_tecnica">Clave técnica de la resolución</param>
		/// <param name="numero_factura">Número de la factura</param>
		/// <param name="fecha_factura">Fecha de elaboración de la factura</param>
		/// <param name="nit_facturador">Documento de identificación del facturador electrónico</param>
		/// <param name="tipo_identificacion_adquiriente">Código del tipo de identificación del adquiriente</param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la factura</param>
		/// <param name="subtotal">Subtotal de la factura</param>
		/// <param name="iva">Iva de la factura</param>
		/// <param name="impto_consumo">Impuesto al consumo de la factura</param>
		/// <param name="rte_ica">Retención del ICA de la factura</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CalcularCUFE(string clave_tecnica, string numero_factura, DateTime fecha_factura, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception(string.Format("Parámetro {0} inválido.", "clave_tecnica"));

				if (string.IsNullOrWhiteSpace(numero_factura))
					throw new Exception(string.Format("Parámetro {0} inválido.", "numero_factura"));

				if (string.IsNullOrWhiteSpace(nit_facturador))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_facturador"));

				if (string.IsNullOrWhiteSpace(tipo_identificacion_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "tipo_identificacion_adquiriente"));

				if (string.IsNullOrWhiteSpace(nit_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_adquiriente"));

				/*
				NumFac = Número de factura.
				FecFac = Fecha de factura en formato (Java) YYYYmmddHHMMss. 
				ValFac = Valor Factura sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp1 = 01 
				ValImp1 = Valor impuesto 01, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp2 = 02 
				ValImp2 = Valor impuesto 02, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp3 = 03 
				ValImp3 = Valor impuesto 03, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				ValImp = Valor IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				NitOFE = NIT del Facturador Electrónico sin puntos ni guiones, sin digito de verificación. 
				TipAdq = tipo de adquiriente, de acuerdo con la tabla Tipos de documentos de identidad del «Anexo 001 Formato estándar XML de la Factura, notas débito y notas crédito electrónicos» 
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				ClTec = Clave técnica del rango de facturación.
				CUFE = SHA-1(NumFac + FecFac + ValFac + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 +
							ValImp3 + ValImp + NitOFE + TipAdq + NumAdq + ClTec) 
			
				NumFac = /fe:Invoice/cbc:ID
				FecFac = sinSimbolos(/fe:Invoice/cbc:IssueDate + /fe:Invoice/cbc:IssueTime) formato AAAAMMDDHHMMSS
				i.e. año + mes + día + hora + minutos + segundos
				ValFac = /fe:Invoice/fe:LegalMonetaryTotal/cbc:LineExtensionAmount
				CodImp1 = /fe:Invoice/fe:TaxTotal[x]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 01
				ValImp1 = /fe:Invoice/fe:TaxTotal[x]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp2 = /fe:Invoice/fe:TaxTotal[y]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 02
				ValImp2 = /fe:Invoice/fe:TaxTotal[y]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp3 = /fe:Invoice/fe:TaxTotal[z]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 03
				ValImp3 = /fe:Invoice/fe:TaxTotal[z]/fe:TaxSubtotal/cbc:TaxAmount
				ValImp = /fe:Invoice/fe:LegalMonetaryTotal/cbc:PayableAmount
				NitOFE = /fe:Invoice/fe:AccountingSupplierParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipAdq = /fe:Invoice/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID/@schemeID
				NumAdq = /fe:Invoice/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				ClTec = no está en el XML 			 
				*/

				string codigo_impuesto = string.Empty;
				DateTime fecha = fecha_factura;
				DateTime fecha_hora = Convert.ToDateTime(fecha_factura);
				TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				fecha = fecha.Date + hora;

				string NumFac = numero_factura;
				string FecFac = fecha.ToString(Fecha.formato_fecha_java);
				string ValFac = subtotal.ToString();


				// formato para validación de valor con dos decimales
				Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = iva;

				//Valida el iva
				if (ValImp1 == 0)
					ValImp1 = Convert.ToDecimal(0.00M);
				else if (!isnumber.IsMatch(Convert.ToString(ValImp1).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor iva {0} no esta bien formado", iva));

				//Impuesto 2
				string CodImp2 = "02";
				decimal ValImp2 = impto_consumo;

				//Valida el impuesto al consumo
				if (ValImp2 == 0)
					ValImp2 = Convert.ToDecimal(0.00M);
				else if (!isnumber.IsMatch(Convert.ToString(ValImp2).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor impto_consumo {0} no esta bien formado", impto_consumo));

				//Impuesto 3
				string CodImp3 = "03";
				decimal ValImp3 = rte_ica;

				//Valida el reteica
				if (ValImp3 == 0)
					ValImp3 = Convert.ToDecimal(0.00M);
				else if (!isnumber.IsMatch(Convert.ToString(ValImp3).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor rte_ica {0} no esta bien formado", rte_ica));

				string ValImp = total.ToString();

				string NitOFE = nit_facturador;

				string TipAdq = tipo_identificacion_adquiriente;
				string NumAdq = nit_adquiriente;

				string cufe = NumFac
					+ FecFac
					+ ValFac.Replace(",", ".")
					+ CodImp1
					+ ValImp1.ToString().Replace(",", ".")
					+ CodImp2
					+ ValImp2.ToString().Replace(",", ".")
					+ CodImp3
					+ ValImp3.ToString().Replace(",", ".")
					+ ValImp.Replace(",", ".")
					+ NitOFE
					+ TipAdq
					+ NumAdq
					+ clave_tecnica
				;

				string cufe_encriptado = Encriptar.Encriptar_SHA1(cufe);
				return cufe_encriptado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
