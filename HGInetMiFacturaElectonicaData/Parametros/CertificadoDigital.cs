using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public class CertificadoDigital : ConfigurationElement
    {

        [ConfigurationProperty("RutaLocal", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string RutaLocal
        {
            get { return (string)this["RutaLocal"]; }
            set { this["RutaLocal"] = value; }
        }

        [ConfigurationProperty("Serial", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Serial
        {
            get { return (string)this["Serial"]; }
            set { this["Serial"] = value; }
        }

        [ConfigurationProperty("Clave", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Clave
        {
            get { return (string)this["Clave"]; }
            set { this["Clave"] = value; }
        }

        [ConfigurationProperty("Certificadora", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Certificadora
        {
            get { return (string)this["Certificadora"]; }
            set { this["Certificadora"] = value; }
        }
		
		public string Propietario
		{
			get;
			set;
		}

		public DateTime Fechavenc
		{
			get;
			set;
		}

		public CertificadoDigital() { }

    }
}
