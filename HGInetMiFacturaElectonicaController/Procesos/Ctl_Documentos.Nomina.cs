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
using LibreriaGlobalHGInet.RegistroLog;
using LibreriaGlobalHGInet.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		public static List<DocumentoRespuesta> Procesar(List<Nomina> documentos)
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

			////Planes y transacciones
			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;


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

				int Sucursal_Obligado = documentos.FirstOrDefault().DatosEmpleador.CodigoSucursal;

				List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				lista_resolucion = _resolucion.ObtenerResolucionesPorTipo(facturador_electronico.StrIdentificacion, TipoDocumento.Nomina.GetHashCode());

				// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
				if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{

					if (!facturador_electronico.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo))
					{
						string set_Id = documentos.FirstOrDefault().NumeroResolucion;

						foreach (var item in lista_resolucion)
						{
							item.StrIdSetDian = set_Id;

						}
					}

					foreach (var item in documentos)
					{
						item.DatosTrabajador.Email = facturador_electronico.StrMailAdmin;

					}
				}

				//**Se agrega validacion y asignacion del aplicativo emisor del documento.

				DateTime fecha_control = new DateTime(2024, 07, 21, 0, 0, 0);
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
					TblEmpresaIntegradores empInt = integradores.Where(x => x.StrIdentificacionInt == integrador_peticion && x.StrIdentificacionEmp == facturador_electronico.StrIdentificacion).FirstOrDefault();
					if (integradores != null && integradores.Count > 0 && empInt == null)
					{
						if (Fecha.GetFecha() < fecha_control)
						{
							try
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
							catch (Exception ex)
							{

								RegistroLog.EscribirLog(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
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
							try
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
							catch (Exception ex)
							{

								RegistroLog.EscribirLog(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Validacion de la identificacion del integrador, Facturador: {0} - IdIntegrador: {1}", facturador_electronico.StrIdentificacion, integrador_peticion));
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

				//Obtiene la lista de objetos de planes para trabajar(Reserva, procesar, idplan) esto puede generar una lista de objetos, ya que pueda que se requiera mas de un plan
				ListaPlanes = Planestransacciones.ObtenerPlanesActivos(facturador_electronico.StrIdentificacion, documentos.Count(), TipoDocPlanes.Nomina.GetHashCode(), Sucursal_Obligado);

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

				//Se valida si en la peticion envian variaciones de nomina para procesarla primero y luego continuar con las demas dominas
				List<Nomina> documentos_novedades = documentos.Where(x => x.VariacionNomina == true).ToList();

				if (documentos_novedades != null && documentos_novedades.Count > 0)
				{
					//Obtengo solo los documentos de los empleados que presentan variacion y las ordeno en orden ascendente para que se gestione primero la principal y luego la variacion
					List<Nomina> doc_nov = documentos.Where(x => documentos_novedades.Any(x2 => x2.DatosTrabajador.Identificacion == x.DatosTrabajador.Identificacion)).OrderBy(y => y.Documento).ToList();

					string prefijo_doc_variacion = string.Empty;
					long doc_variacion = 0;
					foreach (Nomina item in doc_nov)
					{
						bool procesar_novedad = true;

						if (item.VariacionNomina == true)
						{
							string cude_nom_var = string.Empty;
							try
							{
								cude_nom_var = respuesta.Where(x => x.Identificacion == item.DatosTrabajador.Identificacion && x.IdEstado >= 200).FirstOrDefault().Cufe;
							}
							catch (Exception)
							{}
							if (string.IsNullOrEmpty(cude_nom_var))
							{
								Ctl_Documento _nom = new Ctl_Documento();

								//Si envian los documentos uno a uno entonces consulto en el mes nominas recibidas con las condiciones de mes, emisor y trabajador y tomo la primera que debe ser la principal
								if (doc_variacion == 0 && string.IsNullOrEmpty(prefijo_doc_variacion))
								{
									List<TblDocumentos> lista_doc = _nom.ObtenerPorMes(item.DatosEmpleador.Identificacion, Fecha.GetFecha().Month, item.DatosTrabajador.Identificacion);
									if (lista_doc != null && lista_doc.Count > 0)
									{
										prefijo_doc_variacion = lista_doc.Where(x => x.IntDocTipo == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault().StrPrefijo;
										doc_variacion = lista_doc.Where(x => x.IntDocTipo == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault().IntNumero;
									}
								}
								
								//Consulto el documento que esta plataforma segun el numero y el prefijo
								TblDocumentos nomina_validad = _nom.Obtener(item.DatosEmpleador.Identificacion, doc_variacion, prefijo_doc_variacion, TipoDocumento.Nomina.GetHashCode());

								//Si no encuentra el documento o no esta recibido o validado correctamente por la DIAN no gestiona la variacion
								if (nomina_validad == null || nomina_validad.IdCategoriaEstado < 200)
								{
									DocumentoRespuesta respuesta_doc = new DocumentoRespuesta()
									{
										Aceptacion = 0,
										CodigoRegistro = item.CodigoRegistro,
										Cufe = "",
										DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
										DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
										DocumentoTipo = TipoDocumento.Nomina.GetHashCode(),
										Documento = item.Documento,
										Error = new LibreriaGlobalHGInet.Error.Error("No se encontró documento principal de novedad o este documento no esta validado corectamente por la DIAN", LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, null),
										EstadoDian = null,
										FechaRecepcion = fecha_actual,
										FechaUltimoProceso = fecha_actual,
										IdDocumento = "",
										Identificacion = item.DatosTrabajador.Identificacion,
										IdProceso = proceso_actual.GetHashCode(),
										MotivoRechazo = "",
										NumeroResolucion = string.Empty,
										Prefijo = item.Prefijo,
										ProcesoFinalizado = 0,
										UrlPdf = "",
										UrlXmlUbl = "",
										IdEstado = estado.GetHashCode(),
										IdPeticion = id_peticion,
										IdentificacionObligado = (item.DatosEmpleador != null) ? item.DatosEmpleador.Identificacion : "",
										DescuentaSaldo = false,
										IdVersionDian = (facturador_electronico != null) ? facturador_electronico.IntVersionDian : 0
									};
									respuesta.Add(respuesta_doc);
									procesar_novedad = false;
								}
							}

						}
						else
						{
							prefijo_doc_variacion = item.Prefijo;
							doc_variacion = item.Documento;
						}


						if (procesar_novedad == true)
						{
							DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion, integrador_peticion);
							respuesta.Add(item_respuesta);
						}
						
						//Cuando se gestiona los documentos con variacion se quitan del paquete principal y lo gestione el proceso de siempre
						documentos.Remove(item);
					}
				}

				Parallel.ForEach<Nomina>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion, integrador_peticion);
					respuesta.Add(item_respuesta);
				});

				var datos = Planestransacciones.ConciliarPlanes(ListaPlanes, respuesta);

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
				Parallel.ForEach<Nomina>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
						DocumentoTipo = TipoDocumento.Nomina.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(mensaje, LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, ex.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = "",
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
						IdVersionDian = (facturador_electronico != null) ? facturador_electronico.IntVersionDian : 0,
						IdAmbiente = (plataforma_datos.RutaPublica.Contains("habilitacion") || plataforma_datos.RutaPublica.Contains("localhost")) ? 2 : 1,
					};
					respuesta.Add(item_respuesta);
				});

				////Planes y transacciones
				Ctl_Log.Guardar(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, documentos.FirstOrDefault().DatosEmpleador.Identificacion);
				//throw new ApplicationException(ex.Message);

			}

			return respuesta;
		}

		private static DocumentoRespuesta Procesar(Nomina item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion, string Integrador_Peticion)
		{
			DocumentoRespuesta item_respuesta = new DocumentoRespuesta() { DescuentaSaldo = false };

			Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();

			//Si el documento enviado ya existe retorna la informacion que se tiene almacenada
			bool doc_existe = false;

			//radicado del documento
			Guid id_radicado = Guid.NewGuid();

			string prefijo = item.Prefijo;
			string numero = item.Documento.ToString();

			item.IdentificacionIntegrador = string.IsNullOrEmpty(item.IdentificacionIntegrador) ? Integrador_Peticion : item.IdentificacionIntegrador;

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
					DocumentoTipo = TipoDocumento.Nomina.GetHashCode(),
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
				//valida si el Documento ya existe en Base de Datos
				numero_documento = num_doc.Obtener(facturador_electronico.StrIdentificacion, item.Documento, item.Prefijo, TipoDocumento.Nomina.GetHashCode());

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
					else if (numero_documento.IntIdEstado == ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode() || numero_documento.IntIdEstado == ProcesoEstado.EnvioZip.GetHashCode() || numero_documento.IntIdEstado == ProcesoEstado.ConsultaDian.GetHashCode())
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
							{}

						}

						mensaje = string.Format("El documento '{0}' con prefijo '{1}' ya existe para el Empleador '{2}'", item.Documento, prefijo, facturador_electronico.StrIdentificacion);
						throw new ApplicationException(mensaje);
					}
				}

				TblEmpresasResoluciones resolucion = null;

				//List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				//lista_resolucion = _resolucion.ObtenerResolucionesPorTipo(item.DatosEmpleador.Identificacion,TipoDocumento.Nomina.GetHashCode());

				TblEmpresasResoluciones resolucion_doc = null;

				if (lista_resolucion.Count > 0)
				{
					// filtra la resolución del documento con las condiciones de nit, prefijo y tipo de documento
					resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosEmpleador.Identificacion) &&
																									   _resolucion_doc.StrPrefijo.Equals(item.Prefijo) && _resolucion_doc.IntTipoDoc == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault();
				}
				else
				{
					lista_resolucion = _resolucion.ObtenerResolucionesPorTipo(item.DatosEmpleador.Identificacion, TipoDocumento.Nomina.GetHashCode());

					if (lista_resolucion.Count > 0)
						resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosEmpleador.Identificacion) &&
																									   _resolucion_doc.StrPrefijo.Equals(item.Prefijo) && _resolucion_doc.IntTipoDoc == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault();
				}

				//si no existe la resolucion la crea
				if (resolucion_doc == null)
				{
					//Se crea Resolucion
					TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();

					tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.Nomina.GetHashCode(), facturador_electronico.IntVersionDian);

					//Toma el IdsetDian de la resolucion de pruebas de Factura cuando esta en habilitacion
					if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
					{
						if (string.IsNullOrWhiteSpace(item.NumeroResolucion))
							throw new ApplicationException("No se encontró IdSetDian registrado para el facturador electrónico");
						else
							tbl_resolucion.StrIdSetDian = item.NumeroResolucion;
					}

					// crea el registro en base de datos
					tbl_resolucion.StrIdSetDian = item.NumeroResolucion;
					resolucion = _resolucion.Crear(tbl_resolucion);
					item.NumeroResolucion = resolucion.StrNumResolucion;
				}
				else
				{
					//Valida que la resolucion creada si tenga idset y si es habilitacion se le asigna el que trae el objeto y se actualiza en bd
					if ((!facturador_electronico.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo)) &&(facturador_electronico.IntHabilitacion < 99) && (string.IsNullOrEmpty(resolucion_doc.StrIdSetDian) || !resolucion_doc.StrIdSetDian.Equals(item.NumeroResolucion)))
					{
						resolucion_doc.StrIdSetDian = item.NumeroResolucion;
						resolucion_doc.DatFechaActualizacion = Fecha.GetFecha();
						//resolucion_doc.IntVersionDian = resol_factura.IntVersionDian;
						_resolucion.Edit(resolucion_doc);
					}

					resolucion = resolucion_doc;
					item.NumeroResolucion = resolucion.StrNumResolucion;

				}

				try
				{
					mensaje = Enumeracion.GetDescription(estado);
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception) { }

				// realiza el proceso de envío a la DIAN del documento en Validacion Previa V2
				item_respuesta = Procesar_v2(id_peticion, id_radicado, item, TipoDocumento.Nomina, resolucion,
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
						Identificacion = (item.DatosTrabajador != null) ? item.DatosTrabajador.Identificacion : "",
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
				numero_documento = num_doc.Obtener(facturador_electronico.StrIdentificacion, item.Documento, item.Prefijo, TipoDocumento.Nomina.GetHashCode());

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



		public static object ValidarNomina(object documento_obj, TblEmpresas facturador, TipoDocumento tipo_nomina)
		{

			var documento = (dynamic)null;
			documento = documento_obj;

			DateTime validar_fecha = new DateTime();

			// valida objeto recibido
			if (documento == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Nomina"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.CodigoRegistro))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.DataKey))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

			// valida el número del documento no sea valor negativo
			if (documento.Documento < 0)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

			if(documento.FechaGen.Equals(validar_fecha))
				throw new ApplicationException(string.Format("La FechaGen {0} no es válida", documento.FechaGen));


			//Valida que no este vacio y este bien formado 
			ValidarEmpleador(documento.DatosEmpleador);

			bool validar_completo = true;

			//Se valida que no sea un ajuste de tipo eliminacion
			if (tipo_nomina.Equals(TipoDocumento.NominaAjuste) && documento.TipoNota.Equals(2))
			{
				validar_completo = false;
			}

			if (validar_completo == true)
			{
				//Validacion de la informacion del Periodo
				if (documento.DatosPeriodo != null)
				{
					if (documento.DatosPeriodo.FechaIngreso.Equals(validar_fecha))
						throw new ApplicationException(string.Format("La FechaIngreso {0} no es válida", documento.DatosPeriodo.FechaIngreso));

					if (documento.DatosPeriodo.FechaLiquidacionInicio.Equals(validar_fecha))		
						throw new ApplicationException(string.Format("La FechaLiquidacionInicio {0} no es válida", documento.DatosPeriodo.FechaLiquidacionInicio));

					if (documento.DatosPeriodo.FechaLiquidacionFin.Equals(validar_fecha))
						throw new ApplicationException(string.Format("La FechaLiquidacionFin {0} no es válida", documento.DatosPeriodo.FechaLiquidacionFin));

					//if (!DateTime.TryParseExact(documento.DatosPeriodo.FechaLiquidacion.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out validar_fecha))
					//	throw new ApplicationException(string.Format("La FechaLiquidacion '{0}' no es válida", documento.DatosPeriodo.FechaLiquidacion));

					//if (documento.DatosPeriodo.FechaLiquidacionInicio.Day > 1)
					//	throw new ApplicationException(string.Format("El dia que reporta en FechaLiquidacionInicio {0} no es válido según el periodo que esta informando", documento.DatosPeriodo.FechaLiquidacionInicio.Day));

					//if (!(DateTime.DaysInMonth(documento.DatosPeriodo.FechaLiquidacionFin.Year, documento.DatosPeriodo.FechaLiquidacionFin.Month) == documento.DatosPeriodo.FechaLiquidacionFin.Day || documento.DatosPeriodo.FechaLiquidacionInicio.Day + 1 == DateTime.DaysInMonth(documento.DatosPeriodo.FechaLiquidacionFin.Year, documento.DatosPeriodo.FechaLiquidacionFin.Month)))
					//	throw new ApplicationException(string.Format("El dia que reporta en FechaLiquidacionFin {0} no es válido según el periodo que esta informando", documento.DatosPeriodo.FechaLiquidacionFin));

					if (documento.DatosPeriodo.TiempoLaborado <= 0)
						throw new ApplicationException(string.Format("El Tiempo Laborado {0} no es válido", documento.DatosPeriodo.TiempoLaborado));

					//Se agrega validacion para que la nomina que envien sea menor a la fecha actual, puesto que se informa el/los mes anterior al actual
					// si es menor a cero es que la fecha que llega es menor a la actual, si es mayor a cero es que la que llega es mayor a la actual
					int validacion_fecha = DateTime.Compare(documento.DatosPeriodo.FechaLiquidacionFin, Fecha.GetFecha());

					if (validacion_fecha > 0)
					{
						if (documento.DatosPeriodo.FechaLiquidacionFin.Month >= Fecha.GetFecha().Month && documento.DatosPeriodo.FechaLiquidacionFin.Month != 12)
							throw new ApplicationException(string.Format("El mes que reporta {0} no es válido según normatividad de nomina electrónica", documento.DatosPeriodo.FechaLiquidacionFin.Month));
					}
					

					/*
					if (documento.DatosPeriodo.FechaRetiro != null)
					{
						if (DateTime.Equals(documento.DatosPeriodo.FechaRetiro, validar_fecha))
							throw new ApplicationException(string.Format("La FechaRetiro '{0}' no es válida", documento.DatosPeriodo.FechaRetiro));
					}*/

				}
				else
				{
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DatosPeriodo", "Periodo"));
				}

				//Validacion de la informacion del Pago
				if (documento.DatosPago != null)
				{

					if (documento.DatosPago.Forma > 1)
						throw new ApplicationException(string.Format("La Forma de Pago {0} no es válido según Estandar DIAN", documento.DatosPago.Forma));

					ListaFormasPago list_forma = new ListaFormasPago();
					ListaItem forma = list_forma.Items.Where(d => d.Codigo.Equals(documento.DatosPago.Forma.ToString())).FirstOrDefault();
					if (forma == null)
						throw new ApplicationException(string.Format("La Forma de Pago {0} no es válido según Estandar DIAN", documento.DatosPago.Forma));

					ListaMediosPago list_medio = new ListaMediosPago();
					ListaItem medio = list_medio.Items.Where(d => d.Codigo.Equals(documento.DatosPago.Metodo.ToString())).FirstOrDefault();
					if (medio == null || documento.DatosPago.Metodo.Equals(0))
						throw new ApplicationException(string.Format("El Medio de Pago {0} no es válido según Estandar DIAN", documento.DatosPago.Metodo));

					if (medio.Codigo.Equals("42"))
					{
						if (string.IsNullOrEmpty(documento.DatosPago.Banco))
							throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Banco", "string"));

						if (string.IsNullOrEmpty(documento.DatosPago.TipoCuenta))
							throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "TipoCuenta", "string"));

						if (string.IsNullOrEmpty(documento.DatosPago.NumeroCuenta))
							throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroCuenta", "string"));
					}
				}
				else
				{
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DatosPago", "Pago"));
				}

				//******Falta Enumerable o Lita para PeriodoNomina.
				if (documento.PeriodoNomina < PeriodoNomina.Semanal.GetHashCode() || documento.PeriodoNomina > PeriodoNomina.Otro.GetHashCode())
					throw new ApplicationException(string.Format("El PeriodoNomina {0} no es válido según Estandar DIAN", documento.PeriodoNomina));

				//Valida que no este vacio y Formato correcto
				if (string.IsNullOrEmpty(documento.Moneda))
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

				if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
					throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

				if (!documento.Moneda.Equals("COP"))
				{
					if (documento.Trm != null)
					{
						if (documento.Trm.Valor <= 0)
							throw new ApplicationException(string.Format("El campo {0} con valor {1} para informar como tasa de cambio no es valido", "Trm-Valor", documento.Trm.Valor));
					}
					else
					{
						throw new ApplicationException(string.Format("No se encuentra Tasa de Cambio para el documento registrado con la moneda {0}", documento.Moneda));
					}
				}

				if (documento.FechasPagos == null || documento.FechasPagos.Count == 0)
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DateTime", "FechasPagos"));

				//**********Falta Validar que las fechas de pago en cantidad coincidan con el periodo de la nomina (PeriodoNomina) que envio

				//Valida que no este vacio y este bien formado 
				ValidarTrabajador(documento.DatosTrabajador);

				//Valida totales del objeto
				ValidarValoresNomina(documento, tipo_nomina);

			}

			/*
			if (facturador.IntHabilitacion > 0)
			{
				if (documento.DocumentoFormato != null)
				{
					//Valida que envien el titulo del documento y si es vacio lo llena
					if (string.IsNullOrEmpty(documento.DocumentoFormato.Titulo) || documento.DocumentoFormato.Titulo == null)
						documento.DocumentoFormato.Titulo = Enumeracion.GetDescription(TipoDocumento.Nomina).ToUpper();
				}
				else
					throw new ApplicationException("No se encontró información del Formato");
			}*/

			return documento;
		}




	}
}
