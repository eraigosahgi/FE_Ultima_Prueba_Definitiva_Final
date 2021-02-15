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
				var objeto = (dynamic)null;
				objeto = Ctl_Documento.ConvertirServicio(doc, true);
				string email_objeto = string.Empty;
				string telefono_objeto = string.Empty;
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


	}
}
