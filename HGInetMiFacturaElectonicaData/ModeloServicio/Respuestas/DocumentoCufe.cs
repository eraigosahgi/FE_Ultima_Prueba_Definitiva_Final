using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class DocumentoCufe
	{
		/// <summary>
		/// Código de seguridad (autenticación)
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string DataKey { get; set; }

		/// <summary>
		/// Número de identificación del obligado.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string IdentificacionObligado { get; set; }

		/// <summary>
		/// Indica el tipo de Documento(1: factura - 2: nota débito - 3: nota crédito)
		/// </summary>
		[Range(typeof(int), "0", "3", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public int DocumentoTipo { get; set; }

		/// <summary>
		/// Version de la DIAN que fue enviado el Documento
		/// 2: Versión Validación Previa DIAN
		/// </summary>
		public int IdVersionDian { get; set; }

		/// <summary>
		/// Número del Documento
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public long Documento { get; set; }

		/// <summary>
		/// Prefijo
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Clave técnica de la Resolución asignada por la DIAN. (Aplica para Documento tipo Factura)
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string ClaveTecnica { get; set; }

		/// <summary>
		/// Fecha del documento electrónico
		/// </summary>
		public DateTime Fecha { get; set; }

		/// <summary>
		/// Número de identificación del adquiriente.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
		public string IdentificacionAdquiriente { get; set; }

		/// <summary>
		/// Subtotal del documento: valor del documento con descuentos y sin impuestos
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
		//[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public decimal ValorSubtotal { get; set; }

		/// <summary>
		/// Valor total de IVA del documento
		/// </summary>
		//[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public decimal ValorIva { get; set; }

		/// <summary>
		/// Valor total de Impuesto al consumo del documento
		/// </summary>
		//[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public decimal ValorImpuestoConsumo { get; set; }

		/// <summary>
		/// Valor total de Retención de ICA del documento.
		/// </summary>
		//[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public decimal ValorIca { get; set; }

		/// <summary>
		/// Valor total del documento: Subtotal incluyendo descuentos e impuestos agregados
		/// </summary>
		//[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
		public decimal Total { get; set; }

		/// <summary>
		/// CUFE o CUDE calculado por la plataforma
		/// </summary>
		public string Cufe { get; set; }

		/// <summary>
		/// Código QR calculado por la plataforma
		/// </summary>
		public string QR { get; set; }


		/// <summary>
		/// Objeto de tipo Error 
		/// </summary>
		public Error Error { get; set; }

	}
}
