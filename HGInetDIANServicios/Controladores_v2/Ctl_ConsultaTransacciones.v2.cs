using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
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


		public static List<DianWSValidacionPrevia.DianResponse> Consultar_v2(string TrackId, string ruta_xml, string ruta_certificado, string clave_certificado, string ruta_servicio_web)
		{

			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{


				DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
				webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				//Certificado de producción
				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);
				webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;

				//Se agrega instruccion para habilitar la seguridad en el envio
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

				log_categoria = MensajeCategoria.ServicioDian;
				log_accion = MensajeAccion.consulta;

				List<DianWSValidacionPrevia.DianResponse> resultado = null;

				if (!string.IsNullOrEmpty(TrackId))
				{

					resultado = webServiceHab.GetStatusZip(TrackId).ToList();

					//Guardo la respuesta en XML
					foreach (var respuesta in resultado)
					{
						string archivo = string.Format("{0}.xml", respuesta.XmlFileName);

						var ser = new XmlSerializer(typeof(List<DianWSValidacionPrevia.DianResponse>));
						TextWriter writer = new StreamWriter(string.Format(@"{0}\{1}", ruta_xml, archivo));
						ser.Serialize(writer, resultado);

						//Guardo el Base64 de la Respuesta
						string base64 = string.Format("{0}-Base64.xml", respuesta.XmlFileName);

						FileStream fs = new FileStream(string.Format(@"{0}\{1}", ruta_xml, base64), FileMode.Create,FileAccess.ReadWrite);
						BinaryWriter bw = new BinaryWriter(fs, Encoding.Unicode);
						bw.Write(respuesta.XmlBase64Bytes);
						bw.Close();
						fs.Close();
					}

				}
				else
				{
					log_categoria = MensajeCategoria.ServicioDian;
					log_accion = MensajeAccion.ninguna;
					throw new ApplicationException("No se encontro trackid");
				}

				return resultado;

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				//LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Valida la respuesta de la consulta de transacciones
		/// </summary>
		/// <param name="documento">respuesta del servicio web</param>
		/// <returns>validación propia de HGI</returns>
		public static ConsultaDocumento ValidarTransaccionV2(List<DianWSValidacionPrevia.DianResponse> documento)
		{

			MensajeCategoria log_categoria = MensajeCategoria.ServicioDian;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{
				ConsultaDocumento resultado = new ConsultaDocumento();
				resultado.RecepcionDocumento = ValidacionRespuestaDian.Pendiente;
				resultado.Mensaje = "";

				DianWSValidacionPrevia.DianResponse doc_valido = documento.Where(d => d.IsValid == true && d.StatusCode == "0").FirstOrDefault();

				//Se guarda errores presentados por la DIAN
				if (doc_valido.ErrorMessage != null)
				{
					resultado.Mensaje = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(doc_valido.ErrorMessage.ToList(), ";");
				}

				if (doc_valido != null)
				{
					resultado.CodigoEstadoDian = doc_valido.StatusCode;
					resultado.EstadoDianDescripcion = doc_valido.StatusDescription;
					resultado.Estado = EstadoDocumentoDian.Aceptado;
				}
				else
				{
					resultado.CodigoEstadoDian = "99";
					resultado.EstadoDianDescripcion = "validaciones contienen errores en campos mandatorios";
					resultado.Estado = EstadoDocumentoDian.Rechazado;
				}

				return resultado;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				//LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}
	}
}
