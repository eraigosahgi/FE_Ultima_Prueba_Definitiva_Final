using HGInetMiFacturaElectonicaController.PagosElectronicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class PagosController : ApiController
    {
        /// <summary>
        /// Valida el estatus de una lista de pagos
        /// </summary>
        /// <param name="strIdSeguridad"></param>        
        /// <returns></returns>

        [HttpGet]
        [Route("Api/ListaEstadoPagos")]
        public IHttpActionResult ListaEstadoPagos(object ListaPagos)
        {

            try
            {

               

                var jobjeto = (dynamic)ListaPagos;

                string ListaDoc = jobjeto.datos;

                List<dynamic> ListadeDocumentos = new JavaScriptSerializer().Deserialize<List<dynamic>>(jobjeto.datos);

                //List<TblDocumentos> ListaDocumentos = new List<TblDocumentos>();

                //List<long> List_id_seguridad = new List<long>();

                //foreach (var item in ListadeDocumentos)
                //{
                //    List_id_seguridad.Add(item.Documentos);
                //}

                //Ctl_Documento documento = new Ctl_Documento();

                //var lista = documento.DocumentosAcuseTacito(List_id_seguridad);

                //var datos = documento.GenerarAcuseTacito(lista);

                //return Ok();

                return Ok();
            }
            catch (Exception excepcion)
            {

                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }


        }

    }
}