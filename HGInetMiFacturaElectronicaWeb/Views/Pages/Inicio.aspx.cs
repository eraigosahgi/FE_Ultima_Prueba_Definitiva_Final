using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // obtiene de la ruta GET el id de seguridad del usuario autenticado para crear la sesión
            if (Request.QueryString != null)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["ID"]))
                {
                    Sesion sesion = new Sesion();

                    System.Guid id_seguridad = new System.Guid(Request.QueryString["ID"]);

                    sesion.GuardarSesionWeb(id_seguridad);
                }
            }

        }

    }
}