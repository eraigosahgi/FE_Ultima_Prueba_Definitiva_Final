using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Formato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Masters
{
    public partial class MasterPrincipal : PaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // obtiene los datos de la sesión para mostrarlos
                if (Sesion.DatosEmpresa != null && Sesion.DatosUsuario != null)
                {
                    // carga los datos de la empresa en la página master
                    TblEmpresas datos_empresa = Sesion.DatosEmpresa;
                    LblNombreEmpresa.InnerText = datos_empresa.StrRazonSocial;

                    // carga los datos del usuario en la página master
                    TblUsuarios datos_usuario = Sesion.DatosUsuario;
                    LblCodigoUsuario.InnerText = datos_usuario.StrUsuario;
                    LblNombreUsuarioDet.InnerText = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);
                    lblEmailUsuario.InnerText = datos_usuario.StrMail;
                    LblNombreUsuario.InnerText = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);

                    if (PermisoActual != null)
                    {
                        lb_TituloPagina.Text = this.PermisoActual.Titulo;
                        lb_GrupoPagina.Text = this.PermisoActual.GrupoPagina;

                        ActivarMenu(this.PermisoActual);
                        OpcionesMenu();
                    }


                }
            }
            catch (Exception excepcion)
            {
                //Controla la session enviando la url a la pagina inicial para iniciar nuevamente la session
                //PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                //string script = @"<script type='text/javascript'>control_session('" + plataforma.RutaPublica + "');</script>";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "control_session", script, false);
            }
        }

        private void ActivarMenu(PaginaPermiso permiso)
        {
            string id_control = permiso.Codigo;

            id_control = id_control.Replace("-", "_");

            HtmlGenericControl opcion = this.FindControl(string.Format("menu_{0}", id_control)) as HtmlGenericControl;

            if (opcion != null)
            {
                opcion.Attributes["class"] = "active";
            }

            foreach (string item in permiso.CodigosAsociados)
            {
                HtmlGenericControl li_menu = this.FindControl(string.Format("menu_{0}", item.Replace("-", "_"))) as HtmlGenericControl;

                if (li_menu != null)
                {
                    li_menu.Attributes["class"] = "active";
                }
            }
        }

        private void OpcionesMenu()
        {
            try
            {
                List<string> permisos = Coleccion.ConvertirLista(Sesion.PermisosUsuario);
                int i = 0;
                int submenu = 0;
                int itemSubmenu = 0;

                //recorre la lista del menú principal
                foreach (Control liMenu in MenuPrincipal.Controls)
                {
                    if (liMenu is HtmlGenericControl)
                    {
                        string id_liMenu = liMenu.ID;

                        HtmlGenericControl li_menu = this.FindControl(id_liMenu) as HtmlGenericControl;
                        id_liMenu = id_liMenu.Replace("menu_", "");

                        if (li_menu != null)
                        {
                            if (permisos.Contains(id_liMenu))
                            {
                                submenu++;
                                HtmlGenericControl ul_menu = this.FindControl(liMenu.ID.Replace("menu", "ul")) as HtmlGenericControl;
                                if (ul_menu != null)
                                {
                                    submenu = 0;
                                    //recorre la lista del submenú
                                    foreach (Control liSubmenu in ul_menu.Controls)
                                    {
                                        if (liSubmenu is HtmlGenericControl)
                                        {
                                            string id_liSubmenu = liSubmenu.ID;

                                            HtmlGenericControl li_submenu = this.FindControl(id_liSubmenu) as HtmlGenericControl;
                                            id_liSubmenu = id_liSubmenu.Replace("menu_", "");

                                            if (li_submenu != null)
                                            {
                                                if (permisos.Contains(id_liSubmenu))
                                                {
                                                    itemSubmenu++;
                                                    HtmlGenericControl ul_submenu = this.FindControl(li_submenu.ID.Replace("menu", "ul")) as HtmlGenericControl;
                                                    if (ul_submenu != null)
                                                    {
                                                        itemSubmenu = 0;
                                                        //recorre los item del submenú
                                                        foreach (Control liMenuItem in ul_submenu.Controls)
                                                        {
                                                            if (liMenuItem is HtmlGenericControl)
                                                            {
                                                                string id_menuItem = liMenuItem.ID;

                                                                HtmlGenericControl li_menuItem = this.FindControl(id_menuItem) as HtmlGenericControl;
                                                                id_menuItem = id_menuItem.Replace("menu_", "");

                                                                if (li_menuItem != null)
                                                                {
                                                                    if (permisos.Contains(id_menuItem))
                                                                    {
                                                                        itemSubmenu++;
                                                                        HtmlGenericControl link_submenu = this.FindControl(li_menuItem.ID.Replace("menu", "ul")) as HtmlGenericControl;

                                                                        if (link_submenu != null)
                                                                        {
                                                                            itemSubmenu = 0;
                                                                            //recorre los item del submenú
                                                                            foreach (Control li_menusubitem in link_submenu.Controls)
                                                                            {
                                                                                if (li_menusubitem is HtmlGenericControl)
                                                                                {
                                                                                    string id_submenuItem = li_menusubitem.ID;

                                                                                    HtmlGenericControl li_submenuItem = this.FindControl(id_submenuItem) as HtmlGenericControl;
                                                                                    id_submenuItem = id_submenuItem.Replace("menu_", "");

                                                                                    if (li_submenuItem != null)
                                                                                    {
                                                                                        if (permisos.Contains(id_submenuItem))
                                                                                        {
                                                                                            li_submenuItem.Visible = true;
                                                                                            itemSubmenu++;
                                                                                        }
                                                                                        else li_submenuItem.Visible = false;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        li_menuItem.Visible = true;
                                                                        itemSubmenu++;
                                                                    }
                                                                    else li_menuItem.Visible = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (itemSubmenu > 0)
                                                    {
                                                        li_submenu.Visible = true;
                                                        submenu++;
                                                    }
                                                    else li_submenu.Visible = false;
                                                }
                                                else li_submenu.Visible = false;
                                            }
                                        }
                                    }
                                }
                                if (submenu > 0)
                                {
                                    li_menu.Visible = true;
                                    i++;
                                }
                                else li_menu.Visible = false;
                            }
                            else li_menu.Visible = false;
                        }
                    }
                }
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Evento al presionar el link para cerrar la sesión
        /// </summary>
        protected void LinkCerrarSesion_Click(object sender, EventArgs e)
        {
            if (Session != null)
            {
                Session.Clear();
                Session.Abandon();
            }

            Response.Redirect("~/Views/Login/Default.aspx");
        }
    }
}