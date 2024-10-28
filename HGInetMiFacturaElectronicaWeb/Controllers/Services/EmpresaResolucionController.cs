﻿using HGInetMiFacturaElectonicaController;
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
using HGInetMiFacturaElectonicaData.Objetos;
using HGInetMiFacturaElectonicaController.Procesos;

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
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				throw;
			}
		}



		[HttpGet]
		[Route("api/ActualizarConDIAN")]
		public IHttpActionResult ActualizarConDIAN(string codigo_facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				if (string.IsNullOrEmpty(codigo_facturador))
				{
					throw new Exception("Debe seleccionar el Facturador");
				}
				var datos = Ctl_Resoluciones.Obtener(codigo_facturador);

				return Ok();
			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				throw;
			}
		}


		[HttpGet]
		[Route("api/ObtenerResolucionesEnum")]
		public IHttpActionResult ObtenerResolucionesEnum(string codigo_facturador)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

				List<TblEmpresasResoluciones> datos = Resolucion.ObtenerResolucionesPorTipo(codigo_facturador, TipoDocumento.Factura.GetHashCode());

				var retorno = datos.Select(d => new
				{
					ID = d.StrNumResolucion,
					Descripcion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntTipoDoc)) + "-" + ((d.StrPrefijo == null) ? "S / PREFIJO" : (!d.StrPrefijo.Equals("")) ? d.StrPrefijo : "S/PREFIJO") + ((d.IntTipoDoc == 1) ? "-" + d.StrNumResolucion : "")
				});

				return Ok(retorno);
			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
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

				try
				{
					if (string.IsNullOrEmpty(codigo_facturador))
					{
						codigo_facturador = Sesion.DatosUsuario.StrEmpresa;
					}
				}
				catch (Exception)
				{
				}


				List<TblEmpresasResoluciones> datos = Resolucion.ObtenerResoluciones(codigo_facturador, "*");

				var retorno = datos.Select(d => new
				{
					ID = d.StrNumResolucion,
					Descripcion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntTipoDoc)) + "-" + ((d.StrPrefijo == null) ? "S / PREFIJO" : (!d.StrPrefijo.Equals("")) ? d.StrPrefijo : "S/PREFIJO") + ((d.IntTipoDoc == 1) ? "-" + d.StrNumResolucion : "")
				});

				return Ok(retorno);
			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
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
		public IHttpActionResult ObtenerResoluciones(string codigo_facturador, string codigo_Resolucion)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

				Ctl_Empresa CtEmpresa = new Ctl_Empresa();

				TblEmpresas empresa = new TblEmpresas();


				empresa = CtEmpresa.Obtener(codigo_facturador);

				List<TblEmpresasResoluciones> datos = new List<TblEmpresasResoluciones>();

				if (empresa.IntAdministrador)
				{


					if (codigo_Resolucion != "*")
					{
						datos = Resolucion.Obtener(codigo_facturador, codigo_Resolucion, "*", "*");
					}
					else
					{
						datos = Resolucion.Obtener("*", codigo_Resolucion, "*", "*");
					}
				}
				else
				{
					if (empresa.IntIntegrador)
					{
						datos = Resolucion.ObtenerAsociadas(codigo_facturador, codigo_Resolucion, "*", "*");
					}
					else
					{
						datos = Resolucion.Obtener(codigo_facturador, codigo_Resolucion, "*", "*");
					}
				}



				//List<TblEmpresasResoluciones> datos = Resolucion.Obtener(codigo_facturador, "*", "*", "*");

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
					PermiteParciales = d.PermiteParciales,
					IdSetDian = d.StrIdSetDian,
					VersionDian = d.IntVersionDian,
					ComercioConfigId = d.ComercioConfigId,
					ComercioConfigDescrip = d.ComercioConfigDescrip,
					SerialCloud = (d.TblEmpresas.IntManejaPagoE) ? d.TblEmpresas.StrSerialCloudServices : "",
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

		#region Comercios de Pago
		[HttpGet]
		[Route("Api/EditarConfigPago")]
		public IHttpActionResult EditarConfigPago(string Stridseguridad, bool Permitepagosparciales, string IdComercio, string DescripcionComercio)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Controlador = new Ctl_EmpresaResolucion();

				if (!string.IsNullOrEmpty(DescripcionComercio) || !string.IsNullOrEmpty(IdComercio))
				{
					if (string.IsNullOrEmpty(DescripcionComercio))
					{
						throw new ApplicationException("Debe indicar una descripción para la configuración de comercio");
					}
				}
				var Datos = Controlador.EditarConfigPago(Guid.Parse(Stridseguridad), Permitepagosparciales, IdComercio, DescripcionComercio);

				return Ok();

			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, "");
				throw;
			}
		}



		[HttpGet]
		[Route("Api/ObtenerComercios")]
		public IHttpActionResult ObtenerComercios(string codigo_facturador, string serial_cloudservices)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaResolucion Controlador = new Ctl_EmpresaResolucion();

				//try
				//{
				//	codigo_facturador = Sesion.DatosUsuario.StrEmpresa;
				//}
				//catch (Exception)
				//{
				//}

				var Datos = Controlador.ObtenerComercios(codigo_facturador, serial_cloudservices);

				return Ok(Datos);

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

