using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	//[System.Xml.Serialization.XmlRootAttribute("BeneficiosComprador", Namespace = "co:facturaelectronica:ccce:BeneficiosComprador", IsNullable = false)]
	public class BeneficiosComprador
	{
		private InformacionBeneficiosComprador InformacionBeneficiosCompradorField;

		public InformacionBeneficiosComprador InformacionBeneficiosComprador
		{
			get
			{
				return this.InformacionBeneficiosCompradorField;
			}
			set
			{
				this.InformacionBeneficiosCompradorField = value;
			}
		}
	}
}
