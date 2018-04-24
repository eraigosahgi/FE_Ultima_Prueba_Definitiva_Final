using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
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
				{	item.DataKey = dataKey;
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
		/// Calcula el codigo CUFE
		/// </summary>
		/// <param name="nota_credito">Objeto de tipo CreditNoteType que contiene la informacion de la Nota Crédito</param>
		/// <param name="clave_tecnica">Clave técnica de la resolución</param>
		/// <param name="cufe_factura">Identificador de la factura Afectada</param>
		/// <returns></returns>
		public static string CalcularCUFE(string clave_tecnica, string cufe_factura, string numero_nota_credito, DateTime fecha_nota_credito, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception(string.Format("Parámetro {0} inválido.", "clave_tecnica"));

				if (string.IsNullOrWhiteSpace(cufe_factura))
					throw new Exception(string.Format("Parámetro {0} inválido.", "cufe_factura"));
					
				if (string.IsNullOrWhiteSpace(numero_nota_credito))
					throw new Exception(string.Format("Parámetro {0} inválido.", "numero_nota_credito"));

				if (string.IsNullOrWhiteSpace(nit_facturador))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_facturador"));

				if (string.IsNullOrWhiteSpace(tipo_identificacion_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "tipo_identificacion_adquiriente"));

				if (string.IsNullOrWhiteSpace(nit_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_adquiriente"));

				#region Documentación de la creación código CUFE
				/*
				NumCr = Número de Nota Credito.
				Feccr = Fecha de Nota Credito en formato (Java) YYYYmmddHHMMss. 
				ValCr = Valor Nota Credito sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
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
			
				NumCr = /fe:CreditNote/cbc:ID
				FecCr = sinSimbolos(/fe:CreditNote/cbc:IssueDate + /fe:CreditNote/cbc:IssueTime)
						formato AAAAMMDDHHMMSS i.e. año + mes + día + hora + minutos + segundos
				ValCr = /fe:CreditNote/fe:LegalMonetaryTotal/cbc:LineExtensionAmount
				CodImp1 = /fe:CreditNote/fe:TaxTotal[X]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 01
				ValImp1 = /fe:CreditNote/fe:TaxTotal[X]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp2 = /fe:CreditNote/fe:TaxTotal[y]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 02
				ValImp2 = /fe:CreditNote/fe:TaxTotal[y]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp3 = /fe:CreditNote/fe:TaxTotal[z]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 03
				ValImp3 = /fe:CreditNote/fe:TaxTotal[z]/fe:TaxSubtotal/cbc:TaxAmount
				ValPag = /fe:CreditNote/fe:LegalMonetaryTotal/cbc:PayableAmount
				NitOFE = /fe:CreditNote/fe:AccountingSupplierParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipAdq = /fe:CreditNote/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID/@schemeID
				NumAdq = /fe:CreditNote /fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				ClTec = Examine la sección Cálculos para Nota Cr. 			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				DateTime fecha = fecha_nota_credito;
				DateTime fecha_hora = Convert.ToDateTime(fecha_nota_credito);
				TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				fecha = fecha.Date + hora;

				string NumCr = numero_nota_credito;
				string FecCr = fecha.ToString(Fecha.formato_fecha_java);
				string ValCr = subtotal.ToString();


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


				string cufe = NumCr
					+ FecCr
					+ ValCr.Replace(",", ".")
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
					+ cufe_factura
				;

				string cufe_encriptado = Encriptar.Encriptar_SHA1(cufe);
				return cufe_encriptado;
				#endregion
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



	}
}
