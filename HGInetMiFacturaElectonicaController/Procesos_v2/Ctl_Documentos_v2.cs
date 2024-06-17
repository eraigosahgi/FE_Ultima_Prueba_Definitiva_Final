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
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Error;
using HGInetMiFacturaElectonicaController.PagosElectronicos;

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
					Identificacion = (tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) ? documento_obj.DatosTrabajador.Identificacion : documento_obj.DatosAdquiriente.Identificacion,
					IdProceso = ProcesoEstado.Recepcion.GetHashCode(),
					MotivoRechazo = "",
					NumeroResolucion = (tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) ? string.Empty : documento_obj.NumeroResolucion,
					Prefijo = documento_obj.Prefijo,
					ProcesoFinalizado = 0,
					UrlPdf = "",
					UrlXmlUbl = "",
					UrlXmlAttachUbl = "",
					IdPeticion = id_peticion,
					IdentificacionObligado = (tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) ? documento_obj.DatosEmpleador.Identificacion : documento_obj.DatosObligado.Identificacion,
					UrlAuditoria = string.Format("{0}{1}", datos_plataforma.RutaPublica, Constantes.PaginaConsultaAuditoriaDoc.Replace("{id_seguridad_doc}", id_radicado.ToString())),
					IdVersionDian = empresa.IntVersionDian,
					IdAmbiente = (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode()) ? 2 : 1,
				};

				try
				{

					//Si el documento es nomina no se hace validaciones por parte de la plataforma y se ajustan segun validaciones que reporta la DIAN.
					//if (tipo_doc < TipoDocumento.AcuseRecibo)
					//{

					//}

					// valida la información del documento
					respuesta = Procesos.Ctl_Documentos.Validar(documento_obj, tipo_doc, resolucion, ref respuesta, empresa);
					if (respuesta.Error != null && documento_existente == true)
					{
						//Se actualiza el estado del documento en BD para que lo envien de nuevo
						documento_ex.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
						Ctl_Documento num_doc = new Ctl_Documento();
						documento_ex = num_doc.Actualizar(documento_ex);
						respuesta.IdProceso = documento_ex.IntIdEstado;
						respuesta.IdEstado = documento_ex.IdCategoriaEstado;
					}
					Procesos.Ctl_Documentos.ValidarRespuesta(respuesta);

					//Validacion si es un documento de nomina y si se esta validando el objeto enviado
					bool continuar_proceso = true;

					//Cambiar validacion para produccion.
					if ((tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) && (empresa.IntHabilitacionNomina == null || empresa.IntHabilitacionNomina == Habilitacion.Valida_Objeto.GetHashCode()))
						continuar_proceso = false;


					if (empresa.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode() && continuar_proceso == true)
					{

						//Guarda la id de la Peticion con la que se esta haciendo el proceso
						documento_result.IdSeguridadPeticion = id_peticion;

						//Guarda el Id del documento generado por la plataforma
						documento_result.IdSeguridadDocumento = Guid.Parse(respuesta.IdDocumento);

						// Establece la versión de la DIAN
						documento_result.VersionDian = empresa.IntVersionDian;

						//Valida que el Proveedor Receptor enviado exista en Bd
						if (tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste)
						{
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

						if (documento_obj.DocumentoFormato != null && (documento_obj.DocumentoFormato.Codigo == -1 || documento_obj.DocumentoFormato.Codigo == 99) && (string.IsNullOrEmpty(documento_obj.Cufe)))
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error("No se encontró CUFE, generar y enviar de nuevo el documento", LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
							throw new ApplicationException("No se encontró CUFE, generar y enviar de nuevo el documento");
						}

						if (documento_obj.DocumentoFormato != null && (documento_obj.DocumentoFormato.Codigo == -1 || documento_obj.DocumentoFormato.Codigo == 99) && (string.IsNullOrEmpty(documento_obj.DocumentoFormato.ArchivoPdf)))
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error("No se encontró Formato de impresión, generar y enviar de nuevo el documento", LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
							throw new ApplicationException("No se encontró CUFE, generar y enviar de nuevo el documento");
						}

						string cadena_cufe = String.Empty;

						// genera el xml en ubl
						respuesta = UblGenerar(documento_obj, tipo_doc, resolucion, documentoBd, empresa, ref respuesta, ref documento_result, ref cadena_cufe);
						if (respuesta.Error != null && documento_existente == true)
						{
							//Se actualiza el estado del documento en BD para que lo envien de nuevo
							documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
							Ctl_Documento num_doc = new Ctl_Documento();
							documentoBd = num_doc.Actualizar(documentoBd);
							respuesta.IdProceso = documentoBd.IntIdEstado;
							respuesta.IdEstado = documentoBd.IdCategoriaEstado;
						}
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						// Establece la versión de la DIAN segun el Facturador
						documento_result.VersionDian = empresa.IntVersionDian;

						// almacena el xml en ubl
						respuesta = UblGuardar(documentoBd, ref respuesta, ref documento_result);
						if (respuesta.Error != null && documento_existente == true)
						{
							//Se actualiza el estado del documento en BD para que lo envien de nuevo
							documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
							Ctl_Documento num_doc = new Ctl_Documento();
							documentoBd = num_doc.Actualizar(documentoBd);
							respuesta.IdProceso = documentoBd.IntIdEstado;
							respuesta.IdEstado = documentoBd.IdCategoriaEstado;
						}
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						bool documento_adquisi = false;

						if (tipo_doc == TipoDocumento.Factura || tipo_doc == TipoDocumento.NotaCredito)
						{
							if (documento_obj.TipoOperacion == 3)
								documento_adquisi = true;
						}

						//Validacion de Cufe a documento_obj VS el calculado en Plataforma 
						if (documento_obj.DocumentoFormato != null && (documento_obj.DocumentoFormato.Codigo == -1) && (!string.IsNullOrEmpty(documento_obj.Cufe)) && (documento_obj.Cufe != documento_result.CUFE) && documento_adquisi == false)
						{
							//Se agrega validacion para detectar si es una nota generada de version diferente
							bool Valida_nota = true;

							if (tipo_doc == TipoDocumento.NotaCredito && documento_obj.TipoOperacion > 20)
								Valida_nota = false;

							if (tipo_doc == TipoDocumento.NotaDebito && documento_obj.TipoOperacion > 30)
								Valida_nota = false;

							if (Valida_nota == true)
							{
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Calculo del CUFE no es correcto respecto al calculo de la plataforma que lo hace con los valores: {0}. Como resultado genera el CUFE: {1}, Por Favor validar y enviar de nuevo el documento", cadena_cufe, documento_result.CUFE), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
								throw new ApplicationException(string.Format("Calculo del CUFE no es correcto respecto al calculo de la plataforma que lo hace con los valores: {0}. Como resultado genera el CUFE: {1}, Por Favor validar y enviar de nuevo el documento", cadena_cufe, documento_result.CUFE));
							}

						}
						else if (documento_obj.DocumentoFormato != null && documento_obj.DocumentoFormato.Codigo == 99 && documento_adquisi == false)
						{
							if (!string.IsNullOrEmpty(documento_obj.Cufe))
							{
								if (documento_obj.Cufe != documento_result.CUFE)
								{
									respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Calculo del CUFE no es correcto respecto al calculo de la plataforma que lo hace con los valores: {0}. Como resultado genera el CUFE: {1}, Por Favor validar y enviar de nuevo el documento", cadena_cufe, documento_result.CUFE), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
									throw new ApplicationException(string.Format("Calculo del CUFE no es correcto respecto al calculo de la plataforma que lo hace con los valores: {0}. Como resultado genera el CUFE: {1}, Por Favor validar y enviar de nuevo el documento", cadena_cufe, documento_result.CUFE));
								}
							}
							else
							{
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error("No se encontró CUFE, generar y enviar de nuevo el documento", LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
								throw new ApplicationException("No se encontró CUFE, generar y enviar de nuevo el documento");
							}

						}

						//Asignación de Cufe a documento_obj 
						if (tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste)
							documento_obj.Cufe = documento_result.CUFE;
						else
							documento_obj.Cune = documento_result.CUFE;

						//Asignacion del CUFE a la respuesta
						respuesta.Cufe = documento_result.CUFE;

						//Obtiene el Formato Generico de Nomina
						if ((tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) && documento_obj.DocumentoFormato == null)
						{
							try
							{
								Formato formato_nomina = new Formato();
								Ctl_Formatos clase_formatos = new Ctl_Formatos();
								formato_nomina.Codigo = clase_formatos.ObtenerFormatosEmpresa(Constantes.NitResolucionconPrefijo, TipoFormato.FormatoPDF.GetHashCode()).Where(x => x.IntDocTipo == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault().IntCodigoFormato;
								formato_nomina.Titulo = Enumeracion.GetDescription(tipo_doc);
								documento_obj.DocumentoFormato = new Formato();
								documento_obj.DocumentoFormato = formato_nomina;
							}
							catch (Exception)
							{

							}

						}
						else if ((tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) && documento_obj.DocumentoFormato != null)
						{
							try
							{
								Ctl_Formatos clase_formatos = new Ctl_Formatos();
								documento_obj.DocumentoFormato.Codigo = clase_formatos.ObtenerFormatosEmpresa(Constantes.NitResolucionconPrefijo, TipoFormato.FormatoPDF.GetHashCode()).Where(x => x.IntDocTipo == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault().IntCodigoFormato;
								documento_obj.DocumentoFormato.Titulo = Enumeracion.GetDescription(tipo_doc);
							}
							catch (Exception)
							{

							}
						}//Aunque envien un PDF el formato de impresion sera generado por la plataforma puesto que es un documento interno del facturador
						else if (documento_adquisi == true)
						{
							try
							{
								Ctl_Formatos clase_formatos = new Ctl_Formatos();
								if (documento_obj.DocumentoFormato == null)
								{
									documento_obj.DocumentoFormato = new Formato();
								}

								documento_obj.DocumentoFormato.Codigo = 9;//clase_formatos.ObtenerFormatosEmpresa(Constantes.NitResolucionconPrefijo, TipoFormato.FormatoPDF.GetHashCode()).Where(x => x.IntDocTipo == TipoDocumento.Factura.GetHashCode()).FirstOrDefault().IntCodigoFormato;
								if (tipo_doc == TipoDocumento.Factura)
								{
									documento_obj.DocumentoFormato.Titulo = "documento soporte en adquisiciones efectuadas a no obligados a facturar";//Enumeracion.GetDescription(tipo_doc);
									try
									{
										List<FormatoCampo> CamposPredeterminados = new List<FormatoCampo>();
										FormatoCampo formato = new FormatoCampo();
										formato.Ubicacion = "campo2";
										int meses_vigencia = Math.Abs((resolucion.DatFechaVigenciaDesde.Month - resolucion.DatFechaVigenciaHasta.Month) + 12 * (resolucion.DatFechaVigenciaDesde.Year - resolucion.DatFechaVigenciaHasta.Year));
										formato.Valor = string.Format("Resolución No {0}, Fecha: {1}, del No.{2} {3} al {4} {5}, vigencia: {6} meses", resolucion.StrNumResolucion, resolucion.DatFechaVigenciaDesde.ToString("yyyy-MM-dd"), resolucion.StrPrefijo, resolucion.IntRangoInicial, resolucion.StrPrefijo, resolucion.IntRangoFinal, meses_vigencia);
										if (documento_obj.DocumentoFormato.CamposPredeterminados == null)
										{
											CamposPredeterminados.Add(formato);
											documento_obj.DocumentoFormato.CamposPredeterminados = CamposPredeterminados;
										}
										else
										{
											documento_obj.DocumentoFormato.CamposPredeterminados.Add(formato);
										}

									}
									catch (Exception)
									{

									}
								}
								else
									documento_obj.DocumentoFormato.Titulo = "nota de ajuste del documento soporte en adquisiciones efectuadas a no obligados a expedir factura o documento equivalente";//Enumeracion.GetDescription(tipo_doc);

								if (!string.IsNullOrEmpty(documento_obj.DocumentoFormato.ArchivoPdf))
									documento_obj.DocumentoFormato.ArchivoPdf = string.Empty;
							}
							catch (Exception)
							{

							}
						}

						//Guarda o genera el Formato
						if (documento_obj.DocumentoFormato != null)
						{
							respuesta = GuardarFormato(documento_obj, documentoBd, ref respuesta, ref documento_result, empresa);
							if (respuesta.Error != null && documento_existente == true)
							{
								//Se actualiza el estado del documento en BD para que lo envien de nuevo
								documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
								Ctl_Documento num_doc = new Ctl_Documento();
								documentoBd = num_doc.Actualizar(documentoBd);
								respuesta.IdProceso = documentoBd.IntIdEstado;
								respuesta.IdEstado = documentoBd.IdCategoriaEstado;
							}
							Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlPdf);
						}


						// almacena Anexos enviados
						if ((tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste) && documento_obj.ArchivoAnexos != null)
						{
							if (empresa.IntManejaAnexos)
							{
								respuesta = GuardarAnexo(documento_obj.ArchivoAnexos, documentoBd, ref respuesta, ref documento_result);
								if (respuesta.Error != null && documento_existente == true)
								{
									//Se actualiza el estado del documento en BD para que lo envien de nuevo
									documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
									Ctl_Documento num_doc = new Ctl_Documento();
									documentoBd = num_doc.Actualizar(documentoBd);
									respuesta.IdProceso = documentoBd.IntIdEstado;
									respuesta.IdEstado = documentoBd.IdCategoriaEstado;
								}
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
								if (documento_existente == true)
								{
									//Se actualiza el estado del documento en BD para que lo envien de nuevo
									documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
									Ctl_Documento num_doc = new Ctl_Documento();
									documentoBd = num_doc.Actualizar(documentoBd);
									respuesta.IdProceso = documentoBd.IntIdEstado;
									respuesta.IdEstado = documentoBd.IdCategoriaEstado;
								}
								Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, "El Facturador Electrónico no se encuentra habilitado para el procesamiento de anexos");
							}
						}


						Ctl_Empresa empresa_config = new Ctl_Empresa();

						TblEmpresas adquirienteBd = null;

						//Validacion de Adquiriente
						try
						{

							//Obtiene la informacion del Adquiriente que se tiene en BD
							if (tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste)
								adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							else
								adquirienteBd = empresa_config.Obtener(documento_obj.DatosTrabajador.Identificacion);
							try
							{

								//Si no existe Adquiriente se crea en BD y se crea Usuario
								if (adquirienteBd == null)
								{
									empresa_config = new Ctl_Empresa();
									//Creacion del Adquiriente
									if (tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste)
										adquirienteBd = empresa_config.Crear(documento_obj.DatosAdquiriente);
									else
									{
										Tercero trabajador = empresa_config.ConvertirTrabajador(documento_obj.DatosTrabajador);
										adquirienteBd = empresa_config.Crear(trabajador);
									}
								}
								else
								{
									if (tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste)
									{
										if (!adquirienteBd.StrMailAdmin.Equals(documento_obj.DatosTrabajador.Email))
										{
											empresa_config = new Ctl_Empresa();
											adquirienteBd.StrMailAdmin = documento_obj.DatosTrabajador.Email;
											adquirienteBd = empresa_config.Actualizar(adquirienteBd);
										}
									}
								}
							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
								string msg_excepcion = Excepcion.Mensaje(excepcion);

								if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key") && !msg_excepcion.ToLowerInvariant().Contains("insertar una clave duplicada"))
									throw excepcion;
								else
									adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							}
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
							Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
							throw excepcion;
						}

						//Crea el documento en BD
						try
						{
							if (documento_existente == false)
							{
								Ctl_Documento num_doc = new Ctl_Documento();
								TblDocumentos numero_documento = num_doc.Obtener(empresa.StrIdentificacion, documentoBd.IntNumero, documentoBd.StrPrefijo, documentoBd.IntDocTipo);

								if (numero_documento == null)
								{
									documentoBd = documento_tmp.Crear(documentoBd);
									respuesta.DescuentaSaldo = true;
								}
								else
								{
									respuesta.IdEstado = CategoriaEstado.NoRecibido.GetHashCode();
									respuesta.DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido);
									respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
									respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.Validacion);
									respuesta.UrlPdf = null;
									respuesta.UrlXmlUbl = null;
									respuesta.Cufe = null;
									respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("No se puede guardar El documento '{0}' con prefijo '{1}' y el Facturador Electrónico '{2}'", documentoBd.IntNumero, documentoBd.StrPrefijo, empresa.StrIdentificacion), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);

									//Se da una pausa en proceso para que el primer proceso que hizo la insercion termine con el proceso
									System.Threading.Thread.Sleep(8000);

									Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, "No se puede guardar El documento en BD porque ya existe");
								}

								
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
							Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
							throw excepcion;
						}

						// firma el xml
						respuesta = UblFirmar(empresa, documentoBd, ref respuesta, ref documento_result);
						if (respuesta.Error != null)
						{
							//Se actualiza el estado del documento en BD para que lo envien de nuevo
							documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
							Ctl_Documento num_doc = new Ctl_Documento();
							documentoBd = num_doc.Actualizar(documentoBd);
							respuesta.IdProceso = documentoBd.IntIdEstado;
							respuesta.IdEstado = documentoBd.IdCategoriaEstado;
						}
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);


						// comprime el archivo xml firmado                        
						respuesta = UblComprimir(documentoBd, ref respuesta, ref documento_result);
						if (respuesta.Error != null)
						{
							//Se actualiza el estado del documento en BD para que lo envien de nuevo
							documentoBd.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
							Ctl_Documento num_doc = new Ctl_Documento();
							documentoBd = num_doc.Actualizar(documentoBd);
							respuesta.IdProceso = documentoBd.IntIdEstado;
							respuesta.IdEstado = documentoBd.IdCategoriaEstado;
						}
						Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, "", null, false);

						//Contingencia de la DIAN 2024-03-09 desde las 6:00 am hasta las 6:00 PM
						DateTime fecha_ini_cont = new DateTime(2024, 03, 09, 6, 0, 0);
						DateTime fecha_fin_cont = new DateTime(2024, 03, 09, 18, 0, 0);

						//Se valida que no sea un documento para probar la respuesta de los servicios
						if (documentoBd.IntTipoOperacion != 50 || documento_obj.TipoOperacion != 50)
						{
							// envía el archivo zip con el xml firmado a la DIAN
							HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documentoBd, empresa, ref respuesta, ref documento_result, resolucion.StrIdSetDian);
							Procesos.Ctl_Documentos.ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");

							//Valida estado del documento en la Plataforma de la DIAN
							if (acuse.Response == 200)
								respuesta = Consultar(documentoBd, empresa, ref respuesta, acuse.KeyV2);

							//Contingencia DIAN
							if (Fecha.GetFecha() >= fecha_ini_cont && Fecha.GetFecha() < fecha_fin_cont)
							{
								respuesta.EstadoDian.EstadoDocumento = EstadoDocumentoDian.Aceptado.GetHashCode();
							}

							// envía el mail de documentos al adquiriente
							if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
							{
								//Crea el attached para poder ser enviado en el correo con los demas archivos menos en nomina.
								bool attached = false;
								if (tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste && documento_obj.TipoOperacion != 3)
								{
									try
									{
										attached = Ctl_Documento.ConvertirAttachedDoc(documento_obj, documentoBd, empresa);
									}
									catch (Exception excepcion)
									{
										Ctl_Alertas alerta = new Ctl_Alertas();
										alerta.Alertas(empresa.StrIdentificacion, string.Format("{0}{1}", documentoBd.StrPrefijo, documentoBd.IntNumero), LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(excepcion.Message), 1, false);
									}
								}

								if (attached == true)
								{
									// url pública del xml - Attached
									string nombre_archivo = HGInetUBLv2_1.NombramientoArchivo.ObtenerXml(documentoBd.IntNumero.ToString(), empresa.StrIdentificacion, TipoDocumento.Attached, documentoBd.StrPrefijo);
									string url_ppal = string.Format("{0}/{1}/{2}", datos_plataforma.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
									respuesta.UrlXmlAttachUbl = string.Format(@"{0}/{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, nombre_archivo);

								}

								bool enviar_correo = true;

								//Se hace validacion si es documento de nomina y no tiene habilitado el envio de correo
								if ((tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) && empresa.IntEnvioNominaMail == false)
									enviar_correo = false;

								if (tipo_doc != TipoDocumento.Nomina && tipo_doc != TipoDocumento.NominaAjuste && documento_obj.TipoOperacion == 3)
								{
									enviar_correo = false;
								}

								//Validacion para documentos diferentes de Nomina que no tengan correo electronico el adquiriente.
								if (enviar_correo == true && (tipo_doc == TipoDocumento.Factura || tipo_doc == TipoDocumento.NotaCredito || tipo_doc == TipoDocumento.NotaDebito))
								{
									if (string.IsNullOrWhiteSpace(documento_obj.DatosAdquiriente.Email) || documento_obj.DatosAdquiriente.Identificacion == "222222222222")
										enviar_correo = false;
								}


								if (((documentoBd.StrProveedorReceptor == null) || documentoBd.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo)) && (enviar_correo == true))
								{
									//Registro el documento en la tabla correo para gestionarlo
									try
									{
										Ctl_ProcesosCorreos proceso_correo = new Ctl_ProcesosCorreos();
										proceso_correo.Crear(documentoBd.StrIdSeguridad);
									}
									catch (Exception)
									{ }

									//Envio de correo
									respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result);

									if (empresa.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo))
									{
										Ctl_Log.Guardar(new ApplicationException("Envio Correo"), LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Sincronizacion, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.creacion);
									}



									//Se registra el valor de la nota como pago si la empresa maneja pagos electronicos para que afecte el saldo de la factura a pagar
									if (tipo_doc == TipoDocumento.NotaCredito && empresa.IntManejaPagoE == true)
									{
										Ctl_PagosElectronicos pagos = new Ctl_PagosElectronicos();
										Task pago = pagos.GenerarNotaPago(documento_obj.CufeFactura, documentoBd);
									}

									//Envio de SMS
									if (empresa.IntEnvioSms == true && documento_obj.EnvioSms == 1)
									{
										//a los telefonos se le deben agregar el 57 y debe tener 10 digitos y comenzar con 3
										//El nombre de la compañia se toma de la empresa creada en plataforma o desde el objeto del obligado
										//para informacion un correo o un numero telefonico
										//Si es numero telefonico este ya debe llegar con el indicativo 
										List<string> celulares = new List<string>();//documento_obj.DatosAdquiriente.Telefono.Split(',').ToList();
										string num_tel = documento_obj.DatosAdquiriente.Telefono;

										if (num_tel.Substring(0, 1) == "3" && num_tel.Length == 10)
										{
											celulares.Add(documento_obj.DatosAdquiriente.Telefono);

											string mensaje_sms = string.Format("{0} le acaba de generar un Documento Electrónico {1}{2} . Cualquier inquietud al {3}", empresa.StrRazonSocial, documentoBd.StrPrefijo, documentoBd.IntNumero, documento_obj.DatosObligado.Telefono);

											Ctl_Sms.Enviar(mensaje_sms, documentoBd.StrIdSeguridad.ToString(), celulares);
										}
									}

								}
								else if (enviar_correo == false)
								{
									//Se actualiza respuesta de envio solo cuando es la nomina y la empresa no esta habilitada	
									respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.Finalizacion);
									respuesta.FechaUltimoProceso = Fecha.GetFecha();
									respuesta.IdProceso = ProcesoEstado.Finalizacion.GetHashCode();

									//Actualiza Documento en Base de Datos
									documentoBd.DatFechaActualizaEstado = Fecha.GetFecha();
									documentoBd.IntIdEstado = (short)respuesta.IdProceso;

									//Actualizo el estado del documento para enviar al proveedor receptor
									documento_tmp = new Ctl_Documento();
									documento_tmp.Actualizar(documentoBd);

									//Actualiza la categoria con el nuevo estado
									respuesta.IdEstado = documentoBd.IdCategoriaEstado;
									respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));

									try
									{
										ValidarRespuesta(respuesta, respuesta.UrlPdf);

										//int estado_doc = Ctl_Documento.ObtenerCategoria(documentoBd.IntIdEstado);
										//string mensaje = (string.IsNullOrEmpty(respuesta.IdPeticion.ToString()) ? "Reenvio de Documento" : "Proceso Completo");
										// //clase_auditoria.Crear(documento.StrIdSeguridad, Guid.Empty, empresa_obligado.StrIdentificacion, proceso, TipoRegistro.Proceso, procedencia, usuario, "No fue posible el envio del documento, favor validar con el Adquiriente ó hacer el reenvío del documento desde nuestra Plataforma ", string.Format("{0}", excepcion.InnerException), documento.StrPrefijo, Convert.ToString(documento.IntNumero), estado_doc);
										//clase_auditoria.Crear(documentoBd.StrIdSeguridad, Guid.Empty, empresa.StrIdentificacion, ProcesoEstado.Finalizacion, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, mensaje, string.Empty, null, documentoBd.StrPrefijo, Convert.ToString(documentoBd.IntNumero), estado_doc);
									}
									catch (Exception) { }
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
							else if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode() || respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Recibido.GetHashCode())
							{
								#region Proceso anterior V1
								//respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result, true);
								//documento_tmp = new Ctl_Documento();
								//documentoBd.IntEnvioMail = true;
								//documento_tmp.Actualizar(documentoBd);
								//Procesos.Ctl_Documentos.ValidarRespuesta(respuesta);
								#endregion

								//Se actualiza respuesta
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error(respuesta.EstadoDian.Descripcion, LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.ProcesoPausadoPlataformaDian);
								respuesta.FechaUltimoProceso = Fecha.GetFecha();
								respuesta.IdProceso = ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();

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

							//****Contingencia de la DIAN 2024-03-09 
							if (Fecha.GetFecha() >= fecha_ini_cont && Fecha.GetFecha() < fecha_fin_cont)
							{
								//Se actualiza respuesta
								//LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista("Nos permitimos informar que el 09 de marzo de 2024, a partir de las 06:00 am y hasta las 6:00 pm, se realizará una ventana de mantenimiento en el Sistema de Facturación Electrónica DIAN, por lo que durante este tiempo no estará disponible este servicio informático,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma unas horas despues pasada la contingencia de la DIAN").ToArray();
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error("Nos permitimos informar que el 09 de marzo de 2024, a partir de las 06:00 am y hasta las 6:00 pm, se realizará una ventana de mantenimiento en el Sistema de Facturación Electrónica DIAN, por lo que durante este tiempo no estará disponible este servicio informático,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma unas horas despues pasada la contingencia de la DIAN", LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.ProcesoPausadoPlataformaDian);
								respuesta.FechaUltimoProceso = Fecha.GetFecha();
								respuesta.IdProceso = ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();
								respuesta.EstadoDian.EstadoDocumento = EstadoDocumentoDian.Pendiente.GetHashCode();

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
						else
						{
							//Se actualiza respuesta indicando que se completa el proceso
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error("Respuesta efectiva de los servicios, proceso completo", LibreriaGlobalHGInet.Error.CodigoError.OK);
							respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PrevalidacionErrorPlataforma);
							respuesta.FechaUltimoProceso = Fecha.GetFecha();
							respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();

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
					else
					{
						if (respuesta.Error == null)
						{
							respuesta.Error = new Error();
							respuesta.Error.Codigo = CodigoError.OK;
						}

						respuesta.Error.Mensaje = "Objeto cumple con la estructura solicitada y supera validaciones basicas";

					}


				}
				catch (Exception excepcion)
				{
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
					// no se controla excepción
				}

				if (empresa.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo))
				{
					Ctl_Log.Guardar(new ApplicationException("Retorna Respueta del proceso interno"), LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Servicio, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Sincronizacion, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.creacion);
				}

				return respuesta;
			}
			throw new ArgumentException("No se recibieron datos para realizar el proceso");
		}


	}
}
