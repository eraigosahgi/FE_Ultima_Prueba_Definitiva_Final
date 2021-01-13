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

	/// <summary>
	/// 6.5.  Registro de evento: ApplicationResponse Anexo V1.8 de la DIAN
	/// </summary>
	public enum CodigoResponseV2
    {
	    
		Pendiente = 0,
	    
	    [Description("Aceptación Expresa de Documento")]
	    [AmbientValue("033")]
		Expresa = 1,

	    [Description("Rechazo de Documento")]
	    [AmbientValue("031")]
		Rechazado = 2,

	    [Description("Aceptación Expresa de Documento")]
	    [AmbientValue("033")]
		AprobadoTacito = 3,

		[Description("Acuse de Recibo")]
		[AmbientValue("030")]
		Recibido = 4,

		[Description("Recepción de los Bienes y/o Servicios")]
		[AmbientValue("032")]
		Aceptado = 5,

		[Description("Documento validado por la DIAN")]
		[AmbientValue("02")]
		ValidadoDian = 10
	}
}
