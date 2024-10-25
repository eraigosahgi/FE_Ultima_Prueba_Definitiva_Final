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
	public partial class SondaCrearPlanes : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
								
				string Facturador = Request.QueryString["Facturador"];

				string Usuario = Request.QueryString["Usuario"];

				bool Notifica = Convert.ToBoolean(Request.QueryString["Notifica"]);

				if (string.IsNullOrEmpty(Facturador))
					throw new ApplicationException("Debe inidicar el parametro Facturador");

				if (string.IsNullOrEmpty(Usuario))
					throw new ApplicationException("Debe inidicar el parametro Usuario");				

				Procesar(Facturador, Usuario, Notifica);
			}
			catch (Exception ex)
			{
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
			}
		}

		public void Procesar(string EmpresaCrea, string UsuarioCrea, bool Notifica)
		{
			Ctl_PlanesTransacciones controlador = new Ctl_PlanesTransacciones();

			var Tarea1 = controlador.SondaCrearPlanesPostpago( EmpresaCrea,  UsuarioCrea,Notifica);
			lblResultado.Text = string.Format("Termino");
		}
	}
}