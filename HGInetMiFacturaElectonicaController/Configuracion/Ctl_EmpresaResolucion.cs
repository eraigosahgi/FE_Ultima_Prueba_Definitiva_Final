using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_EmpresaResolucion : BaseObject<TblEmpresasResoluciones>
    {
        #region Constructores 

        public Ctl_EmpresaResolucion() : base(new ModeloAutenticacion()) { }
        public Ctl_EmpresaResolucion(ModeloAutenticacion autenticacion) : base(autenticacion) { }

        public Ctl_EmpresaResolucion(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
        #endregion

        /// <summary>
        /// Obtiene la resolución de una empresa
        /// </summary>
        /// <param name="documento_empresa">documento de la empresa</param>
        /// <param name="numero_resolucion">número de resolución</param>
        /// <returns>datos de la resolución</returns>
       public TblEmpresasResoluciones Obtener(string documento_empresa, string numero_resolucion)
		{
			try
			{
				var datos = (from item in context.TblEmpresasResoluciones
							where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
							&& item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
							select item).FirstOrDefault();

				if (datos == null)
					throw new ApplicationException(string.Format("No se encontró el número de resolución {0} para el documento {1}.", numero_resolucion, documento_empresa));
								
				return datos;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}
		}

        /// <summary>
        /// Crea la Resolucion por Empresa en la BD
        /// </summary>
        /// <param name="resoluciones">Resoluciones de la empresa</param>
        /// <returns>Resoluciones guardadas</returns>
        public TblEmpresasResoluciones Crear(TblEmpresasResoluciones resoluciones)
        {
            resoluciones = this.Add(resoluciones);

            return resoluciones;
        }

        /// <summary>
        /// Convierte el Objeto de Servicio en Objeto de Base de Datos
        /// </summary>
        /// <param name="resoluciones">Resoluciones enviadas</param>
        /// <param name="obligado">Identificacion del Obligado</param>
        /// <returns>una Tbl de resoluciones</returns>
        public TblEmpresasResoluciones Crear(ResolucionesFacturacion resoluciones, string obligado)
        {

            Ctl_Empresa empresa = new Ctl_Empresa();
            TblEmpresas empresaBd = empresa.Obtener(obligado);

            List<ResolucionesFacturacion> resolucion = new List<ResolucionesFacturacion>();

            TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones();

            foreach (var item in resoluciones.RangoFacturacion)
            {
                tbl_resolucion.IntIdEmpresa = empresaBd.IntId;
                tbl_resolucion.StrNumResolucion = item.NumeroResolucion.ToString();
                tbl_resolucion.StrPrefijo = (!string.IsNullOrEmpty(item.Prefijo)) ? item.Prefijo : "";
                tbl_resolucion.IntRangoInicial = Convert.ToInt16(item.RangoInicial);
                tbl_resolucion.IntRangoFinal = Convert.ToInt16(item.RangoFinal);
                tbl_resolucion.DatFechaVigenciaDesde = item.FechaVigenciaDesde;
                tbl_resolucion.DatFechaVigenciaHasta = item.FechaVigenciaHasta;
                tbl_resolucion.StrClaveTecnica = item.ClaveTecnica;
                tbl_resolucion.IntIdPlantillaPdf = 0;
                tbl_resolucion.IntIdPlantillaMail = 0;
                tbl_resolucion.StrIdSeguridad = Guid.NewGuid();
                tbl_resolucion.DatFechaIngreso = Fecha.GetFecha();
                tbl_resolucion.DatFechaActualizacion = Fecha.GetFecha();

                tbl_resolucion = Crear(tbl_resolucion);

            }

            return tbl_resolucion;

        }


    }
}
