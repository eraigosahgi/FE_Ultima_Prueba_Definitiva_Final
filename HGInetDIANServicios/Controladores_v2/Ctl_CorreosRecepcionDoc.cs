using System;
using System.Text;
using HGInetDIANServicios.DianWSValidacionPrevia;
using System.Security.Cryptography.X509Certificates;
using LibreriaGlobalHGInet.General;
using System.IO;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetDIANServicios
{
	public class Ctl_CorreosRecepcionDoc
	{

		public static string ObtenerCorreos(string ruta_certificado, string clave_certificado, string ruta_servicio_web, string ruta_archivo)
		{

			MensajeCategoria log_categoria = MensajeCategoria.ServicioDian;
			MensajeAccion log_accion = MensajeAccion.consulta;

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
					log_categoria = MensajeCategoria.ServicioDian;
					log_accion = MensajeAccion.exportar;
					//Guardo el Base64 de la Respuesta
					//string ruta_archivo = @"E:\Desarrollo\jzea\DIAN - Factura con Validacion Previa";
					string nombre_archivo = "Correos para Envio de Documentos de FE";
					ruta_archivo = string.Format(@"{0}\{1}.csv", ruta_archivo, nombre_archivo);
					if (Archivo.ValidarExistencia(ruta_archivo))
						Archivo.Borrar(ruta_archivo);
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
				RegistroLog.EscribirLog(exec, log_categoria, MensajeTipo.Error, log_accion);
				throw new ApplicationException(exec.Message, exec.InnerException);
				//throw new ApplicationException("Error obteniendo archivo de correos de la DIAN", exec);
			}

		}


	}
}
