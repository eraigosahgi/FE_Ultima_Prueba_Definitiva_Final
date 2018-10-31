using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Ctl_Documento ctl_documento = new Ctl_Documento();
                List<TblDocumentos> datos = ctl_documento.ObtenerDocumentosaProcesar();

                lblResultado.Text = string.Format("Numero de Documentos a procesar: {0} ", datos.Count().ToString());

                List<DocumentoRespuesta> datosProcesar = HGInetMiFacturaElectonicaController.Procesos.Ctl_Documentos.Procesar(datos);

                lblResultado.Text =  string.Format("Proceso Finalizado, Total documentos {0}",datos.Count().ToString());

            }
            catch (Exception ex)
            {
                lblResultado.Text = string.Format("El proceso genero un error {0}", ex.Message);
            }


        }
    }
}