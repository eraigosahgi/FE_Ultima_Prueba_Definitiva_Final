using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LibreriaGlobalHGInet.HgiNet.Controladores
{
	public class Ctl_CalculoCufe
	{

		/// <summary>
		/// Proceso para Calcular el CUFE de la Factura
		/// </summary>
		/// <param name="clave_tecnica">Clave Tecnica de la Resolucion</param>
		/// <param name="prefijo">Prefijo del documento si es enviado desde el ERP</param> 
		/// <param name="numero_factura">Numero de la Factura si es desde el ERP, si es de Plataforma envia Prefijo + numero del documento</param>
		/// <param name="fecha_factura">Fecha del documento</param>
		/// <param name="nit_facturador">Nit del Facturador Electrónico</param>
		/// <param name="tipo_identificacion_adquiriente">Tipo de Identificacion</param>
		/// <param name="nit_adquiriente">Nit del Adquiriente de la Factura</param>
		/// <param name="total">Total mas IVA de la Factura</param>
		/// <param name="subtotal">Valor de la Factura antes de IVA</param>
		/// <param name="iva">Valor del IVA</param>
		/// <param name="impto_consumo">Valor del Impuesto al consumo</param>
		/// <param name="rte_ica">Valor de la ReteICA</param>
		/// <param name="Erp">Indica quien envia la informacion para llenar el numero de fectura (NumFac) </param>
		/// <returns>un string con el el valor del cufe encriptado</returns>
		public static string CufeFactura(string clave_tecnica, string prefijo, string numero_factura, DateTime fecha_factura, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica, bool Erp)
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
							ValImp = Valor Factura con IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
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

				//Si el proceso viene del ERP debe llegar prefijo si utiliza y factura por separado, si no la plataforma envia las dos propiedades en numero_factura
				string NumFac = string.Empty;
				if (Erp)
				{
					NumFac = string.Format("{0}{1}", prefijo, numero_factura);
				}
				else
				{
					NumFac = numero_factura;
				}

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

		/// <summary>
		/// Proceso para Calcular el CUFE de la Factura
		/// </summary>
		/// <param name="clave_tecnica">Clave Tecnica de la Resolucion</param>
		/// <param name="prefijo">Prefijo del documento si es enviado desde el ERP</param> 
		/// <param name="numero_factura">Numero de la Factura si es desde el ERP, si es de Plataforma envia Prefijo + numero del documento</param>
		/// <param name="fecha_factura">Fecha del documento</param>
		/// <param name="nit_facturador">Nit del Facturador Electrónico</param>
		/// <param name="ambiente">Ambiente a donde se va enviar el documento </param>
		/// <param name="nit_adquiriente">Nit del Adquiriente de la Factura</param>
		/// <param name="total">Total mas IVA de la Factura</param>
		/// <param name="subtotal">Valor de la Factura antes de IVA</param>
		/// <param name="iva">Valor del IVA</param>
		/// <param name="impto_consumo">Valor del Impuesto al consumo</param>
		/// <param name="rte_ica">Valor de la ReteICA</param>
		/// <param name="Erp">Indica quien envia la informacion para llenar el numero de fectura (NumFac) </param>
		/// <returns>un string con el el valor del cufe encriptado</returns>
		public static string CufeFacturaV2(string clave_tecnica, string prefijo, string numero_factura, string fecha_factura, string nit_facturador, string ambiente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica, bool Erp)
		{

			try
			{
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception(string.Format("Parámetro {0} inválido.", "clave_tecnica"));

				if (string.IsNullOrWhiteSpace(numero_factura))
					throw new Exception(string.Format("Parámetro {0} inválido.", "numero_factura"));

				if (string.IsNullOrWhiteSpace(nit_facturador))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_facturador"));

				if (string.IsNullOrWhiteSpace(nit_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_adquiriente"));

				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception("La clave técnica es inválida.");

				if (string.IsNullOrWhiteSpace(ambiente))
					throw new Exception("El ambiente es inválido.");

				#region Documentación de la creación código CUFE
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
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				ClTec = Clave técnica del rango de facturación.
				TipoAmbiente = Número de identificación del ambiente utilizado por el contribuyente para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas) 

				Composición del CUFE = SHA-384(NumFac + FecFac + HorFac + ValFac + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 + ValImp3 + ValTot + NitOFE +  NumAdq + ClTec + TipoAmbie)  
			
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
				TipoAmb = Invoice/cbc:ProfileExecutionID  			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				//DateTime fecha = fecha_factura;
				//DateTime fecha_hora = Convert.ToDateTime(factura.IssueTime.Value);
				//TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				//fecha = fecha.Date + hora;

				//string hora_gmt = TimeZoneInfo.ConvertTimeToUtc(fecha).ToString("yyyy-MM-ddHH:mm:sszz:ss");
				//Si el proceso viene del ERP debe llegar prefijo si utiliza y factura por separado, si no la plataforma envia las dos propiedades en numero_factura
				string NumFac = string.Empty;
				if (Erp)
				{
					NumFac = string.Format("{0}{1}", prefijo, numero_factura);
				}
				else
				{
					NumFac = numero_factura;
				}

				// formato para validación de valor con dos decimales
				Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");


				if (subtotal == 0)
					subtotal = Convert.ToDecimal(0.00M);
				else if (!isnumber.IsMatch(Convert.ToString(subtotal).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Total {0} no esta bien formado", subtotal));

				string ValFac = subtotal.ToString();

				//Impuesto 1
				string CodImp1 = "01";
				decimal ValImp1 = iva;

				//Valida el iva
				if (ValImp1 == 0)
					ValImp1 = Convert.ToDecimal(0.00M);
				else if (!isnumber.IsMatch(Convert.ToString(ValImp1).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor iva {0} no esta bien formado", iva));


				//Impuesto 2
				string CodImp2 = "04";
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

				if (total == 0)
					total = Convert.ToDecimal(0.00M);
				else if (!isnumber.IsMatch(Convert.ToString(total).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Total {0} no esta bien formado", total));

				string ValImp = total.ToString();

				string NitOFE = nit_facturador;

				string NumAdq = nit_adquiriente;

				string cufe = NumFac
					+ fecha_factura
					+ ValFac.Replace(",", ".")
					+ CodImp1
					+ ValImp1.ToString().Replace(",", ".")
					+ CodImp2
					+ ValImp2.ToString().Replace(",", ".")
					+ CodImp3
					+ ValImp3.ToString().Replace(",", ".")
					+ ValImp.Replace(",", ".")
					+ NitOFE
					+ NumAdq
					+ clave_tecnica
					+ ambiente
				;

				string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
				return cufe_encriptado;
				#endregion
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Calcula el código CUFE de la nota crédito
		/// </summary>
		/// <param name="clave_tecnica">Clave técnica de la resolución de la factura</param>
		/// <param name="cufe_factura">Cufe de la factura</param>
		/// <param name="prefijo">Prefijo de la Nota credito</param>
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
		/// <param name="Erp">Indica si el llamado lo esta haciendo el ERP para la asignacion del numero del documento</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CufeNotaCredito(string clave_tecnica, string cufe_factura, string prefijo, string numero_nota_credito, DateTime fecha_nota_credito, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica, bool Erp)
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

				string NumCr = string.Empty;

				if (Erp)
				{
					NumCr = string.Format("{0}{1}", prefijo, numero_nota_credito);
				}
				else
				{
					NumCr = numero_nota_credito;
				}

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

		/// <summary>
		/// Calcula el código CUFE de la nota crédito
		/// </summary>
		/// <param name="cufe_factura">Cufe de la factura</param>
		/// <param name="prefijo">Prefijo de la Nota credito</param>
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
		/// <param name="Erp">Indica si el llamado lo esta haciendo el ERP para la asignacion del numero del documento</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CufeNotaCreditoV2(string pin_software, string prefijo, string numero_nota_credito, string fecha_nota_credito, string nit_facturador, string ambiente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica, bool Erp)
		{
			try
			{

				if (string.IsNullOrWhiteSpace(pin_software))
					throw new Exception(string.Format("Parámetro {0} inválido.", "pin_software"));

				if (string.IsNullOrWhiteSpace(numero_nota_credito))
					throw new Exception(string.Format("Parámetro {0} inválido.", "numero_nota_credito"));

				if (string.IsNullOrWhiteSpace(nit_facturador))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_facturador"));

				if (string.IsNullOrWhiteSpace(ambiente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "ambiente"));

				if (string.IsNullOrWhiteSpace(nit_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_adquiriente"));

				#region Documentación de la creación código CUDE
				/*
				NumCr = Número de Nota Credito.
				Feccr = Fecha de Nota Credito en formato (Java) YYYYmmddHHMMss. 
				ValCr = Valor Nota Credito sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp1 = 01 
				ValImp1 = Valor impuesto 01, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp2 = 02 
				ValImp2 = Valor impuesto 04, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp3 = 03 
				ValImp3 = Valor impuesto 03, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				ValImp = Valor IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				NitOFE = NIT del Facturador Electrónico sin puntos ni guiones, sin digito de verificación. 
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				Software-PIN = Pin del software registrado en el catalogo del participante, el cual no esta expresado en el XML 
				TipoAmbiente = Número de identificación del ambiente utilizado por el contribuyente para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas) 

				Composición del CUFE = SHA-384(NumCr + Feccr + ValCr + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 + ValImp3 + ValTot + NitOFE +  NumAdq + Software-PIN + TipoAmbie)  
			
			
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
				Software-PIN = No se encuentra en el XML 
				NumAdq = /fe:CreditNote /fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipoAmb = /CreditNote/dcc:ProfileExecutionID  			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;

				string NumCr = string.Empty;

				if (Erp)
				{
					NumCr = string.Format("{0}{1}", prefijo, numero_nota_credito);
				}
				else
				{
					NumCr = numero_nota_credito;
				}

				string FecCr = fecha_nota_credito;

				if (subtotal == 0)
					subtotal = Convert.ToDecimal(0.00M);

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
				string CodImp2 = "04";
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

				if (total == 0)
					total = Convert.ToDecimal(0.00M);

				string ValImp = total.ToString();

				string NitOFE = nit_facturador;

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
				   + NumAdq
				   + pin_software
				   + ambiente
				   ;

				string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
				return cufe_encriptado;
				#endregion
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Calcula el código CUFE de la nota débito
		/// </summary>
		/// <param name="clave_tecnica">Clave técnica de la resolución de la factura</param>
		/// <param name="cufe_factura">Cufe de la factura</param>
		/// <param name="prefijo">Prefijo de la Nota Debito</param>
		/// <param name="numero_nota_debito">Número de la nota débito</param>
		/// <param name="fecha_nota_debito">Fecha de la nota débito</param>
		/// <param name="nit_facturador">Número de identificación del facturador electrónico</param>
		/// <param name="tipo_identificacion_adquiriente">Tipo de identificación del facturador electrónico</param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la nota débito</param>
		/// <param name="subtotal">Subtotal de la nota débito</param>
		/// <param name="iva">Iva de la nota débito</param>
		/// <param name="impto_consumo">Impuesto al consumo de la nota débito</param>
		/// <param name="rte_ica">Retención del ICA de la nota débito</param>
		/// <param name="Erp">Indica si el llamado lo esta haciendo el ERP para la asignacion del numero del documento</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CufeNotaDebito(string clave_tecnica, string cufe_factura, string prefijo, string numero_nota_debito, DateTime fecha_nota_debito, string nit_facturador, string tipo_identificacion_adquiriente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica, bool erp)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(clave_tecnica))
					throw new Exception(string.Format("Parámetro {0} inválido.", "clave_tecnica"));

				if (string.IsNullOrWhiteSpace(cufe_factura))
					throw new Exception(string.Format("Parámetro {0} inválido.", "cufe_factura"));

				if (string.IsNullOrWhiteSpace(numero_nota_debito))
					throw new Exception(string.Format("Parámetro {0} inválido.", "numero_nota_debito"));

				if (string.IsNullOrWhiteSpace(nit_facturador))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_facturador"));

				if (string.IsNullOrWhiteSpace(tipo_identificacion_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "tipo_identificacion_adquiriente"));

				if (string.IsNullOrWhiteSpace(nit_adquiriente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_adquiriente"));

				#region Documentación de la creación código CUFE
				/*
				NumDb = Número de Nota Dbedito.
				Feccr = Fecha de Nota Dbedito en formato (Java) YYYYmmddHHMMss. 
				ValDb = Valor Nota Dbedito sin IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
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
			
				NumDb = /fe:DbeditNote/cbc:ID
				FecDb = sinSimbolos(/fe:DbeditNote/cbc:IssueDate + /fe:DbeditNote/cbc:IssueTime)
						formato AAAAMMDDHHMMSS i.e. año + mes + día + hora + minutos + segundos
				ValDb = /fe:DbeditNote/fe:LegalMonetaryTotal/cbc:LineExtensionAmount
				CodImp1 = /fe:DbeditNote/fe:TaxTotal[X]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 01
				ValImp1 = /fe:DbeditNote/fe:TaxTotal[X]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp2 = /fe:DbeditNote/fe:TaxTotal[y]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 02
				ValImp2 = /fe:DbeditNote/fe:TaxTotal[y]/fe:TaxSubtotal/cbc:TaxAmount
				CodImp3 = /fe:DbeditNote/fe:TaxTotal[z]/fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID = 03
				ValImp3 = /fe:DbeditNote/fe:TaxTotal[z]/fe:TaxSubtotal/cbc:TaxAmount
				ValPag = /fe:DbeditNote/fe:LegalMonetaryTotal/cbc:PayableAmount
				NitOFE = /fe:DbeditNote/fe:AccountingSupplierParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipAdq = /fe:DbeditNote/fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID/@schemeID
				NumAdq = /fe:DbeditNote /fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				ClTec = Examine la sección Cálculos para Nota Db. 			 
				*/
				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;
				DateTime fecha = fecha_nota_debito;
				DateTime fecha_hora = Convert.ToDateTime(fecha_nota_debito);
				TimeSpan hora = new TimeSpan(fecha_hora.Hour, fecha_hora.Minute, fecha_hora.Second);
				fecha = fecha.Date + hora;

				string NumDb = string.Empty;
				if (erp)
				{
					NumDb = string.Format("{0}{1}", prefijo, numero_nota_debito);
				}
				else
				{
					NumDb = numero_nota_debito;
				}

				string FecDb = fecha.ToString(Fecha.formato_fecha_java);
				string ValDb = subtotal.ToString();


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


				string cufe = NumDb
					+ FecDb
					+ ValDb.Replace(",", ".")
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

		/// <summary>
		/// Calcula el código CUFE de la nota débito
		/// </summary>
		/// <param name="pin_software">Pin del software registrado en el catalogo del participante, el cual no esta expresado en el XML</param>
		/// <param name="prefijo">Prefijo de la Nota Debito</param>
		/// <param name="numero_nota_debito">Número de la nota débito</param>
		/// <param name="fecha_nota_debito">Fecha de la nota débito</param>
		/// <param name="nit_facturador">Número de identificación del facturador electrónico</param>
		/// <param name="ambiente">Ambiente a donde se va enviar el documento </param>
		/// <param name="nit_adquiriente">Número de identificación del adquiriente</param>
		/// <param name="total">Total de la nota débito</param>
		/// <param name="subtotal">Subtotal de la nota débito</param>
		/// <param name="iva">Iva de la nota débito</param>
		/// <param name="impto_consumo">Impuesto al consumo de la nota débito</param>
		/// <param name="rte_ica">Retención del ICA de la nota débito</param>
		/// <param name="Erp">Indica si el llamado lo esta haciendo el ERP para la asignacion del numero del documento</param>
		/// <returns>Texto con la encriptación del CUFE</returns>
		public static string CufeNotaDebitoV2(string pin_software, string prefijo, string numero_nota_debito, string fecha_nota_debito, string nit_facturador, string ambiente, string nit_adquiriente, decimal total, decimal subtotal, decimal iva, decimal impto_consumo, decimal rte_ica, bool erp)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(pin_software))
					throw new Exception(string.Format("Parámetro {0} inválido.", "pin_software"));

				if (string.IsNullOrWhiteSpace(numero_nota_debito))
					throw new Exception(string.Format("Parámetro {0} inválido.", "numero_nota_debito"));

				if (string.IsNullOrWhiteSpace(nit_facturador))
					throw new Exception(string.Format("Parámetro {0} inválido.", "nit_facturador"));

				if (string.IsNullOrWhiteSpace(ambiente))
					throw new Exception(string.Format("Parámetro {0} inválido.", "ambiente"));

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
				ValImp2 = Valor impuesto 04, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos. 
				CodImp3 = 03 
				ValImp3 = Valor impuesto 03, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				ValImp = Valor IVA, con punto decimal, con decimales a dos (2) dígitos, sin separadores de miles, ni símbolo pesos.
				NitOFE = NIT del Facturador Electrónico sin puntos ni guiones, sin digito de verificación. 
				NumAdq = Número de identificación del adquirente sin puntos ni guiones, sin digito de verificación. 
				Software-PIN = Pin del software registrado en el catalogo del participante, el cual no esta expresado en el XML 
				TipoAmbiente = Número de identificación del ambiente utilizado por el contribuyente para emitir la factura Seccion 6.1.1 (1=AmbienteProduccion , 2: AmbientePruebas) 

				Composición del CUFE = SHA-384(NumCr + Feccr + ValCr + CodImp1 + ValImp1 + CodImp2 + ValImp2 + CodImp3 + ValImp3 + ValTot + NitOFE +  NumAdq + Software-PIN + TipoAmbie)  
			
			
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
				Software-PIN = No se encuentra en el XML 
				NumAdq = /fe:CreditNote /fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID
				TipoAmb = /CreditNote/dcc:ProfileExecutionID
				*/

				#endregion

				#region Creación Código CUFE

				string codigo_impuesto = string.Empty;

				string NumDb = string.Empty;
				if (erp)
				{
					NumDb = string.Format("{0}{1}", prefijo, numero_nota_debito);
				}
				else
				{
					NumDb = numero_nota_debito;
				}

				string FecDb = fecha_nota_debito;
				string ValDb = subtotal.ToString();


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
				string CodImp2 = "04";
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

				string NumAdq = nit_adquiriente;


				string cufe = NumDb
					+ FecDb
					+ ValDb.Replace(",", ".")
					+ CodImp1
					+ ValImp1.ToString().Replace(",", ".")
					+ CodImp2
					+ ValImp2.ToString().Replace(",", ".")
					+ CodImp3
					+ ValImp3.ToString().Replace(",", ".")
					+ ValImp.Replace(",", ".")
					+ NitOFE
					+ NumAdq
					+ pin_software
					+ ambiente
				;

				string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
				return cufe_encriptado;
				#endregion
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene el código QR del documento electrónico para la representación gráfica
		/// </summary>
		/// <param name="DocumentoTipo"></param>
		/// <param name="Prefijo"></param>
		/// <param name="NumDocumento"></param>
		/// <param name="FechaDocumento"></param>
		/// <param name="IdentificacionObligado"></param>
		/// <param name="IdentificacionAdquiriente"></param>
		/// <param name="SubTotal"></param>
		/// <param name="Iva"></param>
		/// <param name="ImpConsumo"></param>
		/// <param name="Total"></param>
		/// <param name="CufeCude"></param>
		/// <returns></returns>
		public static string ObtenerQR(int DocumentoTipo, string Prefijo, long NumDocumento, DateTime FechaDocumento, string IdentificacionObligado, string IdentificacionAdquiriente, decimal SubTotal, decimal Iva, decimal ImpConsumo, decimal Total, string CufeCude, string ruta_validar_doc)
		{
			string QR = "";

			try
			{
				string DescripcionDoc = "";
				string DescripcionFecha = "";
				string DescripcionHora = "";
				string DescripcionValor = "";
				string DescripcionTotal = "";
				string DescripcionCufe = "";

				if (DocumentoTipo == 1)
				{
					DescripcionDoc = "NumFac:";
					DescripcionFecha = "FecFac:";
					DescripcionHora = "HoraFac:";
					DescripcionValor = "ValFac:";
					DescripcionTotal = "ValTolFac:";
					DescripcionCufe = "CUFE:";
				}
				else if (DocumentoTipo == 2)
				{
					DescripcionDoc = "NumDb:";
					DescripcionFecha = "FecDb:";
					DescripcionHora = "HoraDb:";
					DescripcionValor = "ValDb:";
					DescripcionTotal = "ValTolDb:";
					DescripcionCufe = "CUDE:";
				}
				else if (DocumentoTipo == 3)
				{
					DescripcionDoc = "NumCr:";
					DescripcionFecha = "FecCr:";
					DescripcionHora = "HoraCr:";
					DescripcionValor = "ValCr:";
					DescripcionTotal = "ValTolCr:";
					DescripcionCufe = "CUDE:";
				}

				//@DescripcionDoc + cast(TblTransacciones.StrPrefijo as varchar) + cast(intDocumento as varchar) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", DescripcionDoc, Prefijo, NumDocumento);

				//(@DescripcionFecha + CAST((CAST(year(DatFecha)AS varchar) + '-' + RIGHT('00' + convert(varchar, Month(DatFecha)), 2) + '-' + RIGHT('00' + convert(varchar, day(DatFecha)), 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionFecha, FechaDocumento.ToString(Fecha.formato_fecha_hginet));

				//(@DescripcionHora + RIGHT('00' + convert(varchar, DATEPART(hour, DATEADD(hour, 5, tbldocumentos.DatFechaGra))), 2) + ':' + RIGHT('00' + convert(varchar, DATEPART(minute, tbldocumentos.DatFechaGra)), 2) + ':' + RIGHT('00' + convert(varchar, DATEPART(second, tbldocumentos.DatFechaGra)), 2) + '-5:00') + CHAR(10) +
				QR = string.Format("{0}{1}{2}{3}\n", QR, DescripcionHora, FechaDocumento.ToString(Fecha.formato_hora_completa), "-05:00");

				//('NitFac:' + TblEmpresas.StrNit) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "NitFac:", IdentificacionObligado);

				//('DocAdq:' + tbldocumentos.strtercero) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "DocAdq:", IdentificacionAdquiriente);

				//(@DescripcionValor + CAST(CAST(tbldocumentos.IntSubTotal As numeric(17, 2)) as varchar)) + Char(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionValor, SubTotal.ToString());

				//('ValIva:' + CAST(CAST(tbldocumentos.IntIva AS numeric(17, 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "ValIva:", Iva.ToString());

				//('ValOtroIm:' + CAST(CAST(tbldocumentos.IntVrImpuesto1 AS numeric(17, 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "ValOtroIm:", ImpConsumo.ToString());

				//(@DescripcionTotal + CAST(CAST(tbldocumentos.IntTotal AS numeric(17, 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionTotal, Total.ToString());
				
				//(@DescripcionCufe + ':' + tbldocumentos.strFacturaECufe)
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionCufe, CufeCude);

				QR = string.Format("{0}{1}\n", QR, ruta_validar_doc);

				return QR;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
