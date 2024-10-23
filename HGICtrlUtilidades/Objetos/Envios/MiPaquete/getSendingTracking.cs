using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	public class getSendingTracking
	{
		/*{
		"deliveryCompanyName": "TCC",
		"deliveryCompany": "5ca22d9587981510092322f6",
		"tracking": [
			{
				"updateState": "Envío pendiente por pago",
				"date": "2022-01-04T20:17:34.049Z"

			},
			{
				"updateState": "Procesando tu envío",
				"date": "2022-01-04T20:17:34.120Z"
			},
			{
				"updateState": "Envío programado",
				"date": "2022-01-04T20:17:37.668Z"
			},
			{
				"updateState": "Envío cancelado",
				"date": "2022-01-05T15:47:16.610Z"
			}
		],
		"guideNumber": "4490027156",
		"mpCode": 1086149,
		"origin": "MEDELLÍN",
		"destiny": "MEDELLÍN"
		}*/
		public string deliveryCompanyName { get; set; }
		public string deliveryCompany { get; set; }
		public string guideNumber { get; set; }
		public decimal mpCode { get; set; }
		public string origin { get; set; }
		public string destiny { get; set; }
		public List<tracking> tracking { get; set; }
	}

	public class tracking
	{
		/*
		  "updateState": "Envío programado",
            "date": "2022-01-04T20:17:37.668Z"
		*/
		public string updateState { get; set; }
		public DateTime date { get; set; }
	}
}
