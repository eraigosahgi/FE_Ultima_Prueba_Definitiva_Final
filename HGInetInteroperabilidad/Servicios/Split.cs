using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Servicios
{
    public static class Split
    {
        /// <summary>
        /// Retorna el codigo de respuesta del proceso de recibir documentos
        /// </summary>
        /// <param name="Descripcion">Mensaje del objeto con el | para hacer el split</param>
        /// <returns></returns>
        public static int Codigo(string Descripcion)
        {

            try
            {
                string[] datos = Descripcion.Split('|');

                return Convert.ToInt32(datos[0]);
            }
            catch (Exception)
            {

                return 0;
            }

        }
        /// <summary>
        /// Retorna la descripcion del mensaje de interoperabilidad sin el codigo de respuesta y sin el (|)
        /// </summary>
        /// <param name="Descripcion"></param>
        /// <returns></returns>
        public static string Mensaje(string Descripcion)
        {
            try
            {
                string[] datos = Descripcion.Split('|');

                return datos[1];
            }
            catch (Exception)
            {

                return null;
            }
        }

    }
}
