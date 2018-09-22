using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Properties;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using HGInetUBL;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectronicaWeb.Views.ReportDesigner
{
	public partial class ReportDesignerWeb : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				Sesion.ValidarSesion();

				if (!IsPostBack)
				{
					Ctl_Formatos clase_formatos = new Ctl_Formatos();

					XtraReportDesigner report = new XtraReportDesigner();
					report.CreateDocument(true);

					if (!string.IsNullOrWhiteSpace(Request.QueryString["ID"]))
					{
						int codigo_formato = Convert.ToInt32(Request.QueryString["ID"]);

						TblFormatos datos_formato = clase_formatos.Obtener(codigo_formato, Sesion.DatosEmpresa.StrIdentificacion, TipoFormato.FormatoPDF.GetHashCode());

						if (datos_formato != null)
						{
							MemoryStream datos = new MemoryStream(datos_formato.Formato);
							report.LoadLayoutFromXml(datos);
						}
					}
					else
					{
						//Carga un archivo básico, con la asignación de datos y el código QR
						string ruta = Directorio.ObtenerDirectorioRaiz() + @"Views\ReportDesigner\XmlReporteBase.xml";
						report.LoadLayoutFromXml(ruta);
					}

					report.Extensions[SerializationService.Guid] = SerializeReport.Name;
					report.DataSource = SerializeReport.GenerarColumnas();
					ASPxReportDesignerWeb.OpenReport(report);
				}


			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// evento del control para el almacenamiento de la información
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ASPxReportDesignerWeb_SaveReportLayout(object sender, SaveReportLayoutEventArgs e)
		{
			try
			{
				byte[] byte_formato = e.ReportLayout;
				Ctl_Formatos clase_formatos = new Ctl_Formatos();

				if (Request.QueryString["ID"] != null)
				{
					int codigo_formato = Convert.ToInt32(Request.QueryString["ID"]);
					TblFormatos respuesta = clase_formatos.ActualizarFormato(codigo_formato, Sesion.DatosEmpresa.StrIdentificacion, byte_formato, TipoFormato.FormatoPDF.GetHashCode());

					if (respuesta != null)
					{
						string mensaje = string.Format("El formato Número {0} ha sido actualizado correctamente.", respuesta.IntCodigoFormato);

						ASPxReportDesignerWeb.JSProperties["cpTextoBtnNotificacion"] = "Aceptar";
						ASPxReportDesignerWeb.JSProperties["cpMensajeNotificacion"] = mensaje;
						ASPxReportDesignerWeb.JSProperties["cpTituloNotificacion"] = "¡ Proceso éxitoso !";
					}
				}
				else
				{
					TblFormatos respuesta = clase_formatos.AlmacenarFormato(Sesion.DatosEmpresa.StrIdentificacion, byte_formato, TipoFormato.FormatoPDF.GetHashCode());

					if (respuesta != null)
					{
						string mensaje = string.Format("El formato ha sido almacenado correctamente. Código Identificador: {0}", respuesta.IntCodigoFormato);

						ASPxReportDesignerWeb.JSProperties["cpTextoBtnNotificacion"] = "Aceptar";
						ASPxReportDesignerWeb.JSProperties["cpMensajeNotificacion"] = mensaje;
						ASPxReportDesignerWeb.JSProperties["cpTituloNotificacion"] = "¡ Proceso éxitoso !";
					}
				}
			}
			catch (Exception excepcion)
			{
				ASPxReportDesignerWeb.JSProperties["cpTextoBtnNotificacion"] = "Aceptar";
				ASPxReportDesignerWeb.JSProperties["cpMensajeNotificacion"] = excepcion.Message;
				ASPxReportDesignerWeb.JSProperties["cpTituloNotificacion"] = "¡ Error !";
			}
		}

		protected void BtnGenerar_Click(object sender, EventArgs e)
		{
			try
			{


				XtraReportDesigner rep = new XtraReportDesigner();
				rep.CreateDocument(true);

				string ruta = Directorio.ObtenerDirectorioRaiz() + @"Views\ReportDesigner\XmlReporteBase.xml";

				Ctl_Formatos clase_formatos = new Ctl_Formatos();
				TblFormatos datos_formato = clase_formatos.Obtener(1, Sesion.DatosEmpresa.StrIdentificacion, TipoFormato.FormatoPDF.GetHashCode());

				if (datos_formato != null)
				{
					MemoryStream datos = new MemoryStream(datos_formato.Formato);
					rep.LoadLayoutFromXml(datos);
				}


				//FileStream xml = new FileStream(@"", FileMode.Open);
				XmlTextReader xml = new XmlTextReader("http://localhost:61421/dms/eb821fbe-02ba-4cfc-a7a9-248711513591/FacturaEDian/face_f0811021438003B023972.xml");
				XmlSerializer serializacion = new XmlSerializer(typeof(InvoiceType));

				InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml);

				Factura datos_documento = FacturaXML.Convertir(conversion);


				//rep.LoadLayoutFromXml(@"E:\Desarrollo\atamayo\ArchivoXML.xml");
				//rep.Extensions[SerializationService.Guid] = SerializacionReporte.Name;
				//rep.DataMember = "Reporte Documento";
				rep.DataSource = datos_documento;

				//string propertyName, Type propertyType, string expressionPrefix, string expression
				//ExpressionBinding expressionBinding = new ExpressionBinding("BeforePrint", null, "Text", "[UnitPrice] * [UnitsInStock]");
				//Control myControl1 = rep.Container.FindControl("TextBox2"); //.ExpressionBindings.Add(expressionBinding);

				/*foreach (Band reportBand in rep.Bands)
				{
					XRControl control = FindBandControl(reportBand, "campo1");

					if (control != null)
					{
						break;
					}
				}*/



			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/*
		private XRControl FindBandControl(Band reportBand, string controlName)
		{
			XRControl foundControl = reportBand.FindControl(controlName, false);

			if (foundControl != null)
			{
				return foundControl;
			}

			foreach (Band subBand in reportBand.SubBands)
			{
				FindBandControl(subBand, controlName);
			}

			return null;
		}*/

	}
}