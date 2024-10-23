using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class EntregaDocumentoType
	{
		private string WSField;

		private ParametrosArgumentosType[] ParametroArgumentosField;

		public string WS
		{
			get
			{
				return this.WSField;
			}
			set
			{
				this.WSField = value;
			}
		}

		public ParametrosArgumentosType[] ParametroArgumentos
		{
			get
			{
				return this.ParametroArgumentosField;
			}
			set
			{
				this.ParametroArgumentosField = value;
			}
		}
	}
}
