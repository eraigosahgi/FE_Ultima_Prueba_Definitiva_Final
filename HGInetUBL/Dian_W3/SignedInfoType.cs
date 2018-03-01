using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
	[System.Xml.Serialization.XmlRootAttribute("SignedInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#", IsNullable = false)]
	public partial class SignedInfoType
	{

		private CanonicalizationMethodType1 canonicalizationMethodField;

		private SignatureMethodType1 signatureMethodField;

		private ReferenceType1[] referenceField;

		private string idField;

		/// <comentarios/>
		public CanonicalizationMethodType1 CanonicalizationMethod
		{
			get
			{
				return this.canonicalizationMethodField;
			}
			set
			{
				this.canonicalizationMethodField = value;
			}
		}

		/// <comentarios/>
		public SignatureMethodType1 SignatureMethod
		{
			get
			{
				return this.signatureMethodField;
			}
			set
			{
				this.signatureMethodField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("Reference")]
		public ReferenceType1[] Reference
		{
			get
			{
				return this.referenceField;
			}
			set
			{
				this.referenceField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
		public string Id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
	}
}