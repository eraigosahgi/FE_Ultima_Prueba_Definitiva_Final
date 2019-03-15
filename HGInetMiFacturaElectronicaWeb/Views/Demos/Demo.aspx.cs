using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Demos
{
    public partial class Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			Ctl_DocumentosAudit controlador = new Ctl_DocumentosAudit();
			MensajeValidarEmail objteto = controlador.ObtenerResultadoEmail(Guid.Parse("486E1530-6FBB-471C-8552-E8D9D9C693D3"));
		}
    }
}