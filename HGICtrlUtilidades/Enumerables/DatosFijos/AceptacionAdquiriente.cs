using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Enumerables.DatosFijos
{
	public enum AceptacionAdquiriente
	{
		[Description("Pendiente")]
		Pendiente = 0,

		[Description("Acuse de Recibo")]
		Aprobado = 1,

		[Description("Reclamo de la Factura")]
		Rechazo = 2,

		[Description("Aceptación Tácita")]
		Aprobadotacito = 3,

		[Description("Recibo del Bien y/o prestación serv")]
		ReciboDelBien = 4,
		[Description("Aceptación Expresa")]
		AceptacionE = 5,
		[Description("Inscripción de la factura electrónica")]
		InscripcionF = 6,
		[Description("Endoso en propiedad")]
		Endoso = 7,

	}


}
