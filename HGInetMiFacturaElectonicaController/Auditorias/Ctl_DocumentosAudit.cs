using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaAudit.Controladores;
using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias
{

	public class Ctl_DocumentosAudit //: MongoDBContext<TblAuditDocumentos>
	{
		#region Constructores 

		//public Ctl_DocumentosAudit() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion


		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		public TblAuditDocumentos Crear(TblAuditDocumentos datos)
		{
			try
			{
				Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();
				var data = Srv.Crear(datos);

				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// construye el objeto para el registro de auditoria de un documento.
		/// </summary>
		/// <param name="id_seguridad_doc">id se seguridad del documento.</param>
		/// <param name="id_peticion">id de la petición.</param>
		/// <param name="facturador">Identificación del obligado a facturar</param>
		/// <param name="proceso">Proceso del documento.</param>
		/// <param name="estado">Estado actual del documento.</param>
		/// <param name="tipo_registro">Tipo del registro de auditoría</param>
		/// <param name="procesado_por">Procedencia de la petición del log</param>
		/// <param name="realizado_por">usuario que ejecuta el proceso.</param>
		/// <param name="mensaje">mensaje.</param>
		/// <param name="resultado_proceso">Código Dian, Id Mailjet...entre otros</param>
		/// <returns></returns>
		public TblAuditDocumentos Crear(Guid id_seguridad_doc, Guid id_peticion, string facturador, ProcesoEstado proceso, TipoRegistro tipo_registro, Procedencia procesado_por, string realizado_por, string mensaje, string resultado_proceso, string prefijo, string numero, int estado = 0)
		{
			try
			{
				TblAuditDocumentos datos = new TblAuditDocumentos()
				{
					Id = Guid.NewGuid(),
					StrIdSeguridad = id_seguridad_doc,
					StrIdPeticion = id_peticion.ToString(),
					DatFecha = Fecha.GetFecha(),
					StrObligado = facturador,
					IntIdEstado = (estado == 0) ? Ctl_Documento.ObtenerCategoria(proceso.GetHashCode()) : estado,
					IntIdProceso = proceso.GetHashCode(),
					IntTipoRegistro = tipo_registro.GetHashCode(),
					IntIdProcesadoPor = procesado_por.GetHashCode(),
					StrRealizadoPor = realizado_por,
					StrMensaje = mensaje,
					StrResultadoProceso = resultado_proceso,
					StrPrefijo = prefijo,
					StrNumero = numero
				};

				datos = Crear(datos);

				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// construye el objeto para el registro de auditoria de un documento.
		/// </summary>
		/// <param name="id_seguridad_doc">id se seguridad del documento.</param>
		/// <param name="id_peticion">id de la petición.</param>
		/// <param name="facturador">Identificación del obligado a facturar</param>
		/// <param name="proceso">Proceso del documento.</param>
		/// <param name="estado">Estado actual del documento.</param>
		/// <param name="tipo_registro">Tipo del registro de auditoría</param>
		/// <param name="procesado_por">Procedencia de la petición del log</param>
		/// <param name="realizado_por">usuario que ejecuta el proceso.</param>
		/// <param name="mensaje">mensaje.</param>
		/// <param name="resultado_proceso">Código Dian, Id Mailjet...entre otros</param>
		/// <param name="respuestas_email">Lista de la respuesta de envios de correos</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Crear(Guid id_seguridad_doc, Guid id_peticion, string facturador, ProcesoEstado proceso, TipoRegistro tipo_registro, Procedencia procesado_por, string realizado_por, string mensaje, string resultado_proceso, List<LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta.MensajeEnvio> respuestas_email, string prefijo, string numero, int estado = 0)
		{
			try
			{
				List<TblAuditDocumentos> respuesta_auditoria = new List<TblAuditDocumentos>();

				TblAuditDocumentos datos = new TblAuditDocumentos();

				foreach (var item in respuestas_email)
				{
					foreach (var data in item.Data)
					{
						datos = new TblAuditDocumentos();
						datos.Id = Guid.NewGuid();
						datos.StrIdSeguridad = id_seguridad_doc;
						datos.StrIdPeticion = id_peticion.ToString();
						datos.DatFecha = Fecha.GetFecha();
						datos.StrObligado = facturador;
						datos.IntIdEstado = (estado == 0) ? Ctl_Documento.ObtenerCategoria(proceso.GetHashCode()) : estado;
						datos.IntIdProceso = proceso.GetHashCode();
						datos.IntTipoRegistro = tipo_registro.GetHashCode();
						datos.IntIdProcesadoPor = procesado_por.GetHashCode();
						datos.StrRealizadoPor = realizado_por;
						datos.StrResultadoProceso = resultado_proceso;
						datos.StrPrefijo = prefijo;
						datos.StrNumero = numero;
						datos.StrResultadoProceso = Newtonsoft.Json.JsonConvert.SerializeObject(data);
						datos.StrMensaje = string.Format("{0} : {1}", mensaje, data.Email);
						datos = Crear(datos);
					}
				}


				return respuesta_auditoria;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene todos los registros
		/// </summary>
		/// <returns></returns>
		public List<TblAuditDocumentos> ObtenerTodos()
		{
			try
			{
				Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();
				List<TblAuditDocumentos> registros_audit = Srv.ObtenerTodos();

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los registros por id de seguridad del documento e identificación del obligado
		/// </summary>
		/// <param name="id_seguridad_doc">ID de seguridad del documento</param>
		/// <param name="identificacion_obligado">Número de identificación del obligado.</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Obtener(string id_seguridad_doc, string identificacion_obligado)
		{
			try
			{
				Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();
				List<TblAuditDocumentos> registros_audit = new List<TblAuditDocumentos>();
				registros_audit = Srv.Obtener(id_seguridad_doc, identificacion_obligado);
			
				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public List<TblAuditDocumentos> ObtenerdeMongo(string id_seguridad_doc, string identificacion_obligado)
		{
			try
			{
				List<TblAuditDocumentos> registros_audit = new List<TblAuditDocumentos>();
				Ctl_AuditoriaDocumentos CtlmongoDB = new Ctl_AuditoriaDocumentos();
				var datos = CtlmongoDB.Obtener(id_seguridad_doc, identificacion_obligado);
				foreach (var item in datos)
				{
					TblAuditDocumentos registroAudit = new TblAuditDocumentos();
					registroAudit.DatFecha = item.DatFecha;
					registroAudit.Id = Guid.NewGuid();
					registroAudit.IntIdEstado = item.IntIdEstado;
					registroAudit.IntIdProcesadoPor = item.IntIdProcesadoPor;
					registroAudit.IntIdProceso = item.IntIdProceso;
					registroAudit.IntTipoRegistro = item.IntTipoRegistro;
					registroAudit.StrIdPeticion = item.StrIdPeticion;
					registroAudit.StrIdSeguridad = Guid.Parse(item.StrIdSeguridad);
					registroAudit.StrMensaje = item.StrMensaje;
					registroAudit.StrNumero = item.StrNumero;
					registroAudit.StrObligado = item.StrObligado;
					registroAudit.StrPrefijo = item.StrPrefijo;
					registroAudit.StrRealizadoPor = item.StrRealizadoPor;
					registroAudit.StrResultadoProceso = item.StrResultadoProceso;
					registros_audit.Add(registroAudit);
				}
				return registros_audit;
			}
			catch (Exception)
			{
				//Ctl_Log.Guardar();
				throw;
			}
		}

		/// <summary>
		/// Obtiene los registros por rangos de fecha.
		/// las fechas aplican sobre el campo DatFecha, correspondiente a la fecha de registro de la auditoria
		/// </summary>
		/// <param name="id_seguridad_doc">ID de seguridad del documento</param>
		/// <param name="identificacion_obligado">Número de identificación del obligado.</param>
		/// <param name="fecha_inicio">Fecha inicial del rango de búsqueda</param>
		/// <param name="fecha_fin">Fecha final del rango de búsqueda</param>
		/// <param name="estado">Estado por el cual paso el documento en plataforma</param>
		/// <param name="proceso">Proceso por el cual paso el documento en plataforma</param>
		/// <param name="numero_documento">Busqueda por un Numero de documento especifico</param>
		/// <param name="procedencia">Indica la procedencia de la Auditoria: 1 - Plataforma, 2 - Usuario, 3 - Sonda, 4 - DIAN, 5 - Mail, 6 - Sms</param>
		/// <param name="tipo_registro">Indica que tipo de registro se hizo: 1 - Proceso, 2 - Creacion, 3 - Actualizacion</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Obtener(string numero_documento, string identificacion_obligado, DateTime fecha_inicio, DateTime fecha_fin, string estado, string proceso, string tipo_registro, string procedencia, int Desde, int Hasta)
		{
			try
			{
				Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();

				List<TblAuditDocumentos> registros_audit = Srv.Obtener(numero_documento, identificacion_obligado, fecha_inicio, fecha_fin, estado, proceso, tipo_registro, procedencia, Desde, Hasta);
				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		//public List<TblAuditDocumentos> Obtener(DateTime fecha_inicio, DateTime fecha_fin)
		//{

		//	Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();
		//	List<TblAuditDocumentos> registros_audit = this.GetFilter(x=> x.DatFecha >= fecha_fin).ToList();

		//	return registros_audit;

		//}




		#region Sonda de Email
		/// <summary>
		/// Retorna la respuesta de email de un documento
		/// </summary>
		/// <param name="IdSeguridad">Id de seguridad del documento</param>
		/// <returns></returns>
		public MensajeValidarEmail ObtenerResultadoEmail(Guid IdSeguridad)
		{
			try
			{

				Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();

				List<TblAuditDocumentos> ListaEmail = Srv.ObtenerDocumentoMail(IdSeguridad).ToList();
				MensajeResumen datos_retorno = new MensajeResumen();

				MensajeValidarEmail MailPlataforma = new MensajeValidarEmail();

				Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
				foreach (TblAuditDocumentos Email in ListaEmail)
				{
					try
					{
						if (Email.StrResultadoProceso.Contains("MessageID"))
						{
							email = new Ctl_EnvioCorreos();
							Email.StrResultadoProceso = Email.StrResultadoProceso.Replace("[", "").Replace("]", "");
							dynamic datos = JsonConvert.DeserializeObject(Email.StrResultadoProceso);

							datos_retorno = email.ConsultarCorreo((long)datos.MessageID);
							//Si ya tengo una respuesta se la asigno a este 
							if (MailPlataforma.EmailEnviado == null && datos_retorno.Estado != null)
							{
								MailPlataforma.EmailEnviado = (string)datos.Email;
								MailPlataforma.Estado = datos_retorno.Estado;
								MailPlataforma.IdResultado = datos_retorno.IdResultado;
								MailPlataforma.Recibido = datos_retorno.Recibido;
								if (MailPlataforma.IdResultado == MensajeIdResultado.Entregado.GetHashCode())
								{
									return MailPlataforma;
								}
							}
							else if (datos_retorno.Estado != null)
							{
								MailPlataforma.EmailEnviado = (string)datos.Email;
								MailPlataforma.Estado = datos_retorno.Estado;
								MailPlataforma.IdResultado = datos_retorno.IdResultado;
								MailPlataforma.Recibido = datos_retorno.Recibido;
								if (MailPlataforma.IdResultado == MensajeIdResultado.Entregado.GetHashCode())
								{
									return MailPlataforma;
								}
							}
						}
					}
					catch (Exception)
					{ }
				}

				return MailPlataforma;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		/// <summary>
		/// Obtiene todos los email asociados a un documento
		/// </summary>
		/// <param name="IdSeguridad">identificador unico del documento en la plataforma</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> ObtenerDocumentoMail(Guid IdSeguridad)
		{

			try
			{
				Srv_DocumentosAudit Srv = new Srv_DocumentosAudit();
				List<TblAuditDocumentos> ListaEmail = Srv.ObtenerDocumentoMail(IdSeguridad).ToList();

				return ListaEmail;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
