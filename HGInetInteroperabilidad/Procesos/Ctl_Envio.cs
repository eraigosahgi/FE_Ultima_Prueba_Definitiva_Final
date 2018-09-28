using HGInetInteroperabilidad.Objetos;
using HGInetInteroperabilidad.Servicios;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
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

            string ProveedorEmisor = "811021438";


            Ctl_ConfiguracionInteroperabilidad Controlador = new Ctl_ConfiguracionInteroperabilidad();
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

                Usuario usuario = new Usuario();

                usuario.username = ProveedorDoc.TblConfiguracionInteroperabilidadRecepctor.StrHgiUsuario;
                usuario.password = ProveedorDoc.TblConfiguracionInteroperabilidadRecepctor.StrHgiClave;

                //Serializo el objeto para enviarlo al cliente webapi
                string jsonUsuario = JsonConvert.SerializeObject(usuario);



                //Lo primero es validar si tiene tenemos usuario y password activo con el proveedor
                //Se debe validar si tengo un tokken activo, antes de solicitar otro
                string Token = Ctl_ClienteWebApi.Inter_login(jsonUsuario, ProveedorDoc.TblConfiguracionInteroperabilidadRecepctor.StrUrlApi);

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
                string RutaProveedor = (string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica , Constantes.RutaInteroperabilidadEnvio));
                             
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
                        RegDocumentoAcuse.tipo = Enumeracion.GetEnumObjectByValue<DocumentType>(Documento.IntDocTipo).ToString();
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

                string ruta_fisica = string.Format(@"{0}\{1}\{2}", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadFtp, ProveedorDoc.StrIdSeguridad, "");

                Directorio.CrearDirectorio(ruta_fisica);

                Archivo.CopiarArchivo(string.Format("{0}\\{1}", RutaProveedor, NombreArchivoComprimido),string.Format("{0}\\{1}", ruta_fisica, NombreArchivoComprimido));


                //Aqui elimino el archivo Zip si todo esta OK
                //Archivo.Borrar(RutaProveedor + RutaOrganizar + NombreArchivoComprimido);

                //Aqui se debe hacer peticion webapi

                string RespuestaRegistro = Ctl_ClienteWebApi.Inter_Registrar(jsonListaFacturas, Token, ProveedorDoc.TblConfiguracionInteroperabilidadRecepctor.StrUrlApi);


            }


            return true;

        }
    }
}
