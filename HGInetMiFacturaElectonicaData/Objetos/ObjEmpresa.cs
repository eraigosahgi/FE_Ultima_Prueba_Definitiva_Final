using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Objetos
{
	public class ObjEmpresa
	{		
		public string Identificacion { get; set; }		
		public string RazonSocial { get; set; }
		public string Email { get; set; }
		public string Serial { get; set; }
		public string Perfil { get; set; }
		public string Habilitacion { get; set; }
		public Guid IdSeguridad { get; set; }
		public string Asociado { get; set; }		
		public string EmpresaDescuento { get; set; }
		public int Estado { get; set; }
		public string Postpago { get; set; }
		public int Nusuaurios { get; set; }
		public string HorasAcuse { get; set; }
		public string NotificacionMail { get; set; }
		public string StrMailEnvio { get; set; }
		public string StrMailPagos { get; set; }
		public string StrMailRecepcion { get; set; }
		public string StrMailAcuse { get; set; }
	}
}
