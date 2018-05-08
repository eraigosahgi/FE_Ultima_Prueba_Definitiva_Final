using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.Formato;
using System.Text.RegularExpressions;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData;
using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar los documentos
	/// </summary>
	public class Ctl_Documentos
	{
		/// <summary>
		/// Procesa una lista de documentos tipo Factura
		/// </summary>
		/// <param name="documentos">documentos tipo Factura</param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
		{
			try
			{
				string resolucion_pruebas = "9000000033394696";


				Ctl_Empresa Peticion = new Ctl_Empresa();

				//Válida que la key sea correcta.
				TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

				if (!facturador_electronico.IntObligado)
					throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

				// genera un id único de la plataforma
				Guid id_peticion = Guid.NewGuid();

				DateTime fecha_actual = Fecha.GetFecha();

				List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
				if (facturador_electronico.IntHabilitacion < 99)
				{

					Tercero DatosObligado = new Tercero()
					{
						Identificacion = "811021438",
						IdentificacionDv = 4,
						TipoIdentificacion = 31,
						TipoPersona = 1,
						Regimen = 2,
						NombreComercial = "HGI",
						Departamento = "Antioquia",
						Ciudad = "Medellin",
						Direccion = "Calle 48 Nro. 77C-06",
						Telefono = "4444584",
						Email = "info@hgi.com.co",
						PaginaWeb = null,
						CodigoPais = "CO",
						RazonSocial = "HGI SAS",
						PrimerApellido = null,
						SegundoApellido = null,
						PrimerNombre = null,
						SegundoNombre = null
					};


					Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
					lista_resolucion.Add(_resolucion.Obtener(DatosObligado.Identificacion, resolucion_pruebas));

					foreach (var item in documentos)
					{
						item.NumeroResolucion = resolucion_pruebas;
						item.DatosObligado = DatosObligado;

					}
				}
				else
				{

					// actualiza las resoluciones de los servicios web de la DIAN en la base de datos
					lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, documentos.FirstOrDefault().DatosObligado.Identificacion);
				}


				List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

				foreach (Factura item in documentos)
				{
					Ctl_Documento num_doc = new Ctl_Documento();

					//valida si el Documento ya existe en Base de Datos
					TblDocumentos numero_documento = num_doc.Obtener(item.NumeroResolucion, item.Documento);

					if (numero_documento != null)
						throw new ApplicationException(string.Format("El documento {0} ya xiste para el Facturador Electrónico {1}", item.Documento, facturador_electronico.StrIdentificacion));


					// filtra la resolución del documento
					TblEmpresasResoluciones resolucion = lista_resolucion.Where(_resolucion => _resolucion.StrNumResolucion.Equals(item.NumeroResolucion)).FirstOrDefault();

					// realiza el proceso de envío a la DIAN del documento
					DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.Factura, resolucion, facturador_electronico);

					respuesta.Add(respuesta_tmp);
				}

				return respuesta;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}


		/// <summary>
		/// Realiza el proceso de la plataforma para el documento
		/// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN
		/// </summary>
		/// <param name="id_peticion">id único de identificación de la plataforma</param>
		/// <param name="documento_obj">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos de resultado para el documento</returns>
		public static DocumentoRespuesta Procesar(Guid id_peticion, object documento, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, TblEmpresas empresa)
		{
			string numero_resolucion = string.Empty;
			string prefijo = string.Empty;

			var documento_obj = (dynamic)null;

			if (tipo_doc == TipoDocumento.Factura)
			{
				documento_obj = documento;
				numero_resolucion = documento_obj.NumeroResolucion;
				prefijo = documento_obj.Prefijo;
			}
			else if (tipo_doc == TipoDocumento.NotaCredito)
			{
				documento_obj = documento;
			}
			else if (tipo_doc == TipoDocumento.NotaDebito)
				documento_obj = documento;

			if (documento_obj != null)
			{

				DateTime fecha_actual = Fecha.GetFecha();

				FacturaE_Documento documento_result = new FacturaE_Documento();

				DocumentoRespuesta respuesta = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					CodigoRegistro = documento_obj.CodigoRegistro,
					Cufe = "",
					DescripcionProceso = "Recepción - Información del documento.",
					Documento = documento_obj.Documento,
					Error = null,
					FechaRecepcion = fecha_actual,
					FechaUltimoProceso = fecha_actual,
					IdDocumento = Guid.NewGuid().ToString(),
					Identificacion = documento_obj.DatosObligado.Identificacion,
					IdProceso = 1,
					MotivoRechazo = "",
					NumeroResolucion = numero_resolucion,
					Prefijo = prefijo,
					ProcesoFinalizado = 0,
					UrlPdf = "",
					UrlXmlUbl = ""
				};

				try
				{

					// valida la información del documento
					try
					{
						fecha_actual = Fecha.GetFecha();
						respuesta.DescripcionProceso = "Valida la información del documento.";
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.IdProceso = 2;


						if (tipo_doc == TipoDocumento.Factura)
							documento_obj = Validar(documento_obj, resolucion);
						else if (tipo_doc == TipoDocumento.NotaCredito)
							documento_obj = ValidarNotaCredito(documento_obj, resolucion);
					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la validación del documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);


						throw excepcion; ;
					}




					if (empresa.IntHabilitacion > 0)
					{

						Ctl_Empresa empresa_config = new Ctl_Empresa();

						TblEmpresas adquirienteBd = null;
						TblUsuarios usuarioBd = null;

						bool adquiriente_nuevo = false;



						//Validacion de Adquiriente y usuario
						try
						{

							//Obtiene la informacion del Adquiriente que se tiene en BD
							adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);

							//Si no existe Adquiriente se crea en BD y se crea Usuario
							if (adquirienteBd == null)
							{
								empresa_config = new Ctl_Empresa();
								//Creacion del Adquiriente
								adquirienteBd = empresa_config.Crear(documento_obj.DatosAdquiriente);

								//Creacion del Usuario del Adquiriente
								Ctl_Usuario usuario = new Ctl_Usuario();
								usuarioBd = usuario.Crear(adquirienteBd);

								adquiriente_nuevo = true;
							}
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);

							throw excepcion;
						}

						Ctl_Documento documento_tmp = new Ctl_Documento();

						//guarda documento en BD
						TblDocumentos documentoBd = Ctl_Documento.Convertir(respuesta, documento_obj, resolucion, empresa, adquirienteBd, tipo_doc);

						try
						{

							documento_tmp.Crear(documentoBd);

						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al guardar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);

							throw excepcion;
						}


						// genera el xml en ubl
						try
						{
							fecha_actual = Fecha.GetFecha();
							respuesta.DescripcionProceso = "Genera información en estandar UBL.";
							respuesta.FechaUltimoProceso = fecha_actual;
							respuesta.IdProceso = 3;

							//Actualiza documento en la Base de Datos
							documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
							documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

							documento_tmp.Actualizar(documentoBd);

							//Genera Ubl
							if (tipo_doc == TipoDocumento.Factura)
								documento_result = Ctl_Ubl.Generar(id_peticion, documento_obj, tipo_doc, empresa, resolucion);
							else if (tipo_doc == TipoDocumento.NotaCredito)
								documento_result = Ctl_Ubl.Generar(id_peticion, documento_obj, tipo_doc, empresa, resolucion);

						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la generación del estandar UBL del documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

							throw excepcion;

						}

						// almacena el xml en ubl
						try
						{
							fecha_actual = Fecha.GetFecha();
							respuesta.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
							respuesta.FechaUltimoProceso = fecha_actual;
							respuesta.IdProceso = 4;

							// almacena el xml
							documento_result = Ctl_Ubl.Almacenar(documento_result);

							//Actualiza Documento en Base de Datos
							documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
							documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

							documento_tmp.Actualizar(documentoBd);
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento UBL en XML. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

							throw excepcion;
						}


						//Almacena Formato
						try
						{
							Formato formato_documento = (Formato)documento_obj.DocumentoFormato;

							if (formato_documento != null)
							{
								// almacena archivo en base64
								documento_result.NombrePdf = Ctl_Formato.GuardarArchivo(formato_documento.ArchivoPdf, empresa, documento_result);
								respuesta.UrlPdf = "";

								if (!string.IsNullOrWhiteSpace(documento_result.NombrePdf))
								{
									// url pública del pdf
									string url_ppal_pdf = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_obj.DatosObligado.Identificacion);
									respuesta.UrlPdf = string.Format(@"{0}{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombrePdf);
								}
							}

						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento PDF. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
						}



						// firma el xml
						try
						{
							fecha_actual = Fecha.GetFecha();
							respuesta.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
							respuesta.FechaUltimoProceso = fecha_actual;
							respuesta.IdProceso = 5;

							// obtiene la información de configuración del certificado digital
							CertificadoDigital certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

							// obtiene la empresa certificadora
							EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

							if (certificado.Certificadora.Equals("andes"))
								empresa_certificadora = EnumCertificadoras.Andes;
							else if (certificado.Certificadora.Equals("gse"))
								empresa_certificadora = EnumCertificadoras.Gse;

							// información del certificado digital
							string ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);
							documento_result = Ctl_Firma.Generar(ruta_certificado, certificado.Serial, certificado.Clave, empresa_certificadora, documento_result);

							//Actualiza Documento en Base de Datos
							documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
							documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
							documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

							documento_tmp.Actualizar(documentoBd);
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el firmado del documento UBL en XML. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

							throw excepcion;
						}

						// comprime el archivo xml firmado
						try
						{
							fecha_actual = Fecha.GetFecha();
							respuesta.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
							respuesta.FechaUltimoProceso = fecha_actual;
							respuesta.IdProceso = 6;

							documento_result.NombreZip = Ctl_Compresion.Comprimir(documento_result);

							//Actualiza Documento en Base de Datos
							documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
							documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

							documento_tmp.Actualizar(documentoBd);
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la compresión del documento UBL en XML firmado. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

							throw excepcion;

						}

						// envía el archivo zip con el xml firmado a la DIAN
						HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
						try
						{
							fecha_actual = Fecha.GetFecha();
							respuesta.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
							respuesta.FechaUltimoProceso = fecha_actual;
							respuesta.IdProceso = 7;

							acuse = Ctl_DocumentoDian.Enviar(documento_result);
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

							throw excepcion;

						}

						respuesta.Cufe = documento_result.CUFE;

						// url pública del xml
						string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_obj.DatosObligado.Identificacion);
						respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

						// url pública del zip
						string url_ppal_zip = string.Format(@"{0}{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreZip);

						documentoBd.StrCufe = respuesta.Cufe;
						documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
						documentoBd.StrUrlArchivoZip = url_ppal_zip;
						documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
						documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

						documento_tmp.Actualizar(documentoBd);

						//Se da una pausa en proceso para que el servicio de la DIAN termine la validacion del documento
						System.Threading.Thread.Sleep(5000);

						//Valida estado del documento en la Plataforma de la DIAN
						try
						{
							string IdSoftware = null;
							string PinSoftware = null;
							string clave = null;
							string url_ws_consulta = null;

							// carpeta del xml
							string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), documento_obj.DatosObligado.Identificacion);
							carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

							// valida la existencia de la carpeta
							carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

							// ruta del xml
							string archivo_xml = string.Format(@"{0}{1}.xml", documentoBd.StrPrefijo, documentoBd.IntNumero.ToString());

							// ruta del xml
							string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

							// elimina el archivo xml si existe
							if (Archivo.ValidarExistencia(ruta_xml))
								Archivo.Borrar(ruta_xml);

							// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
							if (empresa.IntHabilitacion < 99)
							{
								// obtiene los datos de prueba del proveedor tecnológico de la DIAN
								DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

								IdSoftware = data_dian_habilitacion.IdSoftware;
								PinSoftware = data_dian_habilitacion.Pin;
								clave = data_dian_habilitacion.ClaveAmbiente;
								url_ws_consulta = data_dian_habilitacion.UrlWSConsultaTransacciones;
							}
							else
							{
								// obtiene los datos del proveedor tecnológico de la DIAN
								DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

								IdSoftware = data_dian.IdSoftware;
								PinSoftware = data_dian.Pin;
								clave = data_dian.ClaveAmbiente;
								url_ws_consulta = data_dian.UrlWSConsultaTransacciones;
							}


							HGInetDIANServicios.DianResultadoTransacciones.DocumentosRecibidos resultado = Ctl_ConsultaTransacciones.Consultar(Guid.NewGuid(), IdSoftware, clave, documentoBd.IntDocTipo, documentoBd.StrPrefijo, documentoBd.IntNumero.ToString(), documento_obj.DatosObligado.Identificacion, documentoBd.DatFechaDocumento, documentoBd.StrCufe, url_ws_consulta, ruta_xml);

							ConsultaDocumento resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccion(resultado);

							string detalle_dian = string.Empty;

							if (resultado_doc.EstadoDian.Equals("7200003"))
							{
								// se indica la respuesta de la DIAN
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
								respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION;
								respuesta.Error.Fecha = fecha_actual;

								detalle_dian = string.Empty;

								if (acuse.ReceivedInvoice != null)
									detalle_dian = acuse.ReceivedInvoice.Comments;

								if (string.IsNullOrWhiteSpace(respuesta.Error.Mensaje))
									respuesta.Error.Mensaje = "ninguno.";

								respuesta.Error.Mensaje = string.Format("Respuesta DIAN: {0} - Cod. {1} - {2} - {3}. Detalles: {4}", acuse.ResponseDateTime, resultado_doc.EstadoDian, acuse.Comments, detalle_dian, respuesta.Error.Mensaje);

							}
							else if (resultado_doc.EstadoDian.Equals("7200002"))
							{
								// se indica la respuesta de la DIAN
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
								respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.OK;
								respuesta.Error.Fecha = fecha_actual;


								respuesta.Error.Mensaje = string.Format("Respuesta DIAN: {0} - Cod. {1} ", resultado_doc.EstadoDianDescripcion, resultado_doc.EstadoDian);

								//Envio de mail
								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								try
								{
									fecha_actual = Fecha.GetFecha();
									respuesta.DescripcionProceso = "Envío correo adquiriente.";
									respuesta.FechaUltimoProceso = fecha_actual;
									respuesta.IdProceso = 8;

									//Si es nuevo en la Plataforma envia Bienvenida a la plataforma
									if (adquiriente_nuevo == true)
									{

										email.Bienvenida(adquirienteBd, usuarioBd);
									}

									//envío de los documentos al Adquiriente
									email.NotificacionDocumento(documentoBd, documento_obj.DatosObligado.Telefono, resultado_doc.EstadoDian);

									//Actualiza Documento en Base de Datos
									documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
									documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

									documento_tmp.Actualizar(documentoBd);
								}
								catch (Exception excepcion)
								{
									respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el Envío correo adquiriente. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

									throw excepcion;

								}

							}
							else if (resultado_doc.EstadoDian.Equals("7200004") || resultado_doc.EstadoDian.Equals("7200005"))
							{

								fecha_actual = Fecha.GetFecha();
								respuesta.DescripcionProceso = "Termina proceso.";
								respuesta.FechaUltimoProceso = fecha_actual;
								respuesta.IdProceso = 99;
								respuesta.ProcesoFinalizado = 1;

								// se indica la respuesta de la DIAN
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
								respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION;
								respuesta.Error.Fecha = fecha_actual;

								respuesta.Error.Mensaje = string.Format("Respuesta DIAN: {0} - Cod. {1} ", resultado_doc.EstadoDianDescripcion, resultado_doc.EstadoDian);

								//Actualiza Documento en Base de Datos
								documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
								documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

								documento_tmp.Actualizar(documentoBd);

							}

						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la consulta del estado del documento en la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

							throw excepcion;

						}
						

					}


				}
				catch (Exception excepcion)
				{
					// no se controla excepción
				}

				return respuesta;
			}
			throw new ArgumentException("No se recibieron datos para realizar el proceso");
		}

		/// <summary>
		/// Procesa una lista de documentos tipo NotaCredito
		/// </summary>
		/// <param name="documentos">documentos tipo NotaCredito</param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<NotaCredito> documentos)
		{
			string resolucion_pruebas = "9000000033394696";


			Ctl_Empresa Peticion = new Ctl_Empresa();

			//Válida que la key sea correcta.
			TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

			if (!facturador_electronico.IntObligado)
				throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

			// genera un id único de la plataforma
			Guid id_peticion = Guid.NewGuid();

			DateTime fecha_actual = Fecha.GetFecha();

			List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

			// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
			if (facturador_electronico.IntHabilitacion < 99)
			{

				Tercero DatosObligado = new Tercero()
				{
					Identificacion = "811021438",
					IdentificacionDv = 4,
					TipoIdentificacion = 31,
					TipoPersona = 1,
					Regimen = 2,
					NombreComercial = "HGI",
					Departamento = "Antioquia",
					Ciudad = "Medellin",
					Direccion = "Calle 48 Nro. 77C-06",
					Telefono = "4444584",
					Email = "info@hgi.com.co",
					PaginaWeb = null,
					CodigoPais = "CO",
					RazonSocial = "HGI SAS",
					PrimerApellido = null,
					SegundoApellido = null,
					PrimerNombre = null,
					SegundoNombre = null
				};


				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
				lista_resolucion.Add(_resolucion.Obtener(DatosObligado.Identificacion, resolucion_pruebas));

				foreach (var item in documentos)
				{
					item.NumeroResolucion = resolucion_pruebas;
					item.DatosObligado = DatosObligado;

				}
			}
			else
			{


				// Obtiene la resolucion de la base de datos
				Ctl_EmpresaResolucion _resolucion_factura = new Ctl_EmpresaResolucion();
				lista_resolucion.Add(_resolucion_factura.Obtener(documentos.FirstOrDefault().DatosObligado.Identificacion, documentos.FirstOrDefault().NumeroResolucion));
			}

			List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

			foreach (NotaCredito item in documentos)
			{
				// filtra la resolución del documento
				TblEmpresasResoluciones resolucion = lista_resolucion.Where(_resolucion => _resolucion.StrNumResolucion.Equals(item.NumeroResolucion)).FirstOrDefault();


				// realiza el proceso de envío a la DIAN del documento
				DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.NotaCredito, resolucion, facturador_electronico);

				respuesta.Add(respuesta_tmp);
			}

			return respuesta;

		}

		/// <summary>
		/// Procesa una lista de documentos tipo NotaDebito
		/// </summary>
		/// <param name="documentos">documentos tipo NotaDebito</param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<NotaDebito> documentos)
		{
			Ctl_Empresa Peticion = new Ctl_Empresa();

			//Válida que la key sea correcta.
			TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

			if (!facturador_electronico.IntObligado)
				throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

			// genera un id único de la plataforma
			Guid id_peticion = Guid.NewGuid();

			DateTime fecha_actual = Fecha.GetFecha();

			// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
			if (facturador_electronico.IntHabilitacion < 99)
			{

				Tercero DatosObligado = new Tercero()
				{
					Identificacion = "811021438",
					IdentificacionDv = 4,
					TipoIdentificacion = 31,
					TipoPersona = 1,
					Regimen = 2,
					NombreComercial = "HGI",
					Departamento = "Antioquia",
					Ciudad = "Medellin",
					Direccion = "Calle 48 Nro. 77C-06",
					Telefono = "4444584",
					Email = "info@hgi.com.co",
					PaginaWeb = null,
					CodigoPais = "CO",
					RazonSocial = "HGI SAS",
					PrimerApellido = null,
					SegundoApellido = null,
					PrimerNombre = null,
					SegundoNombre = null
				};

			}
			List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

			foreach (object item in documentos)
			{
				// realiza el proceso de envío a la DIAN del documento
				DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.NotaDebito, null, facturador_electronico);

				respuesta.Add(respuesta_tmp);
			}

			return respuesta;

		}


		/// <summary>
		/// Validación del Objeto Factura
		/// </summary>
		/// <param name="documento">Objeto factura</param>
		/// <returns></returns>
		public static Factura Validar(Factura documento, TblEmpresasResoluciones resolucion)
		//public static Factura Validar(Factura documento)
		{
			// valida objeto recibido
			if (documento == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Factura"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.CodigoRegistro))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.DataKey))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

			// valida el número del documento no sea valor negativo
			if (documento.Documento < 0)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

			//Validar que no este vacio y este vigente en los terminos.
			if (string.IsNullOrEmpty(documento.NumeroResolucion))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));

			//valida resolucion
			if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
				throw new ApplicationException(string.Format("La Resolución {0} no es valida", documento.NumeroResolucion));

			//valida número de la Factura este entre los rangos
			if (documento.Documento < resolucion.IntRangoInicial && documento.Documento > resolucion.IntRangoFinal)
				throw new ApplicationException(string.Format("El Número de la Factura {0} no es valida según Resolución", documento.Documento));

			if (!resolucion.StrPrefijo.Equals(documento.Prefijo))
				throw new ApplicationException(string.Format("El prefijo {0} no es valido según Resolución", documento.Prefijo));

			//Valida que la fecha este en los terminos
			if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
				throw new ApplicationException(string.Format("La fecha {0} no esta dentro los terminos.", documento.Fecha));


			if (documento.FechaVence.Date < documento.Fecha.Date)
				throw new ApplicationException(string.Format("La fecha {0} debe ser mayor o igual a la generacion del documento", documento.FechaVence));

			//Valida que no este vacio y Formato
			if (string.IsNullOrEmpty(documento.Moneda))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

			if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosObligado, "Obligado");

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

			//Valida totales del objeto
			ValidarTotales(documento, null, null, TipoDocumento.Factura);

			return documento;
		}


		/// <summary>
		/// Validación del Objeto Nota Credito
		/// </summary>
		/// <param name="documento">Objeto NotaCredito</param>
		/// <returns></returns>
		public static NotaCredito ValidarNotaCredito(NotaCredito documento, TblEmpresasResoluciones resolucion)
		{
			// valida objeto recibido
			if (documento == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Nota Crédito"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.CodigoRegistro))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.DataKey))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

			// validar el número del documento y validez con resolucion
			if (documento.Documento < 0)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

			//Validar que no este vacio
			if (string.IsNullOrEmpty(documento.DocumentoRef))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

			//Validar que no este vacio
			if (string.IsNullOrEmpty(documento.CufeFactura))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CufeFactura", "string"));

			//valida resolucion
			if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
				throw new ApplicationException(string.Format("La Resolución {0} no es valida", documento.NumeroResolucion));

			//Validar que no este vacia la fecha
			if (documento.Fecha == null)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Fecha", "DateTime"));

			//Valida que no este vacio el concepto
			if (documento.Concepto == null)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Concepto", "string"));

			//Valida que la fecha este en los terminos
			if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
				throw new ApplicationException(string.Format("La fecha {0} no esta dentro los terminos.", documento.Fecha));

			//Valida que no este vacio y Formato
			if (string.IsNullOrEmpty(documento.Moneda))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

			if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosObligado, "Obligado");

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

			//Valida totales del objeto
			ValidarTotales(null, documento, null, TipoDocumento.NotaCredito);

			return documento;
		}

		/// <summary>
		/// Validacion del objeto tercero
		/// </summary>
		/// <param name="tercero">Objeto</param>
		/// <param name="tipo">Tipo de Tercero: Adquiriente - Obligado</param>
		public static void ValidarTercero(Tercero tercero, string tipo)
		{

			if (tercero == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Tercero", tipo).Replace("de tipo", "del"));

			//valida que la identificacion no contenga contener letras y/o caracteres especiales
			Regex isnumber = new Regex("[^0-9]");
			if (!string.IsNullOrEmpty(tercero.Identificacion))
			{
				if (isnumber.IsMatch(tercero.Identificacion))
					throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener letras y/o caracteres especiales", "Identificacion", tipo));
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", tipo).Replace("de tipo", "del"));

			if ((tercero.IdentificacionDv < 0) || (tercero.IdentificacionDv > 9))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "IdentificacionDv", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Ciudad))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Ciudad", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Departamento))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Departamento", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Direccion))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Direccion", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Telefono))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Telefono", tipo).Replace("de tipo", "del"));

			Regex ismail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
			if (!ismail.IsMatch(tercero.Email))
				throw new ArgumentException(string.Format("El parámetro {0} del {1} no esta bien formado", "Email", tipo));

			if (string.IsNullOrEmpty(tercero.CodigoPais))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoPais", tipo).Replace("de tipo", "del"));

			if (!ConfiguracionRegional.ValidarCodigoPais(tercero.CodigoPais))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 3166-1 alfa-2 en el {2} ", "CodigoPais", tercero.CodigoPais, tipo));

			if (string.IsNullOrEmpty(tercero.RazonSocial))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.RazonSocial))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

			if (tercero.TipoIdentificacion == 13)
			{
				if (string.IsNullOrEmpty(tercero.PrimerApellido))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerApellido", tipo).Replace("de tipo", "del"));

				if (string.IsNullOrEmpty(tercero.PrimerNombre))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerNombre", tipo).Replace("de tipo", "del"));

			}
		}

		/// <summary>
		/// Valida los totales del objeto
		/// </summary>
		/// <param name="documento_fac">Documento Factura</param>
		/// <param name="documento_nc">Documento Nota Credito</param>
		/// <param name="documento_nd">Documento Nota Debito</param>
		/// <param name="tipo_doc">Tipo de Documento enviado</param>
		public static void ValidarTotales(Factura documento_fac, NotaCredito documento_nc, NotaDebito documento_nd, TipoDocumento tipo_doc)
		{

			var documento = (dynamic)null;

			if (tipo_doc == TipoDocumento.Factura)
				documento = documento_fac;
			else if (tipo_doc == TipoDocumento.NotaCredito)
				documento = documento_nc;
			else if (tipo_doc == TipoDocumento.NotaDebito)
				documento = documento_nd;

			if (documento != null)
			{

				ValidarDetalleDocumento(documento.DocumentoDetalles);

				Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

				//Valida el Iva 
				if (documento.ValorIva == 0)
				{
					documento.ValorIva = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.ValorIva).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Iva {0} del encabezado no esta bien formado", documento.ValorIva));

				//Valida el Descuento 
				if (documento.ValorDescuento == 0)
				{
					documento.ValorDescuento = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.ValorDescuento).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Descuento {0} del encabezado no esta bien formado", documento.ValorDescuento));

				//Valida el Subtotal 
				if (documento.ValorSubtotal == 0)
				{
					documento.ValorSubtotal = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.ValorSubtotal).Replace(",", ".")))
					throw new ApplicationException(string.Format("El subtotal {0} del encabezado no esta bien formado", documento.ValorSubtotal));

				//Valida el Impuesto al consumo 
				if (documento.ValorImpuestoConsumo == 0)
				{
					documento.ValorImpuestoConsumo = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.ValorImpuestoConsumo).Replace(",", ".")))
					throw new ApplicationException(string.Format("El Impuesto al Consumo {0} del encabezado no esta bien formado", documento.ValorImpuestoConsumo));

				//Valida la Retencion en la fuente
				if (documento.ValorReteFuente == 0)
				{
					documento.ValorReteFuente = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.ValorReteFuente).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor ReteFuente {0} del encabezado no esta bien formado", documento.ValorReteFuente));

				//Valida el ReteIca 
				if (documento.ValorReteIca == 0)
				{
					documento.ValorReteIca = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.ValorReteIca).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor ReteIca {0} del encabezado no esta bien formado", documento.ValorReteIca));

				//Calculo del total con los campos enviados en el objeto
				if (documento.Total == 0)
				{
					documento.Total = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(documento.Total).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Total {0} no esta bien formado", documento.Total));

				//Suma de las retenciones del documento
				decimal retencion_doc = (documento.ValorReteFuente + documento.ValorReteIca + documento.ValorReteIva);

				//Validacion del Neto calculado con el que es enviado en el documento
				if (documento.Neto == 0)
				{
					documento.Neto = Convert.ToDecimal(0.00M);
				}
				if (isnumber.IsMatch(Convert.ToString(documento.Neto).Replace(",", ".")))
				{
					if ((documento.Total - retencion_doc) != documento.Neto)
						throw new ApplicationException(string.Format("El valor Neto {0} no es correcto.", documento.Neto));
				}
				else
				{
					throw new ApplicationException(string.Format("El valor Neto {0} no esta bien formado", documento.Neto));
				}

				//Validacion del ReteIva calculado con el que es enviado en el documento
				if (documento.ValorReteIva == 0)
				{
					documento.ValorReteIva = Convert.ToDecimal(0.00M);
				}

				if (!isnumber.IsMatch(Convert.ToString(documento.ValorReteIva).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor ReteIva {0} no esta bien formado", documento.ValorReteIva));
				
			}
		}

		/// <summary>
		/// Valida los totales enviados en el detalle
		/// </summary>
		/// <param name="documentoDetalle"></param>
		/// <returns></returns>
		public static DocumentoDetalle ValidarDetalleDocumento(List<DocumentoDetalle> documentoDetalle)
		{

			DocumentoDetalle retorno = new DocumentoDetalle();

			decimal Iva_total = 0;
			decimal Desc_total = 0;
			decimal Subtotal = 0;
			decimal Impcon = 0;
			decimal RetIca = 0;
			decimal ReteFte = 0;

			if (documentoDetalle == null || !documentoDetalle.Any())
				throw new Exception("El detalle del documento es inválido.");

			foreach (DocumentoDetalle Docdet in documentoDetalle)
			{

				//Validacion del valor unitario
				Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

				if (Docdet.ValorUnitario == 0)
				{
					Docdet.ValorUnitario = 0.00M;
				}
				if (!isnumber.IsMatch(Convert.ToString(Docdet.ValorUnitario).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Unitario {0} no esta bien formado", Docdet.ValorUnitario));

				if (Docdet.DescuentoPorcentaje > 1 || Docdet.DescuentoPorcentaje < 0)
					throw new ApplicationException(string.Format("El porcentaje Descuento {0} no es correcto", Docdet.DescuentoValor));

				if (Docdet.DescuentoValor == 0)
				{
					Docdet.DescuentoValor = Convert.ToDecimal(0.00M);
				}

				//Validacion del valor IVA
				if (Docdet.IvaValor == 0)
				{
					Docdet.IvaValor = Convert.ToDecimal(0.00M);
				}
				if (!isnumber.IsMatch(Convert.ToString(Docdet.IvaValor).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor Iva {0} del detalle no esta bien formado", Docdet.IvaValor));

				//Validacion del Valor Subtotal
				if (Docdet.ValorSubtotal == 0)
				{
					Docdet.ValorSubtotal = Convert.ToDecimal(0.00M);
				}
                
                if (!isnumber.IsMatch(Convert.ToString(Docdet.ValorSubtotal).Replace(",", ".")))
					throw new ApplicationException(string.Format("El subtotal {0} del detalle no esta bien formado", Docdet.ValorSubtotal));
				
				//Validacion del Valor del Impuesto al Consumo
				if (Docdet.ValorImpuestoConsumo == 0)
				{
					Docdet.ValorImpuestoConsumo = 0.00M;
				}
				if (!isnumber.IsMatch(Convert.ToString(Docdet.ValorImpuestoConsumo).Replace(",", ".")))
					throw new ApplicationException(string.Format("El Impuesto al Consumo {0} del detalle no esta bien formado", Docdet.ValorImpuestoConsumo));


				//Validacion del Valor del ReteICA
				if (Docdet.ReteIcaValor == 0)
				{
					Docdet.ReteIcaValor = 0.00M;
				}
				if (!isnumber.IsMatch(Convert.ToString(Docdet.ReteIcaValor).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor ReteIca {0} del detalle no esta bien formado", Docdet.ReteIcaValor));

				//Validacion del Valor del ReteFte
				if (Docdet.ReteFuenteValor == 0)
				{
					Docdet.ReteFuenteValor = 0.00M;
				}
				if (!isnumber.IsMatch(Convert.ToString(Docdet.ReteFuenteValor).Replace(",", ".")))
					throw new ApplicationException(string.Format("El valor ReteFuente  {0} del detalle no esta bien formado", Docdet.ReteFuenteValor));

				Iva_total += Docdet.IvaValor;
				Desc_total += Docdet.DescuentoValor;
				Subtotal += Docdet.ValorSubtotal;
				Impcon += Docdet.ValorImpuestoConsumo;
				RetIca += Docdet.ReteIcaValor;
				ReteFte += Docdet.ReteFuenteValor;

			}

			retorno.IvaValor = Iva_total;
			retorno.DescuentoValor = Desc_total;
			retorno.ValorSubtotal = Subtotal;
			retorno.ValorImpuestoConsumo = Impcon;
			retorno.ReteIcaValor = RetIca;
			retorno.ReteFuenteValor = ReteFte;
			return retorno;
		}



	}
}


