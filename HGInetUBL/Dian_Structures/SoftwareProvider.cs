using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures")]
	public partial class SoftwareProvider {
        
		private IdentifierType providerIDField;
        
		private IdentifierType softwareIDField;
        
		/// <comentarios/>
		public IdentifierType ProviderID {
			get {
				return this.providerIDField;
			}
			set {
				this.providerIDField = value;
			}
		}
        
		/// <comentarios/>
		public IdentifierType SoftwareID {
			get {
				return this.softwareIDField;
			}
			set {
				this.softwareIDField = value;
			}
		}
	}
}