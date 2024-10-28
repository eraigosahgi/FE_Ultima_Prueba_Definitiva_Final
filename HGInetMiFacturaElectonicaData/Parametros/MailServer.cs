using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public class MailServer : ConfigurationElement
	{
		[ConfigurationProperty("Servidor", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string Servidor
		{
			get { return (string)this["Servidor"]; }
			set { this["Servidor"] = value; }
		}

		[ConfigurationProperty("Usuario", DefaultValue = "", IsRequired = true)]
		public string Usuario
		{
			get { return (string)this["Usuario"]; }
			set { this["Usuario"] = value; }
		}

		[ConfigurationProperty("Clave", DefaultValue = "", IsRequired = true)]
		public string Clave
		{
			get { return (string)this["Clave"]; }
			set { this["Clave"] = value; }
		}

		[ConfigurationProperty("Puerto", DefaultValue = (int)25, IsRequired = true)]
		public int Puerto
		{
			get { return (int)this["Puerto"]; }
			set { this["Puerto"] = value; }
		}

		[ConfigurationProperty("HabilitaSsl", DefaultValue = (bool)false, IsRequired = true)]
		public bool HabilitaSsl
		{
			get { return (bool)this["HabilitaSsl"]; }
			set { this["HabilitaSsl"] = value; }
		}
		
		[ConfigurationProperty("RemitenteNombre", DefaultValue = "", IsRequired = true)]
		public string RemitenteNombre
		{
			get { return (string)this["RemitenteNombre"]; }
			set { this["RemitenteNombre"] = value; }
		}

		[ConfigurationProperty("RemitenteMail", DefaultValue = "", IsRequired = true)]
		public string RemitenteMail
		{
			get { return (string)this["RemitenteMail"]; }
			set { this["RemitenteMail"] = value; }
		}

		public MailServer() { }

	}
}
