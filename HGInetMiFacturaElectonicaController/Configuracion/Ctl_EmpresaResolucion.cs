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
		public TblEmpresasResoluciones Obtener(string documento_empresa, string numero_resolucion, string prefijo)
        {

            var datos = (from item in context.TblEmpresasResoluciones
                         where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
                         && item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
                         && (item.StrPrefijo.Equals(prefijo) || prefijo.Equals("*"))
                         select item).FirstOrDefault();

            return datos;

        }

		/// <summary>
		/// Obtiene todas las resoluciones
		/// </summary>
		/// <returns></returns>
		public List<TblEmpresasResoluciones> ObtenerTodas()
		{
			List<TblEmpresasResoluciones> datos = (from item in context.TblEmpresasResoluciones
												   select item).ToList();

			return datos;
		}


		/// <summary>
		/// Obtiene todas las resoluciónes de una empresa
		/// </summary>
		/// <param name="documento_empresa">documento de la empresa</param>
		/// <param name="numero_resolucion">número de resolución</param>
		/// <returns>lista de resoluciones</returns>
		public List<TblEmpresasResoluciones> ObtenerResoluciones(string documento_empresa, string numero_resolucion)
        {

            List<TblEmpresasResoluciones> datos = (from item in context.TblEmpresasResoluciones
                                                   where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
                                                   && item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
                                                   orderby item.IntTipoDoc, item.StrNumResolucion, item.StrPrefijo
                                                   select item).ToList();

            return datos;

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
        /// Valida y crea resoluciones
        /// </summary>
        /// <param name="resoluciones">Objeto de resoluciones obtenida de la DIAN</param>
        /// <param name="obligado">Identificacion del facturdor</param>
        /// <returns>Lista de resoluciones</returns>
        public List<TblEmpresasResoluciones> Crear(ResolucionesFacturacion resoluciones, string obligado, string setidpruebas = "")
        {

            Ctl_Empresa empresa = new Ctl_Empresa();
            TblEmpresas empresaBd = empresa.Obtener(obligado);

            List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

            foreach (RangoFacturacion item in resoluciones.RangoFacturacion)
            {
                if (string.IsNullOrEmpty(item.Prefijo))
                    item.Prefijo = string.Empty;

                //Valida si la Resolucion ya existe en BD
                TblEmpresasResoluciones tbl_resolucion_actual = Obtener(empresaBd.StrIdentificacion, item.NumeroResolucion.ToString(), item.Prefijo);

                // convierte el objeto del servicio a base de datos
                TblEmpresasResoluciones tbl_resolucion = Convertir(item, empresaBd, setidpruebas);

                if (tbl_resolucion_actual == null)
                {
                    // crea el registro en base de datos
                    tbl_resolucion = Crear(tbl_resolucion);

                    lista_resolucion.Add(tbl_resolucion);
                }
                else
                {
                    tbl_resolucion_actual.DatFechaVigenciaDesde = tbl_resolucion.DatFechaVigenciaDesde;
                    tbl_resolucion_actual.DatFechaVigenciaHasta = tbl_resolucion.DatFechaVigenciaHasta;
                    tbl_resolucion_actual.IntRangoFinal = tbl_resolucion.IntRangoFinal;
                    tbl_resolucion_actual.IntRangoInicial = tbl_resolucion.IntRangoInicial;
                    tbl_resolucion_actual.StrClaveTecnica = tbl_resolucion.StrClaveTecnica;
                    tbl_resolucion_actual.StrPrefijo = tbl_resolucion.StrPrefijo;
                    tbl_resolucion_actual.DatFechaActualizacion = Fecha.GetFecha();
                    tbl_resolucion_actual.StrIdSetDian = tbl_resolucion.StrIdSetDian;
                    tbl_resolucion_actual.IntVersionDian = tbl_resolucion.IntVersionDian;

                    this.Edit(tbl_resolucion_actual);
                    lista_resolucion.Add(tbl_resolucion_actual);
                }
            }

            return lista_resolucion;

        }

        /// <summary>
        /// Convierte un Objeto de servicio en un Objeto de Base de Datos
        /// </summary>
        /// <param name="item">Resolucion obtenida de la DIAN</param>
        /// <param name="empresa">Identificacion del facturador electrónico</param>
        /// <returns>Tbl de resolucion</returns>
        public static TblEmpresasResoluciones Convertir(RangoFacturacion item, TblEmpresas empresa, string setidpruebas)
        {
            TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones()
            {
                StrEmpresa = empresa.StrIdentificacion,
                StrNumResolucion = item.NumeroResolucion.ToString(),
                StrPrefijo = (!string.IsNullOrEmpty(item.Prefijo)) ? item.Prefijo : "",
                IntRangoInicial = Convert.ToInt32(item.RangoInicial),
                IntRangoFinal = Convert.ToInt32(item.RangoFinal),
                DatFechaVigenciaDesde = item.FechaVigenciaDesde,
                DatFechaVigenciaHasta = item.FechaVigenciaHasta,
                StrClaveTecnica = item.ClaveTecnica,
                StrIdSeguridad = Guid.NewGuid(),
                DatFechaIngreso = Fecha.GetFecha(),
                DatFechaActualizacion = Fecha.GetFecha(),
				StrIdSetDian = setidpruebas,
				IntVersionDian = empresa.IntVersionDian,
				IntTipoDoc = 1


            };

            return tbl_resolucion;
        }

        /// <summary>
        /// Convierte una resolucion de un Objeto tipo Nota Credito, Nota Debito o de Interopeabilidad a Objeto de BD
        /// </summary>
        /// <param name="facturador">Identificacion del Obligado a Factuar</param>
        /// <param name="prefijo">Prefijo del documento</param>
        /// <param name="tipo_doc">Tipo de documento</param>
        /// <param name="resolucion">Resolucion del Facturador Emisor registrada en el Documento</param>
        /// <returns>Tbl de resolucion</returns>
        public static TblEmpresasResoluciones Convertir(string facturador, string prefijo, int tipo_doc, int version_dian , string resolucion = "")
        {

            DateTime fecha_actual = Fecha.GetFecha();

            Guid resolucion_info = Guid.NewGuid();

            TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones()
            {
                StrEmpresa = facturador,
                StrNumResolucion = (!string.IsNullOrEmpty(resolucion)) ? resolucion: resolucion_info.ToString(),
                StrPrefijo = (!string.IsNullOrEmpty(prefijo)) ? prefijo : "",
                IntRangoInicial = 0,
                IntRangoFinal = 0,
                DatFechaVigenciaDesde = fecha_actual,
                DatFechaVigenciaHasta = fecha_actual,
                StrClaveTecnica = resolucion_info.ToString(),
                StrIdSeguridad = resolucion_info,
                DatFechaIngreso = fecha_actual,
                DatFechaActualizacion = fecha_actual,
				IntVersionDian = (short)version_dian,
                IntTipoDoc = tipo_doc
				
            };
            return tbl_resolucion;
        }

        /// <summary>
        /// Convierte una Objeto de Base de Datos a un Objeto de respuesta de Resolución
        /// </summary>
        /// <param name="resolucion_bd">Resolución obtenida de BD</param>
        /// <returns>Objeto de respuesta de Resolución</returns>
        public static Resolucion Convertir(TblEmpresasResoluciones resolucion_bd)
        {
            Resolucion resolucion_respuesta = new Resolucion()
            {
                ClaveTecnica = resolucion_bd.StrClaveTecnica,
                FechaResolucion = resolucion_bd.DatFechaVigenciaDesde,
                FechaVigenciaInicial = resolucion_bd.DatFechaVigenciaDesde,
                FechaVigenciaFinal = resolucion_bd.DatFechaVigenciaHasta,
                NumeroResolucion = resolucion_bd.StrNumResolucion,
                Prefijo = resolucion_bd.StrPrefijo,
                RangoInicial = resolucion_bd.IntRangoInicial,
                RangoFinal = resolucion_bd.IntRangoFinal,
				VersionDian = resolucion_bd.IntVersionDian,
				SetIdDian = resolucion_bd.StrIdSetDian
            };

            return resolucion_respuesta;

        }

    }
}
