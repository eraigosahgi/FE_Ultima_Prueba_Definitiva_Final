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
	public class Ctl_EmpresaIntegradores : BaseObject<TblEmpresaIntegradores>
	{
		#region Constructores 

		public Ctl_EmpresaIntegradores() : base(new ModeloAutenticacion()) { }
		public Ctl_EmpresaIntegradores(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_EmpresaIntegradores(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		public List<TblEmpresaIntegradores> Obtener(string Identificacion, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblEmpresaIntegradores> datos = (from item in context.TblEmpresaIntegradores
												  where item.StrIdentificacionEmp.Equals(Identificacion)
											  select item).ToList();

			return datos;
		}

		public TblEmpresaIntegradores Obtener(string Identificacion, int id_integrador, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			TblEmpresaIntegradores datos = (from item in context.TblEmpresaIntegradores
											where item.StrIdentificacionEmp.Equals(Identificacion) && item.StrIdentificacionInt.Equals(id_integrador)
										select item).FirstOrDefault();

			return datos;
		}

		public TblEmpresaIntegradores Crear(TblEmpresaIntegradores Integrador)
		{
			Integrador = this.Add(Integrador);

			return Integrador;
		}

		public List<TblEmpresaIntegradores> ObtenerEmpresas(string IdentificacionInt, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblEmpresaIntegradores> datos = (from item in context.TblEmpresaIntegradores
												  where item.StrIdentificacionInt.Equals(IdentificacionInt)
												  select item).ToList();

			return datos;
		}

	}
}
