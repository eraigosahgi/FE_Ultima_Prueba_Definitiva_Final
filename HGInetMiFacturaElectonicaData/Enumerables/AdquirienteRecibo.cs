using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public enum AdquirienteRecibo
    {

        [Description("Pendiente")]
        [AmbientValue("0")]
        Pendiente = 0,

        [Description("Aprobado")]
        [AmbientValue("1")]
        Aprobado = 1,

        [Description("Rechazado")]
        [AmbientValue("2")]
        Rechazado = 2,

        [Description("Aprobado Tácito")]
        [AmbientValue("3")]
        AprobadoTacito = 3,

		[Description("Entregado")]
        [AmbientValue("4")]
        Entregado = 4,

		[Description("Leído")]
		[AmbientValue("5")]
		Leido = 5,

		[Description("No Entregado")]
		[AmbientValue("6")]
		NoEntregado = 6,

		[Description("Enviado")]
		[AmbientValue("7")]
		Enviado = 7



	}
}
