using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.General;
using HGInetUBL;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using HGInetMiFacturaElectonicaData.Formatos;
using HGInetMiFacturaElectonicaController.Procesos;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_Formatos : BaseObject<TblFormatos>
	{

		public class ObjFormato
		{
			public int CodigoFormato { get; set; }
			public int NitEmpresa { get; set; }
			public string FormatoB64 { get; set; }
			public Guid IdSeguridad { get; set; }
		}
		List<Factura> DatosPlantilla()
		{

			List<Factura> datos_reporte = new List<Factura>();
			Factura datos = new Factura();
			datos.CodigoRegistro = "1";
			datos.Cufe = "c1c2885ac587ef5869e12a04bd06ca89dc9a8868";
			datos.DataKey = "690c8a6436cc20dfe11d159393e5374d1b4fca6c";
			datos.Documento = 990000909;
			datos.DocumentoRef = "1002";
			datos.Fecha = Fecha.GetFecha();
			datos.FechaVence = Fecha.GetFecha();
			datos.Moneda = "COP";
			datos.Neto = 0.00M;
			datos.NumeroResolucion = "9000000045201025";
			datos.Prefijo = "PRE";
			datos.Total = 0.00M;
			datos.Valor = 0.00M;
			datos.ValorDescuento = 0.00M;
			datos.ValorImpuestoConsumo = 0.00M;
			datos.ValorIva = 0.00M;
			datos.ValorReteFuente = 0.00M;
			datos.ValorReteIca = 0.00M;
			datos.ValorReteIva = 0.00M;
			datos.ValorSubtotal = 0.00M;
			datos.DatosObligado = new Tercero();
			datos.DatosAdquiriente = new Tercero();

			Tercero adquiriente = new Tercero();

			Tercero obj_tercero = new Tercero();
			obj_tercero = new Tercero();
			obj_tercero.Ciudad = "Ciudad";
			obj_tercero.CodigoPais = "Pais";
			obj_tercero.Departamento = "Departamento";
			obj_tercero.Direccion = "Dirección Facturador";
			obj_tercero.Email = "facturador@dominio.com";
			obj_tercero.Identificacion = "Identificación Facturador";
			obj_tercero.IdentificacionDv = 0;
			obj_tercero.NombreComercial = "Nombre Comercial Facturador";
			obj_tercero.PaginaWeb = "Página Web Facturador";
			obj_tercero.PrimerApellido = "Primer Apellido";
			obj_tercero.PrimerNombre = "Primer Nombre";
			obj_tercero.RazonSocial = "Razón social Facturador";
			obj_tercero.Regimen = 1;
			obj_tercero.SegundoApellido = "Segundo Apellido";
			obj_tercero.SegundoNombre = "Segundo Nombre";
			obj_tercero.Telefono = "Teléfono";
			obj_tercero.TipoIdentificacion = 13;
			obj_tercero.TipoPersona = 2;
			datos.DatosObligado = obj_tercero;

			//DATOS ADQUIRIENTE
			obj_tercero = new Tercero();
			obj_tercero.Ciudad = "Ciudad";
			obj_tercero.CodigoPais = "Pais";
			obj_tercero.Departamento = "Departamento";
			obj_tercero.Direccion = "Dirección Adquiriente";
			obj_tercero.Email = "adquiriente@dominio.com";
			obj_tercero.Identificacion = "Identificación Adquiriente";
			obj_tercero.IdentificacionDv = 0;
			obj_tercero.NombreComercial = "Nombre Comercial Adquiriente";
			obj_tercero.PaginaWeb = "Página Web";
			obj_tercero.PrimerApellido = "Primer Apellido";
			obj_tercero.PrimerNombre = "Primer Nombre";
			obj_tercero.RazonSocial = "Razón social Adquiriente";
			obj_tercero.Regimen = 1;
			obj_tercero.SegundoApellido = "Segundo Apellido";
			obj_tercero.SegundoNombre = "Segundo Nombre";
			obj_tercero.Telefono = "Teléfono";
			obj_tercero.TipoIdentificacion = 13;
			obj_tercero.TipoPersona = 2;
			datos.DatosAdquiriente = obj_tercero;

			//DETALLES DOCUMENTO
			datos.DocumentoDetalles = new List<DocumentoDetalle>();
			DocumentoDetalle detalles_doc = new DocumentoDetalle();
			detalles_doc.Bodega = "100";
			detalles_doc.Cantidad = 1;
			detalles_doc.Codigo = 1;
			detalles_doc.DescuentoPorcentaje = 0.00M;
			detalles_doc.DescuentoValor = 0.00M;
			detalles_doc.ImpoConsumoPorcentaje = 0.00M;
			detalles_doc.IvaPorcentaje = 0.00M;
			detalles_doc.IvaValor = 0.00M;
			detalles_doc.ProductoCodigo = "1001";
			detalles_doc.ProductoDescripcion = "Producto de venta n.1";
			detalles_doc.ProductoGratis = false;
			detalles_doc.ProductoNombre = "";
			detalles_doc.ReteFuentePorcentaje = 0.00M;
			detalles_doc.ReteFuenteValor = 0.00M;
			detalles_doc.ReteIcaPorcentaje = 0.00M;
			detalles_doc.ReteIcaValor = 0.00M;
			detalles_doc.UnidadCodigo = "Und";
			detalles_doc.ValorImpuestoConsumo = 0.00M;
			detalles_doc.ValorSubtotal = 0.00M;
			detalles_doc.ValorUnitario = 0.00M;
			datos.DocumentoDetalles.Add(detalles_doc);

			datos.DocumentoDetalles.Add(detalles_doc);

			List<FormatoCampo> campos_predeterminados = new List<FormatoCampo>();
			datos.DocumentoFormato = new Formato();
			datos.DocumentoFormato.CamposPredeterminados = new List<FormatoCampo>();
			datos.DocumentoFormato.CamposPredeterminados.AddRange(campos_predeterminados);

			datos_reporte.Add(datos);

			return datos_reporte;
		}

		#region Agregar

		/// <summary>
		/// Almacena el formato en la base de datos
		/// </summary>
		/// <param name="formato"></param>
		/// <returns></returns>
		public TblFormatos Crear(TblFormatos formato)
		{
			formato = this.Add(formato);

			return formato;
		}

		public TblFormatos AlmacenarFormatoPdf(TblFormatos datos_formato, Guid usuario)
		{
			try
			{
				System.Guid id_seguridad = System.Guid.NewGuid();
				datos_formato.DatFechaRegistro = Fecha.GetFecha();
				datos_formato.StrIdSeguridad = id_seguridad;
				datos_formato.IntCodigoFormato = ObtenerIdFormato(datos_formato.StrEmpresa);
				datos_formato.IntTipo = TipoFormato.FormatoPDF.GetHashCode();

				//Crea el registro en base de datos.
				TblFormatos datos_respueta = Crear(datos_formato);

				//Almacena la auditoría del proceso.
				Ctl_FormatosAudit clase_auditoria = new Ctl_FormatosAudit();
				clase_auditoria.Crear(datos_respueta.IntCodigoFormato, datos_respueta.StrEmpresa, datos_respueta.StrIdSeguridad, TiposProceso.Creacion, usuario, datos_respueta.StrObservaciones);

				return datos_respueta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el código consecutivo del formato
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public int ObtenerIdFormato(string identificacion_empresa)
		{
			try
			{
				int formato_resultado = 0;
				context.Configuration.LazyLoadingEnabled = false;
				IEnumerable<TblFormatos> datos = (from formato in context.TblFormatos
												  where formato.StrEmpresa.Equals(identificacion_empresa)
												  select formato);

				if (datos.Count() > 0)
					formato_resultado = datos.Max(x => x.IntCodigoFormato);

				formato_resultado = formato_resultado + 1;

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Actualizar


		public TblFormatos Actualizar(TblFormatos formato)
		{
			formato = this.Edit(formato);

			return formato;
		}

		public TblFormatos ImportarFormato(ObjFormato objeto, Guid usuario)
		{
			try
			{
				TblFormatos datos_formato = Obtener(objeto.IdSeguridad);

				if (!string.IsNullOrWhiteSpace(objeto.FormatoB64))
				{
					try
					{
						byte[] data = Convert.FromBase64String(objeto.FormatoB64);
						datos_formato.FormatoTmp = data;
					}
					catch (Exception)
					{
					}
				}

				datos_formato.IntEstado = (short)EstadosFormato.SolicitarAprobacion.GetHashCode();
				datos_formato.StrUsuarioActualizacion = usuario;
				datos_formato = this.Edit(datos_formato);

				return datos_formato;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Actualiza los datos del formato (diseño)
		/// </summary>
		/// <param name="id_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="byte_formato"></param>
		/// <param name="tipo_formato"></param>
		/// <returns></returns>
		public TblFormatos ActualizarFormato(int id_formato, string identificacion_empresa, byte[] byte_formato, int tipo_formato, Guid usuario)
		{
			try
			{
				TblFormatos datos_formato = Obtener(id_formato, identificacion_empresa, tipo_formato);
				datos_formato.DatFechaActualizacion = Fecha.GetFecha();
				datos_formato.StrUsuarioActualizacion = usuario;
				datos_formato.FormatoTmp = byte_formato;
				datos_formato.IntEstado = (short)EstadosFormato.SolicitarAprobacion.GetHashCode();
				//Actualiza el registro en base de datos.
				TblFormatos datos_respueta = Actualizar(datos_formato);

				//Almacena la auditoría del proceso.
				Ctl_FormatosAudit clase_auditoria = new Ctl_FormatosAudit();
				clase_auditoria.Crear(datos_respueta.IntCodigoFormato, datos_respueta.StrEmpresa, datos_respueta.StrIdSeguridad, TiposProceso.Edicion, usuario, string.Format("Actualización de Diseño. {0}", datos_respueta.StrObservaciones));

				return datos_respueta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Actualiza el estado del formato Activo/Inactivo
		/// </summary>
		/// <param name="id_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="estado_actual"></param>
		/// <param name="tipo_formato"></param>
		/// <returns></returns>
		public TblFormatos ActualizarEstadoFormato(int id_formato, string identificacion_empresa, int estado_actual, int tipo_formato, TblEmpresas empresa_solicitante, TblUsuarios usuario_solicitante, TiposProceso tipo_proceso, string observaciones)
		{
			try
			{
				TblFormatos datos_formato = Obtener(id_formato, identificacion_empresa, tipo_formato);
				datos_formato.DatFechaActualizacion = Fecha.GetFecha();

				bool notifica = false;
				TipoAlerta tipo_alerta = TipoAlerta.SolicitudAprobacionFormato;

				string des_proceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TiposProceso>(tipo_proceso.GetHashCode()));
				EstadosFormato estado_formato = Enumeracion.GetEnumObjectByValue<EstadosFormato>(estado_actual);
				switch (tipo_proceso)
				{
					//Estado de edición del formato.
					case TiposProceso.Edicion:
						datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.SolicitarAprobacion.GetHashCode());
						break;

					//Cambio de estados del formato Activo - Inactivo.
					case TiposProceso.CambioEstado:
						//Cambia Activo - Inactivo

						//Valida el estado en el cual se encuentra el formato actualmente.
						if (estado_formato.GetHashCode() == EstadosFormato.Activo.GetHashCode())
							datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.Inactivo.GetHashCode());

						else if (estado_formato.GetHashCode() == EstadosFormato.Inactivo.GetHashCode())
							datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.Activo.GetHashCode());

						break;

					//Estado pendiente de aprobación de un diseño.
					case TiposProceso.SolicitudAprobacion:
						datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.PendienteAprobacion.GetHashCode());
						//envió de notificacion
						notifica = true;
						tipo_alerta = TipoAlerta.SolicitudAprobacionFormato;
						break;

					//Aprobación del diseño.
					case TiposProceso.Aprobacion:
						datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.PendientePublicacion.GetHashCode());
						notifica = true;
						tipo_alerta = TipoAlerta.AprobacionFormato;
						break;

					//Rechazo del diseño.
					case TiposProceso.Rechazo:
						datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.SolicitarAprobacion.GetHashCode());
						notifica = true;
						tipo_alerta = TipoAlerta.AprobacionFormato;
						break;

					//Publicación del formato
					case TiposProceso.Publicacion:
						datos_formato.IntEstado = Convert.ToInt16(EstadosFormato.Activo.GetHashCode());
						datos_formato.Formato = datos_formato.FormatoTmp;
						datos_formato.FormatoTmp = null;
						notifica = true;
						tipo_alerta = TipoAlerta.PublicacionFormato;
						break;

				}


				if (notifica)
				{
					GestionarNotificacion(tipo_alerta, tipo_proceso, datos_formato, empresa_solicitante, usuario_solicitante, observaciones);
				}


				//Almacena la auditoría del proceso.
				Ctl_FormatosAudit clase_auditoria = new Ctl_FormatosAudit();
				clase_auditoria.Crear(datos_formato.IntCodigoFormato, datos_formato.StrEmpresa, datos_formato.StrIdSeguridad, tipo_proceso, usuario_solicitante.StrIdSeguridad, string.Format("{0}. {1}", des_proceso, observaciones));

				//Actualiza el registro en base de datos.
				TblFormatos datos_respuesta = Actualizar(datos_formato);



				return datos_respuesta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Obtener

		public TblFormatos Obtener(Guid IdSeguridad)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				TblFormatos formato_resultado = (from formato in context.TblFormatos
												 where formato.StrIdSeguridad == IdSeguridad
												 select formato).FirstOrDefault();

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el formato por código e identificación de la empresa
		/// </summary>
		/// <param name="id_formato">código del formato</param>
		/// <param name="identificacion_empresa">número de identificación de la empresa</param>
		/// <returns></returns>
		public TblFormatos Obtener(int id_formato, string identificacion_empresa, int tipo_formato)
		{
			try
			{
				TblFormatos formato_resultado = (from formato in context.TblFormatos
												 where formato.IntCodigoFormato == id_formato
												 && formato.StrEmpresa.Equals(identificacion_empresa)
												 && formato.IntTipo == tipo_formato
												 select formato).FirstOrDefault();

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public string ConvertirFormatoXml(byte[] formato)
		{
			try
			{
				if (formato == null)
					throw new ApplicationException(string.Format("No se encontró un diseño para el formato {0}.", formato));

				string formato_xml = string.Empty;
				using (var memoryStream = new MemoryStream(formato))
				using (var reader = new StreamReader(memoryStream))
				{
					formato_xml = reader.ReadToEnd();
				}

				return formato_xml;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Obtiene el formato del facturador o de la empresa de dependecia. 
		/// </summary>
		/// <param name="id_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_formato"></param>
		/// <returns></returns>
		public TblFormatos ObtenerFormato(int id_formato, string identificacion_empresa, int tipo_formato, TipoDocumento tipo_doc)
		{
			try
			{
				string empresa_dependencia = string.Empty;

				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				empresa_dependencia = clase_empresa.Obtener(identificacion_empresa).StrEmpresaAsociada;

				int estado_formato = EstadosFormato.Inactivo.GetHashCode();

				TblFormatos formato_resultado = null;

				formato_resultado = (from formato in context.TblFormatos
									 where formato.IntCodigoFormato == id_formato
										   && formato.IntTipo == tipo_formato
										   && formato.StrEmpresa.Equals(identificacion_empresa)
										   && formato.IntEstado != estado_formato
									 select formato).FirstOrDefault();

				if (formato_resultado == null && !identificacion_empresa.Equals(empresa_dependencia))
				{

					formato_resultado = (from formato in context.TblFormatos
										 where formato.IntCodigoFormato == id_formato
											   && formato.IntTipo == tipo_formato
											   && formato.StrEmpresa.Equals(empresa_dependencia)
											   && formato.IntEstado != estado_formato
										 select formato).FirstOrDefault();
				}

				//Si no se obtiene Formato con las Condiciones enviadas, se hace con el formato generico de HGI segun el tipo de documento
				if (formato_resultado == null)
				{
					int codigo_formato = 1;
					switch (tipo_doc)
					{
						case TipoDocumento.Factura:
							codigo_formato = 1;
							break;
						case TipoDocumento.NotaDebito:
							codigo_formato = 2;
							break;
						case TipoDocumento.NotaCredito:
							codigo_formato = 2;
							break;
						default:
							codigo_formato = 1;
							break;
					}

					formato_resultado = (from formato in context.TblFormatos
										 where formato.IntCodigoFormato == codigo_formato
										 && formato.IntTipo == tipo_formato
										 && (formato.StrEmpresa.Equals("811021438"))
										 && formato.IntEstado != estado_formato
										 select formato).FirstOrDefault();

				}

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		///  Obtiene los formatos de la empresa
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_formato">indica si es plantilla pdf (1) -- plantilla html (2)</param>
		/// <returns></returns>
		public List<TblFormatos> ObtenerFormatosEmpresa(string identificacion_empresa, int tipo_formato)
		{
			try
			{
				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				TblEmpresas datos_empresa = clase_empresa.Obtener(identificacion_empresa);

				List<TblFormatos> listado_formatos = new List<TblFormatos>();

				List<string> empresas_asociadas = new List<string>();
				empresas_asociadas.Add(identificacion_empresa);

				if (datos_empresa.IntAdministrador)
				{
					return listado_formatos = (from formato in context.TblFormatos
											   where formato.IntTipo == tipo_formato
											   select formato).ToList();
				}
				else if (datos_empresa.IntIntegrador)
				{
					empresas_asociadas = clase_empresa.ObtenerAsociadas(identificacion_empresa).Select(x => x.StrIdentificacion).ToList();

					foreach (string item in empresas_asociadas)
					{
						listado_formatos.AddRange((from formato in context.TblFormatos
												   where formato.StrEmpresa.Equals(item)
													&& formato.IntTipo == tipo_formato
												   select formato).ToList());
					}
				}
				else if (datos_empresa.IntObligado && !datos_empresa.IntIntegrador)
				{
					//estados visibles para el obligado
					List<int> estados_formato = new List<int>(){
					EstadosFormato.Activo.GetHashCode(),
					EstadosFormato.SolicitarAprobacion.GetHashCode(),
					EstadosFormato.PendienteAprobacion.GetHashCode(),
					EstadosFormato.Aprobado.GetHashCode()
					};

					listado_formatos = (from formato in context.TblFormatos
										where formato.StrEmpresa.Equals(identificacion_empresa)
										&& formato.IntTipo == tipo_formato
										&& estados_formato.Contains(formato.IntEstado)
										select formato).ToList();
				}

				listado_formatos.AddRange((from formato in context.TblFormatos
										   where formato.IntGenerico == true
											&& formato.IntTipo == tipo_formato
										   select formato).ToList());

				return listado_formatos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Notificaciones

		public bool GestionarNotificacion(TipoAlerta tipo_alerta, TiposProceso tipo_proceso, TblFormatos datos_formato, TblEmpresas empresa_autenticada, TblUsuarios datos_usuario, string observaciones_solicitud)
		{
			try
			{
				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				Ctl_Alertas clase_alertas = new Ctl_Alertas();
				Ctl_Usuario clase_usuario = new Ctl_Usuario();
				Ctl_EnvioCorreos clase_correos = new Ctl_EnvioCorreos();
				List<DestinatarioEmail> lista_difusion = new List<DestinatarioEmail>();

				//List<DestinatarioEmail> correos_destino = new List<DestinatarioEmail>();
				string correos_destino = string.Empty;

				//Obtiene los datos de la alerta configurada para el proceso solicitado.
				TblAlertas datos_alerta = clase_alertas.ObtenerPorTipo(tipo_alerta.GetHashCode());
				TblEmpresas datos_empresa_formato = clase_empresa.Obtener(datos_formato.StrEmpresa);

				if (datos_alerta != null)
				{
					// empresa solicita, empresa formato, usuario solicita
					// interno         , cliente        , usuario 

					/*
					 Interno, mails registrados en alerta.
					 Cliente, mails empresa edita y titular formato.
					 usuario, solicitud ->autenticado - otros -> utlimo editor.						 
					 */
					DestinatarioEmail destinatario = new DestinatarioEmail();

					if (datos_alerta.IntInterno)
					{
						if (!string.IsNullOrWhiteSpace(datos_alerta.StrInternoMails))
							correos_destino = datos_alerta.StrInternoMails;

						if (datos_alerta.StrInternoMails.Contains(";"))
						{
							List<string> mails_internos = Coleccion.ConvertirLista(datos_alerta.StrInternoMails, ';');

							foreach (var item_mail in mails_internos)
							{
								//Contruye la lista de destinatarios
								destinatario = new DestinatarioEmail();
								destinatario.Nombre = "ADMINISTRACIÓN";
								destinatario.Email = item_mail;
								lista_difusion.Add(destinatario);
							}
						}
						else
						{
							destinatario = new DestinatarioEmail();
							destinatario.Nombre = "ADMINISTRACIÓN";
							destinatario.Email = datos_alerta.StrInternoMails;
							lista_difusion.Add(destinatario);
						}

						correos_destino = datos_alerta.StrInternoMails;
					}

					if (datos_alerta.IntCliente)
					{
						//Obtiene los datos de la empresa asociada al formato.

						destinatario = new DestinatarioEmail();
						destinatario.Nombre = datos_empresa_formato.StrRazonSocial;
						destinatario.Email = datos_empresa_formato.StrMailAdmin;
						lista_difusion.Add(destinatario);

						//valida que la empresa autenticada sea diferente de la asociada al formato.
						if (!empresa_autenticada.StrIdentificacion.Equals(datos_formato.StrEmpresa))
						{
							destinatario = new DestinatarioEmail();
							destinatario.Nombre = empresa_autenticada.StrRazonSocial;
							destinatario.Email = empresa_autenticada.StrMailAdmin;
							lista_difusion.Add(destinatario);
						}
					}

					if (datos_alerta.IntUsuario)
					{
						if (tipo_proceso.GetHashCode() == TiposProceso.SolicitudAprobacion.GetHashCode())
						{
							destinatario = new DestinatarioEmail();
							destinatario.Nombre = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);
							destinatario.Email = datos_usuario.StrMail;
							lista_difusion.Add(destinatario);
						}
						else
						{
							if (datos_formato.StrUsuarioActualizacion != null)
							{
								datos_usuario = clase_usuario.ObtenerIdSeguridad(datos_formato.StrUsuarioActualizacion.Value);

								destinatario = new DestinatarioEmail();
								destinatario.Nombre = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);
								destinatario.Email = datos_usuario.StrMail;
								lista_difusion.Add(destinatario);
							}
						}
					}
				}

				if (lista_difusion.Count > 0)
				{
					List<MensajeEnvio> respuesta_envio = clase_correos.EnviarNotificacionProcesosFormato(empresa_autenticada, datos_empresa_formato, datos_formato, datos_usuario, observaciones_solicitud, tipo_proceso, lista_difusion);

					Ctl_AlertasHistAudit clase_audit_alerta = new Ctl_AlertasHistAudit();
					string mensaje_audit = string.Format("{0} {1}", observaciones_solicitud, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TiposProceso>(tipo_proceso.GetHashCode())));

					clase_audit_alerta.Crear(datos_alerta.IntIdAlerta, empresa_autenticada.StrIdSeguridad, empresa_autenticada.StrIdentificacion, Newtonsoft.Json.JsonConvert.SerializeObject(respuesta_envio), datos_alerta.IntTipo, Guid.Empty, mensaje_audit);
				}

				return true;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Envío mail prueba

		/// <summary>
		/// Realiza el envío del formato de prueba.
		/// </summary>
		/// <param name="empresa_autenticada"></param>
		/// <param name="id_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_formato"></param>
		/// <param name="email_destino"></param>
		/// <returns></returns>
		public bool EnviarFormatoPrueba(TblEmpresas empresa_autenticada, int id_formato, string identificacion_empresa, int tipo_formato, string email_destino, string empresa_documento, string prefijo, string numero_documento)
		{
			try
			{
				TblFormatos datos_formato = Obtener(id_formato, identificacion_empresa, tipo_formato);
				XtraReportDesigner report = new XtraReportDesigner();

				byte[] formato = null;
				if (datos_formato.FormatoTmp != null)
					formato = datos_formato.FormatoTmp;
				else
					formato = datos_formato.Formato;

				MemoryStream datos = new MemoryStream(formato);
				report.LoadLayoutFromXml(datos);

				//Obtiene los datos del último documento generado
				Ctl_Documento clase_documento = new Ctl_Documento();
				TblDocumentos datos_doc_bd = clase_documento.ObtenerParaPrueba(empresa_documento, numero_documento, prefijo);

				TipoDocumento tipo_documento = Enumeracion.GetEnumObjectByValue<TipoDocumento>(datos_doc_bd.IntDocTipo);

				var documento_obj = (dynamic)null;

				if (datos_doc_bd != null)
				{
					string contenido_xml = Archivo.ObtenerContenido(datos_doc_bd.StrUrlArchivoUbl);

					// valida el contenido del archivo
					if (string.IsNullOrWhiteSpace(contenido_xml))
						throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

					// convierte el contenido de texto a xml
					XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));

					// convierte el objeto de acuerdo con el tipo de documento
					XmlSerializer serializacion = null;
					if (datos_doc_bd.IntVersionDian == 1)
					{
						documento_obj = new Factura();
						serializacion = new XmlSerializer(typeof(HGInetUBL.InvoiceType));
						HGInetUBL.InvoiceType conversion = (HGInetUBL.InvoiceType)serializacion.Deserialize(xml_reader);
						documento_obj = FacturaXML.Convertir(conversion, datos_doc_bd);
					}
					else
					{

						if (tipo_documento == TipoDocumento.Factura)
						{
							serializacion = new XmlSerializer(typeof(InvoiceType));
							InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);
							documento_obj = HGInetUBLv2_1.FacturaXMLv2_1.Convertir(conversion, datos_doc_bd);
							if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(datos_doc_bd.StrFormato))
								documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(datos_doc_bd.StrFormato);

							//Convierte en archivo json a objeto factura y sobre escribe el que se obtuvo
							//Esto es para pruebas si se requiere generar un formato con informacion especifica
							//Factura obj_nc = new Factura();
							//string objeto = System.IO.File.ReadAllText(@"E:\Desarrollo\jzea\Proyectos\HGInetMiFacturaElectronica\Codigo\HGInetMiFacturaElectronicaWeb\dms\Debug\811021438-SETP-990000005.json").ToString();
							//obj_nc = JsonConvert.DeserializeObject<Factura>(objeto);
							//documento_obj = obj_nc;
						}
						else if (tipo_documento == TipoDocumento.NotaCredito)
						{
							serializacion = new XmlSerializer(typeof(CreditNoteType));
							CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);
							documento_obj = HGInetUBLv2_1.NotaCreditoXMLv2_1.Convertir(conversion, datos_doc_bd);
							if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(datos_doc_bd.StrFormato))
								documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(datos_doc_bd.StrFormato);
						}
						else if (tipo_documento == TipoDocumento.NotaDebito)
						{
							serializacion = new XmlSerializer(typeof(DebitNoteType));
							DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

							documento_obj = HGInetUBLv2_1.NotaDebitoXMLv2_1.Convertir(conversion, datos_doc_bd);
							if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(datos_doc_bd.StrFormato))
								documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(datos_doc_bd.StrFormato);
						}
						else if (tipo_documento == TipoDocumento.Nomina)
						{
							serializacion = new XmlSerializer(typeof(NominaIndividualType));
							NominaIndividualType conversion = (NominaIndividualType)serializacion.Deserialize(xml_reader);

							documento_obj = HGInetUBLv2_1.NominaXML.Convertir(conversion, datos_doc_bd);
							if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(datos_doc_bd.StrFormato))
								documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(datos_doc_bd.StrFormato);
						}
						else if (tipo_documento == TipoDocumento.NominaAjuste)
						{
							serializacion = new XmlSerializer(typeof(NominaIndividualDeAjusteType));
							NominaIndividualDeAjusteType conversion = (NominaIndividualDeAjusteType)serializacion.Deserialize(xml_reader);

							documento_obj = HGInetUBLv2_1.NominaAjusteXML.Convertir(conversion, datos_doc_bd);
							if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(datos_doc_bd.StrFormato))
								documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(datos_doc_bd.StrFormato);
						}
					}
				}
				else
				{
					documento_obj = new Factura();
					documento_obj.Documento = numero_documento;
					documento_obj.Prefijo = prefijo;
				}

				if (tipo_documento == TipoDocumento.Nomina || tipo_documento == TipoDocumento.NominaAjuste)
				{
					Planilla objeto_planilla_nomina = new Planilla();
					objeto_planilla_nomina = Ctl_Documentos.convertirObjetoPlanilla(documento_obj, tipo_documento);
					report.DataSource = objeto_planilla_nomina;
				}
				else
				{
					report.DataSource = documento_obj;
				}

				string nombre_archivo = NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), datos_formato.StrEmpresa, TipoDocumento.Factura, documento_obj.Prefijo);

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del pdf.
				string ruta_archivo = string.Format("{0}\\{1}\\{2}\\{3}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, datos_formato.TblEmpresas.StrIdSeguridad.ToString(), RecursoDms.CarpetaFormatosPdf);
				// valida la existencia de la carpeta
				ruta_archivo = Directorio.CrearDirectorio(ruta_archivo);

				//Exporta el formato pdf.
				HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(nombre_archivo, ruta_archivo);
				x.GenerarPdfDev(report, empresa_autenticada.StrIdentificacion);

				string url_archivo = string.Format("{0}/{1}/{2}/{3}/{4}.pdf", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, datos_formato.TblEmpresas.StrIdSeguridad.ToString(), RecursoDms.CarpetaFormatosPdf, nombre_archivo);
				byte[] bytes_pdf = Archivo.ObtenerWeb(url_archivo);
				string ruta_fisica_pdf = Convert.ToBase64String(bytes_pdf);

				List<Adjunto> adjuntos = new List<Adjunto>();

				if (!string.IsNullOrEmpty(ruta_fisica_pdf))
				{
					Adjunto adjunto = new Adjunto();
					adjunto.ContenidoB64 = ruta_fisica_pdf;
					adjunto.Nombre = Path.GetFileName(url_archivo);
					adjuntos.Add(adjunto);
				}

				Ctl_EnvioCorreos clase_correos = new Ctl_EnvioCorreos();

				List<MensajeEnvio> respuesta_email = clase_correos.EnvioFormatoPrueba(empresa_autenticada, datos_formato, adjuntos, email_destino);

				return true;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

	}
}
