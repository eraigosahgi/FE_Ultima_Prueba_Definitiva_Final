using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail
{
	public enum MensajeEstado
	{
		[Description("Procesado")]
		Processed = 0,

		[Description("En cola")]
		Queued = 1,

		[Description("Enviado")]
		Sent = 2,

		[Description("Abierto")]
		Opened = 3,

		[Description("Presionó")]
		Clicked = 4,

		[Description("Rebotado")]
		Bounce = 5,

		[Description("Spam")]
		Spam = 6,

		[Description("Desuscripción")]
		Unsub = 7,

		[Description("Bloqueado")]
		Blocked = 8,

		[Description("Rebotado")]
		SoftBounce = 9,

		[Description("Rebotado")]
		HardBounce = 10,

		[Description("Diferido")]
		Deferred = 11,

		[Description("Rebote Suave")]
		softbounced = 12,

		[Description("rebotó duro")]
		hardbounced = 13



	}
}
