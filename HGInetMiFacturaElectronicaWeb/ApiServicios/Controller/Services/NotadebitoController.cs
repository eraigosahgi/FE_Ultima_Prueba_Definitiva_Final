using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
    public class NotadebitoController : ApiController
    {
		[HttpPost]
		[Route("Api/Notadebito/Recepcion")]
		public HttpResponseMessage Recepcion(List<NotaDebito> documentos)
		{

			try
			{
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
		[Route("Api/Notadebito/ObtenerPorFechasAdquiriente")]
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

				List<NotaDebitoConsulta> respuesta = new List<NotaDebitoConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_NotaDebito ctl_documento = new Ctl_NotaDebito();

				// obtiene los datos
				respuesta = ctl_documento.ObtenerPorFechasAdquiriente(Identificacion, FechaInicio, FechaFinal);

				DateTime fecha_corte = new DateTime(2024, 01, 01, 00, 00, 00);

				bool obtener_historico = true;

				if (FechaInicio >= fecha_corte)
				{
					obtener_historico = false;
				}

				if (obtener_historico == true)
				{
					List<NotaDebitoConsulta> datosH = new List<NotaDebitoConsulta>();

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
		[Route("Api/Notadebito/ObtenerHisPorFechasAdquiriente")]
		public HttpResponseMessage ObtenerHisPorFechasAdquiriente(string Identificacion, DateTime FechaInicio, DateTime FechaFinal)
		{
			try
			{

				DateTime fecha_corte = new DateTime(2023, 12, 31, 00, 00, 00);

				if (FechaFinal >= fecha_corte)
					FechaFinal = fecha_corte;

				Ctl_NotaDebito ctl_documento = new Ctl_NotaDebito();

				// obtiene los datos
				List<NotaDebitoConsulta> respuesta = ctl_documento.ObtenerPorFechasAdquiriente(Identificacion, FechaInicio, FechaFinal);


				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}


		[HttpGet]
		[Route("Api/Notadebito/ObtenerPorIdSeguridadAdquiriente")]
		public HttpResponseMessage ObtenerPorIdSeguridadAdquiriente(string DataKey, string Identificacion, string CodigosRegistros, bool Facturador = false)
		{
			try
			{
				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");


				List<NotaDebitoConsulta> respuesta = new List<NotaDebitoConsulta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_NotaDebito ctl_documento = new Ctl_NotaDebito();

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

		[HttpGet]
		[Route("Api/Notadebito/ObtenerHisPorIdSeguridadAdquiriente")]
		public HttpResponseMessage ObtenerHisPorIdSeguridadAdquiriente(string Identificacion, string CodigosRegistros, bool Facturador = false)
		{
			try
			{
				List<NotaDebitoConsulta> respuesta = new List<NotaDebitoConsulta>();

				Ctl_NotaDebito ctl_documento = new Ctl_NotaDebito();

				// obtiene los datos
				respuesta = ctl_documento.ObtenerPorIdSeguridadAdquiriente(Identificacion, CodigosRegistros, Facturador);

				if (respuesta == null || respuesta.Count == 0)
				{
					List<NotaDebitoConsulta> datosH = new List<NotaDebitoConsulta>();

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
				}

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
