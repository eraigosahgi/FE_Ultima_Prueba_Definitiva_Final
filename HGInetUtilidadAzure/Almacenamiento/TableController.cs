using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HGInetUtilidadAzure.Objetos;
using LibreriaGlobalHGInet.Funciones;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace HGInetUtilidadAzure.Almacenamiento
{

	/// <summary>
	/// Controlador de Tables
	/// https://azure.microsoft.com/es-es/services/storage/tables/
	/// https://docs.microsoft.com/es-es/azure/cosmos-db/table-storage-how-to-use-dotnet
	/// https://docs.microsoft.com/es-es/rest/api/storageservices/designing-a-scalable-partitioning-strategy-for-azure-table-storage
	/// https://docs.microsoft.com/es-es/azure/cosmos-db/table-storage-design-guide
	/// https://github.com/dtretyakov/WindowsAzure
	/// https://www.codeproject.com/Articles/576932/Windows-Azure-Storage-Extensions
	/// https://blog.gbellmann.technology/2015/02/26/haciendo-consultas-sobre-azure-table-storage/
	/// </summary>
	/// <typeparam name="T">objeto relacionado con la tabla, debe contener la propiedad TableNameAttribute</typeparam>
	public class TableController<T> where T : TableEntity, new()
	{

		/// <summary>
		/// Datos de autenticación de Microsoft Azure
		/// </summary>
		private CloudStorageAccount DatosCuenta = null;

		/// <summary>
		/// Cliente para la conexión de tables con Microsoft Azure
		/// </summary>
		private CloudTableClient ClienteTable = null;

		/// <summary>
		/// Tabla de datos de Microsoft Azure
		/// </summary>
		public CloudTable Tabla = null;


		TableRequestOptions opcionesTable = null;

		public TableController(string conexion_azure, int timeout_minutos, bool crear_tabla = false)
		{
			// datos de conexión con la cuenta de Microsoft Azure
			this.DatosCuenta = CloudStorageAccount.Parse(conexion_azure);

			// cliente para operar la tabla
			this.ClienteTable = this.DatosCuenta.CreateCloudTableClient();

			object entidad = ((IEnumerable<object>)typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true)).FirstOrDefault<object>();

			// valida que se encuentre la propiedad TableNameAttribute en la entidad
			if (entidad == null && !crear_tabla)
				throw new ArgumentException(string.Format("No se encuentra la propiedad TableNameAttribute en el objeto {0}"), typeof(T).Name);
			if (entidad == null && crear_tabla)
				this.Crear();
			else
				this.Tabla = this.ClienteTable.GetTableReference((entidad as TableNameAttribute).TableName);

			// opciones adicionales para ejecuciones de la tabla
			this.opcionesTable = new TableRequestOptions()
			{
				ServerTimeout = TimeSpan.FromMinutes(timeout_minutos),
			};
		}

		/// <summary>
		/// Crea la tabla con el nombre asignado al objeto
		/// Realiza la validación del nombre de la tabla para Microsoft Azure
		/// http://msdn.microsoft.com/en-us/library/windowsazure/dd179338.aspx
		/// https://blogs.msdn.microsoft.com/jmstall/2014/06/12/azure-storage-naming-rules/
		/// </summary>
		public void Crear()
		{
			string tabla_nombre = typeof(T).Name;

			// valida el nombre de la tabla
			// http://msdn.microsoft.com/en-us/library/windowsazure/dd179338.aspx
			// https://blogs.msdn.microsoft.com/jmstall/2014/06/12/azure-storage-naming-rules/
			NameValidator.ValidateTableName(tabla_nombre);

			object entidad = ((IEnumerable<object>)typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true)).FirstOrDefault<object>();

			// valida que se encuentre la propiedad TableNameAttribute en la entidad
			if (entidad != null)
				throw new ArgumentException(string.Format("No se encuentra la propiedad TableNameAttribute en el objeto {0}"), tabla_nombre);

			this.Tabla = this.ClienteTable.GetTableReference(tabla_nombre);

			if (!this.Tabla.Exists())
				this.Tabla.Create(this.opcionesTable);
		}

		/// <summary>
		/// Elimina la tabla con el nombre asignado al objeto
		/// </summary>
		public void Eliminar()
		{
			string tabla_nombre = typeof(T).Name;

			object entidad = ((IEnumerable<object>)typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true)).FirstOrDefault<object>();

			// valida que se encuentre la propiedad TableNameAttribute en la entidad
			if (entidad != null)
				throw new ArgumentException(string.Format("No se encuentra la propiedad TableNameAttribute en el objeto {0}"), tabla_nombre);

			// valida que la tabla exista para eliminarla
			if (!this.Tabla.Exists())
				this.Tabla.Delete(this.opcionesTable);
		}


		/// <summary>
		/// Valida las propiedades PartitionKey y RowKey de TableEntity para Microsoft Azure Table
		/// http://msdn.microsoft.com/en-us/library/windowsazure/dd179338.aspx
		/// https://blogs.msdn.microsoft.com/jmstall/2014/06/12/azure-storage-naming-rules/
		/// </summary>
		/// <param name="tableName">nombre de la tabla</param>
		private void ValidarKeys(T entidad)
		{
			// valida que la entidad no sea nula
			if (entidad == null)
				throw new InvalidOperationException(string.Format("El objeto de tipo {0} no puede ser nulo para Microsoft Azure Tables", typeof(T).Name));

			// valida el valor de la propiedad PartitionKey de TableEntity
			if (!Regex.IsMatch(entidad.PartitionKey, "^[A-Za-z][A-Za-z0-9-]{2,62}$"))
				throw new InvalidOperationException(string.Format("El valor {0} en la propiedad PartitionKey es inválido para Microsoft Azure Tables", entidad.PartitionKey));

			// valida que el valor de la propiedad PartitionKey no supere el límite de 1KB
			if (entidad.PartitionKey.Length >= 512)
				throw new InvalidOperationException(string.Format("El valor {0} en la propiedad PartitionKey supera 1KB para Microsoft Azure Tables", entidad.PartitionKey));

			// valida el valor de la propiedad RowKey de TableEntity
			if (!Regex.IsMatch(entidad.RowKey, "^[A-Za-z0-9-:.]{2,62}$"))
				throw new InvalidOperationException(string.Format("El valor {0} en la propiedad RowKey es inválido para Microsoft Azure Tables", entidad.RowKey));

			// valida que el valor de la propiedad RowKey no supere el límite de 1KB
			if (entidad.RowKey.Length >= 512)
				throw new InvalidOperationException(string.Format("El valor {0} en la propiedad RowKey supera 1KB para Microsoft Azure Tables", entidad.PartitionKey));
		}

		/// <summary>
		/// Almacena el registro (creando o reemplazando)
		/// </summary>
		/// <param name="entidad">objeto con datos</param>
		public void Guardar(T entidad)
		{
			// valida las propiedades PartitionKey y RowKey de TableEntity
			ValidarKeys(entidad);

			// inserta o reemplaza la entidad si existe
			TableOperation operacion = TableOperation.InsertOrReplace((ITableEntity)entidad);

			// ejecuta la operación
			this.Tabla.Execute(operacion, this.opcionesTable, (OperationContext)null);
		}

		/// <summary>
		/// Elimina un registro de la tabla
		/// </summary>
		/// <param name="entidad">objeto con datos</param>
		public void Borrar(T entidad)
		{
			// elimina la entidad
			TableOperation operacion = TableOperation.Delete((ITableEntity)entidad);

			// ejecuta la operación
			this.Tabla.Execute(operacion, this.opcionesTable, (OperationContext)null);
		}

		/// <summary>
		/// Elimina los registros de la tabla de acuerdo con el filtro indicado
		/// </summary>
		/// <param name="valor">valor de la propiedad</param>
		/// <param name="campo">tipo de propiedad (PartitionKey o RowKey)</param>
		public void BorrarPorClave(string valor, ClaveTableEnum campo)
		{
			// obtiene la propiedad de acuerdo con el campo indicado
			string propiedad = Enumeracion.GetDescription(campo);

			// construye el filtro
			string filtro = TableQuery.GenerateFilterCondition(propiedad, QueryComparisons.Equal, valor);

			// obtiene los registros de acuerdo con el filtro indicado
			var registros = this.ObtenerPorFiltro(filtro);

			// entidad para ejecutar varias operaciones
			var batchOperation = new TableBatchOperation();

			// número de registros para borrar
			var contador_registros = 0;

			foreach (var entity in registros)
			{
				// agrega la entidad para borrarla luego
				batchOperation.Delete(entity);
				contador_registros++;

				// Cuando alcanza 100 elementos se confirma y reinicia la operación
				if (contador_registros == 100)
				{
					// ejecuta la operación
					this.Tabla.ExecuteBatch(batchOperation);

					// reinicia la operación
					batchOperation = new TableBatchOperation();
					contador_registros = 0;
				}
			}
		}

		/// <summary>
		/// Obtiene todos los registros de una tabla
		/// </summary>
		/// <returns>registros encontrados</returns>
		public IEnumerable<T> Obtener()
		{
			return this.ObtenerPorFiltro(string.Empty);
		}

		/// <summary>
		/// Obtiene los registros de una tabla por una de las propiedades clave (PartitionKey o RowKey)
		/// </summary>
		/// <param name="valor">valor de la propiedad</param>
		/// <param name="campo">tipo de propiedad (PartitionKey o RowKey)</param>
		/// <returns>registros encontrados</returns>
		public IEnumerable<T> ObtenerPorClave(string valor, ClaveTableEnum campo)
		{
			// obtiene la propiedad de acuerdo con el campo indicado
			string propiedad = Enumeracion.GetDescription(campo);

			string filtro = TableQuery.GenerateFilterCondition(propiedad, QueryComparisons.Equal, valor);

			return this.ObtenerPorFiltro(filtro);
		}

		/// <summary>
		/// Obtiene un registro de una tabla por las propiedades clave PartitionKey y RowKey
		/// </summary>
		/// <param name="partitionKey">valor para el campo PartitionKey</param>
		/// <param name="rowKey">valor para el campo RowKey</param>
		/// <returns>registro encontrado</returns>
		public T ObtenerPorClaves(string partitionKey, string rowKey)
		{
			if (string.IsNullOrEmpty(partitionKey) && string.IsNullOrEmpty(rowKey))
			{
				// búsqueda por claves partition y row
				TableOperation operacion = TableOperation.Retrieve<T>(partitionKey, rowKey);

				// ejecuta la operación
				TableResult resultado = this.Tabla.Execute(operacion, this.opcionesTable);

				return (T)resultado.Result;
			}
			else
				throw new InvalidOperationException("Indique correctamente los valores de búsqueda.");
		}

		/// <summary>
		/// Obtiene los registros de una tabla por un filtro
		/// </summary>
		/// <param name="filtro">filtro para obtener registros</param>
		/// <returns>registros encontrados</returns>
		public IEnumerable<T> ObtenerPorFiltro(string filtro)
		{
			TableQuery<T> query = new TableQuery<T>();

			if (!string.IsNullOrEmpty(filtro))
				query.Where(filtro);

			return this.Tabla.ExecuteQuery<T>(query, this.opcionesTable, (OperationContext)null);
		}

	}
}
