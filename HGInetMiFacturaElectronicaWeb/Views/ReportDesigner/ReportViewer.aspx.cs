using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
					//Valido si viene de las plataformas HGI o si viene de la plataforma de pago
					if (!string.IsNullOrWhiteSpace(Request.QueryString["IdSeguridad"]))
					{
						var ObjetoPago = DesEncriptarObjeto(Request.QueryString["IdSeguridad"]);

						//Deserializo el objeto 
						//TblPasarelaPagos ConfigPago = new JavaScriptSerializer().Deserialize<TblPasarelaPagos>(ObjetoPago);

						string CifradoSecundario;
						//Si tenemos el StrIdSeguridadComercio es porque estamos con la versión de configuracion en plataforma
						//Ciframos entonces con este nuevo dato
						//CifradoSecundario = Encriptar.Encriptar_SHA256(ConfigPago.StrIdSeguridadRegistro.ToString() + "-" + ConfigPago.StrClienteIdentificacion + "-" + ConfigPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ConfigPago.StrIdSeguridadComercio.ToString() + "-" + ConfigPago.IntValor.ToString("0.##"));


						//OBTIENE FORMATO
						Ctl_Formatos clase_formatos = new Ctl_Formatos();
						TblFormatos datos_formato = clase_formatos.ObtenerFormato(1, "811021438", TipoFormato.FormatoPDF.GetHashCode());
						XtraReportDesigner report = new XtraReportDesigner();

						byte[] formato = null;
						if (datos_formato.FormatoTmp != null)
							formato = datos_formato.FormatoTmp;
						else
							formato = datos_formato.Formato;

						MemoryStream datos = new MemoryStream(formato);

						//ASIGNA FORMATO A DISEÑADOR.
						report.LoadLayoutFromXml(datos);
						Factura documento_obj = new Factura();

						/*
						//Obtiene los datos del último documento generado
						Ctl_Documento clase_documento = new Ctl_Documento();
						TblDocumentos datos_doc_bd = clase_documento.Obtener(datos_formato.StrEmpresa).OrderByDescending(f => f.DatFechaIngreso).FirstOrDefault();
						string contenido_xml = Archivo.ObtenerContenido(datos_doc_bd.StrUrlArchivoUbl);

						// valida el contenido del archivo
						if (string.IsNullOrWhiteSpace(contenido_xml))
							throw new ArgumentException("El archivo XlML UBL se encuentra vacío.");

						// convierte el contenido de texto a xml
						XmlReader xml_reader = XmlReader.Create(new StringReader(contenido_xml));
						Factura documento_obj = new Factura();
						// convierte el objeto de acuerdo con el tipo de documento
						XmlSerializer serializacion = null;

						if (datos_doc_bd.IntVersionDian == 1)
						{
							serializacion = new XmlSerializer(typeof(HGInetUBL.InvoiceType));
							HGInetUBL.InvoiceType conversion = (HGInetUBL.InvoiceType)serializacion.Deserialize(xml_reader);
							documento_obj = FacturaXML.Convertir(conversion, datos_doc_bd);
						}
						else
						{
							serializacion = new XmlSerializer(typeof(InvoiceType));
							InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);
							documento_obj = HGInetUBLv2_1.FacturaXMLv2_1.Convertir(conversion, datos_doc_bd);
							if (documento_obj.DocumentoFormato == null && !string.IsNullOrEmpty(datos_doc_bd.StrFormato))
								documento_obj.DocumentoFormato = JsonConvert.DeserializeObject<Formato>(datos_doc_bd.StrFormato);
						}
						*/
						report.DataSource = documento_obj;
						ASPxWebDocumentViewerWeb.OpenReport(report);
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
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


	}
}