using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	
	public class DataBaseServer : ConfigurationElement
	{

		[ConfigurationProperty("Servidor", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string Servidor
		{
			get { return (string)this["Servidor"]; }
			set { this["Servidor"] = value; }
		}

		[ConfigurationProperty("BaseDatos", DefaultValue = "", IsRequired = true)]
		public string BaseDatos
		{
			get { return (string)this["BaseDatos"]; }
			set { this["BaseDatos"] = value; }
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

		[ConfigurationProperty("InfoSeguridadPersiste", DefaultValue = (bool)true, IsRequired = true)]
		public bool InfoSeguridadPersiste
		{
			get { return (bool)this["InfoSeguridadPersiste"]; }
			set { this["InfoSeguridadPersiste"] = value; }
		}

		[ConfigurationProperty("Motor", DefaultValue = "", IsKey = true, IsRequired = true)]
		public string Motor
		{
			get { return (string)this["Motor"]; }
			set { this["Motor"] = value; }
		}

		public DataBaseServer()
		{


			//var _Config = ConfigurationManager.GetSection("HgiNet/dataBaseServer");



		}


	}
}
