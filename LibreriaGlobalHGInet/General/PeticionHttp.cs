using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LibreriaGlobalHGInet.General
{
	public class PeticionHttp : Stream
	{
		protected static string fileNameGlobal = null;
		protected string fileName = null;

		protected static object writeLock = null;
		protected Stream sinkStream;
		protected bool inDisk;
		protected bool isClosed;
		protected string id;
		protected bool isResponse;
		protected HttpContext context;


		public PeticionHttp(HttpContext Context, Stream Sink, string Id, string servicio)
		{

			string archivo = string.Empty;

			string carpeta_peticion = string.Empty;

			string filepath = string.Format(@"{0}\peticion\", Directorio.ObtenerDirectorioRaiz());

			//Crea la ruta para guardar la peticion segun el tipo de servicio
			try
			{
				if (!Directory.Exists(filepath))
					Directory.CreateDirectory(filepath);


				if (servicio.Contains("factura"))
				{
					carpeta_peticion = string.Format(@"{0}factura\", filepath);

					if (!Directory.Exists(carpeta_peticion))
					{
						Directory.CreateDirectory(carpeta_peticion);
					}

					archivo = carpeta_peticion + Id + ".txt";

					if (!File.Exists(archivo))
						File.Create(archivo).Dispose();

				}
				else if (servicio.Contains("notacredito"))
				{
					carpeta_peticion = string.Format(@"{0}notacredito\", filepath);

					if (!Directory.Exists(carpeta_peticion))
					{
						Directory.CreateDirectory(carpeta_peticion);
					}

					archivo = carpeta_peticion + Id + ".txt";

					if (!File.Exists(archivo))
						File.Create(archivo).Dispose();

				}
				else if (servicio.Contains("notadebito"))
				{
					carpeta_peticion = string.Format(@"{0}notadebito\", filepath);

					if (!Directory.Exists(carpeta_peticion))
					{
						Directory.CreateDirectory(carpeta_peticion);
					}

					archivo = carpeta_peticion + Id + ".txt";

					if (!File.Exists(archivo))
						File.Create(archivo).Dispose();

				}

				// One lock per file name
				if (String.IsNullOrWhiteSpace(fileNameGlobal) || fileNameGlobal.ToUpper() != fileNameGlobal.ToUpper() || fileNameGlobal != archivo)
				{
					fileNameGlobal = archivo;
					writeLock = new object();
				}
				context = Context;
				fileName = archivo;
				id = Id;
				sinkStream = Sink;
				inDisk = false;
				isClosed = false;
			}
			catch (Exception excepcion)
			{
			//	LogExcepcion.Guardar(new Exception(string.Format("Error al guardar la peticion {0}", excepcion), excepcion));
			}
		}

		public void SetFilter(bool IsResponse)
		{


			isResponse = IsResponse;
			id = (isResponse ? "Reponse " : "Request ") + id;

			//
			// For Request only read the incoming stream and log it as it will not be "filtered" for a WCF request
			//
			if (!IsResponse)
			{
				AppendToFile(String.Format("at {0} --------------------------------------------", DateTime.Now));
				AppendToFile(id);

				if (context.Request.InputStream.Length > 0)
				{
					context.Request.InputStream.Position = 0;
					byte[] rawBytes = new byte[context.Request.InputStream.Length];
					context.Request.InputStream.Read(rawBytes, 0, rawBytes.Length);
					context.Request.InputStream.Position = 0;

					AppendToFile(rawBytes);
				}
				else
				{
					AppendToFile("(no body)");
				}
			}

		}

		public void AppendToFile(string Text)
		{
			byte[] strArray = Encoding.UTF8.GetBytes(Text);
			AppendToFile(strArray);

		}

		public void AppendToFile(byte[] RawBytes)
		{
			bool myLock = System.Threading.Monitor.TryEnter(writeLock, 100);


			if (myLock)
			{
				try
				{

					using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
					{
						stream.Position = stream.Length;
						stream.Write(RawBytes, 0, RawBytes.Length);
						stream.WriteByte(13);
						stream.WriteByte(10);

					}

				}
				catch (Exception ex)
				{
					string str = string.Format("Unable to create log. Type: {0} Message: {1}\nStack:{2}", ex, ex.Message, ex.StackTrace);
					System.Diagnostics.Debug.WriteLine(str);
					System.Diagnostics.Debug.Flush();


				}
				finally
				{
					System.Threading.Monitor.Exit(writeLock);


				}
			}


		}


		public override bool CanRead
		{
			get { return sinkStream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return sinkStream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return sinkStream.CanWrite; }
		}

		public override long Length
		{
			get
			{
				return sinkStream.Length;
			}
		}

		public override long Position
		{
			get { return sinkStream.Position; }
			set { sinkStream.Position = value; }
		}

		//
		// For WCF this code will never be reached
		//
		public override int Read(byte[] buffer, int offset, int count)
		{
			int c = sinkStream.Read(buffer, offset, count);
			return c;
		}

		public override long Seek(long offset, System.IO.SeekOrigin direction)
		{
			return sinkStream.Seek(offset, direction);
		}

		public override void SetLength(long length)
		{
			sinkStream.SetLength(length);
		}

		public override void Close()
		{

			sinkStream.Close();
			isClosed = true;
		}

		public override void Flush()
		{

			sinkStream.Flush();
		}

		// For streamed responses (i.e. not buffered) there will be more than one Response (but the id will match the Request)
		public override void Write(byte[] buffer, int offset, int count)
		{
			sinkStream.Write(buffer, offset, count);
			AppendToFile(String.Format("at {0} --------------------------------------------", DateTime.Now));
			AppendToFile(id);
			AppendToFile(buffer);
		}

	}
}
