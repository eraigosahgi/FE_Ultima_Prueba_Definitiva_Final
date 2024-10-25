using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public enum TipoArchivoStorage
	{
		[Description("Xml-Ubl")]
		XML = 1,

		[Description("Pdf")]
		PDF = 2,

		[Description("Zip")]
		ZIP = 3,

		[Description("Xml-Ubl Ultimo Acuse de Recibo")]
		XMLACUSE = 4,

		[Description("Zip Attachdocument - PDF")]
		ZIPAttached = 5,

		[Description("Zip Anexos")]
		ZIPAnexo = 6,

		[Description("Xml-Ubl Respuesta DIAN")]
		XMLRESPDIAN = 7,

		[Description("Xml-Ubl Attachdocument")]
		XMLAttached = 8,
	}
}
