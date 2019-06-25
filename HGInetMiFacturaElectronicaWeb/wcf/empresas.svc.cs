using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using LibreriaGlobalHGInet.Error;
using System.ServiceModel.Activation;
using System.Text;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class empresas : Iempresas
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Obtiene la información de la empresa
		/// </summary>
		/// <param name="DataKey"></param>
		/// <param name="Identificacion"></param>
		/// <returns></returns>
		public Empresa Obtener(string DataKey, string Identificacion)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(DataKey))
					throw new ApplicationException("DataKey de la empresa inválido.");

				Empresa respuesta = new Empresa();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				//Obtiene los datos de la empresa.
				TblEmpresas datos_tbl = ctl_empresa.Obtener(Identificacion);

				//Valida si se obtuvieron datos y convierte la tbl a Empresa.
				if (datos_tbl != null)
					respuesta = Ctl_Empresa.ConvertirEmpresa(datos_tbl);

				return respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}
	}
}
