using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFacturaETestConsola
{
	public class Documento
	{
		public decimal UBLVersionID { get; set; }
		public decimal CustomizationID { get; set; }
		public Int64 ID { get; set; }
		public DateTime IssueDate { get; set; }
		public DateTime IssueTime { get; set; }
		public DateTime ResponseDate { get; set; }
		public DateTime ResponseTime { get; set; }
		public List<string> Notes { get; set; }
		public string DocumentCurrencyCode { get; set; }
		public int AdditionalAccountID { get; set; }

	}
}
