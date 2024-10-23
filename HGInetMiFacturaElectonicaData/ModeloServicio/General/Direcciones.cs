using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Direcciones
	{
		/// <summary>
		/// Codigo Departamento necesario para V2
		/// </summary>
		public string CodigoDepartamento { get; set; }

		/// <summary>
		/// Departamento
		/// </summary>
		public string Departamento { get; set; }

		/// <summary>
		/// Codigo Ciudad necesario para V2
		/// </summary>
		public string CodigoCiudad { get; set; }

		/// <summary>
		/// Ciudad
		/// </summary>
		public string Ciudad { get; set; }

		/// <summary>
		/// Direccion
		/// </summary>
		public string Direccion { get; set; }

		/// <summary>
		/// Codigo Postal necesario para V2
		/// </summary>
		public string CodigoPostal { get; set; }

		/// <summary>
		/// Código del País según ISO 3166-1 alfa-2 (ej: CO = Colombia, etc.)
		/// </summary>
		public string CodigoPais { get; set; }

		/// <summary>
		/// País según ISO 3166-1 alfa-2 (ej: CO = Colombia, etc.) Se llena con el codigo enviado y es para el PDF
		/// </summary>
		public string Pais { get; set; }
	}
}
