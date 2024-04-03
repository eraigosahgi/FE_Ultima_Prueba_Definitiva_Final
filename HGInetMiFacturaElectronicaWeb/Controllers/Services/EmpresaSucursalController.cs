using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EmpresaSucursalController : ApiController
	{
		[HttpGet]
		[Route("api/ObtenerEmpresaSucursal")]
		public IHttpActionResult ObtenerEmpresaSucursal(string IdentificacionEmpresa)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EmpresaSucursales _Sucursal = new Ctl_EmpresaSucursales();

				List<TblEmpresaSucursal> sucursales = new List<TblEmpresaSucursal>();

				sucursales = _Sucursal.Obtener(IdentificacionEmpresa);

				Sucursales suc = new Sucursales();

				List<Sucursales> lista_suc = new List<Sucursales>();

				foreach (var item in sucursales)
				{
					suc = new Sucursales();
					suc.id = item.IntCodigoSucursal;
					suc.sucursal = item.StrDescSucursal;
					lista_suc.Add(suc);
				}
				if (lista_suc.Count() == 0)
				{
					suc = new Sucursales();
					suc.id = 0;
					suc.sucursal = "GENERAL";
					lista_suc.Add(suc);
				}

				return Ok(lista_suc);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public class Sucursales
		{
			public string empresa { get; set; }
			public int id { get; set; }
			public string sucursal { get; set; }

		}


		[HttpPost]
		[Route("api/GuardarEmpresaSucursal")]
		public IHttpActionResult GuardarEmpresaSucursal(Sucursales sucursal)
		{
			Sesion.ValidarSesion();

			try
			{
				Ctl_EmpresaSucursales _Sucursal = new Ctl_EmpresaSucursales();

				TblEmpresaSucursal empresa_sucursal = new TblEmpresaSucursal();

				empresa_sucursal.StrIdentificacion = sucursal.empresa;
				empresa_sucursal.IntCodigoSucursal = sucursal.id;
				empresa_sucursal.StrDescSucursal = sucursal.sucursal;

				_Sucursal.Add(empresa_sucursal);

				return Ok();
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					if (ex.InnerException.InnerException.Message.Contains("PRIMARY KEY"))
					{
						throw new ApplicationException("Sucursal ya existe");
					}
				}
				throw ex;
			}
		}

		[HttpDelete]
		[Route("api/EliminarEmpresaSucursal")]
		public IHttpActionResult EliminarEmpresaSucursal(string empresa, int id)
		{
			Sesion.ValidarSesion();

			try
			{
				Ctl_EmpresaSucursales _Sucursal = new Ctl_EmpresaSucursales();

				TblEmpresaSucursal empresa_sucursal = new TblEmpresaSucursal();


				var sucur = _Sucursal.Obtener(empresa, id);

				_Sucursal.Delete(sucur);

				return Ok();
			}
			catch (Exception ex)
			{
				if (id == 0)
				{
					throw new ApplicationException("No se puede eliminar la sucursal 0");
				}
				throw ex;
			}
		}
	}
}