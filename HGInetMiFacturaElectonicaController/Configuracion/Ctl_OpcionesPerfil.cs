using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_OpcionesPerfil : BaseObject<TblOpcionesPerfil>
    {
        /// <summary>
        /// Obtien las opciones de permiso por perfil
        /// </summary>
        /// <param name="id_perfil"></param>
        /// <returns></returns>
        public List<TblOpcionesPerfil> ObtenerOpcionesPorPerfil(short id_perfil)
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

    }
}
