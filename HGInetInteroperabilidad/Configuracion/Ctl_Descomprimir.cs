using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.Modelo;

namespace HGInetInteroperabilidad.Configuracion
{
	public class Ctl_Descomprimir
	{

		public static RegistroListaDocRespuesta Procesar(RegistroListaDoc datos, string proveedor_emisor)
		{


			RegistroListaDocRespuesta datos_respuesta = new RegistroListaDocRespuesta();

			if (datos == null)
			{
				datos_respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
				datos_respuesta.trackingIds = null;
				datos_respuesta.mensajeGlobal = string.Format("{2}|No se obtuvo informacion de {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio), RespuestaInterOperabilidad.Zipvacio.GetHashCode());

			}
			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//Obtengo el proveedor emisor
			TblConfiguracionInteroperabilidad proveedor = new TblConfiguracionInteroperabilidad();

			Ctl_ConfiguracionInteroperabilidad configuracion = new Ctl_ConfiguracionInteroperabilidad();

			proveedor = configuracion.Obtener(proveedor_emisor);

			string destino = string.Empty;

			//Obtengo archivos para procesar
			if (!Archivo.ValidarExistencia(string.Format(@"{0}{1}{2}\{3}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, proveedor.StrIdSeguridad, datos.nombre)))
			{
				datos_respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
				datos_respuesta.trackingIds = null;
				datos_respuesta.mensajeGlobal = string.Format("{2}|No se obtuvo informacion de {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio), RespuestaInterOperabilidad.Zipvacio.GetHashCode());

			}
			else
			{
				if (datos.documentos.Count > 100)
				{
					datos_respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
					datos_respuesta.trackingIds = null;
					datos_respuesta.mensajeGlobal = string.Format("{2}|El zip {0} contiene mas de 100 documentos de {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ZipSuperaMaximo), RespuestaInterOperabilidad.ZipSuperaMaximo.GetHashCode());

				}
				else
				{
					string ruta_fisica_zip = string.Format("{0}{1}{2}\\{3}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, proveedor.StrIdSeguridad, datos.nombre);

					string ruta_archivos = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion, proveedor.StrIdSeguridad);

					Directorio.CrearDirectorio(ruta_archivos);

					try
					{
						// Ingreso a la Carpeta para validar informacion
						using (ZipArchive file = ZipFile.OpenRead(ruta_fisica_zip))
						{

							//Recorro la carpeta
							foreach (ZipArchiveEntry archivo in file.Entries)
							{
								//Valida que el archivo no tenga ninguna de estas extensiones
								if ((archivo.FullName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) || (archivo.FullName.EndsWith(".bat", StringComparison.OrdinalIgnoreCase))
									|| (archivo.FullName.EndsWith(".accdb", StringComparison.OrdinalIgnoreCase)) || (archivo.FullName.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase)))
								{
									archivo.Delete();
								}
							}
							// Obtiene la ruta completa para garantizar que se eliminen los segmentos relativos.
							destino = Path.GetFullPath(Path.Combine(ruta_archivos, Path.GetFileNameWithoutExtension(datos.nombre)));

							// genera la descompresión del archivo zip
							file.ExtractToDirectory(destino);

							file.Dispose();
						}


						//Archivo.Borrar(@"C:\inetpub\vhosts\mifacturaenlinea.com.co\fileshab.mifacturaenlinea.com.co\interoperabilidad\publico\f791dbce-c640-4f32-99d4-5966a573701f\811021438_123456_86cfbb40-5dde-4908-bbd1-fc2ae7c6e8b6.zip");
						//ruta_fisica_zip = string.Format(@"{0}{1}{2}\{3}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp.Replace("\\", @"\"), proveedor.StrIdSeguridad, datos.nombre);


						//Envia la informacion para procesarla
						datos_respuesta = Ctl_Recepcion.Procesar(datos, destino, proveedor.StrIdentificacion);

					}
					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
						datos_respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
						datos_respuesta.trackingIds = null;
						datos_respuesta.mensajeGlobal = string.Format("{2}|Error al descomprimir {0} {1}", datos.nombre, Enumeracion.GetDescription(RespuestaInterOperabilidad.ErrorInternoReceptor), RespuestaInterOperabilidad.ErrorInternoReceptor);
						//throw new ApplicationException(string.Format("Error al descomprimir {0} Detalle: {1}", datos.nombre, excepcion.Message));
					}
				}
			}

			//Aqui elimino el archivo Zip si todo esta OK
			try
			{
				Archivo.Borrar(string.Format(@"{0}{1}{2}\{3}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, proveedor.StrIdSeguridad, datos.nombre));
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);

			}

			return datos_respuesta;
		}


		/// <summary>
		/// Metodo para procesar documentos recibidos por correo
		/// </summary>
		/// <returns></returns>
		public static bool ProcesarArchivosCorreo()
		{

			bool datos_respuesta = false;

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			string ruta_archivos = string.Format(@"{0}\{1}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion);

			string[] directorios_Obligado = Directorio.ObtenerSubdirectoriosDirectorio(ruta_archivos);

			string ruta_dir_facturador_borrar = string.Empty;

			string ruta_dir_archivos_borrar = string.Empty;

			try
			{
				if (directorios_Obligado != null && directorios_Obligado.Length > 0)
				{

					foreach (var directorio in directorios_Obligado)
					{

						try
						{
							if (!string.IsNullOrEmpty(ruta_dir_facturador_borrar))
							{
								try
								{
									EliminarDirectorios(ruta_dir_facturador_borrar);

									ruta_dir_facturador_borrar = string.Empty;
								}
								catch (Exception excepcion)
								{
									RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.eliminacion, ruta_dir_facturador_borrar);
								}
							}
						}
						catch (Exception)
						{ }

						int i = 0;

						string[] directorios_archivos = Directorio.ObtenerSubdirectoriosDirectorio(directorio);

						bool procesado = false;

						if (directorios_archivos != null && directorios_archivos.Length > 0)
						{

							try
							{
								if (!string.IsNullOrEmpty(ruta_dir_archivos_borrar))
								{
									try
									{
										EliminarDirectorios(ruta_dir_archivos_borrar);

										ruta_dir_archivos_borrar = string.Empty;
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.eliminacion, ruta_dir_archivos_borrar);
									}
								}
							}
							catch (Exception)
							{ }

							foreach (var item in directorios_archivos)
							{

								try
								{
									if (!string.IsNullOrEmpty(ruta_dir_archivos_borrar))
									{
										try
										{
											EliminarDirectorios(ruta_dir_archivos_borrar);

											ruta_dir_archivos_borrar = string.Empty;
										}
										catch (Exception excepcion)
										{
											RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.eliminacion, ruta_dir_archivos_borrar);
										}
									}
								}
								catch (Exception)
								{}

								var archivos_recibidos = Directorio.ObtenerArchivosDirectorio(item);

								if (archivos_recibidos != null)
								{
									try
									{
										procesado = Ctl_Recepcion.ProcesarCorreo(item);
										datos_respuesta = procesado;

										if (procesado == true)
										{
											ruta_dir_archivos_borrar = item;
											//Directorio.BorrarArchivos(item);
											//Directorio.BorrarDirectorio(item);
										}
										else
										{
											throw new ApplicationException("No se proceso el correo");
										}
									}
									catch (Exception ex)
									{
										string ruta_dir = string.Format(@"{0}\Archivos", ruta_archivos.Replace("recepcion", "no procesados"));
										Directorio.CrearDirectorio(ruta_dir);
										string nom_dir = Path.GetFileName(directorios_archivos[i]);
										try
										{
											Directorio.MoverDirectorio(item, string.Format(@"{0}\{1}", ruta_dir, nom_dir));
										}
										catch (Exception excepcion)
										{
											RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.exportar, excepcion.Message);
										}
										if (Archivo.ValidarExistencia(string.Format(@"{0}\{1}.mail", directorio,Path.GetFileName(directorios_archivos[i]))))
										{
											try
											{
												Archivo.Mover(string.Format(@"{0}\{1}.mail", directorio, Path.GetFileName(directorios_archivos[i])), ruta_dir, string.Format("Mail - {0}.mail", Path.GetFileName(directorios_archivos[i])));
											}
											catch (Exception excepcion)
											{
												RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.exportar, excepcion.Message);
											}
										}
											
									}
								}
								//Directorio.BorrarArchivos(item);
								//try
								//{
								//	Directorio.BorrarDirectorio(item);
								//}
								//catch (Exception)
								//{ }
							}

							ruta_dir_facturador_borrar = directorio;

							//Directorio.BorrarArchivos(directorio);
							try
							{
								EliminarDirectorios(ruta_dir_archivos_borrar);

								ruta_dir_archivos_borrar = string.Empty;
							}
							catch (Exception excepcion)
							{
								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.eliminacion, ruta_dir_archivos_borrar);
							}
						}
						else
						{
							ruta_dir_facturador_borrar = directorio;
						}

						i++;
					}

					try
					{
						if (!string.IsNullOrEmpty(ruta_dir_facturador_borrar))
						{
							try
							{
								EliminarDirectorios(ruta_dir_facturador_borrar);

								ruta_dir_facturador_borrar = string.Empty;
							}
							catch (Exception excepcion)
							{
								RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.eliminacion, ruta_dir_facturador_borrar);
							}
						}
					}
					catch (Exception)
					{ }

				}
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, excepcion.Message);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

