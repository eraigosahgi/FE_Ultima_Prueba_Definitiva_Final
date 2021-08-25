using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HGInetDIANServicios.DianResolucion;
using System.IO;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetDIANServicios
{
	public partial class Ctl_Resolucion
	{
	
		public static DianResolucion.ResolucionesFacturacion Obtener_v2(Guid id_peticion, string identificador_software, string clave, string identificacion_empresa, string identificacion_proveedor, DateTime fecha, string ruta_xml, string ruta_certificado, string clave_certificado, string ruta_servicio_web)
		{
			DateTime fecha_envio = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha();
			try
			{

				DianWSValidacionPrevia.WcfDianCustomerServicesClient webServiceHab = new DianWSValidacionPrevia.WcfDianCustomerServicesClient();
				webServiceHab.Endpoint.Address = new System.ServiceModel.EndpointAddress(ruta_servicio_web);

				//Certificado de producción
				X509Certificate2 cert = new X509Certificate2(ruta_certificado, clave_certificado);
				webServiceHab.ClientCredentials.ClientCertificate.Certificate = cert;
				webServiceHab.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 5);

				//Se agrega instruccion para habilitar la seguridad en el envio
				System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

				DianWSValidacionPrevia.NumberRangeResponseList resultado = webServiceHab.GetNumberingRange(identificacion_empresa,identificacion_proveedor,identificador_software);

				DianResolucion.ResolucionesFacturacion respuesta = new ResolucionesFacturacion();

				if (resultado != null)
				{
					//Guardo la respuesta en XML
					var ser = new XmlSerializer(typeof(DianWSValidacionPrevia.NumberRangeResponseList));
					TextWriter writer = new StreamWriter(ruta_xml);
					ser.Serialize(writer, resultado);

					if (resultado.OperationCode.Equals("100"))
					{
						respuesta.CodigoOperacion = CodigoType.OK;
						respuesta.DescripcionOperacion = resultado.OperationDescription;
						respuesta.IdentificadorOperacion = 0;
						List<RangoFacturacion> numeracion = new List<RangoFacturacion>();
						foreach (var resolucion in resultado.ResponseList)
						{
							RangoFacturacion rango = new RangoFacturacion();
							rango.NumeroResolucion = Convert.ToInt64(resolucion.ResolutionNumber);
							rango.Prefijo = resolucion.Prefix;
							rango.ClaveTecnica = resolucion.TechnicalKey;
							rango.FechaResolucion = Convert.ToDateTime(resolucion.ResolutionDate);
							rango.FechaVigenciaDesde = Convert.ToDateTime(resolucion.ValidDateFrom);
							rango.FechaVigenciaHasta = Convert.ToDateTime(resolucion.ValidDateTo);
							rango.RangoInicial = resolucion.FromNumber;
							rango.RangoFinal = resolucion.ToNumber;
							numeracion.Add(rango);
						}

						respuesta.RangoFacturacion = numeracion.ToArray();
					}
					else
					{
						throw new ApplicationException(string.Format("Error al obtener las resoluciones del facturador electrónico {0}. Respuesta DIAN: {1} - {2}", identificacion_empresa, resultado.OperationCode, resultado.OperationDescription));
					}

				}
				else
				{
					throw new ApplicationException("No hay respuesta del servicio de la DIAN consultando resoluciones en Validación Previa");
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				MensajeCategoria log_categoria = MensajeCategoria.ServicioDian;
				MensajeAccion log_accion = MensajeAccion.consulta;
				DateTime fecha_excepcion = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha();
				string msg_custom = string.Format("Error Exec WS => Consulta Resolucion - Fecha Envio: {0} - Fecha Excepcion: {1}", fecha_envio, fecha_excepcion);

				RegistroLog.EscribirLog(excepcion, log_categoria, MensajeTipo.Error, log_accion, msg_custom);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
