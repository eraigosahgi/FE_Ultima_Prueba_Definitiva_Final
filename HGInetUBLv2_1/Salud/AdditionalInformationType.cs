using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class AdditionalType
	{
		private NameType1 nameField;

		private ValueType valueField;

		private string schemeIDField;

		private string schemeNameField;

		public NameType1 Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		public ValueType Value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string schemeName
		{
			get
			{
				return this.schemeNameField;
			}
			set
			{
				this.schemeNameField = value;
			}
		}

		/// <comentarios/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string schemeID
		{
			get
			{
				return this.schemeIDField;
			}
			set
			{
				this.schemeIDField = value;
			}
		}
	}
}
