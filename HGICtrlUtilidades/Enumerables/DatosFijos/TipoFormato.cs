using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum TipoFormato
	{
		[Description("Documentos")]
		Documentos = 1,

		[Description("Pagos")]
		Pagos = 2,

		[Description("Causaciones")]
		Causaciones = 3,

		[Description("Comprobante")]
		Comprobante = 4,

		[Description("Planilla")]
		Planilla = 5,

        [Description("Recaudos")]
        Recaudos = 12,

        /*[Description("Caso")]
		Caso = 6,

		[Description("Productos")]
		Productos = 7,
	
		[Description("Terceros")]
		Terceros = 8,

		[Description("Asamblea")]
		Asamblea = 9,

		[Description("Cesantias")]
		Cesantias = 10,

		[Description("LiquidacionDef")]
		LiquidacionDef = 11,


		[Description("Prima")]
		Prima = 13,

		[Description("CesantiasAnt")]
		CesantiasAnt = 14,

		[Description("ListaChequeo")]
		ListaChequeo = 15,

		[Description("CertificadoLaboral")]
		CertificadoLaboral = 16,

		[Description("SaldoContable")]
		SaldoContable = 17,

		[Description("PagosConAnexo")]
		PagosConAnexo = 18,

		[Description("Ven_Contratos")]
		Ven_Contratos = 19,*/
    }
}
