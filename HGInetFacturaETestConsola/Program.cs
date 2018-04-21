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
