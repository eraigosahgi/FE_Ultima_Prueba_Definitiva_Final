using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HGInetFacturaEServicios
{
	class Ctl_Utilidades
	{
	
		/// <summary>
		/// Valida la ruta del servicio web
		/// </summary>
		/// <param name="rutaUrl">ruta del servicio web remoto</param>
		/// <returns>ruta del servicio web</returns>
		public static string ValidarUrl(string rutaUrl)
		{
			if (string.IsNullOrEmpty(rutaUrl))
				throw new Exception("Ruta remota de servicios web no encontrada.");

			if (!rutaUrl[rutaUrl.Length - 1].Equals("/"))
				rutaUrl = string.Format("{0}/", rutaUrl);

			return rutaUrl;
		}

		/// <summary>
		/// Encripta un string usando SHA1 
		/// </summary>
		/// <param name="texto">Texto que se va a encriptar en SHA1</param>
		/// <returns>Texto encriptado como string hexadecimal</returns>
		public static string Encriptar_SHA1(string texto)
		{
			SHA1 sha1 = SHA1Managed.Create();
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] stream = null;
			StringBuilder sb = new StringBuilder();
			stream = sha1.ComputeHash(encoding.GetBytes(texto));
			for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
			return sb.ToString();
		}

		public static string Encriptar_SHA512(string texto)
		{
			SHA512 sha512 = SHA512Managed.Create();
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] stream = null;
			StringBuilder sb = new StringBuilder();
			stream = sha512.ComputeHash(encoding.GetBytes(texto));
			for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
			return sb.ToString();
		}

		public static System.ServiceModel.Channels.Binding ObtenerBinding(string url, string binding_name)
        {

            string http_tipo = url.ToLowerInvariant().Split(':')[0];

            if (http_tipo.Equals("https"))
            {
                return new System.ServiceModel.WSHttpBinding();

            }
            else
            {
                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();

                if (!string.IsNullOrEmpty(binding_name))
                    binding = new System.ServiceModel.BasicHttpBinding(binding_name);
                
                return binding;
            }
        }

	}
}
