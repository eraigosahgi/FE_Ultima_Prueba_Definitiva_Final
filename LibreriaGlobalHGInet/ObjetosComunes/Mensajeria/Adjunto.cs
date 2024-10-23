using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria
{
    public class Adjunto
    {
        /// <summary>
        /// Nombre del archivo con extensión
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Datos del archivo en base 64
        /// </summary>
        public string ContenidoB64 { get; set; }
    }
}