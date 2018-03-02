using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUtilidadAzure
{
	/// <summary>
	/// Tipo de acceso al blob
	/// BlobContainerPublicAccessType
	/// </summary>
	public enum TipoAccesoEnum
	{
		/// <summary>
		/// Sin acceso público. Solo el propietario de la cuenta puede leer recursos en este contenedor.
		/// </summary>
		SinAccesoPublico = 0,

		/// <summary>
		/// Acceso público a nivel de contenedor. Los clientes anónimos pueden leer datos de contenedor y blob.
		/// </summary>
		Contenedor = 1,

		/// <summary>
		/// Acceso público a nivel de archivo. Los clientes anónimos pueden leer datos de blobs dentro de este contenedor, pero no datos de contenedor.
		/// </summary>
		Blob = 2,

		/// <summary>
		/// Tipo de acceso desconocido
		/// </summary>
		Unknown = 3

	}
}
