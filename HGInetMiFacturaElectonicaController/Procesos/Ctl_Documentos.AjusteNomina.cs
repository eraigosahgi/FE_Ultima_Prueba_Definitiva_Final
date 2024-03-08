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
using LibreriaGlobalHGInet.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		public static List<DocumentoRespuesta> Procesar(List<NominaAjuste> documentos)
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
				facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosEmpleador.Identificacion);

				if (!facturador_electronico.IntObligado)
					throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

				//Contingencia de la DIAN 2024-03-09 desde las 6:00 am hasta las 6:00 PM
				DateTime fecha_ini_cont = new DateTime(2024, 03, 09, 6, 0, 0);
				DateTime fecha_fin_cont = new DateTime(2024, 03, 09, 18, 0, 0);

				if (Fecha.GetFecha() >= fecha_ini_cont && Fecha.GetFecha() < fecha_fin_cont)
				{
					throw new ApplicationException("Nos permitimos informar que el 09 de marzo de 2024, a partir de las 06:00 am y hasta las 6:00 pm, se realizará una ventana de mantenimiento en el Sistema de Facturación Electrónica DIAN, por lo que durante este tiempo no estará disponible este servicio informático,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma unas horas despues pasada la contingencia de la DIAN");
				}

				List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				lista_resolucion = _resolucion.ObtenerResolucionesPorTipo(facturador_electronico.StrIdentificacion, TipoDocumento.NominaAjuste.GetHashCode());

				// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
				if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{

					string set_Id = documentos.FirstOrDefault().NumeroResolucion;

					if ( lista_resolucion.Count > 0 && !string.IsNullOrEmpty(set_Id) && !facturador_electronico.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo))
					{
						foreach (var item in lista_resolucion)
						{
							item.StrIdSetDian = set_Id;

						}
					}

					foreach (var item in documentos)
					{
						//Se valida que la nota sea de reemplazo
						if (item.DatosTrabajador != null)
							item.DatosTrabajador.Email = facturador_electronico.StrMailAdmin;

					}
				}

				//Valida que si tiene certificado digital este vigente
				if (facturador_electronico.IntCertFirma == 1)
				{
					Ctl_Documento certif = new Ctl_Documento();
					certif.ValidarCertificadoDigital(facturador_electronico);
				}

				//Obtiene la lista de objetos de planes para trabajar(Reserva, procesar, idplan) esto puede generar una lista de objetos, ya que pueda que se requiera mas de un plan
				ListaPlanes = Planestransacciones.ObtenerPlanesActivos(facturador_electronico.StrIdentificacion, documentos.Count(), TipoDocPlanes.Nomina.GetHashCode());

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
				Parallel.ForEach<NominaAjuste>(documentos, item =>
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
				Parallel.ForEach<NominaAjuste>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
						DocumentoTipo = TipoDocumento.NominaAjuste.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(mensaje, LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, ex.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = (item.DatosTrabajador != null) ? item.DatosTrabajador.Identificacion : "",
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = "",
						NumeroResolucion = string.Empty,
						Prefijo = item.Prefijo,
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = "",
						UrlXmlUbl = "",
						IdEstado = estado.GetHashCode(),
						IdPeticion = id_peticion,
						IdentificacionObligado = (item.DatosEmpleador != null) ? item.DatosEmpleador.Identificacion : "",
						DescuentaSaldo = false,
						IdVersionDian = (facturador_electronico != null) ? facturador_electronico.IntVersionDian : 0
					};
					respuesta.Add(item_respuesta);
				});

				////Planes y transacciones
				Ctl_Log.Guardar(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, documentos.FirstOrDefault().DatosEmpleador.Identificacion);
				//throw new ApplicationException(ex.Message);

			}

			return respuesta;
		}

		private static DocumentoRespuesta Procesar(NominaAjuste item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion)
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

			// valida que el documento no sea nulo
			if (item == null)
			{
				mensaje = "Se encontró un documento inválido (sin datos).";

				item_respuesta = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
					DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
					DocumentoTipo = TipoDocumento.NominaAjuste.GetHashCode(),
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

			if (item.TipoDocPred.Equals(0) || item.TipoDocPred > 2)
			{
				mensaje = "No se encontró un tipo de Documento válido al que se va afectar.";

				item_respuesta = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
					DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
					DocumentoTipo = TipoDocumento.NominaAjuste.GetHashCode(),
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

			//Obtiene el tipo de Documento que se va afectar con este ajuste
			TipoDocumento tipo_doc_pred = TipoDocumento.Nomina;

			if (item.TipoDocPred.Equals(2))
				tipo_doc_pred = TipoDocumento.NominaAjuste;

			TblDocumentos numero_documento = new TblDocumentos();
			Ctl_Documento num_doc = new Ctl_Documento();

			try
			{
				
				//valida si el Documento ya existe en Base de Datos
				numero_documento = num_doc.Obtener(facturador_electronico.StrIdentificacion, item.Documento, item.Prefijo, TipoDocumento.NominaAjuste.GetHashCode());

				TblDocumentos documento_bd = new TblDocumentos();

				if (numero_documento != null)
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
						//procesa el documento en un estado pendiente validar proceso
						item_respuesta = ProcesarV2(numero_documento, true);
						if (item_respuesta.Error == null)
							item_respuesta.Error = new Error();

						return item_respuesta;
					}
					else
					{
						//Se agrega validacion para que devuelva el mismo codigo de registro que tiene el documento en la BD del cliente de HGI
						if (item.VersionAplicativo.Contains("Ver. 202") && item.Prefijo.Equals("NP"))
						{
							item_respuesta.CodigoRegistro = item.CodigoRegistro;
							numero_documento.StrObligadoIdRegistro = item.CodigoRegistro;
							try
							{
								numero_documento = num_doc.Actualizar(numero_documento);
							}
							catch (Exception ex)
							{ }

						}

						mensaje = string.Format("El documento '{0}' con prefijo '{1}' ya existe para el Facturador Electrónico '{2}'", item.Documento, prefijo, facturador_electronico.StrIdentificacion);
						throw new ApplicationException(mensaje);
					}
				}


				TblEmpresasResoluciones resolucion = null;

				//List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				//lista_resolucion = _resolucion.ObtenerResolucionesPorTipo(item.DatosEmpleador.Identificacion, TipoDocumento.NominaAjuste.GetHashCode());

				TblEmpresasResoluciones resolucion_doc = null;

				if (lista_resolucion.Count > 0)
				{
					// filtra la resolución del documento con las condiciones de nit, prefijo y tipo de documento
					resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosEmpleador.Identificacion) &&
																									   _resolucion_doc.StrPrefijo.Equals(item.Prefijo) && _resolucion_doc.IntTipoDoc == TipoDocumento.NominaAjuste.GetHashCode()).FirstOrDefault();
				}

				//si no existe la resolucion la crea
				if (resolucion_doc == null)
				{
					//Se crea Resolucion
					TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();

					//Toma el IdsetDian de la resolucion de pruebas de Factura cuando esta en habilitacion
					tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.NominaAjuste.GetHashCode(), facturador_electronico.IntVersionDian);

					//Toma el IdsetDian de la resolucion de pruebas de Factura cuando esta en habilitacion
					if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
					{  
						if (string.IsNullOrWhiteSpace(item.NumeroResolucion))
							throw new ApplicationException("No se encontró IdSetDian registrado para el facturador electrónico");
						else
							tbl_resolucion.StrIdSetDian = item.NumeroResolucion;
					}
					// crea el registro en base de datos
					resolucion = _resolucion.Crear(tbl_resolucion);
					item.NumeroResolucion = resolucion.StrNumResolucion;
				}
				else
				{
					//Toma el IdsetDian de la resolucion de pruebas de Factura cuando esta en habilitacion
					if ((facturador_electronico.IntVersionDian.Equals(2)) && (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode()) && string.IsNullOrEmpty(resolucion_doc.StrIdSetDian))
					{

						TblEmpresasResoluciones resol_nomina = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosEmpleador.Identificacion) &&
																										  !string.IsNullOrEmpty(_resolucion_doc.StrIdSetDian)
																										  && _resolucion_doc.IntTipoDoc == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault();

						if (resol_nomina == null)
							throw new ApplicationException("No se encontró IdSetDian registrado para el facturador electrónico");
						else
						{
							resolucion_doc.StrIdSetDian = resol_nomina.StrIdSetDian;
							resolucion_doc.DatFechaActualizacion = Fecha.GetFecha();
							//resolucion_doc.IntVersionDian = resol_factura.IntVersionDian;
							_resolucion.Edit(resolucion_doc);
						}
					}

					resolucion = resolucion_doc;
					item.NumeroResolucion = resolucion.StrNumResolucion;

				}

				TblEmpresas facturadorelec_proceso = new TblEmpresas();
				facturadorelec_proceso = facturador_electronico;

				//valida si envian documento a afectar
				if ((item.TipoNota > 0) && !string.IsNullOrEmpty(item.CUNEPred))
				{
					//valida si el Documento afectado ya existe en Base de Datos
					Match numero_doc = Regex.Match(item.NumeroPred, "\\d+");

					//Match pref = Regex.Match(item.NumeroPred, "\\D+");
					List<DocumentoRespuesta> doc_ref = num_doc.ConsultaPorNumeros(facturador_electronico.StrIdentificacion, tipo_doc_pred.GetHashCode(), numero_doc.Value);
					if (doc_ref != null && doc_ref.Count > 0)
					{

						DocumentoRespuesta doc_resp = doc_ref.Where(d => d.Cufe.Equals(item.CUNEPred)).FirstOrDefault();
						if (doc_resp == null)
						{
							doc_resp = doc_ref.Where(d => d.Identificacion.Equals(item.DatosTrabajador.Identificacion)).FirstOrDefault();
							if (doc_resp != null)
							{
								item.CUNEPred = doc_resp.Cufe;
							}
							else
							{
								throw new ApplicationException(string.Format("El número de documento {0} para afectar no se encontró registrado en nuestra base de datos", item.NumeroPred));
							}
						}
						else if (item.TipoNota.Equals(2) && item.DatosTrabajador == null)
						{
							TblEmpresas adquiriente = new TblEmpresas();
							Ctl_Empresa clase_empresa = new Ctl_Empresa();
							adquiriente = clase_empresa.Obtener(doc_resp.Identificacion);
							
							item.DatosTrabajador = new Trabajador();
							item.DatosTrabajador.Identificacion = adquiriente.StrIdentificacion;
							item.DatosTrabajador.PrimerApellido = adquiriente.StrRazonSocial;
							item.DatosTrabajador.TipoDocumento = Convert.ToInt16(adquiriente.StrTipoIdentificacion);
							item.DatosTrabajador.Email = item.DatosEmpleador.Email;
							item.DatosTrabajador.Telefono = adquiriente.StrTelefono;
							item.DatosTrabajador.LugarTrabajoMunicipioCiudad = item.DatosEmpleador.MunicipioCiudad;
							item.DatosTrabajador.LugarTrabajoDepartamentoEstado = item.DatosEmpleador.DepartamentoEstado;
							item.DatosTrabajador.LugarTrabajoPais = item.DatosEmpleador.Pais;
							item.DatosTrabajador.LugarTrabajoDireccion = item.DatosEmpleador.Direccion;
						}
					}
					else
					{
						throw new ApplicationException(string.Format("El número de documento {0} para afectar no se encontró registrado en nuestra base de datos", item.NumeroPred));
					}

					try
					{
						mensaje = Enumeracion.GetDescription(estado);
						_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
					}
					catch (Exception) { }
				}
				else
					throw new ApplicationException("No se indica tipo de ajuste o numero de documento para afectar");

				// realiza el proceso de envío a la DIAN del documento en Validacion Previa V2
				item_respuesta = Procesar_v2(id_peticion, id_radicado, item, TipoDocumento.NominaAjuste, resolucion,
					facturador_electronico, documento_bd);
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
						DocumentoTipo = TipoDocumento.Nomina.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(mensaje, LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = "",
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = "",
						NumeroResolucion = string.Empty,//item.NumeroResolucion,
						Prefijo = item.Prefijo,
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = "",
						UrlXmlUbl = "",
						IdEstado = estado.GetHashCode(),
						IdPeticion = id_peticion,
						IdentificacionObligado = (item.DatosEmpleador != null) ? item.DatosEmpleador.Identificacion : "",
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
				numero_documento = num_doc.Obtener(facturador_electronico.StrIdentificacion, item.Documento, item.Prefijo, TipoDocumento.NominaAjuste.GetHashCode());

				if ((numero_documento != null) && (item_respuesta.IdProceso > (short)ProcesoEstado.Recepcion.GetHashCode() || item_respuesta.IdProceso < (short)ProcesoEstado.EnvioZip.GetHashCode()))
				{
					numero_documento.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
					numero_documento = num_doc.Actualizar(numero_documento);
					if (string.IsNullOrEmpty(item_respuesta.Error.Mensaje))
						item_respuesta.Error.Mensaje = "Se presentó inconsistencia al procesar el documento, enviar de nuevo el documento";
				}

				item_respuesta.IdProceso = (short)ProcesoEstado.Validacion.GetHashCode();
				item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();

			}
			else if (item_respuesta.IdProceso == (short)ProcesoEstado.EnvioZip.GetHashCode())
			{
				item_respuesta.IdProceso = (short)ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();
				item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();
				item_respuesta.Error.Mensaje = "La plataforma de la DIAN no dio respuesta del procesamiento del documento, por favor no modificar el documento y enviarlo de nuevo";
			}

			return item_respuesta;

		}

	}
}
