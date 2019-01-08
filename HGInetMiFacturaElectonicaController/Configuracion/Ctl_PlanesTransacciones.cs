using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
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
		public TblPlanesTransacciones Crear(TblPlanesTransacciones datos_plan, bool Envia_email = true)
		{
			datos_plan.DatFecha = Fecha.GetFecha();
			datos_plan.StrIdSeguridad = Guid.NewGuid();

			datos_plan = this.Add(datos_plan);

			Ctl_Empresa empresa = new Ctl_Empresa();

			TblEmpresas facturador = empresa.Obtener(datos_plan.StrEmpresaFacturador);

			if (Envia_email)
			{
				Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

				email.EnviaNotificacionRecarga(facturador.StrIdentificacion, facturador.StrMail, datos_plan);
			}
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

			var ListaFacturadores = FacturadoresPlan(Identificacion);

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

		/// <summary>
		/// Retorna una lista de facturadores (string) con la relación de la bolsa de los planes
		/// </summary>
		/// <param name="Stridentificacion">Facturador que consulta</param>
		/// <returns></returns>
		public IEnumerable<string> FacturadoresPlan(string Stridentificacion)
		{
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
		/// <param name="identificacion">Identificación del facturador</param>
		/// <param name="cantidaddoc">Cantidad de documentos a procesar</param>
		/// <returns></returns>
		public List<ObjPlanEnProceso> ObtenerPlanesActivos(string identificacion, int cantidaddoc)
		{

			int Estado = EstadoPlan.Habilitado.GetHashCode();
			int Plan_PostPago = TipoCompra.PostPago.GetHashCode();
			DateTime Fecha_Actual = Fecha.GetFecha();

			var ListaFacturadores = (from lista in context.TblEmpresas
									 where lista.StrEmpresaAsociada.Equals(
										 (from datos in context.TblEmpresas
										  where datos.StrIdentificacion.Equals(identificacion)
										  select datos.StrEmpresaAsociada).FirstOrDefault())
									 select lista.StrIdentificacion).ToList();


			List<TblPlanesTransacciones> datos_plan = (from t in context.TblPlanesTransacciones
													   where ListaFacturadores.Contains(t.StrEmpresaFacturador)
														&& t.IntEstado == Estado && t.DatFechaVencimiento >= Fecha_Actual
														&& (((t.IntNumTransaccCompra - t.IntNumTransaccProcesadas) > 0)  || (t.IntTipoProceso == Plan_PostPago))
													   select t).OrderBy(x => new { x.IntTipoProceso, x.DatFechaVencimiento }).ToList();

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
		/// Actualiza el campo IntNumTransaccProceso para indicar cuantos documentos estan reservados para procesar
		/// </summary>
		/// <param name="idseguridad">id de seguridad del plan</param>
		/// <param name="cantdocprocesar">Cantidad de documentos reservados para este proceso en este plan</param>
		/// <returns></returns>
		public TblPlanesTransacciones Enproceso(Guid idseguridad, int cantdocprocesar)
		{

			TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
														  where t.StrIdSeguridad.Equals(idseguridad)
														  select t).FirstOrDefault();

			PlanesTransacciones.IntNumTransaccProceso = PlanesTransacciones.IntNumTransaccProceso + cantdocprocesar;

			PlanesTransacciones = this.Edit(PlanesTransacciones);

			return PlanesTransacciones;
		}

		/// <summary>
		/// Actualiza el plan al terminar el proceso, coloca los valores reales luego del proceso
		/// campo intEnProceso le descuenta la cantidad de documentos que se estaban procesando
		/// </summary>		
		/// <param name="planenproceso">Objeto plan que esta en el paralelismo que se usa para conciliar</param>
		/// <returns></returns>
		public TblPlanesTransacciones ConciliarPlanProceso(ObjPlanEnProceso planenproceso)
		{
			TblPlanesTransacciones PlanesTransacciones = (from t in context.TblPlanesTransacciones
														  where t.StrIdSeguridad.Equals(planenproceso.plan)
														  select t).FirstOrDefault();

			//Se debe Sumar el procesado del objeto a la cantidad de documentos procesados en la tabla             
			PlanesTransacciones.IntNumTransaccProcesadas = PlanesTransacciones.IntNumTransaccProcesadas + planenproceso.procesado;

			//En el campo de en proceso, se debe restar la cantidad de documentos reservados del objeto a la cantidad de objetos de la tabla
			PlanesTransacciones.IntNumTransaccProceso = PlanesTransacciones.IntNumTransaccProceso - planenproceso.enProceso;

			if (PlanesTransacciones.IntTipoProceso == (Int16)TipoCompra.PostPago) {
				//Si el plan es post-pago, entonces iguala el numero de transacciones adquiridas, por numero de transacciones procesadas
				//con el fin de que todos los indicadores queden bien representados.
				PlanesTransacciones.IntNumTransaccCompra = PlanesTransacciones.IntNumTransaccProcesadas;
			} else {

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
	}
}
