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
		public static XmlSerializerNamespaces ObtenerNamespaces()
		{

			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
			namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
			namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
			namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
			namespaces.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
			namespaces.Add("xades141", "http://uri.etsi.org/01903/v1.4.1#");
			namespaces.Add("sts", "dian:gov:co:facturaelectronica:Structures-2-1");
			namespaces.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2 http://docs.oasis-open.org/ubl/os-UBL-2.1/xsd/maindoc/UBL-Invoice-2.1.xsd");

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
