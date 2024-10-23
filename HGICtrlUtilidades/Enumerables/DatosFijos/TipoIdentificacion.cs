using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoIdentificacion
	{
		[Description("NI - Numero de Identificación Tributaria")]
		[AmbientValue("NI")]
		NI = 31,

		[Description("CC - Cedula de Ciudadania")]
		[AmbientValue("CC")]
		CC = 13,

		[Description("CE - Cedula de Extranjería")]
		[AmbientValue("CE")]
		CE = 22,

		[Description("TI - Tarjeta de Identidad")]
		[AmbientValue("TI")]
		TI = 12,

		[Description("TE - Tarjeta de Extranjería")]
		[AmbientValue("TE")]
		TE = 21,

		[Description("RC - Registro Civil")]
		[AmbientValue("RC")]
		RC = 11,

		[Description("PA - Pasaporte")]
		[AmbientValue("PA")]
		PA = 41,

		[Description("DE - Tipo de Documento Extranjero")]
		[AmbientValue("DE")]
		DE = 42,

		//Se comenta ya que en la base de datos solo permite dos posiciones
		//[659383]
		//[Description("NIO - NIT de otro país")]
		//[AmbientValue("NIO")]
		//NIO = 50,

		////* Deberá utilizarse solamente para el adquirente, debido a que este tipo de documento no pertenece a los tipos de documento en la base de datos del RUT
		//[Description("NUIP - documento no pertenece a la base de datos del RUT(utilizar solamente para el adquirente)")]
		//[AmbientValue("NUIP")]
		//NUIP = 91,

		/*Permiso de extranjeros*/
		[Description("PE - Permiso Especial de permanencia")]
		[AmbientValue("PE")]
        PE = 92,

        /*Permiso de extranjeros*/
        [Description("PT - Permiso por Protección Temporal")]
        [AmbientValue("PT")]
        PT = 43,


    }
}
