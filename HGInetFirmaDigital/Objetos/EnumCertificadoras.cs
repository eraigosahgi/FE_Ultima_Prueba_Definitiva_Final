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
		/// GSE- GESTIÓN DE SEGURIDAD ELECTRÓNICA S.A.
		/// http://www.gse.com.co
		/// </summary>
		[Description("GSE")]
		Gse = 1,

		/// <summary>
		/// ANDES SCD
		/// https://www.andesscd.com.co/
		/// </summary>
		[Description("Andes")]
		Andes = 2

	}
}
