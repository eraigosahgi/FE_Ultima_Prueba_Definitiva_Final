using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaWeb.Properties;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using HGInetMiFacturaElectronicaWeb.Seguridad.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class ConsultaDocumentoRadian : PaginaContenido
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			this.CodigoOpcion = OpcionesPermisos.ConsultaDocumentosRadian;
			this.ProcesoPagina = OperacionesBD.IntConsultar;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.Page_Load(sender, e);			

			try
			{
				if (!Sesion.DatosEmpresa.IntRadian)
				{
					throw new ApplicationException("El usuario no tiene permisos para esta ejecución.");


				}
			}
			catch (Exception excepcion)
			{

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				this.RutaRedireccionAlerta =  string.Format("{0}/Views/Pages/Inicio.aspx", plataforma.RutaPublica);

				CustomMaster.Modal = new SweetAlert("Alerta", excepcion.Message, true, this.RutaRedireccionAlerta, SweetAlert.TipoMensaje.warning);
				CustomMaster.MostrarModal();
			}

		}
	}
}