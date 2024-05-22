using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Objetos
{
	public class ObjDocumentos
	{
		public string IdFacturador { get; set; }
		public string Facturador { get; set; }
		public string Prefijo { get; set; }
		public string NumeroDocumento { get; set; }
		public DateTime DatFechaIngreso { get; set; }
		public DateTime DatFechaDocumento { get; set; }
		public DateTime DatFechaVencDocumento { get; set; }
		public decimal IntVlrTotal { get; set; }
		public decimal IntValorPagar { get; set; }
		public decimal IntSubTotal { get; set; }
		public decimal IntNeto { get; set; }
		public short EstadoFactura { get; set; }
		public short EstadoCategoria { get; set; }
		public short EstadoAcuse { get; set; }
		public string MotivoRechazo { get; set; }
		public string StrAdquirienteMvoRechazo { get; set; }
		public string IdentificacionAdquiriente { get; set; }
		public string NombreAdquiriente { get; set; }
		public string MailAdquiriente { get; set; }
		public string Xml { get; set; }
		public string Pdf { get; set; }
		public Guid StrIdSeguridad { get; set; }
		public string RutaAcuse { get; set; }
		public int tipodoc { get; set; }
		public string zip { get; set; }
		public string RutaServDian { get; set; }
		public string XmlAcuse { get; set; }
		public short permiteenvio { get; set; }
		public short IntAdquirienteRecibo { get; set; }
		public int Estado { get; set; }
		public string EstadoEnvioMail { get; set; }
		public string MensajeEnvio { get; set; }
		public short EnvioMail { get; set; }
		public int poseeIdComercio { get; set; }
		public short FacturaCancelada { get; set; }
		public int PagosParciales { get; set; }
		public bool poseeIdComercioPSE { get; set; }
		public bool poseeIdComercioTC { get; set; }
		public string IdComercio { get; set; }
		public string DescripComercio { get; set; }
		public string NumResolucion { get; set; }
		public bool Radian { get; set; }
		public int FormaPago { get; set; }
		public int tipoOperacion { get; set; }
		public string MensajeError { get; set; }
	}
}
