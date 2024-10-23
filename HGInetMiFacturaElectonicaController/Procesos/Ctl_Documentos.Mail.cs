using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		public static DocumentoRespuesta MailDocumentos(object documento, TblDocumentos documentoBd, TblEmpresas obligado, bool adquiriente_nuevo, TblEmpresas adquiriente, TblUsuarios adquiriente_usuario, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result, bool notificacion_basica = false)
		{
			Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
			var documento_obj = (dynamic)null;
			documento_obj = documento;

			//Registro el documento en la tabla correo para gestionarlo
			Ctl_ProcesosCorreos proceso_correo = new Ctl_ProcesosCorreos();
			TblProcesoCorreo correo_doc = null;
			correo_doc = proceso_correo.Obtener(documentoBd.StrIdSeguridad);
			if (correo_doc == null)
				correo_doc = proceso_correo.Crear(documentoBd.StrIdSeguridad);

			//Se deshabilita proceso de envio de bienvenida caso 560400
			//Si es nuevo en la Plataforma envia Bienvenida a la plataforma
			/*try
			{
				if (adquiriente_nuevo == true)
				{
					email.Bienvenida(adquiriente, adquiriente_usuario);
				}
			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el Envío de la Bienvenida al Adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
			}*/

			//Se agrega esto para guardar correctamente el estado en la Auditoria
			if (respuesta.EstadoDian != null)
			{
				if (respuesta.EstadoDian.EstadoDocumento.Equals(EstadoDocumentoDian.Aceptado.GetHashCode()))
					documentoBd.IntIdEstado = Convert.ToInt16(ProcesoEstado.EnvioEmailAcuse.GetHashCode());
			}


			//Envia correo al adquiriente que tiene el objeto
			List<MensajeEnvio> mensajes = new List<MensajeEnvio>();

			try
			{
				if (documentoBd.IntDocTipo < TipoDocumento.AcuseRecibo.GetHashCode())
				{
					string nombre_comercial = string.IsNullOrEmpty(documento_obj.DatosObligado.NombreComercial) ? documento_obj.DatosObligado.RazonSocial : documento_obj.DatosObligado.NombreComercial;
					mensajes = email.NotificacionDocumento(documentoBd, documento_obj.DatosObligado.Telefono, documento_obj.DatosAdquiriente.Email, respuesta.IdPeticion.ToString(), Procedencia.Plataforma, "", ProcesoEstado.EnvioEmailAcuse, nombre_comercial);
				}
				else if (documentoBd.IntDocTipo == TipoDocumento.Nomina.GetHashCode() || (documentoBd.IntDocTipo == TipoDocumento.NominaAjuste.GetHashCode() && documento_obj.TipoNota.Equals(1)))
				{
					mensajes = email.NotificacionDocumento(documentoBd, documento_obj.DatosEmpleador.Telefono, documento_obj.DatosTrabajador.Email, respuesta.IdPeticion.ToString(), Procedencia.Plataforma, "", ProcesoEstado.EnvioEmailAcuse, obligado.StrRazonSocial);
					documentoBd.IntEstadoEnvio = (short)respuesta.IdEstadoEnvioMail;
					respuesta.IdEstadoEnvioMail = (short)EstadoEnvio.Enviado.GetHashCode();
					respuesta.DescripcionEstadoEnvioMail = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoEnvio>(respuesta.IdEstadoEnvioMail));
					documentoBd.IntEnvioMail = true;
				}
				
				//Actualiza la respuesta del envio del correo en TblDocumentos
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				//documentoBd.IntEstadoEnvio = (short)respuesta.IdEstadoEnvioMail;
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
			}
			catch (Exception excepcion)
			{
				//Falla el envio de correo por algun motivo de la plataforma o Mailjet
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdEstadoEnvioMail = (short)EstadoEnvio.Pendiente.GetHashCode();
				respuesta.DescripcionEstadoEnvioMail = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoEnvio>(respuesta.IdEstadoEnvioMail));
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el Envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				documentoBd.IntEstadoEnvio = (short)respuesta.IdEstadoEnvioMail;
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
				documentoBd.IntEnvioMail = false;
				
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
			}

			try
			{
				Ctl_Documento documento_tmp = new Ctl_Documento();
				documento_tmp.Actualizar(documentoBd);
			}
			catch (Exception)
			{ }

			try
			{
				//Valida si el proceso es completo para actualizar estados y bd
				//Se inactiva por que ya se hace antes de la tarea asincrona
				/*
				if (!notificacion_basica)
				{
					respuesta.DescripcionProceso = string.Format("{0} - En estado EXITOSA", respuesta.DescripcionProceso);

					//Actualiza la respuesta
					respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioEmailAcuse);
					respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

					//Actualiza Documento en Base de Datos
					documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

					Ctl_Documento documento_tmp = new Ctl_Documento();
					documento_tmp.Actualizar(documentoBd);

					//Actualiza la categoria con el nuevo estado
					respuesta.IdEstado = documentoBd.IdCategoriaEstado;
					respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
					ValidarRespuesta(respuesta, "", mensajes, false);
				}*/
			}
			catch (Exception excepcion)
			{
				//Falla actualizando estados despues del envio del correo
				if (respuesta.Error == null)
				{
					respuesta.FechaUltimoProceso = Fecha.GetFecha();
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error actualizando estados despues del Envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
					documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
				}

			}

			return respuesta;

		}

		/// <summary>
		/// Valida usuario del adquiriente y hace el envio de la informacion
		/// </summary>
		/// <param name="documento">Documento enviado Factura - Nota credito - Nota debito</param>
		/// <param name="documentoBd">Registro del documento en BD</param>
		/// <param name="empresa">Informacion del Obligado a Facturar</param>
		/// <param name="respuesta">Objeto respuesta</param>
		/// <param name="documento_result"></param>
		/// <param name="notificacion_basica">Indica la Notificacion que se debe enviar(PDF o Completa)</param>
		/// <returns></returns>
		public static DocumentoRespuesta Envio(object documento, TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result, bool notificacion_basica = false)
		{
			try
			{
				TblUsuarios usuarioBd = null;
				Ctl_Empresa empresa_config = new Ctl_Empresa();

				TblEmpresas adquirienteBd = null;

				//Obtiene la informacion del Adquiriente que se tiene en BD
				adquirienteBd = empresa_config.Obtener(respuesta.Identificacion);

				bool adquiriente_nuevo = false;

				try
				{

					Ctl_Usuario _usuario = new Ctl_Usuario();

					usuarioBd = _usuario.ObtenerUsuarios(respuesta.Identificacion, respuesta.Identificacion).FirstOrDefault();

					//Creacion del Usuario del Adquiriente siempre y cuando tenga correo registrado en la DIAN resolucion 042
					if (usuarioBd == null)
					{
						Ctl_ObtenerCorreos correo_recep = new Ctl_ObtenerCorreos();
						string correo_registrado = correo_recep.Obtener(respuesta.Identificacion);

						if (!string.IsNullOrEmpty(correo_registrado))
						{
							adquirienteBd.StrMailAdmin = correo_registrado;
							_usuario = new Ctl_Usuario();
							usuarioBd = _usuario.Crear(adquirienteBd);

							adquiriente_nuevo = true;
						}
						
					}
				}
				catch (Exception excepcion)
				{
					string msg_excepcion = Excepcion.Mensaje(excepcion);

					if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key") && !msg_excepcion.ToLowerInvariant().Contains("insertar una clave duplicada"))
						throw excepcion;
					else
						adquiriente_nuevo = false;
				}

				// envía el mail de documentos y de creación de adquiriente
				//respuesta = Ctl_Documentos.MailDocumentos(documento, documentoBd, empresa, adquiriente_nuevo, adquirienteBd, usuarioBd, ref respuesta, ref documento_result, notificacion_basica);
				Task envio = EnviarMailDocumentos(documento, documentoBd, empresa, adquiriente_nuevo, adquirienteBd, usuarioBd, respuesta, documento_result, notificacion_basica);

				//Actualiza la respuesta del envio del correo
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdEstadoEnvioMail = (short)EstadoEnvio.Enviado.GetHashCode();
				respuesta.DescripcionEstadoEnvioMail = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoEnvio>(respuesta.IdEstadoEnvioMail));
				documentoBd.IntEnvioMail = false;
				documentoBd.IntEstadoEnvio = (short)respuesta.IdEstadoEnvioMail;
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;

				//Valida si el proceso es completo para actualizar estados y bd
				if (!notificacion_basica)
				{
					respuesta.DescripcionProceso = string.Format("{0} - En estado EXITOSA", respuesta.DescripcionProceso);

					//Actualiza la respuesta
					respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioEmailAcuse);
					respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

					//Actualiza Documento en Base de Datos
					documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

					Ctl_Documento documento_tmp = new Ctl_Documento();
					documento_tmp.Actualizar(documentoBd);

					//Actualiza la categoria con el nuevo estado
					respuesta.IdEstado = documentoBd.IdCategoriaEstado;
					respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
					ValidarRespuesta(respuesta, "", null, false);
				}



				//ValidarRespuesta(respuesta);
			}
			catch (Exception excepcion)
			{
				if (!notificacion_basica)
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
			}

			return respuesta;
		}

		public static async Task EnviarMailDocumentos(object documento, TblDocumentos documentoBd, TblEmpresas empresa, bool adquiriente_nuevo, TblEmpresas adquirienteBd, TblUsuarios usuarioBd, DocumentoRespuesta respuesta, FacturaE_Documento documento_result, bool notificacion_basica = false)
		{
			try
			{
				var Tarea = TareaEnviarMailDocumentosAsync(documento, documentoBd, empresa, adquiriente_nuevo, adquirienteBd, usuarioBd, respuesta, documento_result, notificacion_basica = false);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}
		}

		public static async Task TareaEnviarMailDocumentosAsync(object documento, TblDocumentos documentoBd, TblEmpresas empresa, bool adquiriente_nuevo, TblEmpresas adquirienteBd, TblUsuarios usuarioBd, DocumentoRespuesta respuesta, FacturaE_Documento documento_result, bool notificacion_basica = false)
		{
			await Task.Factory.StartNew(() =>
			{
				respuesta = Ctl_Documentos.MailDocumentos(documento, documentoBd, empresa, adquiriente_nuevo, adquirienteBd, usuarioBd, ref respuesta, ref documento_result, notificacion_basica);
			});
		}
	}

}
