using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class ajustenomina : Iajustenomina
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Método Web para recibir los documentos de tipo Factura
		/// </summary>
		/// <param name="documentos">colección de documentos de tipo Nomina</param>
		/// <returns>resultado de la operación</returns>
		public List<DocumentoRespuesta> Recepcion(List<NominaAjuste> documentos)
		{
			try
			{

				return Ctl_Documentos.Procesar(documentos);

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}
	}
}
