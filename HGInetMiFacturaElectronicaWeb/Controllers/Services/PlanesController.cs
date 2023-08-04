using HGICtrlUtilidades;
using HGICtrlUtilidades.Recursos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using static HGInetMiFacturaElectronicaWeb.Controllers.Services.PlanesTransaccionesController;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class PlanesController : ApiController
	{
		[HttpGet]
		[Route("api/ObtenerSaldoHgiDocs")]
		public IHttpActionResult ObtenerSaldoHgiDocs(string Facturador)
		{
			try
			{
				//Busco documentos disponibles
				Ctl_PlanesTransacciones CtrPlanes = new Ctl_PlanesTransacciones();
				var Planes = CtrPlanes.obtenerSaldoDisponibles(Facturador);

				//Sin saldo
				if (Planes == null)
				{
					Planes = new { Facturador = 1, Planes = 0, TProcesadas = 0, TCompra = 0, TDisponible = 0, Porcentaje = 0 };
				}

				return Ok(Planes);
			}
			catch (Exception)
			{
				return Ok();
			}
		}

		[HttpGet]
		[Route("api/ObtenerPlanesHgiDocs")]
		public IHttpActionResult ObtenerPlanesHgiDocs(string Facturador)
		{
			try
			{
				//Busco documentos disponibles
				Ctl_PlanesTransacciones CtrPlanes = new Ctl_PlanesTransacciones();
				var Planes = CtrPlanes.obtenerPlanesHgiDocs(Facturador);

				//Sin saldo
				if (Planes == null)
				{
					Planes = new { Identificacion = Facturador, FechaCompra = 0, TProcesadas = 0, TCompra = 0, TDisponible = 0, Porcentaje = 0, Valor = 0 };
				}

				return Ok(Planes);
			}
			catch (Exception)
			{
				return Ok();
			}

		}

		/// <summary>
		/// Servicio para crear los planes, solo desde happgi
		/// </summary>
		/// <param name="identificador_unico">Guid de seguridad que debe enviar happgi</param>
		/// <param name="facturador">Facturador que hizo la recarga</param>
		/// <param name="cantidad">Cantidad</param>
		/// <param name="valor_total">Valor de la recarga</param>
		/// <param name="fecha_vencimiento">Fecha de vencimiento</param>		
		/// <param name="Objeto">Objeto del pago</param>
		/// <returns></returns>
		[HttpPost]
		[Route("api/RecargaDocumentosHappgi")]
		public IHttpActionResult RecargaDocumentosHappgi(Guid identificador_unico, string facturador, int cantidad, decimal valor_total, DateTime fecha_vencimiento, RespuestaHGIPagos Objeto)
		{
			try
			{
				//Se valida que el id sea el mismo para evitar 
				Guid id = Guid.Parse("1f23ea1564af496f93731d46583f13b5");
				if (identificador_unico != id)
				{
					throw new ApplicationException("Identificador unico, incorrecto");
				}

				Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();

				TblPlanesTransacciones postpago = clase_planes.Obtener(facturador).FirstOrDefault(x => x.IntEstado == 0 && (x.IntTipoProceso == 3 || (!string.IsNullOrEmpty(x.DocumentoRef) && x.DocumentoRef.Equals("-1"))));


				if (postpago == null)
				{
					TblPlanesTransacciones ObjPlanTransacciones = new TblPlanesTransacciones();
					ObjPlanTransacciones.IntTipoProceso = Convert.ToByte(TipoCompra.Compra.GetHashCode());
					ObjPlanTransacciones.StrEmpresaUsuario = facturador;
					ObjPlanTransacciones.StrUsuario = facturador;
					ObjPlanTransacciones.IntNumTransaccCompra = cantidad;
					ObjPlanTransacciones.IntNumTransaccProcesadas = 0;
					ObjPlanTransacciones.IntValor = valor_total;
					ObjPlanTransacciones.IntEstado = EstadoPlan.Habilitado.GetHashCode();
					ObjPlanTransacciones.StrObservaciones = string.Format("Pendiente por Facturar, pero cancelado con ticket Nº {0} de HGIPay", Objeto.TicketId);
					ObjPlanTransacciones.StrEmpresaFacturador = facturador;
					ObjPlanTransacciones.DocumentoRef = "-1";
					ObjPlanTransacciones.DatFechaVencimiento = fecha_vencimiento;
					ObjPlanTransacciones.DatFechaInicio = DateTime.Now;

					//Se valida si el numero de documentos comprados es mayor a 500 
					// entonces lo estan haciendo fuera del proceso de happgi ya que este permite solo los planes de la pagina
					if (ObjPlanTransacciones.IntNumTransaccCompra >= 500)
					{
						return Ok("Compra de documentos supera la cantidad de documentos permitidos <br /><br /> Cualquier inquietud comunicarse con nuestra area de Soporte.");
					}
					ObjPlanTransacciones.IntMesesVence = 12;
					ObjPlanTransacciones.IntTipoDocumento = 0;

					clase_planes.Crear(ObjPlanTransacciones, false);

					return Ok("Recarga Exitosa");
				}
				else if (postpago != null && postpago.IntTipoProceso == 3)
				{
					return Ok("El Facturador Electrónico tiene registrado un plan Postpago que no le permite activar uno nuevo.<br /><br /> Cualquier inquietud comunicarse con nuestra area de Soporte.");
				}
				else if (postpago != null && postpago.DocumentoRef.Equals("-1"))
				{
					return Ok("El Facturador Electrónico cuenta con un plan activo,esta pendiente por parte de HGI SAS la generacion de la Factura Electrónica.<br /> Cualquier inquietud comunicarse con nuestra area comercial.");
				}
				else
				{
					return Ok("El Facturador Electrónico tiene registrado un tipo de plan que no le permite activar uno nuevo.<br /><br /> Cualquier inquietud comunicarse con nuestra area de Soporte.");
				}

			}
			catch (Exception ex)
			{
				return Ok(ex.Message);
			}

		}

		public class RespuestaHGIPagos
		{
			public string FechaRegistro { get; set; }
			public string FechaDocumento { get; set; }
			public string FechaConfirmacion { get; set; }
			public string IdSeguridad { get; set; }
			public string TicketId { get; set; }
			public string TransaccionCUS { get; set; }
			public string Estado { get; set; }
			public string CodigoBanco { get; set; }
			public string Banco { get; set; }
			public string Descripcion { get; set; }
			public string Franquicia { get; set; }
			public string MensajeVerificacion { get; set; }
			public string Referencia1 { get; set; }
			public string Referencia2 { get; set; }
			public string Referencia3 { get; set; }
			public string RutaDestino { get; set; }
			public string RutaSync { get; set; }
			public string Valor { get; set; }
			public string MetodoPago { get; set; }
			public string ValorIva { get; set; }
			public string Documento { get; set; }
			public string Ciclo { get; set; }
			public string CodAprobacion { get; set; }

			public string StrClienteIdentificacion { get; set; }
			public string StrClienteNombre { get; set; }
			public string StrClienteEmail { get; set; }
			public string StrClienteTelefono { get; set; }

		}
	}


}
