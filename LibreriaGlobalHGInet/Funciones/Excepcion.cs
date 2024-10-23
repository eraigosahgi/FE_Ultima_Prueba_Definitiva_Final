using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Funciones
{
    public class Excepcion
    {

        public static string Mensaje(Exception exc)
        {
            string mensaje = "";

            if (exc != null)
            {
                mensaje += exc.Message;

                if (exc.InnerException != null)
                    mensaje += Mensaje(exc.InnerException);
            }


            return mensaje;

        }

    }
}
