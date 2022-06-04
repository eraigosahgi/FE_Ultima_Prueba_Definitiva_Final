using HGInetMiFacturaElectonicaController.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaGeneraProcesoStorage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			int anyo = 0;

			//Se envia año para que tome los documetos que corresponde
			if (Request.QueryString["Anyo"] != null)
				Int32.TryParse(Request.QueryString["Anyo"], out anyo);
			else
				anyo = 2021;

			int mes = 0;

			//Se envia año para que tome los documetos que corresponde
			if (Request.QueryString["Mes"] != null)
				Int32.TryParse(Request.QueryString["Mes"], out mes);
			else
				mes = 1;


			Ctl_Documento ctl_documento = new Ctl_Documento();

			var Tarea1 = ctl_documento.SondaProcesoStorage(anyo, mes);
			lblResultado.Text = string.Format("Termino");

		}
	}
}