using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class ConfirmacionEmail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				Guid IdSeguridad = new Guid();
				IdSeguridad = Guid.Parse(Request.QueryString["ID"]);
				string Mail = Request.QueryString["Mail"];

				if (IdSeguridad == null)
				{
					//Generar Error
				}
					
				if (string.IsNullOrEmpty(Mail))
				{
					//Generar Error
				}

				Ctl_Empresa Controlador = new Ctl_Empresa();

				lblResultado.InnerText = Controlador.ConfirmarMail(IdSeguridad, Mail);

			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(string.Format("Error : {0}", excepcion.Message), excepcion);
			}

		}
	}
}