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
            Ptransaccion.IntEstado = datos_plan.IntEstado;
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
                          where (empresa.StrIdentificacion.Equals(Identificacion) || Identificacion.Equals("*") || empresa.StrEmpresaAsociada.Equals(Identificacion))
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

        #region Procesar Transacciones
        //**************************************************************
        /// <summary>
        /// Retorna una lista de ObjPlanenProceso para indicar al proceso, de que plan debe descontar los documentos procesados
        /// </summary>
        /// <param name="Stridentificacion">Identificación del facturador</param>
        /// <param name="CantidadDoc">Cantidad de documentos a procesar</param>
        /// <returns></returns>
        public List<ObjPlanEnProceso> ObtenerPlanesActivos(string Stridentificacion,int CantidadDoc)
        {
            List<TblPlanesTransacciones> datos_plan = (from t in context.TblPlanesTransacciones
                                                       where t.StrEmpresaFacturador.Equals(Stridentificacion)
                                                      // && t.BitProcesada == true
                                                       //&& (t.IntNumTransaccCompra - t.IntNumTransaccProcesadas)>0
                                                       //&& t.DatFechaVencimiento<= Fecha.GetFecha()

                                                       select t).OrderBy(x => new { x.IntTipoProceso,x.DatFechaVencimiento }).ToList();
           

            List<ObjPlanEnProceso> Listaobjproceso = new List<ObjPlanEnProceso>();
            ObjPlanEnProceso objproceso = new ObjPlanEnProceso();

            int Disponibles;
            foreach (var plan in datos_plan)
            {

                if (plan.IntTipoProceso == (Int16)3)
                {
                    objproceso = new ObjPlanEnProceso();
                    objproceso.enProceso = CantidadDoc;
                    objproceso.plan = plan.StrIdSeguridad;
                    Listaobjproceso.Add(objproceso);
                    CantidadDoc = 0;
                    break;
                }
                else
                {
                    Disponibles = (plan.IntNumTransaccCompra - (plan.IntNumTransaccProcesadas + plan.IntNumTransaccProceso));
                    if (Disponibles > 0)
                    {
                        objproceso = new ObjPlanEnProceso();
                        if (CantidadDoc > Disponibles)
                        {
                            objproceso.enProceso = Disponibles;
                            objproceso.plan = plan.StrIdSeguridad;
                            Listaobjproceso.Add(objproceso);
                            CantidadDoc = CantidadDoc - Disponibles;
                        }
                        else
                        {
                            objproceso.enProceso = CantidadDoc;
                            objproceso.plan = plan.StrIdSeguridad;
                            Listaobjproceso.Add(objproceso);
                            CantidadDoc = 0;
                            break;
                        }
                    }
                }
                                
            }

            if (CantidadDoc > 0)
            {
                return null;
            }else
            {
                foreach (var item in Listaobjproceso)
                {
                    Enproceso(item.plan, item.enProceso);
                }
            }
            return Listaobjproceso;
        } 


        /// <summary>
        /// Actualiza el campo IntNumTransaccProceso para indicar cuantos documentos estan reservados para procesar
        /// </summary>
        /// <param name="stridseguridad">id de seguridad del plan</param>
        /// <param name="CantDocProcesar">Cantidad de documentos reservados para este proceso en este plan</param>
        /// <returns></returns>
        public TblPlanesTransacciones Enproceso(Guid stridseguridad, int CantDocProcesar) {
            
            TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
                                                       where t.StrIdSeguridad.Equals(stridseguridad)                                                       
                                                       select t).FirstOrDefault();

            PlanesTransacciones.IntNumTransaccProceso = PlanesTransacciones.IntNumTransaccProceso + CantDocProcesar;

             PlanesTransacciones = this.Edit(PlanesTransacciones);

            return PlanesTransacciones;
        }
       
        /// <summary>
        /// Actualiza el plan al terminar el proceso, coloca los valores reales luego del proceso
        /// campo intEnProceso le descuenta la cantidad de documentos que se estaban procesando
        /// </summary>
        /// <param name="PlanesTransacciones"></param>
        /// <param name="PlanEnProceso"></param>
        /// <returns></returns>
        public TblPlanesTransacciones ConciliarPlanProceso(ObjPlanEnProceso PlanEnProceso)
        {
            TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
                                                          where t.StrIdSeguridad.Equals(PlanEnProceso.plan)
                                                          select t).FirstOrDefault();

            //Se debe Sumar el procesado del objeto a la cantidad de documentos procesados en la tabla             
            PlanesTransacciones.IntNumTransaccProcesadas = PlanesTransacciones.IntNumTransaccProcesadas + PlanEnProceso.procesado;

            //En el campo de en proceso, se debe restar la cantidad de documentos reservados del objeto a la cantidad de objetos de la tabla
            PlanesTransacciones.IntNumTransaccProceso = PlanesTransacciones.IntNumTransaccProceso - PlanEnProceso.enProceso;

            if (PlanesTransacciones.IntTipoProceso != 3)
            {
                if (PlanesTransacciones.IntNumTransaccProcesadas >= PlanesTransacciones.IntNumTransaccCompra)
                {
                    //Aqui es donde se coloca el plan como procesado en su totalidad en caso de no tener mas saldo
                   // PlanesTransacciones.BitProcesada = false;
                }
            }else
            {
                //Si el plan es post-pago, entonces iguala el numero de transacciones adquiridas, por numero de transacciones procesadas
                //con el fin de que todos los indicadores queden bien representados.
                PlanesTransacciones.IntNumTransaccCompra = PlanesTransacciones.IntNumTransaccProcesadas;
            }

            PlanesTransacciones = this.Edit(PlanesTransacciones);

            return PlanesTransacciones;
        }

        /// <summary>
        /// Descuenta del saldo de transacciones de planes, una cantidad especifica de documentos 
        /// </summary>
        /// <param name="IdPlan">id de seguridad del plan que se desea descontar los documentos</param>
        /// <returns></returns>
        public TblPlanesTransacciones DescontarDocumentosFallidos(Guid IdPlan,int CantDoc)
        {
            TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
                                                          where t.StrIdSeguridad.Equals(IdPlan)
                                                          select t).FirstOrDefault();
            if (PlanesTransacciones != null)
            {
                PlanesTransacciones.IntNumTransaccProcesadas = PlanesTransacciones.IntNumTransaccProcesadas - CantDoc;

                PlanesTransacciones = this.Edit(PlanesTransacciones);
            }
            return PlanesTransacciones;
        }


        public class ObjPlanEnProceso
        {
            public Guid plan { get; set; }
            public int enProceso { get; set; }
            public int reservado { get; set; }
            public int procesado { get; set; }
        }


        public class ObjDocError
        {
            public Guid plan { get; set; }            
            public long Doc { get; set; }
        }




        #endregion
    }
}
