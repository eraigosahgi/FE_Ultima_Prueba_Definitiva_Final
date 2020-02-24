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
using HGInetMiFacturaElectonicaData.Enumerables;
using Telerik.Reporting;
using HGInetUBLv2_1.DianListas;

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
		public static DocumentoRespuesta GuardarFormato(object documento, TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result, TblEmpresas facturador)
		{
			respuesta.DescripcionProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PDFGeneracion.GetHashCode()));
			respuesta.FechaUltimoProceso = Fecha.GetFecha();
			respuesta.IdProceso = ProcesoEstado.PDFGeneracion.GetHashCode();
			respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

			try
			{
				var documento_obj = (dynamic)null;
				documento_obj = documento;

				bool generar_pdf = true;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;


				// valida formato en base 64 del objeto
				Formato formato_documento = (Formato)(dynamic)documento_obj.DocumentoFormato;
				if (formato_documento != null)
				{
					if (!string.IsNullOrEmpty(formato_documento.ArchivoPdf))
					{
						// almacena archivo en base64
						documento_result.NombrePdf = Ctl_Formato.GuardarArchivo(formato_documento.ArchivoPdf, documento_result);
						respuesta.UrlPdf = string.Empty;

						if (!string.IsNullOrWhiteSpace(documento_result.NombrePdf))
						{

							// url pública del pdf
							string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());

							respuesta.UrlPdf = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombrePdf);

							documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
						}

						generar_pdf = false;
					}
				}

				if (generar_pdf)
				{

					string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
					string ruta = string.Format(@"{0}/{1}/", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
					Report reporte_pdf = new Report();
					bool reporte_archivado = true;
					/*
                     Para la contrucción de formatos se debe tener en cuenta el envío del parametro TipoDocumento de caracter obligatorio
                     para el reporte pdf, ya que este valor es implementado para la carga de información desde cada formato.
                     */

					//Obtiene el diseño del formato en la base de datos y realiza el proceso de creación del pdf.
					//sino hay un formato en base de datos con el formato especificado, toma los formatos existentes en el proyecto
					Ctl_Formatos clase_formatos = new Ctl_Formatos();
					TblFormatos datos_formato = clase_formatos.ObtenerFormato(formato_documento.Codigo, documentoBd.StrEmpresaFacturador, TipoFormato.FormatoPDF.GetHashCode());

					List<DocumentoDetalle> detalles_formato = new List<DocumentoDetalle>();
					//Recorre los detalles y agrega los items visibles.
					foreach (var item in documento_obj.DocumentoDetalles)
					{
						if (item.OcultarItem == 0)
							detalles_formato.Add(item);
						documento_obj.ValorDescuentoDet += item.DescuentoValor;
					}

					documento_obj.DocumentoDetalles = detalles_formato;

					//Se llena propiedad del medio de pago para poder mostrarla en la representacion grafica
					ListaMediosPago list_medio = new ListaMediosPago();
					ListaItem medio = list_medio.Items.Where(d => d.Codigo.Equals(documento_obj.TerminoPago.ToString())).FirstOrDefault();
					documento_obj.Descripcion_TerminoPago = medio.Descripcion;


					if (datos_formato != null)
					{
						reporte_archivado = false;
						XtraReportDesigner rep = new XtraReportDesigner();

						MemoryStream datos = new MemoryStream(datos_formato.Formato);
						rep.LoadLayoutFromXml(datos);

						rep.DataSource = documento_obj;
						HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
						x.GenerarPdfDev(rep, documentoBd.StrEmpresaFacturador);
					}
					else if(facturador.IntHabilitacion == Habilitacion.Produccion.GetHashCode())
					{
						switch (formato_documento.Codigo)
						{
							case 1:
								switch (documento_result.DocumentoTipo)
								{
									case TipoDocumento.Factura:
										reporte_pdf = new HGInetFacturaEReports.Facturas.Formato1();
										break;
									case TipoDocumento.NotaDebito:
										reporte_pdf = new HGInetFacturaEReports.NotasDebito.Formato1();
										break;
									case TipoDocumento.NotaCredito:
										reporte_pdf = new HGInetFacturaEReports.NotasCredito.Formato1();
										break;
								}
								break;
							case 2:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato2();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;
							case 3:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato3();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;
							case 4:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato4();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;
							case 5:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato5();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;

							default:
								switch (documento_result.DocumentoTipo)
								{
									case TipoDocumento.Factura:
										reporte_pdf = new HGInetFacturaEReports.Facturas.Formato1();
										break;
									case TipoDocumento.NotaDebito:
										reporte_pdf = new HGInetFacturaEReports.NotasDebito.Formato1();
										break;
									case TipoDocumento.NotaCredito:
										reporte_pdf = new HGInetFacturaEReports.NotasCredito.Formato1();
										break;
								}
								break;
						}
					}
					else
					{
						throw new ArgumentException(string.Format("No se encuentra registrado el codigo del formato {0} del Facturador {1}", formato_documento.Codigo,facturador.StrIdentificacion));
					}

					if (reporte_archivado)
					{
						//Asigna los datos al reporte y genera el pdf
						reporte_pdf.DataSource = documento_obj;
						HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
						x.GenerarPdf(reporte_pdf);

					}

					respuesta.UrlPdf = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);
					respuesta.IdEstado = CategoriaEstado.Recibido.GetHashCode();
					respuesta.DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.Recibido);

					//Actualiza el registro en la base de datos.
					documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
					documentoBd.IdCategoriaEstado = CategoriaEstado.Recibido.GetHashCode();
				}



			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento PDF. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
			}

			return respuesta;
		}

	}
}
