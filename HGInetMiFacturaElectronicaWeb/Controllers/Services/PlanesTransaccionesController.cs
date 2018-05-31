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
        /// Obtiene la lista 
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get([FromUri]string Identificacion)
        {
            Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
            var datos = ctl_PlanesTransacciones.Obtener(Identificacion);

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
                Empresa = d.TblUsuarios.TblEmpresas.StrRazonSocial,
                Usuario = d.StrUsuario,
                Valor = d.IntValor,
                TCompra = d.IntNumTransaccCompra,
                TProcesadas = d.IntNumTransaccProcesadas,
                id = d.StrIdSeguridad,
                Fecha = d.DatFecha,
                EmpresaFacturador = d.TblEmpresas.StrRazonSocial,
                Estado = (d.BitProcesada)? "Habilitado" : "Inhabilitado",
                Observaciones = d.StrObservaciones,
                Saldo = d.IntNumTransaccCompra-d.IntNumTransaccProcesadas
            });

            return Ok(retorno);
        }

        /// <summary>
        /// Obtiene el plan por Id de Seguridad
        /// </summary>       
        /// <param name="IdSeguridad"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(System.Guid IdSeguridad)
        {
            Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
            List<TblPlanesTransacciones> datos = ctl_PlanesTransacciones.Obtener(IdSeguridad);

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
                Empresa = d.StrEmpresaUsuario,
                Usuario = d.StrUsuario,
                Valor = d.IntValor,
                TCompra = d.IntNumTransaccCompra,
                TProcesadas = d.IntNumTransaccProcesadas,
                id = d.StrIdSeguridad,
                Fecha = d.DatFecha,
                CodigoEmpresaFacturador = d.TblEmpresas.StrIdentificacion,
                EmpresaFacturador = d.TblEmpresas.StrRazonSocial,
                Tipo = d.IntTipoProceso,
                Observaciones = d.StrObservaciones,
                Estado = d.BitProcesada

            });

            return Ok(retorno);
        }

        /// <summary>
        /// Crea una transaccion ya existente en plan de transaccion
        /// </summary>
        /// <param name="IntTipoProceso"></param>
        /// <param name="StrEmpresa Sesion"></param>
        /// <param name="StrUsuario Sesion"></param>
        /// <param name="IntNumTransaccCompra"></param>
        /// <param name="IntNumTransaccProcesadas"></param>
        /// <param name="IntValor"></param>
        /// <param name="BitProcesada"></param>
        /// <param name="StrObservaciones"></param>
        /// <param name="StrEmpresaFacturador"></param>
        /// <param name="Tipo"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromUri]byte IntTipoProceso, [FromUri]string StrEmpresa, [FromUri]string StrUsuario, [FromUri]int IntNumTransaccCompra, [FromUri]int IntNumTransaccProcesadas, [FromUri] decimal IntValor, [FromUri]bool BitProcesada, [FromUri]string StrObservaciones, [FromUri]string StrEmpresaFacturador)
        {
            try
            {
                Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();

                TblPlanesTransacciones ObjPlanTransacciones = new TblPlanesTransacciones();
                ObjPlanTransacciones.IntTipoProceso = IntTipoProceso;
                ObjPlanTransacciones.StrEmpresaUsuario = StrEmpresa;
                ObjPlanTransacciones.StrUsuario = StrUsuario;
                ObjPlanTransacciones.IntNumTransaccCompra = IntNumTransaccCompra;
                ObjPlanTransacciones.IntNumTransaccProcesadas = IntNumTransaccProcesadas;
                ObjPlanTransacciones.IntValor = IntValor;
                ObjPlanTransacciones.BitProcesada = BitProcesada;
                ObjPlanTransacciones.StrObservaciones = StrObservaciones;
                ObjPlanTransacciones.StrEmpresaFacturador = StrEmpresaFacturador;

                clase_planes.Crear(ObjPlanTransacciones);

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// Crea una transaccion ya existente en plan de transaccion
        /// </summary>
        /// <param name="IntTipoProceso"></param>
        /// <param name="StrEmpresa Sesion"></param>
        /// <param name="StrUsuario Sesion"></param>
        /// <param name="IntNumTransaccCompra"></param>
        /// <param name="IntNumTransaccProcesadas"></param>
        /// <param name="IntValor"></param>
        /// <param name="BitProcesada"></param>
        /// <param name="StrObservaciones"></param>
        /// <param name="StrEmpresaFacturador"></param>
        /// <param name="Tipo"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromUri]byte IntTipoProceso, [FromUri]string StrEmpresa, [FromUri]string StrUsuario, [FromUri]int IntNumTransaccCompra, [FromUri]int IntNumTransaccProcesadas, [FromUri] decimal IntValor, [FromUri]bool BitProcesada, [FromUri]string StrObservaciones, [FromUri]string StrEmpresaFacturador, [FromUri]System.Guid StrIdSeguridad)
        {
            try
            {
                Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();

                TblPlanesTransacciones ObjPTransacciones = new TblPlanesTransacciones();
                ObjPTransacciones.IntTipoProceso = IntTipoProceso;
                ObjPTransacciones.StrEmpresaUsuario = StrEmpresa;
                ObjPTransacciones.StrUsuario = StrUsuario;
                ObjPTransacciones.IntNumTransaccCompra = IntNumTransaccCompra;
                ObjPTransacciones.IntNumTransaccProcesadas = IntNumTransaccProcesadas;
                ObjPTransacciones.IntValor = IntValor;
                ObjPTransacciones.BitProcesada = BitProcesada;
                ObjPTransacciones.StrObservaciones = StrObservaciones;
                ObjPTransacciones.StrEmpresaFacturador = StrEmpresaFacturador;
                ObjPTransacciones.StrIdSeguridad = StrIdSeguridad;

                clase_planes.Editar(ObjPTransacciones);

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
