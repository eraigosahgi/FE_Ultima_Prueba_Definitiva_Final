using LibreriaGlobalHGInet.Enumerables.Aplicacion;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento;
using LibreriaGlobalHGInet.Peticiones;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.HgiNet.Controladores
{
	public class Ctl_ValidacionesLicencia
	{
		/// <summary>
		/// Valida la licencia del aplicativo mediante la API Api/ValidarLicenciamiento (se encuentra en HGInetLicenciamiento).
		/// Este metódo está diseñado para ser implementado desde multiples aplicativos.
		/// NOTA: Se debe tener en cuenta la afectación que pueden sufrir los demás productos con las modificaciones que se realicen sobre este metódo.
		/// </summary>
		/// <param name="objeto_principal">Objeto con los datos principales de la petición.</param>
		/// <param name="datos_licencia">Objeto con los datos adicionales de la petición.</param>
		/// <returns></returns>
		public RespuestaLicenciamiento ValidarLicencia(ObjetosComunes.Licenciamiento.Peticion objeto_principal, ValidarLicencia datos_licencia, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/ValidarLicenciamiento", url_principal);
				RespuestaLicenciamiento respuesta = new RespuestaLicenciamiento();

				if (objeto_principal == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				if (datos_licencia == null)
					throw new ApplicationException("Datos de la licencia inválidos.");

				if (string.IsNullOrWhiteSpace(objeto_principal.IdentificacionEmpresa) || objeto_principal.IdentificacionEmpresa.Equals("0"))
					throw new ApplicationException("Identificación de la empresa inválido.");

				List<string> apps_sin_serial = new List<string>()
				{
					CodigosAplicativo.HGInetSmart.GetHashCode().ToString(),
					CodigosAplicativo.Happgi.GetHashCode().ToString(),
				};

				if (string.IsNullOrWhiteSpace(datos_licencia.SerialAplicativo) && !apps_sin_serial.Contains(datos_licencia.CodigoAplicativo))
					throw new ApplicationException("Serial del aplicativo inválido.");

				try
				{
					string objeto_hijo = JsonConvert.SerializeObject(datos_licencia);

					//Construye la llave para la encripción de los datos.

					string llave_encripta = string.Empty;

					if (!apps_sin_serial.Contains(datos_licencia.CodigoAplicativo))
						llave_encripta = string.Format("{0}#{1}", datos_licencia.IdentificacionEmpresa, datos_licencia.SerialAplicativo);
					else
						llave_encripta = string.Format("{0}#{1}", datos_licencia.IdentificacionEmpresa, datos_licencia.CodigoAplicativo);

					//Encripta el objeto de tipo datos_adicionales en formato json empleando la llave de seguridad.
					string encripta = Seguridad.Encryption(objeto_hijo, llave_encripta);

					//Asigna la encripción de los datos al objeto principal.
					objeto_principal.DatosAdicionales = encripta;

				}
				catch (Exception excepcion)
				{
					throw new ApplicationException(string.Format("Error en la serialización de los datos de la licencia. {0}", excepcion.Message), excepcion.InnerException);
				}

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<RespuestaLicenciamiento> cliente = new ClienteRest<RespuestaLicenciamiento>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(objeto_principal);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("ValidarLicencia: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}

		/// <summary>
		/// Valida si la licencia debe ser consultada nuevamente.
		/// </summary>
		/// <param name="vigencia_verificacion">Fecha de vencimiento de la última validación.</param>
		/// <param name="ultima_verificacion">Fecha de la última validación. (ValidacionCrm)</param>
		/// <returns>True: Valida Licencia - False: No Valida Licencial (UltVerificacionCrm)</returns>
		public bool VerificarLicencia(string vigencia_verificacion, string ultima_verificacion, string identificacion_empresa, int id_aplicativo)
		{
			try
			{
				string llave_maestra = string.Format("{0}#{1}", Aplicativo.ObtenerNombreAplicacion(id_aplicativo).Replace(" ", ""), identificacion_empresa);

				//valida si no tiene fecha limite para la validación.
				if (string.IsNullOrWhiteSpace(vigencia_verificacion))
				{
					// fecha temporal de terminación de licencia
					string fecha_licencia_crm_encriptada = Seguridad.Encriptar("2000-01-01", llave_maestra);

					vigencia_verificacion = fecha_licencia_crm_encriptada;
				}

				DateTime fecha_vigencia_validacion;

				try
				{
					//Valida la fecha limite de la consulta
					if (!DateTime.TryParse(Seguridad.Desencriptar(vigencia_verificacion, llave_maestra), out fecha_vigencia_validacion))
						throw new ApplicationException("Error en desencriptar la fecha de vigencia de la verificación.");

					//indica la ultima fecha de validación de la licencia
					DateTime dat_ultima_verificacion = Fecha.GetFecha();

					//valida si contiene fecha de la ultima verificación.
					if (!string.IsNullOrWhiteSpace(ultima_verificacion))
					{
						if (!DateTime.TryParse(Seguridad.Desencriptar(ultima_verificacion, llave_maestra), out dat_ultima_verificacion))
							throw new ApplicationException("Error en desencriptar la fecha de última verificación");
					}

					//Valida si la fecha de vencimiento de la última validación es mayor o igual a la fecha actual.
					if (fecha_vigencia_validacion.Date >= Fecha.GetFecha().Date)
						return false;
				}
				catch (Exception)
				{
					VerificarLicencia("", "", identificacion_empresa, id_aplicativo);
				}

				return true;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("VerificarLicencia: {0}", JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la información de la licencia de uso del producto solicitado
		/// </summary>
		/// <param name="peticion"></param>
		/// <param name="url_principal"></param>
		/// <returns></returns>
		public InformacionLicencia ObtenerInformacionLicencia(PeticionLicencia peticion, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/DatosLicencia", url_principal);
				InformacionLicencia respuesta = new InformacionLicencia();

				if (peticion == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<InformacionLicencia> cliente = new ClienteRest<InformacionLicencia>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("ObtenerInformacionLicencia: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}

		/// <summary>
		/// Registra la estación.
		/// </summary>
		/// <param name="peticion"></param>
		/// <param name="url_principal"></param>
		/// <returns></returns>
		public InformacionEstacion RegistroEstacion(PeticionEstacion peticion, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/RegistroEstacion", url_principal);
				InformacionEstacion respuesta = new InformacionEstacion();

				if (peticion == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<InformacionEstacion> cliente = new ClienteRest<InformacionEstacion>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("RegistroEstacion: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}


		/// <summary>
		/// Edita los datos de la estación.
		/// </summary>
		/// <param name="peticion"></param>
		/// <param name="url_principal"></param>
		/// <returns></returns>
		public InformacionEstacion EditarEstacion(PeticionEstacion peticion, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/EditarEstacion", url_principal);
				InformacionEstacion respuesta = new InformacionEstacion();

				if (peticion == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<InformacionEstacion> cliente = new ClienteRest<InformacionEstacion>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("EditarEstacion: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}


		/// <summary>
		/// Edita los datos de la estación.
		/// </summary>
		/// <param name="peticion"></param>
		/// <param name="url_principal"></param>
		/// <returns></returns>
		public RespuestaEliminar EliminarEstacion(PeticionEstacion peticion, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/EliminarEstacion", url_principal);
				RespuestaEliminar respuesta = new RespuestaEliminar();

				if (peticion == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<RespuestaEliminar> cliente = new ClienteRest<RespuestaEliminar>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("EliminarEstacion: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}


		/// <summary>
		/// Valida si ya existe un registro para la configuración de servidor y base de datos,
		//	si no existe realiza el registro.
		/// </summary>
		/// <param name="peticion"></param>
		/// <param name="url_principal"></param>
		/// <returns></returns>
		public InformacionServidor ValidarRegistroServidor(PeticionServidor peticion, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/ValidarRegistroServidor", url_principal);
				InformacionServidor respuesta = new InformacionServidor();

				if (peticion == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<InformacionServidor> cliente = new ClienteRest<InformacionServidor>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;

					RegistroLog.RegistroLog.EscribirLog(new Exception(JsonConvert.SerializeObject(ex)), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
					respuesta.BloqueaProceso = true;
					respuesta.Notificacion = new Error.Error()
					{
						Codigo = Error.CodigoError.ERROR_EN_SERVIDOR,
						Fecha = Fecha.GetFecha(),
						Mensaje = ex.InnerException != null ? string.Format("{0} {1} {2}", ex.Message, ex.InnerException.Message, ruta_rest) : string.Format("{0} {1}", ex.Message, ruta_rest)
					};

				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("ValidarRegistroServidor: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}

		/// <summary>
		/// Obtiene el archivo .dat y envia la petición para el almacenamiento
		/// </summary>
		/// <param name="url_principal"></param>
		/// <param name="ruta_archivo"></param>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public bool ProcesarArchivoDat(string url_principal, string ruta_archivo, string identificacion_empresa)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/ProcesarArchivoDat", url_principal);

				string respuesta = string.Empty;

				byte[] bytes = System.IO.File.ReadAllBytes(ruta_archivo);

				ArchivoConfiguracion objeto_peticion = new ArchivoConfiguracion();
				objeto_peticion.IdentificacionEmpresa = identificacion_empresa;
				objeto_peticion.Archivo = bytes;

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<string> cliente = new ClienteRest<string>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(objeto_peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
				}
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}


		/// <summary>
		/// Valida la existencia de la estación.
		/// </summary>
		/// <param name="peticion"></param>
		/// <param name="url_principal"></param>
		/// <returns></returns>
		public InformacionEstacion ValidarExistenciaEstacion(PeticionEstacion peticion, string url_principal)
		{
			try
			{
				string ruta_rest = string.Format("{0}Api/ValidarExistenciaEstacion", url_principal);
				InformacionEstacion respuesta = new InformacionEstacion();

				if (peticion == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				ClienteRest<InformacionEstacion> cliente = new ClienteRest<InformacionEstacion>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					respuesta = cliente.POST(peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return respuesta;
			}
			catch (Exception excepcion)
			{
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("ValidarExistenciaEstacion: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_principal) : string.Format("{0} {1}", excepcion.Message, url_principal);
				throw new ApplicationException(mensaje);
			}
		}


	}
}

