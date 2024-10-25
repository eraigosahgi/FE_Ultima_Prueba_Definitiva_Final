using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class SondaAcuseTacito : System.Web.UI.Page
    {
        protected  void Page_Load(object sender, EventArgs e)
        {
            try
            {
              Procesar();                
            }
            catch (Exception ex)
            {
				RegistroLog.EscribirLog(ex, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
				lblResultado.Text = string.Format("Error en el proceso: {0} ", ex.Message);
            }
        }


        public void Procesar()
        {
            Ctl_Documento ctl_documento = new Ctl_Documento();

            var Tarea1 = ctl_documento.sonda();
            lblResultado.Text = string.Format("Termino");
        }
    }
}