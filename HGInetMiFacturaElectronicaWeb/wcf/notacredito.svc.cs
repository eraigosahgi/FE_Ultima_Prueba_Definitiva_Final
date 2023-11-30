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
	public class notacredito : Inotacredito
	{

		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Método Web para recibir los documentos de tipo Nota Crédito
		/// </summary>
		/// <param name="documentos">colección de documentos de tipo Nota Crédito</param>
		/// <returns>resultado de la operación</returns>
		public List<DocumentoRespuesta> Recepcion(List<NotaCredito> documentos)
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
		/// Obtiene los documentos notas crédito para un adquiriente específico
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado) en formato Sha1</param>
		/// <param name="Identificacion">Número de identificación del adquiriente</param>
		/// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <returns>documentos notas crédito entre fechas por adquiriente</returns>
		public List<NotaCreditoConsulta> ObtenerPorFechasAdquiriente(string DataKey, string Identificacion, DateTime FechaInicio, DateTime FechaFinal)
		{
			try
			{
				
				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

				if (FechaInicio == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

				if (FechaFinal < FechaInicio)
					throw new ApplicationException("Fecha final inválida.");

				List<NotaCreditoConsulta> respuesta = new List<NotaCreditoConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_NotaCredito ctl_documento = new Ctl_NotaCredito();

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

        /// <summary>
        /// Obtiene los documentos de Nota Credito para un adquiriente específico
        /// </summary>
        /// <param name="DataKey">Clave compuesta (serial + identificación obligado) en formato Sha1</param>
        /// <param name="Identificacion">Número de identificación del adquiriente</param>
        /// <param  <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
        /// <returns>documentos facturas entre fechas por adquiriente</returns>
        public List<NotaCreditoConsulta> ObtenerPorIdSeguridadAdquiriente(string DataKey, string Identificacion, string CodigosRegistros, bool Facturador = false)
        {
            try
            {

                if (string.IsNullOrEmpty(DataKey))
                    throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

                if (string.IsNullOrEmpty(Identificacion))
                    throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");


                List<NotaCreditoConsulta> respuesta = new List<NotaCreditoConsulta>();

                //Válida que la key sea correcta.
                Peticion.Validar(DataKey, Identificacion);

                Ctl_NotaCredito ctl_documento = new Ctl_NotaCredito();

                // obtiene los datos
                respuesta = ctl_documento.ObtenerPorIdSeguridadAdquiriente(Identificacion, CodigosRegistros, Facturador);

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
