using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;

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
                // valida
                Peticion.Validar(DataKey, Identificacion);

                return Ctl_Resoluciones.Obtener(Identificacion);
               
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		/// Método Web para consultar una resolución específica ante la DIAN si no existe la crea automáticamente para versión 2
		/// </summary>
		/// <param name="DataKey">código de seguridad</param>
		/// <param name="Identificacion">número de identificación del Facturador Electrónico</param>
		/// <param name="Resolucion">resolución enviada por el Facturador Electrónico</param>
		/// <returns>resoluciones</returns>
		public List<Resolucion> ConsultarResolucion(Resolucion Resolucion)
		{
			try
			{
				// valida 
				Peticion.Validar(Resolucion.DataKey, Resolucion.Identificacion);

				List<Resolucion> resolucion_creada = Ctl_Resoluciones.CrearHabilitacion(Resolucion, Resolucion.Identificacion);

				// obtiene las resoluciones
				List<Resolucion> resoluciones_respuesta = Ctl_Resoluciones.Obtener(Resolucion.Identificacion);

				//Se agrega la que se creo o actualizo en el proceso.
				resoluciones_respuesta.AddRange(resolucion_creada);

				return resoluciones_respuesta;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

	}
}
