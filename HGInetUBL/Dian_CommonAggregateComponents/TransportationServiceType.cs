using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("FinalDeliveryTransportationService", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class TransportationServiceType {
        
		private TransportServiceCodeType transportServiceCodeField;
        
		private TariffClassCodeType tariffClassCodeField;
        
		private PriorityType priorityField;
        
		private FreightRateClassCodeType freightRateClassCodeField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TransportServiceCodeType TransportServiceCode {
			get {
				return this.transportServiceCodeField;
			}
			set {
				this.transportServiceCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TariffClassCodeType TariffClassCode {
			get {
				return this.tariffClassCodeField;
			}
			set {
				this.tariffClassCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PriorityType Priority {
			get {
				return this.priorityField;
			}
			set {
				this.priorityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public FreightRateClassCodeType FreightRateClassCode {
			get {
				return this.freightRateClassCodeField;
			}
			set {
				this.freightRateClassCodeField = value;
			}
		}
	}
}