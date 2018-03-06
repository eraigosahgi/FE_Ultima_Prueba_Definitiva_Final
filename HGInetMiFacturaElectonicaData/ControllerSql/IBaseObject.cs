using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	public interface IBaseObject<T> where T : class
	{
		IQueryable<T> List(params Expression<Func<T, object>>[] includes);

		IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		T FindFirstBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		T FindFirstBy(object id);

		T Add(T entity);

		T Edit(T entity);

		string ImportFromCvs(string path, string key_column, string external_key_column, char separator);

		void Delete(T entity);

		void Delete(IQueryable<T> entities);

		void DeleteAll(string table_name);

		void OnAdding(T entity);

		void OnDeleting(T entity, T attachedEntity);

		void OnEditing(T entity, T attachedEntity);

		void Validate(T entity, Accion accion);

		void OnAdded(T entity);

		void OnDeleted(T entity);

		void OnEdited(T entity, T attachedEntity);

		void OnImportingExternalProperty(T entity, string external_value);

		void Close();

		void Detach(T entity);
	}
}
