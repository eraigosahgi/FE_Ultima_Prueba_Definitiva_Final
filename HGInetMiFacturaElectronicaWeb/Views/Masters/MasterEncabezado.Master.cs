using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Masters
{
	public partial class MasterEncabezado : PaginaPrincipal
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (!Page.IsPostBack)
				{
					// obtiene los datos de la sesión para mostrarlos
					if (Sesion.DatosEmpresa != null && Sesion.DatosUsuario != null)
					{
						// carga los datos de la empresa en la página master
						TblEmpresas datos_empresa = Sesion.DatosEmpresa;

						// carga los datos del usuario en la página master
						TblUsuarios datos_usuario = Sesion.DatosUsuario;
						LblCodigoUsuario.InnerText = datos_usuario.StrUsuario;
						LblNombreUsuarioDet.InnerText = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);
						lblEmailUsuario.InnerText = datos_usuario.StrMail;
						LblNombreUsuario.InnerText = string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);

						lb_TituloPagina.Text = this.PermisoActual.Titulo;
						lb_GrupoPagina.Text = this.PermisoActual.GrupoPagina;

					}
				}
			}
			catch (Exception excepcion)
			{
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

			Response.Cookies["UserSettings"]["HtmlMenu"] = "";
			Response.Cookies["UserSettings"].Expires = DateTime.Now.AddMinutes(1);

			Response.Redirect("~/Views/Login/Default.aspx");
		}
	}
}