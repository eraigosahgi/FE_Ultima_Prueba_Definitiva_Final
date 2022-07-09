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
	public class Ctl_RegistroRecepcion : BaseObject<TblRegistroRecepcion>
	{
		#region Constructores 

		public Ctl_RegistroRecepcion() : base(new ModeloAutenticacion()) { }
		public Ctl_RegistroRecepcion(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_RegistroRecepcion(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		public List<TblRegistroRecepcion> Obtener(bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblRegistroRecepcion> datos = (from item in context.TblRegistroRecepcion
												select item).ToList();

			return datos;
		}


		/// <summary>
		/// Obtener todos los archivos sincronizados del documento
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <param name="LazyLoading"></param>
		/// <returns></returns>
		public TblRegistroRecepcion Obtener(Guid IdSeguridad, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			TblRegistroRecepcion datos = (from item in context.TblRegistroRecepcion
												where item.StrId.Equals(IdSeguridad)
												 select item).FirstOrDefault();

			return datos;
		}

		public TblRegistroRecepcion Crear(TblRegistroRecepcion evento)
		{
			evento = this.Add(evento);

			return evento;
		}

		public TblRegistroRecepcion Actualizar(TblRegistroRecepcion evento)
		{
			evento = this.Edit(evento);

			return evento;
		}

	}
}
