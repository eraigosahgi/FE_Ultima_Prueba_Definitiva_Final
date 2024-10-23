using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_Coleccion
	{
		/// <summary>
		/// convierte un string en un lista
		/// </summary>
		/// <returns></returns>
		public static List<string> ConvertirLista(string lista, char separador)
		{
			List<string> array = new List<string>();

			lista = lista.Trim(' ');

			char[] stringSeparators = new char[] { separador };

			string[] Separators = lista.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in Separators)
			{
				array.Add(item.ToString());
			}

			return array;

		}

		/// <summary>
		/// Convierte una lista de string a un string
		/// </summary>
		/// <param name="lista">lista de string</param>
		/// <param name="seperador">caracter</param>
		/// <returns></returns>
		public static string ConvertirListToString(List<string> lista, string seperador)
		{
			string dato = string.Join(seperador, lista.ToList());

			return dato;
		}

		/// <summary>
		/// Convierte una lista de Guid a un string
		/// </summary>
		/// <param name="lista">lista de string</param>
		/// <param name="seperador">caracter</param>
		/// <returns></returns>
		public static string ConvertirListGuidToString(List<Guid> lista, string seperador)
		{
			string dato = string.Join(seperador, lista.ToList());

			return dato;
		}
	}
}
