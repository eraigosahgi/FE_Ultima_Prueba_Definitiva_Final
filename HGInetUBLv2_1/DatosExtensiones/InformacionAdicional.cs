using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public class InformacionAdicional
	{
		private InformacionTicket InformacionTicketField;

		public InformacionTicket InformacionTicket
		{
			get
			{
				return this.InformacionTicketField;
			}
			set
			{
				this.InformacionTicketField = value;
			}
		}
	}
}
