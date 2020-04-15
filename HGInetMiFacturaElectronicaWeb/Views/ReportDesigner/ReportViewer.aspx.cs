using DevExpress.XtraPrinting.Drawing;
using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Peticiones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectronicaWeb.Views.ReportDesigner
{
	public partial class ReportViewer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{

				if (!IsPostBack)
				{

					string Titulo_Formato = "";
					////Valido si viene de las plataformas HGI o si viene de la plataforma de pago					
					//var ObjetoFactura = Request.Params["Documento"];
					////Valido si viene de las plataformas HGI o si viene de la plataforma de pago
					if (!string.IsNullOrWhiteSpace(Request.QueryString["Documento"]))
					{


						PeticionVistaPrevia vista_previa = new PeticionVistaPrevia();

						string StrVistaPrevia = Request.QueryString["Documento"];
						string ResultadoVistaPrevia = DesEncriptarObjeto(StrVistaPrevia);
						vista_previa = JsonConvert.DeserializeObject<PeticionVistaPrevia>(ResultadoVistaPrevia);


						dynamic Documento;
						switch (vista_previa.tipo_documento)
						{
							case 1:
								ClienteRest<Factura> clienteFac = new ClienteRest<Factura>(string.Format("{0}/api/DocumentosElectronicos/ObtenerDocumentoPI?id_documento={1}&id_transaccion={2}", vista_previa.ruta_servicio, vista_previa.documento, vista_previa.id_transaccion), TipoContenido.Applicationjson.GetHashCode(), vista_previa.token);
								Documento = clienteFac.GET();
								Titulo_Formato = Enumeracion.GetDescription(TipoDocumento.Factura).ToUpper();
								break;
							case 2:
								ClienteRest<NotaDebito> clienteND = new ClienteRest<NotaDebito>(string.Format("{0}/api/DocumentosElectronicos/ObtenerDocumentoPI?id_documento={1}&id_transaccion={2}", vista_previa.ruta_servicio, vista_previa.documento, vista_previa.id_transaccion), TipoContenido.Applicationjson.GetHashCode(), vista_previa.token);
								Documento = clienteND.GET();
								Titulo_Formato = Enumeracion.GetDescription(TipoDocumento.NotaDebito).ToUpper();
								break;
							case 3:
								ClienteRest<NotaCredito> clienteNC = new ClienteRest<NotaCredito>(string.Format("{0}/api/DocumentosElectronicos/ObtenerDocumentoPI?id_documento={1}&id_transaccion={2}", vista_previa.ruta_servicio, vista_previa.documento, vista_previa.id_transaccion), TipoContenido.Applicationjson.GetHashCode(), vista_previa.token);
								Documento = clienteNC.GET();
								Titulo_Formato = Enumeracion.GetDescription(TipoDocumento.NotaCredito).ToUpper();
								break;
							default:
								clienteFac = new ClienteRest<Factura>(string.Format("{0}/api/DocumentosElectronicos/ObtenerDocumentoPI?id_documento={1}&id_transaccion={2}", vista_previa.ruta_servicio, vista_previa.documento, vista_previa.id_transaccion), TipoContenido.Applicationjson.GetHashCode(), vista_previa.token);
								Documento = clienteFac.GET();
								Titulo_Formato = Enumeracion.GetDescription(TipoDocumento.Factura).ToUpper();
								break;
						}



						//OBTIENE FORMATO
						Ctl_Formatos clase_formatos = new Ctl_Formatos();

						TblFormatos datos_formato = null;

						try
						{
							datos_formato = clase_formatos.ObtenerFormato(Documento.DocumentoFormato.Codigo, Documento.DatosObligado.Identificacion, TipoFormato.FormatoPDF.GetHashCode());
						}
						catch (Exception)
						{
							//Si ocurre un error obteniendo el formato, entonces buscamos el formato 1 o 2 de HGI.
							int codigo_formato = 1;
							switch (vista_previa.tipo_documento)
							{
								case 1:
									codigo_formato = 1;
									break;
								case 2:
									codigo_formato = 2;
									break;
								case 3:
									codigo_formato = 2;
									break;
								default:
									codigo_formato = 1;
									break;
							}
							datos_formato = clase_formatos.ObtenerFormato(codigo_formato, "811021438", TipoFormato.FormatoPDF.GetHashCode());
							//lblresultado.Text = "Asegurese de tener formato asociado";
							//ASPxWebDocumentViewerWeb.Visible = false;
						}

						if (datos_formato == null)
						{
							//si no obtenemos el formato del cliente, entonces buscamos el formato 1 o 2 de HGI.
							int codigo_formato = 1;
							switch (vista_previa.tipo_documento)
							{
								case 1:
									codigo_formato = 1;
									break;
								case 2:
									codigo_formato = 2;
									break;
								case 3:
									codigo_formato = 2;
									break;
								default:
									codigo_formato = 1;
									break;
							}
							datos_formato = clase_formatos.ObtenerFormato(codigo_formato, "811021438", TipoFormato.FormatoPDF.GetHashCode());
							if (datos_formato == null)
							{
								lblresultado.Text = "No tiene Formato asociado y tampoco el formato base";
								ASPxWebDocumentViewerWeb.Visible = false;
							}
						}
						if (datos_formato != null)
						{


							try
							{
								XtraReportDesigner report = new XtraReportDesigner();

								byte[] formato = null;
								if (datos_formato.FormatoTmp != null)
									formato = datos_formato.FormatoTmp;
								else
									formato = datos_formato.Formato;

								MemoryStream datos = new MemoryStream(formato);


								if (Documento.DocumentoFormato != null)
								{
									//Valida que envien el titulo del documento y si es vacio lo llena
									if (string.IsNullOrEmpty(Documento.DocumentoFormato.Titulo) || Documento.DocumentoFormato.Titulo == null)
										Documento.DocumentoFormato.Titulo = Titulo_Formato;

								}

								//ASIGNA FORMATO A DISEÑADOR.
								report.LoadLayoutFromXml(datos);

								report.DataSource = Documento;

								if (vista_previa.marca_de_agua)
								{
									report.Watermark.CopyFrom(CreateTextWatermark((vista_previa.texto == "") ? "DOCUMENTO SIN VALIDAR POR LA DIAN" : vista_previa.texto, vista_previa.color));
								}

								ASPxWebDocumentViewerWeb.OpenReport(report);
							}
							catch (Exception ex)
							{
								throw new ApplicationException(string.Format("Se ha producido el siguiente error: {0}", ex));
							}
						}

					}
				}
			}
			catch (Exception ex)
			{
				//throw new ApplicationException(string.Format("Se ha producido el siguiente error: {0}", ex));
				lblresultado.Text = string.Format("Se ha producido el siguiente error: {0}", ex.Message);
				ASPxWebDocumentViewerWeb.Visible = false;
			}
		}

		private Watermark CreateTextWatermark(string text, Color color)
		{
			Watermark textWatermark = new Watermark();

			textWatermark.Text = text;
			textWatermark.TextDirection = DirectionMode.ForwardDiagonal;
			textWatermark.Font = new Font(textWatermark.Font.FontFamily, 40);
			textWatermark.ForeColor = color;
			textWatermark.TextTransparency = 150;
			textWatermark.ShowBehind = false;

			return textWatermark;
		}

		/// <summary>
		/// Desencripta las datos get.
		/// </summary>
		/// <param name="_cadenaAdesencriptar"></param>
		/// <returns></returns>
		private static string DesEncriptarObjeto(string _cadenaAdesencriptar)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(_cadenaAdesencriptar);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		private static string EncriptarObjeto(string _cadenaAencriptar)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_cadenaAencriptar);
			return System.Convert.ToBase64String(plainTextBytes);
		}



		public class PeticionVistaPrevia
		{
			public Guid documento { get; set; }
			public string id_transaccion { get; set; }
			public string token { get; set; }
			public string ruta_servicio { get; set; }
			public int tipo_documento { get; set; }
			public Color color { get; set; }
			public string texto { get; set; }
			public int codigo_formato { get; set; }
			public bool marca_de_agua { get; set; }

		}

	}
}


