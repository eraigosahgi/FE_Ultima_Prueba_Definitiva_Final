using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class ConfigurarPlanes : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				string Facturadores = Request.QueryString["Facturadores"];

				if (string.IsNullOrEmpty(Facturadores))
					throw new ApplicationException("Debe inidicar el parametro Facturadores=(Facturador1,Facturador2) o sustituir la lista por un Facturadores=* ");

				Ctl_Documento controlador = new Ctl_Documento();

				var Tarea1 = controlador.ConfigurarPlanesDocumentos(Facturadores);
				lblResultado.Text="Documentos en proceso....";
			}
			catch (Exception ex)
			{
				lblResultado.Text = ex.Message;
			}

		}
	}
}