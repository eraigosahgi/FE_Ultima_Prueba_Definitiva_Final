﻿/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
[System.Xml.Serialization.XmlRootAttribute("Event", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
public partial class EventType {
    
	private IdentificationIDType identificationIDField;
    
	private OccurrenceDateType occurrenceDateField;
    
	private OccurrenceTimeType occurrenceTimeField;
    
	private TypeCodeType typeCodeField;
    
	private DescriptionType[] descriptionField;
    
	private CompletionIndicatorType completionIndicatorField;
    
	private StatusType[] currentStatusField;
    
	private ContactType[] contactField;
    
	private LocationType1 occurenceLocationField;
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public IdentificationIDType IdentificationID {
		get {
			return this.identificationIDField;
		}
		set {
			this.identificationIDField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OccurrenceDateType OccurrenceDate {
		get {
			return this.occurrenceDateField;
		}
		set {
			this.occurrenceDateField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public OccurrenceTimeType OccurrenceTime {
		get {
			return this.occurrenceTimeField;
		}
		set {
			this.occurrenceTimeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public TypeCodeType TypeCode {
		get {
			return this.typeCodeField;
		}
		set {
			this.typeCodeField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Description", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public DescriptionType[] Description {
		get {
			return this.descriptionField;
		}
		set {
			this.descriptionField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	public CompletionIndicatorType CompletionIndicator {
		get {
			return this.completionIndicatorField;
		}
		set {
			this.completionIndicatorField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("CurrentStatus")]
	public StatusType[] CurrentStatus {
		get {
			return this.currentStatusField;
		}
		set {
			this.currentStatusField = value;
		}
	}
    
	/// <comentarios/>
	[System.Xml.Serialization.XmlElementAttribute("Contact")]
	public ContactType[] Contact {
		get {
			return this.contactField;
		}
		set {
			this.contactField = value;
		}
	}
    
	/// <comentarios/>
	public LocationType1 OccurenceLocation {
		get {
			return this.occurenceLocationField;
		}
		set {
			this.occurenceLocationField = value;
		}
	}
}