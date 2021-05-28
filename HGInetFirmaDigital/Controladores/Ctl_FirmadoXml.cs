using System;
using System.EnterpriseServices;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using FirmaXadesNet;
using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Crypto;
using HGInetFirmaDigital.Properties;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Funciones;
using FirmaXadesNet.Utils;
using LibreriaGlobalHGInet.Objetos;
using System.Collections.Generic;
using System.Linq;
using LibreriaGlobalHGInet.RegistroLog;

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
		/// <param name="certificado_serial">Número de serie del certificado digital "‎‎74 c7 43 04 a4 ac 86 b6" para búsqueda en el almacen (no para certificado físico)</param>
		/// <param name="certificado_clave">Clave del certificado digital</param>
		/// <param name="empresa_certificadora">empresa certificadora</param>
		/// <param name="datos">Archivos XML para firmar</param>
		/// <param name="firma_proveedor">indica si firma el proveedor tecnológico (true) o el obligado(false)</param>
		/// <returns>Archivos Xml procesados</returns>  
		public static FacturaE_Documento FirmarDocumentos(string NitCertificado, string certificado_ruta, string certificado_serial, string certificado_clave, EnumCertificadoras empresa_certificadora, FacturaE_Documento datos, bool firma_proveedor)
		{
			try
			{
				Ctl_FirmadoXml Firmar = new Ctl_FirmadoXml();

				if (datos.VersionDian == 2)
				{
					// firmado de archivos XML desde el almacen de certificados
					if (string.IsNullOrEmpty(certificado_ruta))
					{
						//if (string.IsNullOrWhiteSpace(certificado_serial))
						//	throw new Exception("El serial del certificado digital es inválido.");
						datos = Firmar.FirmarXAdesAlmacen(certificado_serial, certificado_clave, empresa_certificadora, datos);
					}
					// firmado de archivos XML desde un certificado físico
					else
						datos = Firmar.FirmarXAdesFisico_v2(NitCertificado, certificado_ruta, certificado_clave, empresa_certificadora, datos, firma_proveedor);
				}
				else
				{
					// firmado de archivos XML desde el almacen de certificados
					if (string.IsNullOrEmpty(certificado_ruta))
						datos = Firmar.FirmarXAdesAlmacen(certificado_serial, certificado_clave, empresa_certificadora, datos);
					// firmado de archivos XML desde un certificado físico
					else
						datos = Firmar.FirmarXAdesFisico(certificado_ruta, certificado_clave, empresa_certificadora, datos);
				}

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
		/// <param name="ClaveCertificado">Clave del Certificado</param>
		/// <param name="EmpresaCertificadora">Empresa que generó el certificado</param>
		/// <param name="rutas_archivos">Archivos XML para firmar</param>
		/// <returns>Archivos Xml procesados</returns>
		[AutoComplete(true)]
		protected FacturaE_Documento FirmarXAdesFisico(string RutaCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, FacturaE_Documento datos)
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
				parametros.SignerRole.ClaimedRoles.Add("third party");

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
				//parametros.SignaturePolicyInfo.PolicyDigestAlgorithm = DigestMethod.SHA1;

				// Política DIAN v1
				//parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf";
				//parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc=";

				// Política DIAN v2
				parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v2/politicadefirmav2.pdf";
				parametros.SignaturePolicyInfo.PolicyHash = "sbcECQ7v+y/m3OcBCJyvmkBhtFs=";

				/* GENERACIÓN PolicyHash - El valor lo generó Carlos Aguilar en Visual FoxPro
					SET LIBRARY TO LOCFILE("vfpencryption.fll")
					cufehex=STRCONV(Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav1.pdf"), 1), 15) && Easy way to get the hexBinary equivalent
					cufehex =Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav1.pdf"), 1) && Easy way to get the hexBinary equivalent
					cufehex1=Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav2.pdf"), 1)

					?STRCONV(cufehex,13)

					?STRCONV(cufehex1,13)
				 */

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
		protected FacturaE_Documento FirmarXAdesAlmacen(string SerieCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, FacturaE_Documento datos)
		{

			try
			{
				XadesService xadesService = new XadesService();
				SignatureParameters parametros = new SignatureParameters();

				parametros.SignatureMethod = SignatureMethod.RSAwithSHA1;
				parametros.DigestMethod = DigestMethod.SHA1;

				parametros.SignerRole = new SignerRole();
				parametros.SignerRole.ClaimedRoles.Add("third party");

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
				//parametros.SignaturePolicyInfo.PolicyDigestAlgorithm = DigestMethod.SHA1;

				// Política DIAN v1
				//parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf";
				//parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc=";

				// Política DIAN v2
				parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v2/politicadefirmav2.pdf";
				parametros.SignaturePolicyInfo.PolicyHash = "sbcECQ7v+y/m3OcBCJyvmkBhtFs=";

				/* GENERACIÓN PolicyHash - El valor lo generó Carlos Aguilar en Visual FoxPro
					SET LIBRARY TO LOCFILE("vfpencryption.fll")
					cufehex=STRCONV(Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav1.pdf"), 1), 15) && Easy way to get the hexBinary equivalent
					cufehex =Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav1.pdf"), 1) && Easy way to get the hexBinary equivalent
					cufehex1=Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav2.pdf"), 1)

					?STRCONV(cufehex,13)

					?STRCONV(cufehex1,13)
				 */
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
					if (datos.Excepcion == null)
						datos.Excepcion = new ApplicationException("No se encuentra el certificado");

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
		/// Firmado de varios archivos XML a través del certificado físico
		/// </summary>
		/// <param name="RutaCertificado">Ruta física del archivo del Certificado "C:/Certificado.pfx";</param>
		/// <param name="ClaveCertificado">Clave del Certificado</param>
		/// <param name="EmpresaCertificadora">Empresa que generó el certificado</param>
		/// <param name="datos">Archivo XML para firmar</param>
		/// <param name="firma_proveedor">indica si firma el proveedor tecnológico (true) o el obligado(false)</param>
		/// <returns>Archivos Xml procesados</returns>
		[AutoComplete(true)]
		protected FacturaE_Documento FirmarXAdesFisico_v2(string NitCertificado, string RutaCertificado, string ClaveCertificado, EnumCertificadoras EmpresaCertificadora, FacturaE_Documento datos, bool firma_proveedor)
		{
			X509Certificate2 MontCertificat = null;
			SignatureParameters parametros = null;

			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{
				if (!Archivo.ValidarExistencia(RutaCertificado))
				{
					throw new ApplicationException(string.Format("No se encuentra el certificado en la ruta {0}", RutaCertificado));
				}

				log_categoria = MensajeCategoria.Certificado;
				log_accion = MensajeAccion.consulta;

				//"E:/certificadrdb/Certificado_Sunat_PFX_REPDORABEATRIZ.pfx";
				MontCertificat = new X509Certificate2(RutaCertificado, ClaveCertificado);

				if (!MontCertificat.Subject.Contains(NitCertificado))
					throw new ApplicationException(string.Format("Certificado digital {0} no corresponde a  la identificación {1}", Path.GetFileNameWithoutExtension(RutaCertificado).Substring(0, 8), NitCertificado));

				if (MontCertificat.NotAfter < Fecha.GetFecha())
					throw new ApplicationException(string.Format("Certificado digital {0} con fecha de vigencia {1}, se encuentra vencido", Path.GetFileNameWithoutExtension(RutaCertificado).Substring(0, 8), MontCertificat.NotAfter));

				XadesService xadesService = new XadesService();
				parametros = new SignatureParameters();

				parametros.cNombreCertificado = RutaCertificado;
				parametros.cClaveCertificado = ClaveCertificado;
				parametros.cdesdealmacen = "NO";

				parametros.SignatureMethod = SignatureMethod.RSAwithSHA256;
				parametros.DigestMethod = DigestMethod.SHA256;

				parametros.SignerRole = new SignerRole();

				if (firma_proveedor)
					parametros.SignerRole.ClaimedRoles.Add("third party");
				else
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

				//Agrega subjectname y serial en el KeyInfoData para V2
				parametros.cKeyInfoDataAdditional = true;

				//inserta en nodo
				parametros.SignatureDestination = new SignatureXPathExpression();
				parametros.SignatureDestination.Namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
				parametros.SignatureDestination.Namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
				parametros.SignatureDestination.Namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
				parametros.SignatureDestination.Namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
				parametros.SignatureDestination.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
				parametros.SignatureDestination.Namespaces.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
				parametros.SignatureDestination.Namespaces.Add("xades141", "http://uri.etsi.org/01903/v1.4.1#");
				parametros.SignatureDestination.Namespaces.Add("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
				parametros.SignatureDestination.Namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd");

				/*
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
				*/


				// Política de firma 
				parametros.SignaturePolicyInfo = new SignaturePolicyInfo();
				parametros.SignaturePolicyInfo.PolicyDigestAlgorithm = DigestMethod.SHA256;

				// Política DIAN v1
				//parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v1/politicadefirmav1.pdf";
				//parametros.SignaturePolicyInfo.PolicyHash = "61fInBICBQOCBwuTwlaOZSi9HKc=";

				// Política DIAN v2
				parametros.SignaturePolicyInfo.PolicyIdentifier = "https://facturaelectronica.dian.gov.co/politicadefirma/v2/politicadefirmav2.pdf";

				string pdf_b64_sha256 = "dMoMvtcG5aIzgYo0tIsSQeVJBDnUnfSOfBpxXrmor0Y="; // https://hash.online-convert.com/es/generador-sha256

				string pdf_b64_sha512 = "Zcjw1Z9nGQn2j6NyGx8kAaLbOfJGd/fJxRTCeirlqAg7zRG27piJkJOpflGu7XACpMj9hC6dVMcCyzqHxxPZeQ=="; //https://hash.online-convert.com/es/generador-sha512

				parametros.SignaturePolicyInfo.PolicyHash = pdf_b64_sha256;


				/* GENERACIÓN PolicyHash - El valor lo generó Carlos Aguilar en Visual FoxPro
					SET LIBRARY TO LOCFILE("vfpencryption.fll")
					cufehex=STRCONV(Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav1.pdf"), 1), 15) && Easy way to get the hexBinary equivalent
					cufehex =Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav1.pdf"), 1) && Easy way to get the hexBinary equivalent
					cufehex1=Hash(FILETOSTR("E:\FECOLOMBIA\politicadefirmav2.pdf"), 1)

					?STRCONV(cufehex,13)

					?STRCONV(cufehex1,13)
				 */

				parametros.cDescripcionPolicy = "Política de firma para facturas electrónicas de la República de Colombia";
				parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
				parametros.InputMimeType = "text/xml";

			}
			catch (Exception excepcion)
			{

				string msg = string.Format("Error HGInet Firmado al leer el certificado {1} de {3} clave:{2} - Excepción: {0}", excepcion.Message, RutaCertificado, ClaveCertificado, Enumeracion.GetDescription(EmpresaCertificadora));

				if (log_accion == MensajeAccion.consulta)
				{
					msg = string.Format("Error al firmar el documento electrónico - Detalle: {0}", excepcion.Message);
				}

				if (excepcion.InnerException != null)
					msg += " Inner: " + excepcion.InnerException.Message;

				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);

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
		/// valida y firma de documentos XML 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="parametros"></param>
		/// <param name="MontCertificat"></param>
		/// <returns></returns>
		protected FacturaE_Documento Firmar(FacturaE_Documento archivo, SignatureParameters parametros, X509Certificate2 MontCertificat)
		{
			XadesService xadesService = new XadesService();

			string archivo_xml = string.Empty;

			// valida que el archivo no tenga errores para procesarlo
			if (archivo.Excepcion == null)
			{
				try
				{
					// valida el directorio de los archivos xml firmados
					archivo.RutaArchivosProceso = Directorio.CrearDirectorio(archivo.RutaArchivosProceso);

					archivo_xml = string.Format(@"{0}{1}.xml", archivo.RutaArchivosProceso, archivo.NombreXml);

					// lee el archivo xml para firmar
					using (FileStream fs = new FileStream(archivo_xml, FileMode.Open))
					{
						// elemento donde se encontrará la firma de acuerdo con la raíz del documento xml
						XmlDocument documentoXml = XMLUtil.LoadDocument(fs);

						if (archivo.VersionDian != 2)
						{
							parametros.SignatureDestination.XPathExpression = string.Format("/{0}/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent", documentoXml.DocumentElement.Name);
						}
						else
						{
							parametros.SignatureDestination.Namespaces.Add("nsDocument", documentoXml.DocumentElement.NamespaceURI);

							if (archivo.DocumentoTipo.GetHashCode() < TipoDocumento.Attached.GetHashCode())
							{
								int cantidad_extension = 2;


								if (archivo.DocumentoTipo != TipoDocumento.AcuseRecibo)
								{
									//Se valida si el documento trae informacion de algun sector para que sepa en la ruta donde debe agregar la firma del documento
									var documento_obj = (dynamic)null;
									documento_obj = archivo.Documento;
									if (documento_obj.SectorSalud != null && documento_obj.SectorSalud.CamposSector.Count > 0)
									{
										cantidad_extension += 1;
									}
								}
								else
								{
									//Se valida si el documento trae informacion de algun sector para que sepa en la ruta donde debe agregar la firma del documento
									var documento_obj = (dynamic)null;
									documento_obj = archivo.Documento;
									if (documento_obj.CodigoRespuesta == "036" || documento_obj.CodigoRespuesta == "037" || documento_obj.CodigoRespuesta == "038" || documento_obj.CodigoRespuesta == "039")
									{
										cantidad_extension += 1;
									}
								}
								

								//Ruta en el XML donde debe quedar la firma
								parametros.SignatureDestination.XPathExpression = string.Format("/nsDocument:{0}/ext:UBLExtensions/ext:UBLExtension[{1}]/ext:ExtensionContent", documentoXml.DocumentElement.Name, cantidad_extension);

								//parametros.SignatureDestination.XPathExpression = string.Format("/nsDocument:{0}/ext:UBLExtensions/ext:UBLExtension[2]/ext:ExtensionContent", documentoXml.DocumentElement.Name);

							}
							else if (archivo.DocumentoTipo == TipoDocumento.AcuseRecibo || archivo.DocumentoTipo == TipoDocumento.Attached || archivo.DocumentoTipo == TipoDocumento.Nomina || archivo.DocumentoTipo == TipoDocumento.NominaAjuste)
							{
								parametros.SignatureDestination.XPathExpression = string.Format("/nsDocument:{0}/ext:UBLExtensions/ext:UBLExtension[1]/ext:ExtensionContent", documentoXml.DocumentElement.Name);
							}

						}

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

							parametros.Signer.Dispose();
						}
						fs.Close();
					}
				}
				catch (Exception excepcion)
				{
					string msg = string.Format("Firmando el documento {0} - Excepción: {1}", archivo_xml, excepcion.Message);

					if (excepcion.InnerException != null)
						msg += "  - " + excepcion.InnerException.Message;

					msg += " - " + excepcion.StackTrace.ToString();

					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Certificado, MensajeTipo.Error, MensajeAccion.escritura);

					archivo.Excepcion = new ApplicationException(msg, excepcion);
				}
			}


			return archivo;
		}

	}
}
