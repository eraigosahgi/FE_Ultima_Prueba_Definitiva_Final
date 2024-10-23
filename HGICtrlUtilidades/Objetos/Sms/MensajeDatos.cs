using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	/// <summary>
	/// Información resultante al enviar el SMS
	/// </summary>
	public class MensajeDatos
	{
		/// <summary>
		/// ID único e irrepetible del SMS
		/// </summary>
		public string id { get; set; }

		/// <summary>
		/// Número del SMS
		/// </summary>
		public string numero { get; set; }

		/// <summary>
		/// Texto del SMS
		/// </summary>
		public string sms { get; set; }

		/// <summary>
		/// Fecha de envío del SMS
		/// </summary>
		public string fecha_envio { get; set; }

		/// <summary>
		/// Nombre del área de envío
		/// </summary>
		public string ind_area_nom { get; set; }

		/// <summary>
		/// costo del SMS
		/// se obtiene -1 cuando no se genera cobro
		/// </summary>
		public string precio_sms { get; set; }

		/// <summary>
		/// texto resultado del SMS
		/// </summary>
		public string resultado_t { get; set; }

		/// <summary>
		/// Resultado del SMS
		/// 0 => SMS exitoso
		/// 1 => SMS fallido
		/// </summary>
		public string resultado { get; set; }

	}
}
