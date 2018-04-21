using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.Formato;
using System.Text.RegularExpressions;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData;
using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    /// <summary>
    /// Controlador para gestionar los documentos
    /// </summary>
    public class Ctl_Documentos
    {

        /// <summary>
        /// Realiza el proceso de la plataforma para el documento
        /// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN
        /// </summary>
        /// <param name="id_peticion">id único de identificación de la plataforma</param>
        /// <param name="documento_obj">datos del documento</param>
        /// <param name="pruebas">indica si el documento es de pruebas (true)</param>
        /// <returns>datos de resultado para el documento</returns>
        //public static DocumentoRespuesta Procesar(Guid id_peticion, object documento, TipoDocumento tipo_doc, List<Resolucion> resoluciones, bool pruebas = false, bool solo_validar = false)
        public static DocumentoRespuesta Procesar(Guid id_peticion, object documento, TipoDocumento tipo_doc, bool pruebas = false, bool solo_validar = false)
        {
            string numero_resolucion = string.Empty;
            string prefijo = string.Empty;

            var documento_obj = (dynamic)null;

            if (tipo_doc == TipoDocumento.Factura)
            {
                documento_obj = documento;
                numero_resolucion = documento_obj.NumeroResolucion;
                prefijo = documento_obj.Prefijo;
            }
            else if (tipo_doc == TipoDocumento.NotaCredito)
            {
                documento_obj = documento;
            }
            else if (tipo_doc == TipoDocumento.NotaDebito)
                documento_obj = documento;

            if (documento_obj != null)
            {

                DateTime fecha_actual = Fecha.GetFecha();

                FacturaE_Documento documento_result = new FacturaE_Documento();

                DocumentoRespuesta respuesta = new DocumentoRespuesta()
                {
                    Aceptacion = 0,
                    CodigoRegistro = documento_obj.CodigoRegistro,
                    Cufe = "",
                    DescripcionProceso = "Recepción - Información del documento.",
                    Documento = documento_obj.Documento,
                    Error = null,
                    FechaRecepcion = fecha_actual,
                    FechaUltimoProceso = fecha_actual,
                    IdDocumento = Guid.NewGuid().ToString(),
                    Identificacion = documento_obj.DatosObligado.Identificacion,
                    IdProceso = 1,
                    MotivoRechazo = "",
                    NumeroResolucion = numero_resolucion,
                    Prefijo = prefijo,
                    ProcesoFinalizado = 0,
                    UrlPdf = "",
                    UrlXmlUbl = ""
                };

                try
                {

                    // valida la información del documento
                    try
                    {
                        fecha_actual = Fecha.GetFecha();
                        respuesta.DescripcionProceso = "Valida la información del documento.";
                        respuesta.FechaUltimoProceso = fecha_actual;
                        respuesta.IdProceso = 2;
                        /*
                        List<Resolucion> resolucion_doc =  resoluciones.Where(_res => _res.NumeroResolucion.Equals(numero_resolucion)).ToList();

                        if (!resolucion_doc.Any())
                            throw new Exception("Resolucion no encontrada");*/


                        if (tipo_doc == TipoDocumento.Factura)
                            //documento_obj = Validar(documento_obj, resoluciones);
                            documento_obj = Validar(documento_obj);
                        else if (tipo_doc == TipoDocumento.NotaCredito)
                            documento_obj = ValidarNotaCredito(documento_obj);
                    }
                    catch (Exception excepcion)
                    {
                        respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la validación del documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);


                        throw excepcion; ;
                    }

                    if (documento_obj.DatosObligado.Identificacion.Equals("811021438"))
                    {
                        solo_validar = false;
                    }


                    if (!solo_validar)
                    {
                     
                        Ctl_Empresa empresa = new Ctl_Empresa();
                        TblEmpresas empresaBd = null;
                        TblEmpresas adquirienteBd = null;

                        //Validar Obligado en BD
                        try
                        {
                            //Obtiene la informacion del Obligado que se tiene en BD
                            empresaBd = empresa.Obtener(documento_obj.DatosObligado.Identificacion);

                            if (empresaBd == null)
                                throw new ApplicationException(string.Format("No se encontró Obligado {0}", documento_obj.DatosObligado.Identificacion));

                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Facturador Electrónico. Detalle: {0} ",excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
                           
                            throw excepcion;
                        }

                        //Validacion de Adquiriente y usuario
                        try
                        {

                            //Obtiene la informacion del Adquiriente que se tiene en BD
                            adquirienteBd = empresa.Obtener(documento_obj.DatosAdquiriente.Identificacion);

                            //Si no existe Adquiriente se crea en BD y se crea Usuario
                            if (adquirienteBd == null)
                            {
                                empresa = new Ctl_Empresa();
                                //Creacion del Adquiriente
                                adquirienteBd = empresa.Crear(documento_obj.DatosAdquiriente);

                                //Creacion del Usuario del Adquiriente
                                Ctl_Usuario usuario = new Ctl_Usuario();
                                TblUsuarios usuarioBd = usuario.Crear(adquirienteBd);
                            }
                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ",excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);

                            throw excepcion;
                        }

                        //guarda documento en BD
                        try
                        {

                            //Guardado del documento en BD
                            Ctl_Documento documento_tmp = new Ctl_Documento();

                            TblDocumentos documentoBd = Ctl_Documento.Convertir(respuesta, documento_obj, empresaBd, adquirienteBd, tipo_doc);

                            documento_tmp.Crear(documentoBd);
                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al guardar el documento. Detalle: {0} ",excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);

                            throw excepcion;
                        }
                     

                        // genera el xml en ubl
                        try
                        {
                            fecha_actual = Fecha.GetFecha();
                            respuesta.DescripcionProceso = "Genera información en estandar UBL.";
                            respuesta.FechaUltimoProceso = fecha_actual;
                            respuesta.IdProceso = 3;

                            if (tipo_doc == TipoDocumento.Factura)
                                documento_result = Ctl_Ubl.Generar(id_peticion, documento_obj, tipo_doc, null, pruebas);
                            else if (tipo_doc == TipoDocumento.NotaCredito)
                                documento_result = Ctl_Ubl.Generar(id_peticion, documento_obj, tipo_doc, pruebas);


                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la generación del estandar UBL del documento. Detalle: {0} ",excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

                            throw excepcion;

                        }

                        // almacena el xml en ubl
                        try
                        {
                            fecha_actual = Fecha.GetFecha();
                            respuesta.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
                            respuesta.FechaUltimoProceso = fecha_actual;
                            respuesta.IdProceso = 4;

                            // almacena el xml
                            documento_result = Ctl_Ubl.Almacenar(documento_result);
                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento UBL en XML. Detalle: {0} ",excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

                            throw excepcion;
                        }

                        // firma el xml
                        try
                        {
                            fecha_actual = Fecha.GetFecha();
                            respuesta.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
                            respuesta.FechaUltimoProceso = fecha_actual;
                            respuesta.IdProceso = 5;

                            string ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), "certificado_test.p12");
                            documento_result = Ctl_Firma.Generar(ruta_certificado, "6c 0b 07 62 62 6d a0 e2", "persona_juridica_pruebas1", EnumCertificadoras.Andes, documento_result);

                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el firmado del documento UBL en XML. Detalle: {0} ",excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

                            throw excepcion;
                        }

                        // comprime el archivo xml firmado
                        try
                        {
                            fecha_actual = Fecha.GetFecha();
                            respuesta.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
                            respuesta.FechaUltimoProceso = fecha_actual;
                            respuesta.IdProceso = 6;

                            Ctl_Compresion.Comprimir(documento_result);
                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la compresión del documento UBL en XML firmado. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

                            throw excepcion;

                        }

                        // envía el archivo zip con el xml firmado a la DIAN
                        HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
                        try
                        {
                            fecha_actual = Fecha.GetFecha();
                            respuesta.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
                            respuesta.FechaUltimoProceso = fecha_actual;
                            respuesta.IdProceso = 7;

                            acuse = Ctl_DocumentoDian.Enviar(documento_result);
                        }
                        catch (Exception excepcion)
                        {
                            respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

                            throw excepcion;

                        }

                        respuesta.Cufe = documento_result.CUFE;

                        // url pública del xml
                        string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_obj.DatosObligado.Identificacion);
                        respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

                        // se indica la respuesta de la DIAN
                        respuesta.Error = new LibreriaGlobalHGInet.Error.Error();
                        respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.OK;
                        respuesta.Error.Fecha = fecha_actual;

                        string detalle_dian = string.Empty;

                        if (acuse.ReceivedInvoice != null)
                            detalle_dian = acuse.ReceivedInvoice.Comments;


                        respuesta.Error.Mensaje = string.Format("Respuesta DIAN: {0} - Cod. {1} - {2} - {3}", acuse.ResponseDateTime, acuse.Response, acuse.Comments, detalle_dian);

                    }
                   

                }
                catch (Exception excepcion)
                {
                    // no se controla excepción
                }

                return respuesta;
            }
            throw new ArgumentException("No se recibieron datos para realizar el proceso");
        }

        /// <summary>
        /// Procesa una lista de documentos tipo Factura
        /// </summary>
        /// <param name="documentos">documentos tipo Factura</param>
        /// <returns></returns>
        public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
        {

            try
            {
                /*
                Ctl_Empresa obj = new Ctl_Empresa();

                if (obj.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().DatosObligado.Identificacion) != true)
                    throw new ApplicationException("Autenticacion invalida");*/

                // genera un id único de la plataforma
                Guid id_peticion = Guid.NewGuid();

                DateTime fecha_actual = Fecha.GetFecha();



                /*
                                // obtiene los datos de prueba del proveedor tecnológico de la DIAN
                                DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;


                                //Obtiene la resolucion de la DIAN para el Obligado enviado
                                ResolucionesFacturacion resoluciones = Ctl_Resolucion.Obtener(id_peticion, data_dian.IdSoftware, data_dian.ClaveAmbiente, documentos.FirstOrDefault().DatosObligado.Identificacion, data_dian.NitProveedor, fecha_actual);
                                Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();


                                empresa_resolucion.Crear(resoluciones, documentos.FirstOrDefault().DatosObligado.Identificacion);

                                List<Resolucion> resolucion_empresas = new List<Resolucion>();
                                Resolucion resolucion = new Resolucion();

                                foreach (var item in resoluciones.RangoFacturacion)
                                {

                                    resolucion.NumeroResolucion = item.NumeroResolucion.ToString();
                                    resolucion.Prefijo = (!string.IsNullOrEmpty(item.Prefijo)) ? item.Prefijo : "";
                                    resolucion.RangoInicial = Convert.ToInt16(item.RangoInicial);
                                    resolucion.RangoFinal = Convert.ToInt16(item.RangoFinal);
                                    resolucion.FechaResolucion = item.FechaResolucion;
                                    resolucion.FechaVigenciaInicial = item.FechaVigenciaDesde;
                                    resolucion.FechaVigenciaFinal = item.FechaVigenciaHasta;
                                    resolucion.ClaveTecnica = item.ClaveTecnica;

                                    resolucion_empresas.Add(resolucion);
                                }

                                //Valida si es Nit HGI para que agregue la Resolucion de Pruebas
                                if (documentos.FirstOrDefault().DatosObligado.Identificacion == "811021438")
                                {
                                    Ctl_EmpresaResolucion resolucion_prueba = new Ctl_EmpresaResolucion();
                                    TblEmpresasResoluciones resolucion_hgi = resolucion_prueba.Obtener(documentos.FirstOrDefault().DatosObligado.Identificacion, documentos.FirstOrDefault().NumeroResolucion);

                                    resolucion.NumeroResolucion = resolucion_hgi.StrNumResolucion;
                                    resolucion.Prefijo = (!string.IsNullOrEmpty(resolucion_hgi.StrPrefijo)) ? resolucion_hgi.StrPrefijo : "";
                                    resolucion.RangoInicial = resolucion_hgi.IntRangoInicial;
                                    resolucion.RangoFinal = resolucion_hgi.IntRangoFinal;
                                    resolucion.FechaResolucion = resolucion_hgi.DatFechaIngreso;
                                    resolucion.FechaVigenciaInicial = resolucion_hgi.DatFechaVigenciaDesde;
                                    resolucion.FechaVigenciaFinal = resolucion_hgi.DatFechaVigenciaHasta;
                                    resolucion.ClaveTecnica = resolucion_hgi.StrClaveTecnica;

                                    resolucion_empresas.Add(resolucion);
                                }

                */
                List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

                foreach (object item in documentos)
                {

                    //DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.Factura, resolucion_empresas, true, true);
                    DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.Factura, true, true);
                    respuesta.Add(respuesta_tmp);
                }

                return respuesta;

            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.Message);
            }



        }

        /// <summary>
        /// Procesa una lista de documentos tipo NotaCredito
        /// </summary>
        /// <param name="documentos">documentos tipo NotaCredito</param>
        /// <returns></returns>
        public static List<DocumentoRespuesta> Procesar(List<NotaCredito> documentos)
        {
            // genera un id único de la plataforma
            Guid id_peticion = Guid.NewGuid();

            DateTime fecha_actual = Fecha.GetFecha();

            List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

            foreach (object item in documentos)
            {

                //DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.NotaCredito, null, true, true);
                DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.NotaCredito, true, true);
                respuesta.Add(respuesta_tmp);
            }

            return respuesta;

        }

        /// <summary>
        /// Procesa una lista de documentos tipo NotaDebito
        /// </summary>
        /// <param name="documentos">documentos tipo NotaDebito</param>
        /// <returns></returns>
        public static List<DocumentoRespuesta> Procesar(List<NotaDebito> documentos)
        {
            // genera un id único de la plataforma
            Guid id_peticion = Guid.NewGuid();

            DateTime fecha_actual = Fecha.GetFecha();

            List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

            foreach (object item in documentos)
            {

                //DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.NotaDebito, null, true, true);
                DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, TipoDocumento.NotaDebito, true, true);
                respuesta.Add(respuesta_tmp);
            }

            return respuesta;

        }


        /// <summary>
        /// Validación del Objeto Factura
        /// </summary>
        /// <param name="documento">Objeto factura</param>
        /// <returns></returns>
        //public static Factura Validar(Factura documento, List<Resolucion> resoluciones)
        public static Factura Validar(Factura documento)
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
            /*
                        //Validar que no este vacio y este vigente en los terminos.
                        if (string.IsNullOrEmpty(documento.NumeroResolucion))
                            throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));
                        bool resolucion_correcta = false;
                        bool rangos = false;
                        foreach (var item in resoluciones)
                        {
                            if (item.NumeroResolucion == documento.NumeroResolucion)
                                resolucion_correcta = true;
                            if (documento.Documento > item.RangoInicial && documento.Documento < item.RangoFinal)
                                rangos = true;
                        }

                        if (documento.DatosObligado.Identificacion.Equals("811021438"))
                            //valida resolucion
                            if (resolucion_correcta == false)
                                throw new ApplicationException(string.Format("La Resolución {0} no es valida", documento.NumeroResolucion));

                        //valida número de la Factura este entre los rangos
                        if (rangos == false)
                            throw new ApplicationException(string.Format("El Número de la Factura {0} no es valida según Resolución", documento.Documento));
            */
            //Valida que la fecha este en los terminos
            if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
                throw new ApplicationException(string.Format("La fecha {0} no esta dentro los terminos.", documento.Fecha));


            if (documento.FechaVence.Date < documento.Fecha.Date)
                throw new ApplicationException(string.Format("La fecha {0} debe ser mayor o igual a la generacion del documento", documento.FechaVence));

            //Valida que no este vacio y Formato
            if (string.IsNullOrEmpty(documento.Moneda))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

            if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosObligado, "Obligado");

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

            //Valida totales del objeto
            ValidarTotales(documento, null, null, TipoDocumento.Factura);

            return documento;
        }


        /// <summary>
        /// Validación del Objeto Nota Credito
        /// </summary>
        /// <param name="documento">Objeto NotaCredito</param>
        /// <returns></returns>
        public static NotaCredito ValidarNotaCredito(NotaCredito documento)
        {
            // valida objeto recibido
            if (documento == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Nota Crédito"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.CodigoRegistro))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

            //valida que no este vacio y existencia
            if (string.IsNullOrEmpty(documento.DataKey))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

            // validar el número del documento y validez con resolucion
            if (documento.Documento < 0)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

            //Validar que no este vacio
            if (string.IsNullOrEmpty(documento.DocumentoRef))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DocumentoRef", "string"));

            //Validar que no este vacio
            if (string.IsNullOrEmpty(documento.CufeFactura))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CufeFactura", "string"));

            //Validar que no este vacia la fecha
            if (documento.Fecha == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Fecha", "DateTime"));

            //Valida que no este vacio el concepto
            if (documento.Concepto == null)
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Concepto", "string"));

            //Valida que la fecha este en los terminos
            if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
                throw new ApplicationException(string.Format("La fecha {0} no esta dentro los terminos.", documento.Fecha));

            //Valida que no este vacio y Formato
            if (string.IsNullOrEmpty(documento.Moneda))
                throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

            if (!ConfiguracionRegional.ValidarCodigoMoneda(documento.Moneda))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 4217", "Moneda", documento.Moneda));


            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosObligado, "Obligado");

            //Valida que no este vacio y este bien formado 
            ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

            //Valida totales del objeto
            ValidarTotales(null, documento, null, TipoDocumento.NotaCredito);

            return documento;
        }

        /// <summary>
        /// Validacion del objeto tercero
        /// </summary>
        /// <param name="tercero">Objeto</param>
        /// <param name="tipo">Tipo de Tercero: Adquiriente - Obligado</param>
        public static void ValidarTercero(Tercero tercero, string tipo)
        {

            if (tercero == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Tercero", tipo).Replace("de tipo", "del"));

            //valida que la identificacion no contenga contener letras y/o caracteres especiales
            Regex isnumber = new Regex("[^0-9]");
            if (!string.IsNullOrEmpty(tercero.Identificacion))
            {
                if (isnumber.IsMatch(tercero.Identificacion))
                    throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener letras y/o caracteres especiales", "Identificacion", tipo));
            }
            else
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", tipo).Replace("de tipo", "del"));

            if ((tercero.IdentificacionDv < 0) || (tercero.IdentificacionDv > 9))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "IdentificacionDv", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Ciudad))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Ciudad", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Departamento))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Departamento", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Direccion))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Direccion", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.Telefono))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Telefono", tipo).Replace("de tipo", "del"));

            Regex ismail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            if (!ismail.IsMatch(tercero.Email))
                throw new ArgumentException(string.Format("El parámetro {0} del {1} no esta bien formado", "Email", tipo));

            if (string.IsNullOrEmpty(tercero.CodigoPais))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoPais", tipo).Replace("de tipo", "del"));

            if (!ConfiguracionRegional.ValidarCodigoPais(tercero.CodigoPais))
                throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 3166-1 alfa-2 en el {2} ", "CodigoPais", tercero.CodigoPais, tipo));

            if (string.IsNullOrEmpty(tercero.RazonSocial))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

            if (string.IsNullOrEmpty(tercero.RazonSocial))
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

            if (tercero.TipoIdentificacion == 13)
            {
                if (string.IsNullOrEmpty(tercero.PrimerApellido))
                    throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerApellido", tipo).Replace("de tipo", "del"));

                if (string.IsNullOrEmpty(tercero.PrimerNombre))
                    throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerNombre", tipo).Replace("de tipo", "del"));

            }
        }

        /// <summary>
        /// Valida los totales del objeto
        /// </summary>
        /// <param name="documento_fac">Documento Factura</param>
        /// <param name="documento_nc">Documento Nota Credito</param>
        /// <param name="documento_nd">Documento Nota Debito</param>
        /// <param name="tipo_doc">Tipo de Documento enviado</param>
        public static void ValidarTotales(Factura documento_fac, NotaCredito documento_nc, NotaDebito documento_nd, TipoDocumento tipo_doc)
        {

            var documento = (dynamic)null;

            if (tipo_doc == TipoDocumento.Factura)
                documento = documento_fac;
            else if (tipo_doc == TipoDocumento.NotaCredito)
                documento = documento_nc;
            else if (tipo_doc == TipoDocumento.NotaDebito)
                documento = documento_nd;

            if (documento != null)
            {

                DocumentoDetalle retorno = ValidarDetalleDocumento(documento.DocumentoDetalles);

                Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

                //Valida el Iva del detalle sea igual al encabezado
                if (documento.ValorIva == 0)
                {
                    documento.ValorIva = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.ValorIva).Replace(",", ".")))
                {
                    if (documento.ValorIva != retorno.IvaValor)
                        throw new ApplicationException(string.Format("El valor Iva {0} del encabezado no corresponde al detalle.", documento.ValorIva));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor Iva {0} del encabezado no esta bien formado", documento.ValorIva));
                }

                //Valida el Descuento del detalle sea igual al encabezado
                if (documento.ValorDescuento == 0)
                {
                    documento.ValorDescuento = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.ValorDescuento).Replace(",", ".")))
                {
                    if (documento.ValorDescuento != retorno.DescuentoValor)
                        throw new ApplicationException(string.Format("El valor Descuento {0} del encabezado no corresponde al detalle.", documento.ValorDescuento));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor Descuento {0} del encabezado no esta bien formado", documento.ValorDescuento));
                }

                //Valida el Subtotal del detalle sea igual al encabezado
                if (documento.ValorSubtotal == 0)
                {
                    documento.ValorSubtotal = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.ValorSubtotal).Replace(",", ".")))
                {
                    if (documento.ValorSubtotal != retorno.ValorSubtotal)
                        throw new ApplicationException(string.Format("El subtotal {0} del encabezado no corresponde al detalle.", documento.ValorSubtotal));
                }
                else
                {
                    throw new ApplicationException(string.Format("El subtotal {0} del encabezado no esta bien formado", documento.ValorSubtotal));
                }

                //Valida el Impuesto al consumo del detalle sea igual al encabezado
                if (documento.ValorImpuestoConsumo == 0)
                {
                    documento.ValorImpuestoConsumo = Convert.ToDecimal(0.00M);
                }

                if (isnumber.IsMatch(Convert.ToString(documento.ValorImpuestoConsumo).Replace(",", ".")))
                {

                    if (documento.ValorImpuestoConsumo != retorno.ValorImpuestoConsumo)
                        throw new ApplicationException(string.Format("El Impuesto al Consumo {0} del encabezado no corresponde al detalle.", documento.ValorImpuestoConsumo));
                }
                else
                {
                    throw new ApplicationException(string.Format("El Impuesto al Consumo {0} del encabezado no esta bien formado", documento.ValorImpuestoConsumo));
                }

                //Valida la Retencion en la fuente del detalle sea igual al encabezado
                if (documento.ValorReteFuente == 0)
                {
                    documento.ValorReteFuente = Convert.ToDecimal(0.00M);
                }

                if (isnumber.IsMatch(Convert.ToString(documento.ValorReteFuente).Replace(",", ".")))
                {
                    if (documento.ValorReteFuente != retorno.ReteFuenteValor)
                        throw new ApplicationException(string.Format("El valor ReteFuente {0} del encabezado no corresponde al detalle.", documento.ValorReteFuente));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor ReteFuente {0} del encabezado no esta bien formado", documento.ValorReteFuente));
                }

                //Valida el ReteIca del detalle sea igual al encabezado
                if (documento.ValorReteIca == 0)
                {
                    documento.ValorReteIca = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.ValorReteIca).Replace(",", ".")))
                {
                    if (documento.ValorReteIca != retorno.ReteIcaValor)
                        throw new ApplicationException(string.Format("El valor ReteIca {0} del encabezado no corresponde al detalle.", documento.ValorReteIca));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor ReteIca {0} del encabezado no esta bien formado", documento.ValorReteIca));
                }

                //Calculo del total con los campos enviados en el objeto
                if (documento.Total == 0)
                {
                    documento.Total = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.Total).Replace(",", ".")))
                {
                    decimal total_cal = documento.ValorSubtotal + documento.ValorIva;
                    //Validacion del total del objeto con el calculado
                    if (documento.Total != total_cal)
                        throw new ApplicationException(string.Format("El valor Total {0} no es correcto.", documento.Total));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor Total {0} no esta bien formado", documento.Total));
                }

                //Suma de las retenciones del documento
                decimal retencion_doc = (documento.ValorReteFuente + documento.ValorReteIca + documento.ValorReteIva);

                //Validacion del Neto calculado con el que es enviado en el documento
                if (documento.Neto == 0)
                {
                    documento.Neto = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.Neto).Replace(",", ".")))
                {
                    if ((documento.Total - retencion_doc) != documento.Neto)
                        throw new ApplicationException(string.Format("El valor Neto {0} no es correcto.", documento.Neto));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor Neto {0} no esta bien formado", documento.Neto));
                }

                //Validacion del ReteIva calculado con el que es enviado en el documento
                if (documento.ValorReteIva == 0)
                {
                    documento.ValorReteIva = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(documento.ValorReteIva).Replace(",", ".")))
                {
                    if ((documento.Total - documento.Neto - documento.ValorReteFuente - documento.ValorReteIca) != documento.ValorReteIva)
                        throw new ApplicationException(string.Format("El valor ReteIva {0} no es correcto.", documento.ValorReteIva));
                }
                else
                {
                    throw new ApplicationException(string.Format("El valor ReteIva {0} no esta bien formado", documento.ValorReteIva));
                }

            }
        }

        /// <summary>
        /// Valida los totales enviados en el detalle
        /// </summary>
        /// <param name="documentoDetalle"></param>
        /// <returns></returns>
		public static DocumentoDetalle ValidarDetalleDocumento(List<DocumentoDetalle> documentoDetalle)
        {

            DocumentoDetalle retorno = new DocumentoDetalle();

            decimal Iva_total = 0;
            decimal Desc_total = 0;
            decimal Subtotal = 0;
            decimal Impcon = 0;
            decimal RetIca = 0;
            decimal ReteFte = 0;

            if (documentoDetalle == null || !documentoDetalle.Any())
                throw new Exception("El detalle del documento es inválido.");

            foreach (DocumentoDetalle Docdet in documentoDetalle)
            {

                //Validacion del valor unitario
                Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

                if (Docdet.ValorUnitario == 0)
                {
                    Docdet.ValorUnitario = 0.00M;
                }
                if (!isnumber.IsMatch(Convert.ToString(Docdet.ValorUnitario).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El valor Unitario {0} no esta bien formado", Docdet.ValorUnitario));

                if (Docdet.DescuentoPorcentaje > 1 || Docdet.DescuentoPorcentaje < 0)
                    throw new ApplicationException(string.Format("El porcentaje Descuento {0} no es correcto", Docdet.DescuentoValor));

                if (Docdet.DescuentoValor == 0)
                {
                    Docdet.DescuentoValor = Convert.ToDecimal(0.00M);
                }

                //Validacion del valor IVA
                if (Docdet.IvaValor == 0)
                {
                    Docdet.IvaValor = Convert.ToDecimal(0.00M);
                }
                if (!isnumber.IsMatch(Convert.ToString(Docdet.IvaValor).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El valor Iva {0} del detalle no esta bien formado", Docdet.IvaValor));

                //Validacion del Valor Subtotal
                if (Docdet.ValorSubtotal == 0)
                {
                    Docdet.ValorSubtotal = Convert.ToDecimal(0.00M);
                }
                if (isnumber.IsMatch(Convert.ToString(Docdet.ValorSubtotal).Replace(",", ".")))
                {
                    if (Docdet.ValorSubtotal != (((Docdet.ValorUnitario * Docdet.Cantidad)) - Docdet.DescuentoValor))
                        throw new ApplicationException(string.Format("El subtotal {0} del detalle no es correcto.", Docdet.ValorSubtotal));
                }
                else
                {
                    throw new ApplicationException(string.Format("El subtotal {0} del detalle no esta bien formado", Docdet.ValorSubtotal));
                }

                //Validacion del Valor del Impuesto al Consumo
                if (Docdet.ValorImpuestoConsumo == 0)
                {
                    Docdet.ValorImpuestoConsumo = 0.00M;
                }
                if (!isnumber.IsMatch(Convert.ToString(Docdet.ValorImpuestoConsumo).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El Impuesto al Consumo {0} del detalle no esta bien formado", Docdet.ValorImpuestoConsumo));


                //Validacion del Valor del ReteICA
                if (Docdet.ReteIcaValor == 0)
                {
                    Docdet.ReteIcaValor = 0.00M;
                }
                if (!isnumber.IsMatch(Convert.ToString(Docdet.ReteIcaValor).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El valor ReteIca {0} del detalle no esta bien formado", Docdet.ReteIcaValor));

                //Validacion del Valor del ReteFte
                if (Docdet.ReteFuenteValor == 0)
                {
                    Docdet.ReteFuenteValor = 0.00M;
                }
                if (!isnumber.IsMatch(Convert.ToString(Docdet.ReteFuenteValor).Replace(",", ".")))
                    throw new ApplicationException(string.Format("El valor ReteFuente  {0} del detalle no esta bien formado", Docdet.ReteFuenteValor));

                Iva_total += Docdet.IvaValor;
                Desc_total += Docdet.DescuentoValor;
                Subtotal += Docdet.ValorSubtotal;
                Impcon += Docdet.ValorImpuestoConsumo;
                RetIca += Docdet.ReteIcaValor;
                ReteFte += Docdet.ReteFuenteValor;

            }

            retorno.IvaValor = Iva_total;
            retorno.DescuentoValor = Desc_total;
            retorno.ValorSubtotal = Subtotal;
            retorno.ValorImpuestoConsumo = Impcon;
            retorno.ReteIcaValor = RetIca;
            retorno.ReteFuenteValor = ReteFte;
            return retorno;
        }



    }
}


