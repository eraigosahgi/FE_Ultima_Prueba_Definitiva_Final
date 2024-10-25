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

		private AdditionalInformationType1[] additionalInformationField;

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

		[System.Xml.Serialization.XmlElementAttribute("AdditionalInformation")]
		public AdditionalInformationType1[] AdditionalInformation
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
