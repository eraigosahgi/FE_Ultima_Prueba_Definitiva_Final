using HGInetFacturaEServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HGInetFacturaETestConsola
{
	class Program
	{
		static void Main(string[] args)
		{

			PruebaCufe();

		}

		public static void PruebaCufe()
		{
			try
			{

				string cufe = Ctl_Factura.CalcularCUFE("dd85db55545bd6566f36b0fd3be9fd8555c36e", "990000402", new DateTime(2018, 4, 23), "811021438", "13", "1020395355", 47500.00M, 39916.00M, 7584.00M, 0.00M, 0.00M);

				System.Diagnostics.Debug.WriteLine("CUFE GENERADO : " + cufe);


				string cufeNC = Ctl_NotaCredito.CalcularCUFE("5790af0483698d226240b7c4abafd49eb0dc8d480f6a7e44dfe4a523fed2129a", "3d03ae717f28dab49c7588ffa4ef006f776ac468", "602", new DateTime(2018, 4, 23), "811021438", "13", "79758318", 687196.00M, 607870.00M, 109720.00M, 0.00M, 0.00M);

				System.Diagnostics.Debug.WriteLine("cufeNC GENERADO : " + cufeNC);

			}
			catch (Exception excepcion)
			{
				System.Diagnostics.Debug.WriteLine("ERRO GENERANDO CUFE" + excepcion.Message);
			}

		}


		public static void test1()
		{


			//HGInetFacturaEServicios.ServicioFactura.Factura factura = new HGInetFacturaEServicios.ServicioFactura.Factura();

			Ctl_Factura.Test("http://habilitacion.mifacturaenlinea.com.co");


			Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

			//  ^[0-9]+(\\,[0-9]{2,2})?$

			//  ^[0-9]([.,][0-9]{1,3})?$

			//  [0-9]+(\.[0-9][0-9]?)?

			//  ^\d+(\.\d{1,2})?$

			// @"\d{1,12}\.\d\d$"

			//  @"(^(0|([1-9][0-9]*))(\.[0-9]{1,2})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{2,2})?$)"

			decimal valor = 102.3m;
			string valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal1 = isnumber.IsMatch(valor_txt);


			valor = 2365478523102.30m;
			valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal2 = isnumber.IsMatch(valor_txt);



			valor = 102.307m;
			valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal3 = isnumber.IsMatch(valor_txt);


			valor = 0.30m;
			valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal4 = isnumber.IsMatch(valor_txt);



			decimal x = 0.00m;

			decimal y = 12;

			x = y;
		}
	}
}
