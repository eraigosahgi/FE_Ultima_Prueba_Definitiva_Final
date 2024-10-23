using HGICtrlUtilidades.Recursos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_ValidacionesLicencia
	{

		private string mensaje_migracion = "En este momento el sistema no se encuentra disponible por motivo de actualización, por favor intente más tarde.";
		private string clave_migracion = "ProcesoMigracion";
		public RespuestaLicenciamiento ValidarLicencia(Peticion objeto_principal, ValidarLicencia datos_licencia, string url_plataforma)
		{
			string url_licencia = string.Empty;
			try
			{

				Cl_Configuraciones clase_configuraciones = new Cl_Configuraciones();
				url_licencia = clase_configuraciones.ObtenerUrl(url_plataforma, AmbientesUrl.HGInetLicencia, 1);

				string ruta_rest = string.Format("{0}Api/ValidarLicenciamiento", url_licencia);
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
						CodigosAplicativo.HGIEcommerce.GetHashCode().ToString(),
						CodigosAplicativo.HGInetServiciosWeb.GetHashCode().ToString(),
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
					string encripta = Cl_Seguridad.Encryption(objeto_hijo, llave_encripta);

					//Asigna la encripción de los datos al objeto principal.
					objeto_principal.DatosAdicionales = encripta;

				}
				catch (Exception excepcion)
				{
					throw new ApplicationException(string.Format("Error en la serialización de los datos de la licencia. {0}", excepcion.Message), excepcion.InnerException);
				}

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				Cl_ClienteRest<RespuestaLicenciamiento> cliente = new Cl_ClienteRest<RespuestaLicenciamiento>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
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
				//RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("ValidarLicencia: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_licencia) : string.Format("{0} {1}", excepcion.Message, url_licencia);
				throw new ApplicationException(mensaje);
			}
		}

		public RespuestaLicenciaHappgi ValidarLicenciaHappgi(Peticion objeto_principal, ValidarLicencia datos_licencia, string url_plataforma_licencia)
		{
			string url_licencia = url_plataforma_licencia;
			try
			{

				Cl_Configuraciones clase_configuraciones = new Cl_Configuraciones();
				//url_licencia = clase_configuraciones.ObtenerUrl(url_plataforma, AmbientesUrl.HGInetLicencia, 1);
				///se sustituye por parametro en el web config ya que el mismo proyecto busca una url de licenciamiento siendo el mismo proyecto

				if (string.IsNullOrWhiteSpace(url_licencia))
					url_licencia = Cl_InfoConfiguracionServer.ObtenerAppSettings("UrlPortalLicenciamiento");

				string ruta_rest = string.Format("{0}Api/ValidarLicenciamientoHappgi", url_licencia);
				RespuestaLicenciaHappgi respuesta = new RespuestaLicenciaHappgi();

				if (objeto_principal == null)
					throw new ApplicationException("Datos de la petición inválidos.");

				if (datos_licencia == null)
					throw new ApplicationException("Datos de la licencia inválidos.");

				if (string.IsNullOrWhiteSpace(objeto_principal.IdentificacionEmpresa) || objeto_principal.IdentificacionEmpresa.Equals("0"))
					throw new ApplicationException("Identificación de la empresa inválido.");

				try
				{
					string objeto_hijo = JsonConvert.SerializeObject(datos_licencia);

					//Construye la llave para la encripción de los datos.

					string llave_encripta = string.Empty;

					llave_encripta = string.Format("{0}#{1}", datos_licencia.IdentificacionEmpresa, datos_licencia.CodigoAplicativo);

					//Encripta el objeto de tipo datos_adicionales en formato json empleando la llave de seguridad.
					string encripta = Cl_Seguridad.Encryption(objeto_hijo, llave_encripta);

					//Asigna la encripción de los datos al objeto principal.
					objeto_principal.DatosAdicionales = encripta;

				}
				catch (Exception excepcion)
				{
					throw new ApplicationException(string.Format("Error en la serialización de los datos de la licencia. {0}", excepcion.Message), excepcion.InnerException);
				}

				//Realiza la petición a la Api/ValidarLicenciamiento enviando los datos de la petición en b64.
				Cl_ClienteRest<RespuestaLicenciaHappgi> cliente = new Cl_ClienteRest<RespuestaLicenciaHappgi>(string.Format("{0}", ruta_rest), TipoContenido.Applicationjson.GetHashCode(), "");
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
				//RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("ValidarLicencia: Ruta Conexión: {0}; {1}", url_principal, JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				string mensaje = excepcion.InnerException != null ? string.Format("{0} {1} {2}", excepcion.Message, excepcion.InnerException.Message, url_licencia) : string.Format("{0} {1}", excepcion.Message, url_licencia);
				throw new ApplicationException(mensaje);
			}
		}

		/// <summary>
		/// Obtiene los datos de licencia.
		/// </summary>
		/// <param name="objeto_principal"></param>
		/// <param name="datos_licencia"></param>
		/// <param name="url_plataforma">url de plataforma de servicios para obtener la ruta de licencia.</param>
		/// <param name="ruta_fisica">ruta fisica para el almacenamiento de la información</param>
		/// <returns></returns>
		public RespuestaLicenciamiento ProcesarLicencia(Peticion objeto_principal, ValidarLicencia datos_licencia, string url_plataforma, string ruta_fisica, bool eliminar_archivo = false)
		{
			try
			{
				RespuestaLicenciamiento respuesta_licencia = null;

				string ruta_licencia = string.Format(@"{0}\{1}\{2}", ruta_fisica, RecursoDms.CarpetaPrincipal, RecursoDms.CarpetaLicencia).Replace("\\", @"\");
				string ruta_licencia_completa = string.Format(@"{0}\{1}.json", ruta_licencia, objeto_principal.IdentificacionEmpresa);

				bool valida_licencia = true;

				if (eliminar_archivo)
				{
					try
					{
						Cl_Archivo.Borrar(ruta_licencia_completa);
					}
					catch (Exception) { }
				}
				else
				{
					//validar existencia de archivo.
					if (File.Exists(ruta_licencia_completa))
					{
						var webRequest = WebRequest.Create(ruta_licencia_completa);

						using (var response = webRequest.GetResponse())
						using (var content = response.GetResponseStream())
						using (var reader = new StreamReader(content))
						{
							string json = reader.ReadToEnd();

							if (!string.IsNullOrWhiteSpace(json))
							{
								try
								{
									respuesta_licencia = JsonConvert.DeserializeObject<RespuestaLicenciamiento>(json);

									if (respuesta_licencia != null)
									{
										if (respuesta_licencia.DatosAdicionales != null)
										{
											//Valida si se debe validar o no la licencia.
											if (respuesta_licencia.DatosAdicionales.FechaProximaValidacion != null)
												if (respuesta_licencia.DatosAdicionales.FechaProximaValidacion.Date >= Cl_Fecha.GetFecha().Date)
													valida_licencia = false;
										}
										else
											valida_licencia = true;
									}
								}
								catch (Exception)
								{
									valida_licencia = true;
								}
							}
						}
					}
				}

				//sino existe, consultar ruta de licenciamiento y almacenar información si la licencia es éxitosa.
				if (valida_licencia)
				{
					respuesta_licencia = ValidarLicencia(objeto_principal, datos_licencia, url_plataforma);

					if (respuesta_licencia != null)
					{
						//Si la Notifiacion contiene datos, retorna exc
						if (respuesta_licencia.Notificacion != null)
							throw new ApplicationException(respuesta_licencia.Notificacion.Mensaje);
						else
							AlmacenarLicencia(ruta_licencia, ruta_licencia_completa, respuesta_licencia);
					}
				}

				return respuesta_licencia;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		public RespuestaLicenciaHappgi ProcesarLicenciaHappgi(Peticion objeto_principal, ValidarLicencia datos_licencia, string url_plataforma_licencia, string ruta_fisica, bool eliminar_archivo = false)
		{
			try
			{
				RespuestaLicenciaHappgi respuesta_licencia = null;

				string ruta_licencia = string.Format(@"{0}\{1}\{2}", ruta_fisica, RecursoDms.CarpetaPrincipal, RecursoDms.CarpetaLicencia).Replace("\\", @"\");

				string ruta_licencia_completa = string.Format(@"{0}\{1}.json", ruta_licencia, objeto_principal.IdentificacionEmpresa);

				bool valida_licencia = true;

				if (eliminar_archivo)
				{
					try
					{
						Cl_Archivo.Borrar(ruta_licencia_completa);
					}
					catch (Exception) { }
				}
				else
				{               //validar existencia de archivo.
					if (File.Exists(ruta_licencia_completa))
					{
						var webRequest = WebRequest.Create(ruta_licencia_completa);

						using (var response = webRequest.GetResponse())
						using (var content = response.GetResponseStream())
						using (var reader = new StreamReader(content))
						{
							string json = reader.ReadToEnd();

							if (!string.IsNullOrWhiteSpace(json))
							{
								try
								{
									respuesta_licencia = JsonConvert.DeserializeObject<RespuestaLicenciaHappgi>(json);

									if (respuesta_licencia != null)
									{
										if (respuesta_licencia.IP.Contains(clave_migracion))
											throw new ApplicationException(mensaje_migracion);

										//Valida si se debe validar o no la licencia.
										if (respuesta_licencia.FechaProximaValidacion != null)
											if (respuesta_licencia.FechaProximaValidacion.Date >= Cl_Fecha.GetFecha().Date)
												valida_licencia = false;
											else
												valida_licencia = true;
									}
								}
								catch (Exception)
								{
									valida_licencia = true;
								}
							}
						}
					}
				}

				//sino existe, consultar ruta de licenciamiento y almacenar información si la licencia es éxitosa.
				if (valida_licencia)
				{
					if (string.IsNullOrWhiteSpace(url_plataforma_licencia))
						url_plataforma_licencia = Cl_InfoConfiguracionServer.ObtenerAppSettings("UrlPortalLicenciamiento");

					respuesta_licencia = ValidarLicenciaHappgi(objeto_principal, datos_licencia, url_plataforma_licencia);

					if (respuesta_licencia != null)
					{
						// si la respuesta es Ok, se almacena la información de la licencia.
						if (respuesta_licencia.Notificacion != null)
						{
							if (respuesta_licencia.Notificacion.Codigo == NotificacionCodigo.OK)
							{
								if (respuesta_licencia.IP.Contains(clave_migracion))
									throw new ApplicationException(mensaje_migracion);

								AlmacenarLicencia(ruta_licencia, ruta_licencia_completa, respuesta_licencia);
							}
						}
					}

				}

				respuesta_licencia.ActualizaRegistro = valida_licencia;

				return respuesta_licencia;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		public static bool AlmacenarLicencia(string ruta_licencia, string ruta_licencia_completa, object respuesta_licencia)
		{
			try
			{
				try
				{
					Cl_Archivo.Borrar(ruta_licencia_completa);
				}
				catch (Exception)
				{
				}

				if (!Cl_Directorio.ValidarExistencia(ruta_licencia))
					Cl_Directorio.CrearDirectorio(ruta_licencia);

				string json_licencia = JsonConvert.SerializeObject(respuesta_licencia);

				//Almacena el archivo en los dms
				System.IO.File.WriteAllText(ruta_licencia_completa, json_licencia);

				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static RespuestaLicenciaHappgi LicenciaTemporalHappgi(RespuestaLicenciaHappgi licencia_temporal, string ruta_fisica, bool nueva_licencia = false)
		{
			try
			{
				RespuestaLicenciaHappgi respuesta_licencia = null;

				string ruta_licencia = string.Format(@"{0}\{1}\{2}", ruta_fisica, RecursoDms.CarpetaPrincipal, RecursoDms.CarpetaLicencia).Replace("\\", @"\");

				string ruta_licencia_completa = string.Format(@"{0}\{1}.json", ruta_licencia, licencia_temporal.TerceroIdentificacion);

				if (nueva_licencia)
				{
					respuesta_licencia = licencia_temporal;
					respuesta_licencia.FechaContrato = Cl_Fecha.GetFecha().Date;
					respuesta_licencia.FechaVencimiento = Cl_Fecha.GetFecha().AddMonths(12).Date;
					respuesta_licencia.FechaProximaValidacion = Cl_Fecha.GetFecha().AddDays(7);
					respuesta_licencia.FechaVencimientoGracia = licencia_temporal.FechaProximaValidacion;
					respuesta_licencia.CompaniaCodigo = "1";

					for (int i = 0; i < respuesta_licencia.LicenciasRegistradas.Count; i++)
					{
						respuesta_licencia.LicenciasRegistradas[i].NumeroCompanias = 1;
						respuesta_licencia.LicenciasRegistradas[i].NumeroEmpresas = 1;
						respuesta_licencia.LicenciasRegistradas[i].Notificacion = new Notificacion();
					}
				}
				else
				{
					if (File.Exists(ruta_licencia_completa))
					{
						var webRequest = WebRequest.Create(ruta_licencia_completa);

						using (var response = webRequest.GetResponse())
						using (var content = response.GetResponseStream())
						using (var reader = new StreamReader(content))
						{
							string json = reader.ReadToEnd();

							if (!string.IsNullOrWhiteSpace(json))
							{
								respuesta_licencia = JsonConvert.DeserializeObject<RespuestaLicenciaHappgi>(json);

								respuesta_licencia.ProductoCodigo = licencia_temporal.ProductoCodigo;
								respuesta_licencia.ProductoDescripcion = licencia_temporal.ProductoDescripcion;
								respuesta_licencia.FechaVencimiento = licencia_temporal.FechaVencimiento;
								respuesta_licencia.FechaProximaValidacion = Cl_Fecha.GetFecha().AddDays(7);

								for (int i = 0; i < licencia_temporal.LicenciasRegistradas.Count; i++)
								{
									licencia_temporal.LicenciasRegistradas[i].Notificacion = new Notificacion();
								}

								respuesta_licencia.LicenciasRegistradas = licencia_temporal.LicenciasRegistradas;
							}
						}
					}
				}

				AlmacenarLicencia(ruta_licencia, ruta_licencia_completa, respuesta_licencia);

				return respuesta_licencia;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		public RespuestaLicenciamiento ProcesarLicenciaEcommerce(Peticion objeto_principal, ValidarLicencia datos_licencia, string url_plataforma, string identificacion_empresa, string ruta_fisica)
		{
			try
			{
				RespuestaLicenciamiento respuesta_licencia = null;

				string ruta_licencia = string.Format(@"{0}\dms\Licencias\Ecommerce\", ruta_fisica);
				string ruta_licencia_completa = string.Format(@"{0}{1}.json", ruta_licencia, identificacion_empresa);


				//validar existencia de archivo.
				if (File.Exists(ruta_licencia_completa))
				{
					var webRequest = WebRequest.Create(ruta_licencia_completa);

					using (var response = webRequest.GetResponse())
					using (var content = response.GetResponseStream())
					using (var reader = new StreamReader(content))
					{
						string json = reader.ReadToEnd();

						if (!string.IsNullOrWhiteSpace(json))
						{
							respuesta_licencia = JsonConvert.DeserializeObject<RespuestaLicenciamiento>(json);
						}
					}
				}

				return respuesta_licencia;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		public static bool RecargaDocsFETemporal(RecargaDocsFE datos_recarga, string ruta_fisica)
		{
			try
			{
				string ruta_recarga_plan = string.Format(@"{0}\{1}\{2}", ruta_fisica, RecursoDms.CarpetaPrincipal, RecursoDms.CarpetaRecargasFE).Replace("\\", @"\");

				string ruta_licencia_completa = string.Format(@"{0}\{1}.json", ruta_recarga_plan, datos_recarga.TerceroIdentificacion);

				try
				{
					try
					{
						Cl_Archivo.Borrar(ruta_licencia_completa);
					}
					catch (Exception)
					{
					}

					if (!Cl_Directorio.ValidarExistencia(ruta_recarga_plan))
						Cl_Directorio.CrearDirectorio(ruta_recarga_plan);

					string json_recarga_docs = JsonConvert.SerializeObject(datos_recarga);

					//Almacena el archivo en los dms
					System.IO.File.WriteAllText(ruta_licencia_completa, json_recarga_docs);

					return true;
				}
				catch (Exception)
				{
					throw;
				}
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		public RespuestaLicenciamiento ProcesarLicenciaEcommerce2(string identificacion_empresa, string ruta_fisica)
		{
			try
			{
				RespuestaLicenciamiento respuesta_licencia = null;

				string ruta_licencia = string.Format(@"{0}\dms\Licencias\Ecommerce\", ruta_fisica);

				string ruta_licencia_completa = string.Format(@"{0}{1}.json", ruta_licencia, identificacion_empresa);

				//validar existencia de archivo.
				if (File.Exists(ruta_licencia_completa))
				{
					var webRequest = WebRequest.Create(ruta_licencia_completa);

					using (var response = webRequest.GetResponse())
					using (var content = response.GetResponseStream())
					using (var reader = new StreamReader(content))
					{
						string json = reader.ReadToEnd();

						if (!string.IsNullOrWhiteSpace(json))
						{
							try
							{
								respuesta_licencia = JsonConvert.DeserializeObject<RespuestaLicenciamiento>(json);

								if (respuesta_licencia != null)
								{
									//Valida si se debe validar o no la licencia.
									if (respuesta_licencia.DatosAdicionales.FechaProximaValidacion != null)
										if (respuesta_licencia.DatosAdicionales.FechaProximaValidacion.Date < Cl_Fecha.GetFecha().Date)
											throw new ApplicationException("No se encontraron contratos vigentes asociados al producto.");
								}
							}
							catch (Exception exception)
							{
								throw new ApplicationException(exception.Message, exception.InnerException);
							}
						}
					}
				}

				return respuesta_licencia;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

		public bool ELiminarJsonLicencias(string ruta_fisica)
		{
			try
			{
				string ruta_licencia = string.Format(@"{0}\{1}\{2}", ruta_fisica, RecursoDms.CarpetaPrincipal, RecursoDms.CarpetaLicencia).Replace("\\", @"\");
				Cl_Directorio.BorrarArchivos(ruta_licencia);

				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Elimina el json de una licencia en especifica
		/// </summary>
		/// <param name="identificacion"></param>
		/// <returns></returns>
		public string ELiminarJsonLicenciaTercero(string identificacion)
		{
			try
			{
				Utilitario.Almacenar("Ctrl ELiminarJsonLicenciaTercero ", identificacion);

				string ruta_fisica = Cl_InfoConfiguracionServer.ObtenerAppSettings("RutaFisicaPlataforma");

				Utilitario.Almacenar("ruta_fisica " + ruta_fisica, identificacion);

				string ruta_licencia = string.Format(@"{0}\{1}\{2}", ruta_fisica, RecursoDms.CarpetaPrincipal, RecursoDms.CarpetaLicencia).Replace("\\", @"\");

				Utilitario.Almacenar("ruta_licencia " + ruta_licencia, identificacion);
				string ruta_licencia_completa = string.Format(@"{0}\{1}.json", ruta_licencia, identificacion);
				Utilitario.Almacenar("ruta_licencia_completa " + ruta_licencia_completa, identificacion);


				Cl_Archivo.Borrar(ruta_licencia_completa);

				return "Exito";
			}
			catch (Exception ex)
			{

				Utilitario.Almacenar("Error ruta_licencia_completa " + ex.Message, identificacion);
				throw;
			}
		}
	}
}
