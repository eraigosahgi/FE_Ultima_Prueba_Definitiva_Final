using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class InformacionPos
	{
		public FabricanteSoftwarePos DatosSoftware { get; set; }

		public InfoComprador DatosComprador { get; set; }

		public InfoPuntoVenta DatosPuntoVenta { get; set; }

		public InfoTicket DatosTicketPasajero { get; set; }
	}
}
