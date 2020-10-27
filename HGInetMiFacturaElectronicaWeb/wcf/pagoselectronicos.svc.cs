using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class pagoselectronicos : Ipagoselectronicos
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Obtiene los pagos electronicos de documentos por Código de Registro
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>		
		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<PagoElectronicoRespuesta> ConsultaPorCodigoRegistro(string DataKey, string Identificacion, string CodigosRegistros)
		{
			try
			{
				List<PagoElectronicoRespuesta> respuesta = new List<PagoElectronicoRespuesta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				respuesta = ctl_documento.ConsultaPorCodigoRegistro(Identificacion,  CodigosRegistros);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("PagoConsultaPorCodigoRegistro", DataKey, Identificacion, CodigosRegistros.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

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
