using HGInetDIANServicios.DianFactura;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
	public partial class Ctl_Factura
	{

		/// <summary>
		/// Envía el documento por medio del servicio web de la DIAN
		/// </summary>
		/// <param name="ruta_archivo_zip">Ruta del archivo .zip</param>
		/// <param name="nombre_archivo">nombre del archivo</param>
		/// <param name="ruta_certificado">Ruta del archivo del certificado digital .pfx</param>
		/// <param name="clave_certificado">Clave del certificado digital</param>
		/// <param name="clave_dian">Clave proporcionada en la plataforma de la Dian</param>
		/// <param name="ruta_servicio_web">Url del servicio web de la DIAN</param>
		/// <returns></returns>
		public static AcuseRecibo Enviar_v2(string ruta_zip, string nombre_archivo, string ruta_certificado, string clave_certificado, string clave_dian, string ruta_servicio_web)
		{
			try
			{
				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);
				DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
				webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);
				webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

				//Convertir archivo a bytes para su envio
				Byte[] bytes = File.ReadAllBytes(ruta_zip);

				//Se agrega instruccion para habilitar la seguridad en el envio
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
				//Ejecución de prueba DIAN Enviando archivo ZIP	
				DianWSValidacionPrevia.UploadDocumentResponse resultadoHab = webServiceHab.SendTestSetAsync(nombre_archivo, bytes, clave_dian);

				AcuseRecibo acuse_recibo = new AcuseRecibo();
				acuse_recibo.KeyV2 = resultadoHab.ZipKey;
				acuse_recibo.MessagesFieldV2 = resultadoHab.ErrorMessageList;
				acuse_recibo.ReceivedDateTime = Fecha.GetFecha();
				acuse_recibo.ResponseDateTime = Fecha.GetFecha();
				acuse_recibo.Version = "2";

				if(!string.IsNullOrWhiteSpace(resultadoHab.ZipKey))
					acuse_recibo.Response = 200;
				
				return acuse_recibo;
				
			}
			catch (Exception e)
			{
				throw e;
			}
		}



	}
}
