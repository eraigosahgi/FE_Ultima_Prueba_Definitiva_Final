using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("AdditionalItemIdentification", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ItemIdentificationType {
        
		private IDType idField;
        
		private ExtendedIDType extendedIDField;
        
		private PhysicalAttributeType[] physicalAttributeField;
        
		private DimensionType[] measurementDimensionField;
        
		private PartyType issuerPartyField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID {
			get {
				return this.idField;
			}
			set {
				this.idField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ExtendedIDType ExtendedID {
			get {
				return this.extendedIDField;
			}
			set {
				this.extendedIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PhysicalAttribute")]
		public PhysicalAttributeType[] PhysicalAttribute {
			get {
				return this.physicalAttributeField;
			}
			set {
				this.physicalAttributeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("MeasurementDimension")]
		public DimensionType[] MeasurementDimension {
			get {
				return this.measurementDimensionField;
			}
			set {
				this.measurementDimensionField = value;
			}
		}
        
		/// <comentarios/>
		public PartyType IssuerParty {
			get {
				return this.issuerPartyField;
			}
			set {
				this.issuerPartyField = value;
			}
		}
	}
}