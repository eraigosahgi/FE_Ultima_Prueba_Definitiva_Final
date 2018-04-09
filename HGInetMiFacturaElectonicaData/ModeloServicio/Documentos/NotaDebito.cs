using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class NotaDebito
	{
        /// <summary>
		/// Código de seguridad (autenticación)
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string DataKey { get; set; }

        /// <summary>
        /// Identificador del Documento asigando por el Facturador Electrónico
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string CodigoRegistro { get; set; }

    }
}
