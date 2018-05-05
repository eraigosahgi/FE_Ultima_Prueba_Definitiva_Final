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

            tbl_usuario.IntIdEmpresa = empresa.IntId;
            tbl_usuario.StrUsuario = empresa.StrIdentificacion;
            tbl_usuario.StrClave = empresa.StrIdentificacion;
            tbl_usuario.StrNombres = empresa.StrRazonSocial;
            tbl_usuario.StrApellidos = "";
            tbl_usuario.StrMail = empresa.StrMail;
            tbl_usuario.DatFechaIngreso = Fecha.GetFecha();
            tbl_usuario.DatFechaActualizacion = Fecha.GetFecha();
            tbl_usuario.IntIdEstado = 1;
            tbl_usuario.StrIdSeguridad = Guid.NewGuid();
			tbl_usuario.StrIdCambioClave = Guid.NewGuid();

            // agrega el usuario en la base de datos
            tbl_usuario = Crear(tbl_usuario);

            return tbl_usuario;
        }

        #region Validar

        public bool ValidarExistencia(string codigo_empresa, string codigo_usuario, string clave)
        {
            try
            {
                clave = LibreriaGlobalHGInet.General.Encriptar.Encriptar_MD5(clave);

                var respuesta = from usuario in context.TblUsuarios
                                join empresa in context.TblEmpresas on usuario.IntIdEmpresa equals empresa.IntId
                                where empresa.StrIdentificacion.Equals(codigo_empresa)
                                && usuario.StrUsuario.Equals(codigo_usuario)
                                && usuario.StrClave.Equals(clave)
                                select usuario;

                if (respuesta.FirstOrDefault() == null)
                    return false;

                return true;
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
                                join empresa in context.TblEmpresas on usuario.IntIdEmpresa equals empresa.IntId
                                where empresa.StrIdentificacion.Equals(codigo_empresa)
                                && usuario.StrUsuario.Equals(codigo_usuario)
                                select usuario;


                if (!respuesta.Any())
                    return false;

                TblUsuarios usuarioRestablecer = respuesta.FirstOrDefault();
                                
                usuarioRestablecer.DatFechaCambioClave = Fecha.GetFecha();
                usuarioRestablecer.StrIdCambioClave = Guid.NewGuid();
                Actualizar_usuario(usuarioRestablecer);

                TblEmpresas Empresas = (from  empresa in context.TblEmpresas
                                where empresa.StrIdentificacion.Equals(codigo_empresa)                                
                                select empresa).FirstOrDefault() ;

                //Aqui debo enviar el correo a usuarioRestablecer.StrMail
                Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();
                      
                Email.RestablecerClave(Empresas,usuarioRestablecer);

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
                    datos.StrIdCambioClave= Guid.NewGuid();
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

        public bool ValidarIdSeguridad(System.Guid IdSeguridad)
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
                    return true;
                }
                else
                {
                    throw new ApplicationException("Id de seguridad incorrecto.");
                }
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
