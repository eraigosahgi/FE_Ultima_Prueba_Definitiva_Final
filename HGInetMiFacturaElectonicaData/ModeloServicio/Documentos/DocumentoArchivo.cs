using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Documento con el archivo XML en UBL
	/// </summary>
	public class DocumentoArchivo
	{

		#region Propiedades

		/// <summary>
		/// Código de seguridad (autenticación)
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string DataKey { get; set; }

		/// <summary>
		/// Número de identificación
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		[RegularExpression("^\\d+$", ErrorMessage = "La {0} debe contener sólo números.")]
		public string Identificacion { get; set; }

		/// <summary>
		/// Identificador del Documento asigando por el Facturador Electrónico
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string CodigoRegistro { get; set; }

		/// <summary>
		/// Indica el tipo de documento
		/// 1: Factura - 2: Nota Débito - 3: Nota Crédito
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		[Range(typeof(decimal), "0", "3", ErrorMessage = "El campo {0} debe estar entre {1} y {2}")]
		public int TipoDocumento { get; set; }

		/// <summary>
		/// Número del Documento
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public int Documento { get; set; }

		/// <summary>
		/// Número de Resolución del Documento asignado por la DIAN
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		[RegularExpression("^\\d+$", ErrorMessage = "El {0} debe contener sólo números.")]
		public string NumeroResolucion { get; set; }

		/// <summary>
		/// Prefijo del Documento según la Resolución asignada por la DIAN
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Archivo XML en estandar UBL del Documento
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public byte[] ArchivoXmlUbl { get; set; }

		#endregion
	}
}
