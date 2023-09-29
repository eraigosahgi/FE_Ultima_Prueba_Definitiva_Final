using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
		public TblPlanesTransacciones Crear(TblPlanesTransacciones datos_plan, bool Envia_email = true)
		{
			try
			{
				datos_plan.DatFecha = Fecha.GetFecha();
				datos_plan.StrIdSeguridad = Guid.NewGuid();

				datos_plan = this.Add(datos_plan);

				Ctl_Empresa empresa = new Ctl_Empresa();

				TblEmpresas facturador = empresa.Obtener(datos_plan.StrEmpresaFacturador);

				try
				{
					if (Envia_email && datos_plan.IntEstado == EstadoPlan.Habilitado.GetHashCode())
					{
						Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

						email.EnviaNotificacionRecarga(facturador.StrIdentificacion, facturador.StrMailAdmin, datos_plan);
					}
				}
				catch (Exception excepcion)
				{
					Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
				}

				///Se hace ejecución de reinicio de alertas de porcentaje.
				///Basicamente se cambia de estatus dicha alerta en mongodb para que el proceso de consumo pueda ir nuevamente a consultar si ya se notifico.
				///No se coloca en el editar ya que este no permite editar cantidad de documentos.
				try
				{
					if (datos_plan.IntEstado == EstadoPlan.Habilitado.GetHashCode())
					{
						Ctl_AlertasHistAudit _AlertasHist = new Ctl_AlertasHistAudit();
						_AlertasHist.ReiniciarAlertaPorcentaje(facturador.StrIdSeguridad);
					}
				}
				catch (Exception excepcion)
				{
					Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
				}

				return datos_plan;
			}
			catch (Exception ex)
			{
				Ctl_Log.Guardar(ex, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion);
				throw;
			}
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
			int estadoplanactual = Ptransaccion.IntEstado;

			Ptransaccion.IntTipoProceso = datos_plan.IntTipoProceso;
			Ptransaccion.IntNumTransaccCompra = datos_plan.IntNumTransaccCompra;
			Ptransaccion.IntValor = datos_plan.IntValor;
			Ptransaccion.IntEstado = datos_plan.IntEstado;
			Ptransaccion.StrObservaciones = datos_plan.StrObservaciones + "\nMODIFICACIÓN: " + Fecha.GetFecha() + " - USUARIO: " + datos_plan.StrUsuario;
			Ptransaccion.StrEmpresaFacturador = datos_plan.StrEmpresaFacturador.Trim();
			Ptransaccion.DocumentoRef = datos_plan.DocumentoRef;
			Ptransaccion.IntMesesVence = datos_plan.IntMesesVence;
			Ptransaccion.DatFechaVencimiento = datos_plan.DatFechaVencimiento;
			Ptransaccion.DatFechaVencimiento = Ptransaccion.DatFechaVencimiento;
			Ptransaccion.IntTipoDocumento = datos_plan.IntTipoDocumento;

			if (Ptransaccion.IntMesesVence > 0 && Ptransaccion.DatFechaInicio != null)
			{
				Ptransaccion.DatFechaVencimiento = Ptransaccion.DatFechaInicio.Value.AddMonths(Ptransaccion.IntMesesVence);
			}

			Ptransaccion = this.Edit(Ptransaccion);

			try
			{
				if (estadoplanactual != EstadoPlan.Habilitado.GetHashCode() && Ptransaccion.IntEstado == EstadoPlan.Habilitado.GetHashCode())
				{
					Ctl_AlertasHistAudit _AlertasHist = new Ctl_AlertasHistAudit();
					_AlertasHist.ReiniciarAlertaPorcentaje(Ptransaccion.TblEmpresas.StrIdSeguridad);
				}
			}
			catch (Exception)
			{
			}


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
		/// Obtener el plan 
		/// </summary>
		/// <param name="identificacion">identificacion</param>
		/// <param name="fecha_vencimiento">Fecha de vencimiento del plan</param>
		/// <param name="cantidad">Cantidad de documentos</param>
		/// <returns>TblPlanesTransacciones</returns>
		public TblPlanesTransacciones ObtenerPorFechaCantidad(string identificacion, DateTime fecha_vencimiento, int cantidad)
		{
			TblPlanesTransacciones datos_plan = new TblPlanesTransacciones();
			var fecha_inicio = fecha_vencimiento.Date;

			var fecha_fin = new DateTime(fecha_vencimiento.Year, fecha_vencimiento.Month, fecha_vencimiento.Day, 23, 59, 59, 999);

			datos_plan = (from t in context.TblPlanesTransacciones
						  where (t.StrEmpresaFacturador.Equals(identificacion))
						  && (t.DatFechaVencimiento >= fecha_inicio && t.DatFechaVencimiento <= fecha_fin)
						  && t.IntNumTransaccCompra == cantidad
						  select t).FirstOrDefault();

			return datos_plan;
		}

		/// <summary>
		/// obtiene los planes de un facturador o un grupo de facturadores
		/// </summary>
		/// <param name="Identificacion"></param>
		/// <param name="TipoPlan"></param>
		/// <param name="Estado"></param>
		/// <param name="TipoFecha"></param>
		/// <param name="FechaInicio"></param>
		/// <param name="FechaFin"></param>
		/// <returns></returns>
		public List<TblPlanesTransacciones> Obtener(string Identificacion, string TipoPlan, string Estado, int TipoFecha, DateTime FechaInicio, DateTime FechaFin)
		{
			List<TblPlanesTransacciones> datos_plan = new List<TblPlanesTransacciones>();

			var ListaFacturadores = FacturadoresAsociadosPlan(Identificacion);

			List<byte> LstTipoPlan = new List<byte>();
			if (string.IsNullOrEmpty(TipoPlan))
			{
				TipoPlan = "*";
			}
			else
			{
				LstTipoPlan = Coleccion.ConvertirStringByte(TipoPlan);
			}



			List<int> LstEstadoPlan = new List<int>();
			if (string.IsNullOrEmpty(Estado))
			{
				Estado = "*";
			}
			else
			{
				LstEstadoPlan = Coleccion.ConvertirStringInt(Estado);
			}

			context.Configuration.LazyLoadingEnabled = false;

			datos_plan = (from t in context.TblPlanesTransacciones.Include("TblEmpresas")
							  //join empresa in context.TblEmpresas on t.StrEmpresaFacturador equals empresa.StrIdentificacion
							  //join empresacrea in context.TblEmpresas on t.StrEmpresaUsuario equals empresacrea.StrIdentificacion
						  where ListaFacturadores.Contains(t.StrEmpresaFacturador)
						  && (LstTipoPlan.Contains(t.IntTipoProceso) || TipoPlan == "*")
						  && (LstEstadoPlan.Contains(t.IntEstado) || Estado == "*")
						  && ((t.DatFecha >= FechaInicio && t.DatFecha <= FechaFin) || TipoFecha == 2)
						  && ((t.DatFechaVencimiento >= FechaInicio && t.DatFechaVencimiento <= FechaFin) || TipoFecha == 1)
						  orderby t.DatFecha descending
						  select t).ToList();

			return datos_plan;
		}

		/// <summary>
		/// obtiene los planes de un facturador o un grupo de Administrador
		/// </summary>
		/// <param name="Identificacion"></param>
		/// <param name="TipoPlan"></param>
		/// <param name="Estado"></param>
		/// <param name="TipoFecha"></param>
		/// <param name="FechaInicio"></param>
		/// <param name="FechaFin"></param>
		/// <returns></returns>
		public List<TblPlanesTransacciones> ObtenerPlanesAmin(string Identificacion, string TipoPlan, string Estado, int TipoFecha, DateTime FechaInicio, DateTime FechaFin)
		{

			//context.Configuration.LazyLoadingEnabled = false;

			List<TblPlanesTransacciones> datos_plan = new List<TblPlanesTransacciones>();

			//Ctl_Empresa Controlador = new Ctl_Empresa();

			//var ListaFacturadores = Controlador.ObtenerFacturadores();

			List<byte> LstTipoPlan = new List<byte>();
			if (string.IsNullOrEmpty(TipoPlan))
			{
				TipoPlan = "*";
			}
			else
			{
				LstTipoPlan = Coleccion.ConvertirStringByte(TipoPlan);
			}



			List<int> LstEstadoPlan = new List<int>();
			if (string.IsNullOrEmpty(Estado))
			{
				Estado = "*";
			}
			else
			{
				LstEstadoPlan = Coleccion.ConvertirStringInt(Estado);
			}


			datos_plan = (from t in context.TblPlanesTransacciones
						  join empresa in context.TblEmpresas on t.StrEmpresaFacturador equals empresa.StrIdentificacion
						  join empresacrea in context.TblEmpresas on t.StrEmpresaUsuario equals empresacrea.StrIdentificacion
						  where (t.StrEmpresaFacturador.Equals(Identificacion) || Identificacion.Equals("*"))
						  && (LstTipoPlan.Contains(t.IntTipoProceso) || TipoPlan == "*")
						  && (LstEstadoPlan.Contains(t.IntEstado) || Estado == "*")
						  && ((t.DatFecha >= FechaInicio && t.DatFecha <= FechaFin) || TipoFecha == 2)
						  && ((t.DatFechaVencimiento >= FechaInicio && t.DatFechaVencimiento <= FechaFin) || TipoFecha == 1)
						  orderby t.DatFecha descending
						  select t).ToList();

			return datos_plan;
		}

		public List<TblPlanesTransacciones> ObtenerPlanesMixto(string Identificacion)
		{

			//context.Configuration.LazyLoadingEnabled = false;

			List<TblPlanesTransacciones> datos_plan = new List<TblPlanesTransacciones>();

			//Ctl_Empresa Controlador = new Ctl_Empresa();

			//var ListaFacturadores = Controlador.ObtenerFacturadores();

			DateTime Fecha_Actual = Fecha.GetFecha().Date;

			int Estado = EstadoPlan.Habilitado.GetHashCode();
			int TipoPlan = TipoCompra.Compra.GetHashCode();
			int TipoDocumeto = TipoDocPlanes.Mixto.GetHashCode();

			context.Configuration.LazyLoadingEnabled = false;

			//Consultamos los planes del facturador
			datos_plan = (from t in context.TblPlanesTransacciones
						  where (t.StrEmpresaFacturador.Equals(Identificacion))
						   && t.IntEstado == Estado && ((t.DatFechaVencimiento >= Fecha_Actual) || t.DatFechaVencimiento == null)
						   && (((t.IntNumTransaccCompra - t.IntNumTransaccProcesadas) > 0) && (t.IntTipoProceso == TipoPlan))
						   && (t.IntTipoDocumento == TipoDocumeto)
						  select t).OrderBy(x => new { x.IntTipoProceso, x.DatFecha }).ToList();

			return datos_plan;
		}

		/// <summary>
		/// Retorna una lista de facturadores (string) con la relación de la bolsa de los planes
		/// </summary>
		/// <param name="Stridentificacion">Facturador que consulta</param>
		/// <returns></returns>
		public IEnumerable<string> FacturadoresPlan(string Stridentificacion)
		{
			var ListaFacturadores = (from lista in context.TblEmpresas
									 where lista.StrEmpresaDescuento.Equals(
										 (from datos in context.TblEmpresas
										  where datos.StrIdentificacion.Equals(Stridentificacion)
										  select datos.StrEmpresaDescuento).FirstOrDefault())
									 select lista.StrIdentificacion).ToList();

			return ListaFacturadores;
		}


		/// <summary>
		/// Retorna una lista de facturadores (string) con la relación de los asociados
		/// </summary>
		/// <param name="Stridentificacion">Facturador que consulta</param>
		/// <returns></returns>
		public IEnumerable<string> FacturadoresAsociadosPlan(string Stridentificacion)
		{
			context.Configuration.LazyLoadingEnabled = false;

			var ListaFacturadores = (from lista in context.TblEmpresas
									 where lista.StrEmpresaAsociada.Equals(
										 (from datos in context.TblEmpresas
										  where datos.StrIdentificacion.Equals(Stridentificacion)
										  select datos.StrEmpresaAsociada).FirstOrDefault())
									 select lista.StrIdentificacion).ToList();

			return ListaFacturadores;
		}

		/// <summary>
		/// retorna un enumerable con la lista de empresas realicionadas
		/// </summary>
		/// <param name="Stridentificacion">Facturador que consulta</param>
		/// <returns></returns>
		public List<TblEmpresas> FacturadoresBolsa(string Stridentificacion)
		{

			context.Configuration.LazyLoadingEnabled = false;

			var ListaFacturadores = (from lista in context.TblEmpresas
									 where lista.StrEmpresaAsociada.Equals(
										 (from datos in context.TblEmpresas
										  where datos.StrIdentificacion.Equals(Stridentificacion)
										  select datos.StrEmpresaAsociada).FirstOrDefault())
									 select lista).ToList();

			return ListaFacturadores;
		}


		/// <summary>
		/// retorna un enumerable con la lista de empresas realicionadas
		/// </summary>		
		/// <returns></returns>
		public List<TblEmpresas> ConlsutarAdministradorBolsa()
		{
			var ListaFacturadores = (from lista in context.TblEmpresas
									 where lista.StrEmpresaAsociada.Equals(
										 (from datos in context.TblEmpresas
										  select datos.StrEmpresaAsociada).FirstOrDefault())
									 select lista).ToList();

			return ListaFacturadores;
		}

		/// <summary>
		/// Obtiene el plan transaccional por id de seguridad.
		/// </summary>
		/// <param name="id_seguridad">Id de de seguridad del plan</param>
		/// <returns></returns>
		public TblPlanesTransacciones ObtenerIdSeguridad(System.Guid id_seguridad)
		{

			context.Configuration.LazyLoadingEnabled = false;

			TblPlanesTransacciones datos_plan = (from plan in context.TblPlanesTransacciones
												 where plan.StrIdSeguridad.Equals(id_seguridad)
												 select plan).FirstOrDefault();

			return datos_plan;
		}


		/// <summary>
		/// Consulta el plan por Id de Seguridad
		/// <param name="StrIdSeguridad">Id de de seguridad del plan</param>                
		/// </summary>        
		/// <returns></returns>
		public List<TblPlanesTransacciones> Obtener(System.Guid StrIdSeguridad)
		{

			context.Configuration.LazyLoadingEnabled = false;

			List<TblPlanesTransacciones> datos_plan = (from t in context.TblPlanesTransacciones.Include("TblEmpresas")
													   where t.StrIdSeguridad.Equals(StrIdSeguridad)
													   select t).ToList();

			return datos_plan;
		}

		#region Procesar Transacciones
		//**************************************************************
		/// <summary>
		/// Retorna una lista de ObjPlanenProceso para indicar al proceso, de que plan debe descontar los documentos procesados
		/// </summary>
		/// <param name="identificacion">Identificación del facturador</param>
		/// <param name="cantidaddoc">Cantidad de documentos a procesar</param>
		/// <returns></returns>
		public List<ObjPlanEnProceso> ObtenerPlanesActivos(string identificacion, int cantidaddoc, int TipoDoc)
		{
			context.Configuration.LazyLoadingEnabled = false;

			int Estado = EstadoPlan.Habilitado.GetHashCode();
			int Plan_PostPago = TipoCompra.PostPago.GetHashCode();
			DateTime Fecha_Actual = Fecha.GetFecha().Date;

			int tipo_doc_mixto = TipoDocPlanes.Mixto.GetHashCode();


			#region Proceso de descuento por empresa descuento y propios planes
			//************************** 

			//Consultamos los planes del facturador
			List<TblPlanesTransacciones> datos_plan = (from t in context.TblPlanesTransacciones
													   where (t.StrEmpresaFacturador.Equals(identificacion))
														&& t.IntEstado == Estado && ((t.DatFechaVencimiento >= Fecha_Actual) || t.DatFechaVencimiento == null)
														&& (((t.IntNumTransaccCompra - t.IntNumTransaccProcesadas) > 0) || (t.IntTipoProceso == Plan_PostPago))
														&& ((t.IntTipoDocumento == TipoDoc) || t.IntTipoDocumento == tipo_doc_mixto)
													   select t).OrderBy(x => new { x.IntTipoProceso, x.DatFecha }).ToList();

			//Calculamos el saldo disponible de la lista de planes del facturador
			var Saldo = datos_plan.Sum(x => x.IntNumTransaccCompra - (x.IntNumTransaccProcesadas + x.IntNumTransaccProceso));

			//Si el saldo es menor a la cantidad de documentos a procesar, entonces consultamos los planes del facturador y las del asociado
			if (Saldo < cantidaddoc)
			{
				//Validamos si la empresa tiene algun asociado para ver si podemos usar el saldo de este
				var empresa_descuenta = (from d in context.TblEmpresas
										 where d.StrIdentificacion.Equals(identificacion)
										 select d.StrEmpresaDescuento).FirstOrDefault();

				if (string.IsNullOrEmpty(empresa_descuenta))
				{
					empresa_descuenta = identificacion;
				}

				datos_plan = (from t in context.TblPlanesTransacciones
							  where (t.StrEmpresaFacturador.Equals(identificacion) || t.StrEmpresaFacturador.Equals(empresa_descuenta))
							   && t.IntEstado == Estado && ((t.DatFechaVencimiento >= Fecha_Actual) || t.DatFechaVencimiento == null)
							   && (((t.IntNumTransaccCompra - t.IntNumTransaccProcesadas) > 0) || (t.IntTipoProceso == Plan_PostPago))
							  select t).OrderBy(x => new { x.IntTipoProceso, x.DatFecha }).ToList();
			}

			//************************** 
			#endregion


			#region Descuento de Bosla

			//var ListaFacturadores = (from lista in context.TblEmpresas
			//						 where lista.StrEmpresaDescuento.Equals(
			//							 (from datos in context.TblEmpresas
			//							  where datos.StrIdentificacion.Equals(identificacion)
			//							  select datos.StrEmpresaDescuento).FirstOrDefault())
			//						 select lista.StrIdentificacion).ToList();



			//List<TblPlanesTransacciones> datos_plan = (from t in context.TblPlanesTransacciones
			//										   where ListaFacturadores.Contains(t.StrEmpresaFacturador)
			//											&& t.IntEstado == Estado && ((t.DatFechaVencimiento >= Fecha_Actual) || t.DatFechaVencimiento == null)
			//											&& (((t.IntNumTransaccCompra - t.IntNumTransaccProcesadas) > 0) || (t.IntTipoProceso == Plan_PostPago))
			//										   select t).OrderBy(x => new { x.IntTipoProceso, x.DatFecha }).ToList();


			#endregion



			List<ObjPlanEnProceso> listaobjproceso = new List<ObjPlanEnProceso>();
			ObjPlanEnProceso objproceso = new ObjPlanEnProceso();

			int docdisponibles;
			foreach (var plan in datos_plan)
			{

				if (plan.IntTipoProceso == Plan_PostPago)
				{
					objproceso = new ObjPlanEnProceso()
					{
						enProceso = cantidaddoc,
						plan = plan.StrIdSeguridad
					};
					listaobjproceso.Add(objproceso);
					cantidaddoc = 0;
					break;
				}
				else
				{
					docdisponibles = (plan.IntNumTransaccCompra - (plan.IntNumTransaccProcesadas + plan.IntNumTransaccProceso));
					if (docdisponibles > 0)
					{
						objproceso = new ObjPlanEnProceso();
						if (cantidaddoc > docdisponibles)
						{
							objproceso.enProceso = docdisponibles;
							objproceso.plan = plan.StrIdSeguridad;
							listaobjproceso.Add(objproceso);
							cantidaddoc = cantidaddoc - docdisponibles;
						}
						else
						{
							objproceso.enProceso = cantidaddoc;
							objproceso.plan = plan.StrIdSeguridad;
							listaobjproceso.Add(objproceso);
							cantidaddoc = 0;
							break;
						}
					}
				}

			}

			if (cantidaddoc > 0)
			{
				return null;
			}
			else
			{
				foreach (var item in listaobjproceso)
				{
					Enproceso(item.plan, item.enProceso);
				}
			}
			return listaobjproceso;
		}


		/// <summary>
		/// Retorna Documentos Disponibles
		/// </summary>
		/// <param name="identificacion"></param>
		/// <returns></returns>
		public dynamic obtenerSaldoDisponibles(string identificacion)
		{
			context.Configuration.LazyLoadingEnabled = false;

			int tipoplan = (int)TipoCompra.PostPago.GetHashCode();
			int estadoplan = (int)EstadoPlan.Habilitado.GetHashCode();

			int Plan_PostPago = TipoCompra.PostPago.GetHashCode();
			DateTime Fecha_Actual = Fecha.GetFecha().AddDays(1);


			var Plan = (from planes in context.TblPlanesTransacciones
						where planes.IntTipoProceso != tipoplan && planes.IntEstado == estadoplan
						&& planes.StrEmpresaFacturador.Equals(identificacion)
						&& (planes.DatFechaVencimiento == null || planes.DatFechaVencimiento >= Fecha_Actual)
						group planes by new { planes.StrEmpresaFacturador } into saldo_Facturador

						select new
						{
							Identificacion = saldo_Facturador.FirstOrDefault().StrEmpresaFacturador,
							Planes = saldo_Facturador.Count(),
							TProcesadas = saldo_Facturador.Sum(x => x.IntNumTransaccProcesadas),
							TCompra = saldo_Facturador.Sum(x => x.IntNumTransaccCompra),
							Facturador = saldo_Facturador.FirstOrDefault().TblEmpresas.IntObligado
						}).Select(item => new
						{
							item.Identificacion,
							item.Planes,
							item.TProcesadas,
							item.TCompra,
							TDisponible = (item.TCompra - item.TProcesadas),
							Porcentaje = Math.Round(((float)item.TProcesadas / (float)item.TCompra) * 100, 2),
							Tipo = 1,
							item.Facturador
						}).FirstOrDefault();

			if (Plan == null)
			{
				var PlanesPostPago = (from planes in context.TblPlanesTransacciones.Include("TblEmpresas")
									  where planes.IntTipoProceso == tipoplan && planes.IntEstado == estadoplan
									  && planes.StrEmpresaFacturador.Equals(identificacion)
									  && (planes.DatFechaVencimiento == null || planes.DatFechaVencimiento >= Fecha_Actual)
									  group planes by new { planes.StrEmpresaFacturador } into saldo_Facturador

									  select new
									  {
										  Identificacion = saldo_Facturador.FirstOrDefault().StrEmpresaFacturador,
										  Planes = saldo_Facturador.Count(),
										  TProcesadas = saldo_Facturador.Sum(x => x.IntNumTransaccProcesadas),
										  TDisponible = saldo_Facturador.Sum(x => x.IntNumTransaccProcesadas),
										  TCompra = saldo_Facturador.Sum(x => x.IntNumTransaccCompra),
										  Porcentaje = 100,
										  Tipo = 3,
										  Facturador = saldo_Facturador.FirstOrDefault().TblEmpresas.IntObligado
									  }).FirstOrDefault();

				return PlanesPostPago;
			}

			return Plan;
		}


		#region Happgi

		public dynamic obtenerPlanesHgiDocs(string identificacion)
		{
			context.Configuration.LazyLoadingEnabled = false;

			int tipoplan = (int)TipoCompra.PostPago.GetHashCode();

			var Plan = (from planes in context.TblPlanesTransacciones
						where planes.IntTipoProceso != tipoplan
						&& planes.StrEmpresaFacturador.Equals(identificacion)
						select new
						{
							Identificacion = planes.StrEmpresaFacturador,
							TProcesadas = planes.IntNumTransaccProcesadas,
							TCompra = planes.IntNumTransaccCompra,
							Valor = planes.IntValor,
							Estado = planes.IntEstado,
							FechaCompra = planes.DatFecha,
							FechaInicio = planes.DatFechaInicio,
							FechaFin = planes.DatFechaVencimiento
						}).Select(item => new
						{
							item.Identificacion,
							item.TProcesadas,
							item.TCompra,
							TDisponible = (item.TCompra - item.TProcesadas),
							Porcentaje = Math.Round(((float)item.TProcesadas / (float)item.TCompra) * 100, 2),
							item.Estado,
							item.Valor,
							item.FechaCompra,
							item.FechaInicio,
							item.FechaFin
						}).OrderByDescending(x => x.FechaCompra).ToList();

			return Plan;
		}

		#endregion



		/// <summary>
		/// Actualiza el campo IntNumTransaccProceso para indicar cuantos documentos estan reservados para procesar
		/// </summary>
		/// <param name="idseguridad">id de seguridad del plan</param>
		/// <param name="cantdocprocesar">Cantidad de documentos reservados para este proceso en este plan</param>
		/// <returns></returns>
		public TblPlanesTransacciones Enproceso(Guid idseguridad, int cantdocprocesar)
		{
			context.Configuration.LazyLoadingEnabled = false;

			TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
														  where t.StrIdSeguridad.Equals(idseguridad)
														  select t).FirstOrDefault();

			PlanesTransacciones.IntNumTransaccProceso = PlanesTransacciones.IntNumTransaccProceso + cantdocprocesar;

			PlanesTransacciones = this.Edit(PlanesTransacciones);

			return PlanesTransacciones;
		}


		/// <summary>
		/// Inicia el proceso de tareas async
		/// </summary>
		/// <param name="ListaPlanes"></param>
		/// <param name="respuesta"></param>
		/// <returns></returns>
		public async Task ConciliarPlanes(List<ObjPlanEnProceso> ListaPlanes, List<DocumentoRespuesta> respuesta)
		{
			try
			{
				var Tarea = TareaConciliarPlanes(ListaPlanes, respuesta);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}

		}




		public async Task TareaConciliarPlanes(List<ObjPlanEnProceso> ListaPlanes, List<DocumentoRespuesta> respuesta)
		{
			await Task.Factory.StartNew(() =>
			{
				Ctl_PlanesTransacciones Planestransacciones = new Ctl_PlanesTransacciones();
				////Planes y transacciones
				foreach (ObjPlanEnProceso plan in ListaPlanes)
				{
					plan.procesado = respuesta.Where(x => x.IdPlan == plan.plan).Where(x => x.DescuentaSaldo == true).Count();

					Planestransacciones.ConciliarPlanProceso(plan);
				}
			});
		}

		/// <summary>
		/// Actualiza el plan al terminar el proceso, coloca los valores reales luego del proceso
		/// campo intEnProceso le descuenta la cantidad de documentos que se estaban procesando
		/// </summary>		
		/// <param name="planenproceso">Objeto plan que esta en el paralelismo que se usa para conciliar</param>
		/// <returns></returns>
		public TblPlanesTransacciones ConciliarPlanProceso(ObjPlanEnProceso planenproceso)
		{

			context.Configuration.LazyLoadingEnabled = false;

			TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
														  where t.StrIdSeguridad.Equals(planenproceso.plan)
														  select t).FirstOrDefault();


			//Validamos si el plan ya ha procesado algun documento para colocarle la fecha de inicio y la fecha de vencimiento
			if (PlanesTransacciones.IntNumTransaccProcesadas == 0 && planenproceso.procesado > 0)
			{
				PlanesTransacciones.DatFechaInicio = Fecha.GetFecha();

				//Si Meses de Vencimiento es mayor a cero, entonces el plan vence
				if (PlanesTransacciones.IntMesesVence > 0 && PlanesTransacciones.DatFechaVencimiento == null)
				{
					//Se agregan los meses de vencimiento al mes a la fecha actual para que comience a contar desde hoy los meses indicados
					PlanesTransacciones.DatFechaVencimiento = Fecha.GetFecha().AddMonths(PlanesTransacciones.IntMesesVence);
				}

			}

			//Se debe Sumar el procesado del objeto a la cantidad de documentos procesados en la tabla             
			PlanesTransacciones.IntNumTransaccProcesadas = PlanesTransacciones.IntNumTransaccProcesadas + planenproceso.procesado;

			//En el campo de en proceso, se debe restar la cantidad de documentos reservados del objeto a la cantidad de objetos de la tabla
			PlanesTransacciones.IntNumTransaccProceso = PlanesTransacciones.IntNumTransaccProceso - planenproceso.enProceso;

			if (PlanesTransacciones.IntTipoProceso == (Int16)TipoCompra.PostPago)
			{
				//Si el plan es post-pago, entonces iguala el numero de transacciones adquiridas, por numero de transacciones procesadas
				//con el fin de que todos los indicadores queden bien representados.
				PlanesTransacciones.IntNumTransaccCompra = PlanesTransacciones.IntNumTransaccProcesadas;
			}
			else
			{

				if (PlanesTransacciones.IntEstado != EstadoPlan.Procesado.GetHashCode())
				{
					if (PlanesTransacciones.IntNumTransaccProcesadas >= PlanesTransacciones.IntNumTransaccCompra)
					{
						//Aqui es donde se coloca el plan como procesado en su totalidad en caso de no tener mas saldo
						PlanesTransacciones.IntEstado = EstadoPlan.Procesado.GetHashCode();
					}
				}
			}




			PlanesTransacciones = this.Edit(PlanesTransacciones);
			///Validación de alertas y notificaciones
			try
			{
				Ctl_Alertas controlador = new Ctl_Alertas();
				controlador.alertaPorcentajePlan(PlanesTransacciones.StrEmpresaFacturador);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

			return PlanesTransacciones;
		}

		/// <summary>
		/// Descuenta del saldo de transacciones de planes, una cantidad especifica de documentos 
		/// adicionalmente valida si el plan estaba en estado consumido y con este reverso, tiene transacciones disponibles, entonces activa el plan
		/// Si el plan estaba inhabilitado, no toma en cuenta la validación de activación
		/// </summary>
		/// <param name="idplan">id de seguridad del plan que se desea descontar los documentos</param>
		/// <returns></returns>
		public TblPlanesTransacciones DescontarDocumentosFallidos(Guid idplan, int cantdoc)
		{
			TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
														  where t.StrIdSeguridad.Equals(idplan)
														  select t).FirstOrDefault();
			if (PlanesTransacciones != null)
			{
				PlanesTransacciones.IntNumTransaccProcesadas = PlanesTransacciones.IntNumTransaccProcesadas - cantdoc;
				//Valida si se activa el plan en caso de que este consumido y no inhabilitado
				if (PlanesTransacciones.IntEstado == EstadoPlan.Procesado.GetHashCode())
				{
					if (PlanesTransacciones.IntNumTransaccProcesadas < PlanesTransacciones.IntNumTransaccCompra)
					{
						PlanesTransacciones.IntEstado = EstadoPlan.Habilitado.GetHashCode();
					}
				}
				PlanesTransacciones = this.Edit(PlanesTransacciones);
			}
			return PlanesTransacciones;
		}

		/// <summary>
		/// este objeto es el que se utiliza para manejar en el paralelismo la cantidad de documentos que se deben descontar al final del proceso.
		/// </summary>
		public class ObjPlanEnProceso
		{
			public Guid plan { get; set; }
			public int enProceso { get; set; }
			public int reservado { get; set; }
			public int procesado { get; set; }
		}
		#endregion




		#region Sonda Automatica de PostPago
		/// <summary>
		/// Sonda para procesar documentos
		/// </summary>
		/// <returns></returns>
		public async Task SondaCrearPlanesPostpago(string EmpresaCrea, string UsuarioCrea, bool Notifica)
		{
			try
			{
				var Tarea = TareaCrearPlanesPostapago(EmpresaCrea, UsuarioCrea, Notifica);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);
			}
		}

		/// <summary>
		/// Tarea asincrona para crear planes post pago automaticamente cada mes
		/// </summary>
		/// <returns></returns>
		public async Task TareaCrearPlanesPostapago(string EmpresaCrea, string UsuarioCrea, bool Notifica)
		{
			try
			{
				await Task.Factory.StartNew(() =>
					{
						List<TblEmpresas> listaempresas = new List<TblEmpresas>();
						Ctl_Empresa controlador = new Ctl_Empresa();
						listaempresas = controlador.ObtenerEmpPostPago();
						TblPlanesTransacciones plan = new TblPlanesTransacciones();
						foreach (var item in listaempresas)
						{
							try
							{
								if (CerrarplanesPostpago(item.StrIdentificacion))
								{
									plan = new TblPlanesTransacciones();
									//Se suma un mes al primer dia del mes
									DateTime Fecha1 = new DateTime(Fecha.GetFecha().Year, Fecha.GetFecha().Month, 1).AddMonths(1);

									//Se resta un dia para que nos de el ultimo dia del mes
									DateTime FechaVenc = Fecha1.AddDays(-1);
									//Se coloca la ultima hora y minuto del dia para que pueda seguir enviando documentos el ultimo dias del plan.
									FechaVenc = new DateTime(FechaVenc.Year, FechaVenc.Month, FechaVenc.Day, 23, 59, 00, 000);
									plan.DatFecha = Fecha.GetFecha();
									plan.DatFechaVencimiento = FechaVenc;
									plan.IntEstado = EstadoPlan.Habilitado.GetHashCode();
									plan.IntTipoProceso = Convert.ToByte(TipoCompra.PostPago.GetHashCode());
									plan.StrEmpresaFacturador = item.StrIdentificacion;
									plan.StrIdSeguridad = Guid.NewGuid();
									plan.StrEmpresaUsuario = EmpresaCrea;
									plan.StrUsuario = UsuarioCrea;
									string fecha_creacion = plan.DatFecha.ToString(Fecha.formato_fecha_hora);
									string fecha_vencimiento = plan.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet);
									plan.StrObservaciones = string.Format(Constantes.RecargaAutomaticaPostPago, fecha_creacion, fecha_vencimiento);
									Crear(plan, Notifica);
								}
							}
							catch (Exception excepcion)
							{
								Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);
							}
						}
					});
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.creacion);
			}
		}
		/// <summary>
		/// Cierra los planes postpago de un facturador en especifico
		/// </summary>
		/// <param name="Facturador">Documento del Facturador</param>
		public bool CerrarplanesPostpago(string Facturador)
		{
			bool CrearPlan = true;
			try
			{
				List<TblPlanesTransacciones> planes = new List<TblPlanesTransacciones>();
				byte postapago = Convert.ToByte(TipoCompra.PostPago.GetHashCode());
				byte habilitado = Convert.ToByte(EstadoPlan.Habilitado.GetHashCode());

				planes = (from datos in context.TblPlanesTransacciones
						  where datos.StrEmpresaFacturador.Equals(Facturador)
						  && datos.IntEstado == habilitado
						  && datos.IntTipoProceso == postapago
						  select datos).ToList();

				foreach (var item in planes)
				{
					// Si tiene un plan activo el mismo año y mismo mes, no crea nuevo plan
					if (item.DatFecha.Year == Fecha.GetFecha().Year && item.DatFecha.Month == Fecha.GetFecha().Month)
					{
						CrearPlan = false;
					}
					else
					{
						// Cierra plan que no sea del presente año y mes
						string fecha_vencimiento = (item.DatFechaVencimiento.HasValue) ? item.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet) : "";
						string fecha_creacion = item.DatFecha.ToString(Fecha.formato_fecha_hora);
						item.IntEstado = EstadoPlan.Procesado.GetHashCode();
						item.StrObservaciones = string.Format("{0}{1}{2}{3}", item.StrObservaciones, Environment.NewLine, Environment.NewLine, string.Format(Constantes.CierreAutomaticoPostPago, fecha_creacion, item.IntNumTransaccProcesadas, fecha_vencimiento));
						this.Edit(item);
					}
				}
				return CrearPlan;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
				return true;
			}
		}
		#endregion







		#region Sonda: conciliacion de planes


		/// <summary>
		/// Sonda para procesar documentos
		/// </summary>
		/// <returns></returns>
		public async Task TareaSondaConciliarPlanes()
		{
			try
			{
				var Tarea = SondaConciliarPlanes();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}



		/// <summary>
		/// Retorna lista de documentos segun el guid de seguridad del plan
		/// </summary>
		/// <param name="id_plan">guid de seguridad del plan</param>
		/// <returns>total documentos</returns>
		public int CantidadDocumentos(Guid id_plan)
		{
			context.Configuration.LazyLoadingEnabled = false;

			return (from documento in context.TblDocumentos
					where documento.StrIdPlanTransaccion == id_plan
					select documento.IntNumero).Count();

		}



		/// <summary>
		/// Concilia los planes de la siguiente manera:
		/// * Cuando existen diferencias entre el campo numero de documentos procesados(tbltransacciones) y el numero de documentos procesados(tbldocumentos)
		///   Actualiza este campo para que la cantidad de documentos disponibles sea la correcta.
		///   
		/// * Tambien se valida que no existan documentos por procesar, si existen algunos, los coloca en cero(0).
		/// </summary>
		/// <returns></returns>
		public async Task SondaConciliarPlanes()
		{
			bool CrearPlan = true;
			try

			{
				await Task.Factory.StartNew(() =>
				{
					//Obtenemos todos los planes activos				
					List<TblPlanesTransacciones> planes = new List<TblPlanesTransacciones>();
					byte habilitado = Convert.ToByte(EstadoPlan.Habilitado.GetHashCode());

					context.Configuration.LazyLoadingEnabled = false;
					planes = (from datos in context.TblPlanesTransacciones
							  where datos.IntEstado == habilitado
							  select datos).ToList();

					//Itereamos la lista de planes activos validar diferencias
					foreach (var item in planes)
					{

						try
						{

							//Validamos que exista diferencia entre el campo numero de documentos procesados(tbltransacciones) y el numero de documentos procesados(tbldocumentos)

							int total_documentos = CantidadDocumentos(item.StrIdSeguridad);
							if (item.IntNumTransaccProcesadas != total_documentos)
							{
								//Asignamos la cantidad de documentos procesados a la cantidad de transacciones procesadas del plan			
								item.IntNumTransaccProcesadas = total_documentos;
								//Se deja en cero el numero de transacciones en proceso
								item.IntNumTransaccProceso = 0;
								//validamos el tipo de plan.
								switch (item.IntTipoProceso)
								{
									case 1://Contersia
									case 2://Compra
										   //Si es compra o cortesia, se debe validar si supero el numero de documentos adquiridos
										if (item.IntNumTransaccProcesadas >= item.IntNumTransaccCompra)
										{
											//Si es asi, entonces cerramos el plan.
											item.IntEstado = 2;
										}
										break;
									case 3://PostPago
										   //Si es post pago, entonces el numero de transacciones adquiridas, deben ser igual al numero de transacciones procesadas
										item.IntNumTransaccCompra = item.IntNumTransaccProcesadas;
										break;
									default:
										break;
								}

								this.Edit(item);
							}
							else
							{
								// En el caso de que el numero de documentos procesados(tbltransacciones) sea igual, al numero de documentos(tbldocumentos), entonces preguntamos si tiene algun documento pendiente por procesar 
								if (item.IntNumTransaccProceso > 0)
								{
									//Si es asi, entonces lo colocamos en cero ya que estamos conciliando y no puede existir ningún proceso pendiente a esta hora.
									item.IntNumTransaccProceso = 0;
									this.Edit(item);
								}

								if (item.IntNumTransaccProcesadas >= item.IntNumTransaccCompra)
								{
									switch (item.IntTipoProceso)
									{
										case 1://Contersia
										case 2://Compra
											   //Si es compra o cortesia, se debe validar si supero el numero de documentos adquiridos
											if (item.IntNumTransaccProcesadas >= item.IntNumTransaccCompra)
											{
												//Si es asi, entonces cerramos el plan.
												item.IntEstado = 2;
												this.Edit(item);
											}
											break;
										case 3://PostPago
											   //Si es post pago, entonces el numero de transacciones adquiridas, deben ser igual al numero de transacciones procesadas
											if (item.IntNumTransaccCompra != item.IntNumTransaccProcesadas)
											{
												item.IntNumTransaccCompra = item.IntNumTransaccProcesadas;
												this.Edit(item);
											}
											break;
										default:
											break;
									}


								}

							}
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
						}

					}

					try
					{
						var Tarea = TareaPlanesVencidoshabilitados();
					}
					catch (Exception)
					{

					}

					return CrearPlan;
				});
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}

		public async Task TareaPlanesVencidoshabilitados()
		{
			try
			{
				var Tarea = SondaPlanesVencidoshabilitados();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}

		public async Task SondaPlanesVencidoshabilitados()
		{
			bool CrearPlan = true;
			try

			{
				await Task.Factory.StartNew(() =>
				{
					//Obtenemos todos los planes activos				
					List<TblPlanesTransacciones> planes = new List<TblPlanesTransacciones>();
					byte habilitado = Convert.ToByte(EstadoPlan.Habilitado.GetHashCode());

					DateTime fecha_actual = Fecha.GetFecha().Date;

					context.Configuration.LazyLoadingEnabled = false;
					planes = (from datos in context.TblPlanesTransacciones
							  where datos.IntEstado == habilitado &&
							  datos.DatFechaVencimiento < fecha_actual
							  select datos).ToList();

					//Itereamos la lista de planes activos validar diferencias
					foreach (var item in planes)
					{

						try
						{
							item.IntEstado = EstadoPlan.Procesado.GetHashCode();
							this.Edit(item);
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
						}

					}
					return CrearPlan;
				});
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}
		}

		#endregion

	}
}
