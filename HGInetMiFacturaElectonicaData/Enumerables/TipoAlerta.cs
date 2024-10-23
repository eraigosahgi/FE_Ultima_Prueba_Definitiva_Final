using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
	public enum TipoAlerta
	{
		[Description("Porcenjate")]
		Porcenjate = 1,

		[Description("SinPlan")]
		SinPlan = 2,

		[Description("Porvencer")]
		Porvencer = 3,

		[Description("PoRecarga")]
		PoRecarga = 4,

		[Description("PorPago")]
		PorPago = 5,

		[Description("SolicitudAprobacionFormato")]
		SolicitudAprobacionFormato = 6,

		[Description("AprobacionFormato")]
		AprobacionFormato = 7,

		[Description("PublicacionFormato")]
		PublicacionFormato = 8,

		[Description("AlertaDocDIAN")]
		AlertaDocDIAN = 9,
	}


	public enum Notificacion
	{
		[Description("Activa")]
		Activa = 1,

		[Description("Inactiva")]
		Inactiva = 2,
	}
}
