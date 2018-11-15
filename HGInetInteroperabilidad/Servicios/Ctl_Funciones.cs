using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Servicios
{
    public class Ctl_Funciones
    {
        /// <summary>
        /// Valida contrañesa valida para el proceso
        ///  La contraseña deberá tener un mínimo de 8 caracteres 
        ///  ● La contraseña deberá tener mayúsculas, minúsculas, números y caracteres especiales 
        ///  ● Al momento de llevar a cabo un cambio de contraseña, la nueva contraseña no podrá ser igual a alguna                   
        ///    de las 10 contraseñas anteriormente utilizadas.  
        /// </summary>
        /// <param name="ContrasenaActual">Contraseña actual</param>
        /// <param name="ContrasenaNueva">Contraseña Nueva</param>
        /// <returns></returns>
        public static bool ContrasenaValida(string ContrasenaActual, string ContrasenaNueva)
        {
            //Longitud minima 8 caracteres
            if (ContrasenaNueva.Length < 8)
            {
                return false;
            }
            //Valida que las contraseñas no sean igual
            if (ContrasenaActual == ContrasenaNueva)
            {
                return false;
            }
            //Valida que contenga por lo menos una letra en minuscula
            if (!ContrasenaNueva.Any(d => char.IsLower(d)))
            {
                return false;
            }
            //Valida que contenga por lo menos una letra en Mayuscula
            if (!ContrasenaNueva.Any(d => char.IsUpper(d)))
            {
                return false;
            }
            //Valida que contenga por lo menos un Numero
            if (!ContrasenaNueva.Any(d => char.IsNumber(d)))
            {
                return false;
            }
            

            return true;
        }
    }
}
