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
    public class Ctl_PlanesTransacciones : BaseObject<TblPlanesTransacciones>
    {

        /// <summary>
        /// Crea los planes transacciones
        /// </summary>
        /// <param name="datos_plan">datos del plan tipo TblPlanesTransacciones</param>
        /// <param name="codigo_usuario">código del usuario (autenticado)</param>
        /// <returns></returns>
        public TblPlanesTransacciones Crear(TblPlanesTransacciones datos_plan)
        {
            datos_plan.DatFecha = Fecha.GetFecha();
            datos_plan.StrIdSeguridad = Guid.NewGuid();

            datos_plan = this.Add(datos_plan);

            Ctl_Empresa empresa = new Ctl_Empresa();

            TblEmpresas facturador = empresa.Obtener(datos_plan.StrEmpresaFacturador);

            Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

            email.EnviaNotificacionRecarga(facturador.StrIdentificacion, facturador.StrMail, datos_plan);

            return datos_plan;
        }



        /// <summary>
        /// Editar plan de transacciones
        /// </summary>
        /// <param name="datos_plan">datos del plan tipo TblPlanesTransacciones</param>

        /// <returns></returns>
        public TblPlanesTransacciones Editar(TblPlanesTransacciones datos_plan)
        {
            TblPlanesTransacciones Ptransaccion = (from t in context.TblPlanesTransacciones
                                                   where t.StrIdSeguridad.Equals(datos_plan.StrIdSeguridad)
                                                   select t).FirstOrDefault();

            Ptransaccion.IntTipoProceso = datos_plan.IntTipoProceso;
            Ptransaccion.IntNumTransaccCompra = datos_plan.IntNumTransaccCompra;
            Ptransaccion.IntValor = datos_plan.IntValor;
            Ptransaccion.BitProcesada = datos_plan.BitProcesada;
            Ptransaccion.StrObservaciones = datos_plan.StrObservaciones;
            Ptransaccion.StrEmpresaFacturador = datos_plan.StrEmpresaFacturador;

            Ptransaccion = this.Edit(Ptransaccion);

            return Ptransaccion;
        }




        /// <summary>
        /// Consulta los planes
        /// </summary>        
        /// <returns></returns>
        public List<TblPlanesTransacciones> Obtener(string Identificacion)
        {
            List<TblPlanesTransacciones> datos_plan = new List<TblPlanesTransacciones>();


            datos_plan = (from t in context.TblPlanesTransacciones
                          join empresa in context.TblEmpresas on t.StrEmpresaFacturador equals empresa.StrIdentificacion
                          join empresacrea in context.TblEmpresas on t.StrEmpresaUsuario equals empresacrea.StrIdentificacion
                          where (empresa.StrIdentificacion.Equals(Identificacion) || Identificacion.Equals("*"))
                          orderby t.DatFecha descending
                          select t).ToList();

            return datos_plan;
        }

        /// <summary>
        /// Obtiene el plan transaccional por id de seguridad.
        /// </summary>
        /// <param name="id_seguridad"></param>
        /// <returns></returns>
        public TblPlanesTransacciones ObtenerIdSeguridad(System.Guid id_seguridad)
        {
            TblPlanesTransacciones datos_plan = (from plan in context.TblPlanesTransacciones
                                                 where plan.StrIdSeguridad.Equals(id_seguridad)
                                                 select plan).FirstOrDefault();

            return datos_plan;
        }


        /// <summary>
        /// Consulta el plan por Id de Seguridad
        /// <param name="StrIdSeguridad">datos TblPlanesTransacciones </param>                
        /// </summary>        
        /// <returns></returns>
        public List<TblPlanesTransacciones> Obtener(System.Guid StrIdSeguridad)
        {
            List<TblPlanesTransacciones> datos_plan = (from t in context.TblPlanesTransacciones
                                                       where t.StrIdSeguridad.Equals(StrIdSeguridad)
                                                       select t).ToList();

            return datos_plan;
        }

    }
}
