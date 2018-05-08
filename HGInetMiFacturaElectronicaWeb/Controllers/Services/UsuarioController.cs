using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HGInetMiFacturaElectronicaWeb.Seguridad;

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
		[HttpGet]
		public IHttpActionResult Get(string codigo_empresa, string codigo_usuario, string clave)
        {
            Sesion sesion_web = new Sesion();

            Ctl_Usuario ctl_usuario = new Ctl_Usuario();
            List<TblUsuarios> datos = ctl_usuario.ValidarExistencia(codigo_empresa, codigo_usuario, clave);

            if (datos == null)
            {
                return NotFound();
            }

            var retorno = datos.Select(d => new
            {
               Token = d.StrIdSeguridad
            });
            
            return Ok(retorno);
        }

        /// <summary>
        /// Valida la existencia de un usuario para restablecer la Contraseña
        /// </summary>
        /// <param name="codigo_empresa"></param>
        /// <param name="codigo_usuario"></param>        
        /// <returns></returns>
		[HttpPost]
        public IHttpActionResult Post([FromUri]string codigo_empresa, [FromUri]string codigo_usuario)
        {

			Ctl_Usuario ctl_usuario = new Ctl_Usuario();

            bool datos = ctl_usuario.ValidarExistenciaRestablecer(codigo_empresa, codigo_usuario);

            if (!datos)
            {
                return NotFound();
            }

            return Ok(datos);
        }



		/// <summary>
		/// Valida si el codigo de seguridad existe y si tiene vigencia
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post([FromUri]System.Guid id_seguridad, [FromUri]string clave)
        {
            Ctl_Usuario ctl_usuario = new Ctl_Usuario();

            bool datos = ctl_usuario.RestablecerClave(id_seguridad, clave);

            if (datos)
            {
                return Ok(datos);
            }
            return NotFound();
        }

		/// <summary>
		/// Valida si el codigo de seguridad existe y si tiene vigencia
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(System.Guid id_seguridad)
        {
            Ctl_Usuario ctl_usuario = new Ctl_Usuario();

            bool datos = ctl_usuario.ValidarIdSeguridad(id_seguridad);

            if (datos)
            {
                return Ok(datos);
            }
            return NotFound();
        }

    }
}
