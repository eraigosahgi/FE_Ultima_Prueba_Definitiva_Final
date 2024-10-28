using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.Documentos;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetUBLv2_1.XML
{
	public class ValoresAdicionalesXML
	{

		public static AllowanceChargeType[] ObtenerValoresAd(object documento )//, TipoDocumento tipo_doc)
		{
			var documento_obj = (dynamic)null;
			documento_obj = documento;

			int id = 1;
			List<AllowanceChargeType> AllowanceChargeT = new List<AllowanceChargeType>();
			//if (tipo_doc.Equals(TipoDocumento.Factura))
			//{

			//}

			if (documento_obj.Cargos != null)
			{
				foreach (var item in documento_obj.Cargos)
				{
					AllowanceChargeType cargo = new AllowanceChargeType();
					cargo.ID = new IDType();
					cargo.ID.Value = id.ToString();
					cargo.ChargeIndicator = new ChargeIndicatorType();
					cargo.ChargeIndicator.Value = true;
					List<AllowanceChargeReasonType>  List_razon = new List<AllowanceChargeReasonType>();
					AllowanceChargeReasonType razon = new AllowanceChargeReasonType();
					razon.Value = item.Descripcion;
					List_razon.Add(razon);
					cargo.AllowanceChargeReason = List_razon.ToArray();
					cargo.MultiplierFactorNumeric = new MultiplierFactorNumericType();
					cargo.MultiplierFactorNumeric.Value = item.Porcentaje;
					cargo.Amount = new AmountType2();
					cargo.Amount.Value = item.Valor;
					cargo.Amount.currencyID = documento_obj.Moneda;
					cargo.BaseAmount = new BaseAmountType();
					cargo.BaseAmount.Value = documento_obj.ValorSubtotal;
					cargo.BaseAmount.currencyID = documento_obj.Moneda;
					id++;
					AllowanceChargeT.Add(cargo);
				}
			}

			if (documento_obj.Descuentos != null)
			{
				foreach (var item in documento_obj.Descuentos)
				{
					AllowanceChargeType descuento = new AllowanceChargeType();
					descuento.ID = new IDType();
					descuento.ID.Value = id.ToString();
					descuento.ChargeIndicator = new ChargeIndicatorType();
					descuento.ChargeIndicator.Value = false;
					descuento.AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType();
					descuento.AllowanceChargeReasonCode.Value = item.Codigo;
					List<AllowanceChargeReasonType> List_razon = new List<AllowanceChargeReasonType>();
					AllowanceChargeReasonType razon = new AllowanceChargeReasonType();
					ListaCodigoDescuento list_razon_desc = new ListaCodigoDescuento();
					ListaItem razon_desc = list_razon_desc.Items.Where(d => d.Codigo.Equals(item.Codigo)).FirstOrDefault();
					razon.Value = razon_desc.Nombre;
					List_razon.Add(razon);
					descuento.AllowanceChargeReason = List_razon.ToArray();
					descuento.MultiplierFactorNumeric = new MultiplierFactorNumericType();
					descuento.MultiplierFactorNumeric.Value = item.Porcentaje;
					descuento.Amount = new AmountType2();
					descuento.Amount.Value = item.Valor;
					descuento.Amount.currencyID = documento_obj.Moneda;
					descuento.BaseAmount = new BaseAmountType();
					descuento.BaseAmount.Value = documento_obj.ValorSubtotal;
					descuento.BaseAmount.currencyID = documento_obj.Moneda;
					id++;
					AllowanceChargeT.Add(descuento);
				}
			}
			

			return AllowanceChargeT.ToArray();
		}

	}
}
