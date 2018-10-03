using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Servicios;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



            //Creo una lista de objetos de respuesta para tenerla como base de consulta
            List<RespuestaRegistro> ListaResultadoVista = new List<RespuestaRegistro>();

            // Una vez genere la respuesta del api, debo asignar cada respuesta aqui
            List<RespuestaRegistro> DocumetoRespuesta = new List<RespuestaRegistro>();


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
                    //Aqui estoy seleccionado el proveedor Emisor que debe ser HGI
                    string ProveedorEmisor = ProveedorDoc.StrProveedorEmisor;

                    Usuario usuario = new Usuario();



                    //var jj = Encriptar.Encriptar_SHA256("123");

                    usuario.username = ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiUsuario;
                    usuario.password = ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrHgiClave;

                    //Serializo el objeto para enviarlo al cliente webapi
                    string jsonUsuario = JsonConvert.SerializeObject(usuario);



                    //Lo primero es validar si tiene tenemos usuario y password activo con el proveedor
                    //Se debe validar si tengo un tokken activo, antes de solicitar otro
                    string Token = Ctl_ClienteWebApi.Inter_login(jsonUsuario, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);                                        

                    AutenticacionRespuesta r = JsonConvert.DeserializeObject<AutenticacionRespuesta>(Token);
                    Token = r.jwtToken;


                    //Aqui se crea archio zip por proveedor
                    //Serializar el objeto lista facturas
                    Extensiones ExtensionPaquete = new Extensiones();
                    ExtensionPaquete.nombreExt = "ext1";
                    ExtensionPaquete.valorExt = "Valor1";

                    //Creo instancia de lista de documentos para el envio
                    RegistroListaDoc RegistroEnvio = new RegistroListaDoc();


                    PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

                    //Aqui busco la unicacion de la carpeta del proveedor tecnologico, esperar por la ruta real
                    string RutaProveedor = (string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadEnvio));

                    Directorio.CrearDirectorio(RutaProveedor);



                    Guid idenvio = Guid.NewGuid();

                    //Nombre del archivo
                    string NombreArchivoComprimido = ProveedorEmisor + "_" + ProveedorDoc.StrProveedorReceptor + "_" + idenvio + ".zip";

                    //Identifico el archivo que voy a enviar al proveedor
                    RegistroEnvio.nombre = NombreArchivoComprimido;
                    RegistroEnvio.uuid = idenvio.ToString();
                    RegistroEnvio.extensiones = ExtensionPaquete;


                    //Creo el archivo Comprimido
                    ZipArchive archive = ZipFile.Open(RutaProveedor + NombreArchivoComprimido, ZipArchiveMode.Update);

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
                            string Facturador = Documento.TblEmpresasFacturador.StrIdSeguridad.ToString();
                            string RutaCarpeta = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), Facturador);
                            string RutaArchivos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
                            string RutaArchivosAcuse = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlAcuse);

                            //valido que tipo de archivo debo enviar 1. (xml-ubl) y pdf 2. Acuse (xml-ubl)
                            if (Documento.IntIdEstado == ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode())
                            {

                                if (Archivo.ValidarExistencia(string.Format(@"{0}\\{1}", RutaArchivosAcuse, Path.GetFileName(Documento.StrUrlAcuseUbl))))
                                {
                                    archive.CreateEntryFromFile(string.Format(@"{0}\\{1}", RutaArchivosAcuse, Path.GetFileName(Documento.StrUrlAcuseUbl)), Path.GetFileName(Documento.StrUrlAcuseUbl));

                                    Documentos RegDocumentoAcuse = new Documentos();
                                    //Archivo pdf                                       
                                    RegDocumentoAcuse.nombre = Path.GetFileName(Documento.StrUrlAcuseUbl);
                                    RegDocumentoAcuse.sha256 = "sha256";
                                    RegDocumentoAcuse.tipo = Enumeracion.GetEnumObjectByValue<DocumentType>(DocumentType.AcuseDeRecibo.GetHashCode()).ToString();
                                    RegDocumentoAcuse.notaDeEntrega = "";
                                    RegDocumentoAcuse.adjuntos = false;
                                    RegDocumentoAcuse.representacionGraficas = false;
                                    RegDocumentoAcuse.identificacionDestinatario = Documento.StrEmpresaAdquiriente;
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

                                    Documentos RegDocumentoXml = new Documentos();
                                    //Archivo xml
                                    RegDocumentoXml.nombre = Path.GetFileName(Documento.StrUrlArchivoUbl);
                                    RegDocumentoXml.sha256 = "sha256";
                                    RegDocumentoXml.tipo = Enumeracion.GetEnumObjectByValue<DocumentType>(Documento.IntDocTipo).ToString();
                                    RegDocumentoXml.notaDeEntrega = "";
                                    RegDocumentoXml.adjuntos = false;
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
                                }else
                                {
                                    ResultadoMensaje.mensaje = "No se encuentra el archivo Pdf";
                                }
                                

                                

                            }

                           
                            ResultadoVista.Respuesta = ResultadoMensaje;
                            ListaResultadoVista.Add(ResultadoVista);




                        }
                        catch (Exception excepcion)
                        {

                            LogExcepcion.Guardar(excepcion);
                        }

                    }

                    //Asigno la lista de documentos a la lista de documentos del registro que voy a enviar
                    RegistroEnvio.documentos = LstD;

                    if (RegistroEnvio.documentos.Count > 0)
                    {
                        //Serializo el objeto en json para hacer el envio a la webapi
                        string jsonListaFacturas = JsonConvert.SerializeObject(RegistroEnvio, Formatting.Indented);

                        //Cierro el archivo zip
                        archive.Dispose();


                        // Clienteftp.SubirArchivoFTP(ProveedorDoc.StrUrlFtp + NombreArchivoComprimido, ProveedorDoc.StrHgiUsuario, ProveedorDoc.StrClave, RutaProveedor + RutaOrganizar + NombreArchivoComprimido);

                        string ruta_fisica = string.Format(@"{0}\{1}\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrIdSeguridad, "");

                        Directorio.CrearDirectorio(ruta_fisica);

                        Archivo.CopiarArchivo(string.Format("{0}\\{1}", RutaProveedor, NombreArchivoComprimido), string.Format("{0}\\{1}", ruta_fisica, NombreArchivoComprimido));


                        //Aqui elimino el archivo Zip si todo esta OK
                        //Archivo.Borrar(RutaProveedor + RutaOrganizar + NombreArchivoComprimido);

                        //Aqui se debe hacer peticion webapi

                        string RespuestaRegistroApi = Ctl_ClienteWebApi.Inter_Registrar(jsonListaFacturas, Token, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);

                        //string RespuestaRegistroApi = "{\"timeStamp\":\"2018-10-02T14:49:32.5179038\",\"trackingIds\":[{\"nombreDocumento\":\"face_f0811021438FV003A699D68.xml\",\"uuid\":\"a38031fc-b5e4-4954-93ef-08a25e2ba6fa\",\"codigoError\":\"201\",\"mensaje\":\"Documento encolado para procesamiento en el ZIP 811021438_17120703_b301ace9-690d-4373-bc6f-639739501154.zip\"}],\"mensajeGlobal\":\"Documento 811021438_17120703_b301ace9-690d-4373-bc6f-639739501154.zip El zip se radicó exitosamente\"}";
                        RegistroListaDocRespuesta Respuesta = JsonConvert.DeserializeObject<RegistroListaDocRespuesta>(RespuestaRegistroApi);


                        if (Respuesta.trackingIds != null)
                        {

                            foreach (var Detalle in Respuesta.trackingIds)
                            {
                                //Aqui se lee la respuesta de cada uno de los documentos

                                if (Detalle.nombreDocumento != null)
                                {
                                    RespuestaRegistro Resp = ListaResultadoVista.Where(x => x.Documento.StrUrlArchivoUbl.Equals(Detalle.nombreDocumento)).FirstOrDefault();

                                    if (Resp ==null)
                                        throw new ApplicationException("La nombre del archivo de respuesta, no coincide con el nombre del archivo de la petición");

                                    Resp.Respuesta = Detalle;
                                   

                                    int Tipo = 0;
                                    var RespuestaDoc = RegistroEnvio.documentos.Where(x => x.nombre.Equals(Detalle.nombreDocumento));

                                    //RegistroListaDoc RespuestaDoc1 = RegistroEnvio.documentos.Where(x => x.nombre.Equals(Detalle.nombreDocumento));
                                    foreach (var item in RespuestaDoc)
                                    {

                                        if (item.tipo == Enumeracion.GetEnumObjectByValue<DocumentType>(DocumentType.AcuseDeRecibo.GetHashCode()).ToString())
                                        {
                                            Tipo = 1;
                                        }

                                        break;
                                    }

                                    if (string.IsNullOrEmpty(Detalle.nombreDocumento)  || string.IsNullOrEmpty(Detalle.uuid))
                                    {
                                        //try
                                        //{
                                        //    throw new ApplicationException(string.Format("La respuesta no genero ningún uuid o nombre: ", Detalle.nombreDocumento));
                                        //}
                                        //catch (Exception excepcion)
                                        //{

                                        //    LogExcepcion.Guardar(excepcion);
                                            Resp.Respuesta.mensaje = Detalle.mensaje;
                                        //}
                                    }
                                    else
                                    {
                                        bool Actualiza = ActualizaRespuesta(Detalle, Tipo);
                                        if (Actualiza)
                                        {
                                            if (Tipo == 1)
                                            {
                                                Resp.Documento.IntIdEstado = (Int16)(ProcesoEstado.Finalizacion.GetHashCode());
                                            }
                                            else
                                            {
                                                Resp.Documento.IntIdEstado = (Int16)(ProcesoEstado.EnvioExitosoProveedor.GetHashCode());
                                            }
                                        }
                                    }

                                    DocumetoRespuesta.Add(Resp);

                                }else
                                {
                                    ////Llego null la respuesta del objeto de documentos
                                    ////----------------------------------
                                    ListaResultadoVista[ListaResultadoVista.Count - 1].Respuesta.mensaje = "El proveedor no genero ningún resultado";

                                   // return ListaResultadoVista;
                                }
                            }
                            
                        }
                        
                    }
                    else
                    {
                        return ListaResultadoVista;
                    }

                }
                catch (Exception excepcion)
                {

                    LogExcepcion.Guardar(excepcion);
                }




            }

          
            return  DocumetoRespuesta;


        }

        /// <summary>
        /// Actualiza el detalle del documento en la tabla tbldocumentos
        /// </summary>
        /// <param name="Detalle">Recibe un objeto de tipo RegistroListaDetalleDocRespuesta con el detalle del documento del proveedor tecnologico</param>
        /// <returns></returns>
        public static bool ActualizaRespuesta(RegistroListaDetalleDocRespuesta Detalle, int Tipo)
        {

            if (Detalle != null)
            {
                if (Detalle.nombreDocumento != null && Detalle.uuid != null)
                {
                    //string Nombre = Detalle.nombreDocumento.Replace(".xml", "");

                    //int Largo = Nombre.Length;
                    //int LargoPrefijo = 0;
                    //string Prefijo = string.Empty;

                    //string Nit = Nombre.Substring(6, 10);

                    //if (Largo > 26)
                    //{
                    //    LargoPrefijo = Largo - 26;
                    //    Prefijo = Nombre.Substring(16, LargoPrefijo);
                    //}


                    //int NitFacturador = Convert.ToInt32(Nit);
                    //string DocumentoHexadecimal = Nombre.Substring(16 + LargoPrefijo, 10);


                    //int NumeroDocumento = int.Parse(DocumentoHexadecimal, System.Globalization.NumberStyles.HexNumber);

                    Ctl_Documento Documentos = new Ctl_Documento();

                    //TblDocumentos Doc=  Documentos.Obtener(NitFacturador.ToString(), NumeroDocumento, Prefijo);
                    TblDocumentos Doc = Documentos.Obtenerporxml(Detalle.nombreDocumento);


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
                        //Aqui actualizo el Estado del documento y el id de interoperabilidad
                        Doc.StrIdInteroperabilidad = new Guid(Detalle.uuid);
                        Doc.IntIdEstado = (Int16)(ProcesoEstado.EnvioExitosoProveedor.GetHashCode());
                        Doc.DatFechaActualizaEstado = Fecha.GetFecha();
                    }

                    Documentos.Actualizar(Doc);
                }
            }

            return true;
        }

    }
}
