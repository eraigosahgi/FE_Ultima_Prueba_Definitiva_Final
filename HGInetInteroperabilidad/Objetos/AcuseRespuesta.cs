﻿using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Objetos
{
	/// <summary>
	/// Objeto para el manejo de la respuesta del acuse
	/// 
	/// Codigo de respuesta de este servicio
	//  201 El documento electrónico existe en el sistema se procede a retornar el application response
	//  409 El documento electrónico asociado al UUID consultado no existe en el sistema  
	//  406 El documento electrónico asociado al UUID consultado existe en el sistema pero fue registrado por un proveedor de factura diferente  
	//  500 Error interno del receptor del documento electrónico
	/// </summary>
	public class AcuseRespuesta
	{
		public string mensajeGlobal { get; set; }
		public DateTime timeStamp { get; set; }
	}

	public class RespuestaAcuseProceso
	{
		public long Numero { get; set; }
		public string IdSeguridad { get; set; }
		public DateTime FechaProceso { get; set; }
		public short DocumentoTipo { get; set; }
		public string Facturador { get; set; }
		public Error Error { get; set; }
        public string Mensaje { get; set; }
        public string MensajeFinal { get; set; }
	}
}
