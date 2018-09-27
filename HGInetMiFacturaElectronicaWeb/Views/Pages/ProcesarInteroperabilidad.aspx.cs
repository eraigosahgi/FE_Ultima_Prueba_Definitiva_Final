using HGInetMiFacturaElectonicaController.Procesos;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class ProcesarInteroperabilidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {         

            //Obtener guid del proveedor
            string Proveedor = "1234b29a-65db-4c33-8943-587edc98c178";

            //Aqui busco la unicacion de la carpeta del proveedor tecnologico
            string RutaProveedor =  LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), Proveedor);

            //Carpeta donde vamos a preparar el Zip
            string RutaOrganizar = "Z_AComprimir\\";            

            //Ruta Ftp Proveedor
            string RutaFtp = "ftp://ftpprueba@habilitacion.mifacturaenlinea.com.co//";

            //Usuario Ftp
            string UsuarioFtp = "ftpprueba";

            //Password ftp
            string PasswordFtp = "Hgi123";

            //Nombre del archivo
            string NombreArchivoComprimido = "NitProveedorTecnologicoEmisor_NitProveedorTecnologicoReceptor_Guid.zip";


            //este es un ciclo del proveedor para ubicar los documentos por facturador
            ////////////////////////////////////////////////////////////////////////////

            //Guid de seguridad del facturador electronico           
            string Facturador = "eb821fbe-02ba-4cfc-a7a9-248711513591";
            string RutaCarpeta = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), Facturador);
            string RutaArchivos = string.Format(@"{0}{1}", RutaCarpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

            
            //Aqui se debe llamar al metodo que obtiene la lista de documentos a enviar a un facturador especifico
            List<ArchivosAcomprimir> Lista = new List<ArchivosAcomprimir>();
            ArchivosAcomprimir arc1 = new ArchivosAcomprimir();
            arc1.Nombre = "face_c081102143800000009EB.pdf";

            Lista.Add(arc1);

            ArchivosAcomprimir arc2 = new ArchivosAcomprimir();
            arc2.Nombre = "face_f0811021438003B0235A0.pdf";
            Lista.Add(arc2);

            ArchivosAcomprimir arc3 = new ArchivosAcomprimir();
            arc3.Nombre = "face_f0811021438003B0235A1.pdf";

            Lista.Add(arc3);

            //Aqui se cierra el ciclo de los facturadores y se debe enviar la lista de documentos que se deben comprimir para enviar



           


            //Compresion de la lista de archivos
            var ruta = Ctl_Compresion.ComprimirLista(RutaProveedor + RutaOrganizar + NombreArchivoComprimido, RutaArchivos , NombreArchivoComprimido);
        
            Clienteftp.SubirArchivoFTP(RutaFtp+ NombreArchivoComprimido, UsuarioFtp, PasswordFtp, RutaProveedor + RutaOrganizar + NombreArchivoComprimido);

            //Aqui elimino el archivo Zip si todo esta OK
            //Archivo.Borrar(RutaProveedor + RutaOrganizar + NombreArchivoComprimido);



            //Aqui se debe hacer peticion webapi

            //string RespuestaRegistro = Inter_Registrar();









        }


        public class ArchivosAcomprimir
        {
            public string Nombre { get; set; }
        }



        //DescargarFtp("ftp://ftpprueba@habilitacion.mifacturaenlinea.com.co/Doc.zip", "ftpprueba", "Hgi123", "C:\\Users\\jflores.HGI\\Downloads\\Proveedores\\Proveedor1\\Doc.zip");

    }
}