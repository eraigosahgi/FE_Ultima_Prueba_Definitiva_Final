using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Peticiones
{   
    /// <summary>
    /// Tipo de peticion
    /// </summary>
    public enum Peticion
    {
        [Description("GET")]
        GET = 1,

        [Description("POST")]
        POST = 2,

        [Description("PUT")]
        PUT = 3,

        [Description("DELETE")]
        DELETE = 4,
    }

    /// <summary>
    /// Tipo de peticion
    /// </summary>
    public enum TipoContenido
    {
        [Description("application/json")]
        Applicationjson = 1,

        [Description("text/plain")]
        Textplain = 2,

        [Description("application/xml")]
        Applicationxml = 3,

        [Description("text/html")]
        Texthtml = 4,
    }
}
