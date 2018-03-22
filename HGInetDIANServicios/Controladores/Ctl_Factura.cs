using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;

namespace HGInetDIANServicios
{
	public class Ctl_Factura
	{

		/// <summary>
		/// Envía el archivo .zip para verificación a la DIAN
		/// </summary>
		/// <param name="identificador_software">Identificador del software proporcionado en la plataforma de la Dian</param>
		/// <param name="clave">Clave proporcionada en la plataforma de la Dian</param>
		/// <param name="consecutivo">Consecutivo del documento</param>
		/// <param name="fecha">Fecha y hora del documento</param>
		/// <param name="identificacion_empresa">Identificación de la empresa</param>
		/// <param name="ruta_zip">Ruta del archivo .zip</param>
		public static DianFactura.AcuseRecibo Enviar(Guid id_peticion, string identificador_software, string clave, string prefijo, string consecutivo, DateTime fecha, string identificacion_empresa, string ruta_zip, string ruta_servicio_web)
		{
			try
			{
				string password = Encriptar.Encriptar_SHA256(clave);//Se encripta la clave en SHA256

				Byte[] bytes = File.ReadAllBytes(ruta_zip);
				String file = Convert.ToBase64String(bytes);

				DianFactura.facturaElectronicaPortNameClient cliente = new DianFactura.facturaElectronicaPortNameClient();
				cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				var behavior = new PasswordDigestBehavior(identificador_software, password, id_peticion);
				cliente.Endpoint.Behaviors.Add(behavior);

				DianFactura.EnvioFacturaElectronica datos_envio = new DianFactura.EnvioFacturaElectronica()
				{
					Document = bytes,
				InvoiceNumber = string.Format("{0}{1}", prefijo, consecutivo),
				IssueDate = Convert.ToDateTime(fecha.ToString(Fecha.formato_fecha_hora_completa)),
				NIT = identificacion_empresa
				};

				DianFactura.EnvioFacturaElectronicaPeticion peticion = new DianFactura.EnvioFacturaElectronicaPeticion(datos_envio);

				DianFactura.AcuseRecibo acuse = null;

				try
				{
					DianFactura.EnvioFacturaElectronicaRespuesta respuesta = cliente.EnvioFacturaElectronica(peticion);

					acuse = respuesta.EnvioFacturaElectronicaRespuesta1;
				}
				catch (Exception e)
				{
					// valida si la excepción es diferente de serializar para lanzarla
					if (!e.Message.Contains("deserializar") || !e.StackTrace.Contains("XmlSerializer") || !e.StackTrace.Contains("'Security'"))
						throw e;
				}
				finally
				{
					string carpeta = Directorio.CrearDirectorio(AppDomain.CurrentDomain.BaseDirectory + @"_LogFacturaE\");

					string archivo = id_peticion.ToString() + ".xml";

					// almacena el mensaje de respuesta del servicio web
					archivo = Xml.GuardarXml(behavior.Inspector.XmlResponse, carpeta, archivo);

					// corrige el formato entregado por Java Soap -05:00 y Z
					behavior.Inspector.XmlResponse = Fecha.CorregirFormato(behavior.Inspector.XmlResponse);

					// convierte la respuesta a string
					string xml_doc = Xml.Convertir(behavior.Inspector.XmlResponse);

					// convierte el xml al objeto resultado de la ejecución del servicio web
					acuse = Convertir(xml_doc);
				}

				return acuse;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Convierte la respuesta (texto xml) al objeto
		/// </summary>
		/// <param name="soapResponse">texto xml</param>
		/// <returns>objeto del servicio</returns>
		private static DianFactura.AcuseRecibo Convertir(string soapResponse)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(soapResponse);

			var soapBody = xmlDocument.GetElementsByTagName("ns2:EnvioDianFacturaRespuesta")[0];

			// objeto de respuesta principal
			DianFactura.AcuseRecibo acuse = new DianFactura.AcuseRecibo();

			// objeto de respuesta secundario
			List<DianFactura.AcuseRecibo> respuesta_envio = new List<DianFactura.AcuseRecibo>();

			foreach (XmlNode item in xmlDocument.GetElementsByTagName("ns2:EnvioDianFacturaRespuesta")[0])
			{
				if (item.LocalName.Equals("Version"))
					acuse.Version = item.InnerText;

				else if (item.LocalName.Equals("ReceivedDateTime"))
					acuse.ReceivedDateTime = System.DateTime.ParseExact(item.InnerText, Fecha.ObtenerFormato(item.InnerText), System.Globalization.CultureInfo.InvariantCulture);

				else if (item.LocalName.Equals("ResponseDateTime"))
					acuse.ResponseDateTime = System.DateTime.ParseExact(item.InnerText, Fecha.ObtenerFormato(item.InnerText), System.Globalization.CultureInfo.InvariantCulture);

				else if (item.LocalName.Equals("Response"))
					acuse.Response = int.Parse(item.InnerText);

				if (item.LocalName.Equals("Comments"))
					acuse.Comments = item.InnerText;

			}

			return acuse;

		}

	}
}
