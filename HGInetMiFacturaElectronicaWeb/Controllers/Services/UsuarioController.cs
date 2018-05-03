using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class UsuarioController : ApiController
    {
        /// <summary>
        /// Valida la existencia de un usuario
        /// </summary>
        /// <param name="codigo_empresa"></param>
        /// <param name="codigo_usuario"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string codigo_empresa, string codigo_usuario, string clave)
        {
            Ctl_Usuario ctl_usuario = new Ctl_Usuario();

            var datos = ctl_usuario.ValidarExistencia(codigo_empresa, codigo_usuario, clave);

            if (datos == null)
            {
                return NotFound();
            }

            return Ok(datos);
        }

    }
}
