using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	public class cancelSending
	{
		/* {"mpCode": 1086244} */
		public int mpCode { get; set; }
	}

	public class cancelSendingResponse
	{
		/*
		{
    "message": "the shipment has beed updated",
    "cashMovement": {
        "userInfo": {
            "userId": "61bd01f43fc5580ae9c8216d",
            "cash": 285530,
            "cashComission": "shipment does not have collection service",
            "cashSending": 300000,
            "newCash": 300000,
            "message": "user cash was updated"
        },
        "transactions": {
            "productCode": 1086244,
            "paymentType": 101,
            "user": "61bd01f43fc5580ae9c8216d",
            "paymentPrice": 14470,
            "cash": 285530,
            "newCash": 300000,
            "transactionType": "income",
            "description": "MP1086244-Reintegro costo del envío por cancelación",
            "createdAt": "2022-01-19T16:12:36.482Z",
            "updatedAt": "2022-01-19T16:12:36.482Z",
            "_id": "61e8387473c3a347aba05c31"
        },
        "message": "the transaction has been done succesfully"
    },
    "guideCancellation": {
        "message": "Se realizó la correcta anulación de la remesa en los modelos temporal y definitivo."
    }
} 
		*/

		public string message { get; set; }
		public cashMovement cashMovement { get; set; }
		public guideCancellation guideCancellation { get; set; }
	}

	public class cashMovement
	{
		public userInfo userInfo { get; set; }
		public List<transactions> transactions { get; set; }
		public string message { get; set; }
	}

	public class userInfo
	{
		public string userId { get; set; }
		public decimal cash { get; set; }
		public string cashComission { get; set; }
		public decimal cashSending { get; set; }
		public decimal newCash { get; set; }
		public string message { get; set; }
	}

	public class transactions
	{
		public decimal productCode { get; set; }
		public int paymentType { get; set; }
		public string user { get; set; }
		public decimal paymentPrice { get; set; }
		public decimal cash { get; set; }
		public decimal newCash { get; set; }
		public string transactionType { get; set; }
		public string description { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }
		public string _id { get; set; }
	}

	public class guideCancellation
	{
		public string message { get; set; }
	}
}
