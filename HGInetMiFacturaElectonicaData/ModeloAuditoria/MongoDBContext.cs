using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.Funciones;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Modelo
{
	public abstract partial class MongoDBContext<T> : IDatabase<T> where T : class
	{
		/// <summary>
		/// Nombre de la colección-tabla.
		/// </summary>
		private string collectionName = string.Empty;

		public IMongoDatabase db;

		public DbSet<TblAuditDocumentos> TblAuditDocumentos { set; get; }

		public DbSet<TblSeguimientoAlertas> TblSeguimientoAlertas { set; get; }

		/// <summary>
		/// Establece la conexión con el modelo de datos, realiza la asignación de la base de datos y establece el nombre de la colección a gestionar.
		/// </summary>
		/// <param name="auth"></param>
		public MongoDBContext(ModeloAutenticacion auth)
		{
			try
			{
				string conexion = ModeloConexion.GetConnectionString(auth);

				var cliente = new MongoClient(conexion);

				db = cliente.GetDatabase(auth.Basedatos);

				collectionName = typeof(T).Name;
			}
			catch (Exception exce)
			{
				throw;
			}
		}

		/// <summary>
		/// Devuelve una colección o un objeto de vista.
		/// </summary>
		protected IMongoCollection<T> Collection
		{

			get
			{
				return db.GetCollection<T>(collectionName);
			}
			set
			{
				Collection = value;
			}
		}

		/// <summary>
		/// Obtiene una colección desde la base de datos.
		/// </summary>
		public IMongoQueryable<T> GetAll
		{
			get
			{
				return Collection.AsQueryable<T>();
			}
			set
			{
				GetAll = value;
			}
		}

		/// <summary>
		/// Obtiene una colección desde la base de datos.
		/// </summary>
		public List<T> GetAllToList
		{
			get
			{
				return Collection.AsQueryable<T>().ToList<T>();
			}
		}

		/// <summary>
		/// Obtiene la colección de datos desde la base de datos, teniendo en cuenta filtros de búsqueda.
		/// </summary>
		/// <param name="expression">La expresión debe ser de tipo Lambda</param>
		/// <returns></returns>
		public List<T> GetFilter(Expression<Func<T, bool>> expression)
		{
			return Collection.Find(expression).ToList();
		}


		/// <summary>
		/// Obtiene la colección de datos desde la base de datos, teniendo en cuenta filtros de búsqueda.
		/// </summary>
		/// <param name="expression">La expresión debe ser de tipo Lambda</param>
		/// <returns></returns>
		public List<T> Obtener(Expression<Func<T, bool>> expression)
		{
			try
			{
				return Collection.Find(expression).ToList();
			}
			catch (Exception ex)
			{

				return null;
			}
		}

		/// <summary>
		/// Realiza la inserción de una colección de datos en la base de datos.
		/// </summary>
		/// <param name="items"></param>
		public void Insert(IEnumerable<T> items)
		{
			Collection.InsertManyAsync(items);
		}

		/// <summary>
		/// Realiza la inserción de un registro en la base de datos.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public Task Insert(T item)
		{
			return Collection.InsertOneAsync(item);
		}
		
	}

}
