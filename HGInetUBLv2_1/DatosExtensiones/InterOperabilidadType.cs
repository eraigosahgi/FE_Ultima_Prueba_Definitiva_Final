using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class InteroperabilidadType
	{

		private Group groupField;

		private InteroperabilidadPtType InteroperabilidadPTField;

		/// <comentarios/>
		//[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public Group Group
		{
			get
			{
				return this.groupField;
			}
			set
			{
				this.groupField = value;
			}
		}

		/// <comentarios/>
		//[System.Xml.Serialization.XmlAttributeAttribute()]
		public InteroperabilidadPtType InteroperabilidadPT
		{
			get
			{
				return this.InteroperabilidadPTField;
			}
			set
			{
				this.InteroperabilidadPTField = value;
			}
		}

	}
}
