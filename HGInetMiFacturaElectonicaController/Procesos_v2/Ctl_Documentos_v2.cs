using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Realiza el proceso en la plataforma para un documento
		/// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN - 6. Envío Adquiriente
		/// </summary>
		/// <param name="id_peticion">id único de identificación de la plataforma</param>
		/// <param name="documento">datos del documento</param>
		/// <param name="tipo_doc">tipo de documento a procesar</param>
		/// <param name="resolucion">resolución del documento</param>
		/// <param name="empresa">facturador electrónico del documento</param>
		/// <returns>resultado del proceso</returns>
		public static DocumentoRespuesta Procesar_v2(Guid id_peticion, Guid id_radicado, object documento, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, TblEmpresas empresa, TblDocumentos documento_ex)
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();

			string numero_resolucion = string.Empty;
			string prefijo = string.Empty;

			bool documento_existente = false;

			if (documento_ex.StrIdPlanTransaccion != null)
				documento_existente = true;


			var documento_obj = (dynamic)null;
			documento_obj = documento;


			if (documento_obj != null)
			{

				DateTime fecha_actual = Fecha.GetFecha();

				FacturaE_Documento documento_result = new FacturaE_Documento();

				var datos_plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				DocumentoRespuesta respuesta = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					IdEstadoEnvioMail = 0,
					CodigoRegistro = documento_obj.CodigoRegistro,
					Cufe = "",
					DescripcionProceso = "Recepción - Información del documento.",
					DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
					DocumentoTipo = tipo_doc.GetHashCode(),
					Documento = documento_obj.Documento,
					Error = null,
					FechaRecepcion = fecha_actual,
					FechaUltimoProceso = fecha_actual,
					IdDocumento = id_radicado.ToString(),
					Identificacion = documento_obj.DatosAdquiriente.Identificacion,
					IdProceso = ProcesoEstado.Recepcion.GetHashCode(),
					MotivoRechazo = "",
					NumeroResolucion = documento_obj.NumeroResolucion,
					Prefijo = documento_obj.Prefijo,
					ProcesoFinalizado = 0,
					UrlPdf = "",
					UrlXmlUbl = "",
					IdPeticion = id_peticion,
					IdentificacionObligado = documento_obj.DatosObligado.Identificacion,
					UrlAuditoria = string.Format("{0}{1}", datos_plataforma.RutaPublica, Constantes.PaginaConsultaAuditoriaDoc.Replace("{id_seguridad_doc}", id_radicado.ToString())),
					IdVersionDian = empresa.IntVersionDian
				};

				try
				{




					// valida la información del documento
					respuesta = Procesos.Ctl_Documentos.Validar(documento_obj, tipo_doc, resolucion, ref respuesta, empresa);
					Procesos.Ctl_Documentos.ValidarRespuesta(respuesta);


					if (empresa.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode())
					{

						//Guarda la id de la Peticion con la que se esta haciendo el proceso
						documento_result.IdSeguridadPeticion = id_peticion;

						//Guarda el Id del documento generado por la plataforma
						documento_result.IdSeguridadDocumento = Guid.Parse(respuesta.IdDocumento);

						// Establece la versión de la DIAN
						documento_result.VersionDian = empresa.IntVersionDian;

						//Valida que el Proveedor Receptor enviado exista en Bd
						if (documento_obj.IdentificacionProveedor != null)
						{

							if (!documento_obj.IdentificacionProveedor.Equals(Constantes.NitResolucionsinPrefijo))
							{
								TblConfiguracionInteroperabilidad proveedor_receptor = new TblConfiguracionInteroperabilidad();

								Ctl_ConfiguracionInteroperabilidad proveedor = new Ctl_ConfiguracionInteroperabilidad();

								proveedor_receptor = proveedor.Obtener(documento_obj.IdentificacionProveedor);

								//si no existe asigna a HGI como Proveedor Receptor
								if (proveedor_receptor == null)
								{
									documento_obj.IdentificacionProveedor = null;
								}
							}
						}

						Ctl_Documento documento_tmp = new Ctl_Documento();

						//convierte documento para BD
						TblDocumentos documentoBd = Ctl_Documento.Convertir(respuesta, documento_obj, resolucion, empresa, tipo_doc);
						documentoBd.StrIdPlanTransaccion = documento_obj.IdPlan;

						//Valida que si el documento existe continue con el Idseguridad y Plan que se registro inicialmente.
						if (documento_existente == true)
						{
							documentoBd.StrIdSeguridad = documento_ex.StrIdSeguridad;
							documentoBd.StrIdPlanTransaccion = documento_ex.StrIdPlanTransaccion;
						}


						// genera el xml en ubl
						respuesta = UblGenerar(documento_obj, tipo_doc, resolucion, documentoBd, empresa, ref respuesta, ref documento_result);
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						// Establece la versión de la DIAN segun el Facturador
						documento_result.VersionDian = empresa.IntVersionDian;

						// almacena el xml en ubl
						respuesta = UblGuardar(documentoBd, ref respuesta, ref documento_result);
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						//Asignación de Cufe a documento_obj 
						documento_obj.Cufe = documento_result.CUFE;

						//Asignacion del CUFE a la respuesta
						respuesta.Cufe = documento_result.CUFE;

						// almacena Formato
						respuesta = GuardarFormato(documento_obj, documentoBd, ref respuesta, ref documento_result, empresa);
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlPdf);

						// almacena Anexos enviados
						if (documento_obj.ArchivoAnexos != null)
						{
							if (empresa.IntManejaAnexos)
							{
								respuesta = GuardarAnexo(documento_obj.ArchivoAnexos, documentoBd, ref respuesta, ref documento_result);
								Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlAnexo);
							}
							else
							{
								respuesta.IdEstado = CategoriaEstado.NoRecibido.GetHashCode();
								respuesta.DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido);
								respuesta.IdProceso = ProcesoEstado.Validacion.GetHashCode();
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.Validacion);
								respuesta.IdDocumento = null;
								respuesta.UrlPdf = null;
								respuesta.UrlXmlUbl = null;
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("El Facturador Electrónico {0} no se encuentra habilitado para el procesamiento de anexos", documentoBd.StrEmpresaFacturador), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
								Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, "El Facturador Electrónico no se encuentra habilitado para el procesamiento de anexos");
							}
						}


						Ctl_Empresa empresa_config = new Ctl_Empresa();

						TblEmpresas adquirienteBd = null;

						//Validacion de Adquiriente
						try
						{

							//Obtiene la informacion del Adquiriente que se tiene en BD
							adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							try
							{

								//Si no existe Adquiriente se crea en BD y se crea Usuario
								if (adquirienteBd == null)
								{
									empresa_config = new Ctl_Empresa();
									//Creacion del Adquiriente
									adquirienteBd = empresa_config.Crear(documento_obj.DatosAdquiriente);

								}
							}
							catch (Exception excepcion)
							{
								string msg_excepcion = Excepcion.Mensaje(excepcion);

								if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key"))
									throw excepcion;
								else
									adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							}
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
							LogExcepcion.Guardar(excepcion);
							throw excepcion;
						}

						//Crea el documento en BD
						try
						{
							if (documento_existente == false)
							{
								documentoBd = documento_tmp.Crear(documentoBd);
								respuesta.DescuentaSaldo = true;
							}
							else
							{
								documentoBd = documento_tmp.Actualizar(documentoBd);
							}

							documentoBd.TblEmpresasResoluciones = resolucion;

							respuesta.IdPlan = documentoBd.StrIdPlanTransaccion.Value;
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al guardar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
							LogExcepcion.Guardar(excepcion);
							throw excepcion;
						}

						// firma el xml
						respuesta = UblFirmar(documentoBd, ref respuesta, ref documento_result);
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);


						// comprime el archivo xml firmado                        
						respuesta = UblComprimir(documentoBd, ref respuesta, ref documento_result);
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, "", null, false);

						// envía el archivo zip con el xml firmado a la DIAN
						HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documentoBd, empresa, ref respuesta, ref documento_result);
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");

						//Valida estado del documento en la Plataforma de la DIAN
						respuesta = Consultar(documentoBd, empresa, ref respuesta, acuse.KeyV2);


						// envía el mail de documentos al adquiriente
						if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
						{

							if ((documentoBd.StrProveedorReceptor == null) || documentoBd.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo))
							{
								respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result);

							}
							else
							{
								//Se actualiza respuesta	
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PendienteEnvioProveedorDoc);
								respuesta.FechaUltimoProceso = Fecha.GetFecha();
								respuesta.IdProceso = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();

								//Actualiza Documento en Base de Datos
								documentoBd.DatFechaActualizaEstado = Fecha.GetFecha();
								documentoBd.IntIdEstado = (short)respuesta.IdProceso;

								//Actualizo el estado del documento para enviar al proveedor receptor
								documento_tmp = new Ctl_Documento();
								documento_tmp.Actualizar(documentoBd);

								//Actualiza la categoria con el nuevo estado
								respuesta.IdEstado = documentoBd.IdCategoriaEstado;
								respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
							}
						}
						else if ((respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode() || respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Recibido.GetHashCode()) && documentoBd.IntEnvioMail == null)
						{
							respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result, true);
							documento_tmp = new Ctl_Documento();
							documentoBd.IntEnvioMail = true;
							documento_tmp.Actualizar(documentoBd);
							//Procesos.Ctl_Documentos.ValidarRespuesta(respuesta);
						}

					}


				}
				catch (Exception excepcion)
				{
					LogExcepcion.Guardar(excepcion);
					// no se controla excepción
				}

				return respuesta;
			}
			throw new ArgumentException("No se recibieron datos para realizar el proceso");
		}


	}
}
