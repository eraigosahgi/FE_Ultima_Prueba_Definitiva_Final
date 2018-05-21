using HGInetMiFacturaElectronicaWeb.Properties;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class GestionUsuarios : PaginaContenido
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.CodigoOpcion = OpcionesPermisos.GestionUsuariosSistema;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}