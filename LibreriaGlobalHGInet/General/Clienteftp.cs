using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace LibreriaGlobalHGInet.General
{
	public class Clienteftp
	{
		/// <summary>
		/// Descarga archivo de una ruta FTP
		/// </summary>
		/// <param name="RutaFTPOrigen">Ruta FTP indicando archivo con extensión</param>
		/// <param name="UsuarioFTP">Usuario FTP</param>
		/// <param name="ClaveFTP">Clave FTP</param>
		/// <param name="RutaDestino">Ruta fisica donde se descargara el archivo</param>
		/// <returns></returns>
		public static bool DescargarFtp(string RutaFTPOrigen, string UsuarioFTP, string ClaveFTP, string RutaDestino)
		{

			//Codigo para descargar archivo de un cliente FTP
			FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(RutaFTPOrigen);
			ftpRequest.Credentials = new NetworkCredential(UsuarioFTP, ClaveFTP);
			ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
			FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
			Stream stream = null;
			StreamReader reader = null;
			StreamWriter writer = null;
			try
			{
				stream = ftpResponse.GetResponseStream();

				reader = new StreamReader(stream, Encoding.Default);
				writer = new StreamWriter(RutaDestino, false, Encoding.Default);

				writer.Write(reader.ReadToEnd());
				return true;
			}
			finally
			{
				stream.Close();
				reader.Close();
				writer.Close();
			}
		}


		/// <summary>
		/// Sube un archivo de una ruta fisica del servidor a una ruta FTP
		/// </summary>
		/// <param name="RutaFTPOrigen">Ruta FTP indicando archivo con extensión</param>
		/// <param name="UsuarioFTP">Usuario FTP</param>
		/// <param name="ClaveFTP">Clave FTP</param>
		/// <param name="RutaDestino">Ruta fisica donde se descargara el archivo</param>
		/// <returns></returns>
		public static bool SubirArchivoFTP(string RutaDestino, string UsuarioFTP, string ClaveFTP, string RutaFTPOrigen)
		{

			FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(RutaDestino);
			request.Method = WebRequestMethods.Ftp.UploadFile;
			request.Credentials = new NetworkCredential(UsuarioFTP, ClaveFTP);
			request.UsePassive = true;
			request.UseBinary = true;
			request.KeepAlive = true;
			FileStream stream = File.OpenRead(RutaFTPOrigen);
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			stream.Close();
			Stream reqStream = request.GetRequestStream();
			reqStream.Write(buffer, 0, buffer.Length);
			reqStream.Flush();
			reqStream.Close();

			return true;
		}





		/// <summary>
		/// Sube un archivo a un Sftp
		/// </summary>
		/// <param name="Sftp">Servidor Sftp</param>
		/// <param name="Usuario">Usuario</param>
		/// <param name="Password">Password</param>
		/// <param name="Ruta_Archivo">Ruta con nombre del archivo donde se va a ubicar el archivo que se desea enviar</param>
		/// <param name="Nombre_archivo">Nombre que se le va a otorgar al archivo una vez este en el servidor Sftp</param>
		public static bool SubirArchivoSftp(string Sftp, string Usuario, string Password, string Ruta_Archivo, string Nombre_archivo)
		{


			string pathRemoteFile = Nombre_archivo;

			string pathLocalFile = Ruta_Archivo;

			string host = Sftp;
			string username = Usuario;
			string password = Password;
			int puerto = 22;
			string carpeta = string.Empty;

			var sftp_puerto = host.Split(':');

			if (sftp_puerto.Length > 1)
			{
				host = sftp_puerto[0].ToString();
				puerto = Convert.ToInt32(sftp_puerto[1]);
				if (sftp_puerto.Length > 2)
					carpeta = sftp_puerto[2];
			}

			using (SftpClient sftp = new SftpClient(host, puerto, username, password))
			{
				sftp.Connect();
				Stream fileStream = File.OpenRead(pathLocalFile);
				sftp.UploadFile(fileStream, string.Format("{0}{1}", carpeta, pathRemoteFile));
				sftp.Disconnect();
			}
			return true;
		}



		/// <summary>
		/// Sube un archivo a un Sftp
		/// </summary>
		/// <param name="Sftp">Servidor Sftp</param>
		/// <param name="Usuario">Usuario</param>
		/// <param name="Password">Password</param>        
		public static bool ValidarSftp(string Sftp, string Usuario, string Password)
		{
			try
			{
				string host = Sftp;
				string username = Usuario;
				string password = Password;
				int puerto = 22;
				string carpeta = string.Empty;

				var sftp_puerto = host.Split(':');

				if (sftp_puerto.Length > 1)
				{
					host = sftp_puerto[0].ToString();
					puerto = Convert.ToInt32(sftp_puerto[1]);
					if (sftp_puerto.Length > 2)
						carpeta = sftp_puerto[2];
				}

				using (SftpClient sftp = new SftpClient(host, puerto, username, password))
				{
					sftp.Connect();
					sftp.Disconnect();
				}
				return true;
			}
			catch (Exception ex)
			{

				return false;
			}

		}

		
		/// <summary>
		/// Sube archivo a un servidor ssh con archivo de claves privadas .ppk ssh o con clave tradicional
		/// </summary>
		/// <param name="Sftp">Ruta del servidor, si tiene un puerto direfente al 22, debe incluirlo en la ruta sftp ejemplo: sftp.ejemplo.com:3536</param>
		/// <param name="Usuario">Usuario sftp</param>		
		/// <param name="RutaOrigen">Ruta del archivo que se desea subir al servidor(debe incluir nombre y extensión del archivo)</param>
		/// <param name="RutaDestino">Ruta del servidor donde se desea subir el archivo (debe incluir nombre y extensión del archivo)</param>
		/// /// <param name="RutaKeyFile">Ruta del archivo ppk ssh</param>
		/// <param name="Clave"></param>
		/// <returns></returns>
		public static bool SubirArchivoSftp(string Sftp, string Usuario, string RutaOrigen, string RutaDestino,string Clave = "", string RutaKeyFile = "")
		{
			string host = Sftp;
			string username = Usuario;
			string carpeta = string.Empty;
			try
			{									
				int puerto = 22;				
				var sftp_puerto = host.Split(':');
				if (sftp_puerto.Length > 1)
				{
					host = sftp_puerto[0].ToString();
					puerto = Convert.ToInt32(sftp_puerto[1]);
					if (sftp_puerto.Length > 2)
						carpeta = sftp_puerto[2];
				}


				if (!string.IsNullOrEmpty(Clave)) {
					SubirArchivoSftp(Sftp, Usuario, Clave, RutaOrigen, RutaDestino);
				} else {

					ConnectionInfo connectionInfo;
					using (var stream = new FileStream(RutaKeyFile, FileMode.Open, FileAccess.Read))
					{
						var privateKeyFile = new PrivateKeyFile(stream);
						AuthenticationMethod authenticationMethod =
							new PrivateKeyAuthenticationMethod(Usuario, privateKeyFile);

						connectionInfo = new ConnectionInfo(Sftp, Usuario, authenticationMethod);

						var con = new ConnectionInfo(host, puerto, Usuario, authenticationMethod);
						var client = new SftpClient(con);
						//Abrimos la conexión
						client.Connect();
						//Copiamos el archivo
						Stream fileStream = File.OpenRead(RutaOrigen);
						client.UploadFile(fileStream, RutaDestino);
						//Cierro la conexión
						client.Disconnect();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}

		}

		/// <summary>
		/// Metodo para crear el objeto ConnectionInfo que se debe anexar a la petición de conexion cuando se usa archivo de claves
		/// </summary>
		/// <param name="host"></param>
		/// <param name="username"></param>
		/// <param name="port"></param>
		/// <param name="publicKeyPath"></param>
		/// <returns></returns>
		public static ConnectionInfo getSftpConnection(string host, string username, int port, string publicKeyPath)
		{
			return new ConnectionInfo(host, port, username, privateKeyObject(username, publicKeyPath));
		}

		/// <summary>
		/// Objeto de autenticacion cuando se usa archivo de claves
		/// </summary>
		/// <param name="username"></param>
		/// <param name="publicKeyPath"></param>
		/// <returns></returns>
		private static AuthenticationMethod[] privateKeyObject(string username, string publicKeyPath)
		{
			PrivateKeyFile privateKeyFile = new PrivateKeyFile(publicKeyPath);
			PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod =
				 new PrivateKeyAuthenticationMethod(username, privateKeyFile);
			return new AuthenticationMethod[] { privateKeyAuthenticationMethod };
		}



	


	}
}
