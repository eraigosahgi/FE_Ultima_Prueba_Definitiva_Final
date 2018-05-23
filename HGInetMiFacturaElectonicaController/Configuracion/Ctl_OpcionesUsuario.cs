using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    class Ctl_OpcionesUsuario : BaseObject<TblOpcionesUsuario>
    {

        public TblOpcionesUsuario Crear(TblOpcionesUsuario opcion_usuario)
        {
            opcion_usuario = this.Add(opcion_usuario);

            return opcion_usuario;
        }


        /// <summary>
        /// Agrega las opciones de permiso del usuario.
        /// </summary>
        /// <param name="opciones_usuario"></param>
        /// <returns></returns>
        public List<TblOpcionesUsuario> CrearOpciones(List<TblOpcionesUsuario> opciones_usuario)
        {

            foreach (TblOpcionesUsuario item in opciones_usuario)
            {
                Crear(item);
            }

            return opciones_usuario;
        }



    }
}
