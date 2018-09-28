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
        public static bool Procesar()
        {
            ///Obtener todos los documentos de todos los proveedores
            ///



            Ctl_Documento Controlador = new Ctl_Documento();

            List<TblConfiguracionInteroperabilidad> ListaProveedores = new List<TblConfiguracionInteroperabilidad>();

            //Consulto lista de proveedores con documentos pendientes para envio

            List<TblDocumentos> Doc = new List<TblDocumentos>();

            //Consulto lista de documentos
            Doc = Controlador.ObtenerDocumentosProveedores("*");

            //Agrupo por Proveedor
            List<TblDocumentos> ListadeProveedores = Doc
                .GroupBy(p => p.StrProveedorReceptor)
                .Select(g => g.First())
                .ToList();



            //Se itera lista de proveedores
            foreach (var ProveedorDoc in ListadeProveedores)
            {

                //Aqui estoy seleccionado el proveedor Emisor que debe ser HGI
                string ProveedorEmisor = ProveedorDoc.StrProveedorEmisor;

                Usuario usuario = new Usuario();

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

                    ////este es un ciclo del proveedor para ubicar los documentos por facturador
                    //////////////////////////////////////////////////////////////////////////////

                    ////Guid de seguridad del facturador electronico           
                    string Facturador = Documento.TblEmpresasFacturador.StrIdSeguridad.ToString();
                    string RutaCarpeta = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), Facturador);
                    string RutaArchivos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

                    //valido que tipo de archivo debo enviar 1. (xml-ubl) y pdf 2. Acuse (xml-ubl)
                    if (Documento.IntIdEstado == ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode())
                    {
                        archive.CreateEntryFromFile(RutaArchivos + "\\" + Path.GetFileName(Documento.StrUrlAcuseUbl), Path.GetFileName(Documento.StrUrlAcuseUbl));

                        Documentos RegDocumentoAcuse = new Documentos();
                        //Archivo pdf                                       
                        RegDocumentoAcuse.nombre = Path.GetFileName(Documento.StrUrlAcuseUbl);
                        RegDocumentoAcuse.sha256 = "sha256";
                        RegDocumentoAcuse.tipo = Enumeracion.GetEnumObjectByValue<DocumentType>(DocumentType.AcuseDeRecibo.GetHashCode()).ToString();
                        RegDocumentoAcuse.notaDeEntrega = "";
                        RegDocumentoAcuse.adjuntos = false;
                        RegDocumentoAcuse.representacionGraficas = true;
                        RegDocumentoAcuse.identificacionDestinatario = Documento.StrEmpresaAdquiriente;
                        RegDocumentoAcuse.extensiones = ExtensionPaquete;
                        LstD.Add(RegDocumentoAcuse);





                    }
                    else
                    {

                        archive.CreateEntryFromFile(RutaArchivos + "\\" + Path.GetFileName(Documento.StrUrlArchivoUbl), Path.GetFileName(Documento.StrUrlArchivoUbl));

                        archive.CreateEntryFromFile(RutaArchivos + "\\" + Path.GetFileName(Documento.StrUrlArchivoPdf), Path.GetFileName(Documento.StrUrlArchivoPdf));

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

                        LstD.Add(RegDocumentoXml);

                    }


                }

                //Asigno la lista de documentos a la lista de documentos del registro que voy a enviar
                RegistroEnvio.documentos = LstD;

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

                string RespuestaRegistro = Ctl_ClienteWebApi.Inter_Registrar(jsonListaFacturas, Token, ProveedorDoc.TblConfiguracionInteroperabilidadReceptor.StrUrlApi);
                //string RespuestaRegistro = "{\"timeStamp\":\"2018-09-28T14:19:51.9956354\",\"trackingIds\":[{\"nombreDocumento\":\"face_f0811021438003B0235A0.xml\",\"uuid\":\"34bd6e95-f7b5-46cc-ad97-aa90117e44b4\",\"codigoError\":\"201\",\"mensaje\":\"Documento encolado para procesamiento en el ZIP 811021438_860028580_0b334803-d6d2-4c8e-aa41-b2564df91bfe.zip\"},{\"nombreDocumento\":\"face_f0811021438003B0235A1.xml\",\"uuid\":\"12b0929c-f340-4f69-9158-be8426f5caf3\",\"codigoError\":\"201\",\"mensaje\":\"Documento encolado para procesamiento en el ZIP 811021438_860028580_0b334803-d6d2-4c8e-aa41-b2564df91bfe.zip\"},{\"nombreDocumento\":null,\"uuid\":null,\"codigoError\":null,\"mensaje\":null}],\"mensajeGlobal\":\"Documento 811021438_860028580_0b334803-d6d2-4c8e-aa41-b2564df91bfe.zip El zip se radicó exitosamente\"}";
                RegistroListaDocRespuesta Respuesta = JsonConvert.DeserializeObject<RegistroListaDocRespuesta>(RespuestaRegistro);

                foreach (var Detalle in Respuesta.trackingIds)
                {
                    bool Actualiza= ActualizaRespuesta(Detalle);
                }





            }


            return true;

        }

        /// <summary>
        /// Actualiza el detalle del documento en la tabla tbldocumentos
        /// </summary>
        /// <param name="Detalle">Recibe un objeto de tipo RegistroListaDetalleDocRespuesta con el detalle del documento del proveedor tecnologico</param>
        /// <returns></returns>
        public static bool ActualizaRespuesta(RegistroListaDetalleDocRespuesta Detalle)
        {

            if (Detalle != null)
            {
                if (Detalle.nombreDocumento != null)
                {
                    string Nombre = Detalle.nombreDocumento.Replace(".xml", "");

                    int Largo = Nombre.Length;
                    int LargoPrefijo = 0;
                    string Prefijo = string.Empty;

                    string Nit = Nombre.Substring(6, 10);

                    if (Largo > 26)
                    {
                        LargoPrefijo = Largo - 26;
                        Prefijo = Nombre.Substring(16, LargoPrefijo);
                    }


                    int NitFacturador = Convert.ToInt32(Nit);
                    string DocumentoHexadecimal = Nombre.Substring(16 + LargoPrefijo, 10);


                    int NumeroDocumento = int.Parse(DocumentoHexadecimal, System.Globalization.NumberStyles.HexNumber);

                    Ctl_Documento Documentos = new Ctl_Documento();

                    //TblDocumentos Doc=  Documentos.Obtener(NitFacturador.ToString(), NumeroDocumento, Prefijo);
                    TblDocumentos Doc = Documentos.Obtenerporxml(Detalle.nombreDocumento);


                    //Se debe validar si la respuesta es de un documento o de un acuse
                    //Si el documento con el que me estan respondiento, tiene StrIdInteroperabilidad y status 13, entonces actualizo el acuse
                    if (Doc.StrIdInteroperabilidad ==  Guid.Parse(Detalle.uuid) && Doc.IntIdEstado == (Int16)ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode())
                    {
                        //Actualizo Acuse
                        Doc.DatFechaActualizaEstado = Fecha.GetFecha();
                        Doc.IntIdEstado = (Int16)(ProcesoEstado.Finalizacion.GetHashCode());
                    }else
                    {
                        //Aqui actualizo el Estado del documento y el id de interoperabilidad
                        Doc.StrIdInteroperabilidad = new Guid(Detalle.uuid);
                        Doc.IntIdEstado = (Int16)(ProcesoEstado.EnvioEmailAcuse.GetHashCode());
                        Doc.DatFechaActualizaEstado = Fecha.GetFecha();
                    }



                    Documentos.Actualizar(Doc);
                    

                }
            }

            return true;
        }
        
    }
}
