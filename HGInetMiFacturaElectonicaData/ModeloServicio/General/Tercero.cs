using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    public class Tercero
    {

        /// <summary>
        /// Identificacion
        /// Campo: StrIdTercero       
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("^\\d+$", ErrorMessage = "La {0} debe contener sólo números.")]
        public string Identificacion { get; set; }

        /// <summary>
        /// Digito de Verificación de identificacion
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(0, 9, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public int IdentificacionDv { get; set; }

        /// <summary>
        /// Tipo de Documento: 13-Cedula, 22-Cedula Extranjeria, 31-NIT
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(0, 99, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public int TipoIdentificacion { get; set; }

        /// <summary>
        /// Tipo de Persona Juridica: 1-Juridica, 2-Natural
        /// Campo: IntTipoPersona
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(1, 2, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public int TipoPersona { get; set; }

        /// <summary>
        /// Regimen Tributario: 0-Simplificado, 2-Común
        /// Campo: IntRegimen
        /// </summary>
        [Required(ErrorMessage = "{0} es un campo obligatorio")]
        [Range(0, 2, ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public int Regimen { get; set; }

        /// <summary>
        /// Nombre Comercial
        /// Campo: StrNombre
        /// </summary>
        public string NombreComercial { get; set; }

        /// <summary>
        /// Departamento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string Departamento { get; set; }

        /// <summary>
        /// Ciudad
        /// Campo: StrCiudad
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string Ciudad { get; set; }

        /// <summary>
        /// Direccion
        /// Campo: StrDireccion
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string Direccion { get; set; }

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
        public string Mail { get; set; }

        /// <summary>
        /// Pagina Web
        /// </summary>
        public string PaginaWeb { get; set; }

        /// <summary>
        /// Pais: Segun tabla DIAN 
        /// </summary>
        [MaxLength(2, ErrorMessage = "El {0} no puede superar los {1} caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
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
