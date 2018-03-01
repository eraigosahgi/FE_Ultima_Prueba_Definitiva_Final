using LibreriaGlobalHGInet.General;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUtilidadAzure.Almacenamiento
{
	/// <summary>
	/// Controlador de Blobs
	/// https://azure.microsoft.com/es-es/services/storage/blobs/
	/// </summary>
	public class BlobController
	{

		/// <summary>
		/// Datos de autenticación de Microsoft Azure
		/// </summary>
		private CloudStorageAccount DatosCuenta = null;

		/// <summary>
		/// Cliente para la conexión de blobs con Microsoft Azure
		/// </summary>
		private CloudBlobClient ClienteBlob = null;

		/// <summary>
		/// Contenedor de blobs para Microsoft Azure
		/// </summary>
		private CloudBlobContainer ContenedorBlob = null;

		/// <summary>
		/// Opciones para la petición de blobs sobre Microsoft Azure
		/// </summary>
		private BlobRequestOptions OpcionesBlob = null;

		/// <summary>
		/// Constructor del controlador
		/// </summary>
		/// <param name="conexion_azure">string de conexión</param>
		/// <param name="contenedor">nombre del contenedor</param>
		/// <param name="timeout_minutos">indica el tiempo en minutos para el timeout</param>
		public BlobController(string conexion_azure, string contenedor, int timeout_minutos)
		{
			this.DatosCuenta = CloudStorageAccount.Parse(conexion_azure);
			
			this.ClienteBlob = this.DatosCuenta.CreateCloudBlobClient();

			this.ContenedorBlob = this.ClienteBlob.GetContainerReference(contenedor);

			this.OpcionesBlob = new BlobRequestOptions()
			{
				ServerTimeout = TimeSpan.FromMinutes(timeout_minutos),	
			};
		}




		public void Set()
		{
			ServiceProperties serviceProperties = this.ClienteBlob.GetServiceProperties((BlobRequestOptions)null, (OperationContext)null);
			
			serviceProperties.Cors = new CorsProperties();
			
			IList<CorsRule> corsRules = serviceProperties.Cors.CorsRules;
			
			CorsRule corsRule = new CorsRule();
			corsRule.AllowedHeaders = (IList<string>)new List<string>() { "*" };
			corsRule.ExposedHeaders = (IList<string>)new List<string>() { "*" };
			int num1 = 29;
			corsRule.AllowedMethods = (CorsHttpMethods)num1;
			corsRule.AllowedOrigins = (IList<string>)new List<string>() { "*" };
			int num2 = 3600;
			corsRule.MaxAgeInSeconds = num2;
			corsRules.Add(corsRule);
			
			this.ClienteBlob.SetServiceProperties(serviceProperties, (BlobRequestOptions)null, (OperationContext)null);
		}



		/// <summary>
		/// Crea el contenedor con el nombre
		/// </summary>
		/// <param name="nombre">nombre del contenedor</param>
		/// <returns>indica si creó el contenedor, si existe devuelve true</returns>
		public bool CrearContenedor(string nombre)
		{
			bool creo = false;


			if (!this.ContenedorBlob.Exists())
				this.ContenedorBlob.Create(this.OpcionesBlob);
			else
				creo = true;

			return creo;
		}

		/// <summary>
		/// Carga un archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="source">fuente de datos</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(Stream source, string extension, string nombre = null)
		{
			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				nombre = Guid.NewGuid().ToString();

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", (object)nombre, (object)extension));
			
			// obtiene el tipo de contenido
			string contentType = WebHtml.TipoContenido(extension.ToLower());
			
			// asigna el tipo de contenido al archivo
			if (contentType != string.Empty)
				blockBlobReference.Properties.ContentType = contentType;
			
			// carga el archivo con la información en el contenedor del blob
			blockBlobReference.UploadFromStream(source, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
			
			// retorna la url del archivo
			return blockBlobReference.Uri.ToString();
		}


		/// <summary>
		/// Carga un texto como archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="source">texto</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(string source, string extension, string nombre = null)
		{
			using (System.IO.MemoryStream writer = new System.IO.MemoryStream())
			{
				using (System.IO.StreamWriter swriter = new System.IO.StreamWriter(writer))
				{
					swriter.Write(source);
					swriter.Flush();
					writer.Position = 0;
			
					// retorna la url del archivo
					return this.Enviar(swriter.BaseStream, extension, nombre);
				}
			}
		}

		/// <summary>
		/// Carga datos binarios como archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="source">datos binarios</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(byte[] source, string extension, string nombre = null)
		{
			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				nombre = Guid.NewGuid().ToString();

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", (object)nombre, (object)extension));

			// obtiene el tipo de contenido
			string contentType = WebHtml.TipoContenido(extension.ToLower());

			// asigna el tipo de contenido al archivo
			if (contentType != string.Empty)
				blockBlobReference.Properties.ContentType = contentType;

			byte[] buffer = source;
			
			int length = source.Length;

			int index = 0;
			
			// carga el archivo con la información en el contenedor del blob
			blockBlobReference.UploadFromByteArray(buffer, index, length, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
			
			// retorna la url del archivo
			return blockBlobReference.Uri.ToString();
		}

		/// <summary>
		/// Carga un archivo local como archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="archivo_local">datos binarios</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(string archivo_local, string nombre = null)
		{
			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				nombre = Guid.NewGuid().ToString();

			string extension = Path.GetExtension(archivo_local);

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", nombre, extension));


			// obtiene el tipo de contenido
			string contentType = WebHtml.TipoContenido(extension.ToLower());

			// asigna el tipo de contenido al archivo
			if (contentType != string.Empty)
				blockBlobReference.Properties.ContentType = contentType;

			string path = archivo_local;

			// carga el archivo con la información en el contenedor del blob
			blockBlobReference.UploadFromFile(path, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);

			// retorna la url del archivo
			return blockBlobReference.Uri.ToString();
		}




	}
}
