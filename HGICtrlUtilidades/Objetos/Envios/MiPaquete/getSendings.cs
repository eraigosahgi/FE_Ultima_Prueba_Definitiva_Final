using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	public class getSendings
	{
		/*		 
			{
			  "confirmationDate": {
				"init": "2021-07-08",
				"end": "2022-02-17"
			  },
			  "pageSize": 10,
			  "mpCode": 1193851
			}
		 */
		public confirmationDate confirmationDate { get; set; }
		public int pageSize { get; set; }
		public int mpCode { get; set; }		
	}

	public class confirmationDate
	{
		public DateTime init { get; set; }
		public DateTime end { get; set; }
	}

	public class getSendingsResponse
	{
		public int totalItems { get; set; }
		public List<sendings> sendings { get; set; }
	}

	public class sendings
	{
		public string _id { get; set; }
		public string deliveryCompany { get; set; }
		public List<tracking> tracking { get; set; }
		public List<string> pdfGuide { get; set; }
	}

}
