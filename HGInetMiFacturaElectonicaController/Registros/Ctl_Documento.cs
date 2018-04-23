using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Registros
{
    public class Ctl_Documento : BaseObject<TblDocumentos>
    {
        #region Constructores 

        public Ctl_Documento() : base(new ModeloAutenticacion()) { }
        public Ctl_Documento(ModeloAutenticacion autenticacion) : base(autenticacion) { }

        public Ctl_Documento(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
        #endregion

        /// <summary>
        /// Crea un documento en la Base de Datos
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public TblDocumentos Crear(TblDocumentos documento)
        {
            documento = this.Add(documento);

            return documento;
        }


        public void Validar(TblDocumentos documento)
        {


        }


        #region Obtener

        /// <summary>
        /// Obtiene los documentos por número
        /// </summary>
        /// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
        /// <param name="Identificacion">identificación obligado</param>
        /// <param name="TipoDocumento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
        /// <param name="Numeros">número de los documentos (recibe varios números separados por coma)</param>
        /// <returns></returns>
        public List<DocumentoRespuesta> ConsultaPorNumeros(string identificacion_obligado, int tipo_documento, string Numeros)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(identificacion_obligado))
                    throw new ApplicationException("Número de identificación del obligado inválido.");
                if (tipo_documento < 1 || tipo_documento > 3)
                    throw new ApplicationException("Tipo de documento inválido.");
                if (string.IsNullOrWhiteSpace(Numeros))
                    throw new ApplicationException("Filtro por números inválido.");

                List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

                //Convierte Numeros en una lista.
                List<string> lista_documentos = Coleccion.ConvertirLista(Numeros);

                var respuesta = from documento in context.TblDocumentos
                                join empresa in context.TblEmpresas on documento.IntIdEmpresa equals empresa.IntId
                                where identificacion_obligado.Equals(empresa.StrIdentificacion)
                                 && documento.IntDocTipo == tipo_documento
                                 && lista_documentos.Contains(documento.IntNumero.ToString())
                                select documento;

                //Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
                foreach (TblDocumentos item in respuesta)
                {
                    lista_respuesta.Add(Convertir(item));
                }

                return lista_respuesta;
            }
            catch (Exception exec)
            {
                Error error = new Error(CodigoError.VALIDACION, exec);
                throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
            }
        }

        /// <summary>
        /// Obtiene los documentos por Código de Registros
        /// </summary>
        /// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
        /// <param name="Identificacion">identificación obligado</param>
        /// <param name="TipoDocumento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
        /// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
        /// <returns></returns>
        public List<DocumentoRespuesta> ConsultaPorCodigoRegistro(string identificacion_obligado, int tipo_documento, string CodigosRegistros)
        {
            try
            {
                //Valida que los parametros sean correctos.
                if (string.IsNullOrWhiteSpace(identificacion_obligado))
                    throw new ApplicationException("Número de identificación del obligado inválido.");
                if (tipo_documento < 1 || tipo_documento > 3)
                    throw new ApplicationException("Tipo de documento inválido.");
                if (string.IsNullOrWhiteSpace(CodigosRegistros))
                    throw new ApplicationException("Filtro por números inválido.");

                List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

                //Convierte CodigoRegistros en una lista.
                List<string> lista_documentos = Coleccion.ConvertirLista(CodigosRegistros);

                var respuesta = from documento in context.TblDocumentos
                                join empresa in context.TblEmpresas on documento.IntIdEmpresa equals empresa.IntId
                                where identificacion_obligado.Equals(empresa.StrIdentificacion)
                                && documento.IntDocTipo == tipo_documento
                                && lista_documentos.Contains(documento.StrObligadoIdRegistro)
                                select documento;

                //Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
                foreach (TblDocumentos item in respuesta)
                {
                    lista_respuesta.Add(Convertir(item));
                }

                return lista_respuesta;
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
        public List<DocumentoRespuesta> ConsultaPorFechaElaboracion(string identificacion_obligado, int tipo_documento, DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(identificacion_obligado))
                    throw new ApplicationException("Número de identificación del obligado inválido.");
                if (tipo_documento < 1 || tipo_documento > 3)
                    throw new ApplicationException("Tipo de documento inválido.");
                if (FechaInicial == null)
                    throw new ApplicationException("Fecha inicial inválida.");
                if (FechaFinal == null)
                    throw new ApplicationException("Fecha final inválida.");

                List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

                var respuesta = from documento in context.TblDocumentos
                                join empresa in context.TblEmpresas on documento.IntIdEmpresa equals empresa.IntId
                                where identificacion_obligado.Equals(empresa.StrIdentificacion)
                                 && documento.IntDocTipo == tipo_documento
                                 && (documento.DatFechaIngreso >= FechaInicial && documento.DatFechaIngreso <= FechaFinal)
                                select documento;

                //Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
                foreach (TblDocumentos item in respuesta)
                {
                    lista_respuesta.Add(Convertir(item));
                }

                return lista_respuesta;
            }
            catch (Exception exec)
            {
                Error error = new Error(CodigoError.VALIDACION, exec);
                throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
            }
        }

        #endregion


        /// <summary>
        /// Convierte un Objeto de Servicio a un Objeto de Bd
        /// </summary>
        /// <param name="respuesta">Objeto de respuesta</param>
        /// <param name="factura">Objeto enviado por el usuario</param>
        /// <param name="empresa"></param>
        /// <param name="adquiriente"></param>
        /// <param name="tipo_doc"></param>
        /// <returns></returns>
        public static TblDocumentos Convertir(DocumentoRespuesta respuesta, object documento, TblEmpresas empresa, TblEmpresas adquiriente, TipoDocumento tipo_doc)
        {
            try
            {
                //Valida el tipo de documento enviado por el usuario
                var documento_obj = (dynamic)null;

                if (tipo_doc == TipoDocumento.Factura)
                {
                    documento_obj = documento;
                }
                else if (tipo_doc == TipoDocumento.NotaCredito)
                {
                    documento_obj = documento;
                }
                else if (tipo_doc == TipoDocumento.NotaDebito)
                    documento_obj = documento;

                TblDocumentos tbl_documento = new TblDocumentos();

                tbl_documento.DatFechaIngreso = respuesta.FechaRecepcion;
                tbl_documento.IntDocTipo = Convert.ToInt16(tipo_doc);
                tbl_documento.IntNumero = documento_obj.Documento;
                tbl_documento.StrPrefijo = documento_obj.Prefijo;
                tbl_documento.DatFechaDocumento = documento_obj.Fecha;
                tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
                tbl_documento.StrObligadoIdRegistro = documento_obj.CodigoRegistro;
                tbl_documento.IntIdEmpresaResolucion = empresa.TblEmpresasResoluciones.FirstOrDefault().IntId;
                tbl_documento.IntIdEmpresa = empresa.IntId;
                tbl_documento.IntIdEmpresaAdquiriente = adquiriente.IntId;
                tbl_documento.StrCufe = respuesta.Cufe;
                tbl_documento.IntVlrTotal = documento_obj.Total;
                tbl_documento.StrIdSeguridad = Guid.NewGuid();
                tbl_documento.IntAdquirienteRecibo = 0;
                tbl_documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
                tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();


                return tbl_documento;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Convierte un objeto de tipo de base de datos a objeto de servicio.
        /// </summary>
        /// <param name="respuesta">Objeto de tipo TblDocumentos</param>
        /// <returns></returns>
        public static DocumentoRespuesta Convertir(TblDocumentos respuesta)
        {
            try
            {
                //Valida el tipo de documento enviado por el usuario

                if (respuesta == null)
                    throw new ApplicationException("Está Null");

                DocumentoRespuesta obj_documento = new DocumentoRespuesta();

                obj_documento.Aceptacion = respuesta.IntAdquirienteRecibo;
                obj_documento.CodigoRegistro = respuesta.StrObligadoIdRegistro;
                obj_documento.Cufe = respuesta.StrCufe;
                // obj_documento.DescripcionProceso
                obj_documento.Documento = respuesta.IntNumero;
                obj_documento.FechaRecepcion = respuesta.DatFechaIngreso;
                obj_documento.FechaUltimoProceso = respuesta.DatFechaActualizaEstado;
                obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
                obj_documento.Identificacion = respuesta.TblEmpresas.StrIdentificacion;
                obj_documento.IdProceso = respuesta.IntIdEstado;
                obj_documento.MotivoRechazo = respuesta.StrAdquirienteMvoRechazo;
                obj_documento.NumeroResolucion = respuesta.TblEmpresasResoluciones.StrNumResolucion;
                obj_documento.Prefijo = respuesta.TblEmpresasResoluciones.StrPrefijo;
                // obj_documento.ProcesoFinalizado
                obj_documento.UrlPdf = respuesta.StrUrlArchivoPdf;
                obj_documento.UrlXmlUbl = respuesta.StrUrlArchivoUbl;

                return obj_documento;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }

        }

    }


}
