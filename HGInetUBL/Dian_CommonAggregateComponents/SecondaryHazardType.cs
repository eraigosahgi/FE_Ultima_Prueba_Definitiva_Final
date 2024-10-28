using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("SecondaryHazard", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class SecondaryHazardType {
        
		private IDType idField;
        
		private PlacardNotationType placardNotationField;
        
		private PlacardEndorsementType placardEndorsementField;
        
		private EmergencyProceduresCodeType emergencyProceduresCodeField;
        
		private ExtensionType extensionField;
        
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
		public PlacardNotationType PlacardNotation {
			get {
				return this.placardNotationField;
			}
			set {
				this.placardNotationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PlacardEndorsementType PlacardEndorsement {
			get {
				return this.placardEndorsementField;
			}
			set {
				this.placardEndorsementField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public EmergencyProceduresCodeType EmergencyProceduresCode {
			get {
				return this.emergencyProceduresCodeField;
			}
			set {
				this.emergencyProceduresCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ExtensionType Extension {
			get {
				return this.extensionField;
			}
			set {
				this.extensionField = value;
			}
		}
	}
}