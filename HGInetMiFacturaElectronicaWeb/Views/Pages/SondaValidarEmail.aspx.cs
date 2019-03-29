using HGInetMiFacturaElectonicaController.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaValidarEmail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			try
			{
				int Tiempo = 0;
				int Dias = 0;

				//Se envia sobre cuantos dias segun fecha ingreso del doc se consulta en bd para hacer la validacion del correo
				if (Request.QueryString["Dias"] != null)
					Int32.TryParse(Request.QueryString["Dias"], out Dias);

				//Se envia el tiempo de notificacion en Minutos
				if (Request.QueryString["Tiempo"] != null)
					Int32.TryParse(Request.QueryString["Tiempo"], out Tiempo);

				Procesar(Tiempo,Dias);
			}
			catch (Exception ex)
			{
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}
		}

		public void Procesar(int Tiempo, int dias)
		{

			Ctl_Documento ctl_documento = new Ctl_Documento();

			var Tarea1 = ctl_documento.SondaDocumentosValidarEmail(Tiempo,dias);
			lblResultado.Text = string.Format("Termino");
		}
	}


}