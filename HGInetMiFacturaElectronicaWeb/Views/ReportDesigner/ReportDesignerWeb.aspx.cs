using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Properties;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
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
	public partial class ReportDesignerWeb : PaginaContenido
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			this.RutaRedireccionAlerta = "../Pages/Inicio.aspx";
			this.CodigoOpcion = OpcionesPermisos.GestionReportes;
			this.ProcesoPagina = OperacionesBD.IntEditar;
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				SerializeReport clase_serialize = new SerializeReport();

				base.Page_Load(sender, e);
				Ctl_Formatos clase_formatos = new Ctl_Formatos();

				XtraReportDesigner report = new XtraReportDesigner();
				report.CreateDocument(true);

				if (!string.IsNullOrWhiteSpace(Request.QueryString["ID"]) && !string.IsNullOrWhiteSpace(Request.QueryString["Nit"]))
				{
					int codigo_formato = Convert.ToInt32(Request.QueryString["ID"]);
					string cod_empresa = Request.QueryString["Nit"].ToString();

					TipoDocumento tipo_doc = Enumeracion.GetEnumObjectByValue<TipoDocumento>(Convert.ToInt32(Request.QueryString["TipoDoc"]));

					clase_serialize = new SerializeReport(tipo_doc);

					SerializationService.RegisterSerializer(SerializeReport.Name, clase_serialize);

					TblFormatos datos_formato = clase_formatos.Obtener(codigo_formato, cod_empresa, TipoFormato.FormatoPDF.GetHashCode());

					if (datos_formato != null)
					{
						byte[] datos_formato_tmp = null;

						if (datos_formato.FormatoTmp == null)
							datos_formato_tmp = datos_formato.Formato;
						else
							datos_formato_tmp = datos_formato.FormatoTmp;

						MemoryStream datos = new MemoryStream(datos_formato_tmp);
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
				report.DataSource = clase_serialize.GenerarColumnas();
				report.DataMember = clase_serialize.DataMember;
				ASPxReportDesignerWeb.OpenReport(report);

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

				if (!string.IsNullOrWhiteSpace(Request.QueryString["ID"]) && !string.IsNullOrWhiteSpace(Request.QueryString["Nit"]))
				{
					int codigo_formato = Convert.ToInt32(Request.QueryString["ID"]);
					string cod_empresa = Request.QueryString["Nit"].ToString();

					TblFormatos respuesta = clase_formatos.ActualizarFormato(codigo_formato, cod_empresa, byte_formato, TipoFormato.FormatoPDF.GetHashCode(), Sesion.DatosUsuario.StrIdSeguridad);

					if (respuesta != null)
					{
						string mensaje = string.Format("El formato Número {0} ha sido actualizado correctamente.", respuesta.IntCodigoFormato);

						ASPxReportDesignerWeb.JSProperties["cpTextoBtnNotificacion"] = "Salir";
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

	}
}