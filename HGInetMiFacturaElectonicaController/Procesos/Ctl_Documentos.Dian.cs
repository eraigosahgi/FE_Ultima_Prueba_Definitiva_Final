using HGInetDIANServicios;
using HGInetDIANServicios.DianFactura;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Genera el xml con la información del documento en formato UBL
		/// </summary>        
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="empresa">información del facturador electrónico en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static AcuseRecibo EnviarDian(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
		{
			HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
			try
			{
				respuesta.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();

				acuse = Ctl_DocumentoDian.Enviar(documento_result, empresa);
			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				return null;
			}

			respuesta.Cufe = documento_result.CUFE;

            PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

            // url pública del xml
            string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal(plataforma_datos.RutaPublica, documento_result.IdSeguridadTercero.ToString());
			respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);
			
			// url pública del zip
			string url_ppal_zip = string.Format(@"{0}{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreZip);

			documentoBd.StrCufe = respuesta.Cufe;
			documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
			documentoBd.StrUrlArchivoZip = url_ppal_zip;
			documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
			documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

			Ctl_Documento documento_tmp = new Ctl_Documento();
			documento_tmp.Actualizar(documentoBd);

			//Se da una pausa en proceso para que el servicio de la DIAN termine la validacion del documento
			System.Threading.Thread.Sleep(5000);

			return acuse;
		}

		public static void ConsultarDian()
		{

		}

		/// <summary>
		/// Consulta estado de documentos en la DIAN
		/// </summary>
		/// <param name="documentoBd">Documento en BD</param>
		/// <param name="empresa">Obligado a facturar</param>
		/// <param name="respuesta">Objeto de respuesta</param>
		/// <returns>Segun la respuesta de la DIAN cambia el estado del documento</returns>
		public static DocumentoRespuesta Consultar(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta)
		{

			DateTime fecha_actual = Fecha.GetFecha();
			Ctl_Documento documento_tmp = new Ctl_Documento();



			try
			{
				string IdSoftware = null;
				string PinSoftware = null;
				string clave = null;
				string url_ws_consulta = null;
				string obligado_identificacion = string.Empty;

				TipoDocumento doc_tipo = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documentoBd.IntDocTipo);

				// carpeta del xml
				string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// Nombre del archivo Xml 
				string archivo_xml = string.Format(@"{0}.xml", NombramientoArchivo.ObtenerXml(documentoBd.IntNumero.ToString(), documentoBd.StrEmpresaFacturador, doc_tipo));

				// ruta del xml
				string ruta_xml = string.Format(@"{0}{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					// obtiene los datos de prueba del proveedor tecnológico de la DIAN
					DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					clave = data_dian_habilitacion.ClaveAmbiente;
					url_ws_consulta = data_dian_habilitacion.UrlWSConsultaTransacciones;
					obligado_identificacion = Constantes.NitResolucionsinPrefijo;
				}
				else
				{
					// obtiene los datos del proveedor tecnológico de la DIAN
					DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
					clave = data_dian.ClaveAmbiente;
					url_ws_consulta = data_dian.UrlWSConsultaTransacciones;
					obligado_identificacion = empresa.StrIdentificacion;
				}


				HGInetDIANServicios.DianResultadoTransacciones.DocumentosRecibidos resultado = Ctl_ConsultaTransacciones.Consultar(Guid.NewGuid(), IdSoftware, clave, documentoBd.IntDocTipo, documentoBd.StrPrefijo, documentoBd.IntNumero.ToString(), obligado_identificacion, documentoBd.DatFechaDocumento, documentoBd.StrCufe, url_ws_consulta, ruta_xml);

				ConsultaDocumento resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccion(resultado);

                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                //Url publica de la respuesta de la DIAN en xml
                string url_ppal_respuesta = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal(plataforma_datos.RutaPublica, empresa.StrIdSeguridad.ToString());

				// se indica la respuesta de la DIAN
				respuesta.EstadoDian = new RespuestaDian();
				respuesta.EstadoDian.CodigoRespuesta = resultado_doc.CodigoEstadoDian;
				respuesta.EstadoDian.Descripcion = resultado_doc.EstadoDianDescripcion;
				respuesta.EstadoDian.EstadoDocumento = resultado_doc.Estado.GetHashCode();
				respuesta.EstadoDian.FechaConsulta = fecha_actual;
				respuesta.EstadoDian.UrlXmlRespuesta = string.Format(@"{0}{1}/{2}", url_ppal_respuesta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, archivo_xml);


				string detalle_dian = string.Empty;

				if (resultado_doc.Estado == EstadoDocumentoDian.Rechazado)
				{
					fecha_actual = Fecha.GetFecha();
					respuesta.DescripcionProceso = "Termina proceso.";
					respuesta.FechaUltimoProceso = fecha_actual;
					respuesta.IdProceso = ProcesoEstado.FinalizacionErrorDian.GetHashCode();
					respuesta.ProcesoFinalizado = 1;

					respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
					respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION;
					respuesta.Error.Fecha = fecha_actual;

					respuesta.Error.Mensaje = string.Format("Documento rechazado DIAN: {0} - Cod. {1} ", resultado_doc.EstadoDianDescripcion, resultado_doc.CodigoEstadoDian);

					//Actualiza Documento en Base de Datos
					documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
					documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

					documento_tmp.Actualizar(documentoBd);

				}
				return respuesta;
			}

			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la consulta del estado del documento en la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
				throw excepcion;
			}



		}


		/// <summary>
		/// Agrega los campos de la Dian correspondientes al Proveedor Tecnológico
		/// </summary>
		/// <param name="objeto_des"></param>
		/// <param name="tipo"></param>
		/// <param name="facturador"></param>
		/// <returns></returns>
		public static object AgregarCamposDian(object objeto_des, TipoDocumento tipo, TblEmpresas facturador)
		{

			string IdSoftware = null;
			string PinSoftware = null;
			string archivo_xml = string.Empty;

			XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

			// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
			if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
			{
				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

				IdSoftware = data_dian_habilitacion.IdSoftware;
				PinSoftware = data_dian_habilitacion.Pin;
			}
			else
			{
				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

				IdSoftware = data_dian.IdSoftware;
				PinSoftware = data_dian.Pin;
			}

			string software_security_code = string.Format("{0}{1}", IdSoftware, PinSoftware);
			string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);//Encriptación en SHA384 del string que contiene Identificador y el Pin del software

			dynamic objeto_valido = null;


			if (tipo == TipoDocumento.Factura)
			{
				objeto_valido = (InvoiceType)objeto_des;

			}
			else if (tipo == TipoDocumento.NotaDebito)
			{
				objeto_valido = (DebitNoteType)objeto_des;

			}
			else if (tipo == TipoDocumento.NotaCredito)
			{
				objeto_valido = (CreditNoteType)objeto_des;

			}

			//ingreso a los tag para llenar la informacion del Proveedor tecnologico segun el tipo de documento

			var archivo = (InvoiceType)objeto_des;

			foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
			{
				if (item.LocalName.Equals("SoftwareProvider"))
				{
					if (item.LocalName.Equals("SoftwareProvider"))
					{
						foreach (XmlNode item_child in item.ChildNodes)
						{
							if (item_child.LocalName.Equals("ProviderID"))
							{
								item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
							}
							if (item_child.LocalName.Equals("SoftwareID"))
							{
								item_child.ChildNodes[0].Value = IdSoftware;
							}
						}
					}

				}
				if (item.LocalName.Equals("SoftwareSecurityCode"))
				{
					item.ChildNodes[0].Value = software_security_code_encriptado;

				}
			}


			return objeto_valido;

		}


		/// <summary>
		/// Llena el Ubl con los campos del Proveedor tecnologico que son requeridos de la DIAN
		/// </summary>
		/// <param name="documento_result"></param>
		/// <param name="facturador"></param>
		public static void CamposDian(ref FacturaE_Documento documento_result, TblEmpresas facturador, ref DocumentoRespuesta respuesta)
		{
			string IdSoftware = null;
			string PinSoftware = null;
			string archivo_xml = string.Empty;

			try
			{


				XmlSerializerNamespaces namespaces_xml = NamespacesXML.Obtener();

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (facturador.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					// obtiene los datos de prueba del proveedor tecnológico de la DIAN
					DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
				}
				else
				{
					// obtiene los datos del proveedor tecnológico de la DIAN
					DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
				}

				string software_security_code = string.Format("{0}{1}", IdSoftware, PinSoftware);
				string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);//Encriptación en SHA384 del string que contiene Identificador y el Pin del software

				archivo_xml = string.Format(@"{0}{1}.xml", documento_result.RutaArchivosProceso, documento_result.NombreXml);

				try
				{
					// Un FileStream es necesario para leer un XML document.
					FileStream fs = new FileStream(archivo_xml, FileMode.Open);
					// convierte el objeto de acuerdo con el tipo de documento
					XmlSerializer xml_ser = null;

					//ingreso a los tag para llenar la informacion del Proveedor tecnologico segun el tipo de documento
					if (documento_result.DocumentoTipo == TipoDocumento.Factura)
					{
						xml_ser = new XmlSerializer(typeof(InvoiceType));

						InvoiceType archivo = (InvoiceType)xml_ser.Deserialize(fs);

						foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("SoftwareProvider"))
							{
								if (item.LocalName.Equals("SoftwareProvider"))
								{
									foreach (XmlNode item_child in item.ChildNodes)
									{
										if (item_child.LocalName.Equals("ProviderID"))
										{
											item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
										}
										if (item_child.LocalName.Equals("SoftwareID"))
										{
											item_child.ChildNodes[0].Value = IdSoftware;
										}
									}
								}

							}
							if (item.LocalName.Equals("SoftwareSecurityCode"))
							{
								item.ChildNodes[0].Value = software_security_code_encriptado;

							}
						}
						// convierte los datos del objeto en texto XML 
						StringBuilder texto_xml = Xml.Convertir<InvoiceType>(archivo, namespaces_xml);
						documento_result.DocumentoXml = texto_xml;
					}

					else if (documento_result.DocumentoTipo == TipoDocumento.NotaCredito)
					{

						xml_ser = new XmlSerializer(typeof(CreditNoteType));

						CreditNoteType archivo = (CreditNoteType)xml_ser.Deserialize(fs);

						foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("SoftwareProvider"))
							{
								if (item.LocalName.Equals("SoftwareProvider"))
								{
									foreach (XmlNode item_child in item.ChildNodes)
									{
										if (item_child.LocalName.Equals("ProviderID"))
										{
											item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
										}
										if (item_child.LocalName.Equals("SoftwareID"))
										{
											item_child.ChildNodes[0].Value = IdSoftware;
										}
									}
								}

							}
							if (item.LocalName.Equals("SoftwareSecurityCode"))
							{
								item.ChildNodes[0].Value = software_security_code_encriptado;

							}
						}
						// convierte los datos del objeto en texto XML 
						StringBuilder texto_xml = Xml.Convertir<CreditNoteType>(archivo, namespaces_xml);
						documento_result.DocumentoXml = texto_xml;

					}
					else if (documento_result.DocumentoTipo == TipoDocumento.NotaDebito)
					{
						xml_ser = new XmlSerializer(typeof(DebitNoteType));

						DebitNoteType archivo = (DebitNoteType)xml_ser.Deserialize(fs);

						foreach (XmlNode item in archivo.UBLExtensions[0].ExtensionContent.ChildNodes)
						{
							if (item.LocalName.Equals("SoftwareProvider"))
							{
								if (item.LocalName.Equals("SoftwareProvider"))
								{
									foreach (XmlNode item_child in item.ChildNodes)
									{
										if (item_child.LocalName.Equals("ProviderID"))
										{
											item_child.ChildNodes[0].Value = facturador.StrIdentificacion;
										}
										if (item_child.LocalName.Equals("SoftwareID"))
										{
											item_child.ChildNodes[0].Value = IdSoftware;
										}
									}
								}

							}
							if (item.LocalName.Equals("SoftwareSecurityCode"))
							{
								item.ChildNodes[0].Value = software_security_code_encriptado;

							}
						}
						// convierte los datos del objeto en texto XML 
						StringBuilder texto_xml = Xml.Convertir<DebitNoteType>(archivo, namespaces_xml);
						documento_result.DocumentoXml = texto_xml;

					}
					fs.Close();
				}
				catch (Exception excepcion)
				{
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la serializacion del documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					LogExcepcion.Guardar(excepcion);
					throw excepcion;
				}
				try
				{
					// carpeta del xml
					string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), facturador.StrIdSeguridad.ToString());
					carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

					// valida la existencia de la carpeta
					carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

					// ruta del xml
					string nombre_archivo_xml = string.Format(@"{0}.xml", documento_result.NombreXml);

					// ruta del xml
					string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, nombre_archivo_xml);

					// mueve el archivo xml recibido
					string archivo_xml_recibido = string.Format(@"{0}\recepcion_ws\{1}.xml", documento_result.RutaArchivosProceso, documento_result.NombreXml);
					if (Archivo.CopiarArchivo(ruta_xml, archivo_xml_recibido))
					{
						Archivo.Borrar(ruta_xml);

						// almacena el archivo xml
						string ruta_save = Xml.Guardar(documento_result.DocumentoXml, carpeta_xml, nombre_archivo_xml);
					}
				}
				catch (Exception excepcion)
				{
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error guardando archivo fisico con cambios en el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					LogExcepcion.Guardar(excepcion);
					throw excepcion;
				}

			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error agregando datos del Proveedor Tecnologico en el documento enviado. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
				throw excepcion;
			}

		}


	}
}
