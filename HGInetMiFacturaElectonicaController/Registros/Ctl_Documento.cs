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




        public TblDocumentos Actualizar(TblDocumentos documento)
        {
            documento = this.Update(documento);

            return documento;

        }


        #region Obtener

        public TblDocumentos Obtener(string numero_resolucion, int numero_documeto)
        {
            try
            {

                TblDocumentos documento = (from documentos in context.TblDocumentos
                                           where (documentos.IntNumero == numero_documeto)
                                           && (documentos.TblEmpresasResoluciones.StrNumResolucion.Equals(numero_resolucion))
                                           select documentos).FirstOrDefault();

                return documento;


            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


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
                                where empresa.StrIdentificacion.Equals(identificacion_obligado)
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
                                where empresa.StrIdentificacion.Equals(identificacion_obligado)
                                && documento.IntDocTipo == tipo_documento
                                && (lista_documentos.Contains(documento.StrObligadoIdRegistro) || lista_documentos.Contains(documento.StrIdSeguridad.ToString()))
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

                FechaInicial = FechaInicial.Date;
                FechaFinal = FechaFinal.Date.AddDays(1);

                List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

                var respuesta = from documento in context.TblDocumentos
                                join empresa in context.TblEmpresas on documento.IntIdEmpresa equals empresa.IntId
                                where empresa.StrIdentificacion.Equals(identificacion_obligado)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo_adquiente"></param>
        /// <param name="numero_documento"></param>
        /// <param name="estado_recibo"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerPorFechasAdquiriente(string codigo_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {

            fecha_inicio = fecha_inicio.Date;
            fecha_fin = fecha_fin.Date.AddDays(1);

            int num_doc = -1;
            int.TryParse(numero_documento, out num_doc);

            short cod_estado_recibo = -1;
            short.TryParse(estado_recibo, out cod_estado_recibo);

            //fecha_inicio = Convert.ToDateTime(fecha_inicio.ToString(Fecha.formato_fecha_hginet));
            //fecha_fin = Convert.ToDateTime(fecha_inicio.ToString(Fecha.formato_fecha_hginet));

            if (string.IsNullOrWhiteSpace(numero_documento))
                numero_documento = "*";
            if (string.IsNullOrWhiteSpace(estado_recibo))
                estado_recibo = "*";

            var respuesta = (from datos in context.TblDocumentos
                             join empresa in context.TblEmpresas on datos.IntIdEmpresaAdquiriente equals empresa.IntId
                             where (empresa.StrIdentificacion.Equals(codigo_adquiente) || codigo_adquiente.Equals("*"))
                                           && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
                                           && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
                                           && (datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin)
                             orderby datos.IntNumero descending
                             select datos).ToList();

            return respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo_facturador"></param>
        /// <param name="numero_documento"></param>
        /// <param name="codigo_tercero"></param>
        /// <param name="estado_dian"></param>
        /// <param name="estado_recibo"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerPorFechasObligado(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            fecha_inicio = fecha_inicio.Date;
            fecha_fin = fecha_fin.Date.AddDays(1);

            int num_doc = -1;
            int.TryParse(numero_documento, out num_doc);

            short cod_estado_dian = -1;
            short.TryParse(estado_dian, out cod_estado_dian);

            short cod_estado_recibo = -1;
            short.TryParse(estado_recibo, out cod_estado_recibo);

            if (string.IsNullOrWhiteSpace(numero_documento))
                numero_documento = "*";
            if (string.IsNullOrWhiteSpace(codigo_adquiriente))
                codigo_adquiriente = "*";
            if (string.IsNullOrWhiteSpace(estado_dian))
                estado_dian = "*";
            if (string.IsNullOrWhiteSpace(estado_recibo))
                estado_recibo = "*";

            if (estado_dian.Equals("3"))
                estado_dian = "2,3";

            List<string> estados = Coleccion.ConvertirLista(estado_dian);


            List<TblDocumentos> documentos = (from datos in context.TblDocumentos
                                              join obligado in context.TblEmpresas on datos.IntIdEmpresa equals obligado.IntId
                                              join adquiriente in context.TblEmpresas on datos.IntIdEmpresaAdquiriente equals adquiriente.IntId
                                              where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
                                            && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
                                            && (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
                                            && (estados.Contains(datos.IntIdEstado.ToString()) || estado_dian.Equals("*"))
                                            && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
                                            && (datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin)
                                              orderby datos.IntNumero descending
                                              select datos).ToList();

            return documentos;
        }

        /// <summary>
        /// Obtiene los documentos por id se seguridad.
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerPorIdSeguridad(System.Guid id_seguridad)
        {

            try
            {
                var respuesta = (from datos in context.TblDocumentos
                                 where datos.StrIdSeguridad.Equals(id_seguridad)
                                 select datos
                                 );

                return respuesta.ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion


        #region Actualizar

        /// <summary>
        /// Actualiza la respuesta del acuse de recibo.
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <param name="estado"></param>
        /// <param name="motivo_rechazo"></param>
        /// <returns></returns>
        public List<TblDocumentos> ActualizarRespuestaAcuse(System.Guid id_seguridad, short estado, string motivo_rechazo)
        {
            try
            {
                List<TblDocumentos> retorno = new List<TblDocumentos>();
                TblDocumentos doc = ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

                Ctl_Empresa ctl_empresa = new Ctl_Empresa();

                if (doc == null)
                    throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento", doc.IntNumero));

                doc.IntAdquirienteRecibo = estado;
                doc.StrAdquirienteMvoRechazo = motivo_rechazo;
                doc.DatAdquirienteFechaRecibo = Fecha.GetFecha();
                doc.IntIdEstado = 9;
                
                // obtiene los datos del facturador electrónico
                TblEmpresas facturador = ctl_empresa.ObtenerId(doc.IntIdEmpresa);

                // obtiene los datos del adquiriente
                TblEmpresas adquiriente = ctl_empresa.ObtenerId(doc.IntIdEmpresaAdquiriente);

                try
                {   // envía el correo del acuse de recibo al facturador electrónico
                    Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
                    email.RespuestaAcuse(doc, facturador, adquiriente);
                    doc.IntIdEstado = 99;
                }
                catch (Exception) { }

                doc.DatFechaActualizaEstado = Fecha.GetFecha();
                doc = Actualizar(doc);

                doc.TblEmpresas = facturador;
                doc.TblEmpresas1 = adquiriente;
                retorno.Add(doc);

                return retorno;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
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
        public static TblDocumentos Convertir(DocumentoRespuesta respuesta, object documento, TblEmpresasResoluciones resolucion, TblEmpresas empresa, TblEmpresas adquiriente, TipoDocumento tipo_doc)
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

                if (tipo_doc == TipoDocumento.NotaCredito || tipo_doc == TipoDocumento.NotaDebito)
                {
                    tbl_documento.StrPrefijo = "";
                    tbl_documento.DatFechaVencDocumento = documento_obj.Fecha;
                }
                else
                {
                    tbl_documento.StrPrefijo = (string.IsNullOrEmpty(documento_obj.Prefijo)) ? documento_obj.Prefijo : "";
                    tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
                }
                tbl_documento.DatFechaDocumento = documento_obj.Fecha;
                tbl_documento.StrObligadoIdRegistro = documento_obj.CodigoRegistro;
                tbl_documento.IntIdEmpresaResolucion = resolucion.IntId;
                tbl_documento.IntIdEmpresa = empresa.IntId;
                tbl_documento.IntIdEmpresaAdquiriente = adquiriente.IntId;
                tbl_documento.StrCufe = respuesta.Cufe;
                tbl_documento.IntVlrTotal = documento_obj.Total;
                tbl_documento.StrIdSeguridad = Guid.Parse(respuesta.IdDocumento);
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
                obj_documento.Documento = respuesta.IntNumero;
                obj_documento.FechaRecepcion = respuesta.DatFechaIngreso;
                obj_documento.FechaUltimoProceso = respuesta.DatFechaActualizaEstado;
                obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
                obj_documento.Identificacion = respuesta.TblEmpresas.StrIdentificacion;
                obj_documento.IdProceso = respuesta.IntIdEstado;
                obj_documento.MotivoRechazo = respuesta.StrAdquirienteMvoRechazo;
                obj_documento.NumeroResolucion = respuesta.TblEmpresasResoluciones.StrNumResolucion;
                obj_documento.Prefijo = respuesta.TblEmpresasResoluciones.StrPrefijo;
                obj_documento.ProcesoFinalizado = 0;
                obj_documento.UrlPdf = respuesta.StrUrlArchivoPdf;
                obj_documento.UrlXmlUbl = respuesta.StrUrlArchivoUbl;


                // obj_documento.DescripcionProceso

                switch (respuesta.IntIdEstado)
                {
                    case 1:
                        obj_documento.DescripcionProceso = "Recepción - Información del documento.";
                        break;

                    case 2:
                        obj_documento.DescripcionProceso = "Valida la información del documento.";
                        break;

                    case 3:
                        obj_documento.DescripcionProceso = "Genera información en estandar UBL.";
                        break;

                    case 4:
                        obj_documento.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
                        break;

                    case 5:
                        obj_documento.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
                        break;

                    case 6:
                        obj_documento.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
                        break;

                    case 7:
                        obj_documento.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
                        break;

                    case 8:
                        obj_documento.DescripcionProceso = "Envío correo adquiriente";
                        break;

                    case 9:
                        obj_documento.DescripcionProceso = "Recepción acuse de recibo del Adquiriente";
                        break;

                    case 10:
                        obj_documento.DescripcionProceso = "Envío correo acuse de recibo al facturador";
                        break;


                    case 90:
                        obj_documento.DescripcionProceso = "Finalizado con inconsistencias.";
                        obj_documento.ProcesoFinalizado = 1;
                        break;

                    case 99:
                        obj_documento.DescripcionProceso = "Termina proceso";
                        obj_documento.ProcesoFinalizado = 1;
                        break;

                    default:
                        obj_documento.DescripcionProceso = "Proceso desconocido";
                        break;
                }

                if (respuesta.IntIdEstado == 1)
                    obj_documento.DescripcionProceso = "Recepción - Información del documento.";

                return obj_documento;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }

        }

    }


}
