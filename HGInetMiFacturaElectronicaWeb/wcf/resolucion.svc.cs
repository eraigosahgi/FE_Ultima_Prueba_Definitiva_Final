using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class resolucion : Iresolucion
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}


		/// <summary>
		/// Método Web para consultar las resoluciones registradas ante la DIAN
		/// </summary>
		/// <param name="DataKey">código de seguridad</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <returns></returns>
		public List<Resolucion> Consultar(string DataKey, string Identificacion)
		{
			try
			{
				string id_software = "f715d60f-9880-4b17-91f3-4695ea4f64cf";
				string clave = "HgiNet#811";
				string nit_proveedor = "811021438";
				string nit_empresa = Identificacion;
				Guid id_peticion = Guid.NewGuid();

				string respuesta = HGInetMiFacturaElectonicaController.ServiciosDian.Ctl_ResolucionDian.Obtener(id_peticion, id_software, clave, nit_empresa, nit_proveedor);

				return null;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

	}
}
