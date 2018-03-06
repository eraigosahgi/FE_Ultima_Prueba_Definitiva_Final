using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Properties;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	/// <summary>
	/// Repositorio.Buscar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{
		/// <summary>
		/// busca todos los objectos que cumplen la condicion, incluyento las tablas relacionada enviadas
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public virtual IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			try
			{
				if (predicate == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "predicate", "System.Linq.Expressions.Expression"));

				return this.context.Set<T>().Where(predicate).IncludeMultiple(includes);
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// busca un objecto que cumplen la condicion, incluyento las tablas relacionada enviadas
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public virtual T FindFirstBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			try
			{
				if (predicate == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "predicate", "System.Linq.Expressions.Expression"));

				T entity = this.context.Set<T>().IncludeMultiple(includes).FirstOrDefault(predicate);

				return entity;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// busca un objecto por su id
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public virtual T FindFirstBy(object id)
		{
			try
			{
				T entity = this.context.Set<T>().Find(id);
				this.context.Detach(entity);

				return entity;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}
	}
}
