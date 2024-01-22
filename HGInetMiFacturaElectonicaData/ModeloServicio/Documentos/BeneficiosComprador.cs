using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class InfoComprador
	{
		/// <summary>
		/// Corresponse a los Nombres y apellidos del comprador
		/// </summary>
		public string NombreApellido { get; set; }

		/// <summary>
		/// Corresponde al Codigo del comprador Documento identidad
		/// </summary>
		public string Codigo { get; set; }

		/// <summary>
		/// Corresponde a un valor donde se informe la Cantidad de Puntos acumulados por el comprador
		/// </summary>
		public string Puntos { get; set; }
	}
}
