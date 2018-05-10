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
using HGInetMiFacturaElectonicaController;

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
        /// Obtiene los usuarios por codigo de usuario y empresa.
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string codigo_usuario, string codigo_empresa)
        {
            try
            {
                Ctl_Usuario ctl_usuario = new Ctl_Usuario();
                //ObtenerUsuarios
                List<TblUsuarios> datos = ctl_usuario.ObtenerUsuarios(codigo_usuario, codigo_empresa);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    FechaActualizacion = d.DatFechaActualizacion,
                    FechaCambioClave = d.DatFechaCambioClave,
                    FechaIngreso = d.DatFechaIngreso,
                    Estado = d.IntIdEstado,
                    Apellidos = d.StrApellidos,
                    Cargo = d.StrCargo,
                    Celular = d.StrCelular,
                    Clave = d.StrClave,
                    Empresa = d.StrEmpresa,
                    Extension = d.StrExtension,
                    IdCambioClave = d.StrIdCambioClave,
                    IdSeguridad = d.StrIdSeguridad,
                    Mail = d.StrMail,
                    Nombres = d.StrNombres,
                    NombreCompleto = string.Format("{0} {1}", d.StrNombres, d.StrApellidos),
                    Telefono = d.StrTelefono,
                    Usuario = d.StrUsuario,
                    RazonSocial = d.TblEmpresas.StrRazonSocial
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        public IHttpActionResult Get(string codigo_usuario)
        {
            try
            {
                Ctl_Usuario ctl_usuario = new Ctl_Usuario();
                TblUsuarios datos_usuario = ctl_usuario.ObtenerUsuarios(codigo_usuario, "*").FirstOrDefault();

                Ctl_Empresa ctl_empresa = new Ctl_Empresa();
                TblEmpresas datos_empresa = ctl_empresa.Obtener(datos_usuario.StrEmpresa);

                if (datos_usuario == null && datos_empresa == null)
                {
                    return NotFound();
                }

                Ctl_EnvioCorreos clase_email = new Ctl_EnvioCorreos();
                bool respuesta = clase_email.Bienvenida(datos_empresa, datos_usuario);

                return Ok(respuesta);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
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
