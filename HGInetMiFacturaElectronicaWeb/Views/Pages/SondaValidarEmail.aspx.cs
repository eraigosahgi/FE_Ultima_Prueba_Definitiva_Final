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
	public partial class SondaValidarEmail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			try
			{

				int Dias = 0;
				bool CorreoAudit = false;

				//Solo validar que documentos no tiene envio de correo y hacer este proceso
				string SoloCorreo = Request.QueryString["SoloCorreo"];
				
				//Hacer procesos solo ese dia cuando Dias es mayor a 0
				string Solodia = Request.QueryString["SoloDia"];

				//Se envia sobre cuantos dias segun fecha ingreso del doc se consulta en bd para hacer la validacion del correo
				if (Request.QueryString["Dias"] != null)
					Int32.TryParse(Request.QueryString["Dias"], out Dias);

				//Si llega true se hace por el proceso anterior que es consultando la auditoria
				if (Request.QueryString["CorreoAudit"] != null)
					Boolean.TryParse(Request.QueryString["CorreoAudit"], out CorreoAudit);

				Procesar(Dias, Convert.ToBoolean(Solodia), Convert.ToBoolean(SoloCorreo), CorreoAudit);
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);				
			}
		}

		public void Procesar(int dias, bool Solodia, bool SoloCorreo, bool CorreoAudit)
		{

			Ctl_Documento ctl_documento = new Ctl_Documento();

			if (SoloCorreo == true && CorreoAudit == false)
			{
				//Valida que correos no se han enviado
				var Tarea1 = ctl_documento.SondaDocumentosEnviarEmail(dias, Solodia);
			}
			else
			{
				//valida el estado de los correos segun plataforma de servicios
				var Tarea1 = ctl_documento.SondaDocumentosValidarEmail(dias, Solodia, SoloCorreo, CorreoAudit);
			}

			
			lblResultado.Text = string.Format("Termino");
		}
	}


}