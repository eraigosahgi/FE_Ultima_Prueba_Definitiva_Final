using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_Planes : BaseObject<TblPlanesTransacciones>
    {

        public TblPlanesTransacciones Crear(TblPlanesTransacciones datos_plan)
        {
            datos_plan = this.Add(datos_plan);

            return datos_plan;
        }

        /// <summary>
        /// Crea los planes
        /// </summary>
        /// <param name="datos_plan">datos del plan tipo TblPlanesTransacciones</param>
        /// <param name="codigo_usuario">código del usuario (autenticado)</param>
        /// <returns></returns>
        public TblPlanesTransacciones CrearPlan(TblPlanesTransacciones datos_plan)
        {
            datos_plan.DatFecha = Fecha.GetFecha();
            datos_plan.StrIdSeguridad = new System.Guid();

            Crear(datos_plan);

            return datos_plan;
        }


    }
}
