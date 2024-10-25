using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class InfoTicket
	{
		/// <summary>
		/// Corresponde al modo de transporte o tipo de operación
		/// </summary>
		public string ModoTransporte { get; set; }

		/// <summary>
		/// Corresponde a la Placa del Vehiculo
		/// </summary>
		public string IDMediodeTransporte { get; set; }

		/// <summary>
		/// Corresponde al Medio en el que se presta el servicio
		/// </summary>
		public string Mediodetransporte { get; set; }

		/// <summary>
		/// Corresponse al Origen de la operación 
		/// </summary>
		public string LugardeOrigen { get; set; }

		/// <summary>
		/// Corresponde al Destino de la operación 
		/// </summary>
		public string LugardeDestino { get; set; }
	}
}
