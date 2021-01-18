using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFirmaDigital
{

	/// <summary>
	/// Empresas certificadoras digitales
	/// </summary>
	public enum EnumCertificadoras
	{

		/// <summary>
		/// HGI SAS PROVEEDOR TECNOLOGICO
		/// https://www.hgi.com.co
		/// </summary>
		[Description("HGI SAS PROVEEDOR TECNOLOGICO")]
		[AmbientValue("privado")]
		[Category("0")]
		Hgi = 0,

		/// <summary>
		/// GSE- GESTIÓN DE SEGURIDAD ELECTRÓNICA S.A.
		/// http://www.gse.com.co
		/// </summary>
		[Description("GSE")]
		[AmbientValue("publico")]
		[Category("1")]
		Gse = 1,

		/// <summary>
		/// ANDES SCD
		/// https://www.andesscd.com.co/
		/// </summary>
		[Description("Andes")]
		[AmbientValue("publico")]
		[Category("1")]
		Andes = 2,

		/// <summary>
		/// CERTICAMARA
		/// http://www.certicamara.com
		/// </summary>
		[Description("CERTICAMARA")]
		[AmbientValue("publico")]
		[Category("1")]
		Certicamara = 3

	}
}
