using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
	public class Ctl_ConsultaTransacciones
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
		public static DianResultadoTransacciones.DocumentosRecibidos Consultar(Guid id_peticion, string identificador_software, string clave, int tipo_documento, string prefijo, string consecutivo, string identificacion_empresa, DateTime fecha_documento, string cufe, string ruta_servicio_web, string ruta_log)
		{
		
			try
			{
				string password = Encriptar.Encriptar_SHA256(clave);//Se encripta la clave en SHA256
				
				DianResultadoTransacciones.consultaDocumentoPortNameClient cliente = new DianResultadoTransacciones.consultaDocumentoPortNameClient();
				cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);
				
				var behavior = new PasswordDigestBehavior(identificador_software, password, id_peticion);
				cliente.Endpoint.Behaviors.Add(behavior);

				DianResultadoTransacciones.EnvioConsultaDocumento datos_envio = new DianResultadoTransacciones.EnvioConsultaDocumento()
				{
					CUFE = cufe,
					IdentificadorSoftware = identificador_software,
					NitEmisor = identificacion_empresa,
					TipoDocumento = tipo_documento,
					NumeroDocumento = string.Format("{0}{1}", prefijo, consecutivo),
					FechaGeneracion = fecha_documento
				};

				DianResultadoTransacciones.ConsultaResultadoValidacionDocumentosPeticion peticion = new DianResultadoTransacciones.ConsultaResultadoValidacionDocumentosPeticion(datos_envio);

				DianResultadoTransacciones.DocumentosRecibidos acuse = null;

				try
				{
					DianResultadoTransacciones.ConsultaResultadoValidacionDocumentosRespuesta respuesta = cliente.ConsultaResultadoValidacionDocumentos(peticion);

					acuse = respuesta.ConsultaResultadoValidacionDocumentosRespuesta1;
				}
				catch (Exception e)
				{
					// valida si la excepción es diferente de serializar para lanzarla
					if (!e.Message.Contains("deserializar") && !e.StackTrace.Contains("XmlSerializer") && !e.Message.Contains("Security") && !e.StackTrace.Contains("Security"))
						throw e;
				}
				finally
				{
					string carpeta = Path.GetDirectoryName(ruta_log) + @"\";

					string archivo = Path.GetFileNameWithoutExtension(ruta_log) + ".xml";

					// almacena el mensaje de respuesta del servicio web
					archivo = Xml.GuardarXml(behavior.Inspector.XmlResponse, carpeta, archivo);

					// corrige el formato entregado por Java Soap -05:00 y Z
					behavior.Inspector.XmlResponse = Fecha.CorregirFormato(behavior.Inspector.XmlResponse);

					// convierte la respuesta a string
					string xml_doc = Xml.Convertir(behavior.Inspector.XmlResponse);

					// convierte el xml al objeto resultado de la ejecución del servicio web
					//acuse = Convertir(xml_doc);
				}

				return acuse;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
