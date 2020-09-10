using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetDIANServicios.DianWSValidacionPrevia;
using System.Security.Cryptography.X509Certificates;
using HGInetMiFacturaElectonicaData;
using LibreriaGlobalHGInet.General;
using System.IO;

namespace HGInetDIANServicios
{
	public class Ctl_CorreosRecepcionDoc
	{

		public static string ObtenerCorreos(string ruta_certificado, string clave_certificado, string ruta_servicio_web, string ruta_archivo)
		{


			try
			{
				WcfDianCustomerServicesClient webServiceHab = new WcfDianCustomerServicesClient();
				webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				//Certificado de producción
				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);
				webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

				//Se agrega instruccion para habilitar la seguridad en el envio
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

				ExchangeEmailResponse archivo_email = webServiceHab.GetExchangeEmails();

				if (archivo_email.Success.Equals(true))
				{
					//Guardo el Base64 de la Respuesta
					//string ruta_archivo = @"E:\Desarrollo\jzea\DIAN - Factura con Validacion Previa";
					string nombre_archivo = "Correos para Envio de Documentos de FE";
					ruta_archivo = string.Format(@"{0}\{1}.csv", ruta_archivo, nombre_archivo);
					FileStream fs = null;
					//Directorio.CrearDirectorio(ruta_xml);
					using (fs = new FileStream(ruta_archivo,
						FileMode.Create, FileAccess.ReadWrite))
					{
						BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
						bw.Write(archivo_email.CsvBase64Bytes);
						bw.Close();
						fs.Close();
					}

				}

				return ruta_archivo;
			}
			catch (Exception exec)
			{
				throw new ApplicationException("Error obteniendo archivo de correos de la DIAN", exec);
			}

		}


	}
}
