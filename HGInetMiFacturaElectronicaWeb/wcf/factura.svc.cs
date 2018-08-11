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
using HGInetMiFacturaElectonicaController.Registros;

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
		/// Obtiene los documentos facturas para un adquiriente específico en un rango de tiempo especifico
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado) en formato Sha1</param>
		/// <param name="Identificacion">Número de identificación del adquiriente</param>
		/// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <returns>documentos facturas entre fechas por adquiriente</returns>
		public List<FacturaConsulta> ObtenerPorFechasAdquiriente(string DataKey, string Identificacion, DateTime FechaInicio, DateTime FechaFinal)
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

				List<FacturaConsulta> respuesta = new List<FacturaConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Factura ctl_documento = new Ctl_Factura();

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
		/// Obtiene los documentos facturas para un adquiriente específico en un rango de tiempo especifico
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado) en formato Sha1</param>
		/// <param name="Identificacion">Número de identificación del adquiriente</param>
		/// <param  <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns>documentos facturas entre fechas por adquiriente</returns>
		public List<FacturaConsulta> ObtenerPorIdSeguridadAdquiriente(string DataKey, string Identificacion, string CodigosRegistros)
        {
            try
            {

                if (string.IsNullOrEmpty(DataKey))
                    throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

                if (string.IsNullOrEmpty(Identificacion))
                    throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");


                List<FacturaConsulta> respuesta = new List<FacturaConsulta>();

                //Válida que la key sea correcta.
                Peticion.Validar(DataKey, Identificacion);

                Ctl_Factura ctl_documento = new Ctl_Factura();

                // obtiene los datos
                respuesta = ctl_documento.ObtenerPorIdSeguridadAdquiriente(Identificacion, CodigosRegistros);

                return respuesta;

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
