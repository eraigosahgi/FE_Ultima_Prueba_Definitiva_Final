using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public class AzureStorage : ConfigurationElement
	{

		[ConfigurationProperty("connectionString", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string connectionString
		{
			get { return (string)this["connectionString"]; }
			set { this["connectionString"] = value; }
		}

	}
}
