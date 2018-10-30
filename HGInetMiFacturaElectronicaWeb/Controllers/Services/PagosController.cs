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

        [HttpPost]
        [Route("Api/ListaEstadoPagos")]
        public IHttpActionResult ListaEstadoPagos(object ListaPagos)
        {

            try
            {    
                var jobjeto = (dynamic)ListaPagos;

                string ListaDoc = jobjeto.ListaPagos;                

                List<ObjetoConsultaPago> ConfigPago = new JavaScriptSerializer().Deserialize<List<ObjetoConsultaPago>>(ListaDoc);                                         
                foreach (var item in ConfigPago)
                {
                    //

                }
                return Ok();
            }
            catch (Exception excepcion)
            {

                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        public class ObjetoConsultaPago
        {
            public string StrIdRegistro { get; set; }
            public string StrIdSeguridadDoc { get; set; }
        }

    }
}