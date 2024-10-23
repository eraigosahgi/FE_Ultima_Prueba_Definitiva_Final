using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Formato
{
	public static class Coleccion
	{
		/// <summary>
		/// Convertir de IEnumerable a DataTable
		/// </summary>
		/// <typeparam name="TSource">tipo de dato del IEnumerable</typeparam>
		/// <param name="source" type="IEnumerable"></param>
		/// <returns type="DataTable">datos en DataTable</returns>
		public static DataTable Convertir<TSource>(IEnumerable<TSource> source)
		{
			try
			{
				var props = typeof(TSource).GetProperties();

				var dt = new DataTable();
				dt.Columns.AddRange(
				  props.Select(p => new DataColumn(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)).ToArray()
				);

				source.ToList().ForEach(
				  i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
				);

				return dt;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message);
			}


		}


		/// <summary>
		/// convierte un string en un lista
		/// </summary>
		/// <returns></returns>
		public static List<string> ConvertirLista(string lista)
		{
			List<string> array = new List<string>();

			if (lista == "*")
				return array;


			char[] MyChar = { '(', ')', ' ' };
			lista = lista.Trim(MyChar).Trim(' ');

			//Separators = new string[lista.Length + 1];
			string[] stringSeparators = new string[] { ",", "&" };

			string[] Separators = lista.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in Separators)
			{
				array.Add(item.ToString());
			}

			return array;

		}



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
		public static string ConvertListToString(List<string> lista, string seperador)
		{
			string dato = string.Join(seperador, lista.ToList());

			return dato;
		}

		/// <summary>
		/// Convierte un string separado por comas en una lista de byte
		/// </summary>
		/// <param name="lista">string separado por comas</param>
		/// <returns></returns>
		public static List<byte> ConvertirStringByte(string lista)
		{
			List<byte> array = new List<byte>();

			if (lista == "*")
				return array;


			char[] MyChar = { '(', ')', ' ' };
			lista = lista.Trim(MyChar).Trim(' ');

			//Separators = new string[lista.Length + 1];
			string[] stringSeparators = new string[] { ",", "&" };

			string[] Separators = lista.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in Separators)
			{
				array.Add(Convert.ToByte(item));
			}

			return array;

		}
		/// <summary>
		/// Convierte un string separado por comas (,) en una lista de Int
		/// </summary>
		/// <param name="lista">string separado por comas</param>
		/// <returns></returns>
		public static List<int> ConvertirStringInt(string lista)
		{
			List<int> array = new List<int>();

			if (lista == "*")
				return array;


			char[] MyChar = { '(', ')', ' ' };
			lista = lista.Trim(MyChar).Trim(' ');

			//Separators = new string[lista.Length + 1];
			string[] stringSeparators = new string[] { ",", "&" };

			string[] Separators = lista.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in Separators)
			{
				array.Add(Convert.ToInt32(item));
			}

			return array;

		}

		/// <summary>
		/// Convierte un string separado por comas (,) en una lista de Int
		/// </summary>
		/// <param name="lista">string separado por comas</param>
		/// <returns></returns>
		public static List<long> ConvertirStringlong(string lista)
		{
			List<long> array = new List<long>();

			if (lista == "*")
				return array;


			char[] MyChar = { '(', ')', ' ' };
			lista = lista.Trim(MyChar).Trim(' ');

			//Separators = new string[lista.Length + 1];
			string[] stringSeparators = new string[] { ",", "&" };

			string[] Separators = lista.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in Separators)
			{
				array.Add(Convert.ToInt64(item));
			}

			return array;

		}

		/// <summary>
		/// convierte una lista String a un String
		/// </summary>
		/// <returns></returns>
		public static string ConvertirString(List<string[]> lista)
		{
			List<string> array = new List<string>();

			int i;
			var datos = "";
			for (i = 0; i < lista.Count(); i++)
			{
				if (datos != "") datos = datos + ",";
				datos = datos + lista[i][0];
			}

			return datos;

		}

		/// <summary>
		/// Convierte una cadena string a un formato aceptado por SQL
		/// Recibe: (dato1,dato2,dato3
		/// Retorna: 'dato1','dato2','dato3'
		/// </summary>
		/// <param name="lista">cadena de string (dato1,dato2,dato3</param>
		/// <returns>string</returns>
		public static string ConvertirCadenaSQL(string lista)
		{
			string[] stringSeparators = new string[] { ",", "&", "(" };
			string[] Separators = lista.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			string cadena_retorno = string.Empty;

			for (int i = 0; i < Separators.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(cadena_retorno))
					cadena_retorno = string.Format("'{0}'", Separators[i]);
				else
					cadena_retorno = string.Format("{0},'{1}'", cadena_retorno, Separators[i]);
			}
			return cadena_retorno;
		}

	}
}
