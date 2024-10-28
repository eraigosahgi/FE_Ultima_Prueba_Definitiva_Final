using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ItemLocationQuantity", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class ItemLocationQuantityType {
        
		private LeadTimeMeasureType leadTimeMeasureField;
        
		private MinimumQuantityType minimumQuantityField;
        
		private MaximumQuantityType maximumQuantityField;
        
		private HazardousRiskIndicatorType hazardousRiskIndicatorField;
        
		private TradingRestrictionsType[] tradingRestrictionsField;
        
		private AddressType[] applicableTerritoryAddressField;
        
		private PriceType priceField;
        
		private DeliveryUnitType[] deliveryUnitField;
        
		private TaxCategoryType[] applicableTaxCategoryField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public LeadTimeMeasureType LeadTimeMeasure {
			get {
				return this.leadTimeMeasureField;
			}
			set {
				this.leadTimeMeasureField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MinimumQuantityType MinimumQuantity {
			get {
				return this.minimumQuantityField;
			}
			set {
				this.minimumQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MaximumQuantityType MaximumQuantity {
			get {
				return this.maximumQuantityField;
			}
			set {
				this.maximumQuantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public HazardousRiskIndicatorType HazardousRiskIndicator {
			get {
				return this.hazardousRiskIndicatorField;
			}
			set {
				this.hazardousRiskIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("TradingRestrictions", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TradingRestrictionsType[] TradingRestrictions {
			get {
				return this.tradingRestrictionsField;
			}
			set {
				this.tradingRestrictionsField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ApplicableTerritoryAddress")]
		public AddressType[] ApplicableTerritoryAddress {
			get {
				return this.applicableTerritoryAddressField;
			}
			set {
				this.applicableTerritoryAddressField = value;
			}
		}
        
		/// <comentarios/>
		public PriceType Price {
			get {
				return this.priceField;
			}
			set {
				this.priceField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("DeliveryUnit")]
		public DeliveryUnitType[] DeliveryUnit {
			get {
				return this.deliveryUnitField;
			}
			set {
				this.deliveryUnitField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ApplicableTaxCategory")]
		public TaxCategoryType[] ApplicableTaxCategory {
			get {
				return this.applicableTaxCategoryField;
			}
			set {
				this.applicableTaxCategoryField = value;
			}
		}
	}
}