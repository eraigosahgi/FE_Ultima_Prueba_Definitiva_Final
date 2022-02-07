using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.PagosElectronicos;
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
			Ctl_PagosElectronicos controlador = new Ctl_PagosElectronicos();

			string Facturador = Request.QueryString["Facturador"];

			string Fecha_Inicial = Request.QueryString["FechaInicial"];
			string Fecha_Final = Request.QueryString["FechaFinal"];

			if (!string.IsNullOrEmpty(Facturador))
			{
				DateTime FechaInicial = Convert.ToDateTime(Fecha_Inicial);
				DateTime FechaFinal = Convert.ToDateTime(Fecha_Final);
				FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);
				var datos = controlador.ConsultaAgrupadosPorFechaElaboracion(Facturador, FechaInicial, FechaFinal, 0);
				
			}
		}
	}
}