using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		[Description("PartitionKey")]
		PartitionCampo = 0,

		/// <summary>
		/// Campo RowKey
		/// </summary>
		[Description("RowKey")]
		RowCampo = 1

	}
}
