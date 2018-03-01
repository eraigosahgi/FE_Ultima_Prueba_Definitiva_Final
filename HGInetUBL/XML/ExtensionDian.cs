using ConexionDatosHGInet;
using HGInetUBL.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace HGInetUBL
{
	public class ExtensionDian
	{
		/// <summary>
		/// Obtiene la extension requerida por la Dian
		/// </summary>
		/// <param name="documento">Datos del documento</param>
		/// <returns> XmlElement que contiene la extension requerida por la Dian</returns>
		public static XmlElement Obtener(TblDocumentos documento, TblEmpresas empresa, TipoDocumento tipo_doc)
		{

			string IdentificadorSoftware = documento.TblTransacciones.StrIdentificadorDIAN;//Identificador del software proporcionado en la plataforma de la DIAN

			if (String.IsNullOrEmpty(IdentificadorSoftware))
				throw new Exception("El identificador de la DIAN en la transacción es inválido.");

			string Pin = documento.TblTransacciones.StrPinDIAN;//Pin del software proporcionado en la plataforma de la DIAN

			if (String.IsNullOrEmpty(Pin))
				throw new Exception("El PIN de la DIAN en la transacción es inválido.");

			string software_security_code = string.Format("{0}{1}", IdentificadorSoftware, Pin);
			string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);//Encriptación en SHA384 del string que contiene Identificador y el Pin del software


			DianExtensionsType DianExtensions = new DianExtensionsType();
			if (tipo_doc == TipoDocumento.Factura)
			{
				InvoiceControl InvoiceControl = new InvoiceControl();

				#region Resolución de la numeración

				if (String.IsNullOrEmpty(documento.TblTransacciones.StrNumResolucion))
					throw new Exception("El número de resolución de la DIAN en la transacción es inválido.");

				InvoiceControl.InvoiceAuthorization = Convert.ToDecimal(documento.TblTransacciones.StrNumResolucion);
				#endregion

				#region Fechas de inicio y fin de la resolución

				if (documento.TblTransacciones.DatFechaIniResol == null)
					throw new Exception("La fecha inicial de la resolución de la DIAN en la transacción es inválida.");

				PeriodType AuthorizationPeriod = new PeriodType();
				StartDateType StartDate = new StartDateType();
				StartDate.Value = documento.TblTransacciones.DatFechaIniResol.Value;
				AuthorizationPeriod.StartDate = StartDate;

				if (documento.TblTransacciones.DatFechaResol == null)
					throw new Exception("La fecha final de la resolución de la DIAN en la transacción es inválida.");

				EndDateType EndDate = new EndDateType();
				EndDate.Value = documento.TblTransacciones.DatFechaResol.Value;
				AuthorizationPeriod.EndDate = EndDate;
				InvoiceControl.AuthorizationPeriod = AuthorizationPeriod;
				#endregion

				#region Prefijo y rangos resolución
				AuthrorizedInvoices AuthorizedInvoices = new AuthrorizedInvoices();

				if (!documento.TblTransacciones.StrPrefijo.Equals(string.Empty)) //Si tiene prefijo lo agrega
				{
					TextType Prefix = new TextType();
					Prefix.Value = documento.TblTransacciones.StrPrefijo;
					AuthorizedInvoices.Prefix = Prefix;
				}

				AuthorizedInvoices.From = documento.TblTransacciones.IntDocI;
				AuthorizedInvoices.To = documento.TblTransacciones.IntDocF;
				InvoiceControl.AuthorizedInvoices = AuthorizedInvoices;
				#endregion

				DianExtensions.InvoiceControl = InvoiceControl;
			}

			//PENDIENTE!!
			#region Datos de expedición de la factura
			/*País de expedición de la factura
            REGLA: Para el DOCUMENTO FACTURA DE VENTA, el valor de este elemento será “CO” */
			CountryType InvoiceSource = new CountryType();
			IdentificationCodeType IdentificationCode = new IdentificationCodeType();
			IdentificationCode.Value = "CO";
			InvoiceSource.IdentificationCode = IdentificationCode;
			DianExtensions.InvoiceSource = InvoiceSource;
			#endregion

			#region Información del software autorizado

			/*   Descripción: contenedor de las informaciones sobre el software autorizado para expedir facturas
                electrónicas, y la identificación del proveedor de dicho software.
                En general, hay dos tipos de proveedores: 1. El Facturador Electrónico y 2. Un Proveedor
                Tecnológico. El valor del elemento “ProviderID” corresponde al NIT del Facturador Electrónico o
                del Proveedor Tecnológico. Los dos elementos que contiene SoftwareProvider siempre van juntos.  */

			#region NIT del Facturador Electrónico o del Proveedor Tecnológico
			SoftwareProvider SoftwareProvider = new SoftwareProvider();
			IdentifierType ProviderID = new IdentifierType();
			ProviderID.Value = "811021438";
			SoftwareProvider.ProviderID = ProviderID;
			#endregion

			#region Identificador del software autorizado
			IdentifierType SoftwareID = new IdentifierType();
			SoftwareID.Value = IdentificadorSoftware;
			SoftwareProvider.SoftwareID = SoftwareID;
			DianExtensions.SoftwareProvider = SoftwareProvider;
			#endregion

			#endregion

			#region Código de seguridad del software
			/*  Código de seguridad del software para el ambiente de operación, el
                cual se calcula con base a un algoritmo que recibe la clave de contenido técnico de control
                y otros valores propios del Facturador Electrónico; la clave mencionada se asigna a cada rango de
                facturación autorizado; es un número de 96 caracteres hexadecimales; siempre va aparejado con
                el contenido del elemento AuthorizedInvoices.*/
			//SoftwareSecurityCode= sha-384(Identificador del software + PIN del software)
			IdentifierType SoftwareSecurityCode = new IdentifierType();
			SoftwareSecurityCode.Value = software_security_code_encriptado;
			DianExtensions.SoftwareSecurityCode = SoftwareSecurityCode;
			#endregion

			XmlSerializerNamespaces serializer = NamespacesXML.ObtenerExtensionDian();
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;

			xmlSerializer = new XmlSerializer(DianExtensions.GetType());
			MemoryStream memStream = new MemoryStream();
			stWriter = new StreamWriter(memStream);

			xmlSerializer.Serialize(stWriter, DianExtensions, serializer);
			buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

			XmlDocument extension_dian = new XmlDocument();
			extension_dian.LoadXml(buffer);
			return extension_dian.DocumentElement;
		}
	}
}
