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

        [Description("Acuse Recibo")]
        [AmbientValue("1")]
        Aprobado = 1,

        [Description("Rechazado")]
        [AmbientValue("2")]
        Rechazado = 2,

        [Description("Aprobado Tácito")]
        [AmbientValue("3")]
        AprobadoTacito = 3,

        [Description("Recibo Mercancia")]
        [AmbientValue("4")]
        Entregado = 4,

        [Description("Aceptación Expresa")]
        [AmbientValue("5")]
        Leido = 5,

        [Description("Título Valor")]
        [AmbientValue("6")]
        NoEntregado = 6,

        [Description("Endoso")]
        [AmbientValue("7")]
        Enviado = 7,

		[Description("Mandato")]
		[AmbientValue("20")]
		Mandato = 20

	}

	/// <summary>
	/// 6.5.  Registro de evento: ApplicationResponse Anexo V1.8 de la DIAN
	/// </summary>
	public enum CodigoResponseV2
    {
	    
		Pendiente = 0,

		[Description("Acuse de recibo de Factura Electrónica de Venta")]
		[AmbientValue("030")]
		Recibido = 1,

		[Description("Reclamo de la Factura Electrónica de Venta")]
		[AmbientValue("031")]
		Rechazado = 2,

		[Description("Aceptación Tácita")]
		[AmbientValue("034")]
		AprobadoTacito = 3,

		[Description("Recibo del bien y/o prestación del servicio")]
		[AmbientValue("032")]
		Aceptado = 4,

		[Description("Aceptación expresa")]
	    [AmbientValue("033")]
		Expresa = 5,

		[Description("Inscripción de la factura electrónica de venta como título valor - RADIAN")]
		[AmbientValue("036")]
		Inscripcion = 6,

		[Description("Endoso en propiedad")]
		[AmbientValue("037")]
		EndosoPp = 7,

		[Description("Endoso en garantía")]
		[AmbientValue("038")]
		EndosoG = 8,

		[Description("Cancelación de endoso")]
		[AmbientValue("040")]
		CancelacionEG = 9,

		[Description("Documento validado por la DIAN")]
		[AmbientValue("02")]
		ValidadoDian = 10,

		[Description("Aval")]
		[AmbientValue("035")]
		Aval = 12,

		[Description("Endoso en procuración")]
		[AmbientValue("039")]
		EndosoPc = 15,

		[Description("Mandato")]
		[AmbientValue("043")]
		MandatoG = 20,

		[Description("Terminacion del Mandato")]
		[AmbientValue("044")]
		TerminacionMandatoG = 21,

		[Description("Pago de la factura electrónica de venta como título valor")]
		[AmbientValue("045")]
		PagoFvTV = 22,

		[Description("Informe para el pago")]
		[AmbientValue("046")]
		InformePago = 23
	}
}
