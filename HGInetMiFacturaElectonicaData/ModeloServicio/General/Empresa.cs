using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Empresa
	{
		public string Identificacion { get; set; }
		public int IdentificacionDv { get; set; }
		public string RazonSocial { get; set; }
		public string EmailAdmin { get; set; }
		public string EmailEnvio { get; set; }
		public string EmailRecepcion { get; set; }
		public string EmailAcuse { get; set; }
		public string EmailPagos { get; set; }
		public string EmailRecepcionDian { get; set; }
		public string Telefono { get; set; }
		public short HorasAcuseTacito { get; set; }
		public bool ManejaAnexo { get; set; }
		public int VersionDian { get; set; }
		public string PinSoftware { get; set; }
	}
}
