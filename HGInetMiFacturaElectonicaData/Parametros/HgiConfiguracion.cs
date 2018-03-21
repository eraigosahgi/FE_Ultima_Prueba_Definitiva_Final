using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public class HgiConfiguracion : ConfigurationSection
	{

		public static HgiConfiguracion GetConfiguration()
		{
			HgiConfiguracion configuration = ConfigurationManager.GetSection("HgiNet") as HgiConfiguracion;

			if (configuration != null)
				return configuration;

			return new HgiConfiguracion();
		}


		[ConfigurationProperty("dataBaseServer")]
		public DataBaseServer DataBaseServer
		{
			get
			{
				DataBaseServer bd_tmp = this["dataBaseServer"] as DataBaseServer;

				return bd_tmp;
			}
			//set { this["DataBaseServer"] = value; }
		}

		[ConfigurationProperty("mailServer")]
		public MailServer MailServer
		{
			get
			{
				MailServer mail_tmp = this["mailServer"] as MailServer;

				return mail_tmp;
			}
			//set { this["MailServer"] = value; }
		}

		[ConfigurationProperty("dianProveedor")]
		public DianProveedor DianProveedor
		{
			get
			{
				DianProveedor dian_tmp = this["dianProveedor"] as DianProveedor;

				return dian_tmp;
			}
			//set { this["DianProveedor"] = value; }
		}
	}
}
