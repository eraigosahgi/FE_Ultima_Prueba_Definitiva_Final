using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;

namespace HGInetMiFacturaElectronicaWeb.wcf
{

	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class factura : Ifactura
	{

		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Método Web para recibir los documentos de tipo Factura
		/// </summary>
		/// <param name="documentos">colección de documentos de tipo Factura</param>
		/// <returns>resultado de la operación</returns>
		public List<DocumentoRespuesta> Recepcion(List<Factura> documentos)
		{
			try
			{
				// id de la petición en la plataforma
				Guid id_peticion = Guid.NewGuid();

				return Ctl_Documentos.Procesar(documentos);

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

        /// <summary>
		/// Método Web para probar Formato
		/// </summary>
		/// <param name="formato">formato</param>
		/// <returns>resultado de la operación</returns>
		private string TestFormato(Formato formato)
        {
            try
            {
                // id de la petición en la plataforma
                Guid id_peticion = Guid.NewGuid();

                string nombre_pdf = string.Empty;

                nombre_pdf = Ctl_Formato.GuardarArchivo(formato, "log_pdf", id_peticion.ToString());

                return nombre_pdf;
            }
            catch (Exception exec)
            {
                Error error = new Error(CodigoError.VALIDACION, exec);
                throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
            }
        }

    }
}
