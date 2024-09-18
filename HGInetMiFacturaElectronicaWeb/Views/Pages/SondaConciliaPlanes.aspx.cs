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
				string _skip = Request.QueryString["skip"];
				string _pageSize = Request.QueryString["pageSize"];

				int skip = 1;
				int pageSize = 10000;

				if (Request.QueryString["skip"] != null)
					Int32.TryParse(Request.QueryString["skip"], out skip);

				if (Request.QueryString["pageSize"] != null)
					Int32.TryParse(Request.QueryString["pageSize"], out pageSize);

				Procesar(skip, pageSize);
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}
		}

		public void Procesar(int skip, int pageSize)
		{
			Ctl_PlanesTransacciones controlador = new Ctl_PlanesTransacciones();

			var Tarea1 = controlador.TareaSondaConciliarPlanes(skip, pageSize);
			lblResultado.Text = string.Format("Termino");
		}
	}
}