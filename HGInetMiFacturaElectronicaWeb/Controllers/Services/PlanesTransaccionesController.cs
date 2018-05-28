using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class PlanesTransaccionesController : ApiController
    {

        /// <summary>
        /// Crea el plan de transaccion
        /// </summary>
        /// <param name="IntTipoProceso"></param>
        /// <param name="StrEmpresa"></param>
        /// <param name="StrUsuario"></param>
        /// <param name="IntNumTransaccCompra"></param>
        /// <param name="IntNumTransaccProcesadas"></param>
        /// <param name="IntValor"></param>
        /// <param name="BitProcesada"></param>
        /// <param name="StrObservaciones"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromUri]byte IntTipoProceso, [FromUri]string StrEmpresa, [FromUri]string StrUsuario, [FromUri]short IntNumTransaccCompra, [FromUri]short IntNumTransaccProcesadas, [FromUri] decimal IntValor, [FromUri]bool BitProcesada, [FromUri]string StrObservaciones)
        {
            try
            {
                Ctl_Planes clase_planes = new Ctl_Planes();

                TblPlanesTransacciones objeto_creacion = new TblPlanesTransacciones();
                objeto_creacion.IntTipoProceso = IntTipoProceso;
                //objeto_creacion.StrEmpresa = StrEmpresa;
                objeto_creacion.StrUsuario = StrUsuario;
                objeto_creacion.IntNumTransaccCompra = IntNumTransaccCompra;
                objeto_creacion.IntNumTransaccProcesadas = IntNumTransaccProcesadas;
                objeto_creacion.IntValor = IntValor;
                objeto_creacion.BitProcesada = BitProcesada;
                objeto_creacion.BitProcesada = BitProcesada;
                objeto_creacion.StrObservaciones = StrObservaciones;

                clase_planes.CrearPlan(objeto_creacion);

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
