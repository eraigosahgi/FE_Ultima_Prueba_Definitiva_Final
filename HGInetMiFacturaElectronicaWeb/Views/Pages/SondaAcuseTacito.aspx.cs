using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class SondaAcuseTacito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Ctl_Documento ctl_documento = new Ctl_Documento();
                List<TblDocumentos> datos = ctl_documento.ObtenerAcuseTacito("*", "*", "*");

                lblResultado.Text = string.Format("Numero de Documentos a procesar: {0} ", datos.Count().ToString());

                foreach (var item in datos)
                {
                    datos = ctl_documento.ActualizarRespuestaAcuse(item.StrIdSeguridad, (short)AdquirienteRecibo.AprobadoTacito.GetHashCode(), Enumeracion.GetDescription(AdquirienteRecibo.AprobadoTacito));
                }

                lblResultado.Text = string.Format("Documentos procesados: {0} ", datos.Count().ToString());
            }
            catch (Exception ex)
            {

                lblResultado.Text = string.Format("Error en el proceso: {0} ", ex.Message);
            }

        }
    }
}