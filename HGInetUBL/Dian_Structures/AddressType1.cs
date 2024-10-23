using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="AddressType", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("Address", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable=false)]
	public partial class AddressType1 {
        
		private IDType idField;
        
		private AddressTypeCodeType addressTypeCodeField;
        
		private AddressFormatCodeType addressFormatCodeField;
        
		private PostboxType postboxField;
        
		private FloorType floorField;
        
		private RoomType roomField;
        
		private StreetNameType streetNameField;
        
		private AdditionalStreetNameType additionalStreetNameField;
        
		private BlockNameType blockNameField;
        
		private BuildingNameType buildingNameField;
        
		private BuildingNumberType buildingNumberField;
        
		private InhouseMailType inhouseMailField;
        
		private DepartmentType departmentField;
        
		private MarkAttentionType markAttentionField;
        
		private MarkCareType markCareField;
        
		private PlotIdentificationType plotIdentificationField;
        
		private CitySubdivisionNameType citySubdivisionNameField;
        
		private CityNameType cityNameField;
        
		private PostalZoneType postalZoneField;
        
		private CountrySubentityType countrySubentityField;
        
		private CountrySubentityCodeType countrySubentityCodeField;
        
		private RegionType regionField;
        
		private DistrictType districtField;
        
		private TimezoneOffsetType timezoneOffsetField;
        
		private AddressLineType[] addressLineField;
        
		private CountryType countryField;
        
		private LocationCoordinateType locationCoordinateField;
        
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
		public AddressTypeCodeType AddressTypeCode {
			get {
				return this.addressTypeCodeField;
			}
			set {
				this.addressTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AddressFormatCodeType AddressFormatCode {
			get {
				return this.addressFormatCodeField;
			}
			set {
				this.addressFormatCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PostboxType Postbox {
			get {
				return this.postboxField;
			}
			set {
				this.postboxField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public FloorType Floor {
			get {
				return this.floorField;
			}
			set {
				this.floorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RoomType Room {
			get {
				return this.roomField;
			}
			set {
				this.roomField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public StreetNameType StreetName {
			get {
				return this.streetNameField;
			}
			set {
				this.streetNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public AdditionalStreetNameType AdditionalStreetName {
			get {
				return this.additionalStreetNameField;
			}
			set {
				this.additionalStreetNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BlockNameType BlockName {
			get {
				return this.blockNameField;
			}
			set {
				this.blockNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BuildingNameType BuildingName {
			get {
				return this.buildingNameField;
			}
			set {
				this.buildingNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public BuildingNumberType BuildingNumber {
			get {
				return this.buildingNumberField;
			}
			set {
				this.buildingNumberField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InhouseMailType InhouseMail {
			get {
				return this.inhouseMailField;
			}
			set {
				this.inhouseMailField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DepartmentType Department {
			get {
				return this.departmentField;
			}
			set {
				this.departmentField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MarkAttentionType MarkAttention {
			get {
				return this.markAttentionField;
			}
			set {
				this.markAttentionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MarkCareType MarkCare {
			get {
				return this.markCareField;
			}
			set {
				this.markCareField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PlotIdentificationType PlotIdentification {
			get {
				return this.plotIdentificationField;
			}
			set {
				this.plotIdentificationField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CitySubdivisionNameType CitySubdivisionName {
			get {
				return this.citySubdivisionNameField;
			}
			set {
				this.citySubdivisionNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CityNameType CityName {
			get {
				return this.cityNameField;
			}
			set {
				this.cityNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PostalZoneType PostalZone {
			get {
				return this.postalZoneField;
			}
			set {
				this.postalZoneField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CountrySubentityType CountrySubentity {
			get {
				return this.countrySubentityField;
			}
			set {
				this.countrySubentityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public CountrySubentityCodeType CountrySubentityCode {
			get {
				return this.countrySubentityCodeField;
			}
			set {
				this.countrySubentityCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public RegionType Region {
			get {
				return this.regionField;
			}
			set {
				this.regionField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public DistrictType District {
			get {
				return this.districtField;
			}
			set {
				this.districtField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TimezoneOffsetType TimezoneOffset {
			get {
				return this.timezoneOffsetField;
			}
			set {
				this.timezoneOffsetField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("AddressLine", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public AddressLineType[] AddressLine {
			get {
				return this.addressLineField;
			}
			set {
				this.addressLineField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public CountryType Country {
			get {
				return this.countryField;
			}
			set {
				this.countryField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
		public LocationCoordinateType LocationCoordinate {
			get {
				return this.locationCoordinateField;
			}
			set {
				this.locationCoordinateField = value;
			}
		}
	}
}