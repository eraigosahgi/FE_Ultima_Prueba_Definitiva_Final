using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Funciones;
using HGInetMiFacturaElectonicaController.Registros;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.RegistroLog;
using HGInetMiFacturaElectonicaController.Auditorias;
using System.Data.Entity.SqlServer;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;

namespace HGInetMiFacturaElectonicaController
{
	public partial class Ctl_ProcesosCorreos : BaseObject<TblProcesoCorreo>
	{
		#region Constructores 

		public Ctl_ProcesosCorreos() : base(new ModeloAutenticacion()) { }
		public Ctl_ProcesosCorreos(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_ProcesosCorreos(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion


		/// <summary>
		/// Obtiene Todos los correos ordenados por fecha y en estado false
		/// </summary>        
		/// <returns></returns>
		public List<TblProcesoCorreo> ObtenerCorreos(int dias, bool solodia)
		{

			List<TblProcesoCorreo> datos = null;
			DateTime FechaActual = Fecha.GetFecha();
			DateTime FechaInicial = FechaActual.AddDays(-dias).Date;
			DateTime FechaFinal = new DateTime(FechaInicial.Year, FechaInicial.Month, FechaInicial.Day, 23, 59, 59, 999);

			if (dias == 0)
			{

				datos = (from item in context.TblProcesoCorreo
					where item.IntEnvioMail == false
					orderby item.DatFecha ascending
						 select item).ToList();
			}
			else
			{
				if (solodia == true)
				{


					datos = (from item in context.TblProcesoCorreo
						where item.IntEnvioMail == false &&
						      item.DatFecha >= FechaInicial && item.DatFecha <= FechaFinal
						orderby item.DatFecha ascending
							 select item).ToList();
				}
				else
				{
					datos = (from item in context.TblProcesoCorreo
						where item.IntEnvioMail == false &&
						      item.DatFecha >= SqlFunctions.DateAdd("dd", -dias, FechaActual)
						orderby item.DatFecha ascending
							 select item).ToList();
				}

			}

			return datos;
		}

		/// <summary>
		/// Obtiene Todos los correos ordenados por fecha
		/// </summary>        
		/// <returns></returns>
		public List<TblProcesoCorreo> ObtenerPorValidar(int dias, bool solodia)
		{
			List<TblProcesoCorreo> datos = null;
			DateTime FechaActual = Fecha.GetFecha();
			DateTime FechaInicial = FechaActual.AddDays(-dias).Date;
			DateTime FechaFinal = new DateTime(FechaInicial.Year, FechaInicial.Month, FechaInicial.Day, 23, 59, 59, 999);

			if (dias == 0)
			{

				datos = (from item in context.TblProcesoCorreo
					where item.IntValidadoMail == false &&
					      (item.IntEnvioMail == true || item.StrIdMensaje != null) &&
						  item.DatFecha >= FechaInicial && item.DatFecha <= FechaFinal
					orderby item.DatFecha ascending 
					select item).ToList();
			}
			else
			{
				if (solodia == true)
				{
					

					datos = (from item in context.TblProcesoCorreo
						where item.IntValidadoMail == false &&
						      (item.IntEnvioMail == true || item.StrIdMensaje != null) &&
							  item.DatFecha >= FechaInicial && item.DatFecha <= FechaFinal
							 orderby item.DatFecha ascending
							 select item).ToList();
				}
				else
				{
					datos = (from item in context.TblProcesoCorreo
						where item.IntValidadoMail == false &&
						      (item.IntEnvioMail == true || item.StrIdMensaje != null) &&
							  item.DatFecha >= SqlFunctions.DateAdd("dd", -dias, FechaActual)
						orderby item.DatFecha ascending
							 select item).ToList();
				}

			}

			

			return datos;
		}


		/// <summary>
		/// Obtiene correo para enviar con el idseguridad
		/// </summary>
		/// <param name="IdSeguridadDoc">IdSeguridadDoc del documento</param>
		/// <returns></returns>
		public TblProcesoCorreo Obtener(Guid IdSeguridadDoc, bool LazyLoading = true)
		{

			context.Configuration.LazyLoadingEnabled = LazyLoading;

			var datos = (from item in context.TblProcesoCorreo
				where item.StrIdSeguridadDoc.Equals(IdSeguridadDoc)
				&& (item.StrIdMensaje == null	|| string.IsNullOrEmpty(item.StrIdMensaje))
				select item).FirstOrDefault();
			return datos;
		}

		public List<TblProcesoCorreo> ObtenerTodos(Guid IdSeguridadDoc, bool LazyLoading = true)
		{

			context.Configuration.LazyLoadingEnabled = LazyLoading;

			var datos = (from item in context.TblProcesoCorreo
						 where item.StrIdSeguridadDoc.Equals(IdSeguridadDoc)
						 select item).OrderBy(x => x.DatFecha).ToList();
			return datos;
		}

		public TblProcesoCorreo ObtenerPorIdMensaje(string IdMensaje)
		{
			context.Configuration.LazyLoadingEnabled = false;

			var datos = (from item in context.TblProcesoCorreo
						 where item.StrIdMensaje.Equals(IdMensaje)
						 select item).FirstOrDefault();
			return datos;
		}

		/// <summary>
		/// Crea el documento en la tabla de Correos para ser enviado
		/// </summary>
		/// <param name="IdSeguridadDoc">IdSeguridad del documento</param>
		/// <returns></returns>
		public TblProcesoCorreo Crear(Guid IdSeguridadDoc)
		{
			TblProcesoCorreo correo = new TblProcesoCorreo();
			correo.StrIdSeguridadDoc = IdSeguridadDoc;
			correo.DatFecha = Fecha.GetFecha();
			correo.IntEnvioMail = false;
			correo.StrIdSeguridad = Guid.NewGuid();

			correo = this.Add(correo);

			return correo;
		}

		/// <summary>
		/// Actualiza el documento en la tabla de Correos
		/// </summary>        
		/// /// <param name="TblProcesoCorreo">Tbl ProcesoCorreo a actualizar en BD</param>
		/// <returns></returns>
		public TblProcesoCorreo Actualizar(TblProcesoCorreo correo)
		{
			correo = this.Edit(correo);

			return correo;

		}

		public static void EnviarCorreo(TblDocumentos doc)
		{

			try
			{
				string email_objeto = string.Empty;
				string telefono_objeto = string.Empty;
				if (doc.IntDocTipo < TipoDocumento.AcuseRecibo.GetHashCode())
				{
					var objeto = (dynamic)null;
					objeto = Ctl_Documento.ConvertirServicio(doc, true);

					if (doc.IntDocTipo == TipoDocumento.Factura.GetHashCode())
					{
						email_objeto = objeto.DatosFactura.DatosAdquiriente.Email;
						telefono_objeto = objeto.DatosFactura.DatosAdquiriente.Telefono;
					}

					if (doc.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
					{
						email_objeto = objeto.DatosNotaCredito.DatosAdquiriente.Email;
						telefono_objeto = objeto.DatosNotaCredito.DatosAdquiriente.Telefono;
					}

					if (doc.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
					{
						email_objeto = objeto.DatosNotaDebito.DatosAdquiriente.Email;
						telefono_objeto = objeto.DatosNotaDebito.DatosAdquiriente.Telefono;
					}
				}
				else
				{
					email_objeto = doc.TblEmpresasFacturador.StrMailAdmin;
					telefono_objeto = doc.TblEmpresasFacturador.StrTelefono;
				}	

				

				Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
				List<MensajeEnvio> notificacion = new List<MensajeEnvio>();
				try
				{
					notificacion = email.NotificacionDocumento(doc, telefono_objeto, email_objeto, doc.StrIdSeguridad.ToString());
				}
				catch (Exception excepcion)
				{
					//Si se presenta un error en el envio se notifica al facturador para que valide
					doc.IntEstadoEnvio = (short)EstadoEnvio.Desconocido.GetHashCode();
					doc.DatFechaActualizaEstado = Fecha.GetFecha();
					doc.IntEnvioMail = false;
					email.NotificacionCorreofacturador(doc, telefono_objeto, email_objeto, "Error enviando Correo", doc.StrIdSeguridad.ToString());

					throw excepcion;
				}

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.ninguna);

				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);

				throw excepcion;
			}

		}

		/// <summary>
		/// Actualiza el correo en tabla segun respuesta del proveedor de Email(SendGrid)
		/// </summary>
		/// <param name="Correo">Objeto de informacion del correo enviado</param>
		public void ActualizarCorreo(MensajeResumen Correo)
		{
			try
			{
				if (string.IsNullOrEmpty(Correo.id_mensaje))
					throw new ApplicationException("Id del mensaje vacío");

				TblProcesoCorreo tbl_correo = ObtenerPorIdMensaje(Correo.id_mensaje);

				if (tbl_correo == null)
					throw new ApplicationException("Id del mensaje no existe en plataforma");

				Ctl_Documento ctl_doc = new Ctl_Documento();

				TblDocumentos doc_bd = ctl_doc.ObtenerPorIdSeguridad(tbl_correo.StrIdSeguridadDoc, false).FirstOrDefault();

				if (MensajeIdResultado.Entregado.GetHashCode().Equals(Correo.IdResultado))
				{
					doc_bd.IntEstadoEnvio = (short)Correo.IdEstado;
					doc_bd.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(Correo.Estado);
					doc_bd.DatFechaActualizaEstado = Fecha.GetFecha();

				}
				else if (MensajeIdResultado.NoEntregado.GetHashCode().Equals(Correo.IdResultado))
				{
					doc_bd.IntEstadoEnvio = (short)EstadoEnvio.NoEntregado.GetHashCode();
					doc_bd.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(Correo.Estado);
					doc_bd.DatFechaActualizaEstado = (string.IsNullOrEmpty(Correo.Estado)) ? Fecha.GetFecha() : Correo.Recibido;
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					List<MensajeEnvio> notificacion = email.NotificacionCorreofacturador(doc_bd, doc_bd.TblEmpresasAdquiriente.StrTelefono, tbl_correo.StrMailEnviado, Correo.Estado, tbl_correo.StrIdSeguridadDoc.ToString());

				}
				else
				{
					doc_bd.IntEstadoEnvio = (short)Correo.IdEstado;
					doc_bd.IntMensajeEnvio = (short)Enumeracion.GetValueFromDescription<MensajeEstado>(Correo.Estado);
					doc_bd.DatFechaActualizaEstado = (string.IsNullOrEmpty(Correo.Estado)) ? Fecha.GetFecha() : Correo.Recibido;
				}

				tbl_correo.DatFechaValidado = doc_bd.DatFechaActualizaEstado;
				tbl_correo.IntEventoResp = doc_bd.IntMensajeEnvio;
				tbl_correo.IntValidadoMail = true;

				ctl_doc.Actualizar(doc_bd);

				Actualizar(tbl_correo);

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion, "Inconsistencia actualizando correo desde plataforma intermedia");

				throw excepcion;
			}
		}


		public async Task AsyncActualizarCorreo(MensajeResumen Correo)
		{
			try
			{
				var Tarea = TareaActualizarCorreo(Correo);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}

		public async Task TareaActualizarCorreo(MensajeResumen Correo)
		{
			await Task.Factory.StartNew(() =>
			{

				ActualizarCorreo(Correo);

			});
		}


	}
}
