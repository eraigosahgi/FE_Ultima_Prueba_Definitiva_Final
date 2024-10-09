using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using HGInetMiFacturaElectonicaController.Procesos;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class documentos : Idocumentos
    {
        public string DoWork()
        {
            return "¡Prueba correcta!";
        }

        /// <summary>
        /// Obtiene los documentos por número
        /// </summary>
        /// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
        /// <param name="Identificacion">identificación obligado</param>
        /// <param name="TipoDocumento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
        /// <param name="Numeros">número de los documentos (recibe varios números separados por coma)</param>
        /// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorNumeros(string DataKey, string Identificacion, int TipoDocumento, string Numeros)
        {
            try
            {
                List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                //Válida que la key sea correcta.
                Peticion.Validar(DataKey, Identificacion);

                Ctl_Documento ctl_documento = new Ctl_Documento();

                //Obtiene los datos
                respuesta = ctl_documento.ConsultaPorNumeros(Identificacion, TipoDocumento, Numeros);

				if (respuesta == null || respuesta.Count == 0)
				{
					List<DocumentoRespuesta> datosH = new List<DocumentoRespuesta>();

					datosH = ctl_documento.ConsultaHisPorNumeros(Identificacion, TipoDocumento, Numeros);

					if (datosH != null && datosH.Count > 0)
					{
						if (respuesta != null && respuesta.Count > 0)
						{
							respuesta.AddRange(datosH);
						}
						else
						{
							respuesta = datosH;
						}

					}
				}

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorNumeros", DataKey, Identificacion, TipoDocumento.ToString(), Numeros, respuesta.Count.ToString());
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

        /// <summary>
        /// Obtiene los documentos por Código de Registro
        /// </summary>
        /// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
        /// <param name="Identificacion">identificación obligado</param>
        /// <param name="TipoDocumento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
        /// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
        /// <returns></returns>
        public List<DocumentoRespuesta> ConsultaPorCodigoRegistro(string DataKey, string Identificacion, int TipoDocumento, string CodigosRegistros)
        {
            try
            {
                List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                //Válida que la key sea correcta.
                Peticion.Validar(DataKey, Identificacion);

                Ctl_Documento ctl_documento = new Ctl_Documento();

                //Obtiene los datos
                respuesta = ctl_documento.ConsultaPorCodigoRegistro(Identificacion, TipoDocumento, CodigosRegistros);

				if (respuesta == null || respuesta.Count == 0)
				{
					List<DocumentoRespuesta> datosH = new List<DocumentoRespuesta>();

					datosH = ctl_documento.ConsultaHisPorCodigoRegistro(Identificacion, TipoDocumento, CodigosRegistros);

					if (datosH != null && datosH.Count > 0)
					{
						if (respuesta != null && respuesta.Count > 0)
						{
							respuesta.AddRange(datosH);
						}
						else
						{
							respuesta = datosH;
						}

					}
				}

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorCodigoRegistro", DataKey, Identificacion, TipoDocumento.ToString(), CodigosRegistros.ToString(), respuesta.Count.ToString());
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

        /// <summary>
        /// Obtiene los documentos por rangos de fecha de elaboración.
        /// </summary>
        /// <param name="identificacion_obligado">identificación obligado</param>
        /// <param name="tipo_documento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
        /// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
        /// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
        /// <returns></returns>
        public List<DocumentoRespuesta> ConsultaPorFechaElaboracion(string DataKey, string Identificacion, int TipoDocumento, DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {	// se restringe el acceso al servicio web NOVIEMBRE 16
				//DateTime fecha_control = new DateTime(2020,11,15).Date;
				//DateTime fecha_actual = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha().Date;

				//if (fecha_control < fecha_actual)
				//	throw new ApplicationException("El servicio web ConsultaPorFechaElaboracion no se encuentra disponible; por favor realice la consulta mediante el servicio web ConsultaPorCodigoRegistro.");

				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

				if (FechaInicial == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

				if (FechaFinal < FechaInicial)
					throw new ApplicationException("Fecha final inválida.");

				long dif_fecha = LibreriaGlobalHGInet.Funciones.Fecha.Diferencia(FechaInicial, FechaFinal, LibreriaGlobalHGInet.Funciones.Fecha.DateInterval.Day);

				if (dif_fecha > 1)
					throw new ApplicationException("La consulta supera el maximo de 1 día; por favor realice la consulta teniendo en cuenta este maximo");

				List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                //Válida que la key sea correcta.
                Peticion.Validar(DataKey, Identificacion);

                Ctl_Documento ctl_documento = new Ctl_Documento();

                //Obtiene los datos
                respuesta = ctl_documento.ConsultaPorFechaElaboracion(Identificacion, TipoDocumento, FechaInicial, FechaFinal);

				DateTime fecha_corte = new DateTime(2024, 01, 01, 00, 00, 00);

				bool obtener_historico = true;

				if (FechaInicial >= fecha_corte)
				{
					obtener_historico = false;
				}

				if (obtener_historico == true)
				{
					List<DocumentoRespuesta> datosH = new List<DocumentoRespuesta>();

					datosH = ctl_documento.ConsultaHisPorFechaElaboracion(Identificacion, TipoDocumento, FechaInicial, FechaFinal);

					if (datosH != null && datosH.Count > 0)
					{
						if (respuesta != null && respuesta.Count > 0)
						{
							respuesta.AddRange(datosH);
						}
						else
						{
							respuesta = datosH;
						}

					}
				}

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorFechaElaboracion", DataKey, Identificacion, TipoDocumento.ToString(), FechaInicial.ToString(), FechaFinal.ToString(), respuesta.Count.ToString());
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

        public List<DocumentoRespuesta> Recepcion(List<DocumentoArchivo> documentos)
        {
            try
            {

				//throw new ApplicationException("Funcionalidad temporalmente fuera de servicio");

				return Ctl_Documentos.Procesar(documentos);

            }
            catch (Exception exec)
            {
                Error error = new Error(CodigoError.VALIDACION, exec);
                throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
            }
        }

		/// <summary>
		/// Calcula el CUFE o CUDE y QR para la representación gráfica del documento electrónico
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="TipoDocumento">tipo documento 1: factura - 2: nota débito - 3: nota crédito</param>
		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<DocumentoCufe> ObtenerCufe(List<DocumentoCufe> documentos_cufe)
		{
			try
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				documentos_cufe = ctl_documento.ObtenerCufe(documentos_cufe);

				return documentos_cufe;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}
	}
}
