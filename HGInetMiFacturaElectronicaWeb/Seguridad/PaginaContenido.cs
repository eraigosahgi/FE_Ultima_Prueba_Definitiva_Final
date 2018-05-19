using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Seguridad
{
    public class PaginaContenido : System.Web.UI.Page
    {
        protected ContentPlaceHolder contenido;
        private PaginaPrincipal custom_master;
        private string codigo_opcion = string.Empty;
        protected PaginaPermiso PermisoActual { get; set; }

        protected ContentPlaceHolder Contenido
        {
            get
            {
                if (contenido == null)
                {
                    if (Page.Master != null)
                        contenido = Page.Master.FindControl("ContenidoPagina") as ContentPlaceHolder;
                    else
                        contenido = new ContentPlaceHolder();
                }
                return contenido;
            }
        }

        protected PaginaPrincipal CustomMaster
        {
            get
            {
                if (custom_master == null)
                    custom_master = this.Master as PaginaPrincipal;


                return custom_master;
            }
            set { custom_master = value; }
        }

        protected string CodigoOpcion
        {
            get { return codigo_opcion; }
            set { codigo_opcion = value; }
        }

        /// <summary>
        /// Carga la opción de permiso
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private PaginaPermiso CargarOpcion(int codigo)
        {
            try
            {
                Ctl_Permisos permisos = new Ctl_Permisos();
                TblOpciones opcion = permisos.ObtenerOpcion(codigo);

                PaginaPermiso permiso = new PaginaPermiso();

                string ruta_grupo = string.Empty;

                if (opcion != null)
                {
                    // obtener las opciones de base de datos
                    List<TblOpciones> opciones = permisos.ObtenerAsociadas(codigo, false);

                    permiso.CodigosAsociados.AddRange(opciones.Select(_x => _x.IntId.ToString()).ToList());

                    ruta_grupo = permisos.ObtenerRutaGrupo(codigo, false, -1, opciones);

                    permiso.Codigo = codigo.ToString();

                    permiso.Titulo = opcion.StrDescripcion;

                    permiso.GrupoPagina = ruta_grupo;

                    permiso.RutaPaginaInicioUrl = "#";
                }

                this.PermisoActual = permiso;
                this.CustomMaster.PermisoActual = permiso;

                return permiso;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // obtiene la opción de permiso de acuerdo con la página actual
                this.CargarOpcion(Convert.ToInt32(CodigoOpcion));

                // valida las opciones que tienen permisos de usuario
                if (string.IsNullOrEmpty(OpcionesPermisos.FacturacionElectronicaPrincipal.Split(',').Where(_opcion => _opcion.Equals(CodigoOpcion)).FirstOrDefault()))
                {
                    // valida el permiso en la cookie
                    if (!Sesion.ValidarPermiso(CodigoOpcion))
                        Response.Redirect("~/Views/Login/Default.aspx");
                }
            }
            catch (Exception excepcion)
            {
                throw excepcion;
            }
        }

    }
}