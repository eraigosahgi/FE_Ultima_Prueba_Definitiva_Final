using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class PermisosController : ApiController
    {
        /// <summary>
        /// Obtiene los permisos del usuario autenticado
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <param name="identificacion_empresa"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string codigo_usuario, string codigo_empresa)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_OpcionesUsuario clase_opc_usuario = new Ctl_OpcionesUsuario();

                List<TblOpcionesUsuario> datos = clase_opc_usuario.ObtenerOpcionesUsuarios(codigo_usuario, codigo_empresa, "*", false);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    Codigo = d.IntIdOpcion,
                    Descripcion = d.TblOpciones.StrDescripcion,
                    Dependencia = d.TblOpciones.IntIdDependencia,
                    Consultar = d.IntConsultar,
                    Agregar = d.IntAgregar,
                    Editar = d.IntEditar,
                    Eliminar = d.IntEliminar,
                    Anular = d.IntAnular,
                    Gestion = d.IntGestion
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene los permisos del usuario autenticado y el usuario que se  está gestionando (creando - actualizando)
        /// </summary>
        /// <param name="usuario_autenticado"></param>
        /// <param name="empresa_autenticada"></param>
        /// <param name="codigo_usuario"></param>
        /// <param name="identificacion_empresa"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string usuario_autenticado, string empresa_autenticada, string codigo_usuario, string identificacion_empresa)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_OpcionesUsuario clase_opc_usuario = new Ctl_OpcionesUsuario();

                List<TblOpcionesUsuario> datos = clase_opc_usuario.ObtenerOpcionesUsuarios(usuario_autenticado, empresa_autenticada, codigo_usuario, identificacion_empresa);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    Codigo = d.IntIdOpcion,
                    Descripcion = d.TblOpciones.StrDescripcion,
                    Dependencia = d.TblOpciones.IntIdDependencia,
                    Consultar = d.IntConsultar,
                    Agregar = d.IntAgregar,
                    Editar = d.IntEditar,
                    Eliminar = d.IntEliminar,
                    Anular = d.IntAnular,
                    Gestion = d.IntGestion
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el permiso por código
        /// </summary>
        /// <param name="codigo_opcion"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int codigo_opcion)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_Permisos clase_permisos = new Ctl_Permisos();

                List<TblOpciones> datos = clase_permisos.ObtenerDependencias(codigo_opcion);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    Codigo = d.IntId
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        /// <summary>
        /// Obtiene permisos por código, empresa y usuario.
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <param name="identificacion_empresa"></param>
        /// <param name="codigo_opcion"></param>
        /// <returns></returns>
        public IHttpActionResult Get(string codigo_usuario, string identificacion_empresa, string codigo_opcion)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_OpcionesUsuario clase_opc_usuario = new Ctl_OpcionesUsuario();

                List<TblOpcionesUsuario> datos = clase_opc_usuario.ObtenerOpcionesUsuarios(codigo_usuario, identificacion_empresa, codigo_opcion, false);

                if (datos == null)
                {
                    return NotFound();
                }

                var retorno = datos.Select(d => new
                {
                    Codigo = d.IntIdOpcion,
                    Descripcion = d.TblOpciones.StrDescripcion,
                    Consultar = d.IntConsultar,
                    Agregar = d.IntAgregar,
                    Editar = d.IntEditar,
                    Eliminar = d.IntEliminar,
                    Anular = d.IntAnular,
                    Gestion = d.IntGestion
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        /// <summary>
        /// Obtiene los permisos de las opciones de tipo indicadores.
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <param name="identificacion_empresa"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/PermisosIndicadores")]
        public IHttpActionResult PermisosIndicadores(string codigo_usuario, string identificacion_empresa, int tipo_perfil)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_OpcionesUsuario clase_opciones = new Ctl_OpcionesUsuario();

                List<TblOpcionesUsuario> opciones_indicadores = clase_opciones.ObtenerOpcionesTipo(codigo_usuario, identificacion_empresa, HGInetMiFacturaElectonicaData.Enumerables.TipoOpciones.Indicador, tipo_perfil);

                if (opciones_indicadores == null)
                {
                    return NotFound();
                }

                var retorno = opciones_indicadores.Select(d => new
                {
                    Codigo = d.IntIdOpcion,
                    Descripcion = d.TblOpciones.StrDescripcion,
                    Consultar = d.IntConsultar,
                    Agregar = d.IntAgregar,
                    Editar = d.IntEditar,
                    Eliminar = d.IntEliminar,
                    Anular = d.IntAnular,
                    Gestion = d.IntGestion
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Almacena los permisos del usuario en la base de datos
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post(JObject objeto)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_OpcionesUsuario clase_opc_usuario = new Ctl_OpcionesUsuario();

                var jobjeto = (dynamic)objeto;

                JArray itemDetailsJson = jobjeto.OpcionesUsuario;

                string usuario = jobjeto.Datos_usuario;
                string empresa = jobjeto.Datos_empresa;

                List<TblOpcionesUsuario> datos_registro = new List<TblOpcionesUsuario>();

                foreach (var item in itemDetailsJson)
                {
                    TblOpcionesUsuario registro = item.ToObject<TblOpcionesUsuario>();
                    registro.StrUsuario = usuario.ToLower();
                    registro.StrEmpresa = empresa;

                    datos_registro.Add(registro);
                }

                clase_opc_usuario.GestionarOpciones(datos_registro, usuario, empresa);

                return Ok();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

    }
}
