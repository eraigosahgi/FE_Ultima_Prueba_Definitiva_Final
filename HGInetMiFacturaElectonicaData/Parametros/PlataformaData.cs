using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public class PlataformaData : ConfigurationElement
	{

		[ConfigurationProperty("RutaPublica", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string RutaPublica
		{
			get { return (string)this["RutaPublica"]; }
			set { this["RutaPublica"] = value; }
		}

		[ConfigurationProperty("RutaHginetMail", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string RutaHginetMail
		{
			get { return (string)this["RutaHginetMail"]; }
			set { this["RutaHginetMail"] = value; }
		}

		[ConfigurationProperty("Mailenvio", DefaultValue = "", IsRequired = true)]
		public string Mailenvio
		{
			get { return (string)this["Mailenvio"]; }
			set { this["Mailenvio"] = value; }
		}

		[ConfigurationProperty("LicenciaHGInetMail", DefaultValue = "", IsRequired = true)]
		public string LicenciaHGInetMail
		{
			get { return (string)this["LicenciaHGInetMail"]; }
			set { this["LicenciaHGInetMail"] = value; }
		}

		[ConfigurationProperty("IdenticacionHGInetMail", DefaultValue = "", IsRequired = true)]
		public string IdenticacionHGInetMail
		{
			get { return (string)this["IdenticacionHGInetMail"]; }
			set { this["IdenticacionHGInetMail"] = value; }
		}

		public PlataformaData() { }

	}
}
