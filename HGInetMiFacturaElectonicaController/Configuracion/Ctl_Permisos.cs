using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController
{
    public class Ctl_Permisos : BaseObject<TblOpciones>
    {
        /// <summary>
        /// Obtiene los permisos del usuario (por perfil y por usuario)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<TblOpcionesUsuario> ObtenerPermisosUsuario(string usuario)
        {
            try
            {
                if (usuario.Equals("*"))
                    throw new Exception("El método no permite obtener los permisos de todos los usuarios.");

                // obtiene los permisos del usuario.
                List<TblOpcionesUsuario> permisos_usuario = (from opc_usuario in context.TblOpcionesUsuario
                                                             where opc_usuario.StrUsuario.Equals(usuario)
                                                             select opc_usuario).ToList();

                List<TblPerfiles> perfiles = (from perfiles_usuario in context.TblOpcionesPerfil
                                              join usuario_perfil in context.TblUsuariosPorPerfil on perfiles_usuario.IntIdPerfil equals usuario_perfil.IntIdPerfil
                                              where (usuario_perfil.StrUsuario.Equals(usuario))
                                              select usuario_perfil.TblPerfiles).Distinct().ToList();

                List<TblOpcionesPerfil> opciones_perfil = new List<TblOpcionesPerfil>();

                foreach (TblPerfiles item in perfiles)
                {
                    opciones_perfil.AddRange(ObtenerOpcionesPerfiles(item.IntId));
                }

                List<TblOpcionesPerfil> permisos_perfl = new List<TblOpcionesPerfil>();

                //unifica permisos por usuario y perfil.
                foreach (var permisoUsuario in permisos_usuario)
                {
                    permisos_perfl = opciones_perfil.Where(_opcion => _opcion.IntIdOpcion.Equals(permisoUsuario.IntIdOpcion)).ToList();

                    foreach (var permisoPerfil in permisos_perfl)
                    {
                        if (!permisoUsuario.IntAnular && permisoPerfil.IntAnular)
                            permisoUsuario.IntAnular = true;

                        if (!permisoUsuario.IntEditar && permisoPerfil.IntEditar)
                            permisoUsuario.IntEditar = true;

                        if (!permisoUsuario.IntEliminar && permisoPerfil.IntEliminar)
                            permisoUsuario.IntEliminar = true;

                        if (!permisoUsuario.IntGestion && permisoPerfil.IntGestion)
                            permisoUsuario.IntGestion = true;

                        if (!permisoUsuario.IntAgregar && permisoPerfil.IntAgregar)
                            permisoUsuario.IntAgregar = true;

                        if (!permisoUsuario.IntConsultar && permisoPerfil.IntConsultar)
                            permisoUsuario.IntConsultar = true;

                        opciones_perfil.Remove(permisoPerfil);
                    }
                }

                if (opciones_perfil.Count() > 0)
                {
                    foreach (var permiso_perfil in opciones_perfil)
                    {
                        if (permiso_perfil.IntAnular || permiso_perfil.IntEditar || permiso_perfil.IntEliminar || permiso_perfil.IntGestion || permiso_perfil.IntAgregar || permiso_perfil.IntConsultar)
                        {
                            // agrega la opción en la lista de permisos del usuario
                            permisos_usuario.Add(new TblOpcionesUsuario
                            {
                                IntAgregar = permiso_perfil.IntAgregar,
                                IntAnular = permiso_perfil.IntAnular,
                                IntConsultar = permiso_perfil.IntConsultar,
                                IntEditar = permiso_perfil.IntEditar,
                                IntEliminar = permiso_perfil.IntEliminar,
                                IntGestion = permiso_perfil.IntGestion,
                                IntIdOpcion = permiso_perfil.IntIdOpcion
                            });
                        }
                    }
                }

                return permisos_usuario;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);

            }
        }

        /// <summary>
        /// Obtien las opciones de permiso por perfil
        /// </summary>
        /// <param name="id_perfil"></param>
        /// <returns></returns>
        public List<TblOpcionesPerfil> ObtenerOpcionesPerfiles(short id_perfil)
        {
            try
            {
                List<TblOpcionesPerfil> permisos_perfil = (from opc_perfil in context.TblOpcionesPerfil
                                                           where (opc_perfil.IntIdPerfil == id_perfil)
                                                           select opc_perfil).ToList();

                return permisos_perfil;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene la opción por código
        /// </summary>
        /// <param name="id_opcion"></param>
        /// <returns></returns>
        public TblOpciones ObtenerOpcion(int id_opcion)
        {
            try
            {
                TblOpciones opcion = (from opc in context.TblOpciones
                                      where (opc.IntId == id_opcion)
                                      select opc).FirstOrDefault();

                return opcion;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        public List<TblOpciones> ObtenerOpciones()
        {
            try
            {
                List<TblOpciones> opciones = (from opc in context.TblOpciones
                                              select opc).ToList();

                return opciones;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene las opciones asociadas al permiso
        /// </summary>
        /// <param name="codigo_opcion"></param>
        /// <param name="incluir_opcion"></param>
        /// <param name="nivel"></param>
        /// <returns></returns>
        public List<TblOpciones> ObtenerAsociadas(int codigo_opcion, bool incluir_opcion, int nivel = -1)
        {
            int nivelActual = 0;

            // obtener las opciones de base de datos
            List<TblOpciones> opciones = ObtenerOpciones();

            if (!opciones.Any())
                return new List<TblOpciones>();

            List<TblOpciones> opcionesGrupo = new List<TblOpciones>();

            int codigo_tmp = Convert.ToInt32(codigo_opcion);

            while (!codigo_tmp.Equals("0"))
            {
                bool incluir = true;

                if (nivel != -1 && nivel <= nivelActual)
                    break;

                TblOpciones opcion = opciones.Where(_opcion => _opcion.IntId == codigo_tmp).FirstOrDefault();

                if (opcion == null)
                    break;

                if (!incluir_opcion && opcion.IntId.Equals(codigo_opcion))
                    incluir = false;

                if (!opcion.IntIdDependencia.Equals("0") && incluir)
                    opcionesGrupo.Add(opcion);

                codigo_tmp = opcion.IntIdDependencia.Value;
                nivelActual++;

                if (nivelActual == opciones.Count())
                    break;

                if (opcion.IntIdDependencia.Value == opcion.IntId)
                    break;
            }

            return opcionesGrupo.OrderBy(_opcion => _opcion.IntId).ToList();
        }

        /// <summary>
        /// Obtiene la ruta (migas de pan).
        /// </summary>
        /// <param name="codigo_opcion"></param>
        /// <param name="incluir_opcion"></param>
        /// <param name="nivel"></param>
        /// <param name="opciones_asociadas"></param>
        /// <returns></returns>
        public string ObtenerRutaGrupo(int codigo_opcion, bool incluir_opcion, int nivel = -1, List<TblOpciones> opciones_asociadas = null)
        {
            string ruta_grupo = "";

            // obtener las opciones de base de datos
            if (opciones_asociadas == null)
                opciones_asociadas = ObtenerAsociadas(codigo_opcion, incluir_opcion, nivel);

            if (!opciones_asociadas.Any())
                return "";

            foreach (TblOpciones item in opciones_asociadas)
            {
                bool incluir = true;

                if (!incluir_opcion && item.IntId.Equals(codigo_opcion))
                    incluir = false;

                if (incluir)
                {
                    if (string.IsNullOrWhiteSpace(ruta_grupo))
                        ruta_grupo = string.Format("{0}", item.StrDescripcion);
                    else
                        ruta_grupo = string.Format(" / {0} / {1}", ruta_grupo, item.StrDescripcion);
                }
            }
            return ruta_grupo;
        }

    }
}
