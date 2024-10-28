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
				bool emision = false;

				//Se envia para que descargue del buzon de emision
				if (Request.QueryString["Emision"] != null)
					bool.TryParse(Request.QueryString["Emision"], out emision);

				Ctl_Descomprimir correos = new Ctl_Descomprimir();
				var Tarea1 = correos.SondaProcesarCorreos(emision);
				lblResultado.Text = string.Format("Termino");
			}
			catch (Exception excepcion)
			{

			}
		}
	}
}