using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_EmpresaSucursales : BaseObject<TblEmpresaSucursal>
	{
		#region Constructores 

		public Ctl_EmpresaSucursales() : base(new ModeloAutenticacion()) { }
		public Ctl_EmpresaSucursales(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_EmpresaSucursales(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion



		public List<TblEmpresaSucursal> Obtener(string Identificacion, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblEmpresaSucursal> datos = (from item in context.TblEmpresaSucursal
											  where item.StrIdentificacion.Equals(Identificacion)
											  select item).ToList();

			return datos;
		}

		public TblEmpresaSucursal Obtener(string Identificacion, int id_sucursal, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			TblEmpresaSucursal datos = (from item in context.TblEmpresaSucursal
											  where item.StrIdentificacion.Equals(Identificacion) && item.IntCodigoSucursal == id_sucursal
											  select item).FirstOrDefault();

			return datos;
		}


		public TblEmpresaSucursal Crear(TblEmpresaSucursal Sucursal)
		{
			Sucursal = this.Add(Sucursal);

			return Sucursal;
		}

	}
}
