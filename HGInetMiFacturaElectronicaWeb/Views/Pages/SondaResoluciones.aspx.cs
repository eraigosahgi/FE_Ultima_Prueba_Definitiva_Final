using HGInetMiFacturaElectonicaController.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaResoluciones : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				Procesar();
			}
			catch (Exception ex)
			{
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}
		}


		public void Procesar()
		{
			Ctl_Resoluciones ctl_resoluciones = new Ctl_Resoluciones();

			var Tarea1 = ctl_resoluciones.SondaObtenerResoluciones();
			lblResultado.Text = string.Format("Termino");
		}
	}
}