using HGInetDIANServicios.DianFactura;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.RegistroLog;

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
		public static AcuseRecibo Enviar_v2(string ruta_zip, string nombre_archivo, string ruta_certificado, string clave_certificado, string clave_dian, string ruta_servicio_web, string ambiente)
		{

			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{

				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);

				//Convertir archivo a bytes para su envio
				Byte[] bytes = File.ReadAllBytes(ruta_zip);

				DianWSValidacionPrevia.UploadDocumentResponse resultadoHab = null;

				AcuseRecibo acuse_recibo = new AcuseRecibo();

				try
				{
					log_categoria = MensajeCategoria.ServicioDian;
					log_accion = MensajeAccion.envio;

					DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
					webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);
					webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

					//Se agrega instruccion para habilitar la seguridad en el envio
					System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

					if (ambiente.Equals("2"))
					{
						//Ejecución de prueba DIAN Enviando archivo ZIP	y IdsetDian
						resultadoHab = webServiceHab.SendTestSetAsync(nombre_archivo, bytes, clave_dian);
					}
					else
					{
						//Ejecución de produccion DIAN Enviando archivo ZIP	
						resultadoHab = webServiceHab.SendBillAsync(nombre_archivo, bytes);
					}

					acuse_recibo.ReceivedDateTime = Fecha.GetFecha();
					acuse_recibo.ResponseDateTime = Fecha.GetFecha();
					acuse_recibo.MessagesFieldV2 = resultadoHab.ErrorMessageList;
					acuse_recibo.KeyV2 = resultadoHab.ZipKey;
					acuse_recibo.Version = "2";
				}
				catch (Exception excepcion)
				{
					throw excepcion;
				}

				try
				{
					if (!string.IsNullOrWhiteSpace(resultadoHab.ZipKey))
					{
						acuse_recibo.Response = 200;
						acuse_recibo.Comments = "Documento Electrónico recibido exitosamente";
					}

					log_categoria = MensajeCategoria.Archivos;
					log_accion = MensajeAccion.creacion;

					string carpeta = Path.GetDirectoryName(ruta_zip) + @"\";

					string archivo = Path.GetFileNameWithoutExtension(ruta_zip) + ".xml";

					// almacena el mensaje de respuesta del servicio web
					archivo = Xml.GuardarObjeto(resultadoHab, carpeta, archivo);

				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				}


				return acuse_recibo;

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



	}
}
