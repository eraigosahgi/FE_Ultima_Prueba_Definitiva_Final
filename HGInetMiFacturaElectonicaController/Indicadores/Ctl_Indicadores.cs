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

                return documentos_tipo;
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
                DateTime fecha_actual = Fecha.GetFecha();
                DateTime inicio = new DateTime(fecha_actual.AddMonths(-12).Year, fecha_actual.AddMonths(-12).Month, 1);

                List<ValoresTipoDocumento> documentos_tipo = (from documento in context.TblDocumentos
                                                              where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
                                                              && (documento.DatFechaIngreso >= inicio && documento.DatFechaIngreso <= fecha_actual)
                                                              orderby documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.IntDocTipo ascending
                                                              group documento by new { documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.IntDocTipo } into documento_tipo
                                                              select new ValoresTipoDocumento
                                                              {
                                                                  Anyo = documento_tipo.FirstOrDefault().DatFechaIngreso.Year,
                                                                  Mes = documento_tipo.FirstOrDefault().DatFechaIngreso.Month,
                                                                  TipoDoc = documento_tipo.FirstOrDefault().IntDocTipo,
                                                                  Cantidad = documento_tipo.Count(),
                                                                  Valor = documento_tipo.Sum(x => x.IntVlrTotal)
                                                              }).ToList();


                List<ValoresTipoDocumento> datos_respuesta = new List<ValoresTipoDocumento>();



                foreach (var item in documentos_tipo)
                {
                    ValoresTipoDocumento datos_valores = new ValoresTipoDocumento();

                    datos_valores.Mes = item.Mes;
                    datos_valores.DescripcionMes = Fecha.MesLetras(new DateTime(item.Anyo, item.Mes, 1));
                    datos_valores.Anyo = item.Anyo;

                    var item_retorno = datos_respuesta.Where(x => x.Anyo == item.Anyo && x.Mes == item.Mes).FirstOrDefault();

                    if (item_retorno != null)
                    {
                        datos_valores = item_retorno;
                        datos_respuesta.Remove(item_retorno);
                    }

                    switch (item.TipoDoc)
                    {
                        case 1:
                            datos_valores.CantidadFacturas = item.Cantidad;
                            datos_valores.ValorFacturas = item.Valor;
                            break;
                        case 2:
                            datos_valores.CantidadNotasCredito = item.Cantidad;
                            datos_valores.ValorNotasCredito = item.Valor;
                            break;
                        case 3:
                            datos_valores.CantidadNotasDebito = item.Cantidad;
                            datos_valores.ValorNotasDebito = Math.Round(item.Valor, 0);
                            break;
                    }

                    datos_respuesta.Add(datos_valores);
                }

                return datos_respuesta.OrderBy(x => x.Anyo).ThenBy(x => x.Mes).ToList();
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
        public List<dynamic> ResumenPlanesAdquiridos(string identificacion_empresa)
        {
            try
            {
                DateTime fecha_actual = Fecha.GetFecha();

                var planes_adquiridos = (from plan in context.TblPlanesTransacciones
                                         where plan.StrEmpresaFacturador.Equals(identificacion_empresa)
                                         && SqlFunctions.DateAdd("month", 12, plan.DatFecha).Value > fecha_actual
                                         orderby plan.DatFecha descending
                                         select plan).ToList();

                var resumen_planes = planes_adquiridos.Select(x => new
                {
                    IdSeguridad = x.StrIdSeguridad,
                    FechaCompra = x.DatFecha,
                    FechaVencimiento = x.DatFecha.AddMonths(12),
                    CantidadCompradas = x.IntNumTransaccCompra,
                    CantidadProcesadas = x.IntNumTransaccProcesadas
                });

                var plan_actual = resumen_planes.Where(x => x.FechaVencimiento > fecha_actual && x.CantidadProcesadas != 0 && (x.CantidadProcesadas < x.CantidadCompradas)).FirstOrDefault();

                List<dynamic> retorno = new List<dynamic>();
                dynamic resumen = new System.Dynamic.ExpandoObject();
                if (plan_actual != null)
                {
                    resumen.PlanesAdquiridos = resumen_planes;
                    resumen.SaldoPlanActual = plan_actual.CantidadCompradas;
                    resumen.SaldoConsumoPlanActual = plan_actual.CantidadProcesadas;
                    resumen.SaldoCompras = resumen_planes.Where(x => x.IdSeguridad != plan_actual.IdSeguridad).Sum(x => x.CantidadCompradas);
                    resumen.SaldoDisponible = resumen.SaldoCompras + (plan_actual.CantidadCompradas - plan_actual.CantidadProcesadas);
                }
                else
                {
                    resumen.PlanesAdquiridos = 0;
                    resumen.SaldoPlanActual = 0;
                    resumen.SaldoConsumoPlanActual = 0;
                    resumen.SaldoCompras = 0;
                    resumen.SaldoDisponible = 0;
                }
                retorno.Add(resumen);

                return retorno;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        public List<ValoresTipoDocumento> VentasAnual()
        {
            try
            {
                DateTime fecha_actual = Fecha.GetFecha();
                DateTime inicio = new DateTime(fecha_actual.AddMonths(-12).Year, fecha_actual.AddMonths(-12).Month, 1);

                List<ValoresTipoDocumento> documentos_tipo = (from documento in context.TblPlanesTransacciones
                                                              where (documento.DatFecha >= inicio && documento.DatFecha <= fecha_actual)
                                                              orderby documento.DatFecha.Year, documento.DatFecha.Month, documento.IntTipoProceso ascending
                                                              group documento by new { documento.DatFecha.Year, documento.DatFecha.Month, documento.IntTipoProceso } into documento_tipo
                                                              select new ValoresTipoDocumento
                                                              {
                                                                  Anyo = documento_tipo.FirstOrDefault().DatFecha.Year,
                                                                  Mes = documento_tipo.FirstOrDefault().DatFecha.Month,
                                                                  TipoDoc = documento_tipo.FirstOrDefault().IntTipoProceso,
                                                                  Cantidad = documento_tipo.Count(),
                                                                  Valor = documento_tipo.Sum(x => x.IntValor)
                                                              }).ToList();


                List<ValoresTipoDocumento> datos_respuesta = new List<ValoresTipoDocumento>();



                foreach (var item in documentos_tipo)
                {
                    ValoresTipoDocumento datos_valores = new ValoresTipoDocumento();

                    datos_valores.Mes = item.Mes;
                    datos_valores.DescripcionMes = Fecha.MesLetras(new DateTime(item.Anyo, item.Mes, 1));
                    datos_valores.Anyo = item.Anyo;

                    var item_retorno = datos_respuesta.Where(x => x.Anyo == item.Anyo && x.Mes == item.Mes).FirstOrDefault();

                    if (item_retorno != null)
                    {
                        datos_valores = item_retorno;
                        datos_respuesta.Remove(item_retorno);
                    }

                    switch (item.TipoDoc)
                    {
                        case 1:
                            datos_valores.CantidadFacturas = item.Cantidad;
                            datos_valores.ValorFacturas = item.Valor;
                            break;
                        case 2:
                            datos_valores.CantidadNotasCredito = item.Cantidad;
                            datos_valores.ValorNotasCredito = item.Valor;
                            break;
                        case 3:
                            datos_valores.CantidadNotasDebito = item.Cantidad;
                            datos_valores.ValorNotasDebito = Math.Round(item.Valor, 0);
                            break;
                    }

                    datos_respuesta.Add(datos_valores);
                }

                return datos_respuesta.OrderBy(x => x.Anyo).ThenBy(x => x.Mes).ToList();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
    }
}
