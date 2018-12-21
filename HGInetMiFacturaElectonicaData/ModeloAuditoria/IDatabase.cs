using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Modelo
{
	public interface IDatabase<T> where T : class
	{
		IMongoQueryable<T> GetAll { get; set; }

		List<T> GetAllToList { get; }

		void Insert(IEnumerable<T> items);

		Task Insert(T item);
	}
}