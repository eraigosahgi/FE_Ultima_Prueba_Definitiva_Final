using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet;
using System.IO;

namespace HGInetDIANServicios
{
	public class Ctl_Resolucion
	{
		public static DianResolucion.ResolucionesFacturacion Obtener(Guid id_peticion, string identificador_software, string clave, string identificacion_empresa, string identificacion_proveedor, DateTime fecha, string archivo_log, bool prueba = false)
		{
			try
			{
				//LibreriaGlobalHGInet.Formato.ConfiguracionRegional.Predeterminar();

				string ruta_servicio_web = "";
				/*
				if (!prueba)                
					ruta_servicio_web = "https://facturaelectronica.dian.gov.co/servicios/B2BIntegrationEngine-servicios/FacturaElectronica/consultaResolucionesFacturacion.wsdl";
				else
					ruta_servicio_web = "https://facturaelectronica.dian.gov.co/habilitacion/B2BIntegrationEngine/FacturaElectronica/consultaResolucionesFacturacion.wsdl";
				*/

				ruta_servicio_web = "https://facturaelectronica.dian.gov.co/servicios/B2BIntegrationEngine-servicios/FacturaElectronica/consultaResolucionesFacturacion.wsdl";


				string password = Encriptar.Encriptar_SHA256(clave);//Se encripta la clave en SHA256

				DianResolucion.resolucionFacturacionPortNameClient cliente = new DianResolucion.resolucionFacturacionPortNameClient();
				cliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				PasswordDigestBehavior behavior = new PasswordDigestBehavior(identificador_software, password, id_peticion);
				cliente.Endpoint.Behaviors.Add(behavior);

				DianResolucion.ConsultaResoluciones datos_envio = new DianResolucion.ConsultaResoluciones();
				datos_envio.IdentificadorSoftware = identificador_software;
				datos_envio.NITObligadoFacturarElectronicamente = identificacion_empresa;
				datos_envio.NITProveedorTecnologico = identificacion_proveedor;

				DianResolucion.ConsultaResolucionesFacturacionPeticion peticion = new DianResolucion.ConsultaResolucionesFacturacionPeticion(datos_envio);
				DianResolucion.ResolucionesFacturacion respuesta = null;
				try
				{
					DianResolucion.ConsultaResolucionesFacturacionRespuesta respuestaE = cliente.ConsultaResolucionesFacturacion(peticion);
				}
				catch (Exception e)
				{
                    // valida si la excepción es diferente de serializar para lanzarla
                    if (!e.Message.Contains("deserializar") && !e.StackTrace.Contains("XmlSerializer") && !e.Message.Contains("Security") && !e.StackTrace.Contains("Security"))
                        throw e;
                }
				finally
				{
                    if (behavior.Inspector.XmlResponse == null)
                        throw new ApplicationException("No hay respuesta del servicio de la DIAN consultando resoluciones");
						
					string carpeta = Path.GetDirectoryName(archivo_log) + @"\";

					string archivo = Path.GetFileNameWithoutExtension(archivo_log) + ".xml";
					
					// almacena el mensaje de respuesta del servicio web
					archivo = Xml.GuardarXml(behavior.Inspector.XmlResponse, carpeta, archivo);

					// corrige el formato entregado por Java Soap -05:00 y Z
					behavior.Inspector.XmlResponse = Fecha.CorregirFormato(behavior.Inspector.XmlResponse);

					// convierte la respuesta a string
					string xml_doc = Xml.Convertir(behavior.Inspector.XmlResponse);

					// convierte el xml al objeto resultado de la ejecución del servicio web
					if (!xml_doc.Contains("Fault"))
						respuesta = Convertir(xml_doc);
				}

				return respuesta;

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
		private static DianResolucion.ResolucionesFacturacion Convertir(string soapResponse)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(soapResponse);

			//var soapBody = xmlDocument.GetElementsByTagName("SOAP-ENV:Body")[0];

			var soapBody = xmlDocument.GetElementsByTagName("ns2:ConsultaResolucionesFacturacionRespuesta")[0];

			// objeto de respuesta principal
			DianResolucion.ResolucionesFacturacion resoluciones = new DianResolucion.ResolucionesFacturacion();

			// objeto de respuesta secundario
			List<DianResolucion.RangoFacturacion> rangos_resoluciones = new List<DianResolucion.RangoFacturacion>();

			foreach (XmlNode item in xmlDocument.GetElementsByTagName("ns2:ConsultaResolucionesFacturacionRespuesta")[0])
			{
				if (item.LocalName.Equals("CodigoOperacion"))
					resoluciones.CodigoOperacion = Enumeracion.ParseToEnum<DianResolucion.CodigoType>(item.InnerText);

				else if (item.LocalName.Equals("DescripcionOperacion"))
					resoluciones.DescripcionOperacion = item.InnerText;

				else if (item.LocalName.Equals("IdentificadorOperacion"))
					resoluciones.IdentificadorOperacion = Decimal.Parse(item.InnerText);

				else if (item.LocalName.Equals("RangoFacturacion"))
				{
					DianResolucion.RangoFacturacion resolucion = new DianResolucion.RangoFacturacion();

					foreach (XmlNode itemRango in item.ChildNodes)
					{
						if (itemRango.LocalName.Equals("NumeroResolucion"))
							resolucion.NumeroResolucion = long.Parse(itemRango.InnerText);
						else if (itemRango.LocalName.Equals("FechaResolucion"))
							resolucion.FechaResolucion = System.DateTime.ParseExact(itemRango.InnerText, Fecha.ObtenerFormato(itemRango.InnerText), System.Globalization.CultureInfo.InvariantCulture);
						else if (itemRango.LocalName.Equals("Prefijo"))
							resolucion.Prefijo = itemRango.InnerText;
						else if (itemRango.LocalName.Equals("RangoInicial"))
							resolucion.RangoInicial = long.Parse(itemRango.InnerText);
						else if (itemRango.LocalName.Equals("RangoFinal"))
							resolucion.RangoFinal = long.Parse(itemRango.InnerText);
						else if (itemRango.LocalName.Equals("FechaVigenciaDesde"))
							resolucion.FechaVigenciaDesde = System.DateTime.ParseExact(itemRango.InnerText, Fecha.ObtenerFormato(itemRango.InnerText), System.Globalization.CultureInfo.InvariantCulture);
						else if (itemRango.LocalName.Equals("FechaVigenciaHasta"))
							resolucion.FechaVigenciaHasta = System.DateTime.ParseExact(itemRango.InnerText, Fecha.ObtenerFormato(itemRango.InnerText), System.Globalization.CultureInfo.InvariantCulture);
						else if (itemRango.LocalName.Equals("ClaveTecnica"))
							resolucion.ClaveTecnica = itemRango.InnerText;
					}

					rangos_resoluciones.Add(resolucion);
				}
			}

			resoluciones.RangoFacturacion = rangos_resoluciones.ToArray();

			return resoluciones;

			/*
			string innerObject = soapBody.InnerXml;

			XmlSerializer deserializer = new XmlSerializer(typeof(DianResolucion.ResolucionesFacturacion));

			using (StringReader reader = new StringReader(innerObject))
			{
				return (DianResolucion.ResolucionesFacturacion)deserializer.Deserialize(reader);
			}*/
		}

	}
}
