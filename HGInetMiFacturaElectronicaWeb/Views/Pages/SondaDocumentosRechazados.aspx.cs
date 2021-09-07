using HGInetMiFacturaElectonicaController.Registros;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaDocumentosRechazados : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			try
			{

				bool ResumenMes = false;

				//Si llega true se hace por el proceso anterior que es consultando la auditoria
				if (Request.QueryString["ResumenMes"] != null)
					Boolean.TryParse(Request.QueryString["ResumenMes"], out ResumenMes);

				Procesar(ResumenMes);
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}
		}

		public void Procesar(bool ResumenMes)
		{

			Ctl_Documento ctl_documento = new Ctl_Documento();

			//Valida que correos no se han enviado
			var Tarea1 = ctl_documento.SondaValidarDocumentosRechazados(ResumenMes);


			lblResultado.Text = string.Format("Termino");
		}
	}
}