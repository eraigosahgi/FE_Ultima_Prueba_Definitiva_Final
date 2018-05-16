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

            var datos = (from item in context.TblEmpresasResoluciones
                         where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
                         && item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
                         select item).FirstOrDefault();

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
        public List<TblEmpresasResoluciones> Crear(ResolucionesFacturacion resoluciones, string obligado)
        {

            Ctl_Empresa empresa = new Ctl_Empresa();
            TblEmpresas empresaBd = empresa.Obtener(obligado);

            List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

            foreach (RangoFacturacion item in resoluciones.RangoFacturacion)
            {
                //Valida si la Resolucion ya existe en BD
                TblEmpresasResoluciones tbl_resolucion_actual = Obtener(empresaBd.StrIdentificacion, item.NumeroResolucion.ToString());

                // convierte el objeto del servicio a base de datos
                TblEmpresasResoluciones tbl_resolucion = Convertir(item, empresaBd);

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
        public static TblEmpresasResoluciones Convertir(RangoFacturacion item, TblEmpresas empresa)
        {
            TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones()
            {
                StrEmpresa = empresa.StrIdentificacion,
                StrNumResolucion = item.NumeroResolucion.ToString(),
                StrPrefijo = (!string.IsNullOrEmpty(item.Prefijo)) ? item.Prefijo : "",
                IntRangoInicial = Convert.ToInt16(item.RangoInicial),
                IntRangoFinal = Convert.ToInt16(item.RangoFinal),
                DatFechaVigenciaDesde = item.FechaVigenciaDesde,
                DatFechaVigenciaHasta = item.FechaVigenciaHasta,
                StrClaveTecnica = item.ClaveTecnica,
                IntIdPlantillaPdf = 0,
                IntIdPlantillaMail = 0,
                StrIdSeguridad = Guid.NewGuid(),
                DatFechaIngreso = Fecha.GetFecha(),
                DatFechaActualizacion = Fecha.GetFecha()
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
                RangoFinal = resolucion_bd.IntRangoFinal
            };

            return resolucion_respuesta;

        }

    }
}
