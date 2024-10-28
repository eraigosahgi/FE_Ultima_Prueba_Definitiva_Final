using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("CurrentStatus", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class StatusType {
        
		private ConditionCodeType conditionCodeField;
        
		private ReferenceDateType referenceDateField;
        
		private ReferenceTimeType referenceTimeField;
        
		private DescriptionType descriptionField;
        
		private StatusReasonCodeType statusReasonCodeField;
        
		private StatusReasonType statusReasonField;
        
		private SequenceIDType sequenceIDField;
        
		private TextType1 textField;
        
		private IndicationIndicatorType indicationIndicatorField;
        
		private PercentType percentField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ConditionCodeType ConditionCode {
			get {
				return this.conditionCodeField;
			}
			set {
				this.conditionCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReferenceDateType ReferenceDate {
			get {
				return this.referenceDateField;
			}
			set {
				this.referenceDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReferenceTimeType ReferenceTime {
			get {
				return this.referenceTimeField;
			}
			set {
				this.referenceTimeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DescriptionType Description {
			get {
				return this.descriptionField;
			}
			set {
				this.descriptionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public StatusReasonCodeType StatusReasonCode {
			get {
				return this.statusReasonCodeField;
			}
			set {
				this.statusReasonCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public StatusReasonType StatusReason {
			get {
				return this.statusReasonField;
			}
			set {
				this.statusReasonField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public SequenceIDType SequenceID {
			get {
				return this.sequenceIDField;
			}
			set {
				this.sequenceIDField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TextType1 Text {
			get {
				return this.textField;
			}
			set {
				this.textField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IndicationIndicatorType IndicationIndicator {
			get {
				return this.indicationIndicatorField;
			}
			set {
				this.indicationIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PercentType Percent {
			get {
				return this.percentField;
			}
			set {
				this.percentField = value;
			}
		}
	}
}