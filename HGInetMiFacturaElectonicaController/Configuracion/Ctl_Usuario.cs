using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using HGInetMiFacturaElectonicaData.ModeloServicio.General;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_Usuario : BaseObject<TblUsuarios>
    {

        #region Constructores 

        public Ctl_Usuario() : base(new ModeloAutenticacion()) { }
        public Ctl_Usuario(ModeloAutenticacion autenticacion) : base(autenticacion) { }

        public Ctl_Usuario(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
        #endregion

        #region Guardar
        public TblUsuarios Crear(TblUsuarios usuario)
        {
            usuario = this.Add(usuario);

            return usuario;
        }

        /// <summary>
        /// Crea el usuario principal para la empresa
        /// </summary>
        /// <param name="empresa">información de la empresa</param>
        /// <returns>información del usuario</returns>
        public TblUsuarios Crear(TblEmpresas empresa)
        {
            TblUsuarios tbl_usuario = new TblUsuarios();

            tbl_usuario.StrEmpresa = empresa.StrIdentificacion;
            tbl_usuario.StrUsuario = empresa.StrIdentificacion;
            tbl_usuario.StrClave = Encriptar.Encriptar_MD5(empresa.StrIdentificacion);
            tbl_usuario.StrNombres = empresa.StrRazonSocial;
            tbl_usuario.StrApellidos = "";
            tbl_usuario.StrMail = empresa.StrMail;
            tbl_usuario.DatFechaIngreso = Fecha.GetFecha();
            tbl_usuario.DatFechaActualizacion = Fecha.GetFecha();
            tbl_usuario.DatFechaCambioClave = Fecha.GetFecha();
            tbl_usuario.IntIdEstado = 1;
            tbl_usuario.StrIdSeguridad = Guid.NewGuid();
            tbl_usuario.StrIdCambioClave = Guid.NewGuid();

            // agrega el usuario en la base de datos
            tbl_usuario = Crear(tbl_usuario);

            AsignarPermisos(empresa);

            return tbl_usuario;
        }

        /// <summary>
        /// Asigna los permisos al usuario según el perfil de la empresa (Obligado ó Adquiriente).
        /// </summary>
        /// <param name="datos_empresa">datos de la empresa</param>
        public void AsignarPermisos(TblEmpresas datos_empresa)
        {
            try
            {
                Ctl_Permisos clase_permisos = new Ctl_Permisos();
                List<TblOpcionesPerfil> opciones_perfil = new List<TblOpcionesPerfil>();

                //Obtiene permisos del facturador.
                if (datos_empresa.IntObligado)
                    opciones_perfil = clase_permisos.ObtenerOpcionesPerfiles((short)Perfiles.Facturador);

                //Obtiene permisos del adquiriente.
                if (datos_empresa.IntAdquiriente)
                    opciones_perfil = clase_permisos.ObtenerOpcionesPerfiles((short)Perfiles.Adquiriente);

                List<TblOpcionesUsuario> opciones_usuario = new List<TblOpcionesUsuario>();

                //Añade las opciones TblOpcionesPerfil en una lista de tipo TblOpcionesUsuario.
                if (opciones_perfil.Count() > 0)
                {
                    foreach (var permiso_perfil in opciones_perfil)
                    {
                        if (permiso_perfil.IntAnular || permiso_perfil.IntEditar || permiso_perfil.IntEliminar || permiso_perfil.IntGestion || permiso_perfil.IntAgregar || permiso_perfil.IntConsultar)
                        {
                            TblOpcionesUsuario permiso = new TblOpcionesUsuario();

                            permiso.IntAgregar = permiso_perfil.IntAgregar;
                            permiso.IntAnular = permiso_perfil.IntAnular;
                            permiso.IntConsultar = permiso_perfil.IntConsultar;
                            permiso.IntEditar = permiso_perfil.IntEditar;
                            permiso.IntEliminar = permiso_perfil.IntEliminar;
                            permiso.IntGestion = permiso_perfil.IntGestion;
                            permiso.IntIdOpcion = permiso_perfil.IntIdOpcion;
                            permiso.StrUsuario = datos_empresa.StrIdentificacion;
                            permiso.StrEmpresa = datos_empresa.StrIdentificacion;

                            opciones_usuario.Add(permiso);
                        }
                    }
                }

                //Almacena la información de las opciones de permiso del usuario en base de datos.
                if (opciones_usuario.Count > 0)
                {
                    Ctl_OpcionesUsuario clase_opciones_usuario = new Ctl_OpcionesUsuario();
                    clase_opciones_usuario.CrearOpciones(opciones_usuario);
                }
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Crea el usuario principal para la empresa
        /// </summary>
        /// <param name="empresa">información de la empresa</param>
        /// <returns>información del usuario</returns>
        public bool Crear(TblUsuarios usuario, TblEmpresas empresa = null)
        {

            try
            {
                //Valido si es el usuario existe para la misma empresa
                List<TblUsuarios> ConsultaUsuario = ObtenerUsuarios(usuario.StrUsuario, usuario.StrEmpresa);
                if (ConsultaUsuario.Count > 0)
                    throw new ApplicationException("El Usuario :  " + usuario.StrUsuario + " ya existe");

                //Aqui se deben validar los campos del objeto
                TblUsuarios tbl_usuario = new TblUsuarios();
                usuario.StrClave = System.Guid.NewGuid().ToString();

                usuario.DatFechaIngreso = Fecha.GetFecha();
                usuario.DatFechaActualizacion = Fecha.GetFecha();
                usuario.DatFechaCambioClave = Fecha.GetFecha();
                usuario.IntIdEstado = usuario.IntIdEstado;
                usuario.StrIdSeguridad = Guid.NewGuid();
                usuario.StrIdCambioClave = Guid.NewGuid();

                // agrega el usuario en la base de datos
                tbl_usuario = Crear(usuario);


                //Aqui debo enviar el correo a usuarioRestablecer.StrMail
                Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

                empresa = (from a in context.TblEmpresas
                           where a.StrIdentificacion.Equals(usuario.StrEmpresa)
                           select a).FirstOrDefault();

                Email.Bienvenida(empresa, usuario);


                return true;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion

        #region Actualizar

        /// <summary>
        /// Actualiza el usuario en la base ded atos
        /// </summary>
        /// <param name="usuario">información del Usuario</param>
        /// <returns>información del usuario</returns>

        public TblUsuarios Actualizar_usuario(TblUsuarios usuario)
        {
            usuario = this.Edit(usuario);

            return usuario;

        }


        /// <summary>
        /// Recibe parte del usuario y luego se envia a actualizar usuario para guardarlo en db
        /// </summary>
        /// <param name="usuario">información del usuario</param>
        /// <returns>información del usuario</returns>
        public bool Actualizar(TblUsuarios usuario)
        {
            try
            {
                TblUsuarios UsuarioActiliza = (from item in context.TblUsuarios
                                               where item.StrUsuario.Equals(usuario.StrUsuario)
                                               && item.StrEmpresa.Equals(usuario.StrEmpresa)
                                               select item).FirstOrDefault();
                if (UsuarioActiliza != null)
                {
                    UsuarioActiliza.StrEmpresa = usuario.StrEmpresa;
                    UsuarioActiliza.StrNombres = usuario.StrNombres;
                    UsuarioActiliza.StrApellidos = usuario.StrApellidos;
                    UsuarioActiliza.StrMail = usuario.StrMail;
                    UsuarioActiliza.StrTelefono = usuario.StrTelefono;
                    UsuarioActiliza.StrExtension = usuario.StrExtension;
                    UsuarioActiliza.StrCelular = usuario.StrCelular;
                    UsuarioActiliza.StrCargo = usuario.StrCargo;
                    UsuarioActiliza.IntIdEstado = usuario.IntIdEstado;

                    UsuarioActiliza.DatFechaActualizacion = Fecha.GetFecha();
                    UsuarioActiliza.DatFechaCambioClave = Fecha.GetFecha();
                    UsuarioActiliza.StrIdSeguridad = Guid.NewGuid();

                    // agrega el usuario en la base de datos
                    usuario = Actualizar_usuario(UsuarioActiliza);

                    return true;
                }
                else
                {
                    throw new ApplicationException("Datos Invalidos, el Usuario no coincide con la empresa");
                }
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
        #endregion

        #region Validar

        public List<TblUsuarios> ValidarExistencia(string codigo_empresa, string codigo_usuario, string clave)
        {
            try
            {
                clave = LibreriaGlobalHGInet.General.Encriptar.Encriptar_MD5(clave);

                var respuesta = from usuario in context.TblUsuarios
                                join empresa in context.TblEmpresas on usuario.StrEmpresa equals empresa.StrIdentificacion
                                where empresa.StrIdentificacion.Equals(codigo_empresa)
                                && usuario.StrUsuario.Equals(codigo_usuario)
                                && usuario.StrClave.Equals(clave)
                                select usuario;

                return respuesta.ToList();
            }
            catch (Exception excepcion)
            {

                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion

        #region Obtener

        /// <summary>
        /// Obtiene los datos por código de usuario y empresa.
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <returns></returns>
        public List<TblUsuarios> ObtenerUsuarios(string codigo_usuario, string codigo_empresa)
        {
            try
            {
                var respuesta = from usuario in context.TblUsuarios
                                where (usuario.StrUsuario.Equals(codigo_usuario) || codigo_usuario.Equals("*"))
                                && (usuario.StrEmpresa.Equals(codigo_empresa) || codigo_empresa.Equals("*"))
                                select usuario;

                return respuesta.ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el usuario por id seguridad
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <returns></returns>
        public TblUsuarios ObtenerIdSeguridad(System.Guid id_seguridad)
        {
            try
            {
                var respuesta = from usuario in context.TblUsuarios
                                where (usuario.StrIdSeguridad.Equals(id_seguridad))
                                select usuario;

                return respuesta.FirstOrDefault();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion

        #region Validar documento y usuario para restablecer
        public bool ValidarExistenciaRestablecer(string codigo_empresa, string codigo_usuario)
        {
            try
            {
                var respuesta = from usuario in context.TblUsuarios
                                join empresa in context.TblEmpresas on usuario.StrEmpresa equals empresa.StrIdentificacion
                                where empresa.StrIdentificacion.Equals(codigo_empresa)
                                && usuario.StrUsuario.Equals(codigo_usuario)
                                select usuario;


                if (!respuesta.Any())
                    throw new ApplicationException("Datos Incorrectos.");

                TblUsuarios usuarioRestablecer = respuesta.FirstOrDefault();

                usuarioRestablecer.DatFechaCambioClave = Fecha.GetFecha();
                usuarioRestablecer.StrIdCambioClave = Guid.NewGuid();
                Actualizar_usuario(usuarioRestablecer);

                TblEmpresas Empresas = (from empresa in context.TblEmpresas
                                        where empresa.StrIdentificacion.Equals(codigo_empresa)
                                        select empresa).FirstOrDefault();

                //Aqui debo enviar el correo a usuarioRestablecer.StrMail
                Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

                Email.RestablecerClave(Empresas, usuarioRestablecer);

                return true;
            }
            catch (Exception excepcion)
            {

                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
        #endregion

        #region Cambio de Clave
        public bool RestablecerClave(System.Guid IdSeguridad, string clave)
        {
            try
            {
                TblUsuarios datos = (from item in context.TblUsuarios
                                     where item.StrIdCambioClave == IdSeguridad
                                     select item).FirstOrDefault();
                if (datos != null)
                {

                    if (!(datos.DatFechaCambioClave.Value.AddHours(24.0) >= Fecha.GetFecha()))
                    {
                        throw new ApplicationException("El ID de seguridad ha expirado; por favor realice el proceso de recuperación de contraseña nuevamente.");
                    }


                    clave = LibreriaGlobalHGInet.General.Encriptar.Encriptar_MD5(clave);

                    datos.StrClave = clave;
                    datos.DatFechaCambioClave = Fecha.GetFecha();
                    datos.StrIdCambioClave = Guid.NewGuid();
                    Actualizar_usuario(datos);
                    return true;
                }
                else
                {
                    throw new ApplicationException("Id de seguridad incorrecto.");
                }
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException("Id de seguridad incorrecto.", excepcion.InnerException);
            }
        }

        /// <summary>
        /// Valida el id de seguridad para cambio de contraseña
        /// </summary>
        /// <param name="IdSeguridad">id de seguridad</param>
        /// <returns>indica si es válido el cambio de clave</returns>
        public bool ValidarIdSeguridad(System.Guid IdSeguridad)
        {
            try
            {
                List<TblUsuarios> datos = (from item in context.TblUsuarios
                                           where item.StrIdCambioClave == IdSeguridad
                                           select item).ToList();

                if (datos.Any())
                {

                    if (datos.FirstOrDefault() != null)
                    {
                        if (!datos.FirstOrDefault().DatFechaCambioClave.HasValue || !(datos.FirstOrDefault().DatFechaCambioClave.Value.AddHours(24.0) >= Fecha.GetFecha()))
                        {
                            throw new ApplicationException("El ID de seguridad ha expirado; por favor realice el proceso de recuperación de contraseña nuevamente.");
                        }
                        return true;
                    }
                }

                throw new ApplicationException("Id de seguridad incorrecto.");
            }
            catch (ApplicationException excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        #endregion

    }
}
