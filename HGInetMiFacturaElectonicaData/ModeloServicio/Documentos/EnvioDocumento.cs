using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class EnvioDocumento
	{

		/// <summary>
		/// Código de seguridad (autenticación)
		/// </summary>
		public string DataKey { get; set; }

		/// <summary>
		/// Id del radicado del documento dado por la Plataforma
		/// </summary>
		public string RadicadoDocumento { get; set; }

		/// <summary>
		/// Identificación facturador electrónico
		/// </summary>
		public string IdentificacionFacturador { get; set; }

		/// <summary>
		/// Correo donde se va enviar el documento
		/// </summary>
		public string Email { get; set; }
	}
}
