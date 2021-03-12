using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class Collection
	{

		private string schemeNameField;

		private AdditionalType[] additionalInformationField;

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

		public AdditionalType[] AdditionalInformation
		{
			get
			{
				return this.additionalInformationField;
			}
			set
			{
				this.additionalInformationField = value;
			}
		}

	}
}
