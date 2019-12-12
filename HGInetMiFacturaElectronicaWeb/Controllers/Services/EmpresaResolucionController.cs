using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaController.Auditorias;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EmpresaResolucionController : ApiController
	{


		#region Get--Obtener
		/// <summary>
		/// Obtiene la lista de resoluciones de una empresa en especifica
		/// </summary>
		/// <param name="codigo_facturador"></param>        
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult Get(string codigo_facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

				List<TblEmpresasResoluciones> datos = Resolucion.ObtenerResoluciones(codigo_facturador, "*");

				var retorno = datos.Select(d => new
				{
					ID = 1,
					Descripcion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntTipoDoc)) + "-" + ((d.StrPrefijo == null) ? "S / PREFIJO" : (!d.StrPrefijo.Equals("")) ? d.StrPrefijo : "S/PREFIJO") + ((d.IntTipoDoc == 1) ? "-" + d.StrNumResolucion : "")
				});

				return Ok(retorno);
			}
			catch (Exception)
			{

				throw;
			}
		}

		/// <summary>
		/// Obtiene la lista de resoluciones con prefijo para una empresa espeficifa
		/// </summary>
		/// <param name="codigo_facturador"></param>        
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerResPrefijo")]
		public IHttpActionResult ObtenerResPrefijo(string codigo_facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

				List<TblEmpresasResoluciones> datos = Resolucion.ObtenerResoluciones(codigo_facturador, "*");

				var retorno = datos.Select(d => new
				{
					ID = d.StrNumResolucion,
					Descripcion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntTipoDoc)) + "-" + ((d.StrPrefijo == null) ? "S / PREFIJO" : (!d.StrPrefijo.Equals("")) ? d.StrPrefijo : "S/PREFIJO") + ((d.IntTipoDoc == 1) ? "-" + d.StrNumResolucion : "")
				});

				return Ok(retorno);
			}
			catch (Exception)
			{
				throw;
			}
		}




		/// <summary>
		/// Obtiene la lista de resoluciones 
		/// </summary>
		/// <param name="codigo_facturador"></param>        
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerResoluciones")]
		public IHttpActionResult ObtenerResoluciones(string codigo_facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

				List<TblEmpresasResoluciones> datos = Resolucion.Obtener(codigo_facturador, "*", "*", "*");

				var retorno = datos.Select(d => new
				{
					Descripcion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntTipoDoc)) + "-" + ((d.StrPrefijo == null) ? "S / PREFIJO" : (!d.StrPrefijo.Equals("")) ? d.StrPrefijo : "S/PREFIJO") + ((d.IntTipoDoc == 1) ? "-" + d.StrNumResolucion : ""),
					Empresa = d.StrEmpresa,
					NumResolucion = d.StrNumResolucion,
					Prefijo = d.StrPrefijo,
					RangoInicial = d.IntRangoInicial,
					RangoFinal = d.IntRangoFinal,
					FechaVigenciaDesde = d.DatFechaVigenciaDesde,
					FechaVigenciaHasta = d.DatFechaVigenciaHasta,
					ClaveTecnica = d.StrClaveTecnica,
					IdSeguridad = d.StrIdSeguridad,
					Observaciones = d.StrObservaciones,
					FechaIngreso = d.DatFechaIngreso,
					FechaActualizacion = d.DatFechaActualizacion,
					RespuestaServicioWeb = d.StrRespuestaServicioWeb,
					TipoDoc = d.IntTipoDoc,
					PermiteParciales = d.IntPermiteParciales,
					IdSetDian = d.StrIdSetDian,
					VersionDian = d.IntVersionDian,
					ComercioConfigId = d.StrComercioConfigId,
					ComercioConfigDescrip = d.StrComercioConfigDescrip
				});

				return Ok(retorno);
			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				throw;
			}
		}


		#endregion
	}
}

