using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public class NamespacesXML
	{

		/// <summary>
		/// Obtiene los namespaces que seran utilizados en el XML
		/// </summary>
		/// <returns>Namespaces</returns>
		public static XmlSerializerNamespaces ObtenerNamespaces(TipoDocumento tipo)
		{
			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

			if (tipo.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode() || tipo == TipoDocumento.Attached)
			{
				namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
				namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
				namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
				namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
				namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
				namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
				namespaces.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
				namespaces.Add("xades141", "http://uri.etsi.org/01903/v1.4.1#");
				namespaces.Add("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
				if (tipo == TipoDocumento.Factura)
				{
					namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd");
				}
				else if (tipo == TipoDocumento.NotaCredito)
				{
					namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2 http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-CreditNote-2.1.xsd");
				}
				else if (tipo == TipoDocumento.NotaDebito)
				{
					namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:DebitNote-2 http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-DebitNote-2.1.xsd");
				}
				//else if (tipo == TipoDocumento.Attached)
				//{
				//	namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:AttachedDocument-2");
				//}
				
			}
			else if (tipo == TipoDocumento.AcuseRecibo)
			{
				namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
				namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
				namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
				namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
				namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
				namespaces.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
				namespaces.Add("xades141", "http://uri.etsi.org/01903/v1.4.1#");
				namespaces.Add("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
				namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2 http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-ApplicationResponse-2.1.xsd");
			}
			//else if (tipo == TipoDocumento.Nomina)
			//{
			//	namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema-instance");
			//	namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
			//	namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
			//	namespaces.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
			//	namespaces.Add("xades141", "http://uri.etsi.org/01903/v1.4.1#");
			//	namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			//	namespaces.Add("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
			//	namespaces.Add("schemaLocation", "dian:gov:co:facturaelectronica:NominaIndividual NominaIndividualElectronicaXSD.xsd");
			//}

			return namespaces;
		}


		/// <summary>
		/// Obtiene los namespaces que seran utilizados para extension de la DIAN
		/// </summary>
		/// <returns>Namespaces</returns>
		public static XmlSerializerNamespaces ObtenerExtensionDian()
		{
			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
			namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
			return namespaces;
		}
		

		/// <summary>
		/// Obtiene los namespaces que seran utilizados para extension de HGI SAS
		/// </summary>
		/// <returns>Namespaces</returns>
		public static XmlSerializerNamespaces ObtenerExtensionHgi()
		{
			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add("hst", "http://www.mifacturaenlinea.com.co/v1/Structures");
			namespaces.Add("hac", "urn:hginet:names:specification:ubl:colombia:schema:xsd:HgiNetAggregateComponents-1");
			return namespaces;

		}



	}
}
