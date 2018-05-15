using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

using FirmaXadesNet;
using FirmaXadesNet.Signature;
using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Crypto;
using HGInetFirmaDigital.Properties;
using LibreriaGlobalHGInet.General;
using System.Security.Cryptography.Xml;
using LibreriaGlobalHGInet.Funciones;
using FirmaXadesNet.Utils;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetFirmaDigital
{
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ProgId("firmadoXades")]
	/* Permitimos que esta clase pueda ser exportada como interfaz COM, para ser utilizada en entornos no .NET también. */
	[Synchronization(SynchronizationOption.Required), JustInTimeActivation(true), ObjectPooling(Enabled = true)]
	public class Ctl_FirmadoXml
	{
		/// <summary>
		/// Firma de archivos con formato XADES-EPES
		/// </summary>
		/// <param name="certificado_ruta">Ruta física del archivo del Certificado "C:/Certificado.pfx";</param>
		/// <param name="certificado_serial">Número de serie del certificado digital "‎‎74 c7 43 04 a4 ac 86 b6"</param>
		/// <param name="certificado_clave">Clave del certificado digital</param>
		/// <param name="empresa_certificadora">empresa certificadora</param>
		/// <param name="rutas">Archivos XML para firmar</param>
		/// <returns>Archivos Xml procesados</returns>  
		public static List<FacturaE_Documento> FirmarDocumentos(string certificado_ruta, string certificado_serial, string certificado_clave, EnumCertificadoras empresa_certificadora, List<FacturaE_Documento> datos)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(certificado_serial))
					throw new Exception("El serial del certificado digital es inválido.");

				Ctl_FirmadoXml Firmar = new Ctl_FirmadoXml();

				// firmado de archivos XML desde el almacen de certificados
				if (string.IsNullOrEmpty(certificado_ruta))
					datos = Firmar.FirmarXAdesAlmacen(certificado_serial, certificado_clave, empresa_certificadora, datos);
				// firmado de archivos XML desde un certificado físico
				else
					datos = Firmar.FirmarXAdesFisico(certificado_ruta, certificado_serial, certificado_clave, empresa_certificadora, datos);

				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Firmado de varios archivos XML a través del certificado físico
		/// </summary>
		/// <param name="RutaCertificado">Ruta física del archivo del Certificado "C:/Certificado.pfx";</param>
		/// <param name="SerieCertificado">Número de Serie del Certificado "‎‎74 c7 43 04 a4 ac 86 b6"</param>
		/// <param name="ClaveCertificado">Clave del Certificado</param>
		/// <param name="EmpresaCertificadora">Empresa que generó el certificado</param>
		/// <param name="rutas_archivos">Archivos XML para firmar</param>
		/// <returns>Archivos Xml procesados</returns>
		[AutoComplete(true)]
		protected List<FacturaE_Documento> FirmarXAdesFisico(string RutaCertificado, string SerieCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, List<FacturaE_Documento> datos)
		{
			X509Certificate2 MontCertificat = null;
			SignatureParameters parametros = null;
			try
			{
				if (!Archivo.ValidarExistencia(RutaCertificado))
				{
					throw new ApplicationException(string.Format("No se encuentra el certificado en la ruta {0}", RutaCertificado));
				}



				XadesService xadesService = new XadesService();
				parametros = new SignatureParameters();

				parametros.SignatureMethod = SignatureMethod.RSAwithSHA1;
				parametros.DigestMethod = DigestMethod.SHA1;

				parametros.SignerRole = new SignerRole();
				parametros.SignerRole.ClaimedRoles.Add("supplier");

				parametros.DatoIssuername = "";
				parametros.DatoIssuername1 = "";
				parametros.DatoIssuername0 = "";

				// parámetros propios de la empresa que provee el certificado digital
				if (EmpresaCertificadora == EnumCertificadoras.Gse)
				{
					parametros.DatoIssuername0 = CertificadorasDatos.GSE_DatoIssuername0;
					parametros.DatoIssuername1 = CertificadorasDatos.GSE_DatoIssuername1;
					parametros.DatoIssuername = CertificadorasDatos.GSE_DatoIssuername;
				}
				else if (EmpresaCertificadora == EnumCertificadoras.Andes)
				{
					parametros.DatoIssuername0 = CertificadorasDatos.Andes_DatoIssuername0;
					parametros.DatoIssuername1 = CertificadorasDatos.Andes_DatoIssuername1;
					parametros.DatoIssuername = CertificadorasDatos.Andes_DatoIssuername;
				}

				//inserta en nodo
				parametros.SignatureDestination = new SignatureXPathExpression();
				parametros.SignatureDestination.Namespaces.Add("fe", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
				parametros.SignatureDestination.Namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
				parametros.SignatureDestination.Namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
				parametros.SignatureDestination.Namespaces.Add("clm54217", "urn:un:unece:uncefact:codelist:specification:54217:2001");
				parametros.SignatureDestination.Namespaces.Add("clm66411", "urn:un:unece:uncefact:codelist:specification:66411:2001");
				parametros.SignatureDestination.Namespaces.Add("clmIANAMIMEMediaType", "urn:un:unece:uncefact:codelist:specification:IANAMIMEMediaType:2003");
				parametros.SignatureDestination.Namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
				parametros.SignatureDestination.Namespaces.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
				parametros.SignatureDestination.Namespaces.Add("sts", "http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures");
				parametros.SignatureDestination.Namespaces.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
				parametros.SignatureDestination.Namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

				// Política de firma 
				parametros.SignaturePolicyInfo = new SignaturePolicyInfo();
				parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf"; //JOSE CARLOS AGRUEGUE ESTA Y COMENTE LA ANTERIOR
				parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc="; //SEA AGREGO
				parametros.cDescripcionPolicy = "Política de firma para facturas electrónicas de la República de Colombia";
				parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
				parametros.InputMimeType = "text/xml";

				//"E:/certificadrdb/Certificado_Sunat_PFX_REPDORABEATRIZ.pfx";
				MontCertificat = new X509Certificate2(RutaCertificado, ClaveCertificado);

				parametros.cNombreCertificado = RutaCertificado;
				parametros.cClaveCertificado = ClaveCertificado;
				parametros.cdesdealmacen = "NO";

			}
			catch (Exception excepcion)
			{

				string msg = string.Format("Error HGInet Firmado al leer el certificado {1} de {3} clave:{2} - Excepción: {0}", excepcion.Message, RutaCertificado, ClaveCertificado, Enumeracion.GetDescription(EmpresaCertificadora));

				if (excepcion.InnerException != null)
					msg += " Inner: " + excepcion.InnerException.Message;

				throw new ApplicationException(msg, excepcion);
			}


			try
			{

				// generar firma en el StringBuilder
				datos = Firmar(datos, parametros, MontCertificat);

			}
			catch (Exception excepcion)
			{

				string msg = string.Format("Error HGInet Firmado - Excepción: {0}", excepcion.Message);

				throw new ApplicationException(msg, excepcion);
			}

			return datos;
		}

		/// <summary>
		/// Firmado de varios archivos XML a través del almacen de certificados
		/// </summary>
		/// <param name="SerieCertificado">Número de Serie del Certificado "‎‎74 c7 43 04 a4 ac 86 b6"</param>
		/// <param name="ClaveCertificado">Clave del Certificado</param>
		/// <param name="EmpresaCertificadora">Empresa que generó el certificado</param>
		/// <param name="rutas_archivos">Archivos XML para firmar</param>
		/// <returns>Archivos Xml procesados</returns>
		[AutoComplete(true)]
		protected List<FacturaE_Documento> FirmarXAdesAlmacen(string SerieCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, List<FacturaE_Documento> datos)
		{

			try
			{
				XadesService xadesService = new XadesService();
				SignatureParameters parametros = new SignatureParameters();

				parametros.SignatureMethod = SignatureMethod.RSAwithSHA1;
				parametros.DigestMethod = DigestMethod.SHA1;

				parametros.SignerRole = new SignerRole();
				parametros.SignerRole.ClaimedRoles.Add("supplier");

				parametros.DatoIssuername = "";
				parametros.DatoIssuername1 = "";
				parametros.DatoIssuername0 = "";

				//inserta en nodo
				parametros.SignatureDestination = new SignatureXPathExpression();
				parametros.SignatureDestination.Namespaces.Add("fe", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
				parametros.SignatureDestination.Namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
				parametros.SignatureDestination.Namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
				parametros.SignatureDestination.Namespaces.Add("clm54217", "urn:un:unece:uncefact:codelist:specification:54217:2001");
				parametros.SignatureDestination.Namespaces.Add("clm66411", "urn:un:unece:uncefact:codelist:specification:66411:2001");
				parametros.SignatureDestination.Namespaces.Add("clmIANAMIMEMediaType", "urn:un:unece:uncefact:codelist:specification:IANAMIMEMediaType:2003");
				parametros.SignatureDestination.Namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
				parametros.SignatureDestination.Namespaces.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
				parametros.SignatureDestination.Namespaces.Add("sts", "http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures");
				parametros.SignatureDestination.Namespaces.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
				parametros.SignatureDestination.Namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

				// Política de firma 
				parametros.SignaturePolicyInfo = new SignaturePolicyInfo();
				parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf"; //JOSE CARLOS AGRUEGUE ESTA Y COMENTE LA ANTERIOR
				parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc="; //SEA AGREGO
				parametros.cDescripcionPolicy = "Política de firma para facturas electrónicas de la República de Colombia";

				parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
				parametros.InputMimeType = "text/xml";
				parametros.cNombreCertificado = "";
				parametros.cClaveCertificado = ClaveCertificado;
				parametros.cdesdealmacen = "SI";

				var rgx = new Regex("[^a-fA-F0-9]");
				var serial = rgx.Replace(SerieCertificado, string.Empty).ToUpper();
				X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
				store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection Certificados1 = (X509Certificate2Collection)store.Certificates;
				X509Certificate2Collection Certificados3 = Certificados1.Find(X509FindType.FindBySerialNumber, serial, false);

				X509Certificate2 MontCertificat = null;

				// Si encontramos un certificado disponible usamos el primero
				if (Certificados3 != null && Certificados3.Count != 0)
				{

					MontCertificat = Certificados3[0];

					//	MontCertificat = new X509Certificate2(Certificados3[0].RawData, ClaveCertificado);

					parametros.certificadoalmacen = MontCertificat; //el certificado q esta en el almacen


					// parámetros propios de la empresa que provee el certificado digital
					if (EmpresaCertificadora == EnumCertificadoras.Gse)
					{
						parametros.DatoIssuername0 = CertificadorasDatos.GSE_DatoIssuername0;
						parametros.DatoIssuername1 = CertificadorasDatos.GSE_DatoIssuername1;
						parametros.DatoIssuername = CertificadorasDatos.GSE_DatoIssuername;
					}
					else if (EmpresaCertificadora == EnumCertificadoras.Andes)
					{
						parametros.DatoIssuername0 = CertificadorasDatos.Andes_DatoIssuername0;
						parametros.DatoIssuername1 = CertificadorasDatos.Andes_DatoIssuername1;
						parametros.DatoIssuername = CertificadorasDatos.Andes_DatoIssuername;
					}

					// generar firma en el StringBuilder
					datos = Firmar(datos, parametros, MontCertificat);
				}
				else
				{
					// recorre los archivos para firmar
					foreach (FacturaE_Documento archivo in datos)
					{
						if (archivo.Excepcion == null)
							archivo.Excepcion = new ApplicationException("No se encuentra el certificado");
					}
				}

				// Cerramos el almacén de certificados
				store.Close();

			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}

			return datos;

		}

		/// <summary>
		/// valida y firma de documentos XML 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="parametros"></param>
		/// <param name="MontCertificat"></param>
		/// <returns></returns>
		protected List<FacturaE_Documento> Firmar(List<FacturaE_Documento> data, SignatureParameters parametros, X509Certificate2 MontCertificat)
		{
			XadesService xadesService = new XadesService();

			string archivo_xml = string.Empty;

			// recorre los archivos para firmar
			foreach (FacturaE_Documento archivo in data)
			{
				// valida que el archivo no tenga errores para procesarlo
				if (archivo.Excepcion == null)
				{
					try
					{
						archivo_xml = string.Format(@"{0}{1}.xml", archivo.RutaArchivosProceso, archivo.NombreXml);

						// lee el archivo xml para firmar
						using (FileStream fs = new FileStream(archivo_xml, FileMode.Open))
						{
							// elemento donde se encontrará la firma de acuerdo con la raíz del documento xml
							XmlDocument documentoXml = XMLUtil.LoadDocument(fs);
							parametros.SignatureDestination.XPathExpression = string.Format("/{0}/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent", documentoXml.DocumentElement.Name);

							// lee el certificado digital para firmar
							using (parametros.Signer = new Signer(MontCertificat))
							{
								string fecha = Fecha.GetFecha().AddMinutes(-1).ToString("yyy-MM-dd'T'HH:mm:ss.fffK");
								parametros.SigningDate = Convert.ToDateTime(fecha);

								// firma el documento xml
								var docFirmado = xadesService.Sign(fs, parametros, documentoXml);

								archivo_xml = string.Format(@"{0}\{1}.xml", archivo.RutaArchivosEnvio, archivo.NombreXml);

								// almacena el documento xml con firma
								docFirmado.Save(archivo_xml);

								archivo.Excepcion = null;
							}
						}
					}
					catch (Exception excepcion)
					{
						string msg = string.Format("Firmando el documento {0} - Excepción: {1}", archivo_xml, excepcion.Message);

						if (excepcion.InnerException != null)
							msg += "  - " + excepcion.InnerException.Message;

						msg += " - " + excepcion.StackTrace.ToString();

						archivo.Excepcion = new ApplicationException(msg, excepcion);
					}
				}
			}

			return data;
		}

	}
}
