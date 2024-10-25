using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Periodo
	{

		/// <summary>
		/// Se debe indicar la Fecha de Ingreso del trabajador a la empresa, en formato AAAA-MM-DD. si se presenta en este periodo.
		/// </summary>
		public DateTime FechaIngreso { get; set; }

		/// <summary>
		/// Se debe indicar la Fecha de Retiro del trabajador a la empresa, en formato AAAA-MM-DD. si se presenta en este periodo. Ocurrencia 0-1
		/// </summary>
		public DateTime FechaRetiro { get; set; }

		/// <summary>
		/// Se debe indicar la Fecha de Inicio del Periodo de Pago del documento, en formato AAAA-MM-DD.
		/// </summary>
		public DateTime FechaLiquidacionInicio { get; set; }

		/// <summary>
		/// Se debe indicar la Fecha de Fin del Periodo de Pago del documento, en formato AAAA-MM-DD.
		/// </summary>
		public DateTime FechaLiquidacionFin { get; set; }

		/// <summary>
		/// Cantidad de Tiempo que lleva laborando el Trabajador en la empresa. Definido en el numeral 8.4.1. (se reporta en días años de 360, mes 30 y días 8.3.1)
		/// </summary>
		public int TiempoLaborado { get; set; }

		/// <summary>
		/// Debe ir la fecha de emision del documento. Considerando zona horaria de Colombia (-5), en formato AAAA-MM-DD.
		/// </summary>
		//public DateTime FechaGen { get; set; }



	}
}
