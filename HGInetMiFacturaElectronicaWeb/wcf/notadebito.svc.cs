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
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using HGInetMiFacturaElectonicaController.Registros;

namespace HGInetMiFacturaElectronicaWeb.wcf
{

	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class notadebito : Inotadebito
	{

		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Método Web para recibir los documentos de tipo Nota Débito
		/// </summary>
		/// <param name="documentos">colección de documentos de tipo Nota Débito</param>
		/// <returns>resultado de la operación</returns>
		public List<DocumentoRespuesta> Recepcion(List<NotaDebito> documentos)
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

		/// <summary>
		/// Obtiene los documentos notas débito para un adquiriente específico
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado) en formato Sha1</param>
		/// <param name="Identificacion">Número de identificación del adquiriente</param>
		/// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <returns>documentos notas débito entre fechas por adquiriente</returns>
		public List<NotaDebitoConsulta> ObtenerPorFechasAdquiriente(string DataKey, string Identificacion, DateTime FechaInicio, DateTime FechaFinal)
		{
			try
			{
				List<NotaDebitoConsulta> respuesta = new List<NotaDebitoConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_NotaDebito ctl_documento = new Ctl_NotaDebito();

				// obtiene los datos
				respuesta = ctl_documento.ObtenerPorFechasAdquiriente(Identificacion, FechaInicio, FechaFinal);

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
