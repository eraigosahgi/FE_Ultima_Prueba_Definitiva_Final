using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBL
{
    public class NamespacesXML
    {
        /// <summary>
        /// Obtiene los namespaces que seran utilizados en el XML
        /// </summary>
        /// <returns>Namespaces</returns>
        public static XmlSerializerNamespaces Obtener()
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add("schemaLocation", "http://www.dian.gov.co/contratos/facturaelectronica/v1 http://www.dian.gov.co/micrositios/fac_electronica/documentos/XSD/r1/DIAN_UBL.xsd urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2 http://www.dian.gov.co/micrositios/fac_electronica/documentos/common/UnqualifiedDataTypeSchemaModule-2.0.xsd urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2 http://www.dian.gov.co/micrositios/fac_electronica/documentos/common/UBL-QualifiedDatatypes-2.0.xsd");
			namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
            namespaces.Add("sts", "http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures");
            namespaces.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
            namespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            namespaces.Add("clmIANAMIMEMediaType", "urn:un:unece:uncefact:codelist:specification:IANAMIMEMediaType:2003");
            namespaces.Add("clm66411", "urn:un:unece:uncefact:codelist:specification:66411:2001");
            namespaces.Add("clm54217", "urn:un:unece:uncefact:codelist:specification:54217:2001");
            namespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            namespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            namespaces.Add("fe", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
            namespaces.Add("hst", "http://www.mifacturaenlinea.com.co/v1/Structures");
            namespaces.Add("hac", "urn:hginet:names:specification:ubl:colombia:schema:xsd:HgiNetAggregateComponents-1");

            return namespaces;
        }

        /// <summary>
        /// Obtiene los namespaces que seran utilizados para extension de la DIAN
        /// </summary>
        /// <returns>Namespaces</returns>
        public static XmlSerializerNamespaces ObtenerExtensionDian()
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("sts", "http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures");
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
