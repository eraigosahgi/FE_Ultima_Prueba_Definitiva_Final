using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class InformacionPos
	{
		/// <summary>
		/// Datos del software emisor de la informacion
		/// </summary>
		public FabricanteSoftwarePos DatosSoftware { get; set; }

		/// <summary>
		/// Datos del comprador cuando el Documento es Pos de caja registradora
		/// </summary>
		public InfoComprador DatosComprador { get; set; }

		/// <summary>
		/// Datos del punto de venta donde se genera la Venta
		/// </summary>
		public InfoPuntoVenta DatosPuntoVenta { get; set; }

		/// <summary>
		/// Datos del ticket del transporte de pasajero cuando es un documento POS de transporte terrestre
		/// </summary>
		public InfoTicket DatosTicketPasajero { get; set; }
	}
}
