using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaData.ModeloAuditoria;
using HGInetMiFacturaElectronicaAudit.Controladores;
using HGInetMiFacturaElectronicaAudit.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Demos
{
    public partial class Demos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


			//HGInetMiFacturaElectonicaController.Auditorias.Ctl_DocumentosAudit ControladormongoDB = new HGInetMiFacturaElectonicaController.Auditorias.Ctl_DocumentosAudit();


			//var Datos = ControladormongoDB.Obtener(DateTime.Parse("2018-01-01"),DateTime.Parse("2019-02-21"));
			//HGInetMiFacturaElectronicaAudit.Controladores.Srv_DocumentosAudit Controlador = new HGInetMiFacturaElectronicaAudit.Controladores.Srv_DocumentosAudit();



			//foreach (var item in Datos)
			//{
			//	try
			//	{
			//		TblAuditDocumentos DocAudit = new TblAuditDocumentos();

			//		DocAudit.Id = Guid.NewGuid();
			//		DocAudit.DatFecha = item.DatFecha;
			//		DocAudit.IntIdEstado = item.IntIdEstado;
			//		DocAudit.IntIdProcesadoPor = item.IntIdProcesadoPor;
			//		DocAudit.IntIdProceso = item.IntIdProceso;
			//		DocAudit.IntTipoRegistro = item.IntTipoRegistro;
			//		DocAudit.StrIdPeticion = item.StrIdPeticion;
			//		DocAudit.StrIdSeguridad = Guid.Parse(item.StrIdSeguridad);
			//		DocAudit.StrMensaje = item.StrMensaje;
			//		DocAudit.StrNumero = item.StrNumero;
			//		DocAudit.StrObligado = item.StrObligado;
			//		DocAudit.StrPrefijo = item.StrPrefijo;
			//		DocAudit.StrRealizadoPor = item.StrRealizadoPor;
			//		DocAudit.StrResultadoProceso = item.StrResultadoProceso;

			//		Controlador.Crear(DocAudit);
			//	}
			//	catch (Exception ex)
			//	{

			//		throw;
			//	}
			//}

			

        }
    }
}