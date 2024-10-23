using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	/// <summary>
	/// Cotización de envío
	/// </summary>
	public class quoteShipping
	{
		/*{
		"originLocationCode": "05001000",
		"destinyLocationCode": "05001000",
		"height": 100,
		"width": 25,
		"length": 30,
		"weight": 3,
		"quantity": 1,
		"declaredValue": 50000,
		"saleValue": 40000
		}*/
		public string originLocationCode { get; set; }
		public string destinyLocationCode { get; set; }
		public double height { get; set; }
		public double width { get; set; }
		public double length { get; set; }
		public double weight { get; set; }
		public double quantity { get; set; }
		public double declaredValue { get; set; }
		public double saleValue { get; set; }
	}

	public class quoteShippingResponse
	{
		/*{
				"id": "60b4912fe0692b26737d0e7a",
				"deliveryCompanyName": "TCC",
				"deliveryCompanyImgUrl": "https://s3.amazonaws.com/images.dev.mipaquete/tcc.png",
				"shippingCost": 8560,
				"collectionCommissionWithRate": 5200,
				"collectionCommissionWithOutRate": 5200,
				"isMessengerService": true,
				"shippingTime": 2880,
				"realWeightVolume": 3,
				"officeAddress": "Cra 64 # 67 B-35",
				"forwardingService": false,
				"singleOfficeDelivery": false,
				"pickupService": true,
				"pickupTime": 360,
				"deliveryCompanyId": "5ca22d9587981510092322f6",
				"score": 4.6
			}*/

		public string id { get; set; }
		public string deliveryCompanyName { get; set; }
		public string deliveryCompanyImgUrl { get; set; }
		public decimal shippingCost { get; set; }
		public decimal collectionCommissionWithRate { get; set; }
		public decimal collectionCommissionWithOutRate { get; set; }

		/// <summary>
		/// Servicios de Mensajería
		/// </summary>
		public bool isMessengerService { get; set; }

		/// <summary>
		/// Tiempo de envío en minutos
		/// </summary>
		public decimal shippingTime { get; set; }

		/// <summary>
		/// Volumen peso real
		/// </summary>
		public decimal realWeightVolume { get; set; }

		/// <summary>
		/// Dirección oficina
		/// </summary>
		public string officeAddress { get; set; }

		/// <summary>
		/// Servicio de reenvío
		/// </summary>
		public bool forwardingService { get; set; }
		public bool singleOfficeDelivery { get; set; }
		public bool pickupService { get; set; }

		/// <summary>
		/// Hora recogida
		/// </summary>
		public decimal pickupTime { get; set; }

		/// <summary>
		/// Id de compañía transportadora
		/// </summary>
		public string deliveryCompanyId { get; set; }

		/// <summary>
		/// Calificación
		/// </summary>
		public decimal score { get; set; }

		/// <summary>
		/// Fecha estimada para realizar la entrega
		/// </summary>
		public string EntregaEstimada { get; set; }

	}

}
