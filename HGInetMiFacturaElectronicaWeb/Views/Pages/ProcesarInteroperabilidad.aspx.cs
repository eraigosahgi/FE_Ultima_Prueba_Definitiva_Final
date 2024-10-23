using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaWeb.Properties;
using HGInetMiFacturaElectronicaWeb.Seguridad;
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
    public partial class ProcesarInteroperabilidad : PaginaContenido
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			this.CodigoOpcion = OpcionesPermisos.ProcesarInteroperabilidad;
			this.ProcesoPagina = OperacionesBD.IntConsultar;
		}
		protected void Page_Load(object sender, EventArgs e)
        {
			base.Page_Load(sender, e);

			// Ctl_Envio.Procesar();
		}


        //DescargarFtp("ftp://ftpprueba@habilitacion.mifacturaenlinea.com.co/Doc.zip", "ftpprueba", "Hgi123", "C:\\Users\\jflores.HGI\\Downloads\\Proveedores\\Proveedor1\\Doc.zip");

    }
}