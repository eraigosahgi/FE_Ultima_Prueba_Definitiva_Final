
using LibreriaGlobalHGInet.Funciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Demos
{
    public partial class DemoInteroperabilidad : System.Web.UI.Page
    {       
           
        protected void Page_Load(object sender, EventArgs e)
        {

            /////Serializa el objeto de respuesta de acuse
            //AcuseRespuesta Respuesta = new AcuseRespuesta();

            //Respuesta.mensajeGlobal = "Hola";
            //Respuesta.timeStamp = Fecha.GetFecha();

            //string json = JsonConvert.SerializeObject(Respuesta, Formatting.Indented);
            /////Serializa el objeto de respuesta de acuse
            /////

            ////Serializar el objeto lista facturas
            //Extensiones De = new Extensiones();
            //De.nombreExt = "ext1";
            //De.valorExt = "Valor1";

            //List<Documentos> LstD = new List<Documentos>();

            //Documentos d1 = new Documentos();

            //d1.nombre = "face_f0900364710000000031f.xml";
            //d1.sha256 = "db74c940d447e877d119df613edd2700c4a84cd1cf08beb7cbc319bcfaeab97a";
            //d1.tipo = "FacturaNacional";
            //d1.notaDeEntrega = "Facturas de caja menor";
            //d1.adjuntos = true;
            //d1.representacionGraficas = true;
            //d1.identificacionDestinatario = "17120703";
            //d1.extensiones = De;

            //LstD.Add(d1);
            ////----------------------------------------------------------------------------------

            //d1.nombre = "face_f0900364710000000031f.xml";
            //d1.sha256 = "db74c940d447e877d119df613edd2700c4a84cd1cf08beb7cbc319bcfaeab97a";
            //d1.tipo = "FacturaNacional";
            //d1.notaDeEntrega = "Facturas de caja menor";
            //d1.adjuntos = true;
            //d1.representacionGraficas = true;
            //d1.identificacionDestinatario = "17120703";
            //d1.extensiones = De;

            //LstD.Add(d1);


            //RegistroListaDoc Ld = new RegistroListaDoc();

            //Ld.nombre = "face_f0900364710000000031f.xml";
            //Ld.uuid = "4455-dserre-r487rtr-trgr4JJd-s";
            //Ld.extensiones = De;

            //Ld.documentos = LstD;




            //string jsonListaFacturas = JsonConvert.SerializeObject(Ld, Formatting.Indented);

            /////Respuesta de la lista de documentos 6.1.4.2.1
            ///////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////
            /////Respuesta de la lista de documentos 6.1.4.2.1
            /////

            //List<RegistroListaDetalleDocRespuesta> ListRespListaDocumentos = new List<RegistroListaDetalleDocRespuesta>();
            //RegistroListaDetalleDocRespuesta RespListaDocumentos = new RegistroListaDetalleDocRespuesta();
            //RespListaDocumentos.nombreDocumento = "face_eexxsddeererOpMkGbStuHHss.xml";
            //RespListaDocumentos.uuid = "550e8400-e29b-41d4-a716-446655440000";
            //RespListaDocumentos.codigoError = "201";
            //RespListaDocumentos.mensaje = "Documento encolado para procesamiento";

            //ListRespListaDocumentos.Add(RespListaDocumentos);

            //RegistroListaDetalleDocRespuesta RespListaDocumentos2 = new RegistroListaDetalleDocRespuesta();
            //RespListaDocumentos2.nombreDocumento = "face_eexxsddeererOpMkGbStuHHss2.xml";
            //RespListaDocumentos2.uuid = "550e8400-e29b-41d4-a716-4466554400002";
            //RespListaDocumentos2.codigoError = "202";
            //RespListaDocumentos2.mensaje = "Documento encolado para procesamiento2";

            //ListRespListaDocumentos.Add(RespListaDocumentos2);



            //RegistroListaDocRespuesta registroRespuesta = new RegistroListaDocRespuesta();

            //registroRespuesta.timeStamp = Fecha.GetFecha();
            //registroRespuesta.trackingIds = ListRespListaDocumentos;
            //registroRespuesta.mensajeGlobal = "Documento electrónico radicado satisfactoriamente";


            //string jsonListaRespuestaDoc = JsonConvert.SerializeObject(registroRespuesta, Formatting.Indented);


            /////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////



            //List<DetalleDocRespuesta> ListDetalleResp = new List<DetalleDocRespuesta>();

            //DetalleDocRespuesta detalleResp = new DetalleDocRespuesta();

            //detalleResp.nombre = "Radicado";
            //detalleResp.timeStamp = Fecha.GetFecha();
            //detalleResp.mensaje = "Documento radicado en el Operador del receptor";

            //ListDetalleResp.Add(detalleResp);



            //DetalleDocRespuesta detalleResp2 = new DetalleDocRespuesta();

            //detalleResp2.nombre = "Entregado";
            //detalleResp2.timeStamp = Fecha.GetFecha();
            //detalleResp2.mensaje = "Documento Entregado en el Operador del receptor";

            //ListDetalleResp.Add(detalleResp2);



            //DetalleDocRespuesta detalleResp3 = new DetalleDocRespuesta();

            //detalleResp3.nombre = "Aceptado";
            //detalleResp3.timeStamp = Fecha.GetFecha();
            //detalleResp3.mensaje = "Documento Aceptado en el Operador del receptor";

            //ListDetalleResp.Add(detalleResp3);


            //RegistroDocRespuesta docRespuesta = new RegistroDocRespuesta();

            //docRespuesta.mensajeGlobal = "Estado actual Aceptado";
            //docRespuesta.timeStamp = Fecha.GetFecha();
            //docRespuesta.historial = ListDetalleResp;


            //string jsonRespuestaDoc = JsonConvert.SerializeObject(docRespuesta, Formatting.Indented);



        }
    }
}