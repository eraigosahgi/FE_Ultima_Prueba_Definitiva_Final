using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class DatosSesionController : ApiController
    {

        public IHttpActionResult Get()
        {
            try
            {
                Sesion.ValidarSesion();

                List<TblEmpresas> datos = new List<TblEmpresas>();

                datos.Add(Sesion.DatosEmpresa);

                var retorno = datos.Select(d => new
                {
                    FechaActualizacion = d.DatFechaActualizacion,
                    FechaIngreso = d.DatFechaIngreso,
                    Adquiriente = d.IntAdquiriente,
                    Administrador = d.IntAdministrador,
                    Integrador = d.IntIntegrador,
                    Habilitacion = d.IntHabilitacion,
                    IdentificacionDV = d.IntIdentificacionDv,
                    Obligado = d.IntObligado,
                    Identificacion = d.StrIdentificacion,
                    IdSeguridad = d.StrIdSeguridad,
                    Mail = d.StrMail,
                    Observaciones = d.StrObservaciones,
                    RazonSocial = d.StrRazonSocial,
                    Serial = d.StrSerial,
                    TipoIdentificacion = d.StrTipoIdentificacion,
                    Admin = d.IntAdministrador
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        [HttpGet]
        [Route("Api/SesionDatosUsuario")]
        public IHttpActionResult SesionDatosUsuario()
        {
            try
            {
                Sesion.ValidarSesion();

                List<TblUsuarios> datos = new List<TblUsuarios>();

                datos.Add(Sesion.DatosUsuario);

                var retorno = datos.Select(d => new
                {
                    FechaActualizacion = d.DatFechaActualizacion,
                    FechaCambioClave = d.DatFechaCambioClave,
                    d.DatFechaIngreso,
                    FechaIngreso = d.IntIdEstado,
                    Apellidos = d.StrApellidos,
                    Cargo = d.StrCargo,
                    Celular = d.StrCelular,
                    IdentificacionEmpresa = d.StrEmpresa,
                    Extension = d.StrExtension,
                    IdCambioClave = d.StrIdCambioClave,
                    IdSeguridad = d.StrIdSeguridad,
                    Mail = d.StrMail,
                    Nombres = d.StrNombres,
                    Telefono = d.StrTelefono,
                    Usuario = d.StrUsuario
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


    }
}
