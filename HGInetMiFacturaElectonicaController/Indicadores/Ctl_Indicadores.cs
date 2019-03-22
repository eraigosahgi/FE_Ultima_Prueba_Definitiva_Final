using HGInetMiFacturaElectonicaController.Indicadores.Objetos;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Globalization;
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
		public List<PorcentajesResumen> DocumentosPorEstado(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				List<PorcentajesResumen> documentos_estado = (from documento in context.TblDocumentos
															  where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
															  && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
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
		public List<PorcentajesResumen> DocumentosPorEstadoCategoria(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				List<PorcentajesResumen> documentos_estado = (from documento in context.TblDocumentos
															  where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
															   && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
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
						case 300: item.Color = "#62C415"; break;
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
				resumen_inicio.Estado = -1;
				resumen_inicio.IdControl = string.Format("DocumentosEstadoCatRecibidos{0}", descripcion_tipo);
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
		public List<PorcentajesResumen> ReporteEstadosAcuse(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{

				int mes_actual = Fecha.GetFecha().Month;
				int anyo_actual = Fecha.GetFecha().Year;

				List<PorcentajesResumen> documentos_acuse = (from documento in context.TblDocumentos
															 where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
															 && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
															 orderby documento.IntAdquirienteRecibo ascending
															 group documento by new { documento.IntAdquirienteRecibo } into documento_estado
															 select new PorcentajesResumen
															 {
																 Cantidad = documento_estado.Count(),
																 Estado = documento_estado.FirstOrDefault().IntAdquirienteRecibo
															 }).ToList().ToList();

				decimal cantidad_total = documentos_acuse.Sum(x => x.Cantidad);

				string descripcion_tipo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Perfiles>(tipo_empresa));

				foreach (PorcentajesResumen item in documentos_acuse)
				{
					switch (item.Estado)
					{
						case 0: item.Color = "#717171"; break;
						case 1: item.Color = "#62C415"; break;
						case 2: item.Color = "#FF2D00"; break;
						case 3: item.Color = "#96f495"; break;
					}
					item.Titulo = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<AdquirienteRecibo>(item.Estado));
					item.IdControl = string.Format("Acuse{0}{1}", item.Titulo.Replace(" ", "").Replace("á", "a"), descripcion_tipo);
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
		public List<PorcentajesResumen> DocumentosPorTipo(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				List<PorcentajesResumen> documentos_tipo = (from documento in context.TblDocumentos
															where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
															 && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
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
		public List<ValoresTipoDocumento> DocumentosPorTipoAnual(string identificacion_empresa, int tipo_empresa, DateTime fecha_inicio, DateTime fecha_fin, TipoFrecuencia tipo_filtro)
		{
			try
			{
				List<ValoresTipoDocumento> documentos_tipo = new List<ValoresTipoDocumento>();

				if (TipoFrecuencia.Anyo.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Rango.GetHashCode() == tipo_filtro.GetHashCode())
				{

					documentos_tipo = (from documento in context.TblDocumentos
									   where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
									   && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
									   orderby documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.IntDocTipo ascending
									   group documento by new { documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month } into documento_tipo
									   select new ValoresTipoDocumento
									   {
										   FechaCompleta = documento_tipo.FirstOrDefault().DatFechaIngreso,
										   CantidadDocumentos = documento_tipo.Count(),
										   ValorFacturas = (documento_tipo.Where(x => x.IntDocTipo == 1).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 1).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
										   ValorNotasDebito = (documento_tipo.Where(x => x.IntDocTipo == 2).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 2).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
										   ValorNotasCredito = (documento_tipo.Where(x => x.IntDocTipo == 3).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 3).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
									   }).OrderBy(x => x.FechaCompleta).ToList();
				}

				else if (TipoFrecuencia.Mes.GetHashCode() == tipo_filtro.GetHashCode())
				{
					documentos_tipo = (from documento in context.TblDocumentos
									   where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
									   && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
									   orderby documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.IntDocTipo ascending
									   group documento by new { documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.DatFechaIngreso.Day } into documento_tipo
									   select new ValoresTipoDocumento
									   {
										   FechaCompleta = documento_tipo.FirstOrDefault().DatFechaIngreso,
										   CantidadDocumentos = documento_tipo.Count(),
										   ValorFacturas = (documento_tipo.Where(x => x.IntDocTipo == 1).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 1).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
										   ValorNotasDebito = (documento_tipo.Where(x => x.IntDocTipo == 2).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 2).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
										   ValorNotasCredito = (documento_tipo.Where(x => x.IntDocTipo == 3).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 3).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
									   }).ToList();
				}

				else if (TipoFrecuencia.Hoy.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Fecha.GetHashCode() == tipo_filtro.GetHashCode())
				{
					documentos_tipo = (from documento in context.TblDocumentos
									   where ((tipo_empresa == 2) ? documento.StrEmpresaFacturador.Equals(identificacion_empresa) : (tipo_empresa == 3) ? documento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : documento.StrEmpresaFacturador != null)
									   && (documento.DatFechaIngreso >= fecha_inicio.Date && documento.DatFechaIngreso <= fecha_fin.Date)
									   orderby documento.DatFechaIngreso.Year, documento.DatFechaIngreso.Month, documento.IntDocTipo ascending
									   group documento by new { documento.DatFechaIngreso.Hour } into documento_tipo
									   select new ValoresTipoDocumento
									   {
										   FechaCompleta = documento_tipo.FirstOrDefault().DatFechaIngreso,
										   CantidadDocumentos = documento_tipo.Count(),
										   ValorFacturas = (documento_tipo.Where(x => x.IntDocTipo == 1).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 1).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
										   ValorNotasDebito = (documento_tipo.Where(x => x.IntDocTipo == 2).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 2).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
										   ValorNotasCredito = (documento_tipo.Where(x => x.IntDocTipo == 3).Count() > 0) ? (documento_tipo.Where(x => x.IntDocTipo == 3).Select(x => (decimal)x.IntVlrTotal).Sum()) : 0,
									   }).OrderBy(x => x.FechaCompleta).ToList();
				}

				List<ValoresTipoDocumento> datos_respuesta = new List<ValoresTipoDocumento>();

				int i = 0;

				foreach (var item in documentos_tipo)
				{
					i++;
					if (TipoFrecuencia.Mes.GetHashCode() == tipo_filtro.GetHashCode())
					{
						item.DescripcionSerie = string.Format("{0} {1}", Fecha.DiaLetras(item.FechaCompleta), item.FechaCompleta.Day);
					}
					else if (TipoFrecuencia.Anyo.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Rango.GetHashCode() == tipo_filtro.GetHashCode())
					{
						item.DescripcionSerie = string.Format("{0} {1}", Fecha.MesLetras(item.FechaCompleta), item.FechaCompleta.Year);
					}
					else if (TipoFrecuencia.Hoy.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Fecha.GetHashCode() == tipo_filtro.GetHashCode())
					{
						item.DescripcionSerie = item.FechaCompleta.ToString("hh:00 tt");
					}
				}

				return documentos_tipo;
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
		public List<ResumenPlanes> ResumenPlanesAdquiridos(string identificacion_empresa, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				DateTime fecha_actual = Fecha.GetFecha();
				DateTime fecha_siguiente = Fecha.GetFecha().AddDays(1);

				List<ResumenPlanes> planes_adquiridos = (from plan in context.TblPlanesTransacciones
														 where plan.StrEmpresaFacturador.Equals(identificacion_empresa)
														  && (plan.DatFecha >= fecha_inicio.Date && plan.DatFecha <= fecha_fin.Date)
														 orderby plan.DatFecha descending
														 group plan by new { plan.StrEmpresaFacturador } into planes
														 select new ResumenPlanes
														 {
															 TransaccionesAdquiridas = (planes.Select(x => x.IntNumTransaccCompra).Sum() > 0) ? (planes.Select(x => x.IntNumTransaccCompra).Sum()) : 0,
															 TransaccionesProcesadas = (planes.Select(x => x.IntNumTransaccProcesadas).Sum() > 0) ? planes.Select(x => x.IntNumTransaccProcesadas).Sum() : 0,
															 // esta linea obtiene los planes vigentes o sin fechas de vencimientos y calcula las transacciones vigentes sobre el resultado, si la fecha de vencimiento es null, suma un día al día actual y lo toma en cuenta para el calculo.
															 TransaccionesDisponibles = (planes.Where(d => (d.DatFechaVencimiento.HasValue ? d.DatFechaVencimiento.Value : fecha_actual) > fecha_actual || d.DatFechaVencimiento == null).Count() > 0) ? planes.Where(d => (d.DatFechaVencimiento.HasValue ? d.DatFechaVencimiento.Value : fecha_siguiente) > fecha_actual).Select(d => d.IntNumTransaccCompra).Sum() - planes.Where(d => (d.DatFechaVencimiento.HasValue ? d.DatFechaVencimiento.Value : fecha_siguiente) > fecha_actual).Select(d => d.IntNumTransaccProcesadas).Sum() : 0,
															 PlanesAdquiridos = planes.Select(x => new
															 {
																 x.StrIdSeguridad,
																 x.DatFecha,
																 x.DatFechaVencimiento,
																 x.IntNumTransaccCompra,
																 x.IntNumTransaccProcesadas,
																 CodCompra = x.IntTipoProceso,
																 Porcentaje = (x.IntNumTransaccProcesadas == 0) ? 0 : Math.Round(((float)x.IntNumTransaccProcesadas / (float)x.IntNumTransaccCompra) * 100, 2),
																 porcentajeFecha = (x.DatFechaVencimiento != null) ? Math.Round((double)Math.Round((double)SqlFunctions.DateDiff("dd", x.DatFecha, fecha_actual)) / (double)Math.Round((double)SqlFunctions.DateDiff("dd", x.DatFecha, x.DatFechaVencimiento)) * 100, 2) : 0,
																 x.IntEstado
															 }).OrderByDescending(x => x.DatFecha).Take(5)
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
		public List<VentasMensuales> Ventas(DateTime fecha_inicio, DateTime fecha_fin, TipoFrecuencia tipo_filtro)
		{
			try
			{
				List<VentasMensuales> resumen_ventas = new List<VentasMensuales>();

				if (TipoFrecuencia.Anyo.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Rango.GetHashCode() == tipo_filtro.GetHashCode())
				{

					resumen_ventas = (from venta in context.TblPlanesTransacciones
									  where (venta.DatFecha >= fecha_inicio.Date && venta.DatFecha <= fecha_fin.Date)
									  orderby venta.DatFecha.Year, venta.DatFecha.Month, venta.IntTipoProceso ascending
									  group venta by new { venta.DatFecha.Year, venta.DatFecha.Month } into datos_ventas
									  select new VentasMensuales
									  {
										  FechaCompleta = datos_ventas.FirstOrDefault().DatFecha,
										  TipoProceso = datos_ventas.FirstOrDefault().IntTipoProceso,
										  CantidadTransaccionesCortesias = (datos_ventas.Where(x => x.IntTipoProceso == 1).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 1).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  CantidadTransaccionesVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  CantidadTransaccionesPostVenta = (datos_ventas.Where(x => x.IntTipoProceso == 3).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 3).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  ValorVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => x.IntValor).Sum() : 0
									  }).OrderBy(x => x.FechaCompleta).ToList();
				}

				else if (TipoFrecuencia.Mes.GetHashCode() == tipo_filtro.GetHashCode())
				{

					resumen_ventas = (from venta in context.TblPlanesTransacciones
									  where (venta.DatFecha >= fecha_inicio.Date && venta.DatFecha <= fecha_fin.Date)
									  orderby venta.DatFecha.Year, venta.DatFecha.Month, venta.IntTipoProceso ascending
									  group venta by new { venta.DatFecha.Year, venta.DatFecha.Month, venta.DatFecha.Day } into datos_ventas
									  select new VentasMensuales
									  {
										  FechaCompleta = datos_ventas.FirstOrDefault().DatFecha,
										  TipoProceso = datos_ventas.FirstOrDefault().IntTipoProceso,
										  CantidadTransaccionesCortesias = (datos_ventas.Where(x => x.IntTipoProceso == 1).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 1).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  CantidadTransaccionesVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  CantidadTransaccionesPostVenta = (datos_ventas.Where(x => x.IntTipoProceso == 3).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 3).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  ValorVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => x.IntValor).Sum() : 0
									  }).OrderBy(x => x.FechaCompleta).ToList();

				}

				else if (TipoFrecuencia.Hoy.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Fecha.GetHashCode() == tipo_filtro.GetHashCode())
				{

					resumen_ventas = (from venta in context.TblPlanesTransacciones
									  where (venta.DatFecha >= fecha_inicio.Date && venta.DatFecha <= fecha_fin.Date)
									  orderby venta.DatFecha.Year, venta.DatFecha.Month, venta.IntTipoProceso ascending
									  group venta by new { venta.DatFecha.Hour } into datos_ventas
									  select new VentasMensuales
									  {
										  FechaCompleta = datos_ventas.FirstOrDefault().DatFecha,
										  TipoProceso = datos_ventas.FirstOrDefault().IntTipoProceso,
										  CantidadTransaccionesCortesias = (datos_ventas.Where(x => x.IntTipoProceso == 1).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 1).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  CantidadTransaccionesVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  CantidadTransaccionesPostVenta = (datos_ventas.Where(x => x.IntTipoProceso == 3).Count() > 0) ? (datos_ventas.Where(x => x.IntTipoProceso == 3).Select(x => (decimal)x.IntNumTransaccCompra).Sum()) : 0,
										  ValorVentas = (datos_ventas.Where(x => x.IntTipoProceso == 2).Count() > 0) ? datos_ventas.Where(x => x.IntTipoProceso == 2).Select(x => x.IntValor).Sum() : 0
									  }).OrderBy(x => x.FechaCompleta).ToList();
				}

				int i = 0;

				foreach (var item in resumen_ventas)
				{
					i++;
					if (TipoFrecuencia.Mes.GetHashCode() == tipo_filtro.GetHashCode())
					{
						item.DescripcionSerie = string.Format("{0} {1}", Fecha.DiaLetras(item.FechaCompleta), item.FechaCompleta.Day);
					}
					else if (TipoFrecuencia.Anyo.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Rango.GetHashCode() == tipo_filtro.GetHashCode())
					{
						item.DescripcionSerie = string.Format("{0} {1}", Fecha.MesLetras(item.FechaCompleta), item.FechaCompleta.Year);
					}
					else if (TipoFrecuencia.Hoy.GetHashCode() == tipo_filtro.GetHashCode() || TipoFrecuencia.Fecha.GetHashCode() == tipo_filtro.GetHashCode())
					{
						item.DescripcionSerie = item.FechaCompleta.ToString("hh:00 tt");
					}
				}

				return resumen_ventas;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el top 10 de los compradores.
		/// </summary>
		/// <param name="cantidad_top">Indica la cantidad de registros a retornas, 0 si se desean retornar todos </param>
		/// <returns></returns>
		public List<TopCompradores> TopCompradores(DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				List<TopCompradores> resumen_compradores = (from comprador in context.TblPlanesTransacciones
															where comprador.IntTipoProceso != 1
															&& (comprador.DatFecha >= fecha_inicio.Date && comprador.DatFecha <= fecha_fin.Date)
															group comprador by new { comprador.StrEmpresaFacturador } into datos_compradores
															select new TopCompradores
															{
																Identificacion = datos_compradores.FirstOrDefault().StrEmpresaFacturador,
																CantidadTransacciones = datos_compradores.Select(d => d.IntNumTransaccCompra).Sum(),
																ValorCompras = datos_compradores.Select(d => d.IntValor).Sum(),
																RazonSocial = datos_compradores.FirstOrDefault().TblEmpresas.StrRazonSocial
															}).ToList().OrderByDescending(x => x.ValorCompras).ToList();

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
		/// <param name="cantidad_top">Indica la cantidad de registros a retornas, 0 si se desean retornar todos </param>
		/// <param name="tipo_empresa">1: Administrador - 2: Facturador - 3: Adquiriente</param>
		/// <returns></returns>
		public List<TopTransaccional> OtenerTopTransaccional(DateTime fecha_inicio, DateTime fecha_fin, int tipo_empresa, string identificacion_empresa, TipoFrecuencia tipo_filtro)
		{
			try
			{

				int mes_actual = Fecha.GetFecha().Month;
				int mes_anterior = new DateTime(Fecha.GetFecha().Year, Fecha.GetFecha().Month, 1).AddMonths(-1).Month;

				List<TopTransaccional> resumen_transaccional = new List<TopTransaccional>();

				if (tipo_empresa == 2)
				{
					resumen_transaccional = (from movimiento in context.TblDocumentos
											 join empresa in context.TblEmpresas on movimiento.StrEmpresaAdquiriente equals empresa.StrIdentificacion
											 where (movimiento.DatFechaIngreso >= fecha_inicio.Date && movimiento.DatFechaIngreso <= fecha_fin.Date)
											 && movimiento.StrEmpresaFacturador.Equals(identificacion_empresa)
											 group new { empresa, movimiento } by new { movimiento.StrEmpresaAdquiriente } into datos_movimiento
											 select new TopTransaccional
											 {
												 Identificacion = datos_movimiento.FirstOrDefault().movimiento.StrEmpresaAdquiriente,
												 RazonSocial = datos_movimiento.FirstOrDefault().empresa.StrRazonSocial,
												 ValorTotalDocumentos = datos_movimiento.Sum(x => (x.movimiento.IntDocTipo == 3) ? -x.movimiento.IntVlrTotal : x.movimiento.IntVlrTotal),
												 TotalDocumentos = datos_movimiento.Count(),
											 }).OrderByDescending(d => d.TotalDocumentos).ToList();
				}
				else
				{
					resumen_transaccional = (from movimiento in context.TblDocumentos
											 join empresa in context.TblEmpresas on movimiento.StrEmpresaFacturador equals empresa.StrIdentificacion
											 where (movimiento.DatFechaIngreso >= fecha_inicio.Date && movimiento.DatFechaIngreso <= fecha_fin.Date)
											 && ((tipo_empresa == 3) ? movimiento.StrEmpresaAdquiriente.Equals(identificacion_empresa) : movimiento.StrEmpresaFacturador != null)
											 group new { empresa, movimiento } by new { movimiento.StrEmpresaFacturador } into datos_movimiento
											 select new TopTransaccional
											 {
												 Identificacion = datos_movimiento.FirstOrDefault().movimiento.StrEmpresaFacturador,
												 RazonSocial = datos_movimiento.FirstOrDefault().empresa.StrRazonSocial,
												 ValorTotalDocumentos = datos_movimiento.Sum(x => (x.movimiento.IntDocTipo == 3) ? -x.movimiento.IntVlrTotal : x.movimiento.IntVlrTotal),
												 TotalDocumentos = datos_movimiento.Count()
											 }).OrderByDescending(d => d.TotalDocumentos).ToList();
				}




				return resumen_transaccional;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public List<TopTransaccional> TopTransaccional(DateTime fecha_inicio, DateTime fecha_fin, int tipo_empresa, string identificacion_empresa, TipoFrecuencia tipo_frecuencia)
		{
			try
			{
				List<TopTransaccional> datos_actual = new List<Objetos.TopTransaccional>();
				List<TopTransaccional> datos_anterior = new List<Objetos.TopTransaccional>();

				DateTime fecha_inicio_anterior = fecha_inicio;
				DateTime fecha_fin_anterior = fecha_fin;


				switch (tipo_frecuencia)
				{
					case TipoFrecuencia.Hoy:
						//Obtiene el actual.
						datos_actual = OtenerTopTransaccional(fecha_inicio, fecha_fin, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						fecha_inicio_anterior = fecha_inicio.AddDays(-1);
						fecha_fin_anterior = fecha_fin.AddDays(-1);
						//obtiene el anterior
						datos_anterior = OtenerTopTransaccional(fecha_inicio_anterior, fecha_fin_anterior, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						foreach (var item in datos_actual)
						{
							TopTransaccional top_anterior = datos_anterior.Where(x => x.Identificacion.Equals(item.Identificacion)).FirstOrDefault();

							item.CantidadActual = item.TotalDocumentos;

							if (top_anterior != null)
								item.CantidadAnterior = top_anterior.TotalDocumentos;
						}

						break;

					case TipoFrecuencia.Fecha:
						//Obtiene el actual.
						datos_actual = OtenerTopTransaccional(fecha_inicio, fecha_fin, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						fecha_inicio_anterior = fecha_inicio.AddDays(-1);
						fecha_fin_anterior = fecha_fin.AddDays(-1);
						//obtiene el anterior
						datos_anterior = OtenerTopTransaccional(fecha_inicio_anterior, fecha_fin_anterior, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						foreach (var item in datos_actual)
						{
							TopTransaccional top_anterior = datos_anterior.Where(x => x.Identificacion.Equals(item.Identificacion)).FirstOrDefault();

							item.CantidadActual = item.TotalDocumentos;

							if (top_anterior != null)
								item.CantidadAnterior = top_anterior.TotalDocumentos;
						}
						break;

					case TipoFrecuencia.Semana:
						//NO HABILITADO
						break;

					case TipoFrecuencia.Mes:
						//Obtiene el actual.
						datos_actual = OtenerTopTransaccional(fecha_inicio, fecha_fin, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						fecha_inicio_anterior = fecha_inicio.AddMonths(-1);
						fecha_fin_anterior = fecha_fin.AddMonths(-1);
						//obtiene el anterior
						datos_anterior = OtenerTopTransaccional(fecha_inicio_anterior, fecha_fin_anterior, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						foreach (var item in datos_actual)
						{
							TopTransaccional top_anterior = datos_anterior.Where(x => x.Identificacion.Equals(item.Identificacion)).FirstOrDefault();

							item.CantidadActual = item.TotalDocumentos;

							if (top_anterior != null)
								item.CantidadAnterior = top_anterior.TotalDocumentos;
						}

						break;

					case TipoFrecuencia.Anyo:
						//Obtiene el actual.
						datos_actual = OtenerTopTransaccional(fecha_inicio, fecha_fin, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						fecha_inicio_anterior = fecha_inicio.AddYears(-1);
						fecha_fin_anterior = fecha_fin.AddYears(-1);
						//obtiene el anterior
						datos_anterior = OtenerTopTransaccional(fecha_inicio_anterior, fecha_fin_anterior, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						foreach (var item in datos_actual)
						{
							TopTransaccional top_anterior = datos_anterior.Where(x => x.Identificacion.Equals(item.Identificacion)).FirstOrDefault();

							item.CantidadActual = item.TotalDocumentos;

							if (top_anterior != null)
								item.CantidadAnterior = top_anterior.TotalDocumentos;
						}
						break;

					case TipoFrecuencia.Rango:
						//Obtiene el actual.
						datos_actual = OtenerTopTransaccional(fecha_inicio, fecha_fin, tipo_empresa, identificacion_empresa, tipo_frecuencia);

						int dias = (fecha_inicio - fecha_fin).Days;

						fecha_inicio_anterior = fecha_inicio.AddDays(dias);
						fecha_fin_anterior = fecha_fin.AddDays(dias);
						//obtiene el anterior
						datos_anterior = OtenerTopTransaccional(fecha_inicio_anterior, fecha_fin_anterior, tipo_empresa, identificacion_empresa, tipo_frecuencia);


						foreach (var item in datos_actual)
						{
							TopTransaccional top_anterior = datos_anterior.Where(x => x.Identificacion.Equals(item.Identificacion)).FirstOrDefault();

							item.CantidadActual = item.TotalDocumentos;

							if (top_anterior != null)
								item.CantidadAnterior = top_anterior.TotalDocumentos;
						}
						break;
				}

				if (datos_actual.Count > 0)
				{
					TopTransaccional item = datos_actual.FirstOrDefault();


					if (TipoFrecuencia.Hoy.GetHashCode() == tipo_frecuencia.GetHashCode() || TipoFrecuencia.Fecha.GetHashCode() == tipo_frecuencia.GetHashCode())
					{
						item.DescripcionActual = string.Format("{0} {1}", Fecha.MesLetras(fecha_inicio), fecha_inicio.Day);
						item.DescripcionAnterior = string.Format("{0} {1}", Fecha.MesLetras(fecha_inicio_anterior), fecha_inicio_anterior.Day);
					}
					else if (TipoFrecuencia.Mes.GetHashCode() == tipo_frecuencia.GetHashCode())
					{
						item.DescripcionActual = Fecha.MesLetras(fecha_inicio);
						item.DescripcionAnterior = Fecha.MesLetras(fecha_inicio_anterior);
					}
					else if (TipoFrecuencia.Anyo.GetHashCode() == tipo_frecuencia.GetHashCode())
					{
						item.DescripcionActual = fecha_inicio.Year.ToString();
						item.DescripcionAnterior = fecha_inicio_anterior.Year.ToString();
					}
				}


				return datos_actual;
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
