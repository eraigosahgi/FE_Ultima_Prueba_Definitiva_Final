using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    public partial class Ctl_Documentos
    {

        /// <summary>
        /// Procesa una lista de documentos tipo Factura
        /// </summary>
        /// <param name="documentos">documentos tipo Factura</param>
        /// <returns>Objeto tipo respuesta</returns>
        public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
        {
            try
            {
                Ctl_Empresa Peticion = new Ctl_Empresa();

                //Válida que la key sea correcta.
                TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion);

                if (!facturador_electronico.IntObligado)
                    throw new ApplicationException(string.Format("Licencia inválida para la Identificacion {0}.", facturador_electronico.StrIdentificacion));

                // genera un id único de la plataforma
                Guid id_peticion = Guid.NewGuid();

                DateTime fecha_actual = Fecha.GetFecha();

                List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

                // sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
                if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
                {

                    Tercero DatosObligado = new Tercero()
                    {
                        Identificacion = "811021438",
                        IdentificacionDv = 4,
                        TipoIdentificacion = 31,
                        TipoPersona = 1,
                        Regimen = 2,
                        NombreComercial = "HGI",
                        Departamento = "Antioquia",
                        Ciudad = "Medellin",
                        Direccion = "Calle 48 Nro. 77C-06",
                        Telefono = "4444584",
                        Email = "info@hgi.com.co",
                        PaginaWeb = null,
                        CodigoPais = "CO",
                        RazonSocial = "HGI SAS",
                        PrimerApellido = null,
                        SegundoApellido = null,
                        PrimerNombre = null,
                        SegundoNombre = null
                    };

                    //Valida que Resolucion tomar, con Prefijo o sin Prefijo
                    string resolucion_pruebas = string.Empty;
                    string nit_resolucion = string.Empty;
                    string prefijo_prueba = string.Empty;
                    if (documentos.FirstOrDefault().Prefijo.Equals(string.Empty))
                    {
                        resolucion_pruebas = Constantes.ResolucionPruebas;
                        nit_resolucion = Constantes.NitResolucionsinPrefijo;

                    }
                    else
                    {
                        resolucion_pruebas = Constantes.ResolucionPruebas;
                        nit_resolucion = Constantes.NitResolucionconPrefijo;
                        prefijo_prueba = Constantes.PrefijoResolucionPruebas;
                    }



                    Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
                    lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_prueba));

                    foreach (var item in documentos)
                    {
                        item.NumeroResolucion = resolucion_pruebas;
                        item.DatosObligado = DatosObligado;
                        item.Prefijo = prefijo_prueba;

                    }
                }
                else
                {
                    // actualiza las resoluciones de los servicios web de la DIAN en la base de datos
                    lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, documentos.FirstOrDefault().DatosObligado.Identificacion);
                }


                if (lista_resolucion == null)
                    throw new ApplicationException(string.Format("No se encontraron las resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));
                else if (!lista_resolucion.Any())
                    throw new ApplicationException(string.Format("No se encontraron las resoluciones para el Facturador Electrónico '{0}'", facturador_electronico.StrIdentificacion));


                List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                Parallel.ForEach<Factura>(documentos, item =>
                {
                    DocumentoRespuesta item_respuesta = Procesar(item, facturador_electronico, id_peticion, fecha_actual, lista_resolucion);
                    respuesta.Add(item_respuesta);
                });

                PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                if (plataforma_datos.EnvioSms)
                {
                    Ctl_Sms.EnviarSms(respuesta, id_peticion, facturador_electronico, documentos);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                LogExcepcion.Guardar(ex);
                throw new ApplicationException(ex.Message);
            }
        }


        /// <summary>
        /// Procesa un documento por paralelismo
        /// </summary>
        /// <param name="item">objeto de datos factura</param>
        /// <param name="facturador_electronico">facturador electrónico del documento</param>
        /// <param name="id_peticion">identificador de petición</param>
        /// <param name="fecha_actual">fecha actual de recepción del documento</param>
        /// <param name="lista_resolucion">resoluciones habilitadas para el facturador electrónico</param>
        /// <returns>resultado del proceso</returns>
        private static DocumentoRespuesta Procesar(Factura item, TblEmpresas facturador_electronico, Guid id_peticion, DateTime fecha_actual, List<TblEmpresasResoluciones> lista_resolucion)
        {
            DocumentoRespuesta item_respuesta = new DocumentoRespuesta();
            try
            {
                if (string.IsNullOrEmpty(item.NumeroResolucion))
                    throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));


                Ctl_Documento num_doc = new Ctl_Documento();

                //valida si el Documento ya existe en Base de Datos
                TblDocumentos numero_documento = num_doc.Obtener(item.DatosObligado.Identificacion, item.Documento, item.Prefijo);

                if (numero_documento != null)
                    throw new ApplicationException(string.Format("El documento {0} ya existe para el Facturador Electrónico {1}", item.Documento, facturador_electronico.StrIdentificacion));

                TblEmpresasResoluciones resolucion = null;

                try
                {
                    ApplicationException exTMP = new ApplicationException(string.Format("DataRes: {0}", lista_resolucion.FirstOrDefault().StrIdSeguridad));

                    LogExcepcion.Guardar(exTMP);

                    // filtra la resolución del documento
                    resolucion = lista_resolucion.Where(_resolucion_doc => _resolucion_doc.StrEmpresa.Equals(item.DatosObligado.Identificacion)
                                                                            && _resolucion_doc.StrPrefijo.Equals(item.Prefijo)
                                                                            && _resolucion_doc.StrNumResolucion.Equals(item.NumeroResolucion)).FirstOrDefault();

                }
                catch (Exception excepcion)
                {
                    throw new ApplicationException(string.Format("No se encontró la resolución {0} para el Facturador Electrónico {1}", item.NumeroResolucion, facturador_electronico.StrIdentificacion));
                }

                if (resolucion == null)
                    throw new ApplicationException(string.Format("No se encontró la resolución {0} para el Facturador Electrónico {1} con prefijo '{2}'", item.NumeroResolucion, facturador_electronico.StrIdentificacion, item.Prefijo));



                // realiza el proceso de envío a la DIAN del documento
                item_respuesta = Procesar(id_peticion, item, TipoDocumento.Factura, resolucion, facturador_electronico);
            }
            catch (Exception excepcion)
            {

                ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
                LogExcepcion.Guardar(excepcion);
                item_respuesta = new DocumentoRespuesta()
                {
                    Aceptacion = 0,
                    CodigoRegistro = item.CodigoRegistro,
                    Cufe = "",
                    DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
                    DocumentoTipo = TipoDocumento.Factura.GetHashCode(),
                    Documento = item.Documento,
                    Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                    EstadoDian = null,
                    FechaRecepcion = fecha_actual,
                    FechaUltimoProceso = fecha_actual,
                    IdDocumento = "",
                    Identificacion = "",
                    IdProceso = proceso_actual.GetHashCode(),
                    MotivoRechazo = "",
                    NumeroResolucion = item.NumeroResolucion,
                    Prefijo = item.Prefijo,
                    ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                    UrlPdf = "",
                    UrlXmlUbl = ""
                };

            }
            if (item_respuesta.Error == null)
                item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error();

            return item_respuesta;

        }

        /// <summary>
        /// Validación del Objeto Factura
        /// </summary>
        /// <param name="documento">Objeto factura</param>
        /// <returns></returns>
        public static Factura Validar(Factura documento, TblEmpresasResoluciones resolucion)
        //public static Factura Validar(Factura documento)
        {
            // valida objeto recibido
            if (documento == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Factura"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.CodigoRegistro))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.DataKey))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

            // valida el número del documento no sea valor negativo
            if (documento.Documento < 0)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

            //Inicializa la propiedad, no es un campo requerido
            if (string.IsNullOrEmpty(documento.DocumentoRef))
                documento.DocumentoRef = string.Empty;

            //setea el campo y lo deja en blanco
            documento.Cufe = string.Empty;

            //Validar que no este vacio y este vigente en los terminos.
            if (string.IsNullOrEmpty(documento.NumeroResolucion))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));

            //valida resolucion
            if (!resolucion.StrNumResolucion.Equals(documento.NumeroResolucion))
                throw new ApplicationException(string.Format("El número de resolución '{0}' no es válido", documento.NumeroResolucion));

            //valida número de la Factura este entre los rangos
            if (documento.Documento < resolucion.IntRangoInicial || documento.Documento > resolucion.IntRangoFinal)
                throw new ApplicationException(string.Format("El número del documento '{0}' no es válido según la resolución", documento.Documento));

            if (!resolucion.StrPrefijo.Equals(documento.Prefijo))
                throw new ApplicationException(string.Format("El prefijo '{0}' no es válido según Resolución", documento.Prefijo));

            //Valida que la fecha este en los terminos
            if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
                throw new ApplicationException(string.Format("La fecha de elaboración '{0}' no está dentro los términos.", documento.Fecha));

            if (documento.FechaVence.Date < documento.Fecha.Date)
                throw new ApplicationException(string.Format("La fecha de vencimiento '{0}' debe ser mayor o igual a la fecha de elaboración del documento '{1}'", documento.FechaVence, documento.Fecha));

            //Valida que no este vacio y Formato
            if (string.IsNullOrEmpty(documento.Moneda))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

            if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor '{1}' según ISO 4217", "Moneda", documento.Moneda));

            //Valida que el codigo del formato que envia este disponible.
            if (string.IsNullOrEmpty(documento.DocumentoFormato.ArchivoPdf))
            {
                if (documento.DocumentoFormato.Codigo < 1 || documento.DocumentoFormato.Codigo > 4)
                    throw new ApplicationException(string.Format("El Formato '{0}' no se encuentra disponible en la plataforma.", documento.DocumentoFormato.Codigo));
            }

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosObligado, "Obligado");

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

            //Valida totales del objeto
            ValidarTotales(documento, null, null, TipoDocumento.Factura);

            return documento;
        }

    }
}