			return datos_respuesta;
		}
		
		public static string Procesar(string archivo_zip, string carpeta_descomprimir = "")
		{
			string destino = string.Empty;
			
			// Obtiene la ruta completa para garantizar que se eliminen los segmentos relativos
			if (string.IsNullOrEmpty(carpeta_descomprimir))
				destino = Path.GetFullPath(Path.Combine(archivo_zip.Replace(Path.GetFileName(archivo_zip), ""), Path.GetFileNameWithoutExtension(archivo_zip)));
			else
				destino = carpeta_descomprimir;
				
			//Obtengo archivos para procesar
			if (!Archivo.ValidarExistencia(archivo_zip))
			{
				throw new ApplicationException(string.Format("{2}|No se obtuvo información de {0} {1}", archivo_zip, Enumeracion.GetDescription(RespuestaInterOperabilidad.Zipvacio), RespuestaInterOperabilidad.Zipvacio.GetHashCode()));
			}
			else
			{
				try
				{
					// validar información del archivo zip
					using (ZipArchive file = ZipFile.OpenRead(archivo_zip))
					{
						foreach (ZipArchiveEntry archivo in file.Entries)
						{
							// valida que los archivos no tengan extensiones definidas
							if ((archivo.FullName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) || (archivo.FullName.EndsWith(".bat", StringComparison.OrdinalIgnoreCase))
								|| (archivo.FullName.EndsWith(".accdb", StringComparison.OrdinalIgnoreCase)) || (archivo.FullName.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase))
								|| (archivo.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
							{
								archivo.Delete();
							}
						}

						// genera la descompresión del archivo zip
						try
						{ 
							file.ExtractToDirectory(destino);
						}
						catch (Exception excepcion)
						{
							string msg = string.Format("Error al extaer los archivos del zip '{0}'", destino);
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, msg);
						}

						file.Dispose();
					}


					// se elimina el archivo ZIP
					try
					{
						Archivo.Borrar(archivo_zip);
					}
					catch (Exception excepcion)
					{
						string msg = string.Format("Error al eliminar el archivo '{0}'", archivo_zip);
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, msg);

					}

				}
				catch (Exception excepcion)
				{
					throw new ApplicationException(string.Format("Error al descomprimir {0} Detalle: {1}", archivo_zip, excepcion.Message));
				}
			}

			return destino;
		}


		/// <summary>
		/// Sonda para descargar los correos de interoperabilidad y alojarlos en una ruta para procesarlos
		/// </summary>
		/// <returns></returns>
		public async Task SondaProcesarCorreos()
		{
			try
			{
				var Tarea = TareaProcesarCorreos();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error ejecutando la sonda para descargar los correos electrónicos");
				RegistroLog.EscribirLog(excepcion, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.lectura, msg);
			}

		}

		public async Task TareaProcesarCorreos()
		{
			await Task.Factory.StartNew(() =>
			{
				ProcesarArchivosCorreo();
			});
		}

		public static void EliminarDirectorios(string ruta)
		{
			try
			{
				Directorio.BorrarArchivos(ruta);
				Directorio.BorrarDirectorio(ruta);
			}
			catch (Exception exec)
			{
				RegistroLog.EscribirLog(exec, LibreriaGlobalHGInet.RegistroLog.MensajeCategoria.Sonda, LibreriaGlobalHGInet.RegistroLog.MensajeTipo.Error, LibreriaGlobalHGInet.RegistroLog.MensajeAccion.eliminacion, "Error eliminando directorios y Archivos");
				throw new ApplicationException(exec.Message);
			}
		}

	}
}
