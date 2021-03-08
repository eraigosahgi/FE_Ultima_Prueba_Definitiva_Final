using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class HGIpayConsultaDocumentos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{


			try
			{
				// obtiene de la ruta GET el id de seguridad del usuario autenticado para crear la sesión
				if (Request.QueryString != null)
				{
					if (!string.IsNullOrWhiteSpace(Request.QueryString["ID"]))
					{
						Sesion sesion = new Sesion();

						System.Guid id_seguridad = new System.Guid(Request.QueryString["ID"]);

						sesion.GuardarSesionWeb(id_seguridad);

						lblUsuario.Text = string.Format("{0} {1}", Sesion.DatosUsuario.StrNombres, Sesion.DatosUsuario.StrApellidos);

					}else
					{
						Response.Redirect("~/Views/Login/Default.aspx");
					}

					if (!string.IsNullOrWhiteSpace(Request.QueryString["Serial"]))
					{
						Ctl_Empresa _controlador_empresa = new Ctl_Empresa();
						TblEmpresas empresa = new TblEmpresas();
						empresa = _controlador_empresa.Obtener(Guid.Parse(Request.QueryString["Serial"]), false).FirstOrDefault();

						if (empresa != null)
						{
							lblNombre_Facturador.Text = empresa.StrRazonSocial;
							lblNit_Facturador.Text = empresa.StrIdentificacion;
						}
						else
						{
							Response.Redirect("~/Views/Login/Default.aspx");
						}

					}else
					{
						Response.Redirect("~/Views/Login/Default.aspx");
					}

				}

				//Ruta de consulta de estado de pago en la plataforma intermedia(Pagos electronicos)
				PasarelaPagos Ruta_servicio_pago = HgiConfiguracion.GetConfiguration().PasarelaPagos;
				Hdf_RutaSrvPagos.Value = Ruta_servicio_pago.RutaPlataforma.ToString();
				Hdf_RutaPagos.Value = Ruta_servicio_pago.RutaPaginaPago.ToString();
			}
			catch (Exception ex)
			{
				Response.Redirect("~/Views/Login/Default.aspx");				
			}

		}
	}
}