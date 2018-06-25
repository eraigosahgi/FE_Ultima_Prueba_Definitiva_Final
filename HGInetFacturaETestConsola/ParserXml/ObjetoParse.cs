using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetFacturaETestConsola
{
	[XmlRootAttribute(Namespace = "http://www.dian.gov.co/contratos/facturaelectronica/v1", ElementName ="CreditNote")]
	public class CreditNote
	{
		[XmlElement(ElementName = "cbc:UBLVersionID", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		string id_ubl { get; set; }

	}
}
