using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class ImportacionDocController : ApiController
    {
		[HttpPost]
		[Route("api/ImportarArchivo")]
		public IHttpActionResult ImportarArchivo(System.Guid StrIdSeguridad, int Operacion)
		{
			try
			{
				
				var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

				string segundos = Fecha.GetFecha().ToString("ss");

				string extension_file = Path.GetExtension(file.FileName);

				if (!extension_file.Equals(".xlsx") && !extension_file.Equals(".xls"))
					throw new ApplicationException(string.Format("El archivo {0} no es de extension de xlsx ó xls para procesar", file.FileName));

				bool proceso = true;

				if (file != null && file.ContentLength > 0)
				{
					Ctl_ImportacionDoc Controlador = new Ctl_ImportacionDoc();
					proceso = Controlador.ImportarArchivo(file, StrIdSeguridad, Operacion);
				}

				if (proceso == false)
				{
					return NotFound();
				}

				return Ok(proceso);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("api/ObtenerDocRecibidos")]
		public IHttpActionResult ObtenerDocRecibidos(Guid StrIdSeguridad_Empresa)
		{
			Sesion.ValidarSesion();

			TblEmpresas empresa = new TblEmpresas();

			Ctl_Empresa CtEmpresa = new Ctl_Empresa();

			empresa = CtEmpresa.Obtener(StrIdSeguridad_Empresa).FirstOrDefault();

			List<TblImportacionDoc> datos = new List<TblImportacionDoc>();

			Ctl_ImportacionDoc Controlador = new Ctl_ImportacionDoc();

			if (empresa != null)
			{
				datos = Controlador.Obtener(empresa.StrIdentificacion, 1);
			}
			else
			{
				return NotFound();
			}

			if (datos == null)
			{
				return NotFound();
			}

			return Ok(datos);
		}

		[HttpGet]
		[Route("api/ObtenerDocEmitidos")]
		public IHttpActionResult ObtenerDocEmitidos(Guid StrIdSeguridad_Empresa)
		{
			Sesion.ValidarSesion();

			TblEmpresas empresa = new TblEmpresas();

			Ctl_Empresa CtEmpresa = new Ctl_Empresa();

			empresa = CtEmpresa.Obtener(StrIdSeguridad_Empresa).FirstOrDefault();

			List<TblImportacionDoc> datos = new List<TblImportacionDoc>();

			Ctl_ImportacionDoc Controlador = new Ctl_ImportacionDoc();

			if (empresa != null)
			{
				datos = Controlador.Obtener(empresa.StrIdentificacion, 0);
			}
			else
			{
				return NotFound();
			}

			if (datos == null)
			{
				return NotFound();
			}

			return Ok(datos);
		}

		[HttpPost]
		[Route("api/ProcesarDocs")]
		public IHttpActionResult ProcesarDocs(string lista_documentos, int Operacion)
		{
			try
			{
				Ctl_ImportacionDoc Controlador = new Ctl_ImportacionDoc();
			
				List<Guid> List_id_seguridad = new List<Guid>();

				List<string> lista_ = Coleccion.ConvertirLista(lista_documentos, ',');

				foreach (var item in lista_)
				{
					List_id_seguridad.Add(Guid.Parse(item));
				}

				List<TblImportacionDoc> Lista_Importa = Controlador.ObtenerDocImportados(List_id_seguridad);

				var datos = Controlador.ProcesarDocImp(Lista_Importa, Operacion);

				//var retorno = datos.Select(d => new
				//{
				//	d.IdSeguridad,
				//	d.IdProceso,
				//	d.StrObservaciones,
				//	d.StrPrefijo,
				//	d.IntNumero,
				//	d.StrCufe,
				//	d.StrEmpresaFacturador,
				//	d.StrNombreFacturador,
				//	d.StrTipodoc,
				//	idTipoDoc = Enumeracion.GetValueFromDescription<TipoDocumento>(d.StrTipodoc),
				//	d.DatFechaRecepcion,
				//	d.DatFechaEmision,
				//	d.StrEstadoDian,
				//	d.IntVlrIva,
				//	d.IntVlrIca,
				//	d.IntVlrIPC,
				//	d.IntVlrTotal,
				//	d.StrGrupo									   
				//});

				return Ok(datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
