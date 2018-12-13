using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class Sonda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string ListaEstados = Request.QueryString["Estados"];
                string Consultar = Request.QueryString["Consultar"];

                Procesar(ListaEstados,Convert.ToBoolean(Consultar));
            }
            catch (Exception ex)
            {
                lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
            }


        }


        public void Procesar(string ListaEstados,bool Consultar=true)
        {
            Ctl_Documento ctl_documento = new Ctl_Documento();

            var Tarea1 = ctl_documento.SondaProcesarDocumentos(ListaEstados, Consultar);
            lblResultado.Text = string.Format("Termino");
        }
    }
}