using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ItemInstance", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ItemInstanceType {
        
		private ProductTraceIDType productTraceIDField;
        
		private ManufactureDateType manufactureDateField;
        
		private ManufactureTimeType manufactureTimeField;
        
		private RegistrationIDType registrationIDField;
        
		private SerialIDType serialIDField;
        
		private ItemPropertyType[] additionalItemPropertyField;
        
		private LotIdentificationType lotIdentificationField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ProductTraceIDType ProductTraceID {
			get {
				return this.productTraceIDField;
			}
			set {
				this.productTraceIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ManufactureDateType ManufactureDate {
			get {
				return this.manufactureDateField;
			}
			set {
				this.manufactureDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ManufactureTimeType ManufactureTime {
			get {
				return this.manufactureTimeField;
			}
			set {
				this.manufactureTimeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RegistrationIDType RegistrationID {
			get {
				return this.registrationIDField;
			}
			set {
				this.registrationIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SerialIDType SerialID {
			get {
				return this.serialIDField;
			}
			set {
				this.serialIDField = value;
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
        
		/// <comentarios/>
		public LotIdentificationType LotIdentification {
			get {
				return this.lotIdentificationField;
			}
			set {
				this.lotIdentificationField = value;
			}
		}
	}
}