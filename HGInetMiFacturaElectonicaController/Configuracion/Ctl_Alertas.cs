using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_Alertas : BaseObject<TblAlertas>
	{

		#region Basicas

		public List<TblAlertas> ObtenerListaAlertas() {

			return context.TblAlertas.ToList();

		}
		#endregion



		#region Validacion de Alto consumo (Porcentaje Plan)
		/// <summary>
		/// Notifica al Facturador o personal Hgi, según sea el caso en el que el facturador tenga un % de saldo especifico según configuración o cuando no tiene saldo disponible
		/// </summary>
		/// <returns>Identificacion del facturador</returns>
		public bool alertaPorcentajePlan(string StrIdFacturador)
		{
			try
			{

				int tipoplan = (int)TipoCompra.PostPago.GetHashCode();
				int estadoplan = (int)EstadoPlan.Habilitado.GetHashCode();

				var Plan = (from planes in context.TblPlanesTransacciones
							where planes.IntTipoProceso != tipoplan && planes.IntEstado == estadoplan
							&& planes.StrEmpresaFacturador.Equals(StrIdFacturador)
							group planes by new { planes.StrEmpresaFacturador } into saldo_Facturador

							select new
							{
								Identificacion = saldo_Facturador.FirstOrDefault().StrEmpresaFacturador,
								StrIdSeguridadFact = saldo_Facturador.FirstOrDefault().TblEmpresas.StrIdSeguridad,
								Planes = saldo_Facturador.Count(),
								TProcesadas = saldo_Facturador.Sum(x => x.TblDocumentos.Count()),
								TCompra = saldo_Facturador.Sum(x => x.IntNumTransaccCompra),
								Email = saldo_Facturador.FirstOrDefault().TblEmpresas.StrMailAdmin,
							}).Select(item => new
							{
								item.Identificacion,
								item.StrIdSeguridadFact,
								item.Planes,
								item.TProcesadas,
								item.TCompra,
								TDisponible = (item.TCompra - item.TProcesadas),
								Porcentaje = Math.Round(((float)item.TProcesadas / (float)item.TCompra) * 100, 2),
								item.Email
							}).FirstOrDefault();


				Ctl_AlertasHistAudit _AlertasHist = new Ctl_AlertasHistAudit();



				Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

				//Obtenemos la configuración de alertas
				List<TblAlertas> Alertas = new List<TblAlertas>();

				string IdentificacionFacturador = string.Empty;
				if (Plan != null)
				{
					IdentificacionFacturador = Plan.Identificacion;
					//Obtenemos la lista de alertas disponible para este escenario
					Alertas = ObtenerAlerta(Plan.Porcentaje, (Int32)TipoAlerta.Porcenjate);
				}
				else
				{
					IdentificacionFacturador = StrIdFacturador;
					alertaSinSaldo(StrIdFacturador);
					return true;
				}

				//Por cada Facturador se itera lista de Alertas posibles 
				foreach (TblAlertas alerta in Alertas)
				{
					//Si se cumple la condición de evaluación, entonces se hace una consulta en el historico de alertas
					//para validar si ya se le envio la notificación y no repetir la misma notificación.


					if (Plan.Porcentaje > alerta.IntValor)
					{
						List<TblHistAlertas> HistAlerta = _AlertasHist.Obtener(IdentificacionFacturador, alerta.IntIdAlerta);
						//Si no se le ha notificado
						if (HistAlerta == null || HistAlerta.Count == 0)
						{
							string correo = string.Empty;
							//Evalua el tipo  envio de notificación
							if (alerta.IntCliente == true)
							{
								//2: enviar email de notificación al correo del facturador								
								email.EnviaNotificacionAlerta(Plan.Identificacion, Plan.Email, Plan.TCompra, Plan.TProcesadas, Plan.TDisponible, Plan.Porcentaje);
								//Guardar en el historico de notificaciones							
							}
							if (alerta.IntInterno == true)
							{
								//1: enviar email de notificación al correo configurado por Administración								
								email.EnviaNotificacionAlertaConsumoHGI(Plan.Identificacion, alerta.StrInternoMails, Plan.TCompra, Plan.TProcesadas, Plan.TDisponible, Plan.Porcentaje);
								//Guardar en el historico de notificaciones								
							}
							_AlertasHist = new Ctl_AlertasHistAudit();
							_AlertasHist.Crear(alerta.IntIdAlerta, Plan.StrIdSeguridadFact, Plan.Identificacion, string.Format("Notificación : {0}  Compra:{1} Procesados:{2} Disponibles:{3}  Porcentaje:{4}", alerta.StrDescripcion, Plan.TCompra, Plan.TProcesadas, Plan.TDisponible, Plan.Porcentaje), (Int32)TipoAlerta.Porcenjate, Guid.Empty);
							//break;
						}

					}
				}
				return true;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				return false;
			}
		}
		#endregion

		#region Validación de Facturadores Sin Saldo o Sin Plan
		/// <summary>
		/// Notifica al Facturador o personal Hgi, según sea el caso en el que el facturador no cuente con saldo disponible
		/// </summary>
		/// <returns>Identificacion del facturador</returns>
		public bool alertaSinSaldo(string StrIdFacturador)
		{
			try
			{
				int tipoplan = (int)TipoCompra.PostPago.GetHashCode();
				int estadoplan = (int)EstadoPlan.Habilitado.GetHashCode();

				var Facturador_Alerta = (from empresa in context.TblEmpresas
										 where empresa.StrIdentificacion.Equals(StrIdFacturador)
										 select new
										 {
											 StrIdentificacion = empresa.StrIdentificacion,
											 StrIdSeguridad = empresa.StrIdSeguridad,
											 plan = empresa.TblPlanesTransacciones.Count(x => (x.IntTipoProceso == tipoplan) && (x.IntEstado == estadoplan)),
											 Email = empresa.StrMailAdmin
										 }).FirstOrDefault();

				if (Facturador_Alerta.plan == 0)
				{
					Ctl_AlertasHistAudit _AlertasHist = new Ctl_AlertasHistAudit();

					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

					var Alertas = ObtenerAlerta(100, (Int16)TipoAlerta.SinPlan.GetHashCode()).FirstOrDefault();
					//Si se cumple la condición de evaluación, entonces se hace una consulta en el historico de alertas
					//para validar si ya se le envio la notificación y no repetir la misma notificación.
					List<TblHistAlertas> HistAlerta = _AlertasHist.Obtener(StrIdFacturador, Alertas.IntIdAlerta);
					//Si no se le ha notificado
					if (HistAlerta == null || HistAlerta.Count == 0)
					{
						string correo = string.Empty;
						//Evalua el tipo  envio de notificación
						if (Alertas.IntCliente == true)
						{
							//2: enviar email de notificación al correo del facturador								
							email.EnviaNotificacionSinSaldo(Facturador_Alerta.StrIdentificacion, Facturador_Alerta.Email,1);
							//Guardar en el historico de notificaciones							
						}
						if (Alertas.IntInterno == true)
						{
							//1: enviar email de notificación al correo configurado por Administración								
							email.EnviaNotificacionSinSaldo(Facturador_Alerta.StrIdentificacion, Alertas.StrInternoMails,2);
							//Guardar en el historico de notificaciones								
						}
						_AlertasHist = new Ctl_AlertasHistAudit();
						_AlertasHist.Crear(Alertas.IntIdAlerta, Facturador_Alerta.StrIdSeguridad, StrIdFacturador, string.Format("Notificación : {0} ", Alertas.StrDescripcion), (Int32)TipoAlerta.SinPlan.GetHashCode(), Guid.Empty);
					}
				}
				return true;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				return false;
			}
		}
		#endregion

		#region Alerta de planes por Vencer
		/// <summary>
		/// Notifica al Facturador o personal Hgi, según sea el caso en el que el facturador tenga un plan cerca de vencer según la configuración.
		/// </summary>		
		public bool alertaPlanporVencer(string Facturador = "*")
		{
			try
			{
				//No se notifica o no se toma en cuenta el vencimiento de un plan Inhabilitado
				int estadoplan = (int)EstadoPlan.Habilitado.GetHashCode();
				//Obtengo el indicador de fecha por vencer
				TblAlertas Alertas = ObtenerAlerta(0, (Int16)TipoAlerta.Porvencer.GetHashCode()).FirstOrDefault();
				//declaro mi fecha para poder hacer el calculo de los planes que estan cerca de vencer segun el indicador
				DateTime FechaActual = Fecha.GetFecha();

				var Planes = (from planes in context.TblPlanesTransacciones
							  where planes.IntEstado == estadoplan
							  && planes.TblEmpresas.IntCobroPostPago == 0
							  && planes.DatFechaVencimiento <= SqlFunctions.DateAdd("dd", Alertas.IntValor, FechaActual)
							  && (planes.StrEmpresaFacturador.Equals(Facturador) || Facturador.Equals("*"))
							  select planes).OrderBy(x => x.StrEmpresaFacturador).ToList();

				if (Planes != null && Planes.Count != 0)
				{

					Ctl_AlertasHistAudit _AlertasHist = new Ctl_AlertasHistAudit();

					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

					List<NotificacionAlertaporVencer> ListaNotificacion = new List<NotificacionAlertaporVencer>();

					//Si se cumple la condición de evaluación, entonces se hace una consulta en el historico de alertas
					foreach (var Plan in Planes)
					{
						//para validar si ya se le envio la notificación y no repetir la misma notificación.					
						TblHistAlertas HistAlerta = _AlertasHist.Obtener(Plan.StrEmpresaFacturador, Alertas.IntIdAlerta, Plan.StrIdSeguridad);
						//Si no se le ha notificado
						if (HistAlerta == null)
						{
							string correo = string.Empty;
							//Evalua el tipo  envio de notificación
							if (Alertas.IntCliente == true)
							{
								List<NotificacionAlertaporVencer> NotificacionIndividual = new List<NotificacionAlertaporVencer>();
								NotificacionAlertaporVencer Notificacion = new NotificacionAlertaporVencer();
								Notificacion.adquiridas = Plan.IntNumTransaccCompra;
								Notificacion.procesados = Plan.IntNumTransaccProcesadas;
								Notificacion.disponibles = Plan.IntNumTransaccCompra - Plan.IntNumTransaccProcesadas;
								Notificacion.Facturador = Plan.TblEmpresas.StrRazonSocial;
								Notificacion.Documento = Plan.StrEmpresaFacturador;
								Notificacion.Fecha = Plan.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet);
								Notificacion.tipo = Plan.IntTipoProceso;
								//
								NotificacionIndividual.Add(Notificacion);
								//Envia el email al Facturador
								email.EnviaNotificacionAlertaPorvencer(Plan.TblEmpresas.StrMailAdmin, NotificacionIndividual);
							}
							if (Alertas.IntInterno == true)
							{
								NotificacionAlertaporVencer Notificacion = new NotificacionAlertaporVencer();
								Notificacion.adquiridas = Plan.IntNumTransaccCompra;
								Notificacion.procesados = Plan.IntNumTransaccProcesadas;
								Notificacion.disponibles = Plan.IntNumTransaccCompra - Plan.IntNumTransaccProcesadas;
								Notificacion.Facturador = Plan.TblEmpresas.StrRazonSocial;
								Notificacion.Documento = Plan.StrEmpresaFacturador;
								Notificacion.Fecha = Plan.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet);
								Notificacion.tipo = Plan.IntTipoProceso;
								//
								ListaNotificacion.Add(Notificacion);
							}

							_AlertasHist = new Ctl_AlertasHistAudit();
							_AlertasHist.Crear(Alertas.IntIdAlerta, Plan.TblEmpresas.StrIdSeguridad, Plan.StrEmpresaFacturador, string.Format("Notificación : {0} ", Alertas.StrDescripcion), (Int32)TipoAlerta.Porvencer.GetHashCode(), Plan.StrIdSeguridad);
						}
					}
					if (ListaNotificacion.Count > 0)
					{
						email.EnviaNotificacionAlertaPorvencerHGI(Alertas.StrInternoMails, ListaNotificacion);
					}
				}

				return true;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				return false;
			}
		}
		#endregion

		#region Obtiene la historia de un tipo de alerta
		/// <summary>
		/// 
		/// </summary>
		/// <param name="valor"></param>
		/// <param name="talerta"></param>
		/// <returns></returns>
		public List<TblAlertas> ObtenerAlerta(double valor, Int16 talerta)
		{
			if (valor == 0)
			{
				return context.TblAlertas.Where(_x => (_x.IntTipo == talerta)).OrderByDescending(x => x.IntValor).ToList();
			}
			else
			{
				return context.TblAlertas.Where(_x => (_x.IntValor <= valor) && (_x.IntTipo == talerta)).OrderByDescending(x => x.IntValor).ToList();
			}
		}
		#endregion

		#region Vista Notificación de alertas

		/// <summary>
		/// Notifica al Facturador o personal Hgi, según sea el caso en el que el facturador tenga un % de saldo especifico según configuración o cuando no tiene saldo disponible
		/// </summary>
		/// <returns>Identificacion del facturador</returns>
		public List<NotificacionPlanes> ObtenerNotificaciones()
		{
			List<NotificacionPlanes> Listanotificacion = new List<NotificacionPlanes>();

			try
			{

				int tipoplan = (int)TipoCompra.PostPago.GetHashCode();
				int estadoplan = (int)EstadoPlan.Habilitado.GetHashCode();
				int idalertasinplan = (int)TipoAlerta.SinPlan.GetHashCode();
				int empresaActiva = 1;
				
				#region Alto Consumo
				var Plan = (from planes in context.TblPlanesTransacciones
							where planes.IntTipoProceso != tipoplan && planes.IntEstado == estadoplan
							group planes by new { planes.StrEmpresaFacturador } into saldo_Facturador

							select new
							{
								idseguridadEmpresa= saldo_Facturador.FirstOrDefault().TblEmpresas.StrIdSeguridad,
								Identificacion = saldo_Facturador.FirstOrDefault().StrEmpresaFacturador,
								Facturador = saldo_Facturador.FirstOrDefault().TblEmpresas.StrRazonSocial,
								StrIdSeguridadFact = saldo_Facturador.FirstOrDefault().TblEmpresas.StrIdSeguridad,
								Planes = saldo_Facturador.Count(),
								TProcesadas = saldo_Facturador.Sum(x => x.TblDocumentos.Count()),
								TCompra = saldo_Facturador.Sum(x => x.IntNumTransaccCompra),
								Email = saldo_Facturador.FirstOrDefault().TblEmpresas.StrMailAdmin,
								Fechavencimiento = saldo_Facturador.Min(x => x.DatFechaVencimiento)
							}).Select(item => new
							{
								item.idseguridadEmpresa,
								item.Identificacion,
								item.Facturador,
								item.StrIdSeguridadFact,
								item.Planes,
								item.TProcesadas,
								item.TCompra,
								TDisponible = (item.TCompra - item.TProcesadas),
								Porcentaje = Math.Round(((float)item.TProcesadas / (float)item.TCompra) * 100, 2),
								item.Email,
								item.Fechavencimiento
							}).ToList();


				Ctl_AlertasHistAudit _AlertasHist = new Ctl_AlertasHistAudit();

				NotificacionPlanes notificacion = new NotificacionPlanes();
				
				//Obtenemos la configuración de alertas
				List<TblAlertas> Alertas = new List<TblAlertas>();

				foreach (var Obligado in Plan)
				{
					//Obtenemos la lista de alertas disponible para este escenario
					Alertas = ObtenerAlerta(Obligado.Porcentaje, (Int32)TipoAlerta.Porcenjate);
					
					//alertaSinSaldo(Obligado.Identificacion);
					//Por cada Facturador se itera lista de Alertas posibles 
					foreach (TblAlertas alerta in Alertas)
					{						
						notificacion = new NotificacionPlanes();
						if (Obligado.Porcentaje > alerta.IntValor)
						{
							List<TblHistAlertas> HistAlerta = _AlertasHist.Obtener(Obligado.Identificacion, alerta.IntIdAlerta);							
							notificacion.identificacion = (string.IsNullOrEmpty(Obligado.Identificacion))?"":Obligado.Identificacion;							
							notificacion.facturador = (string.IsNullOrEmpty(Obligado.Facturador))?"": Obligado.Facturador;							
							notificacion.fechavencimiento = (Obligado.Fechavencimiento==null)?"": Obligado.Fechavencimiento.Value.ToString(Fecha.formato_fecha_hginet);							
							notificacion.nplanes = Obligado.Planes;							
							notificacion.tcompra = Obligado.TCompra;							
							notificacion.tprocesadas = Obligado.TProcesadas;							
							notificacion.tdisponibles = Obligado.TDisponible;							
							notificacion.valorindicador = string.Format("{0}%", Obligado.Porcentaje.ToString("F"));							
							notificacion.alerta = alerta.StrDescripcion;							
							notificacion.idalerta = alerta.IntIdAlerta;							
							notificacion.idseguridadEmpresa = Obligado.idseguridadEmpresa;							
							//Si no se le ha notificado																			
							if (HistAlerta == null || HistAlerta.Count == 0)
							{
								notificacion.notificado = "NO";
							}
							else
							{
								notificacion.notificado = "SI";
							}
							if (alerta.IntCliente == true && alerta.IntInterno == true)
							{
								notificacion.email = string.Format("Interno:{0} Cliente:{1}", alerta.StrInternoMails, Obligado.Email);
							}
							else
							{
								//Evalua el tipo  envio de notificación
								if (alerta.IntCliente == true)
								{
									notificacion.email = string.Format("Cliente:{0}", Obligado.Email);
								}
								if (alerta.IntInterno == true)
								{
									notificacion.email = string.Format("Interno:{0}", alerta.StrInternoMails);
								}
							}
							LogExcepcion.Guardar(new Exception("Paso 6"));
							Listanotificacion.Add(notificacion);
							//break;
						}
					}
				}
				#endregion

				#region Plan por Vencer				

				//Obtengo el indicador de fecha por vencer
				TblAlertas Alerta = ObtenerAlerta(0, (Int16)TipoAlerta.Porvencer.GetHashCode()).FirstOrDefault();
				//declaro mi fecha para poder hacer el calculo de los planes que estan cerca de vencer segun el indicador
				DateTime FechaActual = Fecha.GetFecha();

				var Planes = (from planes in context.TblPlanesTransacciones
							  where planes.IntEstado == estadoplan
							  && planes.TblEmpresas.IntCobroPostPago == 0
							  && planes.DatFechaVencimiento <= SqlFunctions.DateAdd("dd", Alerta.IntValor, FechaActual)
							  select new
							  {
								  idseguridadEmpresa = planes.TblEmpresas.StrIdSeguridad,
								  StrEmpresaFacturador = planes.StrEmpresaFacturador,
								  StrRazonSocial = planes.TblEmpresas.StrRazonSocial,
								  planes.IntNumTransaccCompra,
								  planes.IntNumTransaccProcesadas,
								  planes.DatFechaVencimiento,
								  planes.StrIdSeguridad,
								  planes.TblEmpresas.StrMailAdmin,
								  DiasRestantes = ((int)SqlFunctions.DateDiff("dd", FechaActual, planes.DatFechaVencimiento))
							  }).OrderBy(x => x.StrEmpresaFacturador).ToList();



				if (Planes != null && Planes.Count != 0)
				{
					foreach (var plan in Planes)
					{

						TblHistAlertas HistAlerta = _AlertasHist.Obtener(plan.StrEmpresaFacturador, Alerta.IntIdAlerta, plan.StrIdSeguridad);

						notificacion = new NotificacionPlanes();

						notificacion.identificacion = plan.StrEmpresaFacturador;
						notificacion.facturador = plan.StrRazonSocial;
						notificacion.nplanes = 1;
						notificacion.tcompra = plan.IntNumTransaccCompra;
						notificacion.tprocesadas = plan.IntNumTransaccProcesadas;
						notificacion.tdisponibles = (plan.IntNumTransaccCompra - plan.IntNumTransaccProcesadas);
						notificacion.valorindicador = string.Format("{0} Días", plan.DiasRestantes.ToString());
						notificacion.alerta = Alerta.StrDescripcion;
						notificacion.fechavencimiento = plan.DatFechaVencimiento.Value.ToString(Fecha.formato_fecha_hginet);
						notificacion.idalerta = Alerta.IntIdAlerta;
						notificacion.idseguridadEmpresa = plan.idseguridadEmpresa;
						if (Alerta.IntCliente == true && Alerta.IntInterno == true)
						{
							notificacion.email = string.Format("Interno:{0} Cliente:{1}", Alerta.StrInternoMails, plan.StrMailAdmin);
						}
						else
						{
							//Evalua el tipo  envio de notificación
							if (Alerta.IntCliente == true)
							{
								notificacion.email = string.Format("Cliente:{0}", plan.StrMailAdmin);
							}
							if (Alerta.IntInterno == true)
							{
								notificacion.email = string.Format("Interno:{0}", Alerta.StrInternoMails);
							}
						}


						if (HistAlerta == null)
						{
							notificacion.notificado = "NO";
						}
						else
						{
							notificacion.notificado = "SI";
						}

						Listanotificacion.Add(notificacion);
					}
				}
				#endregion

				#region Sin Saldo o sin Plan
				var FSinPlan = (from empresa in context.TblEmpresas
								where empresa.IntObligado == true && empresa.IntIdEstado == empresaActiva								
								select new
								{
									idseguridadEmpresa = empresa.StrIdSeguridad,
									Identificacion = empresa.StrIdentificacion,
									Facturador = empresa.StrRazonSocial,
									StrIdSeguridad = empresa.StrIdSeguridad,
									plan = empresa.TblPlanesTransacciones.Count(x => x.IntEstado == estadoplan),
									Email = empresa.StrMailAdmin,
									documentos = empresa.TblEmpresasFacturador.Count()
								}).ToList();

				Alerta = ObtenerAlerta(0, (Int16)TipoAlerta.SinPlan.GetHashCode()).FirstOrDefault();
				foreach (var Obligado in FSinPlan)
				{					
					//Valida que no tenga ningun plan activo y que tenga por lo menos un documento en la plataforma ya que asi garantiza que si es un Facturador activo en la plataforma.					
					if (Obligado.plan < 1 && Obligado.documentos > 0)
					{
						notificacion = new NotificacionPlanes();

						List<TblHistAlertas> HistAlerta = _AlertasHist.Obtener(Obligado.Identificacion, idalertasinplan);

						notificacion.identificacion = Obligado.Identificacion;
						notificacion.facturador = Obligado.Facturador;
						notificacion.fechavencimiento = "";
						notificacion.nplanes = Obligado.plan;
						notificacion.idalerta = Alerta.IntIdAlerta;
						notificacion.alerta = Alerta.StrDescripcion;
						notificacion.idseguridadEmpresa = Obligado.idseguridadEmpresa;
						//Si no se le ha notificado																			
						if (HistAlerta == null || HistAlerta.Count == 0)
						{
							notificacion.notificado = "NO";
						}
						else
						{
							notificacion.notificado = "SI";
						}
						if (Alerta.IntCliente == true && Alerta.IntInterno == true)
						{
							notificacion.email = string.Format("Interno:{0} Cliente:{1}", Alerta.StrInternoMails, Obligado.Email);
						}
						else
						{
							//Evalua el tipo  envio de notificación
							if (Alerta.IntCliente == true)
							{
								notificacion.email = string.Format("Cliente:{0}", Obligado.Email);
							}
							if (Alerta.IntInterno == true)
							{
								notificacion.email = string.Format("Interno:{0}", Alerta.StrInternoMails);
							}
						}
						Listanotificacion.Add(notificacion);
					}

				}






				#endregion

				return Listanotificacion;
			}
			catch (Exception ex)
			{
				LogExcepcion.Guardar(ex);
				return Listanotificacion;
			}
		}

		#endregion

		#region Objetos
		/// <summary>
		/// Objeto de Notificación de Alertas por fecha de vencimiento de planes, Facturadores sin planes o porcentaje de planes con alto consumo.
		/// </summary>
		public class NotificacionAlertaporVencer
		{
			public string Documento { get; set; }
			public string Facturador { get; set; }
			public int adquiridas { get; set; }
			public int procesados { get; set; }
			public int disponibles { get; set; }
			public double Porcentaje { get; set; }
			public string Fecha { get; set; }
			public int tipo { get; set; }
			public Guid idseguridadEmpresa { get; set; }
		}


		public class NotificacionPlanes
		{
			public DateTime fecha { get; set; }
			public string fechavencimiento { get; set; }
			public string identificacion { get; set; }
			public string facturador { get; set; }
			public int nplanes { get; set; }
			public int tcompra { get; set; }
			public int tprocesadas { get; set; }
			public int tdisponibles { get; set; }
			public string valorindicador { get; set; }
			public string email { get; set; }
			public string alerta { get; set; }
			public string notificado { get; set; }
			public int idalerta { get; set; }
			public Guid idseguridadEmpresa { get; set; }
		}



		#endregion

		#region Sonda
		public async Task SondaPlanesPorVencer()
		{
			try
			{
				var Tarea = TareaPlanesPorVencer();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
			}
		}


		/// <summary>
		/// Tarea asincrona para crear planes post pago automaticamente cada mes
		/// </summary>
		/// <returns></returns>
		public async Task TareaPlanesPorVencer()
		{
			try
			{
				await Task.Factory.StartNew(() =>
				{
					alertaPlanporVencer();
				});
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
			}
		}

		#endregion
	}
}
