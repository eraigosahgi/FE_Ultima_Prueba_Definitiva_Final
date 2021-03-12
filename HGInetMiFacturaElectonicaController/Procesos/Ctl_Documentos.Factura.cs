using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ModeloServicio.Documentos;
using LibreriaGlobalHGInet.Error;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;
using ListaTipoMoneda = HGInetUBLv2_1.DianListas.ListaTipoMoneda;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Procesa una lista de documentos tipo Factura
		/// </summary>
		/// <param name="documentos">documentos tipo Factura</param>
		/// <returns>Objeto tipo respuesta</returns>
		public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
		{

			Ctl_PlanesTransacciones Planestransacciones = new Ctl_PlanesTransacciones();
			List<ObjPlanEnProceso> ListaPlanes = new List<ObjPlanEnProceso>();
			List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

			ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
			string proceso_txt = Enumeracion.GetDescription(proceso_actual);
			CategoriaEstado estado = Enumeracion.GetEnumObjectByValue<CategoriaEstado>(Ctl_Documento.ObtenerCategoria(proceso_actual.GetHashCode()));
			DateTime fecha_actual = Fecha.GetFecha();

			TblEmpresas facturador_electronico = null;

			// genera un id único de la plataforma
			Guid id_peticion = Guid.NewGuid();

			try
			{
				Ctl_Empresa Peticion = new Ctl_Empresa();

				//Válida que la key sea correcta.
				facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

				if (!facturador_electronico.IntObligado)
					throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

				List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
				if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{

					if (facturador_electronico.IntVersionDian == 1)
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

						//Valida que Resolucion tomar, con Prefijo o sin Prefijo
						string resolucion_pruebas = string.Empty;
						string nit_resolucion = string.Empty;
						string prefijo_prueba = string.Empty;
						if (documentos.FirstOrDefault().Prefijo.Equals(string.Empty))
						{
							resolucion_pruebas = Constantes.ResolucionPruebas;
							nit_resolucion = Constantes.NitResolucionsinPrefijo;

						}
						else
						{
							resolucion_pruebas = Constantes.ResolucionPruebas;
							nit_resolucion = Constantes.NitResolucionconPrefijo;
							prefijo_prueba = Constantes.PrefijoResolucionPruebas;
						}

						lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_prueba));

						foreach (var item in documentos)
						{
							item.NumeroResolucion = resolucion_pruebas;
							item.DatosObligado = DatosObligado;
							item.Prefijo = prefijo_prueba;

						}
					}
					else
					{
						lista_resolucion = _resolucion.ObtenerResoluciones(facturador_electronico.StrIdentificacion, "*", false);
						foreach (var item in documentos)
						{
							item.DatosAdquiriente.Email = facturador_electronico.StrMailAdmin;

						}

					}
				}
				else
				{
					lista_resolucion = _resolucion.ObtenerResoluciones(facturador_electronico.StrIdentificacion, "*", false);


					List<string> resoluciones_docs = documentos.Select(_res => _res.NumeroResolucion).Distinct().ToList();

					List<string> resoluciones_bd = lista_resolucion.Select(_res => _res.StrNumResolucion).Distinct().ToList<string>();

					//Valida si hay items de una lista que no esten en otra
					List<string> resol = resoluciones_docs.Except(resoluciones_bd, StringComparer.OrdinalIgnoreCase).ToList();

					if ((resol != null || resol.Count > 0) && documentos[0].TipoOperacion != 50)
					{
						// actualiza las resoluciones de los servicios web de la DIAN en la base de datos
						lista_resolucion = new List<TblEmpresasResoluciones>();
						lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, facturador_electronico);
					}

				}


				if (lista_resolucion == null)
					throw new ApplicationException(string.Format("No se encontraron las resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));
				else if (!lista_resolucion.Any())
					throw new ApplicationException(string.Format("No se encontraron las resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));

				//Obtiene la lista de objetos de planes para trabajar(Reserva, procesar, idplan) esto puede generar una lista de objetos, ya que pueda que se requiera mas de un plan

				ListaPlanes = Planestransacciones.ObtenerPlanesActivos(facturador_electronico.StrIdentificacion, documentos.Count());

				if (ListaPlanes == null)
				{
					///Validación de alertas y notificaciones
					try
					{
						Ctl_Alertas controlador = new Ctl_Alertas();
						controlador.alertaSinSaldo(facturador_electronico.StrIdentificacion);
					}
					catch (Exception excepcion)
					{
						LogExcepcion.Guardar(excepcion);
					}
					throw new ApplicationException("No se encontró saldo disponible para procesar los documentos");
				}


				int i = 0;
				//Planes y transacciones
				foreach (var item in documentos)
				{
					if (item != null)
					{
						if (ListaPlanes[i].reservado >= ListaPlanes[i].enProceso)
						{
							i = i + 1;
						}
						item.IdPlan = Guid.Parse(ListaPlanes[i].plan.ToString());
						ListaPlanes[i].reservado = ListaPlanes[i].reservado + 1;
					}
				}
				//Planes y transacciones
				Parallel.ForEach<Factura>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion);
					respuesta.Add(item_respuesta);
				});
				
				var datos = Planestransacciones.ConciliarPlanes(ListaPlanes, respuesta);

				////Planes y transacciones
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//Valida la plataforma para envio de sms
				if (plataforma_datos.EnvioSms)
				{
					Ctl_Sms.EnviarSms(respuesta, id_peticion, facturador_electronico, documentos);
				}

			}
			catch (Exception ex)
			{
				try
				{
					var datos = Planestransacciones.ConciliarPlanes(ListaPlanes, respuesta);
				}
				catch (Exception exep)
				{
					Ctl_Log.Guardar(exep, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				}

				string mensaje = ex.Message;
				Parallel.ForEach<Factura>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
						DocumentoTipo = TipoDocumento.Factura.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(mensaje, LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, ex.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = "",
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = "",
						NumeroResolucion = item.NumeroResolucion,
						Prefijo = item.Prefijo,
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = "",
						UrlXmlUbl = "",
						IdEstado = estado.GetHashCode(),
						IdPeticion = id_peticion,
						IdentificacionObligado = (item.DatosObligado != null) ? item.DatosObligado.Identificacion : "",
						DescuentaSaldo = false,
						IdVersionDian = (facturador_electronico != null) ? facturador_electronico.IntVersionDian : 0
					};
					respuesta.Add(item_respuesta);
				});

				////Planes y transacciones
				Ctl_Log.Guardar(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, documentos.FirstOrDefault().DatosObligado.Identificacion);
				//throw new ApplicationException(ex.Message);

			}

			return respuesta;
		}

		/// <summary>
		/// Procesa un documento por paralelismo
		/// </summary>
		/// <param name="item">objeto de datos factura</param>
		/// <param name="facturador_electronico">facturador electrónico del documento</param>
		/// <param name="id_peticion">identificador de petición</param>
		/// <param name="fecha_actual">fecha actual de recepción del documento</param>
		/// <param name="lista_resolucion">resoluciones habilitadas para el facturador electrónico</param>
		/// <returns>resultado del proceso</returns>
		private static DocumentoRespuesta Procesar(Factura item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion)
		{
			DocumentoRespuesta item_respuesta = new DocumentoRespuesta() { DescuentaSaldo = false };

			Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();

			//Si el documento enviado ya existe retorna la informacion que se tiene almacenada
			bool doc_existe = false;

			//radicado del documento
			Guid id_radicado = Guid.NewGuid();

			string prefijo = item.Prefijo;
			string numero = item.Documento.ToString();

			ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
			string proceso_txt = Enumeracion.GetDescription(proceso_actual);
			CategoriaEstado estado = Enumeracion.GetEnumObjectByValue<CategoriaEstado>(Ctl_Documento.ObtenerCategoria(proceso_actual.GetHashCode()));

			string mensaje = string.Empty;

			// valida que el documento no sea nulo
			if (item == null)
			{
				mensaje = "Se encontró un documento inválido (sin datos).";

				item_respuesta = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
					DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
					DocumentoTipo = TipoDocumento.Factura.GetHashCode(),
					Documento = 0,
					Error = new LibreriaGlobalHGInet.Error.Error(mensaje, LibreriaGlobalHGInet.Error.CodigoError.VALIDACION),
					EstadoDian = null,
					FechaRecepcion = fecha_actual,
					FechaUltimoProceso = fecha_actual,
					IdProceso = proceso_actual.GetHashCode(),
					ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
					IdEstado = estado.GetHashCode(),
					IdPeticion = id_peticion,
					DescuentaSaldo = false
				};
				return item_respuesta;
			}

			TblDocumentos numero_documento = new TblDocumentos();
			Ctl_Documento num_doc = new Ctl_Documento();

			try
			{
				if (string.IsNullOrEmpty(item.NumeroResolucion))
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));

				//valida si el Documento ya existe en Base de Datos
				numero_documento = num_doc.Obtener(facturador_electronico.StrIdentificacion, item.Documento, item.Prefijo);

				TblDocumentos documento_bd = new TblDocumentos();

				if (numero_documento != null)
				{
					if (facturador_electronico.IntVersionDian == 1)
					{
						mensaje = string.Format("El documento '{0}' con prefijo '{1}' ya existe para el Facturador Electrónico '{2}'", item.Documento, prefijo, facturador_electronico.StrIdentificacion);

						item_respuesta = Ctl_Documento.Convertir(numero_documento);
						item_respuesta.IdPeticion = id_peticion;
						id_radicado = Guid.Parse(item_respuesta.IdDocumento);
						doc_existe = true;

						throw new ApplicationException(mensaje);
					}
					else
					{
						item_respuesta = Ctl_Documento.Convertir(numero_documento);
						item_respuesta.IdPeticion = id_peticion;
						id_radicado = Guid.Parse(item_respuesta.IdDocumento);
						doc_existe = true;
						if (numero_documento.IntIdEstado == ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode() || numero_documento.IntIdEstado == ProcesoEstado.PrevalidacionErrorDian.GetHashCode())
						{
							//guardo algunas de las propiedades que estan en Bd para hacer la actualizacion con lo que llega
							documento_bd = numero_documento;

							//Se actualiza el estado para evitar que lo envien de nuevo mientras se termina este proceso
							numero_documento.IntIdEstado = (short)ProcesoEstado.Recepcion.GetHashCode();
							numero_documento = num_doc.Actualizar(numero_documento);
						}
						else if (numero_documento.IntIdEstado == ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode() || numero_documento.IntIdEstado == ProcesoEstado.EnvioZip.GetHashCode())
						{
							// procesa el documento en V2
							item_respuesta = ProcesarV2(numero_documento, true);
							if (item_respuesta.Error == null)
								item_respuesta.Error = new Error();
							return item_respuesta;
						}
						else
						{
							mensaje = string.Format("El documento '{0}' con prefijo '{1}' ya existe para el Facturador Electrónico '{2}'", item.Documento, prefijo, facturador_electronico.StrIdentificacion);
							throw new ApplicationException(mensaje);
						}
					}
				}


				TblEmpresasResoluciones resolucion = null;


				//resolucion = Obtenerresolucion(lista_resolucion, item.NumeroResolucion, item.Prefijo, id_peticion, facturador_electronico);
				// filtra la resolución del documento
				try
				{
					resolucion = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion)
																			&& _resolucion_doc.StrPrefijo.Equals(item.Prefijo)
																			&& _resolucion_doc.StrNumResolucion.Equals(item.NumeroResolucion)).FirstOrDefault();
				}
				catch (Exception excepcion)
				{
					mensaje = string.Format("No se encontró la resolución '{0}' para el Facturador Electrónico '{1}'", item.NumeroResolucion, facturador_electronico.StrIdentificacion);

					throw new ApplicationException(mensaje);
				}

				if (resolucion == null)
				{
					throw new ApplicationException(string.Format("No se encontró la resolución '{0}' para el Facturador Electrónico '{1}' con prefijo '{2}'", item.NumeroResolucion, facturador_electronico.StrIdentificacion, item.Prefijo));
				}

				if (resolucion.IntVersionDian != facturador_electronico.IntVersionDian)
				{
					throw new ApplicationException("La versión de la resolución no corresponde a la versión del documento");
				}

				try
				{
					mensaje = Enumeracion.GetDescription(estado);
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception) { }

				if (facturador_electronico.IntDebug == true)
				{
					try
					{
						PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

						// ruta física de la carpeta
						string carpeta_debug = string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaDocumentosDebug);

						// valida la existencia de la carpeta
						carpeta_debug = Directorio.CrearDirectorio(carpeta_debug);

						// nombre del archivo
						string archivo_debug = string.Format(@"{0}-{1}-{2}.json", facturador_electronico.StrIdentificacion, item.Prefijo, item.Documento);

						string ruta_archivo = string.Format("{0}\\{1}", carpeta_debug, archivo_debug);

						// almacena el objeto en archivo json
						File.WriteAllText(ruta_archivo, JsonConvert.SerializeObject(item));

					}
					catch (Exception excepcion)
					{
						mensaje = string.Format("Error al guardar el objeto peticion. Detalle: {0} ", excepcion.Message);

						Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
					}

					//Se lee un archivo json y se convierte a objeto Factura para pruebas
					//Factura obj_nc = new Factura();
					//string objeto = System.IO.File.ReadAllText(@"E:\Desarrollo\jzea\Proyectos\HGInetMiFacturaElectronica\Codigo\HGInetMiFacturaElectronicaWeb\dms\Debug\811021438-SETP-990001209-.json").ToString();
					//obj_nc = JsonConvert.DeserializeObject<Factura>(objeto);
					//item = obj_nc;

				}

				if (facturador_electronico.IntVersionDian == 1)
				{
					// realiza el proceso de envío a la DIAN del documento
					item_respuesta = Procesar(id_peticion, id_radicado, item, TipoDocumento.Factura, resolucion,
						facturador_electronico);
				}
				else
				{
					// realiza el proceso de envío a la DIAN del documento en Validacion Previa V2
					item_respuesta = Procesar_v2(id_peticion, id_radicado, item, TipoDocumento.Factura, resolucion,
						facturador_electronico, documento_bd);
				}
			}
			catch (Exception excepcion)
			{
				mensaje = string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message);

				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);

				try
				{
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception ex)
				{
					Ctl_Log.Guardar(ex, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
				}


				if (!doc_existe)
				{
					item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
						DocumentoTipo = TipoDocumento.Factura.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(mensaje, LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = "",
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = "",
						NumeroResolucion = item.NumeroResolucion,
						Prefijo = item.Prefijo,
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = "",
						UrlXmlUbl = "",
						IdEstado = estado.GetHashCode(),
						IdPeticion = id_peticion,
						IdentificacionObligado = (item.DatosObligado != null) ? item.DatosObligado.Identificacion : "",
						DescuentaSaldo = false,
						IdVersionDian = facturador_electronico.IntVersionDian
					};
				}
				else
				{
					item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					//Si el estado es menor a firmado, la respuesta del estado siempre
					if (item_respuesta.IdProceso < (short)ProcesoEstado.FirmaXml.GetHashCode())
					{
						item_respuesta.IdProceso = (short)ProcesoEstado.Validacion.GetHashCode();
						item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();
					}
					if (facturador_electronico.IntVersionDian == 2)
					{
						if (numero_documento.IntIdEstado == (short)ProcesoEstado.Recepcion.GetHashCode())
							item_respuesta.IdProceso = ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();

						//Se actualiza el estado del documento en BD para que lo envien de nuevo
						numero_documento.IntIdEstado = (short)item_respuesta.IdProceso;
						numero_documento = num_doc.Actualizar(numero_documento);
					}
				}

			}
			if (item_respuesta.Error == null)
				item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
			//Si el estado es menor a firmado, la respuesta del estado siempre
			if ((item_respuesta.IdProceso < (short)ProcesoEstado.EnvioZip.GetHashCode() || (item_respuesta.IdProceso > (short)ProcesoEstado.EnvioEmailAcuse.GetHashCode() && item_respuesta.IdProceso < (short)ProcesoEstado.FinalizacionErrorDian.GetHashCode())) 
			    && (item_respuesta.IdEstado >= (short)CategoriaEstado.NoRecibido.GetHashCode() || item_respuesta.IdEstado < (short)CategoriaEstado.EnvioDian.GetHashCode()))
			{
				//Se actualiza el estado del documento en BD para que lo envien de nuevo
				numero_documento = num_doc.Obtener(facturador_electronico.StrIdentificacion, item.Documento, item.Prefijo);

				if ((numero_documento != null) && (item_respuesta.IdProceso > (short) ProcesoEstado.Recepcion.GetHashCode() || item_respuesta.IdProceso < (short) ProcesoEstado.EnvioZip.GetHashCode()))
				{
					numero_documento.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
					numero_documento = num_doc.Actualizar(numero_documento);
					if (string.IsNullOrEmpty(item_respuesta.Error.Mensaje))
						item_respuesta.Error.Mensaje = "Se presentó inconsistencia al procesar el documento, enviar de nuevo el documento";
				}

				item_respuesta.IdProceso = (short)ProcesoEstado.Validacion.GetHashCode();
				item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();

			}
			else if (item_respuesta.IdProceso == (short) ProcesoEstado.EnvioZip.GetHashCode())
			{
				item_respuesta.IdProceso = (short)ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();
				item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();
				item_respuesta.Error.Mensaje = "La plataforma de la DIAN no dio respuesta del procesamiento del documento, por favor no modificar el documento y enviarlo de nuevo";
			}

			return item_respuesta;

		}

		/// <summary>
		/// Procesa un documento por paralelismo
		/// </summary>
		/// <param name="item">objeto de datos factura</param>
		/// <param name="facturador_electronico">facturador electrónico del documento</param>
		/// <param name="id_peticion">identificador de petición</param>
		/// <param name="fecha_actual">fecha actual de recepción del documento</param>
		/// <param name="lista_resolucion">resoluciones habilitadas para el facturador electrónico</param>
		/// <returns>resultado del proceso</returns>
		private static DocumentoRespuesta Procesar_v2(Factura item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion)
		{
			DocumentoRespuesta item_respuesta = new DocumentoRespuesta() { DescuentaSaldo = false };

			return item_respuesta;
		}

		/// <summary>
		/// Validación del Objeto Factura
		/// </summary>
		/// <param name="documento">Objeto factura</param>
		/// <returns></returns>
		public static Factura Validar(Factura documento, TblEmpresasResoluciones resolucion, TblEmpresas facturador)
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

			//Inicializa la propiedad, no es un campo requerido
			if (string.IsNullOrEmpty(documento.DocumentoRef))
				documento.DocumentoRef = string.Empty;

			//Inicializa la propiedad, no es un campo requerido
			if (string.IsNullOrEmpty(documento.PedidoRef))
				documento.PedidoRef = string.Empty;

			//setea el campo y lo deja en blanco si no es de HGI
			//if (documento.DocumentoFormato.Codigo != -1 && !string.IsNullOrEmpty(documento.Cufe))
			//	documento.Cufe = string.Empty;

			//Validar que no este vacio y este vigente en los terminos.
			if (string.IsNullOrEmpty(documento.NumeroResolucion))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));

			//valida resolucion
			if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
				throw new ApplicationException(string.Format("El número de resolución {0} no es válido", documento.NumeroResolucion));

			//valida número de la Factura este entre los rangos
			if (documento.Documento < resolucion.IntRangoInicial || documento.Documento > resolucion.IntRangoFinal)
				throw new ApplicationException(string.Format("El número del documento {0} no es válido según la resolución", documento.Documento));

			//Valida que la fecha este en los terminos
			if (documento.Fecha.Date > resolucion.DatFechaVigenciaHasta || documento.Fecha.Date < resolucion.DatFechaVigenciaDesde)
				throw new ApplicationException(string.Format("El documento {0} no cumple con los términos de la Resolución. Fecha del documento: {1}", documento.Documento, documento.Fecha.Date));

			if (!resolucion.StrPrefijo.Equals(documento.Prefijo))
				throw new ApplicationException(string.Format("El prefijo '{0}' no es válido según Resolución", documento.Prefijo));

			//Valida que la fecha este en los terminos
			if (facturador.IntVersionDian == 1)
			{
				if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
					throw new ApplicationException(string.Format("La fecha de elaboración {0} no está dentro los términos.", documento.Fecha));
			}
			else
			{
				//if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-5).Date || documento.Fecha.Date > Fecha.GetFecha().Date.AddDays(10))
				//	throw new ApplicationException(string.Format("La fecha de elaboración {0} no está dentro los términos.", documento.Fecha));

				if (documento.FormaPago != 0)
				{
					ListaFormasPago list_forma = new ListaFormasPago();
					ListaItem forma = list_forma.Items.Where(d => d.Codigo.Equals(documento.FormaPago.ToString())).FirstOrDefault();
					if (forma == null)
						throw new ApplicationException(string.Format("La Forma de Pago '{0}' no es válido según Estandar DIAN", documento.FormaPago));
				}
				else
				{
					throw new ApplicationException(string.Format("La Forma de Pago '{0}' no es válido según Estandar DIAN", documento.FormaPago));
				}

				if (documento.FormaPago == 2)
				{

					ListaMediosPago list_medio = new ListaMediosPago();
					ListaItem medio = list_medio.Items.Where(d => d.Codigo.Equals(documento.TerminoPago.ToString())).FirstOrDefault();
					if (medio == null)
						throw new ApplicationException(string.Format("El Medio de Pago '{0}' no es válido según Estandar DIAN", documento.TerminoPago));
				}

				//Valida si es contingencia que llegue el documento referenciado
				if (documento.TipoOperacion.Equals(1))
				{
					if (documento.DocumentosReferencia != null)
					{
						bool contingencia = documento.DocumentosReferencia.Exists(d => d.CodigoReferencia.Equals("FTC"));
						if (contingencia == false)
							throw new ApplicationException("El tipo de documento referenciado para el documento de contingencia no es válido según Estandar DIAN");
					}
					else
					{
						throw new ApplicationException("No se encontró documento referenciado para el documento de contingencia");
					}
				}
				else if (documento.TipoOperacion.Equals(2))//Valida que si es Exportacion llegue los campos MARCA y MODELO, por el momento se llenan si no llega
				{
					if (documento.Trm != null)
					{
						if (string.IsNullOrEmpty(documento.Trm.Moneda))
							throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Trm-Moneda", "string"));

						if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Trm.Moneda))
							throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Trm-Moneda", documento.Trm.Moneda));

						if (documento.Trm.FechaTrm == DateTime.MinValue)
							throw new ArgumentException(string.Format("La {0} registrada con valor {1} no es valida", "FechaTrm", documento.Trm.FechaTrm));

						if (documento.Trm.Valor <= 0)
							throw new ApplicationException(string.Format("El campo {0} con valor {1} para informar como tasa de cambio en una exportación no es valido", "Trm-Valor", documento.Trm.Valor));
					}
					else
					{
						throw new ApplicationException("No se encontró Tasa de Cambio para el documento de exportación");
					}

					if (documento.TipoEntrega != null )
					{

						ListaTipoEntrega list_tipo_entrega = new ListaTipoEntrega();
						ListaItem entrega = list_tipo_entrega.Items.Where(d => d.Codigo.Equals(documento.TipoEntrega.CodCondicionEntrega)).FirstOrDefault();
						if (entrega == null)
							throw new ApplicationException(string.Format("El código de la Condicion de Entrega '{0}' no es válido según Estandar DIAN", documento.TipoEntrega.CodCondicionEntrega));
					}
				}

				if (documento.DocumentosReferencia != null)
				{
					foreach (ReferenciaAdicional item in documento.DocumentosReferencia)
					{
						ListaTipoReferenciaAdicional list_refAd = new ListaTipoReferenciaAdicional();
						ListaItem refad = list_refAd.Items.Where(d => d.Codigo.Equals(item.CodigoReferencia)).FirstOrDefault();
						if (refad == null)
							throw new ApplicationException(string.Format("El tipo de documento referenciado '{0}' no es válido según Estandar DIAN", item.CodigoReferencia));
					}
				}
			}

			if (documento.FechaVence.Date < documento.Fecha.Date)
				throw new ApplicationException(string.Format("La fecha de vencimiento {0} debe ser mayor o igual a la fecha de elaboración del documento {1}", documento.FechaVence, documento.Fecha));

			//Valida que no este vacio y Formato
			if (string.IsNullOrEmpty(documento.Moneda))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

			if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

			if (documento.FechaEntrega.Date >= Fecha.GetFecha().Date)
			{
				if (documento.FechaEntrega.Date < documento.Fecha.Date)
					throw new ApplicationException(string.Format("La fecha de entrega {0} debe ser mayor o igual a la fecha de elaboración del documento {1}", documento.FechaEntrega, documento.Fecha));

				if (documento.FechaEntrega.Date > Fecha.GetFecha().Date.AddDays(10))
					throw new ApplicationException(string.Format("La fecha de entrega {0} no puede ser mayor a 10 dias de la fecha de recepcion en la plataforma.", documento.FechaEntrega));

			}
			else if(documento.DatosAdquiriente.DireccionEntrega != null)
				throw new ApplicationException("La fecha de entrega es requerida cuando se informa la dirección de entrega.");

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosObligado, "Obligado", facturador);

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosAdquiriente, "Adquiriente", facturador);

			//Valida totales del objeto
			ValidarTotales(documento, null, null, TipoDocumento.Factura, facturador);

			if (facturador.IntHabilitacion > 0)
			{
				if (documento.DocumentoFormato != null)
				{
					//Valida que envien el titulo del documento y si es vacio lo llena
					if (string.IsNullOrEmpty(documento.DocumentoFormato.Titulo) || documento.DocumentoFormato.Titulo == null)
						documento.DocumentoFormato.Titulo = Enumeracion.GetDescription(TipoDocumento.Factura).ToUpper();
				}
				else
					throw new ApplicationException("No se encontró información del Formato");
			}

			//Validacion Sector Salud
			if (documento.SectorSalud != null)
			{
				if (documento.SectorSalud.CamposSector.Count == 0 || documento.SectorSalud.CamposSector.Count < 22 || documento.SectorSalud.CamposSector.Count > 21)
					throw new ApplicationException("No se encontró la cantidad correcta de información del Sector Salud, deben ser 21 items Según Resolucion 084 del ministerio de Salud");

				CampoValor valid_salud = new CampoValor();
				string valid_enum_salud = string.Empty;

				valid_salud = documento.SectorSalud.CamposSector[1];
				try
				{
					TipoIdentificacionSalud dato_iden = Enumeracion.GetValueFromAmbiente<TipoIdentificacionSalud>(valid_salud.Valor);
					valid_enum_salud = Enumeracion.GetDescription(dato_iden);
				}
				catch (Exception)
				{
					throw new ApplicationException(string.Format("El tipo de identificacion del usuario {0} no corresponde a ninguno del listado del Sector Salud.", valid_salud.Valor));
				}

				valid_salud = documento.SectorSalud.CamposSector[7];
				try
				{
					TipoUsuarioSalud dato_enum = Enumeracion.GetEnumObjectByValue<TipoUsuarioSalud>(Convert.ToInt16(valid_salud.Valor));
					valid_enum_salud = Enumeracion.GetDescription(dato_enum);
				}
				catch (Exception)
				{

					throw new ApplicationException(string.Format("El tipo de usuario {0} no corresponde a ninguno del listado del Sector Salud.", valid_salud.Valor));
				}

				valid_salud = documento.SectorSalud.CamposSector[9];
				try
				{
					CoberturaSalud dato_enum = Enumeracion.GetEnumObjectByValue<CoberturaSalud>(Convert.ToInt16(valid_salud.Valor));
					valid_enum_salud = Enumeracion.GetDescription(dato_enum);
				}
				catch (Exception)
				{

					throw new ApplicationException(string.Format("La Cobertura o Plan de beneficio {0} no corresponde a ninguno del listado del Sector Salud.", valid_salud.Valor));
				}

			}

			return documento;
		}


		/// <summary>
		/// Filtra la resolucion del documento entre listado de resoluciones de bd y actualiza listado si es necesario
		/// </summary>
		/// <param name="lista_resolucion">Resoluciones que se tienen en BD del Obligado</param>
		/// <param name="numeroResolucion">Numero de resolucion del documento enviado</param>
		/// <param name="prefijo">Prefijo del documento enviado</param>
		/// <param name="id_peticion">peticion con la que se esta haciendo el proceso</param>
		/// <param name="facturador_electronico">objeto de informacion del Obligado</param>
		/// <returns></returns>
		/*public static TblEmpresasResoluciones Obtenerresolucion(List<TblEmpresasResoluciones> lista_resolucion, string numeroResolucion, string prefijo, Guid id_peticion, TblEmpresas facturador_electronico)
		{

			TblEmpresasResoluciones resolucion = null;
			// filtra la resolución del documento
			try
			{
				resolucion = lista_resolucion.Where(_resolucion_doc =>_resolucion_doc.StrEmpresa.Equals(facturador_electronico.StrIdentificacion)
																		&& _resolucion_doc.StrPrefijo.Equals(prefijo)
																		&& _resolucion_doc.StrNumResolucion.Equals(numeroResolucion)).FirstOrDefault();
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
			}

			if (resolucion == null && facturador_electronico.IntHabilitacion == Habilitacion.Produccion.GetHashCode())
			{
				try
				{
					// actualiza las resoluciones de los servicios web de la DIAN en la base de datos
					lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, facturador_electronico);

					// filtra la resolución del documento en las resoluciones actualizadas
					resolucion = lista_resolucion.Where(_resolucion_doc =>_resolucion_doc.StrEmpresa.Equals(facturador_electronico.StrIdentificacion)
																			&& _resolucion_doc.StrPrefijo.Equals(prefijo)
																			&& _resolucion_doc.StrNumResolucion.Equals(numeroResolucion)).FirstOrDefault();
				}
				catch (Exception excepcion)
				{
					LogExcepcion.Guardar(excepcion);
				}
			}

			return resolucion;

		}*/

	}
}
