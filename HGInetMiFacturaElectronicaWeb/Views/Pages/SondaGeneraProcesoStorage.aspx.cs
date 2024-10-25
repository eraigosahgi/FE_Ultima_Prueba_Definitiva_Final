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
				anyo = 2018;

			bool buscar_faltantes = false;

			//Se envia año para que tome los documetos que corresponde
			if (Request.QueryString["Faltante"] != null)
				bool.TryParse(Request.QueryString["Faltante"], out buscar_faltantes);

			int mes = 1;

			//Se envia Mes para que tome los documetos que corresponde
			if (Request.QueryString["Mes"] != null)
				Int32.TryParse(Request.QueryString["Mes"], out mes);


			Ctl_Documento ctl_documento = new Ctl_Documento();

			var Tarea1 = ctl_documento.SondaProcesoStorage(anyo, buscar_faltantes, mes);
			lblResultado.Text = string.Format("Termino");

		}
	}
}