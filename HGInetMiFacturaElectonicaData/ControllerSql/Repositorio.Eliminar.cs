using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Properties;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	/// <summary>
	/// Repositorio.Eliminar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{

		/// <summary>
		/// elimina un objeto
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Delete)]
		public virtual void Delete(T entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "entity", typeof(T)));

				Type t = typeof(T);
				PropertyInfo[] p = t.GetPropertiesInfo(this.context.FindPrimaryKey<T>());

				object[] values = p.Select(p_ => p_.GetValue(entity, null)).ToArray();

				T attachedEntity = this.context.Set<T>().Find(values);
				if (attachedEntity != null)
				{
					this.Validate(attachedEntity, Accion.eliminacion);
					this.OnDeleting(entity, attachedEntity);

					this.context.Set<T>().Remove(attachedEntity);
					this.context.SaveChanges();
					this.OnDeleted(entity);
				}
				else
					throw new InvalidOperationException(string.Format(RecursoMensajes.DeleteNoExistingEntityError, t.Name));
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// elimina un grupo de elemento
		/// </summary>
		/// <param name="entities"></param>
		[DataObjectMethod(DataObjectMethodType.Delete)]
		public virtual void Delete(IQueryable<T> entities)
		{
			try
			{
				if (entities == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "entities", typeof(T)));

				foreach (T entity in entities.ToList())
					this.context.Set<T>().Remove(entity);

				this.context.SaveChanges();
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// elimina los objetos de una tabla
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Delete)]
		public virtual void DeleteAll(string table_name)
		{
			try
			{
				this.context.Database.ExecuteSqlCommand(string.Format("TRUNCATE TABLE {0}", table_name));
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// evento antes de agregar el objeto
		/// </summary>
		public virtual void OnDeleting(T entity, T attachedEntity)
		{
		}

		/// <summary>
		/// evento despues de agregar el objeto
		/// </summary>
		public virtual void OnDeleted(T entity)
		{
		}

	}
}
