using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2")]
	[System.Xml.Serialization.XmlRootAttribute("Person", Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", IsNullable=false)]
	public partial class PersonType {
        
		private FirstNameType firstNameField;
        
		private FamilyNameType familyNameField;
        
		private TitleType titleField;
        
		private MiddleNameType middleNameField;
        
		private NameSuffixType nameSuffixField;
        
		private JobTitleType jobTitleField;
        
		private OrganizationDepartmentType organizationDepartmentField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public FirstNameType FirstName {
			get {
				return this.firstNameField;
			}
			set {
				this.firstNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public FamilyNameType FamilyName {
			get {
				return this.familyNameField;
			}
			set {
				this.familyNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public TitleType Title {
			get {
				return this.titleField;
			}
			set {
				this.titleField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public MiddleNameType MiddleName {
			get {
				return this.middleNameField;
			}
			set {
				this.middleNameField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public NameSuffixType NameSuffix {
			get {
				return this.nameSuffixField;
			}
			set {
				this.nameSuffixField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public JobTitleType JobTitle {
			get {
				return this.jobTitleField;
			}
			set {
				this.jobTitleField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public OrganizationDepartmentType OrganizationDepartment {
			get {
				return this.organizationDepartmentField;
			}
			set {
				this.organizationDepartmentField = value;
			}
		}
	}
}