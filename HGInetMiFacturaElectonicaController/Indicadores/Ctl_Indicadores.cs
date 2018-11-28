using HGInetMiFacturaElectonicaController.Indicadores.Objetos;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores
{
    public class Ctl_Indicadores : BaseObject<TblDocumentos>
    {
        /// <summary>
        /// Obtiene los porcentajes de documentos por estado.
        /// </summary>
        /// <param name="identificacion_empresa"></param>
        /// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
        /// <returns></returns>
        public List<PorcentajesResumen> DocumentosPorEstado(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                List<PorcentajesResumen> documentos_estado = (from documento in context.TblDocumentos
                                                              where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
                                                              orderby documento.IntIdEstado ascending
                                                              group documento by new { documento.IntIdEstado } into documento_estado
                                                              select new PorcentajesResumen
                                                              {
                                                                  Cantidad = documento_estado.Count(),
                                                                  Estado = documento_estado.FirstOrDefault().IntIdEstado,
                                                              }).ToList().ToList();

                decimal cantidad_total = documentos_estado.Sum(x => x.Cantidad);

                decimal sumatoria_cantidades = 0;

                string descripcion_tipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Perfiles>(tipo_empresa));

                foreach (PorcentajesResumen item in documentos_estado)
                {
                    switch (item.Estado)
                    {
                        case 90: item.Color = "#FF2D00"; break;
                        case 99: item.Color = "#62C415"; break;
                        default: item.Color = "#717171"; break;
                    }

                    sumatoria_cantidades += item.Cantidad;

                    item.Titulo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(item.Estado));
                    item.IdControl = string.Format("DocumentosEstado{0}{1}", item.Titulo, descripcion_tipo).Replace(" ", "").Replace("í", "i").Replace("ó", "o").Replace(",", "");
                    item.Observaciones = string.Format("{0} Documentos", item.Cantidad);
                    item.Porcentaje = Math.Round(((item.Cantidad / cantidad_total) * 100), 1);
                }

                //Construye el resumen de documentos recibidos por la plataforma.
                PorcentajesResumen resumen_inicio = new PorcentajesResumen();
                resumen_inicio.Cantidad = cantidad_total;
                resumen_inicio.Estado = 0;
                resumen_inicio.IdControl = string.Format("DocumentosEstadoRecibidos{0}", descripcion_tipo);
                resumen_inicio.Observaciones = string.Format("{0} Documentos", cantidad_total);
                resumen_inicio.Porcentaje = 100;
                resumen_inicio.Titulo = "Recibidos";
                resumen_inicio.Color = "#EEE713";
                documentos_estado.Add(resumen_inicio);

                return documentos_estado.OrderBy(x => x.Estado).ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene los porcentajes de documentos por estado.
        /// </summary>
        /// <param name="identificacion_empresa"></param>
        /// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
        /// <returns></returns>
        public List<PorcentajesResumen> DocumentosPorEstadoCategoria(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                List<PorcentajesResumen> documentos_estado = (from documento in context.TblDocumentos
                                                              where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
                                                              orderby documento.IdCategoriaEstado ascending
                                                              group documento by new { documento.IdCategoriaEstado } into documento_estado
                                                              select new PorcentajesResumen
                                                              {
                                                                  Cantidad = documento_estado.Count(),
                                                                  Estado = documento_estado.FirstOrDefault().IdCategoriaEstado,
                                                              }).ToList().ToList();

                decimal cantidad_total = documentos_estado.Sum(x => x.Cantidad);

                decimal sumatoria_cantidades = 0;

                string descripcion_tipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Perfiles>(tipo_empresa));

                foreach (PorcentajesResumen item in documentos_estado)
                {
                    switch (item.Estado)
                    {
                        case 400: item.Color = "#FF2D00"; break;
                        //case 99: item.Color = "#62C415"; break;
                        case 300:item.Color = "#62C415"; break;
                        default: item.Color = "#717171"; break;
                    }

                    sumatoria_cantidades += item.Cantidad;

                    item.Titulo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(item.Estado));
                    item.IdControl = string.Format("DocumentosEstado{0}{1}", item.Titulo, descripcion_tipo).Replace(" ", "").Replace("í", "i").Replace("ó", "o").Replace(",", "");
                    item.Observaciones = string.Format("{0} Documentos", item.Cantidad);
                    item.Porcentaje = Math.Round(((item.Cantidad / cantidad_total) * 100), 1);
                }

                //Construye el resumen de documentos recibidos por la plataforma.
                PorcentajesResumen resumen_inicio = new PorcentajesResumen();
                resumen_inicio.Cantidad = cantidad_total;
                resumen_inicio.Estado = 0;
                resumen_inicio.IdControl = string.Format("DocumentosEstadoRecibidos{0}", descripcion_tipo);
                resumen_inicio.Observaciones = string.Format("{0} Documentos", cantidad_total);
                resumen_inicio.Porcentaje = 100;
                resumen_inicio.Titulo = "Total";
                resumen_inicio.Color = "#EEE713";
                documentos_estado.Add(resumen_inicio);

                return documentos_estado.OrderBy(x => x.Estado).ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el indicador de las respuestas de acuse de los documentos.
        /// </summary>
        /// <param name="identificacion_empresa">Número de identificación de la empresa para el filtro de búsqueda.</param>
        /// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
        /// <param name="mensual">True: Reporte por mes actual - False: Reporte Acumulado</param>
        /// <returns></returns>
        public List<PorcentajesResumen> ReporteEstadosAcuse(string identificacion_empresa, int tipo_empresa, bool mensual)
        {
            try
            {

                int mes_actual = Fecha.GetFecha().Month;
                int anyo_actual = Fecha.GetFecha().Year;

                List<PorcentajesResumen> documentos_acuse = (from documento in context.TblDocumentos
                                                             where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
                                                              && ((mensual) ? (documento.DatFechaDocumento.Month == mes_actual && documento.DatFechaDocumento.Year == anyo_actual) : documento.DatFechaDocumento != null)
                                                             orderby documento.IntAdquirienteRecibo ascending
                                                             group documento by new { documento.IntAdquirienteRecibo } into documento_estado
                                                             select new PorcentajesResumen
                                                             {
                                                                 Cantidad = documento_estado.Count(),
                                                                 Estado = documento_estado.FirstOrDefault().IntAdquirienteRecibo
                                                             }).ToList().ToList();

                decimal cantidad_total = documentos_acuse.Sum(x => x.Cantidad);

                string descripcion_tipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Perfiles>(tipo_empresa));
                string rango_consulta = string.Empty;

                if (mensual)
                    rango_consulta = "Mensual";
                else
                    rango_consulta = "Acumulado";

                foreach (PorcentajesResumen item in documentos_acuse)
                {
                    switch (item.Estado)
                    {
                        case 0: item.Color = "#717171"; break;
                        case 1: item.Color = "#62C415"; break;
                        case 2: item.Color = "#FF2D00"; break;
                    }

                    item.Titulo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<AdquirienteRecibo>(item.Estado));
                    item.IdControl = string.Format("Acuse{0}{1}{2}", item.Titulo, descripcion_tipo, rango_consulta);
                    item.Observaciones = string.Format("{0} Documentos", item.Cantidad);
                    item.Porcentaje = Math.Round(((item.Cantidad / cantidad_total) * 100), 1);
                }

                return documentos_acuse;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el indicador del acumulado de los documentos por tipo
        /// </summary>
        /// <param name="identificacion_empresa">Número de identificación de la empresa para el filtro de búsqueda.</param>
        /// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
        /// <returns></returns>
        public List<PorcentajesResumen> DocumentosPorTipo(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                List<PorcentajesResumen> documentos_tipo = (from documento in context.TblDocumentos
                                                            where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
                                                            orderby documento.IntDocTipo ascending
                                                            group documento by new { documento.IntDocTipo } into documento_tipo
                                                            select new PorcentajesResumen
                                                            {
                                                                Cantidad = documento_tipo.Count(),
                                                                Estado = documento_tipo.FirstOrDefault().IntDocTipo
                                                            }).ToList().ToList();

                decimal cantidad_total = documentos_tipo.Sum(x => x.Cantidad);

                string descripcion_tipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Perfiles>(tipo_empresa));

                foreach (PorcentajesResumen item in documentos_tipo)
                {
                    item.Titulo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<LibreriaGlobalHGInet.Objetos.TipoDocumento>(item.Estado));
                    item.IdControl = string.Format("Documentos{0}{1}", item.Titulo, descripcion_tipo).Replace(" ", "").Replace("é", "e");
                    item.Observaciones = string.Format("{0} Documentos", item.Cantidad);
                    item.Porcentaje = Math.Round(((item.Cantidad / cantidad_total) * 100), 1);
                    item.Color = "717171";
                }

                return documentos_tipo.OrderBy(x => x.Estado).ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el indicador de documentos por tipo mensual durante los ultimos doce meses
        /// </summary>
        /// <param name="identificacion_empresa">Número de identificación de la empresa para el filtro de búsqueda.</param>
        /// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
        /// <returns></returns>
        public List<ValoresTipoDocumento> DocumentosPorTipoAnual(string identificacion_empresa, int tipo_empresa)
        {
            try
            {
                DateTime fecha_actual = Fecha.GetFecha().Date;
                DateTime inicio = new DateTime(fecha_actual.AddMonths(-12).Year, fecha_actual.AddMonths(-12).Month, 1).Date;

                List<ValoresTipoDocumento> documentos_tipo = (from documento in context.TblDocumentos
                                                              where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
                                                              && (documento.DatFechaIngreso >= inicio)
                                                              orderby documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.IntDocTipo ascending
                                                              group documento by new { documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month } into documento_tipo
                                                              select new ValoresTipoDocumento
                                                              {
                                                                  Anyo = documento_tipo.FirstOrDefault().DatFechaIngreso.Year,
                                                                  Mes = documento_tipo.FirstOrDefault().DatFechaIngreso.Month,
                                                                  CantidadFacturas = (documento_tipo.Where(x => x.IntDocTipo == 1).Count() > 0) ? documento_tipo.Where(x => x.IntDocTipo == 1).Count() : 0,
                                                                  CantidadNotasDebito = (documento_tipo.Where(x => x.IntDocTipo == 2).Count() > 0) ? documento_tipo.Where(x => x.IntDocTipo == 2).Count() : 0,
                                                                  CantidadNotasCredito = (documento_tipo.Where(x => x.IntDocTipo == 3).Count() > 0) ? documento_tipo.Where(x => x.IntDocTipo == 3).Count() : 0,
                                                                  ValorFacturas = (documento_tipo.Where(x => x.IntDocTipo == 1).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 1).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
                                                                  ValorNotasDebito = (documento_tipo.Where(x => x.IntDocTipo == 2).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 2).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
                                                                  ValorNotasCredito = (documento_tipo.Where(x => x.IntDocTipo == 3).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 3).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
                                                              }).ToList();


                List<ValoresTipoDocumento> datos_respuesta = new List<ValoresTipoDocumento>();

                foreach (var item in documentos_tipo)
                {
                    item.DescripcionMes = Fecha.MesLetras(new DateTime(item.Anyo, item.Mes, 1));
                }

                return documentos_tipo.OrderBy(x => x.Anyo).ThenBy(x => x.Mes).ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene los planes adquiridos por el facturador.
        /// </summary>
        /// <param name="identificacion_empresa"></param>
        /// <returns></returns>
        public List<ResumenPlanes> ResumenPlanesAdquiridos(string identificacion_empresa)
        {
            try
            {
                DateTime fecha_actual = Fecha.GetFecha();
                DateTime fecha_siguiente = Fecha.GetFecha().AddDays(1);

                List<ResumenPlanes> planes_adquiridos = (from plan in context.TblPlanesTransacciones
                                                         where plan.StrEmpresaFacturador.Equals(identificacion_empresa)
                                                         orderby plan.DatFecha descending
                                                         group plan by new { plan.StrEmpresaFacturador } into planes
                                                         select new ResumenPlanes
                                                         {
                                                             TransaccionesAdquiridas = (planes.Select(x => x.IntNumTransaccCompra).Sum() > 0) ? (planes.Select(x => x.IntNumTransaccCompra).Sum()) : 0,
                                                             TransaccionesProcesadas = (planes.Select(x => x.IntNumTransaccProcesadas).Sum() > 0) ? planes.Select(x => x.IntNumTransaccProcesadas).Sum() : 0,
                                                             // esta linea obtiene los planes vigentes o sin fechas de vencimientos y calcula las transacciones vigentes sobre el resultado, si la fecha de vencimiento es null, suma un día al día actual y lo toma en cuenta para el calculo.
                                                             TransaccionesDisponibles = (planes.Where(d => (d.DatFechaVencimiento.HasValue ? d.DatFechaVencimiento.Value : fecha_actual) > fecha_actual || d.DatFechaVencimiento == null).Count() > 0) ? planes.Where(d => (d.DatFechaVencimiento.HasValue ? d.DatFechaVencimiento.Value : fecha_siguiente) > fecha_actual).Select(d => d.IntNumTransaccCompra).Sum() - planes.Where(d => (d.DatFechaVencimiento.HasValue ? d.DatFechaVencimiento.Value : fecha_siguiente) > fecha_actual).Select(d => d.IntNumTransaccProcesadas).Sum() : 0,
                                                             PlanesAdquiridos = planes.Select(x => new { x.StrIdSeguridad, x.DatFecha, x.DatFechaVencimiento, x.IntNumTransaccCompra, x.IntNumTransaccProcesadas }).OrderByDescending(x => x.DatFecha).Take(5)
                                                         }).ToList();

                return planes_adquiridos;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el indicador de ventas durante los últimos doce meses.
        /// </summary>
        /// <returns></returns>
        public List<VentasMensuales> VentasAnuales()
        {
            try
            {
                DateTime fecha_actual = Fecha.GetFecha();
                DateTime inicio = new DateTime(fecha_actual.AddMonths(-12).Year, fecha_actual.AddMonths(-12).Month, 1);

                List<VentasMensuales> resumen_ventas = (from venta in context.TblPlanesTransacciones
                                                        where (venta.DatFecha >= inicio && venta.DatFecha <= fecha_actual)
                                                        orderby venta.DatFecha.Year, venta.DatFecha.Month, venta.IntTipoProceso ascending
                                                        group venta by new { venta.DatFecha.Year, venta.DatFecha.Month } into datos_ventas
                                                        select new VentasMensuales
                                                        {
                                                            Anyo = datos_ventas.FirstOrDefault().DatFecha.Year,
                                                            Mes = datos_ventas.FirstOrDefault().DatFecha.Month,
                                                            TipoProceso = datos_ventas.FirstOrDefault().IntTipoProceso,
                                                            CantidadTransaccionesCortesias = (datos_ventas.Where(x => x.IntTipoProceso == 1).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 1).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
                                                            CantidadTransaccionesVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
                                                            CantidadTransaccionesPostVenta = (datos_ventas.Where(x => x.IntTipoProceso == 3).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 3).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
                                                            ValorVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => x.IntValor).Sum() : 0
                                                        }).ToList();

                foreach (VentasMensuales item in resumen_ventas)
                {
                    item.DescripcionMes = Fecha.MesLetras(new DateTime(item.Anyo, item.Mes, 1));
                }

                return resumen_ventas.OrderBy(x => x.Anyo).ThenBy(x => x.Mes).ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el top 10 de los compradores.
        /// </summary>
        /// <returns></returns>
        public List<TopCompradores> TopCompradores()
        {
            try
            {
                List<TopCompradores> resumen_compradores = (from comprador in context.TblPlanesTransacciones
                                                            where comprador.IntTipoProceso != 1
                                                            group comprador by new { comprador.StrEmpresaFacturador } into datos_compradores
                                                            select new TopCompradores
                                                            {
                                                                Identificacion = datos_compradores.FirstOrDefault().StrEmpresaFacturador,
                                                                CantidadTransacciones = (decimal)datos_compradores.Select(d => d.IntNumTransaccCompra).Sum(),
                                                                ValorCompras = (decimal)datos_compradores.Select(d => d.IntValor).Sum(),
                                                            }).ToList().OrderByDescending(x => x.ValorCompras).Take(10).ToList();

                int i = 1;

                foreach (var item in resumen_compradores)
                {
                    i++;
                    item.Posicion = i;
                    item.RazonSocial = RazonSocialEmpresa(item.Identificacion);
                }

                return resumen_compradores;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene el top 10 de las empresas con mayor movimiento transaccional.
        /// </summary>
        /// <returns></returns>
        public List<TopTransaccional> TopTransaccional()
        {
            try
            {
                int mes_actual = Fecha.GetFecha().Month;
                int mes_anterior = Fecha.GetFecha().AddMonths(-1).Month;

                List<TopTransaccional> datos_respuesta = (from movimiento in context.TblDocumentos
                                                          where movimiento.DatFechaIngreso.Month >= mes_anterior
                                                          && (movimiento.IntIdEstado >= 7)
                                                          group movimiento by new { movimiento.StrEmpresaFacturador } into datos_movimiento
                                                          select new TopTransaccional
                                                          {
                                                              Identificacion = datos_movimiento.FirstOrDefault().StrEmpresaFacturador,
                                                              CantidadMesActual = (datos_movimiento.Where(d => d.DatFechaIngreso.Month == mes_actual).Count() > 0) ? datos_movimiento.Where(d => d.DatFechaIngreso.Month == mes_actual).Count() : 0,
                                                              CantidadMesAnterior = (datos_movimiento.Where(d => d.DatFechaIngreso.Month == mes_anterior).Count() > 0) ? datos_movimiento.Where(d => d.DatFechaIngreso.Month == mes_anterior).Count() : 0
                                                          }).ToList();

                foreach (TopTransaccional item in datos_respuesta)
                {
                    item.RazonSocial = RazonSocialEmpresa(item.Identificacion);
                }

                return datos_respuesta;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene la razón social de la empresa.
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        public string RazonSocialEmpresa(string identificacion)
        {
            try
            {
                return (from comprador in context.TblEmpresas
                        where comprador.StrIdentificacion.Equals(identificacion)
                        select comprador).FirstOrDefault().StrRazonSocial;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }



    }
}
