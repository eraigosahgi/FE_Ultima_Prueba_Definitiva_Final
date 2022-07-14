using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Formato;
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

		public List<TblRegistroRecepcion> ObtenerPorFecha(DateTime Fecha_ini, DateTime Fecha_fin, string estado, int Desde, int Hasta)
		{

			context.Configuration.LazyLoadingEnabled = false;

			List<int> LstEstado = new List<int>();
			if (string.IsNullOrEmpty(estado))
			{
				estado = "*";
			}
			else
			{
				LstEstado = Coleccion.ConvertirStringInt(estado);
			}

			Fecha_ini = Fecha_ini.Date;

			Fecha_fin = new DateTime(Fecha_fin.Year, Fecha_fin.Month, Fecha_fin.Day, 23, 59, 59, 999);

			List<TblRegistroRecepcion> datos = (from item in context.TblRegistroRecepcion
												where item.DatFechaRegistro >= Fecha_ini && item.DatFechaRegistro <= Fecha_fin
												&& (LstEstado.Contains(item.IntEstado) || estado == "*")
												select item).OrderByDescending(x => x.DatFechaRegistro).Skip(Desde).Take(Hasta).ToList();

			return datos;

		}

	}
}
