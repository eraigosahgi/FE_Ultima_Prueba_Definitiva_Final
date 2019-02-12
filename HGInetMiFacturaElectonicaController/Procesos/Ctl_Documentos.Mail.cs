using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
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
			DocumentoRespuesta respuesta_email = new DocumentoRespuesta();
			try
			{
				var documento_obj = (dynamic)null;
				documento_obj = documento;

				//Si es nuevo en la Plataforma envia Bienvenida a la plataforma
				if (adquiriente_nuevo == true)
				{
					email.Bienvenida(adquiriente, adquiriente_usuario);
				}

				//Envia correo al adquiriente que tiene el objeto
				List<MensajeEnvio> mensajes= email.NotificacionDocumento(documentoBd, documento_obj.DatosObligado.Telefono, documento_obj.DatosAdquiriente.Email,respuesta.IdPeticion.ToString());

				//Valida si el proceso es completo para actualizar estados y bd
				if (!notificacion_basica)
				{
					respuesta.DescripcionProceso = string.Format("{0} - En estado EXITOSA", respuesta.DescripcionProceso);

					//Actualiza la respuesta
					respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioEmailAcuse);
					respuesta.FechaUltimoProceso = Fecha.GetFecha();
					respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

					//Actualiza Documento en Base de Datos
					documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
					documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
					documentoBd.IntEnvioMail = true;

					Ctl_Documento documento_tmp = new Ctl_Documento();
					documento_tmp.Actualizar(documentoBd);

					//Actualiza la categoria con el nuevo estado
					respuesta.IdEstado = documentoBd.IdCategoriaEstado;
					respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
					ValidarRespuesta(respuesta,"", mensajes,false);
				}
				else
				{
					//Actualiza la respuesta
					respuesta.FechaUltimoProceso = Fecha.GetFecha();

					//Actualiza Documento en Base de Datos
					documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
				}


			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el Envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

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

					//Creacion del Usuario del Adquiriente
					if (usuarioBd == null)
					{
						_usuario = new Ctl_Usuario();
						usuarioBd = _usuario.Crear(adquirienteBd);

						adquiriente_nuevo = true;
					}
				}
				catch (Exception excepcion)
				{
					string msg_excepcion = Excepcion.Mensaje(excepcion);

					if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key"))
						throw excepcion;
					else
						adquiriente_nuevo = false;
				}

				// envía el mail de documentos y de creación de adquiriente
				respuesta = Ctl_Documentos.MailDocumentos(documento, documentoBd, empresa, adquiriente_nuevo, adquirienteBd, usuarioBd, ref respuesta, ref documento_result, notificacion_basica);
				//ValidarRespuesta(respuesta);

			}
			catch (Exception excepcion)
			{
				if (!notificacion_basica)
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
			}

			return respuesta;


		}
	}
}
