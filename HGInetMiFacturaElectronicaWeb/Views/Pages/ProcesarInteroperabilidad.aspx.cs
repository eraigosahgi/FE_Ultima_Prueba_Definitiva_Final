using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Procesos;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class ProcesarInteroperabilidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            Ctl_Envio.Procesar();
        }


        //DescargarFtp("ftp://ftpprueba@habilitacion.mifacturaenlinea.com.co/Doc.zip", "ftpprueba", "Hgi123", "C:\\Users\\jflores.HGI\\Downloads\\Proveedores\\Proveedor1\\Doc.zip");

    }
}