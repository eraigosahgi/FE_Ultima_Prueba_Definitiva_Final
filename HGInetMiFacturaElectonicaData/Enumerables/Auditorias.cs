using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	/// <summary>
	/// Indica el tipo de tarea ejecutada
	/// </summary>
	public enum TipoRegistro
	{
		[Description("Proceso")]
		Proceso = 1,

		[Description("Creación")]
		Creacion = 2,

		[Description("Actualización")]
		Actualizacion = 3,
	}

	/// <summary>
	/// Indica desde donde es realizado un proceso
	/// </summary>
	public enum Procedencia
	{
		[Description("Plataforma")]
		Plataforma = 1,

		[Description("Usuario")]
		Usuario = 2,

		[Description("Sonda")]
		Sonda = 3,

		[Description("Dian")]
		Dian = 4,

		[Description("Mail")]
		Mail = 5,

		[Description("Sms")]
		Sms = 6,

	}
}
