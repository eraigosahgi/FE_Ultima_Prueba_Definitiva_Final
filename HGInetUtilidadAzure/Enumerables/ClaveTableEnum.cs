using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUtilidadAzure
{
	/// <summary>
	/// Tipo de campo clave para Microsoft Azure Table
	/// </summary>
	public enum ClaveTableEnum
	{
		/// <summary>
		/// Campo PartitionKey
		/// </summary>
		PartitionCampo = 0,

		/// <summary>
		/// Campo RowKey
		/// </summary>
		RowCampo = 1

	}
}
