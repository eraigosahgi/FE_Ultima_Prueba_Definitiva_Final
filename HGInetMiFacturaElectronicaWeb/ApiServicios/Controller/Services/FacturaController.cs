using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
    public class FacturaController : ApiController
    {
		[HttpPost]
		[Route("Api/Factura/Recepcion")]
		public HttpResponseMessage Recepcion(List<Factura> documentos)
		{

			try
			{
				//List<Factura> datos = new List<Factura>();
				//datos.Add(documentos);

				//return Request.CreateResponse(HttpStatusCode.NotFound, new FaultReason("Sitio no habilitado para peticiones de recepción"));

				List<DocumentoRespuesta> lista_registros = Ctl_Documentos.Procesar(documentos);

				return Request.CreateResponse(HttpStatusCode.OK, lista_registros);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		[HttpGet]
		[Route("Api/Factura/ObtenerPorFechasAdquiriente")]
		public HttpResponseMessage ObtenerPorFechasAdquiriente(string DataKey, string Identificacion, DateTime FechaInicio, DateTime FechaFinal, int Procesados_ERP = 0)
		{
			try
			{
				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

				if (FechaInicio == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

				if (FechaFinal < FechaInicio)
					throw new ApplicationException("Fecha final inválida.");

				List<FacturaConsulta> respuesta = new List<FacturaConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Factura ctl_documento = new Ctl_Factura();

				// obtiene los datos
				respuesta = ctl_documento.ObtenerPorFechasAdquiriente(Identificacion, FechaInicio, FechaFinal, Procesados_ERP);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ObtenerPorFechasAdquiriente", DataKey, Identificacion, FechaInicio.ToString(), FechaFinal.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				DateTime fecha_corte = new DateTime(2024, 01, 01, 00, 00, 00);

				bool obtener_historico = true;

				if (FechaInicio >= fecha_corte)
				{
					obtener_historico = false;
				}

				if (obtener_historico == true)
				{
					List<FacturaConsulta> datosH = new List<FacturaConsulta>();

					datosH = ctl_documento.ObtenerHisPorFechasAdquiriente(Identificacion, FechaInicio, FechaFinal, Procesados_ERP);

					if (datosH != null && datosH.Count > 0)
					{
						if (respuesta != null && respuesta.Count > 0)
						{
							respuesta.AddRange(datosH);
						}
						else
						{
							respuesta = datosH;
						}

					}
				}

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}

		[HttpGet]
		[Route("Api/Factura/ObtenerHisPorFechasAdquiriente")]
		public HttpResponseMessage ObtenerHisPorFechasAdquiriente(string Identificacion, DateTime FechaInicio, DateTime FechaFinal, int Procesados_ERP = 0)
		{
			try
			{

				DateTime fecha_corte = new DateTime(2023, 12, 31, 00, 00, 00);

				if (FechaFinal >= fecha_corte)
					FechaFinal = fecha_corte;

				Ctl_Factura ctl_documento = new Ctl_Factura();

				// obtiene los datos
				List<FacturaConsulta> respuesta = ctl_documento.ObtenerPorFechasAdquiriente(Identificacion, FechaInicio, FechaFinal, Procesados_ERP);
 

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}


		[HttpGet]
		[Route("Api/Factura/ObtenerPorIdSeguridadAdquiriente")]
		public HttpResponseMessage ObtenerPorIdSeguridadAdquiriente(string DataKey, string Identificacion, string CodigosRegistros, bool Facturador = false)
		{
			try
			{
				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");


				List<FacturaConsulta> respuesta = new List<FacturaConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Factura ctl_documento = new Ctl_Factura();

				// obtiene los datos
				respuesta = ctl_documento.ObtenerPorIdSeguridadAdquiriente(Identificacion, CodigosRegistros, Facturador);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ObtenerPorIdSeguridadAdquiriente", DataKey, Identificacion, CodigosRegistros, respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				if (respuesta == null || respuesta.Count == 0)
				{
					List<FacturaConsulta> datosH = new List<FacturaConsulta>();

					datosH = ctl_documento.ObtenerHisPorIdSeguridadAdquiriente(Identificacion, CodigosRegistros, Facturador);

					if (datosH != null && datosH.Count > 0)
					{
						if (respuesta != null && respuesta.Count > 0)
						{
							respuesta.AddRange(datosH);
						}
						else
						{
							respuesta = datosH;
						}

					}

					//Almacena la petición
					try
					{
						Task tarea = Peticion.GuardarPeticionAsync("ObtenerHisPorFechasAdquiriente", DataKey, Identificacion, CodigosRegistros, respuesta.Count.ToString());
					}
					catch (Exception)
					{
					}
				}

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}

		[HttpGet]
		[Route("Api/Factura/ObtenerHisPorIdSeguridadAdquiriente")]
		public HttpResponseMessage ObtenerHisPorIdSeguridadAdquiriente(string Identificacion, string CodigosRegistros, bool Facturador = false)
		{
			try
			{
				List<FacturaConsulta> respuesta = new List<FacturaConsulta>();

				Ctl_Factura ctl_documento = new Ctl_Factura();

				// obtiene los datos
				respuesta = ctl_documento.ObtenerPorIdSeguridadAdquiriente(Identificacion, CodigosRegistros, Facturador);

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}

	}
}
