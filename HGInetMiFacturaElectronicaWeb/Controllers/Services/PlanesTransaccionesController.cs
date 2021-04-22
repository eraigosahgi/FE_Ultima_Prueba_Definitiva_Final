using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class PlanesTransaccionesController : ApiController
	{

		/// <summary>
		/// Obtiene la lista 
		/// </summary>        
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get([FromUri]string Identificacion)
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				if (datosempresa.IntAdministrador)
				{
					Identificacion = "*";
				}

				Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
				var datos = ctl_PlanesTransacciones.Obtener(Identificacion);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					Empresa = d.TblUsuarios.TblEmpresas.StrRazonSocial,
					Usuario = d.StrUsuario,
					Valor = d.IntValor,
					TCompra = d.IntNumTransaccCompra,
					TProcesadas = d.IntNumTransaccProcesadas,
					id = d.StrIdSeguridad,
					Fecha = d.DatFecha,
					EmpresaFacturador = d.TblEmpresas.StrRazonSocial,
					Estado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPlan>(d.IntEstado)),
					Observaciones = (d.StrObservaciones != null) ? d.StrObservaciones : "",
					Saldo = d.IntNumTransaccCompra - d.IntNumTransaccProcesadas,
					Tipoproceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(d.IntTipoProceso)),
					CodigoEstado = d.IntEstado,
					Facturador = d.StrEmpresaFacturador,
					CodCompra = d.IntTipoProceso,
					Porcentaje = (d.IntTipoProceso != 3) ? (((float)d.IntNumTransaccProcesadas / (float)d.IntNumTransaccCompra) * 100) : 0
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		#region Vista Facturador
		[HttpGet]
		[Route("api/ConsultarPlanesFacturador")]
		public IHttpActionResult ConsultarPlanesFacturador(string Identificacion, string TipoPlan, string Estado, int TipoFecha, DateTime FechaInicio, DateTime FechaFin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
				var datos = ctl_PlanesTransacciones.Obtener(Identificacion, TipoPlan, Estado, TipoFecha, FechaInicio, FechaFin);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					Empresa = d.StrEmpresaUsuario,
					Usuario = d.StrUsuario,
					Valor = d.IntValor,
					TCompra = d.IntNumTransaccCompra,
					TProcesadas = d.IntNumTransaccProcesadas,
					id = d.StrIdSeguridad,
					Fecha = d.DatFecha,
					EmpresaFacturador = d.TblEmpresas.StrRazonSocial,
					Estado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPlan>(d.IntEstado)),
					Observaciones = (d.StrObservaciones != null) ? d.StrObservaciones : "",
					Saldo = d.IntNumTransaccCompra - d.IntNumTransaccProcesadas,
					Tipoproceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(d.IntTipoProceso)),
					CodigoEstado = d.IntEstado,
					CodCompra = d.IntTipoProceso,
					Porcentaje = (d.IntTipoProceso != 3) ? (((float)d.IntNumTransaccProcesadas / (float)d.IntNumTransaccCompra) * 100) : 0
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("api/ConsultarPlanesAdministrador")]
		public IHttpActionResult ConsultarPlanesAdministrador(string Identificacion, string TipoPlan, string Estado, int TipoFecha, DateTime FechaInicio, DateTime FechaFin)
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				TblEmpresas datosempresa = datosSesion.FirstOrDefault();

				if (datosempresa.IntAdministrador)
				{
					if (string.IsNullOrEmpty(Identificacion) || datosempresa.StrIdentificacion.Equals(Identificacion))
					{
						Identificacion = "*";
					}
				}

				Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
				var datos = ctl_PlanesTransacciones.ObtenerPlanesAmin(Identificacion, TipoPlan, Estado, TipoFecha, FechaInicio, FechaFin);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					Empresa = d.StrEmpresaUsuario,
					Usuario = d.StrUsuario,
					Valor = d.IntValor,
					TCompra = d.IntNumTransaccCompra,
					TProcesadas = d.IntNumTransaccProcesadas,
					id = d.StrIdSeguridad,
					Fecha = d.DatFecha,
					EmpresaFacturador = d.TblEmpresas.StrRazonSocial,
					Estado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadoPlan>(d.IntEstado)),
					Observaciones = (d.StrObservaciones != null) ? d.StrObservaciones : "",
					Saldo = d.IntNumTransaccCompra - d.IntNumTransaccProcesadas,
					Tipoproceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoCompra>(d.IntTipoProceso)),
					CodigoEstado = d.IntEstado,
					Facturador = d.StrEmpresaFacturador,
					CodCompra = d.IntTipoProceso,
					Porcentaje = (d.IntTipoProceso != 3) ? (((float)d.IntNumTransaccProcesadas / (float)d.IntNumTransaccCompra) * 100) : 0
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("api/ConsultarFacturadorBolsa")]
		public IHttpActionResult ConsultarFacturadorBolsa(string Identificacion)
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);



				Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
				var datos = ctl_PlanesTransacciones.FacturadoresBolsa(Sesion.DatosEmpresa.StrIdentificacion);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					ID = d.StrIdentificacion,
					Texto = d.StrRazonSocial
				});

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("api/ConsultarBolsaAdmin")]
		public IHttpActionResult ConsultarBolsaAdmin()
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblEmpresas> datosSesion = new List<TblEmpresas>();

				datosSesion.Add(Sesion.DatosEmpresa);

				if (Sesion.DatosEmpresa.IntAdministrador)
				{
					Ctl_Empresa Controlador = new Ctl_Empresa();
					var datos = Controlador.ObtenerFacturadores();

					if (datos == null)
					{
						return NotFound();
					}

					var retorno = datos.Select(d => new
					{
						ID = d.StrIdentificacion,
						Texto = d.StrRazonSocial
					});

					return Ok(retorno);
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion
		/// <summary>
		/// Obtiene el plan por Id de Seguridad
		/// </summary>       
		/// <param name="IdSeguridad"></param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(System.Guid IdSeguridad)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_PlanesTransacciones ctl_PlanesTransacciones = new Ctl_PlanesTransacciones();
				List<TblPlanesTransacciones> datos = ctl_PlanesTransacciones.Obtener(IdSeguridad);

				if (datos == null)
				{
					return NotFound();
				}

				var retorno = datos.Select(d => new
				{
					Empresa = d.StrEmpresaUsuario,
					Usuario = d.StrUsuario,
					Valor = d.IntValor,
					TCompra = d.IntNumTransaccCompra,
					TProcesadas = d.IntNumTransaccProcesadas,
					TDisponibles = d.IntNumTransaccCompra - d.IntNumTransaccProcesadas,
					id = d.StrIdSeguridad,
					Fecha = d.DatFecha,
					CodigoEmpresaFacturador = d.TblEmpresas.StrIdentificacion,
					EmpresaFacturador = d.TblEmpresas.StrRazonSocial,
					Tipo = d.IntTipoProceso,
					Observaciones = d.StrObservaciones,
					Estado = d.IntEstado,
					FechaVence = d.DatFechaVencimiento,
					MesesVence = d.IntMesesVence,
					DocRef = d.DocumentoRef,
					FechaInicio = (d.DatFechaInicio == null) ? "" : d.DatFechaInicio.Value.ToString(Fecha.formato_fecha_hginet)
				});

				return Ok(retorno);

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Crea una transaccion ya existente en plan de transaccion
		/// </summary>
		/// <param name="IntTipoProceso"></param>
		/// <param name="StrEmpresa Sesion"></param>
		/// <param name="StrUsuario Sesion"></param>
		/// <param name="IntNumTransaccCompra"></param>
		/// <param name="IntNumTransaccProcesadas"></param>
		/// <param name="IntValor"></param>
		/// <param name="BitProcesada"></param>
		/// <param name="StrObservaciones"></param>
		/// <param name="StrEmpresaFacturador"></param>
		/// <param name="Tipo"></param>
		/// <returns></returns>
		public IHttpActionResult Post([FromUri]byte IntTipoProceso, [FromUri]string StrEmpresa, [FromUri]string StrUsuario, [FromUri]int IntNumTransaccCompra, [FromUri]int IntNumTransaccProcesadas, [FromUri] decimal IntValor, [FromUri]int Estado, [FromUri]string StrObservaciones, [FromUri]string StrEmpresaFacturador, [FromUri] bool Envia_email, [FromUri]bool Vence, [FromUri] DateTime FechaVence, [FromUri] short MesesVence, [FromUri] string DocRef)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();

				TblPlanesTransacciones ObjPlanTransacciones = new TblPlanesTransacciones();
				ObjPlanTransacciones.IntTipoProceso = IntTipoProceso;
				ObjPlanTransacciones.StrEmpresaUsuario = StrEmpresa.Trim();
				ObjPlanTransacciones.StrUsuario = StrUsuario;
				ObjPlanTransacciones.IntNumTransaccCompra = IntNumTransaccCompra;
				ObjPlanTransacciones.IntNumTransaccProcesadas = IntNumTransaccProcesadas;
				ObjPlanTransacciones.IntValor = IntValor;
				ObjPlanTransacciones.IntEstado = Estado;
				ObjPlanTransacciones.StrObservaciones = StrObservaciones;
				ObjPlanTransacciones.StrEmpresaFacturador = StrEmpresaFacturador.Trim();
				ObjPlanTransacciones.DocumentoRef = DocRef;
				if (Vence)
				{
					ObjPlanTransacciones.IntMesesVence = MesesVence;
				}


				//if (Vence && MesesVence>0)
				//{
				//	ObjPlanTransacciones.DatFechaVencimiento = DateTime.Now.AddMonths(MesesVence);					
				//}

				clase_planes.Crear(ObjPlanTransacciones, Envia_email);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Crea una transaccion ya existente en plan de transaccion
		/// </summary>
		/// <param name="IntTipoProceso"></param>
		/// <param name="StrEmpresa Sesion"></param>
		/// <param name="StrUsuario Sesion"></param>
		/// <param name="IntNumTransaccCompra"></param>
		/// <param name="IntNumTransaccProcesadas"></param>
		/// <param name="IntValor"></param>
		/// <param name="BitProcesada"></param>
		/// <param name="StrObservaciones"></param>
		/// <param name="StrEmpresaFacturador"></param>
		/// <param name="Tipo"></param>
		/// <returns></returns>
		public IHttpActionResult Post([FromUri]byte IntTipoProceso, [FromUri]string StrEmpresa, [FromUri]string StrUsuario, [FromUri]int IntNumTransaccCompra, [FromUri]int IntNumTransaccProcesadas, [FromUri] decimal IntValor, [FromUri]int Estado, [FromUri]string StrObservaciones, [FromUri]string StrEmpresaFacturador, [FromUri]bool Vence, [FromUri] DateTime FechaVence, [FromUri]System.Guid StrIdSeguridad, [FromUri] short MesesVence, [FromUri] string DocRef, [FromUri]bool Editar)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();

				TblPlanesTransacciones ObjPTransacciones = new TblPlanesTransacciones();
				ObjPTransacciones.IntTipoProceso = IntTipoProceso;
				ObjPTransacciones.StrEmpresaUsuario = StrEmpresa.Trim();
				ObjPTransacciones.StrUsuario = StrUsuario;
				ObjPTransacciones.IntNumTransaccCompra = IntNumTransaccCompra;
				ObjPTransacciones.IntNumTransaccProcesadas = IntNumTransaccProcesadas;
				ObjPTransacciones.IntValor = IntValor;
				ObjPTransacciones.IntEstado = Estado;
				ObjPTransacciones.StrObservaciones = StrObservaciones;
				ObjPTransacciones.StrEmpresaFacturador = StrEmpresaFacturador.Trim();
				ObjPTransacciones.StrIdSeguridad = StrIdSeguridad;
				ObjPTransacciones.DocumentoRef = (!string.IsNullOrEmpty(DocRef)) ? DocRef.Trim() : string.Empty;
				if (Vence)
				{
					ObjPTransacciones.IntMesesVence = MesesVence;
				}

				//if (Vence && MesesVence > 0)
				//{
				//	ObjPTransacciones.DatFechaVencimiento = DateTime.Now.AddMonths(MesesVence);
				//}

				clase_planes.Editar(ObjPTransacciones);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los datos del saldo de los planes
		/// </summary>
		/// <param name="Facturador"></param>
		/// <returns>Rotorna un objeto anonimo con los datos del saldo</returns>
		[HttpGet]
		[Route("api/ObtenerSaldo")]
		public IHttpActionResult ObtnerSaldo(string Facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Facturador = Sesion.DatosEmpresa.StrIdentificacion;

				//Busco documentos disponibles
				Ctl_PlanesTransacciones CtrPlanes = new Ctl_PlanesTransacciones();
				var Planes = CtrPlanes.obtenerSaldoDisponibles(Facturador);

				var IntObligado = Sesion.DatosEmpresa.IntObligado;
				//Sin saldo
				if (Planes == null)
				{
					//Facturador sin Saldo
					if (IntObligado)
					{
						Planes = new { Facturador = IntObligado, Planes = 0, TProcesadas = 0, TCompra = 0, TDisponible = 0, Porcentaje = 0 };
					}
					else
					{
						//Adquiriente
						Planes = new { Facturador = IntObligado };
					}

				}

				return Ok(Planes);
			}
			catch (Exception)
			{
				return Ok();
			}

		}




	}
}
