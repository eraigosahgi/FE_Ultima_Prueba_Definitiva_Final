using HGInetDIANServicios;
using HGInetDIANServicios.DianFactura;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Genera el xml con la información del documento en formato UBL
		/// </summary>        
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="empresa">información del facturador electrónico en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static AcuseRecibo EnviarDian(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
		{
			HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
			try
			{
				respuesta.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();

				acuse = Ctl_DocumentoDian.Enviar(documento_result, empresa);
			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				return null;
			}

			respuesta.Cufe = documento_result.CUFE;
			/*
			// url pública del xml
			
			respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);
			*/
			// url pública del zip
			string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_result.IdSeguridadTercero.ToString());
			string url_ppal_zip = string.Format(@"{0}{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreZip);

			documentoBd.StrCufe = respuesta.Cufe;
			//documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
			documentoBd.StrUrlArchivoZip = url_ppal_zip;
			documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
			documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

			Ctl_Documento documento_tmp = new Ctl_Documento();
			documento_tmp.Actualizar(documentoBd);

			//Se da una pausa en proceso para que el servicio de la DIAN termine la validacion del documento
			System.Threading.Thread.Sleep(5000);

			return acuse;
		}

		public static void ConsultarDian()
		{

		}

		/// <summary>
		/// Consulta estado de documentos en la DIAN
		/// </summary>
		/// <param name="documentoBd">Documento en BD</param>
		/// <param name="empresa">Obligado a facturar</param>
		/// <param name="respuesta">Objeto de respuesta</param>
		/// <returns>Segun la respuesta de la DIAN cambia el estado del documento</returns>
		public static DocumentoRespuesta Consultar(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta)
		{

			DateTime fecha_actual = Fecha.GetFecha();
			Ctl_Documento documento_tmp = new Ctl_Documento();



			try
			{
				string IdSoftware = null;
				string PinSoftware = null;
				string clave = null;
				string url_ws_consulta = null;


				// carpeta del xml
				string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// ruta del xml
				string archivo_xml = string.Format(@"{0}{1}.xml", documentoBd.StrPrefijo, documentoBd.IntNumero.ToString());

				// ruta del xml
				string ruta_xml = string.Format(@"{0}{1}", carpeta_xml, archivo_xml);
                
                // elimina el archivo xml si existe
                if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					// obtiene los datos de prueba del proveedor tecnológico de la DIAN
					DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					clave = data_dian_habilitacion.ClaveAmbiente;
					url_ws_consulta = data_dian_habilitacion.UrlWSConsultaTransacciones;
				}
				else
				{
					// obtiene los datos del proveedor tecnológico de la DIAN
					DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
					clave = data_dian.ClaveAmbiente;
					url_ws_consulta = data_dian.UrlWSConsultaTransacciones;
				}


				HGInetDIANServicios.DianResultadoTransacciones.DocumentosRecibidos resultado = Ctl_ConsultaTransacciones.Consultar(Guid.NewGuid(), IdSoftware, clave, documentoBd.IntDocTipo, documentoBd.StrPrefijo, documentoBd.IntNumero.ToString(), empresa.StrIdentificacion, documentoBd.DatFechaDocumento, documentoBd.StrCufe, url_ws_consulta, ruta_xml);

				ConsultaDocumento resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccion(resultado);

				//Url publica de la respuesta de la DIAN en xml
				string url_ppal_respuesta = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", empresa.StrIdSeguridad.ToString());

				// se indica la respuesta de la DIAN
				respuesta.EstadoDian = new RespuestaDian();
				respuesta.EstadoDian.CodigoRespuesta = resultado_doc.CodigoEstadoDian;
				respuesta.EstadoDian.Descripcion = resultado_doc.EstadoDianDescripcion;
				respuesta.EstadoDian.EstadoDocumento = resultado_doc.Estado.GetHashCode();
				respuesta.EstadoDian.FechaConsulta = fecha_actual;
				respuesta.EstadoDian.UrlXmlRespuesta = string.Format(@"{0}{1}/{2}", url_ppal_respuesta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian, archivo_xml);


				string detalle_dian = string.Empty;

				if (resultado_doc.Estado == EstadoDocumentoDian.Rechazado)
				{
					fecha_actual = Fecha.GetFecha();
					respuesta.DescripcionProceso = "Termina proceso.";
					respuesta.FechaUltimoProceso = fecha_actual;
					respuesta.IdProceso = ProcesoEstado.FinalizacionErrorDian.GetHashCode();
					respuesta.ProcesoFinalizado = 1;

					respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
					respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION;
					respuesta.Error.Fecha = fecha_actual;
									
					respuesta.Error.Mensaje = string.Format("Documento rechazado DIAN: {0} - Cod. {1} ", resultado_doc.EstadoDianDescripcion, resultado_doc.CodigoEstadoDian);

					//Actualiza Documento en Base de Datos
					documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
					documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

					documento_tmp.Actualizar(documentoBd);

				}
				return respuesta;
			}

			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la consulta del estado del documento en la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
				throw excepcion;
			}



		}


	}
}
