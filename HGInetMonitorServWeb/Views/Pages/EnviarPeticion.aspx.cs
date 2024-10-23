using HGInetMonitorServController.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMonitorServWeb.Views.Pages
{
	public partial class EnviarPeticion : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Ctl_Documento ctl_documento = new Ctl_Documento();

			var Tarea1 = ctl_documento.SondaEnviarPeticion();
			lblResultado.Text = string.Format("Termino");
		}
	}
}