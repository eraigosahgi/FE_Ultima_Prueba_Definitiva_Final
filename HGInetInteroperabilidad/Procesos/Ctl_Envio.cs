using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Servicios;
using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetInteroperabilidad.Procesos
{
    public class Ctl_Envio
    {

        /// <summary>
        ///  Envia documentos a proveedores via Interoperabilidad
        ///  Documentos con estatus por enviar y Acuses pendientes por notificar
        /// </summary>
        /// <param name="Doc">Lista de Documentos para enviar a proveedor(Interoperabilidad)</param>
        /// <returns></returns>
        public static List<RespuestaRegistro> Procesar(List<TblDocumentos> Doc)//(TblDocumentos Doc)
        {
            string errorGenerico = string.Empty;

            //Creo una lista de objetos de respuesta para tenerla como base de consulta
            List<RespuestaRegistro> ListaResultadoVista = new List<RespuestaRegistro>();

            // Una vez genere la respuesta del api, debo asignar cada respuesta aqui
            List<RespuestaRegistro> DocumetoRespuesta = new List<RespuestaRegistro>();

            //deserializa el json de la respuesta del proveedor y la convierte a este objeto
            RegistroListaDocRespuesta Respuesta = new RegistroListaDocRespuesta();

            ///Obtener todos los documentos de todos los proveedores
            ///

            Ctl_Documento Controlador = new Ctl_Documento();

            List<TblConfiguracionInteroperabilidad> ListaProveedores = new List<TblConfiguracionInteroperabilidad>();

            //Consulto lista de proveedores con documentos pendientes para envio

            ///List<TblDocumentos> Doc = new List<TblDocumentos>();

            //Consulto lista de documentos
            // Doc = Controlador.ObtenerDocumentosProveedores("*");

            //Agrupo por Proveedor
            List<TblDocumentos> ListadeProveedores = Doc
                .GroupBy(p => p.StrProveedorReceptor)
                .Select(g => g.First())
                .ToList();

            //Se itera lista de proveedores
            foreach (var ProveedorDoc in ListadeProveedores)
            {

                try
                {
                    Ctl_ConfiguracionInteroperabilidad ControladorPro = new Ctl_ConfiguracionInteroperabilidad();

                    TblConfiguracionInteroperabilidad Prov_Envio = ControladorPro.Obtener((ProveedorDoc.StrProveedorEmisor != Constantes.NitResolucionsinPrefijo) ? ProveedorDoc.StrProveedorEmisor : ProveedorDoc.StrProveedorReceptor);

                    //Aqui estoy seleccionado el proveedor Emisor que debe ser HGI
                    string Proveedor_Receptor = (ProveedorDoc.StrProveedorEmisor != Constantes.NitResolucionsinPrefijo) ? ProveedorDoc.StrProveedorEmisor : ProveedorDoc.StrProveedorReceptor;

                    Usuario usuario = new Usuario();

                    //var jj = Encriptar.Encriptar_SHA256("Po&on271");
                    //usuario.u = ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiUsuario;
                    //usuario.p = ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiClave;

                    usuario.u = Prov_Envio.StrHgiUsuario;
                    usuario.p = Prov_Envio.StrHgiClave;

                    //Serializo el objeto para enviarlo al cliente webapi
                    string jsonUsuario = JsonConvert.SerializeObject(usuario);

                    //Lo primero es validar si tiene tenemos usuario y password activo con el proveedor
                    //Se debe validar si tengo un tokken activo, antes de solicitar otro
                    //HttpWebResponse RToken = Ctl_ClienteWebApi.Inter_login(jsonUsuario, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);
                    HttpWebResponse RToken = Ctl_ClienteWebApi.Inter_login(jsonUsuario, Prov_Envio.StrUrlApi);

                    int code = RToken.StatusCode.GetHashCode();

                    string resp;
                    using (StreamReader reader = new StreamReader(RToken.GetResponseStream()))
                    {
                        resp = reader.ReadToEnd();
                    }

                    if (code != RespuestaInterLogin.Exitoso.GetHashCode())
                    {
                        throw new ApplicationException(Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterLogin>(code)));
                    }

                    string Token = string.Empty;

                    AutenticacionRespuesta r = JsonConvert.DeserializeObject<AutenticacionRespuesta>(resp);
                    Token = r.jwtToken;

                    //Aqui se crea archio zip por proveedor
                    //Serializar el objeto lista facturas
                    Extensiones ExtensionPaquete = new Extensiones();
                    ExtensionPaquete.nombreExt = "ext1";
                    ExtensionPaquete.valorExt = "Valor1";

                    //Creo instancia de lista de documentos para el envio
                    RegistroListaDoc RegistroEnvio = new RegistroListaDoc();

                    PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                    //Aqui busco la ubicacion de la carpeta del proveedor tecnologico, esperar por la ruta real
                    string RutaProveedor = (string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadEnvio));

                    Directorio.CrearDirectorio(RutaProveedor);

                    Guid idenvio = Guid.NewGuid();

                    //Nombre del archivo
                    string NombreArchivoComprimido = string.Format("{0}_{1}_{2}.zip", Constantes.NitResolucionsinPrefijo, Proveedor_Receptor, idenvio, ".zip");

                    //Identifico el archivo que voy a enviar al proveedor
                    RegistroEnvio.nombre = NombreArchivoComprimido;
                    RegistroEnvio.uuid = idenvio.ToString();
                    RegistroEnvio.extensiones = ExtensionPaquete;

                    //Creo el archivo Comprimido
                    ZipArchive archive = ZipFile.Open(string.Format("{0}{1}", RutaProveedor, NombreArchivoComprimido), ZipArchiveMode.Update);

                    //Creo la lista de documentos
                    List<Documentos> LstD = new List<Documentos>();

                    //Itero la lista de documentos del proveedor en el que estoy 
                    foreach (TblDocumentos Documento in Doc)
                    {
                        try
                        {
                            RespuestaRegistro ResultadoVista = new RespuestaRegistro();
                            RegistroListaDetalleDocRespuesta ResultadoMensaje = new RegistroListaDetalleDocRespuesta();

                            ResultadoVista.Documento = Documento;

                            ////este es un ciclo del proveedor para ubicar los documentos por facturador
                            //////////////////////////////////////////////////////////////////////////////                            

                            ////Guid de seguridad del facturador electronico           
                            //string Facturador = Documento.TblEmpresasFacturador.StrIdSeguridad.ToString();

                            string Guid_ProveedorReceptor = Documento.TblEmpresasFacturador.StrIdSeguridad.ToString();  //Prov_Envio.StrIdSeguridad.ToString();

                            //string RutaCarpeta = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), Facturador);
                            string RutaCarpeta = string.Format("{0}\\{1}\\{2}\\", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, Guid_ProveedorReceptor);
                            string RutaArchivos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
                            string RutaArchivosAcuse = string.Format(@"{0}\{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);
                            string RutaAnexos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEAnexos);

                            //valido que tipo de archivo debo enviar 1. (xml-ubl) y pdf 2. Acuse (xml-ubl)
                            if (Documento.IntIdEstado == ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode())
                            {

                                if (Archivo.ValidarExistencia(string.Format(@"{0}\\{1}", RutaArchivosAcuse, Path.GetFileName(Documento.StrUrlAcuseUbl))))
                                {
                                    archive.CreateEntryFromFile(string.Format(@"{0}\\{1}", RutaArchivosAcuse, Path.GetFileName(Documento.StrUrlAcuseUbl)), Path.GetFileName(Documento.StrUrlAcuseUbl));

                                    Documentos RegDocumentoAcuse = new Documentos();
                                    //Archivo pdf                                       
                                    RegDocumentoAcuse.nombre = Path.GetFileName(Documento.StrUrlAcuseUbl);

                                    //// Archivo PDF
                                    //var ArchivoUbl = string.Format(@"{0}\\{1}", RutaArchivosAcuse, Path.GetFileName(Documento.StrUrlAcuseUbl));
                                    //string archivo = Encriptar.Archivo_Sha256(string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlAcuseUbl)));

                                    RegDocumentoAcuse.sha256 = "";
                                    RegDocumentoAcuse.tipo = Enumeracion.GetEnumObjectByValue<DocumentType>(DocumentType.AcuseDeRecibo.GetHashCode()).ToString();
                                    RegDocumentoAcuse.notaDeEntrega = "";
                                    RegDocumentoAcuse.adjuntos = false;
                                    RegDocumentoAcuse.representacionGraficas = false;
                                    RegDocumentoAcuse.identificacionDestinatario = Documento.StrEmpresaFacturador;
                                    RegDocumentoAcuse.extensiones = ExtensionPaquete;
                                    //Este es el objeto json que se envia al proveedor                                        
                                    LstD.Add(RegDocumentoAcuse);

                                    //Este es el Objeto que se debe ir llenando para retornar una respuesta al usuario
                                    ResultadoVista.Documento.StrUrlArchivoUbl = RegDocumentoAcuse.nombre;
                                    ListaResultadoVista.Add(ResultadoVista);
                                }
                                else
                                {
                                    //Aqui debo llenar el objeto de respuesta ya que no lo voy a enviar al proveedor, por falta de archivo
                                    ResultadoMensaje.mensaje = "No se encuentra el archivo Acuse";
                                }
                            }
                            else

                            {
                                if (Archivo.ValidarExistencia(string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlArchivoUbl))))
                                {
                                    archive.CreateEntryFromFile(string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlArchivoUbl)), Path.GetFileName(Documento.StrUrlArchivoUbl));

                                    bool TieneAnexos = false;
                                    string anexo = Ctl_Documentos.RutaAnexos(Documento);
                                    if (!string.IsNullOrEmpty(anexo))
                                    {
                                        TieneAnexos = true;
                                        archive.CreateEntryFromFile(anexo, Path.GetFileName(Documento.StrUrlAnexo));
                                    }

                                    Documentos RegDocumentoXml = new Documentos();
                                    //Archivo xml
                                    RegDocumentoXml.nombre = Path.GetFileName(Documento.StrUrlArchivoUbl);

                                    //// ruta física del xml
                                    //var ArchivoUbl = string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlArchivoUbl));
                                    //string archivo = Encriptar.Archivo_Sha256(string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlArchivoUbl)));                                    
                                    RegDocumentoXml.sha256 = "";
                                    RegDocumentoXml.tipo = Enumeracion.GetEnumObjectByValue<DocumentType>(Documento.IntDocTipo).ToString();
                                    RegDocumentoXml.notaDeEntrega = "";
                                    RegDocumentoXml.adjuntos = TieneAnexos;
                                    RegDocumentoXml.representacionGraficas = true;
                                    RegDocumentoXml.identificacionDestinatario = Documento.StrEmpresaAdquiriente;
                                    RegDocumentoXml.extensiones = ExtensionPaquete;
                                    //Este es el objeto json que se envia al proveedor
                                    LstD.Add(RegDocumentoXml);

                                    //Este es el Objeto que se debe ir llenando para retornar una respuesta al usuario
                                    ResultadoVista.Documento.StrUrlArchivoUbl = RegDocumentoXml.nombre;
                                }
                                else
                                {
                                    //Aqui debo llenar el objeto de respuesta ya que no lo voy a enviar al proveedor, por falta de archivo
                                    ResultadoMensaje.mensaje = "No se encuentra el archivo xml";
                                }


                                if (Archivo.ValidarExistencia(string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlArchivoPdf))))
                                {
                                    archive.CreateEntryFromFile(string.Format(@"{0}\\{1}", RutaArchivos, Path.GetFileName(Documento.StrUrlArchivoPdf)), Path.GetFileName(Documento.StrUrlArchivoPdf));
                                }
                                else
                                {
                                    ResultadoMensaje.mensaje = "No se encuentra el archivo Pdf";
                                }



                            }
                            //Antes de enviar la peticion, guardo un objeto para luego llenar las respuestas del servidor
                            ResultadoVista.Respuesta = ResultadoMensaje;
                            ListaResultadoVista.Add(ResultadoVista);
                        }
                        catch (Exception excepcion)
                        {
                            errorGenerico = excepcion.Message.ToString();
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
						}

                    }

                    //Asigno la lista de documentos a la lista de documentos del registro que voy a enviar
                    RegistroEnvio.documentos = LstD;

                    if (RegistroEnvio.documentos.Count > 0)
                    {
                        //Serializo el objeto en json para hacer el envio a la webapi
                        //string jsonListaFacturas = JsonConvert.SerializeObject(RegistroEnvio, Formatting.Indented);
                        string jsonListaFacturas = JsonConvert.SerializeObject(RegistroEnvio);

                        //Cierro el archivo zip
                        archive.Dispose();

                        //string ruta_fisica = string.Format(@"{0}\{1}\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrIdSeguridad, "");
                        string ruta_fisica = string.Format(@"{0}\{1}\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, Prov_Envio.StrIdSeguridad, "");

                        //Validar Directorio para 
                        //Directorio.CrearDirectorio(ruta_fisica);
                        //Subir archivo FTP
                        //ArchivoEnviado = Clienteftp.SubirArchivoSftp(ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlFtp, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiUsuario, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiClave, string.Format("{0}{1}", RutaProveedor, NombreArchivoComprimido), NombreArchivoComprimido);                        
                        bool ArchivoEnviado = false;
                        try
                        {
                            ArchivoEnviado = Clienteftp.SubirArchivoSftp(Prov_Envio.StrUrlFtp, Prov_Envio.StrHgiUsuario, Prov_Envio.StrHgiClave, string.Format("{0}{1}", RutaProveedor, NombreArchivoComprimido), NombreArchivoComprimido);
                        }
                        catch (Exception excepcion)
                        {
                            errorGenerico = excepcion.Message.ToString();
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
							if (!ArchivoEnviado)
                            {
                                throw new ApplicationException(string.Format("Problemas con el FTP : {0}", excepcion));								
							}
                        }

                        //Aqui elimino el archivo Zip si todo esta OK
                        try
                        {
                            Archivo.Borrar(string.Format("{0}{1}", RutaProveedor, NombreArchivoComprimido));
                        }
                        catch (Exception excepcion)
                        {
                            errorGenerico = excepcion.Message.ToString();
							RegistroLog.EscribirLog(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
						}

                        //Aqui se debe hacer peticion webapi
                        //HttpWebResponse RespuestaRegistroApi = Ctl_ClienteWebApi.Inter_Registrar(jsonListaFacturas, Token, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);
                        HttpWebResponse RespuestaRegistroApi = Ctl_ClienteWebApi.Inter_Registrar(jsonListaFacturas, Token, Prov_Envio.StrUrlApi);                        
                        code = RespuestaRegistroApi.StatusCode.GetHashCode();
                        string respuesta;
                        using (StreamReader reader = new StreamReader(RespuestaRegistroApi.GetResponseStream()))
                        {
                            respuesta = reader.ReadToEnd();
                        }

                        //Recibo la respuesta y la convierto a Objeto
                        Respuesta = JsonConvert.DeserializeObject<RegistroListaDocRespuesta>(respuesta);

                        //Creo esta lista para retornaren caso en que uno de los codigos de respuesta sea uno de los adecuados
                        RegistroListaDetalleDocRespuesta RespListaCodigo = new RegistroListaDetalleDocRespuesta();
                        RespuestaRegistro RespCodigo = new RespuestaRegistro();

                        switch (code)
                        {
                            case 401://Error de autenticacion
                            case 406://No tiene convenio
                            case 500://Error interno
                            case 415:
                            case 414://Contine mas de 100                                                                
                                {
                                    TblDocumentos doc = new TblDocumentos();
                                    RespListaCodigo.mensaje = string.Format("error {0} Mensaje {1}", code, Respuesta.mensajeGlobal);
                                    RespCodigo.Respuesta = RespListaCodigo;
                                    RespCodigo.Documento = doc;
                                    RespCodigo.MensajeZip = string.Format("{0},  Objeto respuesta:{1} ", Respuesta.mensajeGlobal, respuesta);
                                    DocumetoRespuesta.Add(RespCodigo);
                                    return DocumetoRespuesta;
                                }
                        }
                        //Aqui guardo la respuesta del paquete, se debe crear una lista para tener un solo encabezado
                        ListaResultadoVista[0].MensajeZip = Respuesta.mensajeGlobal;
                        if (Respuesta.trackingIds != null)
                        {
                            foreach (var Detalle in Respuesta.trackingIds)
                            {
                                try
                                {
                                    //Aqui se lee la respuesta de cada uno de los documentos
                                    if (Detalle.nombreDocumento != null)
                                    {
                                        RespuestaRegistro Resp = ListaResultadoVista.Where(x => x.Documento.StrUrlArchivoUbl.Equals(Detalle.nombreDocumento)).FirstOrDefault();
                                        if (Resp == null)
                                            throw new ApplicationException(string.Format("El nombre del archivo de respuesta, no coincide con el nombre del archivo de la petición : Documento: {0}  Mensaje: {1}", Detalle.nombreDocumento, Detalle.mensaje));

                                        Resp.Respuesta = Detalle;
                                        Resp.MensajeZip = Respuesta.mensajeGlobal;
                                        int Tipo = 0;
                                        var RespuestaDoc = RegistroEnvio.documentos.Where(x => x.nombre.Equals(Detalle.nombreDocumento));
                                        //Si el documento no es del proveedor al que envie el documento, debo enviar correo al adquiriente
                                        if (Detalle.codigoError == RespuestaInterOperabilidad.ClienteNoEncontrado.GetHashCode().ToString())
                                        {
                                            try
                                            {
                                                //Codigo para enviar correo al adquiriente y continuar con el proceso sin interoerabilidad
                                                Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
                                                Ctl_Documento ControladorEmail = new Ctl_Documento();

                                                TblDocumentos DocumentoEmail = new TblDocumentos();

                                                DocumentoEmail = ControladorEmail.ObtenerPorIdSeguridad(Resp.Documento.StrIdSeguridad).FirstOrDefault();

                                                email.NotificacionDocumento(DocumentoEmail, Resp.Documento.TblEmpresasFacturador.StrTelefono);

                                                DocumentoEmail.IntIdEstado = (short)ProcesoEstado.EnvioEmailAcuse.GetHashCode();

                                                ControladorEmail.Actualizar(DocumentoEmail);
                                            }
                                            catch (Exception excepcion)
                                            {
                                                errorGenerico = excepcion.Message.ToString();
												RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
											}
                                        }
                                        else
                                        {
                                            //Busco el tipo de documento  ----Documento o Acuse
                                            foreach (var item in RespuestaDoc)
                                            {
                                                if (item.tipo == Enumeracion.GetEnumObjectByValue<DocumentType>(DocumentType.AcuseDeRecibo.GetHashCode()).ToString())
                                                {
                                                    Tipo = 1;
                                                }
                                                break;
                                            }
                                            //Luego Valido si en la respuesta no viene codigo o nombre.  aunque si es acuse, puede venir null el uuid
                                            if (string.IsNullOrEmpty(Detalle.nombreDocumento) || (Tipo != 1 && string.IsNullOrEmpty(Detalle.uuid)))
                                            {
                                                Resp.Respuesta.mensaje = Detalle.mensaje;
                                            }
                                            else
                                            {
                                                //Aqui actualizo
                                                TblDocumentos Actualiza = ActualizaRespuesta(Detalle, Tipo);
                                                if (Actualiza != null)
                                                {
                                                    Resp.Documento.IntIdEstado = Actualiza.IntIdEstado;
                                                }
                                            }
                                        }
                                        DocumetoRespuesta.Add(Resp);
                                    }
                                    else
                                    {
                                        //Si en algun documento no me llega el nombre, genero un error para guardarlo en el log y continuo
                                        throw new ApplicationException(string.Format("la respuesta no tiene nombre de archivo : {0}", Detalle.mensaje));										
									}
                                }
                                catch (Exception excepcion)
                                {
                                    errorGenerico = excepcion.Message.ToString();
									RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
								}
                            }
                        }
                        else
                        {
                            //No hay datos en el traking de la respuesta
                            foreach (var item in ListaResultadoVista)
                            {
                                if (string.IsNullOrEmpty(item.Respuesta.mensaje))
                                {
                                    item.Respuesta.mensaje = Respuesta.mensajeGlobal;
                                }
                            }
                            //Agrupo Documento
                            ListaResultadoVista = ListaResultadoVista
                               .GroupBy(p => p.Documento.IntNumero)
                               .Select(g => g.First())
                               .ToList();

                            return ListaResultadoVista;
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ListaResultadoVista[0].MensajeZip))
                        {
                            ListaResultadoVista[0].MensajeZip = ListaResultadoVista[0].Respuesta.mensaje;
                        }
                        return ListaResultadoVista;
                    }

                }
                catch (Exception excepcion)
                {
                    errorGenerico = excepcion.Message.ToString();
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
				}
            }

            List<RespuestaRegistro> Datos = new List<RespuestaRegistro>();

            if (DocumetoRespuesta.Count < 1)
            {
                RespuestaRegistro RespuestaVacia = new RespuestaRegistro();
                TblDocumentos doc = new TblDocumentos();
                RegistroListaDetalleDocRespuesta resp = new RegistroListaDetalleDocRespuesta();
                resp.mensaje = string.Format("{0} Revisar Log", errorGenerico);
                RespuestaVacia.Documento = doc;
                RespuestaVacia.Respuesta = resp;

                RespuestaVacia.MensajeZip = (string.IsNullOrEmpty(Respuesta.mensajeGlobal)) ? string.Format("{0} Revisar Log", errorGenerico) : Respuesta.mensajeGlobal;
                DocumetoRespuesta.Add(RespuestaVacia);
                Datos = DocumetoRespuesta;
            }
            else
            {
                Datos = ListaResultadoVista
                   .GroupBy(p => p.Documento.IntNumero)
                   .Select(g => g.First())
                   .ToList();
            }
            return Datos;
        }

        /// <summary>
        /// Actualiza el detalle del documento en la tabla tbldocumentos
        /// </summary>
        /// <param name="Detalle">Recibe un objeto de tipo RegistroListaDetalleDocRespuesta con el detalle del documento del proveedor tecnologico</param>
        /// <returns></returns>
        public static TblDocumentos ActualizaRespuesta(RegistroListaDetalleDocRespuesta Detalle, int Tipo)
        {
            TblDocumentos Doc = new TblDocumentos();
            if (Detalle != null)
            {
                if (Detalle.nombreDocumento != null)
                {
                    if (!Detalle.codigoError.Equals(RespuestaInterOperabilidad.PendienteProcesamiento.GetHashCode().ToString()))
                    {
                        return null;
                    }
                    Ctl_Documento Documentos = new Ctl_Documento();
                    //TblDocumentos Doc=  Documentos.Obtener(NitFacturador.ToString(), NumeroDocumento, Prefijo);
                    Doc = Documentos.Obtenerporxml(Detalle.nombreDocumento);
                    //Se debe validar si la respuesta es de un documento o de un acuse
                    //Si el documento con el que me estan respondiento, tiene StrIdInteroperabilidad y status 13, entonces actualizo el acuse
                    //if (Doc.StrIdInteroperabilidad ==  Guid.Parse(Detalle.uuid) && Doc.IntIdEstado == (Int16)ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode())
                    if (Tipo == 1)
                    {
                        //Actualizo Acuse
                        Doc.DatFechaActualizaEstado = Fecha.GetFecha();
                        Doc.IntIdEstado = (Int16)(ProcesoEstado.Finalizacion.GetHashCode());
                    }
                    else
                    {
                        if (Detalle.uuid != null)
                        {
                            //Aqui actualizo el Estado del documento y el id de interoperabilidad
                            Doc.StrIdInteroperabilidad = new Guid(Detalle.uuid);
                            Doc.IntIdEstado = (Int16)(ProcesoEstado.EnvioExitosoProveedor.GetHashCode());
                            Doc.DatFechaActualizaEstado = Fecha.GetFecha();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    Documentos.Actualizar(Doc);
                }
            }
            return Doc;
        }

        public static List<RespuestaAcuseProceso> ProcesarAcuses(List<TblDocumentos> Doc)
        {

            Ctl_Documento Controlador = new Ctl_Documento();

            List<TblConfiguracionInteroperabilidad> ListaProveedores = new List<TblConfiguracionInteroperabilidad>();

            List<RespuestaAcuseProceso> lista_respuesta = new List<RespuestaAcuseProceso>();

            //Agrupo por Proveedor
            List<TblDocumentos> ListadeProveedores = Doc
                .GroupBy(p => p.StrProveedorReceptor)
                .Select(g => g.First())
                .ToList();

            //Se itera lista de proveedores
            foreach (var ProveedorDoc in ListadeProveedores)
            {
                Usuario usuario = new Usuario();

                usuario.u = ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiUsuario;
                usuario.p = ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiClave;

                //Serializo el objeto para enviarlo al cliente webapi
                string jsonUsuario = JsonConvert.SerializeObject(usuario);

                //Lo primero es validar si tiene tenemos usuario y password activo con el proveedor
                //Se debe validar si tengo un tokken activo, antes de solicitar otro
                HttpWebResponse RToken = Ctl_ClienteWebApi.Inter_login(jsonUsuario, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);

                int code = RToken.StatusCode.GetHashCode();

                string resp;
                using (StreamReader reader = new StreamReader(RToken.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }
                RespuestaAcuseProceso item_respuesta = new RespuestaAcuseProceso();

                if (code != RespuestaInterLogin.Exitoso.GetHashCode())
                {
                    item_respuesta = new RespuestaAcuseProceso()
                    {
                        Numero = 0,
                        IdSeguridad = "",
                        FechaProceso = Fecha.GetFecha(),
                        DocumentoTipo = 1,
                        Error = new LibreriaGlobalHGInet.Error.Error(resp, LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, null),
                        MensajeFinal = resp,
                        Facturador = "",
                        Mensaje = resp

                    };
                    lista_respuesta.Add(item_respuesta);
                    break;
                }


                string Token;

                AutenticacionRespuesta r = JsonConvert.DeserializeObject<AutenticacionRespuesta>(resp);
                Token = r.jwtToken;
                DateTime Fechaexp = Convert.ToDateTime(r.passwordExpiration);


                foreach (TblDocumentos item in Doc)
                {
                    try
                    {
                        HttpWebResponse RespuestaEstado = Ctl_ClienteWebApi.Inter_ConsultarEstado(item.StrIdInteroperabilidad.ToString(), Token, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);

                        code = RespuestaEstado.StatusCode.GetHashCode();

                        using (StreamReader reader = new StreamReader(RespuestaEstado.GetResponseStream()))
                        {
                            resp = reader.ReadToEnd();
                        }


                        RegistroDocRespuesta obj_RespuestaEstado = JsonConvert.DeserializeObject<RegistroDocRespuesta>(resp);

                        bool acuse_respuesta = false;



                        if (code != RespuestaInterOperabilidadUUID.ConsultaExitosa.GetHashCode())
                        {
                            item_respuesta = new RespuestaAcuseProceso()
                            {
                                Numero = item.IntNumero,
                                IdSeguridad = item.StrIdInteroperabilidad.ToString(),
                                FechaProceso = Fecha.GetFecha(),
                                DocumentoTipo = (short)item.IntDocTipo,
                                Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el acuse de recibo. Detalle: {0} - Respuesta: {1}", obj_RespuestaEstado.mensajeGlobal, RespuestaEstado), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, null),
                                MensajeFinal = obj_RespuestaEstado.mensajeGlobal,
                                Facturador = item.StrEmpresaFacturador,
                                Mensaje = obj_RespuestaEstado.mensajeGlobal

                            };
                            lista_respuesta.Add(item_respuesta);

                        }
                        else
                        {
                            foreach (var historial_estado in obj_RespuestaEstado.historial)
                            {
                                if (historial_estado.nombre.Equals(ResponseCode.Aceptado.ToString()) || historial_estado.nombre.Equals(ResponseCode.Rechazado.ToString()) || historial_estado.nombre.Equals(ResponseCode.AprobadoTacito.ToString())
                                    || historial_estado.nombre.Equals(ResponseCode.Pagado.ToString()))
                                {
                                    acuse_respuesta = true;

                                    HttpWebResponse RespuestaAcuse = Ctl_ClienteWebApi.Inter_ConsultaAcuse(item.StrIdInteroperabilidad.ToString(), Token, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);
                                    code = RespuestaEstado.StatusCode.GetHashCode();
                                    using (StreamReader reader = new StreamReader(RespuestaAcuse.GetResponseStream()))
                                    {
                                        resp = reader.ReadToEnd();
                                    }
                                    if (code != RespuestaInterAcuse.AcuseExitoso.GetHashCode())
                                    {
                                        item_respuesta = new RespuestaAcuseProceso()
                                        {
                                            Numero = item.IntNumero,
                                            IdSeguridad = item.StrIdSeguridad.ToString(),
                                            FechaProceso = Fecha.GetFecha(),
                                            DocumentoTipo = (short)item.IntDocTipo,
                                            Facturador = item.StrEmpresaFacturador,
                                            Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Codigo de Respuesta {0} Error al Procesar {1} ", code, resp), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, null),
                                            Mensaje = resp,
                                            MensajeFinal = string.Format("Proceso Exitoso {0} ", resp)


                                        };
                                        lista_respuesta.Add(item_respuesta);
                                        throw new ApplicationException(string.Format("Codigo de Error {0}", code));
                                    }
                                    AcuseRespuesta obj_RespuestaAcuse = JsonConvert.DeserializeObject<AcuseRespuesta>(resp);
                                    //Convierte el mensajeGlobal a Xml
                                    byte[] bytes = Convert.FromBase64String(obj_RespuestaAcuse.mensajeGlobal);
                                    string data = Encoding.UTF8.GetString(bytes);
                                    System.Xml.XmlReader xml_reader = System.Xml.XmlReader.Create(new StringReader(data));
                                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                    doc.Load(xml_reader);

                                    //Obtiene el nombre del archivo.
                                    if (!string.IsNullOrWhiteSpace(item.StrUrlArchivoUbl))
                                    {
                                        string nombre_archivo = string.Empty;
                                        nombre_archivo = Path.GetFileNameWithoutExtension(item.StrUrlArchivoUbl);

                                        PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
                                        string ruta_acuse = string.Format(@"{0}\{1}{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, ProveedorDoc.StrIdSeguridad);
                                        ruta_acuse = string.Format(@"{0}\{1}", ruta_acuse, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);

                                        //Valida la existencia de la ruta de almacenamiento, sino existe la crea.
                                        if (!Directorio.ValidarExistenciaArchivo(ruta_acuse))
                                            Directorio.CrearDirectorio(ruta_acuse);

                                        ruta_acuse = string.Format("{0}\\{1}.xml", ruta_acuse, nombre_archivo);
                                        //Almacena el archivo en la ruta especificada
                                        doc.Save(ruta_acuse);

                                        Ctl_Empresa clase_empresa = new Ctl_Empresa();
                                        TblEmpresas datos_facturador = new TblEmpresas();

                                        datos_facturador = clase_empresa.Obtener(item.TblEmpresasFacturador.StrIdentificacion);

                                        //Serializa el objeto

                                        System.Xml.XmlReader xml_reader_serializacion = System.Xml.XmlReader.Create(new StringReader(data));
                                        XmlSerializer serializacion = null;
                                        ApplicationResponseType obj_acuse_serializado = new ApplicationResponseType();
                                        serializacion = new XmlSerializer(typeof(ApplicationResponseType));
                                        obj_acuse_serializado = (ApplicationResponseType)serializacion.Deserialize(xml_reader_serializacion);

                                        //convierte la serialización a objeto Acuse
                                        Acuse objeto_acuse = AcuseXML.Convertir(obj_acuse_serializado);

                                        try
                                        {
                                            if (objeto_acuse != null)
                                            {
                                                RegistroListaDetalleDocRespuesta respuesta = Ctl_Recepcion.ProcesarAcuse(objeto_acuse, ruta_acuse, nombre_archivo, datos_facturador);
                                                string Historia = "";
                                                int i = 0;
                                                foreach (var mensaje in obj_RespuestaEstado.historial)
                                                {
                                                    i = i + 1;
                                                    Historia += string.Format("<br/> Estado {0}: {1}", i, mensaje.mensaje);
                                                    Historia += string.Format("<br/> Fecha :{0}", Convert.ToDateTime(mensaje.timeStamp).ToString("yyyy-MM-dd HH:mm"));
                                                    Historia += "<br/>";
                                                }

                                                item_respuesta = new RespuestaAcuseProceso()
                                                {
                                                    Numero = item.IntNumero,
                                                    IdSeguridad = item.StrIdSeguridad.ToString(),
                                                    FechaProceso = Fecha.GetFecha(),
                                                    DocumentoTipo = (short)item.IntDocTipo,
                                                    Facturador = item.StrEmpresaFacturador,
                                                    Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Proceso Exitoso {0} ", respuesta.mensaje), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, null),
                                                    Mensaje = Historia,
                                                    MensajeFinal = string.Format("Proceso Exitoso {0} ", respuesta.mensaje)
                                                };
                                            }
                                        }
                                        catch (Exception excepcion)
                                        {
                                            string Historia = "";
                                            int i = 0;
                                            foreach (var mensaje in obj_RespuestaEstado.historial)
                                            {
                                                i = i + 1;
                                                Historia += string.Format("<br/> Estado {0}: {1}", i, mensaje.mensaje);
                                                Historia += string.Format("<br/> Fecha :{0}", Convert.ToDateTime(mensaje.timeStamp).ToString("yyyy-MM-dd HH:mm"));
                                                Historia += "<br/>";
                                            }
                                            item_respuesta = new RespuestaAcuseProceso()
                                            {
                                                Numero = item.IntNumero,
                                                IdSeguridad = item.StrIdSeguridad.ToString(),
                                                FechaProceso = Fecha.GetFecha(),
                                                DocumentoTipo = (short)item.IntDocTipo,
                                                Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el acuse de recibo. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                                                Mensaje = Historia,
                                                MensajeFinal = string.Format("Error al procesar el acuse de recibo. Detalle: {0} ", excepcion.Message)
                                            };
                                        }
                                    }
                                    break;
                                }
                            }

                            if (!acuse_respuesta)
                            {
                                string Historia = "";
                                int i = 0;
                                foreach (var mensaje in obj_RespuestaEstado.historial)
                                {
                                    i = i + 1;
                                    Historia += string.Format("<br/> Estado {0}: {1}", i, mensaje.mensaje);
                                    Historia += string.Format("<br/> Fecha :{0}", Convert.ToDateTime(mensaje.timeStamp).ToString("yyyy-MM-dd HH:mm"));
                                    Historia += "<br/>";
                                }

                                item_respuesta = new RespuestaAcuseProceso()
                                {
                                    Numero = item.IntNumero,
                                    IdSeguridad = item.StrIdSeguridad.ToString(),
                                    FechaProceso = Fecha.GetFecha(),
                                    DocumentoTipo = (short)item.IntDocTipo,
                                    Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el acuse de recibo. Detalle: {0} ", string.Format("El acuse no ha sido procesado.")), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, null),
                                    Mensaje = Historia,
                                    MensajeFinal = string.Format("El acuse no ha sido procesado."),
                                    Facturador = item.StrEmpresaFacturador
                                };
                            }

                            lista_respuesta.Add(item_respuesta);
                        }
                    }
                    catch (Exception excepcion)
                    {
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.actualizacion);
					}
                }
            }
            return lista_respuesta;
        }

        /// <summary>
        /// Retorna un base64 con el contenido del archivo xml acuse.
        /// </summary>
        /// <param name="uuid">Id de seguridad del Documento</param>
        /// <param name="Identifiacion">Proveedor Emisor</param>
        /// <returns></returns>
        public static MensajeGlobal ObtenerAcusebase64(Guid uuid, string Identifiacion)
        {

            Ctl_Documento Controlador = new Ctl_Documento();

            TblDocumentos Doc = Controlador.ObtenerHistorialDococumento(uuid, Identifiacion);

            PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

            // ruta física del xml
            string Ruta_Acuse = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, Doc.TblEmpresasFacturador.StrIdSeguridad.ToString());
            Ruta_Acuse = string.Format(@"{0}\{1}\{2}", Ruta_Acuse, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse, Path.GetFileName(Doc.StrUrlAcuseUbl));

            string readText = File.ReadAllText(Ruta_Acuse);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(readText);
            string archivo = System.Convert.ToBase64String(plainTextBytes);

            //Se debe enviar esta respuesta
            MensajeGlobal Respuesta = new MensajeGlobal();

            Respuesta.timeStamp = Fecha.FechaUtc(Convert.ToDateTime(Doc.DatAdquirienteFechaRecibo));
            Respuesta.mensajeGlobal = archivo;

            return Respuesta;

        }








    }




}
