using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
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
	/// https://docs.microsoft.com/es-mx/azure/storage/blobs/storage-dotnet-how-to-use-blobs
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
		public BlobController(string conexion_azure, string contenedor)
		{
			// Enable TLS 1.2 before connecting to Azure Storage
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

			this.DatosCuenta = CloudStorageAccount.Parse(conexion_azure);

			this.ClienteBlob = this.DatosCuenta.CreateCloudBlobClient();

			if (!string.IsNullOrEmpty(contenedor))
				this.ContenedorBlob = this.ClienteBlob.GetContainerReference(contenedor);

			//this.OpcionesBlob = new BlobRequestOptions()
			//{
			//	ServerTimeout = TimeSpan.FromMinutes(timeout_minutos),	
			//};
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
		/// Asignar propiedades al blob como: ContentType, Metadata
		/// </summary>
		/// <param name="blockBlobReference">datos del blob</param>
		/// <param name="extension">extensión del archivo blob</param>
		private void SetBlobPropiedades(CloudBlockBlob blockBlobReference, string extension, Dictionary<string, string> metadata)
		{
			// asigna el tipo de contenido al archivo
			if (extension != string.Empty)
			{   // obtiene el tipo de contenido
				string contentType = WebHtml.TipoContenido(extension.ToLower());

				if (contentType != string.Empty)
					blockBlobReference.Properties.ContentType = contentType;
			}

			// asigna la metadata al archivo
			foreach (var meta in metadata)
			{
				blockBlobReference.Metadata.Add(meta.Key, meta.Value);
			}
		}

		/// <summary>
		/// Crea el contenedor con el nombre y permisos de acceso
		/// </summary>
		/// <param name="nombre">nombre del contenedor</param>
		/// <param name="acceso">permisos de acceso</param>
		/// <param name="metadata">metadata del contenedor</param>
		/// <returns>indica si creó el contenedor, si existe devuelve true</returns>
		public bool CrearContenedor(string nombre, TipoAccesoEnum acceso, Dictionary<string, string> metadata)
		{
			bool creo = false;

			// valida el nombre del contenedor
			NameValidator.ValidateContainerName(nombre);

			// validar existencia del contenedor
			if (!this.ContenedorBlob.Exists())
			{
				// asigna la metadata al contenedor
				if (metadata != null)
				{
					foreach (var meta in metadata)
					{
						this.ContenedorBlob.Metadata.Add(meta.Key, meta.Value);
					}
				}

				// crea el contenedor
				this.ContenedorBlob.Create();

				BlobContainerPublicAccessType tipo_permiso = BlobContainerPublicAccessType.Unknown;

				if (acceso == TipoAccesoEnum.Blob)
					tipo_permiso = BlobContainerPublicAccessType.Blob;
				else if (acceso == TipoAccesoEnum.Contenedor)
					tipo_permiso = BlobContainerPublicAccessType.Container;
				else if (acceso == TipoAccesoEnum.SinAccesoPublico)
					tipo_permiso = BlobContainerPublicAccessType.Off;

				// asigna el permiso de lectura al contenedor
				this.ContenedorBlob.SetPermissions(new BlobContainerPermissions()
				{
					PublicAccess = tipo_permiso
				});

				creo = true;
			}
			else
				creo = true;

			return creo;
		}

		/// <summary>
		/// Carga un archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="source">fuente de datos</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="metadata">metadata del blob</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(Stream source, string extension, Dictionary<string, string> metadata, string nombre = null)
		{
			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				nombre = Guid.NewGuid().ToString();

			// valida el nombre del blob
			NameValidator.ValidateBlobName(nombre);

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", (object)nombre, (object)extension));

			if (!blockBlobReference.Exists())
			{
				// asignar propiedades al blob
				this.SetBlobPropiedades(blockBlobReference, extension, metadata);

				// carga el archivo con la información en el contenedor del blob
				blockBlobReference.UploadFromStream(source);
				blockBlobReference.SetStandardBlobTierAsync(StandardBlobTier.Cool);
			}

			// retorna la url del archivo
			return blockBlobReference.Uri.ToString();
		}


		/// <summary>
		/// Carga un texto como archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="source">texto</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="metadata">metadata del blob</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(string source, string extension, Dictionary<string, string> metadata, string nombre = null)
		{
			using (System.IO.MemoryStream writer = new System.IO.MemoryStream())
			{
				using (System.IO.StreamWriter swriter = new System.IO.StreamWriter(writer))
				{
					swriter.Write(source);
					swriter.Flush();
					writer.Position = 0;

					// retorna la url del archivo
					return this.Enviar(swriter.BaseStream, extension, metadata, nombre);
				}
			}
		}

		/// <summary>
		/// Carga datos binarios como archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="source">datos binarios</param>
		/// <param name="extension">extensión del archivo</param>
		/// <param name="metadata">metadata del blob</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(byte[] source, string extension, Dictionary<string, string> metadata, string nombre = null)
		{
			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				nombre = Guid.NewGuid().ToString();

			// valida el nombre del blob
			NameValidator.ValidateBlobName(nombre);

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", (object)nombre, (object)extension));

			if (!blockBlobReference.Exists())
			{
				// asignar propiedades al blob
				this.SetBlobPropiedades(blockBlobReference, extension, metadata);

				byte[] buffer = source;

				int length = source.Length;

				int index = 0;

				// carga el archivo con la información en el contenedor del blob
				blockBlobReference.UploadFromByteArray(buffer, index, length, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
				blockBlobReference.SetStandardBlobTierAsync(StandardBlobTier.Cool);
			}

			// retorna la url del archivo
			return blockBlobReference.Uri.ToString();
		}

		/// <summary>
		/// Carga un archivo local como archivo al blob de Microsoft Azure
		/// </summary>
		/// <param name="archivo_local">datos binarios</param>
		/// <param name="metadata">metadata del blob</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns>url del archivo en el blob</returns>
		public string Enviar(string archivo_local, Dictionary<string, string> metadata, string nombre = null)
		{
			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				nombre = Guid.NewGuid().ToString();

			// valida el nombre del blob
			NameValidator.ValidateBlobName(nombre);

			string extension = Path.GetExtension(archivo_local);

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", nombre, extension));

			if (!blockBlobReference.Exists())
			{
				// asignar propiedades al blob
				this.SetBlobPropiedades(blockBlobReference, extension, metadata);

				string path = archivo_local;

				// carga el archivo con la información en el contenedor del blob
				blockBlobReference.UploadFromFile(path, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
				blockBlobReference.SetStandardBlobTierAsync(StandardBlobTier.Cool);
			}

			// retorna la url del archivo
			return blockBlobReference.Uri.ToString();
		}

		/// <summary>
		/// Elimina el archivo del blob de Microsoft Azure 
		/// </summary>
		/// <param name="nombre">nombre del archivo</param>
		/// <param name="instantaneas">indica si sólo elimina instantáneas</param>
		public void Eliminar(string nombre, bool instantaneas = false)
		{

			// si no envía nombre se asigna uno
			if (string.IsNullOrEmpty(nombre))
				throw new ApplicationException("Indique un nombre válido.");

			// valida el nombre del blob
			NameValidator.ValidateBlobName(nombre);

			// obtiene la referencia al archivo
			CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(nombre);

			// elimina el archivo del contenedor del blob
			if (instantaneas)
				blockBlobReference.Delete(DeleteSnapshotsOption.DeleteSnapshotsOnly);
			else
				blockBlobReference.Delete(DeleteSnapshotsOption.IncludeSnapshots);
		}

		/// <summary>
		/// Descarga un Blob tipo texto y lo retorna en string para serializar
		/// </summary>
		/// <param name="extension">debe ser .xml</param>
		/// <param name="nombre">nombre del archivo</param>
		/// <returns></returns>
		public string LecturaBlob(string extension, string nombre)
		{
			string lectura = string.Empty;

			try
			{
				// valida el nombre del blob
				NameValidator.ValidateBlobName(nombre);

				// obtiene la referencia al archivo
				CloudBlockBlob blockBlobReference = this.ContenedorBlob.GetBlockBlobReference(string.Format("{0}{1}", (object)nombre, (object)extension));


				if (!blockBlobReference.Exists())
				{
					lectura = blockBlobReference.DownloadText();
				}

			}
			catch (Exception exception)
			{
				RegistroLog.EscribirLog(exception, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.consulta);
			}

			return lectura;

		}
	}
}