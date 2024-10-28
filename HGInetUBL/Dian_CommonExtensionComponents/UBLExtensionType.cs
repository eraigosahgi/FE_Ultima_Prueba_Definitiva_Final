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
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("UBLExtension", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", IsNullable = false)]
	public partial class UBLExtensionType
	{

		private IDType idField;

		private NameType1 nameField;

		private ExtensionAgencyIDType extensionAgencyIDField;

		private ExtensionAgencyNameType extensionAgencyNameField;

		private ExtensionVersionIDType extensionVersionIDField;

		private ExtensionAgencyURIType extensionAgencyURIField;

		private ExtensionURIType extensionURIField;

		private ExtensionReasonCodeType extensionReasonCodeField;

		private ExtensionReasonType extensionReasonField;

		private System.Xml.XmlElement extensionContentField;

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID
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

		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NameType1 Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <comentarios/>
		public ExtensionAgencyIDType ExtensionAgencyID
		{
			get
			{
				return this.extensionAgencyIDField;
			}
			set
			{
				this.extensionAgencyIDField = value;
			}
		}

		/// <comentarios/>
		public ExtensionAgencyNameType ExtensionAgencyName
		{
			get
			{
				return this.extensionAgencyNameField;
			}
			set
			{
				this.extensionAgencyNameField = value;
			}
		}

		/// <comentarios/>
		public ExtensionVersionIDType ExtensionVersionID
		{
			get
			{
				return this.extensionVersionIDField;
			}
			set
			{
				this.extensionVersionIDField = value;
			}
		}

		/// <comentarios/>
		public ExtensionAgencyURIType ExtensionAgencyURI
		{
			get
			{
				return this.extensionAgencyURIField;
			}
			set
			{
				this.extensionAgencyURIField = value;
			}
		}

		/// <comentarios/>
		public ExtensionURIType ExtensionURI
		{
			get
			{
				return this.extensionURIField;
			}
			set
			{
				this.extensionURIField = value;
			}
		}

		/// <comentarios/>
		public ExtensionReasonCodeType ExtensionReasonCode
		{
			get
			{
				return this.extensionReasonCodeField;
			}
			set
			{
				this.extensionReasonCodeField = value;
			}
		}

		/// <comentarios/>
		public ExtensionReasonType ExtensionReason
		{
			get
			{
				return this.extensionReasonField;
			}
			set
			{
				this.extensionReasonField = value;
			}
		}

		/// <comentarios/>
		public System.Xml.XmlElement ExtensionContent
		{
			get
			{
				return this.extensionContentField;
			}
			set
			{
				this.extensionContentField = value;
			}
		}
	}
}