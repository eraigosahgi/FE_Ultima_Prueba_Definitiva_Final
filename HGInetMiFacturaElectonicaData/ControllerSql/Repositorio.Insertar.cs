using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Properties;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	/// <summary>
	/// Repositorio.Insertar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{
		/// <summary>
		/// Inserta un registro en la base de datos
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Insert)]
		public virtual T Add(T entity)
		{
			try
			{
				this.context.Configuration.LazyLoadingEnabled = false;
				if (entity == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "entity", typeof(T)));

				this.Validate(entity, Accion.creacion);
				this.OnAdding(entity);

				this.context.Set<T>().Add(entity);

				this.context.SaveChanges();
				this.OnAdded(entity);

				return entity;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message, exec.InnerException);
			}
		}

        /// <summary>
		/// Actualiza un registro en la base de datos
		/// </summary>
		[DataObjectMethod(DataObjectMethodType.Insert)]
        public virtual T Update(T entity)
        {
            try
            {
                this.context.Configuration.LazyLoadingEnabled = false;
                if (entity == null)
                    throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "entity", typeof(T)));

                this.Validate(entity, Accion.actualizacion);
                this.OnEditing(entity);

                this.context.SaveChanges();
                this.OnEdited(entity);

                return entity;
            }
            catch (Exception exec)
            {
                throw new ApplicationException(exec.Message, exec.InnerException);
            }
        }

        /// <summary>
		/// evento despues de editar el objeto
		/// </summary>
        public virtual void OnEdited(T entity)
        {
        }

        /// <summary>
        /// evento antes de editar el objeto
        /// </summary>
        public virtual void OnEditing(T entity)
        {
        
        }

        /// <summary>
        /// evento antes de agregar el objeto
        /// </summary>
        public virtual void OnAdding(T entity)
		{
		}

		/// <summary>
		/// evento despues de agregar el objeto
		/// </summary>
		public virtual void OnAdded(T entity)
		{
		}

	}
}
