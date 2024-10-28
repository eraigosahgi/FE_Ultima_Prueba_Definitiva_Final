﻿using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HGInetDIANServicios
{
	/// <summary>
	/// Consultas del estado de transacciones en la DIAN
	/// https://www.dian.gov.co/fizcalizacioncontrol/herramienconsulta/FacturaElectronica/Factura%20Electrnica/Nuevo%20servicio%20Web%20Consulta%20del%20Resultado%20de%20Transacciones.pdf
	/// </summary>
	public partial class Ctl_ConsultaTransacciones
	{
		/// <summary>
		/// Consulta el estado de documentos en la DIAN
		/// </summary>
		/// <param name="identificador_software">Identificador del software proporcionado en la plataforma de la Dian</param>
		/// <param name="clave">Clave proporcionada en la plataforma de la Dian</param>
		/// <param name="tipo_documento">Tipo de documento (Factura, Nota Débito, Nota Crédito)
		/// <param name="prefijo">Prefijo del documento</param>
		/// <param name="consecutivo">Consecutivo del documento</param>
		/// <param name="identificacion_empresa">Identificación del facturador electrónico</param>
		/// <param name="fecha_documento">Fecha y hora de elaboración del documento</param>
		/// <param name="cufe">Cufe del documento</param>
		/// <param name="ruta_servicio_web">Ruta del servicio web de la DIAN</param>
		/// <param name="ruta_log">Ruta del servicio web de la DIAN</param>
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
					if (behavior.Inspector.XmlResponse == null)
						throw new ApplicationException("No hay respuesta del servicio de la DIAN consultando estado del documento");

					string carpeta = Path.GetDirectoryName(ruta_log) + @"\";

					string archivo = Path.GetFileNameWithoutExtension(ruta_log) + ".xml";

					// almacena el mensaje de respuesta del servicio web
					archivo = Xml.GuardarXml(behavior.Inspector.XmlResponse, carpeta, archivo);

					// corrige el formato entregado por Java Soap -05:00 y Z
					behavior.Inspector.XmlResponse = Fecha.CorregirFormato(behavior.Inspector.XmlResponse);

					// convierte la respuesta a string
					string xml_doc = Xml.Convertir(behavior.Inspector.XmlResponse);

					if (xml_doc.Contains("<SOAP-ENV:Fault>"))
						throw new ApplicationException("No hay respuesta del servicio de la DIAN consultando estado del documento");

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
		private static DianResultadoTransacciones.DocumentosRecibidos Convertir(string soapResponse)
		{

			// objeto de respuesta principal
			DianResultadoTransacciones.DocumentosRecibidos acuse = new DianResultadoTransacciones.DocumentosRecibidos();

			DianResultadoTransacciones.DocumentoRecibido detalle = new DianResultadoTransacciones.DocumentoRecibido();

			detalle.DatosBasicosDocumento = new DianResultadoTransacciones.DatosBasicosDocumento();

			List<DianResultadoTransacciones.DocumentoRecibido> detalles = new List<DianResultadoTransacciones.DocumentoRecibido>();


			//Bandera para detener proceso cuando sean varias respuesta y encuentre la respuesta exitosa
			bool encontro_resp = false;

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(soapResponse);

			var soapBody = xmlDocument.GetElementsByTagName("ns3:ConsultaResultadoValidacionDocumentosRespuesta")[0];

			//Valida que en la respuesta no este un Proceso de Validacion o de recibida
			bool validacion = soapResponse.Contains("EstadoDocumento>" + RespuestaDocumentoDian.Proceso.GetHashCode() + "</");

			bool recibida = soapResponse.Contains("EstadoDocumento>" + RespuestaDocumentoDian.Recibida.GetHashCode() + "</");

			if (validacion == false && recibida == false )
			{
				foreach (XmlNode item in soapBody)
				{
					if (item.LocalName.Equals("CodigoTransaccion"))
						acuse.CodigoTransaccion = Convert.ToInt32(item.InnerText);

					else if (item.LocalName.Equals("FechaTransaccion"))
						acuse.FechaTransaccion = System.DateTime.ParseExact(item.InnerText, Fecha.ObtenerFormato(item.InnerText), System.Globalization.CultureInfo.InvariantCulture);

					else if (item.LocalName.Equals("DescripcionTransaccion"))
						acuse.DescripcionTransaccion = item.InnerText;

					else if (item.LocalName.Equals("DocumentoRecibido") && !encontro_resp)
					{
						var received = xmlDocument.GetElementsByTagName("ns3:DocumentoRecibido")[0];

						foreach (XmlNode item2 in received)
						{
							if (item2.LocalName.Equals("DatosBasicosDocumento") && !encontro_resp)
							{
								var seguimiento = xmlDocument.GetElementsByTagName("ns3:DatosBasicosDocumento");

								foreach (XmlNode item3 in seguimiento)
								{

									if (!encontro_resp)
									{
										//se recorre todas las respuestas
										foreach (XmlNode item_child in item3.ChildNodes)
										{
											if (item_child.LocalName.Equals("Emisor"))
												detalle.DatosBasicosDocumento.Emisor = item_child.InnerText;
											else if (item_child.LocalName.Equals("FechaHoraEmision"))
												detalle.DatosBasicosDocumento.FechaHoraEmision = item_child.InnerText;
											else if (item_child.LocalName.Equals("EstadoDocumento"))
											{
												detalle.DatosBasicosDocumento.EstadoDocumento = item_child.InnerText;
												if (detalle.DatosBasicosDocumento.EstadoDocumento == RespuestaDocumentoDian.Exitosa.GetHashCode().ToString())
													encontro_resp = true;
											}
											else if (item_child.LocalName.Equals("DescripcionEstado"))
												detalle.DatosBasicosDocumento.DescripcionEstado = item_child.InnerText;
											else if (item_child.LocalName.Equals("NumeroDocumento"))
												detalle.DatosBasicosDocumento.NumeroDocumento = item_child.InnerText;
											else if (item_child.LocalName.Equals("CUFE"))
												detalle.DatosBasicosDocumento.CUFE = item_child.InnerText;
											else if (item_child.LocalName.Equals("FechaHoraRecepcion"))
												detalle.DatosBasicosDocumento.FechaHoraRecepcion = System.DateTime.ParseExact(item.InnerText, Fecha.ObtenerFormato(item.InnerText), System.Globalization.CultureInfo.InvariantCulture);
										}
									}

								}

								if (detalle.DatosBasicosDocumento.EstadoDocumento != RespuestaDocumentoDian.Exitosa.GetHashCode().ToString())
									detalle.DatosBasicosDocumento.DescripcionEstado = detalle.DatosBasicosDocumento.DescripcionEstado + " " + ObtenerErroresRespuesta(xmlDocument);
								detalles.Add(detalle);
							}
						}

						acuse.DocumentoRecibido = detalles.ToArray();
					}
				}
			}
			else
			{
				if (validacion == true)
				{
					detalle.DatosBasicosDocumento.EstadoDocumento = RespuestaDocumentoDian.Proceso.GetHashCode().ToString();
					detalle.DatosBasicosDocumento.DescripcionEstado = Enumeracion.GetDescription(RespuestaDocumentoDian.Proceso);
				}
				else
				{
					detalle.DatosBasicosDocumento.EstadoDocumento = RespuestaDocumentoDian.Recibida.GetHashCode().ToString();
					detalle.DatosBasicosDocumento.DescripcionEstado = Enumeracion.GetDescription(RespuestaDocumentoDian.Recibida);
				}
				detalles.Add(detalle);
				acuse.DocumentoRecibido = detalles.ToArray();

			}

			return acuse;

		}

		/// <summary>
		/// Retorna un string con la lista de errores del XML de respuesta de la Dian
		/// </summary>
		/// <param name="XmlDocument"></param>
		/// <returns></returns>
		public static string ObtenerErroresRespuesta(XmlDocument xmlDocument)
		{

			string DetalleIncorrecto = "";
			var receivedJS = xmlDocument.GetElementsByTagName("ns3:VerificacionFuncional")[0];

			if (receivedJS != null)
			{

				int i = 0;
				foreach (XmlNode item2 in receivedJS)
				{
					if (item2.LocalName.Equals("VerificacionDocumento"))
					{
						var seguimiento = xmlDocument.GetElementsByTagName("ns3:DescripcionVeriFunc")[i];

						foreach (XmlNode item3 in seguimiento)
						{
							if (item3.InnerText.Contains("Incorrecta:"))
							{
								DetalleIncorrecto = (DetalleIncorrecto != "") ? DetalleIncorrecto + ", " : DetalleIncorrecto;
								DetalleIncorrecto = DetalleIncorrecto + (item3.InnerText.Replace("Incorrecta:", (i + 1) + ". "));
							}

						}
						i = i + 1;
					}

				}
			}

			return DetalleIncorrecto;
		}


		/// <summary>
		/// Valida la respuesta de la consulta de transacciones
		/// </summary>
		/// <param name="documento">respuesta del servicio web</param>
		/// <returns>validación propia de HGI</returns>
		public static ConsultaDocumento ValidarTransaccion(DianResultadoTransacciones.DocumentosRecibidos documento)
		{

			ConsultaDocumento resultado = new ConsultaDocumento();
			resultado.RecepcionDocumento = ValidacionRespuestaDian.Pendiente;
			resultado.Mensaje = "";

			/*Descripción de la transacción
                200 = Transacción Exitosa
                300 = Excepción en el Sistema
                310 = Parámetros enviados con error
                320 = No existe información
            */
			resultado.ConsultaEstado = documento.CodigoTransaccion;

			switch (resultado.ConsultaEstado)
			{

				case 100: resultado.ConsultaEstadoDescripcion = "Error al procesar la solicitud WS entrante"; resultado.RecepcionDocumento = ValidacionRespuestaDian.Pendiente; break;

				case 200: resultado.ConsultaEstadoDescripcion = "Transacción Exitosa"; resultado.RecepcionDocumento = ValidacionRespuestaDian.Recibido; break;

				case 300: resultado.ConsultaEstadoDescripcion = "Excepción en el Sistema"; resultado.RecepcionDocumento = ValidacionRespuestaDian.Pendiente; break;

				case 310: resultado.ConsultaEstadoDescripcion = "Parámetros enviados con error"; resultado.RecepcionDocumento = ValidacionRespuestaDian.Recibido; break;

				case 320: resultado.ConsultaEstadoDescripcion = "No existe información"; resultado.RecepcionDocumento = ValidacionRespuestaDian.NoRecibido; break;

				default: resultado.ConsultaEstadoDescripcion = "No existe documentación del estado."; resultado.RecepcionDocumento = ValidacionRespuestaDian.Recibido; break;
			}


			if (documento.DocumentoRecibido != null)
			{
				/*Descripción del estado del documento
					7200000 = NO HAY RESPUESTA DE LA DIAN    
					7200001 = RECIBIDA
                    7200002 = EXITOSA
                    7200003 = EN PROCESO DE VALIDACIÓN
                    7200004 = FALLIDA (Documento no cumple 1 o más validaciones de DIAN)
                    7200005 = ERROR (El xml no es válido)
                */
				resultado.CodigoEstadoDian = documento.DocumentoRecibido[0].DatosBasicosDocumento.EstadoDocumento;

				switch (resultado.CodigoEstadoDian)
				{
					case "7200001":
						resultado.EstadoDianDescripcion = "RECIBIDA";
						resultado.Estado = EstadoDocumentoDian.Recibido;
						break;

					case "7200002":
						resultado.EstadoDianDescripcion = "EXITOSA";
						resultado.Estado = EstadoDocumentoDian.Aceptado;
						break;

					case "7200003":
						resultado.EstadoDianDescripcion = "EN PROCESO DE VALIDACIÓN";
						resultado.Estado = EstadoDocumentoDian.Recibido;
						break;

					case "7200004":
						resultado.EstadoDianDescripcion = documento.DocumentoRecibido[0].DatosBasicosDocumento.DescripcionEstado;
						resultado.Estado = EstadoDocumentoDian.Rechazado;
						break;

					case "7200005":
						resultado.EstadoDianDescripcion = "ERROR (El xml no es válido)";
						resultado.Estado = EstadoDocumentoDian.Rechazado;
						break;

					default:
						resultado.EstadoDianDescripcion = "No existe documentación del estado.";
						resultado.Estado = EstadoDocumentoDian.Pendiente;
						break;
				}
			}
			else
			{
				resultado.CodigoEstadoDian = resultado.RecepcionDocumento.ToString();
				resultado.EstadoDianDescripcion = documento.DescripcionTransaccion;
			}

			return resultado;
		}

	}

}
