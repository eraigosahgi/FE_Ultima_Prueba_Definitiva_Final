using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para procesos de Resoluciones
	/// </summary>
	public class Ctl_Resoluciones
	{
		/// <summary>
		/// Obtiene las resoluciones de facturación para el obligado
		/// </summary>
		/// <param name="identificacion_obligado">número de identificación</param>
		/// <returns>resoluciones</returns>
		public static List<Resolucion> Obtener(string identificacion_obligado)
		{
			List<Resolucion> resoluciones_respuesta = new List<Resolucion>();

			Guid id_peticion = Guid.NewGuid();

			Ctl_Empresa Peticion = new Ctl_Empresa();

			//obtiene los datos de la empresa
			TblEmpresas facturador_electronico = Peticion.Obtener(identificacion_obligado);

			List<TblEmpresasResoluciones> resoluciones_bd = null;

			if (facturador_electronico.IntVersionDian == 1)
			{
				resoluciones_bd = Actualizar(id_peticion, facturador_electronico);
			}
			else if (facturador_electronico.IntVersionDian == 2)
			{
				//obtiene las Resoluciones del facturador y las actualiza para que no permita la recepcion de documento
				Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();
				resoluciones_bd = empresa_resolucion.ObtenerResoluciones(identificacion_obligado, "*");
			}

			if (resoluciones_bd != null)
			{
				foreach (TblEmpresasResoluciones item in resoluciones_bd)
				{
					resoluciones_respuesta.Add(Ctl_EmpresaResolucion.Convertir(item));
				}
			}

			return resoluciones_respuesta;
		}

		/// <summary>
		/// Crea la resolución en versión 2 y modo habilitación
		/// </summary>
		/// <param name="resoluciones_dian">datos de la resolución</param>
		/// <param name="obligado">identificación del obligado</param>
		/// <returns>resolución creada</returns>
		public static List<Resolucion> CrearHabilitacion(Resolucion datos_resolucion, string obligado)
		{
			//obtiene los datos de la empresa
			Ctl_Empresa Peticion = new Ctl_Empresa();
			TblEmpresas facturador_electronico = Peticion.Obtener(obligado);

			List<Resolucion> resoluciones_respuesta = new List<Resolucion>();
			
			ResolucionesFacturacion resolucion_dian = new ResolucionesFacturacion();
			resolucion_dian.RangoFacturacion = new RangoFacturacion[1];
			resolucion_dian.RangoFacturacion[0].ClaveTecnica = datos_resolucion.ClaveTecnica;
			resolucion_dian.RangoFacturacion[0].FechaResolucion = datos_resolucion.FechaResolucion;
			resolucion_dian.RangoFacturacion[0].FechaVigenciaDesde = datos_resolucion.FechaVigenciaInicial;
			resolucion_dian.RangoFacturacion[0].FechaVigenciaHasta = datos_resolucion.FechaVigenciaFinal;
			resolucion_dian.RangoFacturacion[0].NumeroResolucion = Convert.ToInt64(datos_resolucion.NumeroResolucion);
			resolucion_dian.RangoFacturacion[0].Prefijo = datos_resolucion.Prefijo;
			resolucion_dian.RangoFacturacion[0].RangoFinal = datos_resolucion.RangoFinal;
			resolucion_dian.RangoFacturacion[0].RangoInicial = datos_resolucion.RangoInicial;

			if (facturador_electronico.IntVersionDian == 2 && facturador_electronico.IntHabilitacion.Value < 99)
			{
				// crea o actualiza las resoluciones obtenidas en la base de datos
				Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();
				List<TblEmpresasResoluciones> resoluciones_bd = empresa_resolucion.Crear(resolucion_dian, obligado);
				
				if (resoluciones_bd != null)
				{
					foreach (TblEmpresasResoluciones item in resoluciones_bd)
					{
						resoluciones_respuesta.Add(Ctl_EmpresaResolucion.Convertir(item));
					}
				}
			}

			return resoluciones_respuesta;
		}


		/// <summary>
		/// Actualiza las resoluciones del servicio de la DIAN en la base de datos
		/// </summary>
		/// <param name="id_peticion">id de la petición</param>
		/// <param name="identificacion_obligado">nùmero de identificaciòn del facturador electrónico</param>
		/// <param name="ruta_log">ruta física del log de respuesta</param>
		/// <returns>resoluciones de base de datos</returns>
		public static List<TblEmpresasResoluciones> Actualizar(Guid id_peticion, TblEmpresas obligado)
		{
			DateTime fecha_actual = Fecha.GetFecha();


			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			// ruta física del xml
			string carpeta = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, obligado.StrIdSeguridad.ToString());
			string archivo_log = string.Format(@"{0}\{1}\{2}.xml", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaEResoluciones, id_peticion);

			// obtiene los datos de prueba del proveedor tecnológico de la DIAN
			DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

			//Obtiene la resolucion de la DIAN para el Obligado enviado
			ResolucionesFacturacion resoluciones_dian = Ctl_Resolucion.Obtener(id_peticion, data_dian.IdSoftware, data_dian.ClaveAmbiente, obligado.StrIdentificacion, data_dian.NitProveedor, fecha_actual, archivo_log);

			if (resoluciones_dian.CodigoOperacion == CodigoType.OK)
			{
				// crea o actualiza las resoluciones obtenidas en la base de datos
				Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();
				List<TblEmpresasResoluciones> lista_resolucion = empresa_resolucion.Crear(resoluciones_dian, obligado.StrIdentificacion);

				return lista_resolucion;
			}
			else
			{

				Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();

				//obtiene las Resoluciones del facturador y las actualiza para que no permita la recepcion de documento
				List<TblEmpresasResoluciones> resoluciones_bd = empresa_resolucion.ObtenerResoluciones(obligado.StrIdentificacion, "*");

				foreach (TblEmpresasResoluciones item in resoluciones_bd)
				{
					item.DatFechaVigenciaHasta = fecha_actual.AddDays(-2);
					item.StrObservaciones = string.Format("{0} - Respuesta DIAN: {1} - {2}", fecha_actual, resoluciones_dian.CodigoOperacion, resoluciones_dian.DescripcionOperacion);

					empresa_resolucion = new Ctl_EmpresaResolucion();
					empresa_resolucion.Edit(item);
				}

				throw new ApplicationException(string.Format("Error al obtener las resoluciones del facturador electrónico {0}. Respuesta DIAN: {1} - {2}", obligado.StrIdentificacion, resoluciones_dian.CodigoOperacion, resoluciones_dian.DescripcionOperacion));

			}

		}

		#region Sonda Obtener Resoluciones

		/// <summary>
		/// Sonda para obtener resoluciones
		/// </summary>
		/// <returns></returns>
		public async Task SondaObtenerResoluciones()
		{
			try
			{
				var Tarea = TareaProcesarResoluciones();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
			}
		}

		/// <summary>
		/// Tarea para procesar resoluciones
		/// </summary>
		/// <returns></returns>
		public async Task TareaProcesarResoluciones()
		{
			await Task.Factory.StartNew(() =>
			{

				Ctl_EmpresaResolucion ctl_facturador = new Ctl_EmpresaResolucion();
				var facturador = ctl_facturador.ObtenerTodas().GroupBy(d => d.StrEmpresa).Select(g => g.First()).ToList();


				foreach (var item in facturador)
				{
					try
					{
						Ctl_Resoluciones.Obtener(item.StrEmpresa);
					}
					catch (Exception excepcion)
					{
						LogExcepcion.Guardar(excepcion);

					}
				}

			});

		}

		#endregion
	}
}
