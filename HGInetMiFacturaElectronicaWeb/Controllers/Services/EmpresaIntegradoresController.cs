using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EmpresaIntegradoresController : ApiController
	{

		[HttpPost]
		[Route("api/GuardarEmpresaIntegradores")]
		public IHttpActionResult GuardarEmpresaIntegradores(ObjEmpresaIntegradores obj_empresa_integradores)
		{
			Sesion.ValidarSesion();

			if (obj_empresa_integradores == null)
			{
				throw new ApplicationException("Objeto de integradores null");
			}

			try
			{

				Ctl_EmpresaIntegradores _empresa_integradores = new Ctl_EmpresaIntegradores();
				List<TblEmpresaIntegradores> datos = _empresa_integradores.Obtener(obj_empresa_integradores.Identificacion);

				foreach (var item in datos)
				{
					_empresa_integradores.Delete(item);
				}

				foreach (var item in obj_empresa_integradores.Integradores)
				{
					TblEmpresaIntegradores obj = new TblEmpresaIntegradores();
					obj.StrIdentificacionEmp = obj_empresa_integradores.Identificacion;
					obj.StrIdentificacionInt = item;
					obj.StrIdSeguridad = Guid.NewGuid();

					_empresa_integradores.Add(obj);
				}

				return Ok();
			}

			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("api/ObtenerEmpresaIntegradores")]
		public IHttpActionResult ObtenerEmpresaIntegradores(string identificacion)
		{
			Sesion.ValidarSesion();
			
			try
			{
				List<string> resultado = new List<string>();


				Ctl_EmpresaIntegradores _empresa_integradores = new Ctl_EmpresaIntegradores();
				List<TblEmpresaIntegradores> datos = _empresa_integradores.Obtener(identificacion);

				foreach (var item in datos)
				{
					resultado.Add(item.StrIdentificacionInt);
				}

				return Ok(resultado);
			}

			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public class ObjEmpresaIntegradores
		{
			public string Identificacion { get; set; }
			public List<string> Integradores { get; set; }

		}

	}
}
