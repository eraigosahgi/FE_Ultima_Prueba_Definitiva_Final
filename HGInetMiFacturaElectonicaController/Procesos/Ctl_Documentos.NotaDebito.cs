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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Procesa una lista de documentos tipo NotaDebito
		/// </summary>
		/// <param name="documentos">documentos tipo NotaDebito</param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<NotaDebito> documentos)
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

				//Obtiene la lista de objetos de planes para trabajar(Reserva, procesar, idplan) esto puede generar una lista de objetos, ya que pueda que se requiera mas de un plan
				ListaPlanes = Planestransacciones.ObtenerPlanesActivos(documentos[0].DatosObligado.Identificacion, documentos.Count());

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
						lista_resolucion = _resolucion.ObtenerResoluciones(facturador_electronico.StrIdentificacion, "*");
						foreach (var item in documentos)
						{
							item.DatosAdquiriente.Email = facturador_electronico.StrMailAdmin;

						}

					}

				}
				else
				{
					// Obtiene las resoluciones de la base de datos
					lista_resolucion = _resolucion.ObtenerResoluciones(documentos.FirstOrDefault().DatosObligado.Identificacion, "*");

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
				Parallel.ForEach<NotaDebito>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion);
					respuesta.Add(item_respuesta);
				});
				////Planes y transacciones
				foreach (ObjPlanEnProceso plan in ListaPlanes)
				{
					plan.procesado = respuesta.Where(x => x.IdPlan == plan.plan).Where(x => x.DescuentaSaldo = true).Count();

					Planestransacciones.ConciliarPlanProceso(plan);
				}
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
					foreach (ObjPlanEnProceso plan in ListaPlanes)
					{
						plan.procesado = respuesta.Where(x => x.IdPlan == plan.plan).Where(x => x.DescuentaSaldo == true).Count();

						Planestransacciones.ConciliarPlanProceso(plan);
					}
				}
				catch (Exception)
				{
					Ctl_Log.Guardar(ex, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
				}

				string mensaje = ex.Message;
				Parallel.ForEach<NotaDebito>(documentos, item =>
				{
					DocumentoRespuesta item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
						DocumentoTipo = TipoDocumento.NotaDebito.GetHashCode(),
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
		/// <param name="item">objeto de datos Nota Debito</param>
		/// <param name="facturador_electronico">facturador electrónico del documento</param>
		/// <param name="id_peticion">identificador de petición</param>
		/// <param name="fecha_actual">fecha actual de recepción del documento</param>
		/// <param name="lista_resolucion">resoluciones habilitadas para el facturador electrónico</param>
		/// <returns>resultado del proceso</returns>
		private static DocumentoRespuesta Procesar(NotaDebito item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion)
		{
			TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

			Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

			DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

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
						else if (numero_documento.IntIdEstado == ProcesoEstado.ProcesoPausadoPlataformaDian.GetHashCode())
						{
							// procesa el documento en V2
							item_respuesta = ProcesarV2(numero_documento, true);
							return item_respuesta;
						}
						else
						{
							mensaje = string.Format("El documento '{0}' con prefijo '{1}' ya existe para el Facturador Electrónico '{2}'", item.Documento, prefijo, facturador_electronico.StrIdentificacion);
							throw new ApplicationException(mensaje);
						}
					}
				}

				// filtra la resolución del documento con las condiciones de nit, prefijo y tipo de documento
				TblEmpresasResoluciones resolucion_doc = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion) &&
																			_resolucion_doc.StrPrefijo.Equals(item.Prefijo)
																			&& _resolucion_doc.IntTipoDoc == TipoDocumento.NotaDebito.GetHashCode()).FirstOrDefault();
				//si no existe la resolucion la crea
				if (resolucion_doc == null)
				{
					//Se crea Resolucion
					TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();
					if (facturador_electronico.IntVersionDian == 1)
					{

						if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
						{
							tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.NotaDebito.GetHashCode(), facturador_electronico.IntVersionDian);
						}
						else
						{
							tbl_resolucion = Ctl_EmpresaResolucion.Convertir(item.DatosObligado.Identificacion, item.Prefijo, TipoDocumento.NotaDebito.GetHashCode(), facturador_electronico.IntVersionDian);
						}
					}
					else
					{
						tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.NotaDebito.GetHashCode(), facturador_electronico.IntVersionDian);

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
					//aplicarán el conjunto de validaciones correspondiente
					if (item.TipoOperacion == 0)
						item.TipoOperacion = 30;

					//valida si envian documento a afectar
					if (!string.IsNullOrEmpty(item.DocumentoRef))
					{
						//valida si el Documento afectado ya existe en Base de Datos
						List<DocumentoRespuesta> doc_ref = num_doc.ConsultaPorNumeros(facturador_electronico.StrIdentificacion, TipoDocumento.Factura.GetHashCode(), item.DocumentoRef);
						if (doc_ref != null)
						{
							DocumentoRespuesta doc_resp = doc_ref.Where(d => d.Cufe.Equals(item.CufeFactura)).FirstOrDefault();
							if (doc_resp != null)
							{
								//Si el documento afectado es diferente a la version de la empresa emisora se cambia el tìpo de operacion
								if (doc_resp.IdVersionDian == 1)
									item.TipoOperacion = 33;
								//throw new ApplicationException(string.Format("El número de Factura afectada {0} no es válida para la Versión que se esta enviando", item.DocumentoRef));
							}
							else
							{
								doc_resp = doc_ref.Where(d => d.Identificacion.Equals(item.DatosAdquiriente.Identificacion)).FirstOrDefault();
								if (doc_resp != null)
								{
									//Si el documento afectado es diferente a la version de la empresa emisora se cambia el tìpo de operacion
									if (doc_resp.IdVersionDian == 1)
									{
										item.TipoOperacion = 33;
										item.CufeFactura = doc_resp.Cufe;
									}

									//throw new ApplicationException(string.Format("El número de Factura afectada {0} no es válida para la Versión que se esta enviando", item.DocumentoRef));
								}
								else
								{
									//si el documento afectado no existe en BD pero lo envian con CUFE cambio el tipo de operacion
									if (!string.IsNullOrEmpty(item.CufeFactura))
									{
										item.TipoOperacion = 33;
									}

									//si el documento afectado no existe en BD y no envian el CUFE cambio el tipo de operacion
									item.TipoOperacion = 32;
								}

								//throw new ApplicationException(string.Format("El número de Factura afectada {0} no se encuentra registrada", item.DocumentoRef));
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(item.CufeFactura))
							{
								item.TipoOperacion = 33;
							}

							item.TipoOperacion = 32;
							//throw new ApplicationException(string.Format("El número de Factura afectada {0} no se encuentra registrada", item.DocumentoRef));
						} 
					}
					else
					{
						item.TipoOperacion = 32;
					}

				}

				try
				{
					mensaje = Enumeracion.GetDescription(estado);
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception) { }

				if (facturadorelec_proceso.IntVersionDian == 1)
				{
					// realiza el proceso de envío a la DIAN del documento en V1
					item_respuesta = Procesar(id_peticion, id_radicado, item, TipoDocumento.NotaDebito, resolucion,
						facturadorelec_proceso);
				}
				else
				{
					// realiza el proceso de envío a la DIAN del documento en V2
					item_respuesta = Procesar_v2(id_peticion, id_radicado, item, TipoDocumento.NotaDebito, resolucion,
						facturadorelec_proceso, documento_bd);
				}
			}
			catch (Exception excepcion)
			{
				mensaje = string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message);

				try
				{
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception ex)
				{
					Ctl_Log.Guardar(ex, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
				}
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				if (!doc_existe)
				{
					item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = 0,
						CodigoRegistro = item.CodigoRegistro,
						Cufe = "",
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						DocumentoTipo = TipoDocumento.NotaDebito.GetHashCode(),
						Documento = item.Documento,
						Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
						EstadoDian = null,
						FechaRecepcion = fecha_actual,
						FechaUltimoProceso = fecha_actual,
						IdDocumento = "",
						Identificacion = "",
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = "",
						NumeroResolucion = item.NumeroResolucion,
						Prefijo = "",
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
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
			//Si el estado es menor a firmado, la respuesta del estado siempre va ser no recibido
			if (item_respuesta.IdProceso < (short)ProcesoEstado.FirmaXml.GetHashCode())
			{
				item_respuesta.IdProceso = (short)ProcesoEstado.Validacion.GetHashCode();
				item_respuesta.IdEstado = (short)CategoriaEstado.NoRecibido.GetHashCode();
			}

			return item_respuesta;
		}


		/// <summary>
		/// Validación del Objeto Nota Debito
		/// </summary>
		/// <param name="documento">Objeto NotaDebito</param>
		/// <returns></returns>
		public static NotaDebito ValidarNotaDebito(NotaDebito documento, TblEmpresasResoluciones resolucion, TblEmpresas facturador)
		{
			// valida objeto recibido
			if (documento == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Nota Débito"));

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
			if (string.IsNullOrEmpty(documento.DocumentoRef) && documento.TipoOperacion == 30)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

			//Inicializa la propiedad, no es un campo requerido
			if (string.IsNullOrEmpty(documento.PedidoRef))
				documento.PedidoRef = string.Empty;

			//Validar que no este vacia la fecha del documento de referencia
			if (documento.FechaFactura == null && documento.TipoOperacion == 30)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "FechaFactura", "DateTime"));

			if (string.IsNullOrEmpty(documento.CufeFactura) && documento.TipoOperacion == 30)
				throw new ArgumentException("El Cufe de la Factura afectada no esta bien formado");

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
			if (facturador.IntVersionDian == 1)
			{
				if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
					throw new ApplicationException(string.Format("La fecha de elaboración {0} no está dentro los términos.", documento.Fecha));
			}
			else
			{
				if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-5).Date || documento.Fecha.Date > Fecha.GetFecha().Date.AddDays(10))
					throw new ApplicationException(string.Format("La fecha de elaboración {0} no está dentro los términos.", documento.Fecha));

				ListaConceptoNotaDebito list_concepto = new ListaConceptoNotaDebito();
				ListaItem concepto = list_concepto.Items.Where(d => d.Codigo.Equals(documento.Concepto)).FirstOrDefault();
				if (concepto == null)
					throw new ApplicationException(string.Format("El concepto {0} no es válido según Estandar DIAN", documento.Concepto));

				/*
				Ctl_Documento num_doc = new Ctl_Documento();

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
			ValidarTotales(null, null, documento, TipoDocumento.NotaDebito, facturador);

			if (facturador.IntHabilitacion > 0)
			{
				if (documento.DocumentoFormato != null)
				{
					//Valida que envien el titulo del documento y si es vacio lo llena
					if (string.IsNullOrEmpty(documento.DocumentoFormato.Titulo) || documento.DocumentoFormato.Titulo == null)
						documento.DocumentoFormato.Titulo = Enumeracion.GetDescription(TipoDocumento.NotaDebito).ToUpper();
				}
				else
					throw new ApplicationException("No se encontró información del Formato");
			}

			return documento;
		}


	}
}
