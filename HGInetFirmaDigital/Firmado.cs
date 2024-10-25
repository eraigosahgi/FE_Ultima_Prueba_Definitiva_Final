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

namespace HGInetFirmaDigital
{
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ProgId("firmadoXades")]
	/* Permitimos que esta clase pueda ser exportada como interfaz COM, para ser utilizada en entornos no .NET también. */
	[Synchronization(SynchronizationOption.Required), JustInTimeActivation(true), ObjectPooling(Enabled = true)]
	public class FirmaXML
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
		public static List<ArchivoXml> FirmarDocumentos(string certificado_ruta, string certificado_serial, string certificado_clave, EnumCertificadoras empresa_certificadora, List<ArchivoXml> rutas)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(certificado_serial))
					throw new Exception("El serial del certificado digital es inválido.");

				FirmaXML Firmar = new FirmaXML();
				List<ArchivoXml> archivos_procesados = new List<ArchivoXml>();

				// firmado de archivos XML desde el almacen de certificados
				if (string.IsNullOrEmpty(certificado_ruta))
					archivos_procesados = Firmar.FirmarXAdesAlmacen(certificado_serial, certificado_clave, empresa_certificadora, rutas);
				// firmado de archivos XML desde un certificado físico
				else
					archivos_procesados = Firmar.FirmarXAdesFisico(certificado_ruta, certificado_serial, certificado_clave, empresa_certificadora, rutas);

				return archivos_procesados;
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
		protected List<ArchivoXml> FirmarXAdesFisico(string RutaCertificado, string SerieCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, List<ArchivoXml> rutas_archivos)
		{
			try
			{
				if (!Archivo.ValidarExistencia(RutaCertificado))
				{
					// recorre los archivos para firmar
					foreach (ArchivoXml archivo in rutas_archivos)
					{
						if (archivo.Excepcion == null)
							archivo.Excepcion = new ApplicationException(string.Format("No se encuentra el certificado en la ruta {0}", RutaCertificado));

					}

					throw new ApplicationException(string.Format("No se encuentra el certificado en la ruta {0}", RutaCertificado));
				}

				XadesService xadesService = new XadesService();
				SignatureParameters parametros = new SignatureParameters();

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
				X509Certificate2 MontCertificat = new X509Certificate2(RutaCertificado, ClaveCertificado);

				parametros.cNombreCertificado = RutaCertificado;
				parametros.cClaveCertificado = ClaveCertificado;
				parametros.cdesdealmacen = "NO";

				// generar firma en el xml
				Firmar(rutas_archivos, parametros, MontCertificat);

			}
			catch (Exception excepcion)
			{
				MessageBox.Show("ERROR HGInet Firma Digital Token. " + excepcion.Message, "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return rutas_archivos;
		}


		/// <summary>
		/// Validación del firmado de varios archivos XML a través del certificado físico
		/// </summary>
		/// <param name="RutaCertificado">Ruta física del archivo del Certificado "C:/Certificado.pfx";</param>
		/// <param name="SerieCertificado">Número de Serie del Certificado "‎‎74 c7 43 04 a4 ac 86 b6"</param>
		/// <param name="ClaveCertificado">Clave del Certificado</param>
		/// <param name="EmpresaCertificadora">Empresa que generó el certificado</param>
		/// <param name="rutas_archivos">Archivos XML para firmar</param>
		/// <returns>Archivos Xml procesados</returns>
		public static List<ArchivoXml> VerificarFirmadoFisico(string RutaCertificado, string SerieCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, List<ArchivoXml> rutas_archivos)
		{
			try
			{
				// obtiene el certificado digital físico
				X509Certificate2 certificate = new X509Certificate2(RutaCertificado, ClaveCertificado);

				foreach (ArchivoXml archivo in rutas_archivos)
				{
					try
					{
						// valida que el archivo no tenga errores para procesarlo
						if (archivo.Validar() && archivo.Excepcion == null)
						{

							// carga el archivo a validar
							XmlDocument xmlDocument = new XmlDocument();
							xmlDocument.Load(archivo.RutaConFirma);

							// obtiene el nodo del XML a validar
							XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");

							// valida la firma digital del archivo
							SignedXml signedXml = new SignedXml(xmlDocument);
							signedXml.LoadXml((XmlElement)nodeList[0]);

							bool validacion_firma = signedXml.CheckSignature(certificate, false);

							archivo.FirmaValida = validacion_firma;
							archivo.Excepcion = null;

							if (!validacion_firma)
								archivo.Excepcion = new ApplicationException("ERROR HGInet Firma Digital Validación.");
						}
					}
					catch (Exception excepcion)
					{
						archivo.FirmaValida = false;
						archivo.Excepcion = excepcion;

						MessageBox.Show("ERROR HGInet Firma Digital Validación. " + excepcion.Message, "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}

			}
			catch (Exception excepcion)
			{
				MessageBox.Show("ERROR HGInet Firma Digital Validación. " + excepcion.Message, "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return rutas_archivos;

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
		protected List<ArchivoXml> FirmarXAdesAlmacen(string SerieCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, List<ArchivoXml> rutas_archivos)
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

					// generar firma en el xml
					Firmar(rutas_archivos, parametros, MontCertificat);
				}
				else
				{
					// recorre los archivos para firmar
					foreach (ArchivoXml archivo in rutas_archivos)
					{
						if (archivo.Excepcion == null)
							archivo.Excepcion = new ApplicationException("No se encuentra el certificado");
					}

					MessageBox.Show("ERROR HGInet Firma Digital No se encuentra el certificado", "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				// Cerramos el almacén de certificados
				store.Close();

			}
			catch (Exception excepcion)
			{
				MessageBox.Show("ERROR HGInet Firma Digital Token. " + excepcion.Message, "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return rutas_archivos;

		}


		/// <summary>
		/// valida y firma de documentos XML 
		/// </summary>
		/// <param name="rutas_archivos"></param>
		/// <param name="parametros"></param>
		/// <param name="MontCertificat"></param>
		/// <returns></returns>
		protected List<ArchivoXml> Firmar(List<ArchivoXml> rutas_archivos, SignatureParameters parametros, X509Certificate2 MontCertificat)
		{
			XadesService xadesService = new XadesService();

			// recorre los archivos para firmar
			foreach (ArchivoXml archivo in rutas_archivos)
			{
				// valida que el archivo no tenga errores para procesarlo
				if (archivo.Validar() && archivo.Excepcion == null)
				{
					try
					{
						// lee el archivo xml para firmar
						using (FileStream fs = new FileStream(archivo.RutaSinFirma, FileMode.Open))
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

								// almacena el documento xml con firma
								docFirmado.Save(archivo.RutaConFirma);

								MessageBox.Show("HGInet Firma Digital. Firmado correctamente.", "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Information);

								archivo.Generado = true;
								archivo.Excepcion = null;

								parametros.Signer.Dispose();
							}
							fs.Close();
						}
					}
					catch (Exception excepcion)
					{
						MessageBox.Show("ERROR HGInet Firma Digital. No Firmado. " + excepcion.Message, "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Error);

						archivo.Generado = false;
						archivo.Excepcion = excepcion;
					}
				}
			}

			return rutas_archivos;
		}

		

		/***  MÉTODOS INICIALES 






		[AutoComplete(true)]
		public string FirmarXAdes(string CRUTA, string XMLFILE, string CRUTASALIDA, string XMLFILESALIDA, string crutanombreCertificado, string cClavecertificado)
		{
			string rRRpta;
			rRRpta = "ERROR";
			XadesService xadesService = new XadesService();
			SignatureParameters parametros = new SignatureParameters();
			string ficheroFactura = CRUTA + XMLFILE;

			parametros.SignatureMethod = SignatureMethod.RSAwithSHA1;
			parametros.DigestMethod = DigestMethod.SHA1;

			string fecha = Fecha.GetFecha().ToString("yyy-MM-dd'T'hh:mm:ss.fffK");
			parametros.SigningDate = Convert.ToDateTime(fecha);

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

			parametros.SignatureDestination.XPathExpression = "/fe:Invoice/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";

			// Política de firma 
			parametros.SignaturePolicyInfo = new SignaturePolicyInfo();

			// parametros.SignaturePolicyInfo.PolicyIdentifier = "http://www.facturae.es/politica_de_firma_formato_facturae/politica_de_firma_formato_facturae_v3_1.pdf"; //aca hay que cambiar
			// parametros.SignaturePolicyInfo.PolicyHash = "Ohixl6upD6av8N7pEvDABhEL6hM="; //SE  COMENTO

			parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf"; //JOSE CARLOS AGRUEGUE ESTA Y COMENTE LA ANTERIOR
			parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc="; //SEA AGREGO
			parametros.cDescripcionPolicy = "Política de firma para facturas electrónicas de la República de Colombia";


			parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
			parametros.InputMimeType = "text/xml";

			string cnomcert = crutanombreCertificado; //"E:/certificadrdb/Certificado_Sunat_PFX_REPDORABEATRIZ.pfx";
			X509Certificate2 MontCertificat = new X509Certificate2(cnomcert, cClavecertificado);

			parametros.cNombreCertificado = cnomcert;
			parametros.cClaveCertificado = cClavecertificado;
			parametros.cdesdealmacen = "NO";
			using (parametros.Signer = new Signer(MontCertificat))
			// using (parametros.Signer = new Signer(FirmaXadesNet.Utils.CertUtil.SelectCertificate()))
			{
				using (FileStream fs = new FileStream(ficheroFactura, FileMode.Open))
				{
					var docFirmado = xadesService.Sign(fs, parametros);
					docFirmado.Save(CRUTASALIDA + XMLFILESALIDA);
					rRRpta = "OK";
					//  MessageBox.Show("Fichero guardado correctamente.");
				}
			}

			return rRRpta;

		}

		[AutoComplete(true)]
		public string FirmarXAdesC(string CRUTA, string XMLFILE, string CRUTASALIDA, string XMLFILESALIDA, string crutanombreCertificado, string cClavecertificado)
		{
			string rRRpta;
			rRRpta = "ERROR";
			XadesService xadesService = new XadesService();
			SignatureParameters parametros = new SignatureParameters();
			string ficheroFactura = CRUTA + XMLFILE;

			parametros.SignatureMethod = SignatureMethod.RSAwithSHA1;
			parametros.DigestMethod = DigestMethod.SHA1;

			string fecha = Fecha.GetFecha().ToString("yyy-MM-dd'T'hh:mm:ss.fffK");
			parametros.SigningDate = Convert.ToDateTime(fecha);

			parametros.SignerRole = new SignerRole();
			parametros.SignerRole.ClaimedRoles.Add("supplier");

			// parametros.DatoIssuername = "C=CO,L=BOGOTÁ\\, D.C.,STREET=Carrera 21 A No 124 - 55 Oficina 303. https://www.gse.com.co/direccion,OU=http://www.gse.com.co,2.5.4.12=#0c0b41432047534520532e412e,O=GESTION DE SEGURIDAD ELECTRONICA S.A.,1.2.840.113549.1.9.1=#160d6361406773652e636f6d2e636f,2.5.4.5=#130e4e49542039303032303432373238,CN=SUB001 AC GSE S.A.,2.5.4.13=#0c27436572746966696361646f205375626f7264696e616461203030312041432047534520532e412e";
			// parametros.DatoIssuername1 = "1.3.6.1.4.1.31136.1.1.10.2=#0c819a456e7469646164206465204365727469666963616369c3b36e204469676974616c2041626965727461204175746f72697a61646120706f72206c61205375706572696e74656e64656e63696120646520496e64757374726961207920436f6d657263696f20646520436f6c6f6d6269612e2068747470733a2f2f7777772e6773652e636f6d2e636f2f5265736f6c7563696f6e5349432e706466,C=CO,L=BOGOTÁ\\, D.C.,STREET=Carrera 21 A No 124 - 55 Oficina 303. https://www.gse.com.co/direccion,OU=http://www.gse.com.co,2.5.4.5=#130e4e49542039303032303432373238,O=GESTION DE SEGURIDAD ELECTRONICA S.A.,CN=ROOT AC GSE S.A.,1.2.840.113549.1.9.1=#160d6361406773652e636f6d2e636f";
			// parametros.DatoIssuername0 = "1.3.6.1.4.1.31136.1.1.10.2=#0c819a456e7469646164206465204365727469666963616369c3b36e204469676974616c2041626965727461204175746f72697a61646120706f72206c61205375706572696e74656e64656e63696120646520496e64757374726961207920436f6d657263696f20646520436f6c6f6d6269612e2068747470733a2f2f7777772e6773652e636f6d2e636f2f5265736f6c7563696f6e5349432e706466,C=CO,L=BOGOTÁ\\, D.C.,STREET=Carrera 21 A No 124 - 55 Oficina 303. https://www.gse.com.co/direccion,OU=http://www.gse.com.co,2.5.4.5=#130e4e49542039303032303432373238,O=GESTION DE SEGURIDAD ELECTRONICA S.A.,CN=ROOT AC GSE S.A.,1.2.840.113549.1.9.1=#160d6361406773652e636f6d2e636f";

			parametros.DatoIssuername = "";
			parametros.DatoIssuername1 = "";
			parametros.DatoIssuername0 = "";
			int counter = 1;
			string line;

			// Read the file and display it line by line.  
			System.IO.StreamReader file = new System.IO.StreamReader(CRUTA + "test.txt", false);
			while ((line = file.ReadLine()) != null)
			{
				if (counter == 1)
				{
					parametros.DatoIssuername = line;
				}
				if (counter == 2)
				{
					parametros.DatoIssuername1 = line;
				}
				if (counter == 3)
				{
					parametros.DatoIssuername0 = line;
				}
				counter++;
			}
			file.Close();

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

			parametros.SignatureDestination.XPathExpression = "/fe:Invoice/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";

			// Política de firma 
			parametros.SignaturePolicyInfo = new SignaturePolicyInfo();
			parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf"; //JOSE CARLOS AGRUEGUE ESTA Y COMENTE LA ANTERIOR
			parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc="; //SEA AGREGO
			parametros.cDescripcionPolicy = "Política de firma para facturas electrónicas de la República de Colombia";
			parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
			parametros.InputMimeType = "text/xml";

			string cnomcert = crutanombreCertificado; //"E:/certificadrdb/Certificado_Sunat_PFX_REPDORABEATRIZ.pfx";
			X509Certificate2 MontCertificat = new X509Certificate2(cnomcert, cClavecertificado);

			parametros.cNombreCertificado = cnomcert;
			parametros.cClaveCertificado = cClavecertificado;
			parametros.cdesdealmacen = "NO";
			using (parametros.Signer = new Signer(MontCertificat))
			{
				using (FileStream fs = new FileStream(ficheroFactura, FileMode.Open))
				{
					var docFirmado = xadesService.Sign(fs, parametros);
					docFirmado.Save(CRUTASALIDA + XMLFILESALIDA);
					MessageBox.Show("HGInet Firma Digital. Firmado correctamente.", "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
					rRRpta = "OK";
				}
			}

			return rRRpta;

		}


		[AutoComplete(true)]
		public string FirmarXAdesAlmacen(string CRUTA, string XMLFILE, string CRUTASALIDA, string XMLFILESALIDA, string SerieCertificado, string ctxt) //el ultimo parametro es para ver si lee la cadena de certificacion desde un archivo de texto 1 si, 0 no
		{
			// SerieCertificado = "‎‎74 c7 43 04 a4 ac 86 b6"; asi se debe enviar el parametro 
			string rRRpta;
			rRRpta = "ERROR";
			XadesService xadesService = new XadesService();
			SignatureParameters parametros = new SignatureParameters();
			string ficheroFactura = CRUTA + XMLFILE;

			parametros.SignatureMethod = SignatureMethod.RSAwithSHA1;
			parametros.DigestMethod = DigestMethod.SHA1;

			string fecha = Fecha.GetFecha().ToString("yyy-MM-dd'T'hh:mm:ss.fffK");
			parametros.SigningDate = Convert.ToDateTime(fecha);

			parametros.SignerRole = new SignerRole();
			parametros.SignerRole.ClaimedRoles.Add("supplier");

			parametros.DatoIssuername = "";
			parametros.DatoIssuername1 = "";
			parametros.DatoIssuername0 = "";

			if (ctxt == "1")
			{
				int counter = 1;
				string line;
				System.IO.StreamReader file = new System.IO.StreamReader(CRUTA + "test.txt", false);
				while ((line = file.ReadLine()) != null)
				{
					if (counter == 1)
					{
						parametros.DatoIssuername0 = line;
					}
					if (counter == 2)
					{
						parametros.DatoIssuername1 = line;
					}
					if (counter == 3)
					{
						parametros.DatoIssuername = line;
					}
					counter++;
				}
				file.Close();
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

			parametros.SignatureDestination.XPathExpression = "/fe:Invoice/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent";

			// Política de firma 
			parametros.SignaturePolicyInfo = new SignaturePolicyInfo();
			parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf"; //JOSE CARLOS AGRUEGUE ESTA Y COMENTE LA ANTERIOR
			parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc="; //SEA AGREGO
			parametros.cDescripcionPolicy = "Política de firma para facturas electrónicas de la República de Colombia";

			parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
			parametros.InputMimeType = "text/xml";
			parametros.cNombreCertificado = "";
			parametros.cClaveCertificado = "";
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
				parametros.certificadoalmacen = MontCertificat; //el certificado q esta en el almacen
				using (parametros.Signer = new Signer(MontCertificat))
				{
					using (FileStream fs = new FileStream(ficheroFactura, FileMode.Open))
					{
						var docFirmado = xadesService.Sign(fs, parametros);
						docFirmado.Save(CRUTASALIDA + XMLFILESALIDA);

						MessageBox.Show("HGInet Firma Digital. Firmado correctamente.", "HGInet SAS", MessageBoxButtons.OK, MessageBoxIcon.Information);

						rRRpta = "OK";
					}
				}
			}
			else
			{
				rRRpta = "No se encuentra el certificado";
			}
			// Cerramos el almacén de certificados
			store.Close();
			return rRRpta;

		}
		**/

	}
}