using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Empleador
	{

		/// <summary>
		/// Debe ir el Nombre o Razón Social del Empleador
		/// </summary>
		public string RazonSocial { get; set; }

		/// <summary>
		/// Debe ir el Primer Apellido del Empleador. Ocurrencia 0-1
		/// </summary>
		public string PrimerApellido { get; set; }

		/// <summary>
		/// Debe ir el Segundo Apellido del Empleador. Ocurrencia 0-1
		/// </summary>
		public string SegundoApellido { get; set; }

		/// <summary>
		/// Debe ir el Primer Nombre del Empleador. Ocurrencia 0-1
		/// </summary>
		public string PrimerNombre { get; set; }

		/// <summary>
		/// Deben ir los Otros Nombres del Empleador. Ocurrencia 0-1
		/// </summary>
		public string OtrosNombres { get; set; }

		/// <summary>
		/// Tipo de identificación: 13-Cedula, 22-Cedula Extranjeria, 31-NIT
		/// Tipo de documento de identificación que actualmente tiene el empleador. Código de la tabla 5.2.1.
		/// </summary>
		public int TipoDocumento { get; set; }

		/// <summary>
		/// Debe ir el NIT del Empleador sin guiones ni DV.
		/// </summary>
		public string Identificacion { get; set; }

		/// <summary>
		/// Debe ir el DV del Empleador.
		/// </summary>
		public int DV { get; set; }

		/// <summary>
		/// Código del país donde se encuentra ubicada la empresa del empleador en el mes que se está reportando. Código alfa-2 de la tabla 5.4.1. (ej: CO = Colombia, etc.)
		/// </summary>
		public string Pais { get; set; }

		/// <summary>
		/// Nombre del país, lo llena la plataforma y para uso de formatos (ej: CO = Colombia, etc.)
		/// </summary>
		public string PaisNombre { get; set; }

		/// <summary>
		/// Código del departamento donde se encuentra ubicada la empresa del empleador en el mes que se está reportando. El Código de la tabla 5.4.2.
		/// </summary>
		public string DepartamentoEstado { get; set; }

		/// <summary>
		/// Nombre del departamento,  lo llena la plataforma y para uso de formatos. El Código de la tabla 5.4.2.
		/// </summary>
		public string DepartamentoNombre { get; set; }

		/// <summary>
		/// Código del municipio o ciudad donde se encuentra ubicada la empresa del empleador en el mes que se está reportando de la tabla 5.4.3.
		/// </summary>
		public string MunicipioCiudad { get; set; }

		/// <summary>
		/// Nombre del municipio o ciudad,  lo llena la plataforma y para uso de formatos, referencia tabla 5.4.3.
		/// </summary>
		public string CiudadNombre { get; set; }

		/// <summary>
		/// Debe corresponder a la dirección del lugar físico de expedición del documento.
		/// </summary>
		public string Direccion { get; set; }

		/// <summary>
		/// Telefono
		/// </summary>
		public string Telefono { get; set; }

		/// <summary>
		/// Correo 
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Pagina Web
		/// </summary>
		public string PaginaWeb { get; set; }


	}
}
