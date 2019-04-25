using HGInetMiFacturaElectonicaController.Registros;
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

			string list_Idseguridad = Request.QueryString["Documentos"];

			Procesar(list_Idseguridad);

			}
			catch (Exception ex)
			{
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}

		}

		public void Procesar(string list_Idseguridad)
		{
			Ctl_Documento ctl_documento = new Ctl_Documento();

			var Tarea1 = ctl_documento.SondaGenerarPDF(list_Idseguridad);
			lblResultado.Text = string.Format("Termino");
		}
	}
}