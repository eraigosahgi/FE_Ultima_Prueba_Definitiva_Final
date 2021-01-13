using HGInetInteroperabilidad.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaProcesarCorreo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				Ctl_Descomprimir correos = new Ctl_Descomprimir();
				var Tarea1 = correos.SondaProcesarCorreos();
				lblResultado.Text = string.Format("Termino");
			}
			catch (Exception excepcion)
			{

			}
		}
	}
}