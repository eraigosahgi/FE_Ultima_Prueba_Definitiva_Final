using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.Objetos;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.ApiServicios.Controller.Services
{
	public class DocumentosApiController : ApiController
	{

		[HttpGet]
		[Route("Api/DocumentosApi/ConsultaPorNumeros")]
		public HttpResponseMessage ConsultaPorNumeros(string DataKey, string Identificacion, int TipoDocumento, string Numeros)
		{
			try
			{
				List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				respuesta = ctl_documento.ConsultaPorNumeros(Identificacion, TipoDocumento, Numeros);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorNumeros", DataKey, Identificacion, TipoDocumento.ToString(), Numeros, respuesta.Count.ToString());
				}
				catch (Exception)
				{
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
		[Route("Api/DocumentosApi/ConsultaPorCodigoRegistro")]
		public HttpResponseMessage ConsultaPorCodigoRegistro(string DataKey, string Identificacion, int TipoDocumento, string CodigosRegistros)
		{
			try
			{
				List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				respuesta = ctl_documento.ConsultaPorCodigoRegistro(Identificacion, TipoDocumento, CodigosRegistros);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorCodigoRegistro", DataKey, Identificacion, TipoDocumento.ToString(), CodigosRegistros.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
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
		[Route("Api/DocumentosApi/ConsultaPorFechaElaboracion")]
		public HttpResponseMessage ConsultaPorFechaElaboracion(string DataKey, string Identificacion, int TipoDocumento, DateTime FechaInicial, DateTime FechaFinal)
		{
			try
			{
				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

				if (FechaInicial == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

				if (FechaFinal < FechaInicial)
					throw new ApplicationException("Fecha final inválida.");

				long dif_fecha = LibreriaGlobalHGInet.Funciones.Fecha.Diferencia(FechaInicial, FechaFinal, LibreriaGlobalHGInet.Funciones.Fecha.DateInterval.Day);

				if (dif_fecha > 30)
					throw new ApplicationException("La consulta supera el maximo de 30 dias; por favor realice la consulta teniendo en cuenta este maximo");

				List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				respuesta = ctl_documento.ConsultaPorFechaElaboracion(Identificacion, TipoDocumento, FechaInicial, FechaFinal);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorFechaElaboracion", DataKey, Identificacion, TipoDocumento.ToString(), FechaInicial.ToString(), FechaFinal.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				return Request.CreateResponse(HttpStatusCode.OK, respuesta);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				return Request.CreateResponse(HttpStatusCode.Conflict, exec.Message);
			}
		}


		[HttpPost]
		[Route("Api/DocumentosApi/Recepcion")]
		public HttpResponseMessage Recepcion(List<DocumentoArchivo> documentos)
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

		[HttpPost]
		[Route("Api/DocumentosApi/ObtenerCufe")]
		public HttpResponseMessage ObtenerCufe(List<DocumentoCufe> documentos_cufe)
		{

			try
			{
				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				documentos_cufe = ctl_documento.ObtenerCufe(documentos_cufe);

				return Request.CreateResponse(HttpStatusCode.OK, documentos_cufe);
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}


		[HttpGet]
		[Route("Api/ObtenerDocumentosRechazado")]
		public HttpResponseMessage ObtenerDocumentosRechazado(DateTime FechaInicial, DateTime FechaFinal)
		{
			try
			{

				if (FechaInicial == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

				if (FechaFinal < FechaInicial)
					throw new ApplicationException("Fecha final inválida.");

				long dif_fecha = LibreriaGlobalHGInet.Funciones.Fecha.Diferencia(FechaInicial, FechaFinal, LibreriaGlobalHGInet.Funciones.Fecha.DateInterval.Day);

				if (dif_fecha > 5)
					throw new ApplicationException("La consulta supera el maximo de 5 dias; por favor realice la consulta teniendo en cuenta este maximo");


				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				List<ObjDocumentos> respuesta = ctl_documento.ObtenerDocumentosRechazado(FechaInicial, FechaFinal);

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
