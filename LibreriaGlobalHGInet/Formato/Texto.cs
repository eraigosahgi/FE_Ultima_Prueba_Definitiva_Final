using LibreriaGlobalHGInet.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LibreriaGlobalHGInet.Formato
{
	public static class Texto
	{
		/// <summary>
		/// Valida si una cadena de texto es numérica o no
		/// </summary>
		/// <param name="text" type="string">cadena de texto</param>
		/// <returns type="bool">indica si es numérico</returns>
		public static bool IsNumeric(this string text)
		{
			Regex regex = new Regex("^[0-9]+$");

			if (regex.IsMatch(text))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Valida si una cadena de texto cumple con la Expresion Regular
		/// </summary>
		/// <param name="expresion">Expresion Regular a utilizar</param>
		/// <param name="texto">cadena de texto a validar</param>
		/// <returns></returns>
		public static bool ValidarExpresion(TipoExpresion expresion, string texto)
		{

			switch (expresion)
			{
				case TipoExpresion.PaginaWeb:
					Regex IsWeb = new Regex("([\\w-]+\\.)+(/[\\w- ./?%&=]*)?");
					if (IsWeb.IsMatch(texto))
						return true;

					break;

				case TipoExpresion.Email:
					//Regex IsMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$");
					//Regex IsMail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
					//Regex IsMail = new Regex("^[a-zA-Z\u00f1\u00d10-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z\u00f1\u00d10-9-._]+(?:\\.[a-zA-Z\u00f1\u00d10-9-]+)*$");
					//Mejorada desde chatgpt
					Regex IsMail = new Regex(@"^[a-zA-ZñÑ0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-ZñÑ0-9-._]+(?:\.[a-zA-ZñÑ0-9-]+)*$");
					if (IsMail.IsMatch(texto))
						return true;

					break;

				case TipoExpresion.Numero:
					Regex IsNumeric = new Regex("^[0-9]+$");

					if (IsNumeric.IsMatch(texto))
						return true;

					break;

				case TipoExpresion.NumeroNotStartZero:
					Regex IsNumericNotZero = new Regex(@"^[1-9]\d*$");

					if (IsNumericNotZero.IsMatch(texto))
						return true;

					break;

				case TipoExpresion.Decimal:
					Regex IsDecimal = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d{1,6}$)$"); //new Regex(@"^(0|([1-9][0-9]*))([.][0-9]{2})?$"); 

					if (IsDecimal.IsMatch(texto))
						return true;
					break;

				case TipoExpresion.Celular:
					Regex IsCelular = new Regex(@"^\d{10}$");

					if (IsCelular.IsMatch(texto))
						return true;
					break;

				case TipoExpresion.Alfanumerico:
					Regex IsAlfanumeric = new Regex("^[a-zA-Z0-9]+$");

					if (IsAlfanumeric.IsMatch(texto))
						return true;
					break;

				//Encuentra si el texto tiene espacios en blanco: (inicio, final o inicio y final)
				case TipoExpresion.EspaciosEnBlanco:
					Regex IsEspacios = new Regex(@"\s");

					if (IsEspacios.IsMatch(texto))
						return true;
					break;

				default:
					return false;
					break;
			}

			return false;
		}

		/// <summary>
		/// Valida que el numero de celular sea solo numeros y tenga 10 digitos 
		/// </summary>
		/// <param name="numero_celular">numero del celular</param>
		/// <param name="codigo_pais">Codigo del pais para llamadas internacionales</param>
		/// <param name="completar"></param>
		/// <returns>El numero celular completo: codigo de pais mas el numero de celular</returns>
		public static string NumeroCelular(string numero_celular, int codigo_pais)
		{
			string celular_completo = string.Empty;

			if (!string.IsNullOrEmpty(numero_celular))
			{
				if (ValidarExpresion(TipoExpresion.Numero, numero_celular) && ValidarExpresion(TipoExpresion.Celular, numero_celular))
				{
					if (codigo_pais > 0)
					{
						celular_completo = string.Format("{0}{1}", codigo_pais, numero_celular);
					}
				}

			}

			return celular_completo;
		}

		/// <summary>
		/// Valida si una cadena de texto es vacía o no
		/// </summary>
		/// <param name="text" type="string">cadena de texto</param>
		/// <returns type="bool">indica si es numérico</returns>
		public static string FormatString(this string text)
		{


			if (string.IsNullOrEmpty(text) || text.Trim().ToLower().Equals("null"))
				return string.Empty;
			else
			{
				// elimina la comilla simple
				text = text.Replace("'", "");

				return text.Trim();
			}
		}

		/// <summary>
		/// Retorna el nombre sin caracteres
		/// </summary>
		/// <param name="text" type="string">cadena de texto</param>
		/// <returns type="bool">nombre sin caracter</returns>
		public static string NombreString(this string text)
		{
			string nombre_columna = "";
			string[] Separators = new string[text.Length + 1];
			string[] stringSeparators = new string[] { "_" };

			if (!string.IsNullOrEmpty(text) || !text.Trim().ToLower().Equals("null"))
			{
				if (text.Contains("_"))
					Separators = text.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
				else
				{
					char[] array = text.ToCharArray();

					for (int i = 0; i < array.Length; i++)
					{

						if (char.IsUpper(array[i]) && i > 0)
						{
							Separators[i] += " " + array[i].ToString();
						}
						else
							Separators[i] = array[i].ToString();
					}

					foreach (var item in Separators)
					{
						nombre_columna += item;
					}

				}
			}

			return RenombrarString(nombre_columna);
		}

		/// <summary>
		/// Renombra el nombre del campo
		/// </summary>
		/// <param name="text" type="string">cadena de texto</param>
		/// <returns type="bool">indica si es numérico</returns>
		public static string RenombrarString(this string text)
		{
			string nombre_columna = "";

			if (!string.IsNullOrEmpty(text) || !text.Trim().ToLower().Equals("null"))
			{
				if (text.Contains("Valor") && text.Contains(" "))
					nombre_columna = text.Replace("Valor", "Vrl.");
				else if (text.Contains("Codigo") && text.Contains(" "))
					nombre_columna = text.Replace("Codigo", "Cod.");
				else if (text.Contains("Anyo"))
					nombre_columna = text.Replace("Anyo", "Año");
				else if (text.Contains("Ano"))
					nombre_columna = text.Replace("Ano", "Año");
				else
					nombre_columna = text;

			}

			return nombre_columna.Trim();
		}


		#region Genera una clave automáticamente
		/// <summary>
		/// Genera codigos aleatorios
		/// </summary>
		/// <param name="clave_tamano"></param>
		/// <param name="tipo">AlfaNumerico = 0 - Alfa = 1 - Numerico = 2 </param>
		/// <returns></returns>
		public static string DatosAleatorios(int longitud, int tipo)
		{
			try
			{
				string texto = "";
				string alfaNumerico = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789@$?¿#";
				string alfa = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
				string numerico = "0123456789";
				int conteo;

				Byte[] randomBytes = new Byte[longitud];
				char[] chars = new char[longitud];

				for (int i = 0; i < longitud; i++)
				{
					Random randomObj = new Random();
					randomObj.NextBytes(randomBytes);

					if (tipo == 0)
					{
						texto = string.Format("{0}{1}", texto, alfaNumerico);
						conteo = alfaNumerico.Length;
						chars[i] = alfaNumerico[(int)randomBytes[i] % conteo];
					}

					if (tipo == 1)
					{
						texto = string.Format("{0}{1}", texto, alfa);
						conteo = alfa.Length;
						chars[i] = alfa[(int)randomBytes[i] % conteo];
					}

					if (tipo == 2)
					{
						texto = string.Format("{0}{1}", texto, numerico);
						conteo = numerico.Length;
						chars[i] = numerico[(int)randomBytes[i] % conteo];
					}

				}

				return new string(chars);
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}
		}
		#endregion
	}
}
