using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
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
		public static List<Resolucion> Obtener(string identificacion_obligado)
		{
			Guid id_peticion = Guid.NewGuid();

			Ctl_Empresa Peticion = new Ctl_Empresa();

			//obtiene los datos de la empresa
			TblEmpresas facturador_electronico = Peticion.Obtener(identificacion_obligado);

			List<TblEmpresasResoluciones> resoluciones_bd = Actualizar(id_peticion, facturador_electronico);

			List<Resolucion> resoluciones_respuesta = new List<Resolucion>();

			foreach (TblEmpresasResoluciones item in resoluciones_bd)
			{
				resoluciones_respuesta.Add(Ctl_EmpresaResolucion.Convertir(item));
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
			string carpeta = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, obligado.StrIdSeguridad.ToString());
			string archivo_log = string.Format(@"{0}/{1}/{2}.xml", carpeta, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaEResoluciones, id_peticion);
			
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
				throw new ApplicationException(string.Format("Error al obtener las resoluciones del facturador electrónico {0}. Respuesta DIAN: {1} - {2}", obligado.StrIdentificacion, resoluciones_dian.CodigoOperacion, resoluciones_dian.DescripcionOperacion));

			}

		}

	}
}
