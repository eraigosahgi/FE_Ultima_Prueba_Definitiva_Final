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


		[ConfigurationProperty("dataBaseAuditoria")]
		public DataBaseAuditoria DbAuditoria
		{
			get
			{
				DataBaseAuditoria bd_tmp = this["dataBaseAuditoria"] as DataBaseAuditoria;

				return bd_tmp;
			}
			//set { this["dataBaseAuditoria"] = value; }
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

		[ConfigurationProperty("dianProveedorV2")]
		public DianProveedorV2 DianProveedorV2
		{
			get
			{
				DianProveedorV2 dian_tmp = this["dianProveedorV2"] as DianProveedorV2;

				return dian_tmp;
			}
			//set { this["DianProveedor"] = value; }
		}

		[ConfigurationProperty("dianProveedorTest")]
		public DianProveedorTest DianProveedorTest
		{
			get
			{
				DianProveedorTest dian_tmp = this["dianProveedorTest"] as DianProveedorTest;

				return dian_tmp;
			}
			//set { this["DianProveedor"] = value; }
		}

		[ConfigurationProperty("plataformaData")]
		public PlataformaData PlataformaData
		{
			get
			{
				PlataformaData plataforma_tmp = this["plataformaData"] as PlataformaData;

				return plataforma_tmp;
			}

		}

        [ConfigurationProperty("certificadoDigital")]
        public CertificadoDigital CertificadoDigitalData
        {
            get
            {
                CertificadoDigital certificado_tmp = this["certificadoDigital"] as CertificadoDigital;

                return certificado_tmp;
            }

        }

        [ConfigurationProperty("PasarelaPagos")]
        public PasarelaPagos PasarelaPagos
        {
            get
            {
                PasarelaPagos pasarela_tmp = this["PasarelaPagos"] as PasarelaPagos;

                return pasarela_tmp;
            }

        }

		[ConfigurationProperty("AzureStorage")]
		public AzureStorage AzureStorage
		{
			get
			{
				AzureStorage azureStorage_tmp = this["AzureStorage"] as AzureStorage;

				return azureStorage_tmp;
			}

		}
	}
}
