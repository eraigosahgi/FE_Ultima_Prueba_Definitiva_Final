using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaController.Configuracion;
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

            List<TblEmpresasResoluciones> resoluciones_bd = Actualizar(id_peticion, identificacion_obligado);

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
        /// <param name="identificacion_proveedor">nùmero de identificaciòn del proveedor</param>
        /// <returns>resoluciones de base de datos</returns>
        public static List<TblEmpresasResoluciones> Actualizar(Guid id_peticion, string identificacion_obligado)
        {
            DateTime fecha_actual = Fecha.GetFecha();

                // obtiene los datos de prueba del proveedor tecnológico de la DIAN
                DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

                //Obtiene la resolucion de la DIAN para el Obligado enviado
                ResolucionesFacturacion resoluciones_dian = Ctl_Resolucion.Obtener(id_peticion, data_dian.IdSoftware, data_dian.ClaveAmbiente, identificacion_obligado, data_dian.NitProveedor, fecha_actual);

                if (!resoluciones_dian.CodigoOperacion.Equals("OK"))
                {
                    // crea o actualiza las resoluciones obtenidas en la base de datos
                    Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();
                    List<TblEmpresasResoluciones> lista_resolucion = empresa_resolucion.Crear(resoluciones_dian, identificacion_obligado);

                    return lista_resolucion;
                }
                else
                {
                    throw new ApplicationException(string.Format("Error al obtener las resoluciones del facturador electrónico {0}. Respuesta DIAN: {1} - {2}", identificacion_obligado, resoluciones_dian.CodigoOperacion, resoluciones_dian.DescripcionOperacion));

                }
      
        }

    }
}
