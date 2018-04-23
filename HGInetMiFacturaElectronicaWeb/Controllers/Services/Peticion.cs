using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class Peticion
    {
        /// <summary>
        /// Valida la DataKey.
        /// </summary>
        /// <param name="DataKey"></param>
        /// <param name="identificacion_obligado"></param>
        public static void Validar(string DataKey, string identificacion_obligado)
        {
            Ctl_Empresa ctl_empresa = new Ctl_Empresa();

            //Válida que la key sea correcta.
            ctl_empresa.Validar(DataKey, identificacion_obligado);

        }



        /// <summary>
        /// Almacena la solicitud (petición) wcf en un XML
        /// </summary>	
        /// <param name="contexto">solicitud</param>
        /// <param name="nombre_servicio_web">nombre de la solicitud</param>
        /// <param name="carpeta_nit">identificación para la carpeta</param>
        /// <param name="registro">texto para identificar el registro en el nombre del archivo</param>
        public static void ToXml(RequestContext contexto, string nombre_servicio_web, string carpeta_nit, string registro)
        {
            try
            {
                // Initialize soap request XML
                System.Xml.XmlDocument xmlSoapRequest = new System.Xml.XmlDocument();

                string mensaje = contexto.RequestMessage.ToString();

                xmlSoapRequest.LoadXml(mensaje);

                // carpeta del xml
                string ruta_carpeta = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), carpeta_nit);
                ruta_carpeta = string.Format(@"{0}{1}\\{2}\\", ruta_carpeta, "ws_logs", nombre_servicio_web);

                // valida la existencia de la carpeta
                ruta_carpeta = Directorio.CrearDirectorio(ruta_carpeta);

                string nombre_archivo = string.Format("{0}-{1}.xml", Fecha.GetFecha().ToString(Fecha.formato_fecha_hora_archivo), registro);

                Xml.GuardarXml(xmlSoapRequest, ruta_carpeta, nombre_archivo);
            }
            catch (Exception excepcion)
            {

            }
        }

    }
}