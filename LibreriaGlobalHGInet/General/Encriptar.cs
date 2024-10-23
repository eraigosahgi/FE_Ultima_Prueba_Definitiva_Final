using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LibreriaGlobalHGInet.General
{
    public class Encriptar
    {
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

        /// <summary>
        /// Encripta un string usando SHA256 
        /// </summary>
        /// <param name="texto">Texto que se va a encriptar en SHA256</param>
        /// <returns>Texto encriptado como string hexadecimal</returns>
        public static string Encriptar_SHA256(string texto)
        {
            SHA256 sha256 = SHA256Managed.Create();            
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(texto));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Encripta un string usando SHA384
        /// </summary>
        /// <param name="texto">Texto que se va a encriptar en SHA384 </param>
        /// <returns>texto encriptado como string hexadecimal</returns>
        public static string Encriptar_SHA384(string texto)
        {
            SHA384 sha1 = SHA384Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(texto));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        
        /// <summary>
        /// encripta un string implementando MD5
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string Encriptar_MD5(string texto)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();

            //convierte a bytes
            ASCIIEncoding codificar = new ASCIIEncoding();

            byte[] hashData = null;

            //Crear una nueva instancia de StringBuilder para guardar datos hash
            StringBuilder texto_encriptado = new StringBuilder();

            hashData = md5.ComputeHash(codificar.GetBytes(texto));

            for (int i = 0; i < hashData.Length; i++) texto_encriptado.AppendFormat("{0:x2}", hashData[i]);

            return texto_encriptado.ToString();
        }



        
        /// <summary>
        /// Convierte un archivo en sha256
        /// </summary>
        /// <param name="Archivo">Se debe enviar la ruta con el nombre del archivo a convertir</param>
        /// <returns></returns>
        public static string  Archivo_Sha256(string Archivo)
        {            
            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            var fileStream = new FileStream(Archivo, FileMode.OpenOrCreate,
                FileAccess.Read);
            string str2 = GetChecksumBuffered(fileStream);

            return str2;
        }

        private static string GetChecksumBuffered(Stream stream)
        {
            using (var bufferedStream = new BufferedStream(stream, 1024 * 32))
            {
                var sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(bufferedStream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }



        /// <summary>
        /// Encripta un string usando SHA512
        /// </summary>
        /// <param name="texto">Texto que se va a encriptar en SHA256</param>
        /// <returns>Texto encriptado como string hexadecimal</returns>
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

    }
}
