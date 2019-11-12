using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaConciliaPlanes : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
							
				Procesar();
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}
		}

		public void Procesar()
		{
			Ctl_PlanesTransacciones controlador = new Ctl_PlanesTransacciones();

			var Tarea1 = controlador.TareaSondaConciliarPlanes();
			lblResultado.Text = string.Format("Termino");
		}
	}
}