using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Tercero
	{

		/// <summary>
		/// Id único de Registro
		/// </summary>
		public int CodigoRegistro { get; set; }

		/// <summary>
		/// Identificacion
		/// Campo: StrIdTercero       
		/// </summary>
		public string Identificacion { get; set; }

		/// <summary>
		/// Digito de Verificación de identificacion
		/// </summary>
		public int IdentificacionDv { get; set; }

		/// <summary>
		/// Tipo de Documento: 13-Cedula, 22-Cedula Extranjeria, 31-NIT
		/// Campo: StrTipoId
		/// </summary>
		public int TipoIdentificacion { get; set; }

		/// <summary>
		/// Tipo de Persona Juridica: 1-Juridica, 2-Natural
		/// Campo: IntTipoPersona
		/// </summary>
		public int TipoPersona { get; set; }

		/// <summary>
		/// Regimen Tributario: 0-Simplificado, 2-Común
		/// Campo: IntRegimen
		/// </summary>
		public int Regimen { get; set; }

		/// <summary>
		/// Nombre Comercial
		/// Campo: StrNombre
		/// </summary>
		public string NombreComercial { get; set; }

		/// <summary>
		/// Departamento
		/// </summary>
		public string Departamento { get; set; }

		/// <summary>
		/// Ciudad
		/// Campo: StrCiudad
		/// </summary>
		public string Ciudad { get; set; }

		/// <summary>
		/// Direccion
		/// Campo: StrDireccion
		/// </summary>
		public string Direccion { get; set; }

		/// <summary>
		/// Telefono
		/// </summary>
		public string Telefono { get; set; }

		/// <summary>
		/// Correo 
		/// </summary>
		public string Mail { get; set; }

		/// <summary>
		/// Pagina Web
		/// </summary>
		public string PaginaWeb { get; set; }

		/// <summary>
		/// Pais: Segun tabla DIAN 
		/// </summary>
		public string CodigoPais { get; set; }

		/// <summary>
		/// Razon Social
		/// Campo: StrApellido1
		/// </summary>
		public string RazonSocial { get; set; }

		/// <summary>
		/// Primer Apellido
		/// Campo: StrApellido1-StrApellido2
		/// </summary>
		public string PrimerApellido { get; set; }

		/// <summary>
		/// Segundo Apellido
		/// </summary>
		public string SegundoApellido { get; set; }

		/// <summary>
		/// Primer Nombre
		/// Campo: StrNombre1
		/// </summary>
		public string PrimerNombre { get; set; }

		/// <summary>
		/// Segundo nombre
		/// Campo: StrNombre2
		/// </summary>
		public string SegundoNombre { get; set; }


	}
}
