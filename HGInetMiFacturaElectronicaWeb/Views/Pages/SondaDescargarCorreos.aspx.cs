using HGInetInteroperabilidad.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaDescargarCorreos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				Ctl_MailRecepcion mail = new Ctl_MailRecepcion();
				var Tarea1 = mail.SondaDescargarCorreos();
				lblResultado.Text = string.Format("Termino");
			}
			catch (Exception excepcion)
			{

			}
		}
	}
}