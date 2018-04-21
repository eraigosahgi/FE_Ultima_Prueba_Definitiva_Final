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

            // agrega el usuario en la base de datos
            tbl_usuario = Crear(tbl_usuario);

            return tbl_usuario;
        }
        


    }
}
