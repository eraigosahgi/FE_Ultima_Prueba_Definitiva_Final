using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaAudit.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.Peticiones;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class AuditoriasController : ApiController
	{
		[HttpGet]
		[Route("Api/AuditoriaDocumento_old")]
		public IHttpActionResult AuditoriaDocumento_old(string id_seguridad_doc)
		{
			try
			{
				//Valida los datos de la sesión.
				//Sesion.ValidarSesion();

				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditDocumentos> datos_audit = clase_audit_doc.Obtener(id_seguridad_doc, "*").OrderByDescending(x => x.DatFecha).ToList();

				if (datos_audit == null)
				{
					return NotFound();
				}

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos del documento en la base de datos.
				TblDocumentos datos_doc = ctl_documento.ObtenerPorIdSeguridad(new Guid(id_seguridad_doc)).FirstOrDefault();

				var datos_retorno = datos_audit.Select(d => new
				{
					StrIdSeguridad = d.StrIdSeguridad,
					StrIdPeticion = d.StrIdPeticion,
					DatFecha = d.DatFecha.ToString("yyyy-MM-dd HH:mm:ss.fff"),
					StrObligado = d.StrObligado,
					IntIdEstado = d.IntIdEstado,
					StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(d.IntIdEstado)),
					IntIdProceso = d.IntIdProceso,
					StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(d.IntIdProceso)),
					IntTipoRegistro = d.IntTipoRegistro,
					IntIdProcesadoPor = d.IntIdProcesadoPor,
					StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
					StrRealizadoPor = d.StrRealizadoPor,
					StrDesRealizadoPor = (!string.IsNullOrWhiteSpace(d.StrRealizadoPor)) ? NombreUsuario(new Guid(d.StrRealizadoPor)) : string.Empty,
					StrMensaje = d.StrMensaje,
					StrResultadoProceso = d.StrResultadoProceso,
					StrPrefijo = d.StrPrefijo,
					StrNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.StrNumero),
					//Asigna las rutas de los archivos a las propiedades de retorno, estas se obtiene de la consulta del documento a la bd.
					RutaXml = (datos_doc != null) ? datos_doc.StrUrlArchivoUbl : string.Empty,
					RutaPdf = (datos_doc != null) ? datos_doc.StrUrlArchivoPdf : string.Empty,
					RutaXmlAcuse = (datos_doc != null) ? datos_doc.StrUrlAcuseUbl : string.Empty,
					TipoDocumento = (datos_doc != null) ? Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(datos_doc.IntDocTipo)) : "Documento",
				}).ToList();

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		[HttpGet]
		[Route("Api/AuditoriaDocumento")]
		public IHttpActionResult AuditoriaDocumento(string id_seguridad_doc)
		{
			try
			{


				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos del documento en la base de datos.
				TblDocumentos datos_doc = ctl_documento.ObtenerPorIdSeguridad(new Guid(id_seguridad_doc)).FirstOrDefault();

				string ruta = string.Format("{0}", datos_doc.StrCufe);

				return Ok(ruta);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna el nombre completo del usuario.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		private string NombreUsuario(Guid id_seguridad)
		{
			try
			{
				if (id_seguridad == null)
					return string.Empty;

				Ctl_Usuario clase_usuario = new Ctl_Usuario();
				TblUsuarios datos_usuario = clase_usuario.ObtenerIdSeguridad(id_seguridad);

				if (datos_usuario != null)
					return string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);

				return string.Empty;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("Api/DetallesRespuesta")]
		public IHttpActionResult DetallesRespuesta(string respuesta)
		{
			try
			{
				//Valida los datos de la sesión.
				//Sesion.ValidarSesion();
				respuesta = respuesta.Replace("[", "").Replace("]", "");
				dynamic datos = JsonConvert.DeserializeObject(respuesta);

				Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
				MensajeResumen datos_retorno = new MensajeResumen();

				datos_retorno = email.ConsultarCorreo((string)datos.MessageID);


				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/ConsultaAuditoria")]
		public IHttpActionResult ConsultaAuditoria(DateTime fecha_inicio, DateTime fecha_fin, string identificacion_obligado, string estado, string proceso, string tipo_registro, string procedencia, string numero_documento, int Desde, int Hasta)
		{
			try
			{

				//Valida los datos de la sesión.
				//Sesion.ValidarSesion();

				//Con este proceso no validamos si tiene el permiso o no, porque este servicio es llamado desde varias partes, sin importar si este en la plataforma autenticado
				string usuario = string.Empty;
				string empresa = string.Empty;
				bool muestra_detalle = false;
				try
				{
					usuario = Sesion.DatosUsuario.StrUsuario;
					empresa = Sesion.DatosUsuario.StrEmpresa;
					if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(empresa))
					{
						Ctl_OpcionesUsuario _opciones = new Ctl_OpcionesUsuario();
						var datos = _opciones.ObtenerPermiso(usuario, empresa, 1347); //Permiso de auditoria
																					  //Validamos si tiene el permiso de auditoria
						if (datos != null)
						{
							//luego retornamos el permiso de gestión
							muestra_detalle = datos.IntGestion;
						}
					}
				}
				catch (Exception)
				{ }

				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditDocumentos> datos_audit = clase_audit_doc.Obtener(numero_documento, identificacion_obligado, fecha_inicio, fecha_fin, estado, proceso, tipo_registro, procedencia, Desde, Hasta);

				if (datos_audit.Count == 0)
				{
					return Ok();
				}


				var datos_retorno = datos_audit.Select(d => new
				{
					Firma = d.Id,
					StrIdSeguridad = ((d.StrIdSeguridad != null)) ? d.StrIdSeguridad.ToString() : "",
					DatFecha = d.DatFecha.ToString("yyyy-MM-dd HH:mm:ss.fff"),
					StrObligado = (string.IsNullOrEmpty(d.StrObligado) ? null : d.StrObligado),
					IntIdEstado = d.IntIdEstado,
					StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(d.IntIdEstado)),
					IntIdProceso = d.IntIdProceso,
					StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(d.IntIdProceso)),
					IntTipoRegistro = d.IntTipoRegistro,
					StrDescTipoReg = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoRegistro>(d.IntTipoRegistro)),
					IntIdProcesadoPor = d.IntIdProcesadoPor,
					StrMensaje = d.StrMensaje,
					StrResultadoProceso = d.StrResultadoProceso,
					StrDescProcePor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
					StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
					StrRealizadoPor = (!string.IsNullOrWhiteSpace(d.StrRealizadoPor)) ? NombreUsuario(new Guid(d.StrRealizadoPor)) : string.Empty,
					StrNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.StrNumero),
					mostrar_detalle = muestra_detalle
				}).ToList();

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Obtiene la auditoría de la gestión de formatos.
		/// </summary>
		/// <param name="codigo_formato">código del formato</param>
		/// <param name="identificacion_empresa">identificación de la empresa</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/AuditoriaFormatos")]
		public IHttpActionResult AuditoriaFormatos(int codigo_formato, string identificacion_empresa)
		{
			try
			{
				Sesion.ValidarSesion();

				HGInetMiFacturaElectonicaController.Auditorias.Ctl_FormatosAudit clase_audit_doc = new HGInetMiFacturaElectonicaController.Auditorias.Ctl_FormatosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditFormatos> datos_audit = clase_audit_doc.Obtener(codigo_formato, identificacion_empresa);

				if (datos_audit.Count == 0)
				{
					return Ok();
				}

				var datos_retorno = datos_audit.Select(d => new
				{
					DatFechaProceso = d.DatFechaProceso.ToString("yyyy-MM-dd HH:mm:ss.fff"),
					d.IntCodigoFormato,
					d.IntTipoProceso,
					StrProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TiposProceso>(d.IntTipoProceso)),
					d.StrEmpresa,
					d.StrIdSeguridad,
					d.StrObservaciones,
					d.StrUsuarioProceso,
					NombreUsuario = (!string.IsNullOrWhiteSpace(d.StrUsuarioProceso)) ? NombreUsuario(new Guid(d.StrUsuarioProceso)) : string.Empty,
				}).ToList();

				return Ok(datos_retorno);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la Auditoria de los Emails asociados a un Documento
		/// </summary>
		/// <param name="id_seguridad_doc">Identificador unico del documento en la Plataforma</param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/AuditoriaMail")]
		public IHttpActionResult AuditoriaMail(string id_seguridad_doc)
		{
			try
			{
				//Valida los datos de la sesión.
				//Sesion.ValidarSesion();

				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditDocumentos> ListaEmail = clase_audit_doc.ObtenerDocumentoMail(Guid.Parse(id_seguridad_doc));

				Ctl_ProcesosCorreos ctl_correo = new Ctl_ProcesosCorreos();

				List<TblProcesoCorreo> ListaCorreoTbl = null;

				bool correo_tbl = false;

				if (ListaEmail == null || ListaEmail.Count == 0)
				{
					ListaCorreoTbl = ctl_correo.ObtenerTodos(Guid.Parse(id_seguridad_doc));

					if (ListaCorreoTbl != null)
					{
						correo_tbl = true;
					}
					else
					{
						return NotFound();
					}
				}

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos del documento en la base de datos.
				TblDocumentos datos_doc = ctl_documento.ObtenerPorIdSeguridad(new Guid(id_seguridad_doc)).FirstOrDefault();

				//[691671] - Se agrega proceso para actualizar estado del mail en el documento cuando se vea auditoria del correo.
				try
				{
					if (datos_doc.IntEstadoEnvio == (short)EstadoEnvio.NoEntregado.GetHashCode() || datos_doc.IntEstadoEnvio != (short)EstadoEnvio.Leido.GetHashCode())
					{
						short mejor_estado = datos_doc.IntEstadoEnvio;

						if (correo_tbl == false)
						{
							foreach (TblAuditDocumentos item in ListaEmail)
							{
								string respuesta = item.StrResultadoProceso.Replace("[", "").Replace("]", "");
								dynamic datos = JsonConvert.DeserializeObject(respuesta);

								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								MensajeResumen datos_resumen = new MensajeResumen();

								datos_resumen = email.ConsultarCorreo((string)datos.MessageID);

								if (datos_resumen.IdResultado == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeIdResultado.Enviado.GetHashCode() || datos_resumen.IdResultado == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeIdResultado.Entregado.GetHashCode())
								{
									short estado_mail = (short)Enumeracion.GetValueFromDescription<LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado>(datos_resumen.Estado);

									if (estado_mail == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Clicked.GetHashCode() || estado_mail == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Opened.GetHashCode())
									{
										mejor_estado = (short)EstadoEnvio.Leido.GetHashCode();
									}
									else if (estado_mail <= LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Sent.GetHashCode())
									{
										mejor_estado = (short)EstadoEnvio.Entregado.GetHashCode();
									}

								}
							}
						}
						else
						{
							foreach (TblProcesoCorreo item in ListaCorreoTbl)
							{

								Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
								MensajeResumen datos_resumen = new MensajeResumen();

								datos_resumen = email.ConsultarCorreo(item.StrIdMensaje);

								if (datos_resumen.IdResultado == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeIdResultado.Enviado.GetHashCode() || datos_resumen.IdResultado == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeIdResultado.Entregado.GetHashCode())
								{
									short estado_mail = (short)Enumeracion.GetValueFromDescription<LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado>(datos_resumen.Estado);

									if (estado_mail == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Clicked.GetHashCode() || estado_mail == LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Opened.GetHashCode())
									{
										mejor_estado = (short)EstadoEnvio.Leido.GetHashCode();
									}
									else if (estado_mail <= LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.MensajeEstado.Sent.GetHashCode())
									{
										mejor_estado = (short)EstadoEnvio.Entregado.GetHashCode();
									}

								}
							}
						}

						if (mejor_estado != datos_doc.IntEstadoEnvio)
						{
							datos_doc.IntEstadoEnvio = mejor_estado;
							datos_doc.DatFechaActualizaEstado = Fecha.GetFecha();
							ctl_documento.Actualizar(datos_doc);
						}

					}
				}
				catch (Exception excepcion)
				{
					LibreriaGlobalHGInet.RegistroLog.RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Auditoria, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.actualizacion, string.Format("Se presenta incosistencia actualizando estado del mail en el documento {0}", datos_doc.StrIdSeguridad));

				}

				if (correo_tbl == false)
				{
					var datos_retorno = ListaEmail.Select(d => new
					{
						StrIdSeguridad = d.StrIdSeguridad,
						StrIdPeticion = d.StrIdPeticion,
						DatFecha = d.DatFecha.ToString("yyyy-MM-dd HH:mm:ss.fff"),
						StrObligado = d.StrObligado,
						IntIdEstado = d.IntIdEstado,
						StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(d.IntIdEstado)),
						IntIdProceso = d.IntIdProceso,
						StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(d.IntIdProceso)),
						IntTipoRegistro = d.IntTipoRegistro,
						IntIdProcesadoPor = d.IntIdProcesadoPor,
						StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
						StrRealizadoPor = d.StrRealizadoPor,
						StrDesRealizadoPor = (!string.IsNullOrWhiteSpace(d.StrRealizadoPor)) ? NombreUsuario(new Guid(d.StrRealizadoPor)) : string.Empty,
						StrMensaje = d.StrMensaje,
						StrResultadoProceso = d.StrResultadoProceso,
						StrPrefijo = d.StrPrefijo,
						StrNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.StrNumero),
						//Asigna las rutas de los archivos a las propiedades de retorno, estas se obtiene de la consulta del documento a la bd.
						RutaXml = (datos_doc != null) ? datos_doc.StrUrlArchivoUbl : string.Empty,
						RutaPdf = (datos_doc != null) ? datos_doc.StrUrlArchivoPdf : string.Empty,
						RutaXmlAcuse = (datos_doc != null) ? datos_doc.StrUrlAcuseUbl : string.Empty,
						TipoDocumento = (datos_doc != null) ? Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(datos_doc.IntDocTipo)) : "Documento",
					}).ToList();

					return Ok(datos_retorno);
				}
				else
				{
					LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta.MensajeEnvioItem data_correo = null;

					var datos_retorno = ListaCorreoTbl.Select(d => new
					{
						StrIdSeguridad = d.StrIdSeguridadDoc,
						StrIdPeticion = d.StrIdSeguridad,
						DatFecha = d.DatFecha.ToString("yyyy-MM-dd HH:mm:ss.fff"),
						StrObligado = datos_doc.StrEmpresaFacturador,
						IntIdEstado = datos_doc.IdCategoriaEstado,
						StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(datos_doc.IdCategoriaEstado)),
						IntIdProceso = datos_doc.IntIdEstado,
						StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(datos_doc.IntIdEstado)),
						IntTipoRegistro = 1,
						IntIdProcesadoPor = 1,
						StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(1)),
						StrRealizadoPor = string.Empty,
						StrDesRealizadoPor = string.Empty,
						StrMensaje = string.Empty,
						StrResultadoProceso = JsonConvert.SerializeObject(data_correo = new LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta.MensajeEnvioItem
						{
							Email = d.StrMailEnviado,
							MessageID = d.StrIdMensaje,
							MessageUUID = new Guid(),
						}),
						StrPrefijo = datos_doc.StrPrefijo,
						StrNumero = string.Format("{0}{1}", (datos_doc.StrPrefijo == null) ? "" : (!datos_doc.StrPrefijo.Equals("0")) ? datos_doc.StrPrefijo : "", datos_doc.IntNumero),
						//Asigna las rutas de los archivos a las propiedades de retorno, estas se obtiene de la consulta del documento a la bd.
						RutaXml = (datos_doc != null) ? datos_doc.StrUrlArchivoUbl : string.Empty,
						RutaPdf = (datos_doc != null) ? datos_doc.StrUrlArchivoPdf : string.Empty,
						RutaXmlAcuse = (datos_doc != null) ? datos_doc.StrUrlAcuseUbl : string.Empty,
						TipoDocumento = (datos_doc != null) ? Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(datos_doc.IntDocTipo)) : "Documento",
					}).ToList();

					return Ok(datos_retorno);
				}

				
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
