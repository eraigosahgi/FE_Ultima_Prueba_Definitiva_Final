using HGInetMiFacturaElectonicaController.Indicadores;
using HGInetMiFacturaElectonicaController.Indicadores.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class IndicadoresController : ApiController
    {

        [HttpGet]
        [Route("Api/ReporteEstadosAcuse")]
        public IHttpActionResult ReporteEstadosAcuse(string identificacion_empresa, int tipo_empresa, bool mensual)
        {
            try
            {
                Sesion.ValidarSesion();

                List<PorcentajesResumen> datos_EstadoAcuseMensual = new List<PorcentajesResumen>();
                Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

                datos_EstadoAcuseMensual = clase_indicadores.ReporteEstadosAcuse(identificacion_empresa, tipo_empresa, mensual);

                return Ok(datos_EstadoAcuseMensual);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        [HttpGet]
        [Route("Api/ReporteDocumentosPorTipo")]
        public IHttpActionResult ReporteDocumentosPorTipo(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                Sesion.ValidarSesion();

                List<PorcentajesResumen> datos_EstadoAcuseMensual = new List<PorcentajesResumen>();
                Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

                datos_EstadoAcuseMensual = clase_indicadores.DocumentosPorTipo(identificacion_empresa, tipo_empresa);

                return Ok(datos_EstadoAcuseMensual);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        [HttpGet]
        [Route("Api/ReporteDocumentosPorEstado")]
        public IHttpActionResult ReporteDocumentosPorEstado(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                Sesion.ValidarSesion();

                List<PorcentajesResumen> datos_EstadoAcuseMensual = new List<PorcentajesResumen>();
                Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

                datos_EstadoAcuseMensual = clase_indicadores.DocumentosPorEstado(identificacion_empresa, tipo_empresa);

                return Ok(datos_EstadoAcuseMensual);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        [HttpGet]
        [Route("Api/ReporteDocumentosPorTipoAnual")]
        public IHttpActionResult ReporteDocumentosPorTipoAnual(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                Sesion.ValidarSesion();

                List<ValoresTipoDocumento> datos_EstadoAcuseMensual = new List<ValoresTipoDocumento>();
                Ctl_Indicadores clase_indicadores = new Ctl_Indicadores();

                datos_EstadoAcuseMensual = clase_indicadores.DocumentosPorTipoAnual(identificacion_empresa, tipo_empresa);

                return Ok(datos_EstadoAcuseMensual);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


    }
}
