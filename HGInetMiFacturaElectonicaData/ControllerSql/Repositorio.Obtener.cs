using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{

		/// <summary>
		/// obtiene todos los registros
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public virtual IQueryable<T> List(params Expression<Func<T, object>>[] includes)
		{
			try
			{
				return this.context.Set<T>().IncludeMultiple(includes);
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// obtiene todos los registros ordenados por la expresión
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Select)]
		public virtual IQueryable<T> List(string sort_expression, params Expression<Func<T, object>>[] includes)
		{
			try
			{
				return List(includes).OrderBy(sort_expression);
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

	}
}
