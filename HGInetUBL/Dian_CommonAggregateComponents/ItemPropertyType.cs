using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("AdditionalItemProperty", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ItemPropertyType {
        
		private NameType1 nameField;
        
		private ValueType valueField;
        
		private PeriodType usabilityPeriodField;
        
		private ItemPropertyGroupType[] itemPropertyGroupField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NameType1 Name {
			get {
				return this.nameField;
			}
			set {
				this.nameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ValueType Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
        
		/// <comentarios/>
		public PeriodType UsabilityPeriod {
			get {
				return this.usabilityPeriodField;
			}
			set {
				this.usabilityPeriodField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ItemPropertyGroup")]
		public ItemPropertyGroupType[] ItemPropertyGroup {
			get {
				return this.itemPropertyGroupField;
			}
			set {
				this.itemPropertyGroupField = value;
			}
		}
	}
}