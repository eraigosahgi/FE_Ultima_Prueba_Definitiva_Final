using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFirmaDigital.Funciones
{
	public class Seguridad
	{
		public static string Encrypt(string input, string key)
		{
			byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.Zeros;
			ICryptoTransform cTransform = tripleDES.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
			tripleDES.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		public static string Decrypt(string input, string key)
		{
			byte[] inputArray = Convert.FromBase64String(input);
			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.Zeros;
			ICryptoTransform cTransform = tripleDES.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
			tripleDES.Clear();
			return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
		}

	}
}
