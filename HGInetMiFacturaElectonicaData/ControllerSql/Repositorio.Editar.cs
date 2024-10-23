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
	/// Repositorio.Editar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{

		/// <summary>
		/// edita un objeto
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Update)]
		public virtual T Edit(T entity)
		{
			try
			{
				if (entity == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "entity", typeof(T)));

				Type t = typeof(T);
				PropertyInfo[] keys = t.GetPropertiesInfo(this.context.FindPrimaryKey<T>());

				object[] values = keys.Select(p_ => p_.GetValue(entity, null)).ToArray();

				T attachedEntity = this.context.Set<T>().Find(values);
				if (attachedEntity != null)
				{
					this.Validate(entity, Accion.actualizacion);
					this.OnEditing(entity, attachedEntity);

					var attachedEntry = this.context.Entry(attachedEntity);
					attachedEntry.CurrentValues.SetValues(entity);

					this.context.SaveChanges();
					this.OnEdited(entity, attachedEntity);
				}
				else
					throw new InvalidOperationException(string.Format(RecursoMensajes.EntityEditNoExistingError, t.Name));

				return entity;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

		/// <summary>
		/// evento antes de editar el objeto
		/// </summary>
		public virtual void OnEditing(T entity, T attachedEntity)
		{
		}

		/// <summary>
		/// evento despues de editar el objeto
		/// </summary>
		public virtual void OnEdited(T entity, T attachedEntity)
		{
		}
	}
}
