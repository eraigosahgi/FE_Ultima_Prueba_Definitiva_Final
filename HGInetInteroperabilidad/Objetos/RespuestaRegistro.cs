using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
    /// <summary>
    /// 
    /// </summary>
    public class RespuestaRegistro
    {
        public TblDocumentos Documento { get; set; }

        public RegistroListaDetalleDocRespuesta Respuesta { get; set; }

    }
}
