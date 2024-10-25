using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Hora
	{

		/// <summary>
		/// Tipo de la Hora a reportar según Clasificación DIAN. Ocurrencia 1-1(enumerable TipoHoraNomina)
		/// </summary>
		public int TipoHora { get; set; }

		/// <summary>
		/// código Dian de la hora a Reportar. Es llenado por la plataforma para la generacion de Formatos
		/// </summary>
		public string CodigoConcepto { get; set; }

		/// <summary>
		/// En formato YYYY-MM-DDTHH:MM:SS. Ocurrencia 0-1
		/// </summary>
		public DateTime HoraInicio { get; set; }

		/// <summary>
		/// En formato YYYY-MM-DDTHH:MM:SS. Ocurrencia 0-1
		/// </summary>
		public DateTime HoraFin { get; set; }

		/// <summary>
		/// Cantidad de Horas. Ocurrencia 1-1
		/// </summary>
		public decimal Cantidad { get; set; }

		/// <summary>
		/// Se debe colocar el Porcentaje que corresponda de la tabla 5.5.1.5 y el tipo de hora que reporta. Ocurrencia 1-1
		/// </summary>
		public decimal Porcentaje { get; set; }

		/// <summary>
		/// Valor Pagado por las Horas correspondiente a: (Sueldo /240) x Porcentaje x Cantidad. Ocurrencia 1-1
		/// </summary>
		public decimal Valor { get; set; }

	}
}
