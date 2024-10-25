using HGInetMiFacturaElectonicaController.Registros;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaGenerarPDF : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			try
			{

				bool ValidacionDian = false;

				//Si llega true solo se agrega al pdf existente la fecha de validacion que tiene el documento en la DIAN
				if (Request.QueryString["ValidacionDian"] != null)
					Boolean.TryParse(Request.QueryString["ValidacionDian"], out ValidacionDian);

				string list_Idseguridad = Request.QueryString["Documentos"];

				Procesar(list_Idseguridad, ValidacionDian);

			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}

		}

		public void Procesar(string list_Idseguridad, bool validacion_dian)
		{
			Ctl_Documento ctl_documento = new Ctl_Documento();

			var Tarea1 = ctl_documento.SondaGenerarPDF(list_Idseguridad, validacion_dian);
			lblResultado.Text = string.Format("Termino");
		}
	}
}