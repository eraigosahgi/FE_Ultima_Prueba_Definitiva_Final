using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Enumerables;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController
{
	public class Ctl_EnvioDocumento
	{

		/// <summary>
		/// Proceso para reenviar documentos a un correo diferente
		/// </summary>
		/// <param name="documentos"></param>
		/// <returns></returns>
		public static List<NotificacionCorreo> Procesar(List<EnvioDocumento> documentos)
		{

			Ctl_Empresa Peticion = new Ctl_Empresa();

			//Válida que la key sea correcta.
			TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().IdentificacionFacturador);

			if (!facturador_electronico.IntObligado)
				throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

			try
			{
				List<NotificacionCorreo> respuesta = new List<NotificacionCorreo>();

				if (documentos != null)
				{
					//recorro la lista de documentos enviado para hacer el proceso
					foreach (EnvioDocumento item in documentos)
					{

						NotificacionCorreo item_respuesta = new NotificacionCorreo()
						{
							IdSeguridad = item.RadicadoDocumento,
							Email = item.Email
						};

						//Valida si el objeto viene si correo
						if (string.IsNullOrEmpty(item.Email))
						{
							Ctl_Documento Controlador = new Ctl_Documento();
							//Obtiene el documento desde el radicado
							TblDocumentos datos = Controlador.ObtenerPorIdSeguridad(Guid.Parse(item.RadicadoDocumento)).FirstOrDefault();
							if (datos == null)
							{
								throw new ApplicationException(string .Format("No se encontraro documento con el id de seguridad: {0}", item.RadicadoDocumento));
							}
							var objeto = (dynamic)null;
							//Luego obtiene el objeto ubl
							objeto = Ctl_Documento.ConvertirServicio(datos, true);

							//Asigno el email del ubl a la notificación del correo
							if (datos.IntDocTipo == TipoDocumento.Factura.GetHashCode())
							{
								item_respuesta.Email = objeto.DatosFactura.DatosAdquiriente.Email;
								item.Email = objeto.DatosFactura.DatosAdquiriente.Email;
							}

							if (datos.IntDocTipo == TipoDocumento.NotaCredito.GetHashCode())
							{
								item_respuesta.Email = objeto.DatosNotaCredito.DatosAdquiriente.Email;
								item.Email = objeto.DatosNotaCredito.DatosAdquiriente.Email;
							}

							if (datos.IntDocTipo == TipoDocumento.NotaDebito.GetHashCode())
							{
								item_respuesta.Email = objeto.DatosNotaDebito.DatosAdquiriente.Email;
								item.Email = objeto.DatosNotaDebito.DatosAdquiriente.Email;
							}
						
						}

						try
						{

							if (string.IsNullOrEmpty(item.RadicadoDocumento))
								throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Radicado", "GUID"));

							Ctl_Documento ctl_documento = new Ctl_Documento();
							List<TblDocumentos> list_tbldocumento = ctl_documento.ObtenerPorIdSeguridad(new Guid(item.RadicadoDocumento));

							if (list_tbldocumento == null || !list_tbldocumento.Any())
								throw new ApplicationException(string.Format("No se encontró información con el radicado {0}", item.RadicadoDocumento));

							if (!Texto.ValidarExpresion(TipoExpresion.Email, item.Email))
								throw new ArgumentException(string.Format("El Email {0} no esta bien formado", item.Email));

							Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
							List<MensajeEnvio> envio = clase_email.NotificacionDocumento(list_tbldocumento.FirstOrDefault(), list_tbldocumento.FirstOrDefault().TblEmpresasFacturador.StrTelefono, item.Email);

							//if (envio == true)
							if (envio != null)
							{
								item_respuesta.IdEstado = EstadoEmail.Exitoso.GetHashCode();
								item_respuesta.Mensaje = Enumeracion.GetDescription(EstadoEmail.Exitoso);
							}

						}
						catch (Exception excepcion)
						{							
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							item_respuesta.Mensaje = string.Format("Error al enviar el documento. Detalle: {0}", excepcion.Message);
						}
						respuesta.Add(item_respuesta);
					}

					return respuesta;
				}
				else
				{
					throw new ApplicationException("No se encontraron datos");
				}

			}
			catch (Exception ex)
			{
				Ctl_Log.Guardar(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(ex.Message);
			}

		}


	}
}
