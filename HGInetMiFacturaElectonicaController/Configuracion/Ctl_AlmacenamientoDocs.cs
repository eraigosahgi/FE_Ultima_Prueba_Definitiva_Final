using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_AlmacenamientoDocs : BaseObject<TblAlmacenamientoDocs>
	{
		#region Constructores 

		public Ctl_AlmacenamientoDocs() : base(new ModeloAutenticacion()) { }
		public Ctl_AlmacenamientoDocs(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_AlmacenamientoDocs(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		public List<TblAlmacenamientoDocs> Obtener(bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblAlmacenamientoDocs> datos = (from item in context.TblAlmacenamientoDocs
												 select item).ToList();

			return datos;
		}


		/// <summary>
		/// Obtener todos los archivos sincronizados del documento
		/// </summary>
		/// <param name="IdSeguridad"></param>
		/// <param name="LazyLoading"></param>
		/// <returns></returns>
		public List<TblAlmacenamientoDocs> Obtener(Guid IdSeguridad, bool LazyLoading = false)
		{
			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblAlmacenamientoDocs> datos = (from item in context.TblAlmacenamientoDocs
												 where item.StrIdSeguridadDoc.Equals(IdSeguridad)
											select item).ToList();

			return datos;
		}

		public TblAlmacenamientoDocs Crear(TblAlmacenamientoDocs evento)
		{
			evento = this.Add(evento);

			return evento;
		}

		public TblAlmacenamientoDocs Actualizar(TblAlmacenamientoDocs evento)
		{
			evento = this.Edit(evento);

			return evento;
		}

		/// <summary>
		/// Proceso para llenar tbl
		/// </summary>
		/// <param name="doc_StrIdSeguridad">id del documento Electronico</param>
		/// <param name="fecha_ingreso_doc">Fecha de ingreso del documento electronico a la plataforma</param>
		/// <param name="tipo_archivo">1 - Factura, 2 - Nota Debito, 3 - Nota Credito, 4 Acuse(Ultimo evento registrado), 5 - Respuesta Dian, 10 - Nomina, 11 - Ajuste Nomina, Mayor a 6 los eventos del documento</param>
		/// <param name="ruta_anterior"></param>
		/// <param name="ruta_actual"></param>
		/// <returns></returns>
		public TblAlmacenamientoDocs Convertir(Guid doc_StrIdSeguridad, DateTime fecha_ingreso_doc, int tipo_archivo, string ruta_anterior, string ruta_actual, bool buscar_faltantes)
		{
			TblAlmacenamientoDocs archivo = new TblAlmacenamientoDocs();

			archivo.StrIdSeguridadDoc = doc_StrIdSeguridad;
			archivo.DatFechaRegistroDoc = fecha_ingreso_doc;
			archivo.DatFechaSincronizacion = buscar_faltantes == false ? Fecha.GetFecha() : Fecha.GetFecha().AddSeconds(-2);
			archivo.IntConsecutivo = tipo_archivo;
			archivo.StrUrlAnterior = ruta_anterior;
			archivo.StrUrlActual = ruta_actual;

			return archivo;
		}


	}
}
