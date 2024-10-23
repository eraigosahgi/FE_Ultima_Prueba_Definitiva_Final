using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LibreriaGlobalHGInet.General
{
	public static class Seguridad
	{

		// This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
		// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
		// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
		private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

		// This constant is used to determine the keysize of the encryption algorithm.
		private const int keysize = 256;

		public static string Encriptar(string v, object hgiNet_LlaveEncripcion)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Encripta una cadena de texto con una llave
		/// </summary>
		/// <param name="plainText">texto a encriptar</param>
		/// <param name="passPhrase">llave para encriptar/desencriptar</param>
		/// <returns>texto encriptado</returns>
		public static string Encriptar(string plainText, string passPhrase)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
			{
				byte[] keyBytes = password.GetBytes(keysize / 8);
				using (RijndaelManaged symmetricKey = new RijndaelManaged())
				{
					symmetricKey.Mode = CipherMode.CBC;
					using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
							{
								cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
								cryptoStream.FlushFinalBlock();
								byte[] cipherTextBytes = memoryStream.ToArray();
								return Convert.ToBase64String(cipherTextBytes);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Desencripta una cadena de texto con una llave
		/// </summary>
		/// <param name="cipherText">texto encriptado</param>
		/// <param name="passPhrase">llave para encriptar/desencriptar</param>
		/// <returns>texto desencriptado</returns>
		public static string Desencriptar(string cipherText, string passPhrase)
		{
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
			using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
			{
				byte[] keyBytes = password.GetBytes(keysize / 8);
				using (RijndaelManaged symmetricKey = new RijndaelManaged())
				{
					symmetricKey.Mode = CipherMode.CBC;
					using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
					{
						using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
						{
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
							{
								byte[] plainTextBytes = new byte[cipherTextBytes.Length];
								int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
								return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
							}
						}
					}
				}
			}
		}




		/// <summary>
		/// Encripta un texto plano de acuerdo con las claves de parámetros
		/// </summary>
		/// <param name="texto">texto para encriptar</param>
		/// <param name="key">clave</param>
		/// <returns>texto encriptado</returns>
		public static string Encryption(string texto, string key)
		{
			byte[] bytes_cifrar = UTF8Encoding.UTF8.GetBytes(texto);

			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

			tdes.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			// transforma la cadena
			ICryptoTransform cTransform = tdes.CreateEncryptor();

			// arreglo de bytes donde se guarda la cadena cifrada
			byte[] resultado = cTransform.TransformFinalBlock(bytes_cifrar, 0, bytes_cifrar.Length);

			hashmd5.Clear();
			tdes.Clear();

			// retorna el resultado en forma de una cadena
			string texto_cifrado = Convert.ToBase64String(resultado, 0, resultado.Length).Replace("/", "()").Replace("+", "(.)");

			return texto_cifrado;
		}

		/// <summary>
		/// Desencripta un texto plano de acuerdo con las claves de parámetros
		/// </summary>
		/// <param name="texto">texto encriptado</param>
		/// <param name="key">clave</param>
		/// <returns></returns>
		public static string Decryption(string texto, string key)
		{
			texto = texto.Replace("()", "/").Replace("(.)", "+");

			// convierte el texto en una secuencia de bytes
			byte[] bytes_descifrar = Convert.FromBase64String(texto);

			// encriptación hashing algoritmo MD5
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateDecryptor();

			byte[] resultArray = cTransform.TransformFinalBlock(bytes_descifrar, 0, bytes_descifrar.Length);

			hashmd5.Clear();
			tdes.Clear();

			// retorna la cadena
			return UTF8Encoding.UTF8.GetString(resultArray);
		}


	}
}
