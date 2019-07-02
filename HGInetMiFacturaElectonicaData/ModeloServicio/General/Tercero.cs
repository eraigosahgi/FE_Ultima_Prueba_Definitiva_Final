using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Representación del Facturador Electrónico y del Adquiriente
	/// </summary>
	public class Tercero
	{

		/// <summary>
		/// Número de identificación
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		[RegularExpression("^\\d+$", ErrorMessage = "La {0} debe contener sólo números.")]
		public string Identificacion { get; set; }

		/// <summary>
		/// Digito de verificación del número de identificación
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		[Range(0, 9, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public int IdentificacionDv { get; set; }

		/// <summary>
		/// Tipo de identificación: 13-Cedula, 22-Cedula Extranjeria, 31-NIT
		/// Anexo Técnico 001 Formatos de los Documentos XML de Facturación Electrónica.pdf
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		[Range(0, 99, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public int TipoIdentificacion { get; set; }

		/// <summary>
		/// Tipo de Persona: 1-Juridica, 2-Natural
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		[Range(1, 2, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public int TipoPersona { get; set; }

		/// <summary>
		/// Regimen Tributario: 0-Simplificado, 2-Común
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		[Range(0, 2, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public int Regimen { get; set; }

		/// <summary>
		/// Nombre Comercial
		/// </summary>
		public string NombreComercial { get; set; }

		/// <summary>
		/// Codigo Departamento necesario para V2
		/// </summary>
		public string CodigoDepartamento { get; set; }


		/// <summary>
		/// Departamento
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string Departamento { get; set; }

		/// <summary>
		/// Codigo Ciudad necesario para V2
		/// </summary>
		public string CodigoCiudad { get; set; }

		/// <summary>
		/// Ciudad
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string Ciudad { get; set; }

		/// <summary>
		/// Direccion
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string Direccion { get; set; }

		/// <summary>
		/// Codigo Postal necesario para V2
		/// </summary>
		public string CodigoPostal { get; set; }

		/// <summary>
		/// Telefono
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		[StringLength(10, MinimumLength = 7,
		ErrorMessage = "La propiedad {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
		[RegularExpression("^\\d+$", ErrorMessage = "El Teléfono debe contener sólo números.")]
		public string Telefono { get; set; }

		/// <summary>
		/// Correo 
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		[RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Mail incorrecto")]
		public string Email { get; set; }

		/// <summary>
		/// Pagina Web
		/// </summary>
		public string PaginaWeb { get; set; }

		/// <summary>
		/// Código del País según ISO 3166-1 alfa-2 (ej: CO = Colombia, etc.)
		/// </summary>
		[MaxLength(2, ErrorMessage = "El {0} no puede superar los {1} caracteres")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string CodigoPais { get; set; }

		/// <summary>
		/// Razón Social
		/// </summary>
		public string RazonSocial { get; set; }

		/// <summary>
		/// Primer Apellido
		/// </summary>
		public string PrimerApellido { get; set; }

		/// <summary>
		/// Segundo Apellido
		/// </summary>
		public string SegundoApellido { get; set; }

		/// <summary>
		/// Primer Nombre
		/// </summary>
		public string PrimerNombre { get; set; }

		/// <summary>
		/// Segundo nombre
		/// </summary>
		public string SegundoNombre { get; set; }

		/// <summary>
		/// Regimen Fiscal: 04-Simple, 05-Ordinario
		/// </summary>
		public string RegimenFiscal { get; set; }

		/// <summary>
		/// Responsabilidades fiscales lista 6.2.7
		/// </summary>
		public List<string> Responsabilidades { get; set; }

		/// <summary>
		///Código del tributo del que es responsable ejemplo:(01-IVA-Impuesto de Valor Agregado )(04-INC-Impuesto Nacional al Consumo)(05-ReteIVA-Retención sobre el IVA) 
		/// </summary>
		public string CodigoTributo { get; set; }

	}
}
