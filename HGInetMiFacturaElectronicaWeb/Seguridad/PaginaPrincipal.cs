using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
namespace HGInetMiFacturaElectronicaWeb.Seguridad
{
    public class PaginaPrincipal : System.Web.UI.MasterPage
    {

        public PaginaPermiso PermisoActual { get; set; }
        public SweetAlert Modal { get; set; }

        public void MostrarModal()
        {
            try
            {
                if (this.Modal != null)
                {
                    string scriptKey = "NotifyMessageScript";
                    StringBuilder script = this.Modal.ObtenerScript();

                    RegistrarJavaScript(scriptKey, script, true);
                }
            }
            catch (Exception excepcion)
            {
            }
        }
        protected void RegistrarJavaScript(string scriptKey, StringBuilder script, bool addTagJS = true)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm != null && !sm.IsInAsyncPostBack && !Page.ClientScript.IsStartupScriptRegistered(this.Page.GetType(), scriptKey))
            {
                Page.ClientScript.RegisterStartupScript(this.Page.GetType(), scriptKey, script.ToString(), addTagJS);
            }

        }
    }
}