using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public class ValidacionObjeto
	{

		public bool Validar(object documento_obj, TipoDocumento tipo_doc)
		{

			bool resultado = false;

			//var documento = (dynamic)null;
			if (tipo_doc == TipoDocumento.Factura)
			{
				Factura documento = (Factura)documento_obj;

				ListaFormasPago list_forma_pago = new ListaFormasPago();

				ListaItem forma_pago = list_forma_pago.Items.Where(d => d.Codigo.Equals(documento.FormaPago.ToString())).FirstOrDefault();

				ListaTipoMoneda list_Moneda = new ListaTipoMoneda();

				ListaItem moneda = list_Moneda.Items.Where(d => d.Codigo.Equals(documento.Moneda.ToString())).FirstOrDefault();

			}



			return resultado;
		}
	}
}
