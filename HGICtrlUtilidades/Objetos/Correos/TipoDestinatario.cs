using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	/// <summary>
	/// Indica el tipo de destinatario del correo electrónico (From, To, CC, BCC, Reply)
	/// </summary>
	public enum TipoDestinatario
	{
		[Description("[From] Correo electrónico de quien envía")]
		Desde = 0,

		[Description("[To] Correo electrónico destinatario principal")]
		Para = 1,

		[Description("[CC] Correo electrónico destinatario copia")]
		Copia = 2,

		[Description("[BCC] Correo electrónico destinatario copia oculta")]
		CopiaOculta = 3,

		[Description("[Reply] Correo electrónico destinatario copia oculta")]
		Responder = 4,
	}
}
