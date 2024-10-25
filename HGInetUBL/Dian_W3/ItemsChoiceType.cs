using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2000/09/xmldsig#", IncludeInSchema=false)]
	public enum ItemsChoiceType {
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlEnumAttribute("##any:")]
		Item,
        
		/// <comentarios/>
		X509CRL,
        
		/// <comentarios/>
		X509Certificate,
        
		/// <comentarios/>
		X509IssuerSerial,
        
		/// <comentarios/>
		X509SKI,
        
		/// <comentarios/>
		X509SubjectName,
	}
}