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
                    Habilitacion = d.IntHabilitacion,
                    IdentificacionDV = d.IntIdentificacionDv,
                    Obligado = d.IntObligado,
                    Identificacion = d.StrIdentificacion,
                    IdSeguridad = d.StrIdSeguridad,
                    Mail = d.StrMail,
                    Observaciones = d.StrObservaciones,
                    RazonSocial = d.StrRazonSocial,
                    Serial = d.StrSerial,
                    TipoIdentificacion = d.StrTipoIdentificacion
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
