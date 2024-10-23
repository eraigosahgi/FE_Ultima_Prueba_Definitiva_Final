using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.HgiNet.Funciones
{
	public class ValidarRespuestas
	{
		/// <summary>
		/// Indica si debe o no retornar la excepción con el mensaje de la respuesta.
		/// </summary>
		/// <param name="id_respuesta"></param>
		/// <returns></returns>
		public bool ValidarRespuesta(List<string> id_respuesta)
		{
			try
			{
				foreach (string item in id_respuesta)
				{
					if (!item.Equals("1"))
						return true;
				}

				return false;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

	}
}
