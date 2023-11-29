using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LibreriaGlobalHGInet.Error;
using HGInetMiFacturaElectonicaData.ModeloServicio;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "HGInetFacturaElectronica.ServiciosWcf", Name = "ServicioEmpresas")]
    public interface Iempresas
    {
        [OperationContract(Name = "Test")]
        [WebInvoke(Method = "GET")]
        string DoWork();

        [OperationContract(Name = "Obtener")]
        [FaultContract(typeof(Error), Action = "Obtener", Name = "Error")]
        [WebInvoke(Method = "GET")]
        Empresa Obtener(string DataKey, string Identificacion);

        [OperationContract(Name = "ConsultarAdquiriente")]
        [FaultContract(typeof(Error), Action = "ConsultarAdquiriente", Name = "Error")]
        [WebInvoke(Method = "GET")]
        Empresa ConsultarAdquiriente(string DataKey, string IdentificacionEmisor, string IdentificacionAdquiriente);

        [OperationContract(Name = "Crear")]
        [FaultContract(typeof(Error), Action = "Crear", Name = "Error")]
        [WebInvoke(Method = "Post")]
        bool Crear(Empresa empresa_nueva);
    }
}
