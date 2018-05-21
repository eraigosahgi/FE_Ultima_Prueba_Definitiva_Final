using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class EmpresasController : ApiController
    {

        /// <summary>
        /// Obtiene la lista 
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {

            Ctl_Empresa ctl_empresa = new Ctl_Empresa();
            List<TblEmpresas> datos = ctl_empresa.ObtenerTodas();

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
                Identificacion = d.StrIdentificacion,
                RazonSocial = d.StrRazonSocial,
                Email = d.StrMail,
                Serial = d.StrSerial,
                Perfil = d.IntAdquiriente ? "Adquiriente" : "Facturador",
                Habilitacion = d.IntHabilitacion,
                IdSeguridad = d.StrIdSeguridad
            });

            return Ok(retorno);
        }

        /// <summary>
        /// Obtiene la lista 
        /// </summary>
        /// /// <param name="IdSeguridad">id de seguridad</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(System.Guid IdSeguridad)
        {

            Ctl_Empresa ctl_empresa = new Ctl_Empresa();
            List<TblEmpresas> datos = ctl_empresa.Obtener(IdSeguridad);

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
                TipoIdentificacion = d.StrTipoIdentificacion,
                Identificacion = d.StrIdentificacion,
                RazonSocial = d.StrRazonSocial,
                Email = d.StrMail,
                Serial = d.StrSerial,
                Perfil = d.IntAdquiriente,
                Habilitacion = d.IntHabilitacion,
                IdSeguridad = d.StrIdSeguridad,
                Intadquiriente = d.IntAdquiriente,
                intObligado = d.IntObligado,
                IntIdentificacionDv = d.IntIdentificacionDv,
                StrResolucionDian = d.StrResolucionDian
            });

            return Ok(retorno);
        }

        /// <summary>
        /// Crear una nueva Empresa
        /// </summary>
        /// <param name="codigo_empresa"></param>
        /// <param name="codigo_usuario"></param>        
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromUri] string TipoIdentificacion, [FromUri]string Identificacion, [FromUri]string RazonSocial, [FromUri]string Email, [FromUri]bool Intadquiriente, [FromUri]bool IntObligado, [FromUri]Byte IntHabilitacion, [FromUri] string StrEmpresaAsociada, [FromUri]int tipo)//1.- Nuevo -- 2.- Editar
        {
            Ctl_Empresa ctl_empresa = new Ctl_Empresa();
            TblEmpresas Empresa = new TblEmpresas();
            try
            {
                if (tipo == 1)//Nuevo
                {
                    Empresa.StrTipoIdentificacion = TipoIdentificacion;
                    Empresa.StrIdentificacion = Identificacion;
                    Empresa.StrRazonSocial = RazonSocial;
                    Empresa.StrMail = Email;
                    Empresa.IntAdquiriente = Intadquiriente;
                    Empresa.IntHabilitacion = IntHabilitacion;
                    Empresa.IntObligado = IntObligado;
                    Empresa.StrEmpresaAsociada = StrEmpresaAsociada;

                    var datos = ctl_empresa.Guardar(Empresa);
                }

                if (tipo == 2)//Editar
                {
                    Empresa.StrIdentificacion = Identificacion;
                    Empresa.StrRazonSocial = RazonSocial;
                    Empresa.StrMail = Email;
                    Empresa.IntAdquiriente = Intadquiriente;
                    Empresa.IntHabilitacion = IntHabilitacion;
                    Empresa.IntObligado = IntObligado;
                    Empresa.StrEmpresaAsociada = StrEmpresaAsociada;

                    var datos = ctl_empresa.Editar(Empresa);
                }

                return Ok();
            }
            catch (Exception excepcion)
            {

                throw new ApplicationException(excepcion.Message, excepcion.InnerException);

            }
        }


    }

}
