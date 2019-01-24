using HGInetMiFacturaElectonicaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class AcuseRecibo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Ruta de servicio para generar el pago en la plataforma intermedia (Pagos electronicos)
            PasarelaPagos Ruta_Pago = HgiConfiguracion.GetConfiguration().PasarelaPagos;
            Hdf_RutaPagos.Value = Ruta_Pago.RutaPaginaPago.ToString();

            //Ruta de consulta de estado de pago en la plataforma intermedia(Pagos electronicos)
            PasarelaPagos Ruta_servicio_pago = HgiConfiguracion.GetConfiguration().PasarelaPagos;
            Hdf_RutaSrvPagos.Value = Ruta_servicio_pago.RutaServicio.ToString();

        }
    }
}