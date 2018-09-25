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

		[ConfigurationProperty("IdentificacionHGInetMail", DefaultValue = "", IsRequired = true)]
		public string IdentificacionHGInetMail
		{
			get { return (string)this["IdentificacionHGInetMail"]; }
			set { this["IdentificacionHGInetMail"] = value; }
		}

        [ConfigurationProperty("EnvioSms", DefaultValue = (bool)false, IsRequired = true)]

        public bool EnvioSms
        {
            get { return (bool)this["EnvioSms"]; }
            set { this["EnvioSms"] = value; }
        }

        [ConfigurationProperty("RutaDmsFisica", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string RutaDmsFisica
        {
            get { return (string)this["RutaDmsFisica"]; }
            set { this["RutaDmsFisica"] = value; }
        }

        [ConfigurationProperty("RutaDmsPublica", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string RutaDmsPublica
        {
            get { return (string)this["RutaDmsPublica"]; }
            set { this["RutaDmsPublica"] = value; }
        }


        public PlataformaData() { }

	}
}
