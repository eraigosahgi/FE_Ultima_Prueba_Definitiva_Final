using HGInetInteroperabilidad.Objetos;
using LibreriaGlobalHGInet.Funciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Configuracion
{
    public class Ctl_int_Consulta
    {
        public RegistroDocRespuesta ConsultarUUID(string IdentificacionProveedor, string UUID)
        {

            List<DetalleDocRespuesta> ListDetalleResp = new List<DetalleDocRespuesta>();

            DetalleDocRespuesta detalleResp = new DetalleDocRespuesta();

            detalleResp.nombre = "Radicado";
            detalleResp.timeStamp = Fecha.GetFecha();
            detalleResp.mensaje = "Documento radicado en el Operador del receptor";

            ListDetalleResp.Add(detalleResp);



            DetalleDocRespuesta detalleResp2 = new DetalleDocRespuesta();

            detalleResp2.nombre = "Entregado";
            detalleResp2.timeStamp = Fecha.GetFecha();
            detalleResp2.mensaje = "Documento Entregado en el Operador del receptor";

            ListDetalleResp.Add(detalleResp2);



            DetalleDocRespuesta detalleResp3 = new DetalleDocRespuesta();

            detalleResp3.nombre = "Aceptado";
            detalleResp3.timeStamp = Fecha.GetFecha();
            detalleResp3.mensaje = "Documento Aceptado en el Operador del receptor";

            ListDetalleResp.Add(detalleResp3);


            RegistroDocRespuesta docRespuesta = new RegistroDocRespuesta();

            docRespuesta.mensajeGlobal = "Estado actual Aceptado";
            docRespuesta.timeStamp = Fecha.GetFecha();
            docRespuesta.historial = ListDetalleResp;



            



            return docRespuesta;
            
        }
    }
}
