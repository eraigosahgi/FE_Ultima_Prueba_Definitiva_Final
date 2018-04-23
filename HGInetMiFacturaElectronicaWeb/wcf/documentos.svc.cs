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
        /// <param name="TipoDocumento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
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
        /// <param name="TipoDocumento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
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
        /// <param name="tipo_documento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
        /// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
        /// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
        /// <returns></returns>
        public List<DocumentoRespuesta> ConsultaPorFechaElaboracion(string DataKey, string Identificacion, int TipoDocumento, DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {
                List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                //Válida que la key sea correcta.
                Peticion.Validar(DataKey, Identificacion);

                Ctl_Documento ctl_documento = new Ctl_Documento();

                //Obtiene los datos
                respuesta = ctl_documento.ConsultaPorFechaElaboracion(Identificacion, TipoDocumento, FechaInicial, FechaFinal);

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
