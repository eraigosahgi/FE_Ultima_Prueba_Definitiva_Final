using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_EventosRadian : BaseObject<TblEventosRadian>
	{
		#region Constructores 

		public Ctl_EventosRadian() : base(new ModeloAutenticacion()) { }
		public Ctl_EventosRadian(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_EventosRadian(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		/// <summary>
		/// Obtener todos los eventos de un documento
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <param name="LazyLoading"></param>
		/// <returns></returns>
		public List<TblEventosRadian> Obtener(Guid IdSeguridad, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblEventosRadian> datos = (from item in context.TblEventosRadian
									   where item.StrIdSeguridadDoc.Equals(IdSeguridad)
									   select item).ToList();

			return datos;
		}


		public TblEventosRadian Convertir(Guid doc_StrIdSeguridad, short tipo_evento, long numero_evento, DateTime fecha_evento)
		{
			TblEventosRadian evento = new TblEventosRadian();

			evento.StrIdSeguridadDoc = doc_StrIdSeguridad;
			evento.IntEstadoEvento = tipo_evento;
			evento.IntNumeroEvento = numero_evento;
			evento.DatFechaEvento = fecha_evento;

			return evento;
		}


		public TblEventosRadian Crear(TblEventosRadian evento)
		{
			evento = this.Add(evento);

			return evento;
		}

		public TblEventosRadian Actualizar(TblEventosRadian evento)
		{
			evento = this.Edit(evento);

			return evento;
		}


		public async Task ProcesoCrearAcuseRecibo(string StrIdMensaje, Guid StrIdSeguridadDoc)
		{
			try
			{
				var Tarea = TareaProcesoCrearAcuseRecibo(StrIdMensaje, StrIdSeguridadDoc);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				//Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.alarma);
			}
		}


		/// <summary>
		/// Tarea asincrona para crear planes post pago automaticamente cada mes
		/// </summary>
		/// <returns></returns>
		public async Task TareaProcesoCrearAcuseRecibo(string StrIdMensaje, Guid StrIdSeguridadDoc)
		{
			try
			{
				await Task.Factory.StartNew(() =>
				{
					CrearAcuseRecibo(StrIdMensaje, StrIdSeguridadDoc);
				});
			}
			catch (Exception excepcion)
			{
				//Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.alarma);
			}
		}

		public void CrearAcuseRecibo(string StrIdMensaje, Guid StrIdSeguridadDoc)
		{

			try
			{
				Ctl_Documento documento = new Ctl_Documento();

				//Si el proceso viene del proceso de enviar el correo al adquiriente
				if (!string.IsNullOrEmpty(StrIdMensaje))
				{
					MensajeResumen datos_retorno = null;
					
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					datos_retorno = email.ConsultarCorreo(Convert.ToInt64(StrIdMensaje));

					if (datos_retorno != null && !string.IsNullOrEmpty(datos_retorno.Estado))
					{

						if (MensajeIdResultado.NoEntregado.GetHashCode().Equals(datos_retorno.IdResultado))
						{

							TblDocumentos doc = documento.ObtenerPorIdSeguridad(StrIdSeguridadDoc, true).FirstOrDefault();

							if (doc.TblEmpresasAdquiriente == null)
							{
								Ctl_Empresa ctlempresa = new Ctl_Empresa();
								TblEmpresas adquiriente = ctlempresa.Obtener(doc.StrEmpresaAdquiriente);
								doc.TblEmpresasAdquiriente = adquiriente;
							}

							email = new Ctl_EnvioCorreos();
							List<MensajeEnvio> notificacion = new List<MensajeEnvio>();
							try
							{
								notificacion = email.NotificacionDocumento(doc, doc.TblEmpresasAdquiriente.StrTelefono, doc.TblEmpresasAdquiriente.StrMailAdmin, doc.StrIdSeguridad.ToString());
							}
							catch (Exception excepcion)
							{
								//Si se presenta un error en el envio se notifica al facturador para que valide
								email.NotificacionCorreofacturador(doc, doc.TblEmpresasAdquiriente.StrTelefono, doc.TblEmpresasAdquiriente.StrMailAdmin, "Error enviando Correo", doc.StrIdSeguridad.ToString());

								throw excepcion;
							}
						}

					
					}
					else
					{

					}
				}

				List<TblDocumentos> datos = documento.ActualizarRespuestaAcuse(StrIdSeguridadDoc, (short)CodigoResponseV2.Recibido.GetHashCode(), Enumeracion.GetDescription(CodigoResponseV2.Recibido));




			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.alarma);
			}


		}



	}
}
