using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public class DianProveedorTest : ConfigurationElement
	{
		[ConfigurationProperty("NitProveedor", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string NitProveedor
		{
			get { return (string)this["NitProveedor"]; }
			set { this["NitProveedor"] = value; }
		}

		[ConfigurationProperty("IdSoftware", DefaultValue = "", IsRequired = true)]
		public string IdSoftware
		{
			get { return (string)this["IdSoftware"]; }
			set { this["IdSoftware"] = value; }
		}

		[ConfigurationProperty("Pin", DefaultValue = "", IsRequired = true)]
		public string Pin
		{
			get { return (string)this["Pin"]; }
			set { this["Pin"] = value; }
		}

		[ConfigurationProperty("ClaveAmbiente", DefaultValue = "", IsRequired = true)]
		public string ClaveAmbiente
		{
			get { return (string)this["ClaveAmbiente"]; }
			set { this["ClaveAmbiente"] = value; }
		}

		[ConfigurationProperty("UrlServicioWeb", DefaultValue = "", IsRequired = true)]
		public string UrlServicioWeb
		{
			get { return (string)this["UrlServicioWeb"]; }
			set { this["UrlServicioWeb"] = value; }
		}

		[ConfigurationProperty("UrlWSConsultaTransacciones", DefaultValue = "", IsRequired = true)]
		public string UrlWSConsultaTransacciones
		{
			get { return (string)this["UrlWSConsultaTransacciones"]; }
			set { this["UrlWSConsultaTransacciones"] = value; }
		}
		
		public DianProveedorTest() { }

	}
}
