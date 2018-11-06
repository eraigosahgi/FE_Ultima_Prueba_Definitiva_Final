using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
						LogExcepcion.Guardar(excepcion);
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
				LogExcepcion.Guardar(excepcion);

			}

			return datos_respuesta;
		}


	}
}
