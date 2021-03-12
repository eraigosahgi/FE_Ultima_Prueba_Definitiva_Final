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
	public partial class ConfirmacionUsuarioPagos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{

				Guid IdSeguridad = new Guid();
				IdSeguridad = Guid.Parse(Request.QueryString["id_seguridad"]);

				Ctl_UsuarioPagos _cotrolador = new Ctl_UsuarioPagos();

				var datos = _cotrolador.ConfirmarUsuarioPagos(IdSeguridad);

				if (datos)//Exitoso
				{
					imgExitoso.Visible = true;
					lblResultado.InnerText = "El Usuario se ha activado con Exito y se ha enviado un correo al mismo para su ingreso";
				}
				else
				{
					imgFallido.Visible = true;
				}

			}
			catch (Exception excepcion)
			{
				imgFallido.Visible = true;
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
				lblResultado.InnerText = string.Format("Error : {0}", excepcion.Message);
			}

		}
	}
}