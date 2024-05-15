using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetUBLv2_1.DianListas;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using LibreriaGlobalHGInet.Error;
using Newtonsoft.Json;
using LibreriaGlobalHGInet.RegistroLog;
using System.Text.RegularExpressions;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{
		/// <summary>
		/// Procesa una lista de documentos tipo NotaCredito
		/// </summary>
		/// <param name="documentos">documentos tipo NotaCredito</param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<NotaCredito> documentos)
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

				//Contingencia de la DIAN 2024-03-09 desde las 6:00 am hasta las 6:00 PM
				DateTime fecha_ini_cont = new DateTime(2024, 03, 09, 6, 0, 0);
				DateTime fecha_fin_cont = new DateTime(2024, 03, 09, 18, 0, 0);

				if (Fecha.GetFecha() >= fecha_ini_cont && Fecha.GetFecha() < fecha_fin_cont)
				{
					throw new ApplicationException("Nos permitimos informar que el 09 de marzo de 2024, a partir de las 06:00 am y hasta las 6:00 pm, se realizará una ventana de mantenimiento en el Sistema de Facturación Electrónica DIAN, por lo que durante este tiempo no estará disponible este servicio informático,Por favor no hacer modificaciones al documento y enviarlo de nuevo a la plataforma unas horas despues pasada la contingencia de la DIAN");
				}

				int Sucursal_Obligado = documentos.FirstOrDefault().DatosObligado.CodigoSucursal;

				//Obtiene la lista de objetos de planes para trabajar(Reserva, procesar, idplan) esto puede generar una lista de objetos, ya que pueda que se requiera mas de un plan
				ListaPlanes = Planestransacciones.ObtenerPlanesActivos(documentos[0].DatosObligado.Identificacion, documentos.Count(), TipoDocPlanes.Documento.GetHashCode(), Sucursal_Obligado);

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

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
				if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					if (facturador_electronico.IntVersionDian == 1)
					{
						string resolucion_pruebas = Constantes.ResolucionPruebas;
						string nit_resolucion = Constantes.NitResolucionsinPrefijo;
						string prefijo_pruebas = string.Empty;

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

						//obtiene la resolucion de factura de pruebas
						lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_pruebas));

						foreach (var item in documentos)
						{
							item.NumeroResolucion = resolucion_pruebas;
							item.DatosObligado = DatosObligado;

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
					// Obtiene las resoluciones de la base de datos
					lista_resolucion = _resolucion.ObtenerResoluciones(documentos.FirstOrDefault().DatosObligado.Identificacion, "*", false);

				}

				//**Se agrega validacion y asignacion del aplicativo emisor del documento.

				DateTime fecha_control = new DateTime(2024, 05, 21, 0, 0, 0);
				Ctl_EmpresaIntegradores Emp_int = new Ctl_EmpresaIntegradores();
				List<TblEmpresaIntegradores> integradores = Emp_int.Obtener(facturador_electronico.StrIdentificacion);
				//string integrador_peticion = string.Empty;

				string integrador_peticion = string.Empty;

				try
				{
					integrador_peticion = documentos.Find(_inte => !string.IsNullOrEmpty(_inte.IdentificacionIntegrador)).IdentificacionIntegrador;
				}
				catch (Exception)
				{
				}

				if (string.IsNullOrWhiteSpace(integrador_peticion) && (Fecha.GetFecha() <= fecha_control))
				{
					try
					{
						integrador_peticion = Emp_int.Obtener(facturador_electronico.StrIdentificacion).FirstOrDefault().StrIdentificacionInt;
						if (string.IsNullOrWhiteSpace(integrador_peticion))
						{
							RegistroLog.EscribirLog(new ApplicationException(string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion)), MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
							integrador_peticion = Constantes.NitResolucionconPrefijo;
						}
					}
					catch (Exception ex)
					{
						RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
					}
				}
				else if (!string.IsNullOrWhiteSpace(integrador_peticion))
				{
					if (integradores != null && integradores.Count > 0 && !integradores.Select(x => x.StrIdentificacionInt == integrador_peticion && x.StrIdentificacionEmp == facturador_electronico.StrIdentificacion).FirstOrDefault())
					{
						if (Fecha.GetFecha() < fecha_control)
						{
							Ctl_Empresa ctr_empresa = new Ctl_Empresa();
							TblEmpresas Integrador = ctr_empresa.Obtener(integrador_peticion);
							if (Integrador.IntIntegrador == true)
							{
								TblEmpresaIntegradores empresa_inte = new TblEmpresaIntegradores();
								empresa_inte.StrIdentificacionEmp = facturador_electronico.StrIdentificacion;
								empresa_inte.StrIdentificacionInt = integrador_peticion;
								empresa_inte.StrIdSeguridad = Guid.NewGuid();

								try
								{
									Emp_int = new Ctl_EmpresaIntegradores();
									Emp_int.Crear(empresa_inte);
								}
								catch (Exception ex)
								{

									RegistroLog.EscribirLog(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
								}

							}
							else
							{
								//throw new ApplicationException(string.Format("La identificación del Integrador '{0}' correspondiente al aplicativo emisor no esta habilitado en nuestra plataforma", item.IdentificacionIntegrador));
								RegistroLog.EscribirLog(new ApplicationException(string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion)), MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
							}
						}
						else
						{
							throw new ApplicationException(string.Format("La identificación del Integrador '{0}' correspondiente al aplicativo emisor no esta habilitado en nuestra plataforma", integrador_peticion));
						}
					}
					else if (integradores == null || integradores.Count == 0)
					{
						if (Fecha.GetFecha() < fecha_control)
						{
							Ctl_Empresa ctr_empresa = new Ctl_Empresa();
							TblEmpresas Integrador = ctr_empresa.Obtener(integrador_peticion);
							if (Integrador.IntIntegrador == true)
							{
								TblEmpresaIntegradores empresa_inte = new TblEmpresaIntegradores();
								empresa_inte.StrIdentificacionEmp = facturador_electronico.StrIdentificacion;
								empresa_inte.StrIdentificacionInt = integrador_peticion;
								empresa_inte.StrIdSeguridad = Guid.NewGuid();

								try
								{
									Emp_int = new Ctl_EmpresaIntegradores();
									Emp_int.Crear(empresa_inte);
								}
								catch (Exception ex)
								{

									RegistroLog.EscribirLog(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
								}
							}
							else
							{
								//throw new ApplicationException(string.Format("La identificación del Integrador '{0}' correspondiente al aplicativo emisor no esta habilitado en nuestra plataforma", item.IdentificacionIntegrador));
								RegistroLog.EscribirLog(new ApplicationException(string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion)), MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.consulta);
							}
						}
						else
						{
							if (facturador_electronico.StrIdentificacion == facturador_electronico.StrEmpresaAsociada || facturador_electronico.StrEmpresaAsociada == Constantes.NitResolucionsinPrefijo)
							{
								TblEmpresaIntegradores empresa_inte = new TblEmpresaIntegradores();
								empresa_inte.StrIdentificacionEmp = facturador_electronico.StrIdentificacion;
								empresa_inte.StrIdentificacionInt = Constantes.NitResolucionsinPrefijo;
								empresa_inte.StrIdSeguridad = Guid.NewGuid();

								try
								{
									Emp_int = new Ctl_EmpresaIntegradores();
									Emp_int.Crear(empresa_inte);
								}
								catch (Exception ex)
								{

									RegistroLog.EscribirLog(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
								}
							}
							else
							{
								throw new ApplicationException("No se encontró información del Integrador correspondiente al aplicativo emisor, por favor indicar a su proveedor de software de esta inconsistencia");
							}
						}
					}

				}
				else
				{
					if (facturador_electronico.StrIdentificacion == facturador_electronico.StrEmpresaAsociada || facturador_electronico.StrEmpresaAsociada == Constantes.NitResolucionsinPrefijo)
					{
						TblEmpresaIntegradores empresa_inte = new TblEmpresaIntegradores();
						empresa_inte.StrIdentificacionEmp = facturador_electronico.StrIdentificacion;
						empresa_inte.StrIdentificacionInt = Constantes.NitResolucionsinPrefijo;
						empresa_inte.StrIdSeguridad = Guid.NewGuid();

						try
						{
							Emp_int = new Ctl_EmpresaIntegradores();
							Emp_int.Crear(empresa_inte);
						}
						catch (Exception ex)
						{

							RegistroLog.EscribirLog(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
						}
						integrador_peticion = Constantes.NitResolucionconPrefijo;
					}
					else
					{
						throw new ApplicationException("No se encontró información del Integrador correspondiente al aplicativo emisor, por favor indicar a su proveedor de software de esta inconsistencia");
					}


				}

				//Valida que si tiene certificado digital este vigente
				if (facturador_electronico.IntCertFirma == 1)
				{
					Ctl_Documento certif = new Ctl_Documento();
					certif.ValidarCertificadoDigital(facturador_electronico);
				}

				//Valida que si tiene certificado digital de HGI la fecha presupuestada para permitir firmar documentos con el certificado del Proveedor este vigente
				if (facturador_electronico.IntCertFirma == 0)
				{
					if (facturador_electronico.DatCertVence < Fecha.GetFecha())
						throw new ApplicationException("El certificado digital se encuentra vencido");
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
				Parallel.ForEach<NotaCredito>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion, integrador_peticion);
					respuesta.Add(item_respuesta);
				});

				//Conciliar Planes
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
					//Conciliar Planes
					var datos = Planestransacciones.ConciliarPlanes(ListaPlanes, respuesta);
				}
				catch (Exception exep)
				{
					Ctl_Log.Guardar(exep, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
				}

				string mensaje = ex.Message;
				Parallel.ForEach<NotaCredito>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
						DocumentoTipo = TipoDocumento.NotaCredito.GetHashCode(),
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

				Ctl_Log.Guardar(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				//throw new ApplicationException(ex.Message);

			}

			return respuesta;

		}


		/// <summary>
		/// Procesa un documento por paralelismo
		/// </summary>
		/// <param name="item">objeto de datos Nota Credito</param>
		/// <param name="facturador_electronico">facturador electrónico del documento</param>
		/// <param name="id_peticion">identificador de petición</param>
		/// <param name="fecha_actual">fecha actual de recepción del documento</param>
		/// <param name="lista_resolucion">resoluciones habilitadas para el facturador electrónico</param>
		/// <returns>resultado del proceso</returns>
		private static DocumentoRespuesta Procesar(NotaCredito item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion, string Integrador_Peticion)
		{
			TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

			Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

			DocumentoRespuesta item_respuesta = new DocumentoRespuesta() { DescuentaSaldo = false };

			Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();

			string mensaje = string.Empty;

			item.IdentificacionIntegrador = string.IsNullOrEmpty(item.IdentificacionIntegrador) ? Integrador_Peticion : item.IdentificacionIntegrador;

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

			}

			//Se lee un archivo json y se convierte a objeto nota para pruebas
			//NotaCredito obj_nc = new NotaCredito();
			//string objeto = System.IO.File.ReadAllText(@"E:\Desarrollo\jzea\Proyectos\HGInetMiFacturaElectronica\Codigo\HGInetMiFacturaElectronicaWeb\dms\Debug\811021438-NQA-990007445.json").ToString();
			//obj_nc = JsonConvert.DeserializeObject<NotaCredito>(objeto);
			//item = obj_nc;

			//Si el documento enviado ya existe retorna la informacion que se tiene almacenada
			bool doc_existe = false;

			if (string.IsNullOrEmpty(item.Prefijo))
				item.Prefijo = string.Empty;

			//radicado del documento
			Guid id_radicado = Guid.NewGuid();
			string prefijo = item.Prefijo;
			string numero = item.Documento.ToString();
			ProcesoEstado proceso_act = ProcesoEstado.Recepcion;
			string proceso_txt = Enumeracion.GetDescription(proceso_act);
			CategoriaEstado estado = Enumeracion.GetEnumObjectByValue<CategoriaEstado>(Ctl_Documento.ObtenerCategoria(proceso_act.GetHashCode()));

			TblDocumentos numero_documento = new TblDocumentos();
			Ctl_Documento num_doc = new Ctl_Documento();

			try
			{

				//valida si el Documento ya existe en Base de Datos
				numero_documento = num_doc.Obtener(item.DatosObligado.Identificacion, item.Documento, item.Prefijo);

				TblDocumentos documento_bd = new TblDocumentos();

				if (numero_documento != null)
				{
					if (facturador_electronico.IntVersionDian == 1)
					{
						item_respuesta = Ctl_Documento.Convertir(numero_documento);
						item_respuesta.IdPeticion = id_peticion;
						id_radicado = Guid.Parse(item_respuesta.IdDocumento);
						doc_existe = true;
						throw new ApplicationException(string.Format("El documento {0} con prefijo {1} ya xiste para el Facturador Electrónico {2}", item.Documento, item.Prefijo, facturador_electronico.StrIdentificacion));
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

				//Valido que si el prefijo trae espacios en blanco, los quite y luego valide
				string prefijo_sin_espacio = Regex.Replace(item.Prefijo, @"\s", "");

				if (!item.Prefijo.Equals(prefijo))
					throw new ApplicationException(string.Format("El prefijo {0} contiene espacio en blanco, corrijalo y envie de nuevo", item.Prefijo));

				// filtra la resolución del documento con las condiciones de nit, prefijo y tipo de documento
				TblEmpresasResoluciones resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion) &&
																				_resolucion_doc.StrPrefijo.Equals(item.Prefijo)
																				&& _resolucion_doc.IntTipoDoc == TipoDocumento.NotaCredito.GetHashCode()).FirstOrDefault();
				//si no existe la resolucion la crea
				if (resolucion_doc == null)
				{
					//Se crea Resolucion
					TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();
					if (facturador_electronico.IntVersionDian == 1)
					{
						if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.NotaCredito.GetHashCode(), facturador_electronico.IntVersionDian);
						}
						else
						{
							tbl_resolucion = Ctl_EmpresaResolucion.Convertir(item.DatosObligado.Identificacion, item.Prefijo, TipoDocumento.NotaCredito.GetHashCode(), facturador_electronico.IntVersionDian);
						}
					}
					else
					{
						tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.NotaCredito.GetHashCode(), facturador_electronico.IntVersionDian);

						//Toma el IdsetDian de la resolucion de pruebas de Factura cuando esta en habilitacion
						if (facturador_electronico.IntHabilitacion < 99)
						{
							TblEmpresasResoluciones resol_factura = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion) &&
																					  !string.IsNullOrEmpty(_resolucion_doc.StrIdSetDian)
																					  && _resolucion_doc.IntTipoDoc == TipoDocumento.Factura.GetHashCode()).FirstOrDefault();

							if (resol_factura == null)
								throw new ApplicationException("No se encontró IdSetDian registrado para el facturador electrónico");
							else
								tbl_resolucion.StrIdSetDian = resol_factura.StrIdSetDian;
						}
					}
					// crea el registro en base de datos
					resolucion = _resolucion.Crear(tbl_resolucion);
					item.NumeroResolucion = resolucion.StrNumResolucion;
				}
				else
				{
					//Toma el IdsetDian de la resolucion de pruebas de Factura cuando esta en habilitacion
					if ((facturador_electronico.IntVersionDian.Equals(2)) && (facturador_electronico.IntHabilitacion < 99) && string.IsNullOrEmpty(resolucion_doc.StrIdSetDian))
					{

						TblEmpresasResoluciones resol_factura = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion) &&
																										  !string.IsNullOrEmpty(_resolucion_doc.StrIdSetDian)
																										  && _resolucion_doc.IntTipoDoc == TipoDocumento.Factura.GetHashCode()).FirstOrDefault();

						if (resol_factura == null)
							throw new ApplicationException("No se encontró IdSetDian registrado para el facturador electrónico");
						else
						{
							resolucion_doc.StrIdSetDian = resol_factura.StrIdSetDian;
							//resolucion_doc.IntVersionDian = resol_factura.IntVersionDian;
							_resolucion.Edit(resolucion_doc);
						}
					}
					resolucion = resolucion_doc;
					item.NumeroResolucion = resolucion.StrNumResolucion;

				}

				TblEmpresas facturadorelec_proceso = new TblEmpresas();
				facturadorelec_proceso = facturador_electronico;

				//Se valida en V2 que la factura afectada exista y en que version esta
				if (facturador_electronico.IntVersionDian == 2)
				{
					//Se hace todo igual mientras no sea Nota de ajuste de documento de adquisiciones
					if (item.TipoOperacion != 3)
					{
						//aplicarán el conjunto de validaciones correspondiente
						if (item.TipoOperacion == 0)
							item.TipoOperacion = 20;

						//valida si envian documento a afectar
						if (!string.IsNullOrEmpty(item.DocumentoRef) && !string.IsNullOrEmpty(item.CufeFactura) && (item.TipoOperacion != 22))
						{
							//valida si el Documento afectado ya existe en Base de Datos
							List<DocumentoRespuesta> doc_ref = num_doc.ConsultaPorNumeros(facturador_electronico.StrIdentificacion, TipoDocumento.Factura.GetHashCode(), item.DocumentoRef);
							if (doc_ref != null && doc_ref.Count > 0)
							{

								DocumentoRespuesta doc_resp = doc_ref.Where(d => d.Cufe.Equals(item.CufeFactura)).FirstOrDefault();
								if (doc_resp != null)
								{
									//Si el documento afectado es diferente a la version de la empresa emisora se cambia el tìpo de operacion
									if (doc_resp.IdVersionDian == 1)
										item.TipoOperacion = 22;
									//throw new ApplicationException(string.Format("El número de Factura afectada {0} no es válida para la Versión que se esta enviando", item.DocumentoRef));
									item.DocumentoRef = string.IsNullOrWhiteSpace(doc_resp.Prefijo) ? doc_resp.Documento.ToString()  : string.Format("{0}{1}", doc_resp.Prefijo, doc_resp.Documento);
								}
								else
								{
									doc_resp = doc_ref.Where(d => d.Identificacion.Equals(item.DatosAdquiriente.Identificacion)).FirstOrDefault();
									if (doc_resp != null)
									{
										//Si el documento afectado es diferente a la version de la empresa emisora se cambia el tìpo de operacion
										if (doc_resp.IdVersionDian == 1)
										{
											item.TipoOperacion = 22;
											item.CufeFactura = doc_resp.Cufe;
										}
										else if (!doc_resp.Cufe.Equals(item.CufeFactura))
										{
											item.TipoOperacion = 22;
										}
									}
									else
									{
										//si el documento afectado no existe en BD y no envian el CUFE cambio el tipo de operacion
										item.TipoOperacion = 22;
									}
								}
							}
							else
							{
								if (!string.IsNullOrEmpty(item.CufeFactura) && !item.CufeFactura.Equals("0"))
								{
									item.TipoOperacion = 20;
								}
								else
								{
									//si el documento afectado no existe en BD y no envian el CUFE cambio el tipo de operacion
									item.TipoOperacion = 22;
								}

								//Si envian Prefijo de la factura que estan afectando se concatena para la impresion
								if (!string.IsNullOrEmpty(item.PrefijoFactura))
								{
									item.DocumentoRef = string.Format("{0} - {1}", item.PrefijoFactura, item.DocumentoRef);
								}
								else
								{
									item.PrefijoFactura = string.Empty;
								}
							}
						}
						else
						{
							item.TipoOperacion = 22;

							//Si envian Prefijo de la factura que estan afectando se concatena para la impresion
							if (!string.IsNullOrEmpty(item.PrefijoFactura))
							{
								item.DocumentoRef = string.Format("{0} - {1}", item.PrefijoFactura, item.DocumentoRef);
							}
							else
							{
								item.PrefijoFactura = string.Empty;
							}
						}
					}
					else
					{
						//valida si envian documento a afectar
						if (!string.IsNullOrEmpty(item.DocumentoRef) || string.IsNullOrEmpty(item.CufeFactura))
						{
							//valida si el Documento afectado ya existe en Base de Datos
							TblDocumentos doc_ref = num_doc.ConsultaDocSoporte(facturador_electronico.StrIdentificacion, Convert.ToInt32(item.DocumentoRef), TipoDocumento.Factura.GetHashCode(), item.DatosAdquiriente.Identificacion, item.PrefijoFactura);
							if (doc_ref != null)
							{

								item.DocumentoRef = string.Format("{0}{1}", doc_ref.StrPrefijo, doc_ref.IntNumero);
								item.CufeFactura = doc_ref.StrCufe;
								item.PrefijoFactura = doc_ref.StrPrefijo;
							}
							else
							{
								throw new ApplicationException(string.Format("No se encontró Documento Soporte {0} registrado en nuestra plataforma", item.DocumentoRef));
							}
						}

						if (string.IsNullOrEmpty(item.CufeFactura) || item.CufeFactura.Equals("0"))
							throw new ApplicationException(string.Format("No se encontró Documento Soporte {0} registrado en nuestra plataforma con CUDS {1}", item.DocumentoRef, item.CufeFactura));
					}
				}

				try
				{
					mensaje = Enumeracion.GetDescription(estado);
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_act, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception) { }

				if (facturadorelec_proceso.IntVersionDian == 1)
				{
					// realiza el proceso de envío a la DIAN del documento en V1
					item_respuesta = Procesar(id_peticion, id_radicado, item, TipoDocumento.NotaCredito, resolucion,
						facturadorelec_proceso);
				}
				else
				{
					// realiza el proceso de envío a la DIAN del documento en V2
					item_respuesta = Procesar_v2(id_peticion, id_radicado, item, TipoDocumento.NotaCredito, resolucion,
						facturadorelec_proceso, documento_bd);
				}

			}
			catch (Exception excepcion)
			{
				mensaje = string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message);

				try
				{
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_act, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception ex)
				{
					Ctl_Log.Guardar(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
				}
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				if (!doc_existe)
				{
					item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_act),
						DocumentoTipo = TipoDocumento.NotaCredito.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = "",
						IdProceso = proceso_act.GetHashCode(),
						MotivoRechazo = "",
						NumeroResolucion = item.NumeroResolucion,
						Prefijo = "",
						ProcesoFinalizado = (proceso_act == ProcesoEstado.Finalizacion || proceso_act == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = "",
						UrlXmlUbl = ""
					};
				}
				else
				{
					item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					//Si el estado es menor a firmado, la respuesta del estado siempre va ser no recibido
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

				if ((numero_documento != null) && (item_respuesta.IdProceso > (short)ProcesoEstado.Recepcion.GetHashCode() || item_respuesta.IdProceso < (short)ProcesoEstado.EnvioZip.GetHashCode()))
				{
					numero_documento.IntIdEstado = (short)ProcesoEstado.PrevalidacionErrorPlataforma.GetHashCode();
					numero_documento = num_doc.Actualizar(numero_documento);
					if (string.IsNullOrEmpty(item_respuesta.Error.Mensaje))
						item_respuesta.Error.Mensaje = "Se presentó inconsistencia al procesar el documento, enviar de nuevo el documento";
				}

				// [749311] Se agrega validación para que no cambie el proceso cuando se realice algun evento sobre el documento.
				if (item_respuesta.IdProceso != (short)ProcesoEstado.RecepcionAcuse.GetHashCode() && item_respuesta.IdProceso != (short)ProcesoEstado.EnvioRespuestaAcuse.GetHashCode() && item_respuesta.IdProceso != (short)ProcesoEstado.AcuseVisto.GetHashCode())
				{
					item_respuesta.IdProceso = (short)ProcesoEstado.Validacion.GetHashCode();
					item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();
				}

			}
			else if (item_respuesta.IdProceso == (short)ProcesoEstado.EnvioZip.GetHashCode())
			{
				item_respuesta.IdProceso = (short)ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode();
				item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();
				item_respuesta.Error.Mensaje = "La plataforma de la DIAN no dio respuesta del procesamiento del documento, por favor no modificar el documento y enviarlo de nuevo";
			}

			return item_respuesta;
		}


		/// <summary>
		/// Validación del Objeto Nota Credito
		/// </summary>
		/// <param name="documento">Objeto NotaCredito</param>
		/// <returns></returns>
		public static NotaCredito ValidarNotaCredito(NotaCredito documento, TblEmpresasResoluciones resolucion, TblEmpresas facturador)
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
			if (string.IsNullOrEmpty(documento.DocumentoRef) && documento.TipoOperacion == 20)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

			//Inicializa la propiedad, no es un campo requerido
			if (string.IsNullOrEmpty(documento.PedidoRef))
				documento.PedidoRef = string.Empty;

			//Validar que no este vacia la fecha del documento de referencia
			if (documento.FechaFactura == null && documento.TipoOperacion == 20)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "FechaFactura", "DateTime"));

			if (string.IsNullOrEmpty(documento.CufeFactura) && documento.TipoOperacion == 20)
				throw new ArgumentException("El Cufe de la Factura afectada no esta bien formado");

			//valida resolucion
			if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
				throw new ApplicationException(string.Format("El número de resolución {0} no es válido", documento.NumeroResolucion));

			//valida el prefijo si es null lo llena vacio
			/*if (string.IsNullOrEmpty(documento.Prefijo))
				documento.Prefijo = string.Empty;*/

			//Validar que no este vacia la fecha
			if (documento.Fecha == null)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Fecha", "DateTime"));

			//Valida que no este vacio el concepto
			if (documento.Concepto == null)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Concepto", "string"));

			//Valida que la fecha este en los terminos
			if (facturador.IntVersionDian == 1)
			{
				if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
					throw new ApplicationException(string.Format("La fecha de elaboración {0} no está dentro los términos.", documento.Fecha));
			}
			else
			{
				if (documento.Fecha.Date != Fecha.GetFecha().Date && !documento.TipoOperacion.Equals(3))
					throw new ApplicationException(string.Format("La fecha de elaboración del Documento no puede ser diferente a la fecha actual. Fecha Documento: {0}", documento.Fecha.ToString("yyyy-MM-dd")));

				ListaConceptoNotaCredito list_concepto = new ListaConceptoNotaCredito();
				ListaItem concepto = list_concepto.Items.Where(d => d.Codigo.Equals(documento.Concepto)).FirstOrDefault();
				if (concepto == null)
				{
					throw new ApplicationException(string.Format("El concepto {0} no es válido según Estandar DIAN", documento.Concepto));
				}
				else
				{
					documento.ConceptoDescripcion = concepto.Descripcion;
				}


				/*Ctl_Documento num_doc = new Ctl_Documento();

				//valida si el Documento afectado ya existe en Base de Datos
				List<DocumentoRespuesta> doc_ref = num_doc.ConsultaPorNumeros(facturador.StrIdentificacion, TipoDocumento.Factura.GetHashCode(), documento.DocumentoRef);
				DocumentoRespuesta doc_resp = doc_ref.Find(d => d.Cufe.Equals(documento.CufeFactura));
				if (doc_resp != null)
				{
					if (doc_resp.IdVersionDian != facturador.IntVersionDian)
						throw new ApplicationException(string.Format("El número de Factura afectada {0} no es válida para la Versión que se esta enviando", documento.DocumentoRef));
				}
				else
				{
					throw new ApplicationException(string.Format("El número de Factura afectada {0} no se encuentra registrada", documento.DocumentoRef));
				}*/

				if (documento.TipoOperacion == 22)
				{
					//documento.FechaFactura = documento.FechaFactura.AddMonths(-3);
					if (documento.FechaFinFactura < documento.FechaFactura)
						documento.FechaFinFactura = documento.FechaFactura;

					if (concepto.Codigo == "2")
						throw new ApplicationException(string.Format("El concepto {0} en este tipo de Notas Crédito no puede anular Facturas", documento.Concepto));
				}

			}

			//Valida que no este vacio y Formato
			if (string.IsNullOrEmpty(documento.Moneda))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

			if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosObligado, "Obligado", facturador);

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosAdquiriente, "Adquiriente", facturador);

			//Valida totales del objeto
			ValidarTotales(null, documento, null, TipoDocumento.NotaCredito, facturador);

			if (facturador.IntHabilitacion > 0 && documento.TipoOperacion != 3)
			{
				if (documento.DocumentoFormato != null)
				{
					//Valida que envien el titulo del documento y si es vacio lo llena
					if (string.IsNullOrEmpty(documento.DocumentoFormato.Titulo) || documento.DocumentoFormato.Titulo == null)
						documento.DocumentoFormato.Titulo = Enumeracion.GetDescription(TipoDocumento.NotaCredito).ToUpper();
				}
				else
					throw new ApplicationException("No se encontró información del Formato");
			}

			//Se valida que si el documento tiene una moneda diferente a pesos colombianos envien la tasa de cambio
			if (!documento.Moneda.Equals("COP") && documento.Trm == null)
				throw new ArgumentException(string.Format("No se encontró información de la tasa de Cambio para la Moneda {0}", documento.Moneda));

			//Validacion Sector Salud
			if (documento.SectorSalud != null)
			{
				if (documento.SectorSalud.CamposSector.Count == 0 || documento.SectorSalud.CamposSector.Count < 11 || documento.SectorSalud.CamposSector.Count > 11)
					throw new ApplicationException("No se encontró la cantidad correcta de información del Sector Salud, deben ser 11 items Según Resolucion 510 del ministerio de Salud");

				CampoValor valid_salud = new CampoValor();
				string valid_enum_salud = string.Empty;

				if (documento.SectorSalud.CamposSector.Count == 11)
				{
					valid_salud = documento.SectorSalud.CamposSector[1];
					try
					{
						ModalidadDePago dato_iden = Enumeracion.GetValueFromAmbiente<ModalidadDePago>(valid_salud.Valor);
						valid_enum_salud = Enumeracion.GetDescription(dato_iden);
					}
					catch (Exception)
					{
						throw new ApplicationException(string.Format("La modalidad de pago {0} no corresponde a ninguno del listado del Sector Salud.", valid_salud.Valor));
					}

					valid_salud = documento.SectorSalud.CamposSector[2];
					try
					{
						CoberturaSalud dato_enum = Enumeracion.GetEnumObjectByValue<CoberturaSalud>(Convert.ToInt16(valid_salud.Valor));
						valid_enum_salud = Enumeracion.GetDescription(dato_enum);
					}
					catch (Exception)
					{

						throw new ApplicationException(string.Format("La cobertura de salud {0} no corresponde a ninguno del listado del Sector Salud.", valid_salud.Valor));
					}
				}

				if (documento.SectorSalud.CamposSector.Count == 21)
				{
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
			}

			return documento;
		}


	}
}
