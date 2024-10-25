using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Guarda el formato PDF del documento
		/// </summary>
		/// <param name="documento_obj">información del documento</param>
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static DocumentoRespuesta GuardarAnexo(Anexo anexo, TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
		{

			try
			{
				if (anexo != null)
				{

					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					// ruta física de los adjuntos
					string carpeta_adjuntos = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
					carpeta_adjuntos = string.Format(@"{0}\{1}", carpeta_adjuntos, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos);

					// valida la existencia de la carpeta
					carpeta_adjuntos = Directorio.CrearDirectorio(carpeta_adjuntos);

					// ruta del archivo
					string ruta_adjuntos = string.Format("{0}{1}.zip", carpeta_adjuntos, documento_result.NombreXml);

					// url pública del Anexo
					string url_ppal_anexo = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());


					if (!string.IsNullOrEmpty(anexo.Archivo))
					{

						//convierte el array de byte en archivo
						try
						{
							File.WriteAllBytes(ruta_adjuntos, Convert.FromBase64String(anexo.Archivo));
						}
						catch (Exception e)
						{

							if (e.Message.Contains("Longitud no válida"))
								throw new ApplicationException("El tamaño del archivo zip supera el maximo permitido");

						}

						if (!Archivo.ValidarExistencia(ruta_adjuntos))
							throw new ApplicationException(string.Format("No se encontro el archivo {0}", ruta_adjuntos));

						FileInfo adjunto = new FileInfo(ruta_adjuntos);

						//Valida que el archivo no supere el peso de 2MB
						if (adjunto.Length > Convert.ToInt32(Constantes.TamanoAnexo))
						{
							Archivo.Borrar(ruta_adjuntos);
							throw new ApplicationException("El tamaño del archivo zip supera el maximo permitido");

						}

						respuesta.UrlAnexo = string.Format(@"{0}/{1}/{2}.zip", url_ppal_anexo, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos, documento_result.NombreXml);
						documentoBd.IntPesoAnexo = Convert.ToInt32(adjunto.Length);

					}
					else if (!string.IsNullOrEmpty(anexo.Url))
					{
						respuesta.UrlAnexo = anexo.Url;
					}

					documentoBd.StrUrlAnexo = respuesta.UrlAnexo;
					documentoBd.StrObservacionAnexo = anexo.Anotacion;

					//Ctl_Documento documento_tmp = new Ctl_Documento();
					//documentoBd = documento_tmp.Actualizar(documentoBd);
				}

			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del archivo Anexo. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				respuesta.IdEstado = CategoriaEstado.NoRecibido.GetHashCode();
				respuesta.DescripcionEstado = LibreriaGlobalHGInet.Funciones.Enumeracion.GetDescription(CategoriaEstado.NoRecibido);
				respuesta.IdProceso = ProcesoEstado.Validacion.GetHashCode();
				respuesta.DescripcionProceso = LibreriaGlobalHGInet.Funciones.Enumeracion.GetDescription(ProcesoEstado.Validacion);
			}

			return respuesta;

		}




        public static string RutaAnexos(TblDocumentos Documento)
        {
            try
            {
                string Ruta = "";
                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;                
                //Aqui busco la ubicacion de la carpeta del proveedor tecnologico, esperar por la ruta real
                string RutaProveedor = (string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadEnvio));
               
                string Guid_ProveedorReceptor = Documento.TblEmpresasFacturador.StrIdSeguridad.ToString();  //Prov_Envio.StrIdSeguridad.ToString();

                //string RutaCarpeta = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), Facturador);
                string RutaCarpeta = string.Format("{0}\\{1}\\{2}\\", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, Guid_ProveedorReceptor);
                //string RutaArchivos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
                //string RutaArchivosAcuse = string.Format(@"{0}\{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);
                string RutaAnexos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos);

                //Valido si el documento posee un archivo anexo (Fisico)
                if (Documento.IntPesoAnexo > 0 && !string.IsNullOrEmpty(Documento.StrUrlAnexo))
                {
                    Ruta= string.Format(@"{0}\\{1}", RutaAnexos, Path.GetFileName(Documento.StrUrlAnexo));                    
                }
                //Valido si el documento posee una ruta como archivo anexo 
                if (Documento.IntPesoAnexo == null && !string.IsNullOrEmpty(Documento.StrUrlAnexo))
                {
                    //Creo el archivo Comprimido
                    ZipArchive archivoAnexo = ZipFile.Open(string.Format("{0}\\{1}", RutaAnexos, Path.GetFileName(Documento.StrUrlAnexo)), ZipArchiveMode.Update);
                    
                    System.IO.File.WriteAllText(string.Format(@"{0}\\{1}.url", RutaAnexos, Path.GetFileName(Documento.StrUrlAnexo)), Documento.StrUrlAnexo);

                    archivoAnexo.CreateEntryFromFile(string.Format(@"{0}\\{1}.url", RutaAnexos, Path.GetFileName(Documento.StrUrlAnexo)),string.Format("{0}.url", Path.GetFileName(Documento.StrUrlAnexo)));

                    //Cierro el archivo zip
                    archivoAnexo.Dispose();

                    Ruta = string.Format("{0}\\{1}", RutaAnexos, Path.GetFileName(Documento.StrUrlAnexo));
                }

                return Ruta;
            //                archive.CreateEntryFromFile(string.Format("{0}{1}.{2}", RutaProveedor, Path.GetFileName(Documento.StrUrlAnexo), ".zip"), string.Format("{0}.{1}", Path.GetFileName(Documento.StrUrlAnexo), "zip"));


            }
            catch (Exception excepcion)
            {
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.creacion);
				return "";
            }
            
        }

	}
}
