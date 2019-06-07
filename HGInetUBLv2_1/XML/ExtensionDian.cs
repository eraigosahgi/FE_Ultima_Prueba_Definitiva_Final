using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public class ExtensionDian
	{


		/// <summary>
		/// Genera el elemento XML para la extensión de la DIAN
		/// </summary>
		public static XmlElement Obtener(HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension, TipoDocumento tipo_doc, string numero_documento)
		{

			string IdentificadorSoftware = extension.IdSoftware;//Identificador del software proporcionado en la plataforma de la DIAN

			if (String.IsNullOrEmpty(IdentificadorSoftware))
				throw new Exception("El identificador de la DIAN en la transacción es inválido.");

			string Pin = extension.PinSoftware;//Pin del software proporcionado en la plataforma de la DIAN

			if (String.IsNullOrEmpty(Pin))
				throw new Exception("El PIN de la DIAN en la transacción es inválido.");

			string software_security_code = string.Format("{0}{1}{2}", IdentificadorSoftware, Pin, numero_documento);
			string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);//Encriptación en SHA384 del string que contiene Identificador y el Pin del software


			DianExtensionsType DianExtensions = new DianExtensionsType();
			if (tipo_doc == TipoDocumento.Factura)
			{
				InvoiceControl InvoiceControl = new InvoiceControl();

				#region Resolución de la numeración 

				if (String.IsNullOrEmpty(extension.NumResolucion))
					throw new Exception("El número de resolución de la DIAN en la transacción es inválido.");

				InvoiceControl.InvoiceAuthorization = new NumericType1();
				InvoiceControl.InvoiceAuthorization.Value = Convert.ToDecimal(extension.NumResolucion);

				#endregion

				#region Fechas de inicio y fin de la resolución

				if (extension.FechaResIni == null)
					throw new Exception("La fecha inicial de la resolución de la DIAN en la transacción es inválida.");

				PeriodType AuthorizationPeriod = new PeriodType();
				StartDateType StartDate = new StartDateType();
				StartDate.Value = extension.FechaResIni;
				AuthorizationPeriod.StartDate = StartDate;

				if (extension.FechaResFin == null)
					throw new Exception("La fecha final de la resolución de la DIAN en la transacción es inválida.");

				EndDateType EndDate = new EndDateType();
				EndDate.Value = extension.FechaResFin;
				AuthorizationPeriod.EndDate = EndDate;
				InvoiceControl.AuthorizationPeriod = AuthorizationPeriod;
				#endregion

				#region Prefijo y rangos resolución
				AuthrorizedInvoices AuthorizedInvoices = new AuthrorizedInvoices();

				if (!string.IsNullOrEmpty(extension.Prefijo)) //Si tiene prefijo lo agrega
				{
					TextType Prefix = new TextType();
					Prefix.Value = extension.Prefijo;
					AuthorizedInvoices.Prefix = Prefix.Value.ToString();
				}

				AuthorizedInvoices.From = extension.RangoIni;
				AuthorizedInvoices.To = extension.RangoFin;
				InvoiceControl.AuthorizedInvoices = AuthorizedInvoices;
				#endregion

				DianExtensions.InvoiceControl = InvoiceControl;
			}


			#region Datos de expedición de la factura
			/*País de expedición de la factura
            REGLA: Para el DOCUMENTO FACTURA DE VENTA, el valor de este elemento será “CO” */
			CountryType InvoiceSource = new CountryType();
			IdentificationCodeType IdentificationCode = new IdentificationCodeType();
			IdentificationCode.Value = extension.CodPaisExpedicion;
			IdentificationCode.listAgencyID = "6";
			IdentificationCode.listAgencyName = "United Nations Economic Commission for Europe";
			IdentificationCode.listSchemeURI = "urn:oasis:names:specification:ubl:codelist:gc:CountryIdentificationCode-2.1";
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
			SoftwareProvider.ProviderID = new coID2Type();
			SoftwareProvider.ProviderID.Value = extension.NitProveedor;
			SoftwareProvider.ProviderID.schemeID = coID2TypeSchemeID.Item4;
			SoftwareProvider.ProviderID.schemeName = "31";
			SoftwareProvider.ProviderID.schemeAgencyID = coID2TypeSchemeAgencyID.Item195;
			SoftwareProvider.ProviderID.schemeAgencyName = coID2TypeSchemeAgencyName.CODIANDireccióndeImpuestosyAduanasNacionales;
			#endregion

			#region Identificador del software autorizado
			IdentifierType1 SoftwareID = new IdentifierType1();
			SoftwareID.Value = IdentificadorSoftware;
			SoftwareProvider.SoftwareID = SoftwareID;
			SoftwareProvider.SoftwareID.schemeAgencyID = "195";
			SoftwareProvider.SoftwareID.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
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
			IdentifierType1 SoftwareSecurityCode = new IdentifierType1();
			SoftwareSecurityCode.Value = software_security_code_encriptado;
			SoftwareSecurityCode.schemeAgencyID = "195";
			SoftwareSecurityCode.schemeAgencyName = "CO, DIAN (Dirección de Impuestos y Aduanas Nacionales)";
			DianExtensions.SoftwareSecurityCode = SoftwareSecurityCode;
			#endregion


			DianExtensions.QRCode = "Cadena para calcular el CUFE";


			AuthorizationProvider ProviderAuthorization = new AuthorizationProvider();
			ProviderAuthorization.AuthorizationProviderID = new coID2Type();
			ProviderAuthorization.AuthorizationProviderID.Value = "800197268";
			ProviderAuthorization.AuthorizationProviderID.schemeID = coID2TypeSchemeID.Item4;
			ProviderAuthorization.AuthorizationProviderID.schemeName = "31";
			ProviderAuthorization.AuthorizationProviderID.schemeAgencyID = coID2TypeSchemeAgencyID.Item195;
			ProviderAuthorization.AuthorizationProviderID.schemeAgencyName = coID2TypeSchemeAgencyName.CODIANDireccióndeImpuestosyAduanasNacionales;
			DianExtensions.AuthorizationProvider = ProviderAuthorization;

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
