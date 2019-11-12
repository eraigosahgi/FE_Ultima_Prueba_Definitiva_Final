using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_Empresa;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class ConfirmacionEmail : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{

				ObjResultado Result = new ObjResultado(); 
				imgExitoso.Visible = false;
				imgPrecaucion.Visible = false;
				imgFallido.Visible = false;
				

				Guid IdSeguridad = new Guid();
				IdSeguridad = Guid.Parse(Request.QueryString["ID"]);
				string Mail = Request.QueryString["Mail"];

				if (IdSeguridad == null)
				{
					imgFallido.Visible =true;
					throw  new Exception("No se encontro el id de seguridad");
				}
					
				if (string.IsNullOrEmpty(Mail))
				{
					imgFallido.Visible = true;
					throw new Exception("No se encontro el Email");
				}
				
				Ctl_Empresa Controlador = new Ctl_Empresa();

				Result = Controlador.ConfirmarMail(IdSeguridad, Mail);
			
				lblResultado.InnerText = Result.Descripcion;

				if (Result.Codigo == 1)//Exitoso
				{
					imgExitoso.Visible = true;
				}

				if (Result.Codigo == 2)//precaución
				{
					imgPrecaucion.Visible = true;
				}

				if (Result.Codigo == 3)//Fallido
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