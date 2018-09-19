using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.Web;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace HGInetMiFacturaElectronicaWeb.Views.ReportDesigner
{
	public partial class ReportDesignerWeb : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				//Asigna los parametros al diseñador de reportes.
				XtraReportDesigner report = new XtraReportDesigner();
				report.CreateDocument(true);
				report.LoadLayoutFromXml(@"E:\Desarrollo\atamayo\ArchivoXML.xml");
				report.Extensions[SerializationService.Guid] = SerializeReport.Name;
				report.DataSource = SerializeReport.GenerarColumnas();
				ASPxReportDesignerWeb.OpenReport(report);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		protected void ASPxReportDesignerWeb_SaveReportLayout(object sender, SaveReportLayoutEventArgs e)
		{
			try
			{
				Sesion.ValidarSesion();

				string reportName = e.Parameters;
				byte[] byte_formato = e.ReportLayout;

				Ctl_Formatos clase_formatos = new Ctl_Formatos();
				TblFormatos respuesta = clase_formatos.AlmacenarFormato("811021438", byte_formato, TipoFormato.FormatoPDF.GetHashCode());

				if (respuesta != null)
				{
					//mensaje de registro exitoso.
				}
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		
	}
}