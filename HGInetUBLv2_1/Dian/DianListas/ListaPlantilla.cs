using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaPlantilla
	{
		public string ShortName = "[ShortName]";
		public string LongName = "[LongName]";
		public string Version = "[Version]";
		public string CanonicalUri = "[CanonicalUri]";
		public string CanonicalVersionUri = "[CanonicalVersionUri]";
		public string LocationUri = "[LocationUri]";
		public string AgencyName = "[AgencyName]";
		public string AgencyIdentifier = "[AgencyIdentifier]";

		public List<ListaItem> Items = new List<ListaItem>()
		{
			// [items]
			
		};

	}
}
