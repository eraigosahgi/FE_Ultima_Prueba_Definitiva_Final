using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_OpcionesUsuario : BaseObject<TblOpcionesUsuario>
    {
        #region Crear

        public TblOpcionesUsuario Crear(TblOpcionesUsuario opcion_usuario)
        {
            try
            {
                opcion_usuario = this.Add(opcion_usuario);

                return opcion_usuario;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Agrega las opciones de permiso del usuario.
        /// </summary>
        /// <param name="opciones_usuario"></param>
        /// <returns></returns>
        public List<TblOpcionesUsuario> CrearOpciones(List<TblOpcionesUsuario> opciones_usuario)
        {
            try
            {
                foreach (TblOpcionesUsuario item in opciones_usuario)
                {
                    Crear(item);
                }

                return opciones_usuario;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Actualiza los permisos del usuario
        /// Agrega o Elimina, según la nueva información
        /// </summary>
        /// <param name="opciones_usuario">Lista de opciones a validar</param>
        /// <returns></returns>
        public List<TblOpcionesUsuario> GestionarOpciones(List<TblOpcionesUsuario> opciones_usuario, string codigo_usuario, string codigo_empresa)
        {
            try
            {
                //Obtiene los permisos del usuario actualmente en la base de datos.
                List<TblOpcionesUsuario> opciones_usuario_bd = new List<TblOpcionesUsuario>();
                opciones_usuario_bd = ObtenerOpcionesUsuarios(codigo_usuario, codigo_empresa);

                List<TblOpcionesUsuario> opciones_grabar = new List<TblOpcionesUsuario>();


                //Valida que los permisos de la base de datos se encuentren en la nueva lista, de lo contrario los elimina.
                foreach (var item_bd in opciones_usuario_bd)
                {
                    TblOpcionesUsuario registro_bd = opciones_usuario.Where(x => x.IntIdOpcion == item_bd.IntIdOpcion).FirstOrDefault();

                    if (registro_bd == null)
                    {
                        Eliminar(item_bd);
                    }
                }

                //Valida si los permisos de la nueva lista se encuentran en los permisos de base de datos, si no existen, los genera.
                foreach (TblOpcionesUsuario item in opciones_usuario)
                {
                    TblOpcionesUsuario registro_nuevo = opciones_usuario_bd.Where(x => x.IntIdOpcion == item.IntIdOpcion).FirstOrDefault();

                    if (registro_nuevo == null)
                    {
                        opciones_grabar.Add(item);
                    }
                    else
                    {
                        Actualizar(item);
                    }
                }

                if (opciones_grabar.Count() > 0)
                    CrearOpciones(opciones_grabar);

                return opciones_usuario;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion

        #region Actualizar

        /// <summary>
        /// Actualiza el registro en la base de datos
        /// </summary>
        /// <param name="opcion_usuario"></param>
        /// <returns></returns>
        public TblOpcionesUsuario Actualizar(TblOpcionesUsuario opcion_usuario)
        {
            try
            {
                opcion_usuario = this.Edit(opcion_usuario);

                return opcion_usuario;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion

        #region Obtener

        /// <summary>
        /// Obtiene las opciones de permisos por usuario y empresa
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <param name="identificacion_empresa"></param>
        /// <returns></returns>
        public List<TblOpcionesUsuario> ObtenerOpcionesUsuarios(string codigo_usuario, string identificacion_empresa, string codigo_opcion = "*")
        {
            try
            {
                int cod_opcion = -1;
                int.TryParse(codigo_opcion, out cod_opcion);

                List<TblOpcionesUsuario> permisos_usuario = (from opcion in context.TblOpcionesUsuario
                                                             where opcion.StrUsuario.Equals(codigo_usuario)
                                                             && opcion.StrEmpresa.Equals(identificacion_empresa)
                                                             && (opcion.IntIdOpcion == cod_opcion || codigo_opcion.Equals("*"))
                                                             select opcion).ToList();

                return permisos_usuario;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene los permisos del usuario a gestionar, en base a los permisos del usuario autenticado.
        /// </summary>
        /// <param name="usuario_autenticado"></param>
        /// <param name="empresa_autenticada"></param>
        /// <param name="codigo_usuario"></param>
        /// <param name="identificacion_empresa"></param>
        /// <param name="codigo_opcion"></param>
        /// <returns></returns>
        public List<TblOpcionesUsuario> ObtenerOpcionesUsuarios(string usuario_autenticado, string empresa_autenticada, string codigo_usuario, string identificacion_empresa, string codigo_opcion = "*")
        {
            try
            {
                int cod_opcion = -1;
                int.TryParse(codigo_opcion, out cod_opcion);


                List<TblOpcionesUsuario> permisos_usuario_autenticado = (from opcion in context.TblOpcionesUsuario
                                                                         where opcion.StrUsuario.Equals(usuario_autenticado)
                                                                         && opcion.StrEmpresa.Equals(empresa_autenticada)
                                                                         && (opcion.IntIdOpcion == cod_opcion || codigo_opcion.Equals("*"))
                                                                         select opcion).ToList();

                List<TblOpcionesUsuario> permisos_usuario = (from opcion in context.TblOpcionesUsuario
                                                             where opcion.StrUsuario.Equals(codigo_usuario)
                                                             && opcion.StrEmpresa.Equals(identificacion_empresa)
                                                             && (opcion.IntIdOpcion == cod_opcion || codigo_opcion.Equals("*"))
                                                             select opcion).ToList();


                foreach (TblOpcionesUsuario item in permisos_usuario_autenticado)
                {
                    TblOpcionesUsuario item_registro = permisos_usuario.Where(d => d.IntIdOpcion == item.IntIdOpcion).FirstOrDefault();

                    if (item_registro != null)
                    {
                        item.IntConsultar = item_registro.IntConsultar;
                        item.IntAgregar = item_registro.IntAgregar;
                        item.IntEditar = item_registro.IntEditar;
                        item.IntEliminar = item_registro.IntEliminar;
                        item.IntAnular = item_registro.IntAnular;
                        item.IntGestion = item_registro.IntGestion;
                    }
                    else
                    {
                        item.IntConsultar = false;
                        item.IntAgregar = false;
                        item.IntEditar = false;
                        item.IntEliminar = false;
                        item.IntAnular = false;
                        item.IntGestion = false;
                    }
                }

                return permisos_usuario_autenticado;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        #endregion

        #region Eliminar

        /// <summary>
        /// Elimina el registro de la base de datos.
        /// </summary>
        /// <param name="opcion_usuario"></param>
        /// <returns></returns>
        public bool Eliminar(TblOpcionesUsuario opcion_usuario)
        {
            try
            {
                this.Delete(opcion_usuario);

                return true;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion
    }
}
