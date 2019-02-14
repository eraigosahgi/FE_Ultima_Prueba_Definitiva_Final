using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Telerik.Reporting;

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
		public static DocumentoRespuesta GuardarFormato(object documento, TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
		{
			respuesta.DescripcionProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PDFGeneracion.GetHashCode()));
			respuesta.FechaUltimoProceso = Fecha.GetFecha();
			respuesta.IdProceso = ProcesoEstado.PDFGeneracion.GetHashCode();
			respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

			try
			{
				var documento_obj = (dynamic)null;
				documento_obj = documento;
				respuesta.UrlPdf = string.Empty;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;


				// valida formato en base 64 del objeto
				Formato formato_documento = (Formato)(dynamic)documento_obj.DocumentoFormato;
				//valido si el formato es enviado del ERP
				if (formato_documento.Codigo == -1 || formato_documento.Codigo == 0)
				{
					if (!string.IsNullOrEmpty(formato_documento.ArchivoPdf))
					{
						// almacena archivo en base64
						documento_result.NombrePdf = Ctl_Formato.GuardarArchivo(formato_documento.ArchivoPdf, documento_result);

						if (!string.IsNullOrWhiteSpace(documento_result.NombrePdf))
						{
							// url pública del pdf
							string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
							respuesta.UrlPdf = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombrePdf);

						}

					}
					else
					{
						throw new ArgumentException(string.Format("No se encontró Formato PDF del documento {0} ", documento_obj.Documento));
					}
				}
				else
				{

					string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
					string ruta = string.Format(@"{0}/{1}/", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
					Report reporte_pdf = new Report();

					/*
                     Para la contrucción de formatos se debe tener en cuenta el envío del parametro TipoDocumento de caracter obligatorio
                     para el reporte pdf, ya que este valor es implementado para la carga de información desde cada formato.
                     */

					//Obtiene el diseño del formato en la base de datos y realiza el proceso de creación del pdf.
					//sino hay un formato en base de datos con el formato especificado, toma los formatos existentes en el proyecto
					Ctl_Formatos clase_formatos = new Ctl_Formatos();
					TblFormatos datos_formato = clase_formatos.ObtenerFormato(formato_documento.Codigo, documentoBd.StrEmpresaFacturador, TipoFormato.FormatoPDF.GetHashCode());

					if (datos_formato != null)
					{
						XtraReportDesigner rep = new XtraReportDesigner();

						MemoryStream datos = new MemoryStream(datos_formato.Formato);
						rep.LoadLayoutFromXml(datos);

						List<DocumentoDetalle> detalles_formato = new List<DocumentoDetalle>();
						//Recorre los detalles y agrega los items visibles.
						foreach (var item in documento_obj.DocumentoDetalles)
						{
							if (item.OcultarItem == 0)
								detalles_formato.Add(item);
						}

						documento_obj.DocumentoDetalles = detalles_formato;

						rep.DataSource = documento_obj;
						HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
						x.GenerarPdfDev(rep, documentoBd.StrEmpresaFacturador);
					}
					else
					{
						throw new ApplicationException(string.Format("El formato N.{0} no se encuentra disponible para el facturador {1}.", formato_documento.Codigo, documentoBd.StrEmpresaFacturador));
					}

					respuesta.UrlPdf = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

				}

				respuesta.DescripcionProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PDFAlmacenamiento.GetHashCode()));
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdProceso = ProcesoEstado.PDFAlmacenamiento.GetHashCode();
				respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

				//Actualiza el registro en la base de datos.
				documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
				documentoBd.IdCategoriaEstado = CategoriaEstado.Recibido.GetHashCode();

			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento PDF. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
			}

			return respuesta;
		}

	}
}
