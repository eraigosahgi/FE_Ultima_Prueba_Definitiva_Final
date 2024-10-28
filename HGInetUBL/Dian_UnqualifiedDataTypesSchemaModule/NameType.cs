using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(VesselNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(TechnicalNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(StreetNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(RegistrationNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(NameType1))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(ModelNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(MiddleNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(HolderNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(FirstNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(FamilyNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CitySubdivisionNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CityNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BuildingNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BrandNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(BlockNameType))]
	[System.Xml.Serialization.XmlIncludeAttribute(typeof(AdditionalStreetNameType))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2")]
	public partial class NameType {
        
		private string languageIDField;
        
		private string valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute(DataType="language")]
		public string languageID {
			get {
				return this.languageIDField;
			}
			set {
				this.languageIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}