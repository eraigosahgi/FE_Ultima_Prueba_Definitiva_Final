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

            return tbl_usuario;
        }

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
        /// Obtiene los datos por código de usuario.
        /// </summary>
        /// <param name="codigo_usuario"></param>
        /// <returns></returns>
        public TblUsuarios ObtenerUsuario(System.Guid codigo_usuario)
        {
            try
            {
                var respuesta = from usuario in context.TblUsuarios
                                where (usuario.StrUsuario.Equals(codigo_usuario))
                                select usuario;

                return respuesta.FirstOrDefault();
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

        #region Actualizar TblUsuarios        
        public TblUsuarios Actualizar_usuario(TblUsuarios usuario)
        {
            usuario = this.Update(usuario);

            return usuario;

        }
        #endregion
    }
}
