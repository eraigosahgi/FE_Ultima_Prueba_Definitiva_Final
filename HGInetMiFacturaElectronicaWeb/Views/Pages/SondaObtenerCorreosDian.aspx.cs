using HGInetMiFacturaElectonicaController.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaObtenerCorreosDian : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Ctl_ObtenerCorreos ctrl_correosdian = new Ctl_ObtenerCorreos();

			var Tarea1 = ctrl_correosdian.SondaObtenerCorreosDian();
			lblResultado.Text = string.Format("Termino");
		}
	}
}