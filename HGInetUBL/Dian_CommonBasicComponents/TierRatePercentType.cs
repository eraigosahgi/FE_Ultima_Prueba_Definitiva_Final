using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("TierRatePercent", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", IsNullable=false)]
	public partial class TierRatePercentType {
        
		private decimal valueField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlTextAttribute()]
		public decimal Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}