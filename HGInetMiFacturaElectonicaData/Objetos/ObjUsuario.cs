using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Objetos
{
	public class ObjUsuario
	{
		public DateTime FechaActualizacion { get; set; }
		public string FechaCambioClave { get; set; }
		public DateTime FechaIngreso { get; set; }
		public int Estado { get; set; }
		public string Apellidos { get; set; }
		public string Cargo { get; set; }
		public string Celular { get; set; }
		public string Clave { get; set; }
		public string Empresa { get; set; }
		public string Extension { get; set; }
		public string IdCambioClave { get; set; }
		public Guid IdSeguridad { get; set; }
		public string Mail { get; set; }
		public string Nombres { get; set; }
		public string NombreCompleto { get; set; }
		public string Telefono { get; set; }
		public string Usuario { get; set; }
		public string RazonSocial { get; set; }
	}
}
