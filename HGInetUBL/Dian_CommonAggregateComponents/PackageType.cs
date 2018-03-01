using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("ActualPackage", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class PackageType {
        
		private IDType idField;
        
		private QuantityType1 quantityField;
        
		private ReturnableMaterialIndicatorType returnableMaterialIndicatorField;
        
		private PackageLevelCodeType packageLevelCodeField;
        
		private PackagingTypeCodeType1 packagingTypeCodeField;
        
		private PackingMaterialType[] packingMaterialField;
        
		private PackageType[] containedPackageField;
        
		private GoodsItemType[] goodsItemField;
        
		private DimensionType[] measurementDimensionField;
        
		private DeliveryUnitType[] deliveryUnitField;
        
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
		public QuantityType1 Quantity {
			get {
				return this.quantityField;
			}
			set {
				this.quantityField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReturnableMaterialIndicatorType ReturnableMaterialIndicator {
			get {
				return this.returnableMaterialIndicatorField;
			}
			set {
				this.returnableMaterialIndicatorField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackageLevelCodeType PackageLevelCode {
			get {
				return this.packageLevelCodeField;
			}
			set {
				this.packageLevelCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackagingTypeCodeType1 PackagingTypeCode {
			get {
				return this.packagingTypeCodeField;
			}
			set {
				this.packagingTypeCodeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("PackingMaterial", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PackingMaterialType[] PackingMaterial {
			get {
				return this.packingMaterialField;
			}
			set {
				this.packingMaterialField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("ContainedPackage")]
		public PackageType[] ContainedPackage {
			get {
				return this.containedPackageField;
			}
			set {
				this.containedPackageField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute("GoodsItem")]
		public GoodsItemType[] GoodsItem {
			get {
				return this.goodsItemField;
			}
			set {
				this.goodsItemField = value;
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
		[System.Xml.Serialization.XmlElementAttribute("DeliveryUnit")]
		public DeliveryUnitType[] DeliveryUnit {
			get {
				return this.deliveryUnitField;
			}
			set {
				this.deliveryUnitField = value;
			}
		}
	}
}