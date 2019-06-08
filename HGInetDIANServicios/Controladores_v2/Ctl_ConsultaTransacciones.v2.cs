using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HGInetDIANServicios
{
	public partial class Ctl_ConsultaTransacciones
	{


		public static string Consultar_v2(string TrackId, string ruta_xml, string ruta_certificado, string clave_certificado, string clave_dian, string ruta_servicio_web)
		{

			try
			{
				
				DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
				webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				//Certificado de producción
				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);
				webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;
				
				List<DianWSValidacionPrevia.DianResponse> resultado = webServiceHab.GetStatusZip(TrackId).ToList();
				
				//Guardo la respuesta en XML
				foreach (var respuesta in resultado)
				{
					string archivo = string.Format("{0}-Respuesta.xml", respuesta.XmlFileName);
					
					var ser = new XmlSerializer(typeof(List<DianWSValidacionPrevia.DianResponse>));
					TextWriter writer = new StreamWriter(string.Format(@"{0}\{1}", ruta_xml, archivo));
					ser.Serialize(writer, resultado);

					//Guardo el Base64 de la Respuesta
					string base64 = string.Format("{0}-Base64.xml", respuesta.XmlFileName);

					FileStream fs = new FileStream(string.Format(@"{0}\{1}", ruta_xml, base64), FileMode.Create, FileAccess.ReadWrite);
					BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
					bw.Write(respuesta.XmlBase64Bytes);
					bw.Close();
					fs.Close();
				}

				return ruta_xml;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
