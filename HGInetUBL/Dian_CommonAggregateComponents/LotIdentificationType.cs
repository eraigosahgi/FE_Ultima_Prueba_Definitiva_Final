using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("LotIdentification", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class LotIdentificationType {
        
		private LotNumberIDType lotNumberIDField;
        
		private ExpiryDateType expiryDateField;
        
		private ItemPropertyType[] additionalItemPropertyField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LotNumberIDType LotNumberID {
			get {
				return this.lotNumberIDField;
			}
			set {
				this.lotNumberIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ExpiryDateType ExpiryDate {
			get {
				return this.expiryDateField;
			}
			set {
				this.expiryDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AdditionalItemProperty")]
		public ItemPropertyType[] AdditionalItemProperty {
			get {
				return this.additionalItemPropertyField;
			}
			set {
				this.additionalItemPropertyField = value;
			}
		}
	}
}