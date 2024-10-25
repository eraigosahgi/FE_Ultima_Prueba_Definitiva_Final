using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.RegistroLog;
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
				int dias = 0;

				if (Request.QueryString["Dias"] != null)
					Int32.TryParse(Request.QueryString["Dias"],out dias);


				Procesar(ListaEstados,dias,Convert.ToBoolean(Consultar));
            }
            catch (Exception ex)
            {
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
				lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
            }


        }


        public void Procesar(string ListaEstados, int dias, bool Consultar=true)
        {
            Ctl_Documento ctl_documento = new Ctl_Documento();

            var Tarea1 = ctl_documento.SondaProcesarDocumentos(ListaEstados, dias, Consultar);
            lblResultado.Text = string.Format("Termino");
        }
    }
}