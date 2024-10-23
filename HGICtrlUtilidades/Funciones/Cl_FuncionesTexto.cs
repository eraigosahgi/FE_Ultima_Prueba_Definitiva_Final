using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Funciones
{
	public class Cl_FuncionesTexto
	{
		public static string ReemplazaCaracter(string PstrDato, string PstrCaracter, string PstrCambiarPor)
		{
			string texto_retorno = string.Empty;
			try
			{
				if (PstrDato.Length > 0)
				{
					texto_retorno = PstrDato.Replace(PstrCaracter, PstrCambiarPor);
				}
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
			return texto_retorno;
		}

	}
}
