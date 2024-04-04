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
	public class Ctl_Integradores : BaseObject<TblIntegradores>
	{
		#region Constructores 

		public Ctl_Integradores() : base(new ModeloAutenticacion()) { }
		public Ctl_Integradores(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Integradores(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		public TblIntegradores Obtener(string Identificacion, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			TblIntegradores datos = (from item in context.TblIntegradores
									 where item.StrIdentificacion.Equals(Identificacion)
											select item).FirstOrDefault();

			return datos;
		}

		public TblIntegradores Crear(TblIntegradores Integrador)
		{
			Integrador = this.Add(Integrador);

			return Integrador;
		}

	}
}
