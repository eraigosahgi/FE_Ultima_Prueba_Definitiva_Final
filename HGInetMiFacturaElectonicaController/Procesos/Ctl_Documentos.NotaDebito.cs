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
			try
			{
				Ctl_Empresa Peticion = new Ctl_Empresa();

				//Válida que la key sea correcta.
				TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

				if (!facturador_electronico.IntObligado)
					throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));


				//Obtiene la lista de objetos de planes para trabajar(Reserva, procesar, idplan) esto puede generar una lista de objetos, ya que pueda que se requiera mas de un plan
				Ctl_PlanesTransacciones Planestransacciones = new Ctl_PlanesTransacciones();
				List<ObjPlanEnProceso> ListaPlanes = new List<ObjPlanEnProceso>();
				ListaPlanes = Planestransacciones.ObtenerPlanesActivos(documentos[0].DatosObligado.Identificacion, documentos.Count());

				if (ListaPlanes == null)
					throw new ApplicationException("No se encontró saldo disponible para procesar los documentos");

				// genera un id único de la plataforma
				Guid id_peticion = Guid.NewGuid();

				DateTime fecha_actual = Fecha.GetFecha();

				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();

				List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

				string resolucion_pruebas = Constantes.ResolucionPruebas;
				string nit_resolucion = Constantes.NitResolucionsinPrefijo;
				string prefijo_pruebas = string.Empty;

				// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
				if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
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
					// Obtiene las resoluciones de la base de datos
					lista_resolucion = _resolucion.ObtenerResoluciones(documentos.FirstOrDefault().DatosObligado.Identificacion, "*");

				}

				List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

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

				return respuesta;
			}
			catch (Exception ex)
			{
				LogExcepcion.Guardar(ex);
				throw new ApplicationException(ex.Message);
			}
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

			try
			{

				Ctl_Documento num_doc = new Ctl_Documento();

				//valida si el Documento ya existe en Base de Datos
				TblDocumentos numero_documento = num_doc.Obtener(item.DatosObligado.Identificacion, item.Documento, item.Prefijo);

				if (numero_documento != null)
				{
					item_respuesta = Ctl_Documento.Convertir(numero_documento);
					doc_existe = true;
					throw new ApplicationException(string.Format("El documento {0} con prefijo {1} ya xiste para el Facturador Electrónico {2}", item.Documento, item.Prefijo, facturador_electronico.StrIdentificacion));
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
					if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
					{
						tbl_resolucion = Ctl_EmpresaResolucion.Convertir(facturador_electronico.StrIdentificacion, item.Prefijo, TipoDocumento.NotaDebito.GetHashCode());
					}
					else
					{
						tbl_resolucion = Ctl_EmpresaResolucion.Convertir(item.DatosObligado.Identificacion, item.Prefijo, TipoDocumento.NotaDebito.GetHashCode());
					}
					// crea el registro en base de datos
					resolucion = _resolucion.Crear(tbl_resolucion);
					item.NumeroResolucion = resolucion.StrNumResolucion;
				}
				else
				{
					resolucion = resolucion_doc;
					item.NumeroResolucion = resolucion.StrNumResolucion;
				}

				try
				{
					mensaje = Enumeracion.GetDescription(estado);
					_auditoria.Crear(id_radicado, id_peticion, facturador_electronico.StrIdentificacion, proceso_actual, TipoRegistro.Proceso, Procedencia.Plataforma, string.Empty, proceso_txt, mensaje, prefijo, numero);
				}
				catch (Exception) { }

				// realiza el proceso de envío a la DIAN del documento
				item_respuesta = Procesar(id_peticion, id_radicado, item, TipoDocumento.NotaDebito, resolucion, facturador_electronico);

			}
			catch (Exception excepcion)
			{

				//ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
				LogExcepcion.Guardar(excepcion);
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
				}
			}
			if (item_respuesta.Error == null)
				item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error();

			return item_respuesta;
		}


		/// <summary>
		/// Validación del Objeto Nota Debito
		/// </summary>
		/// <param name="documento">Objeto NotaDebito</param>
		/// <returns></returns>
		public static NotaDebito ValidarNotaDebito(NotaDebito documento, TblEmpresasResoluciones resolucion)
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
			if (string.IsNullOrEmpty(documento.DocumentoRef))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

			//Inicializa la propiedad, no es un campo requerido
			if (string.IsNullOrEmpty(documento.PedidoRef))
				documento.PedidoRef = string.Empty;

			//Validar que no este vacia la fecha del documento de referencia
			if (documento.FechaFactura == null)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "FechaFactura", "DateTime"));

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

			//valida el Formato enviado
			if (documento.DocumentoFormato.Codigo == -1 || documento.DocumentoFormato.Codigo == 0)
			{
				if (string.IsNullOrEmpty(documento.DocumentoFormato.ArchivoPdf))
					throw new ArgumentException(string.Format("No se encontró Formato PDF del documento {0} ", documento.Documento));
			}
			else
			{
				//Valida que el codigo del formato que envia este disponible.
				if (documento.DocumentoFormato.Codigo > 5)
					throw new ApplicationException(string.Format("El formato {0} no se encuentra disponible en la plataforma.", documento.DocumentoFormato.Codigo));
			}

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosObligado, "Obligado");

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

			//Valida totales del objeto
			ValidarTotales(null, null, documento, TipoDocumento.NotaDebito);

			//Valida que envien el titulo del documento y si es vacio lo llena
			if (string.IsNullOrEmpty(documento.DocumentoFormato.Titulo) || documento.DocumentoFormato.Titulo == null)
				documento.DocumentoFormato.Titulo = Enumeracion.GetDescription(TipoDocumento.NotaCredito).ToUpper();

			return documento;
		}


	}
}
