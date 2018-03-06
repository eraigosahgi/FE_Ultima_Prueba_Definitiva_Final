using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	/// <summary>
	/// Repositorio.Validar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{

		/// <summary>
		/// Propiedades para validar
		/// </summary>
		protected virtual string[] properties_validate { get; private set; }

		/// <summary>
		/// evento para validar el objeto
		/// </summary>
		public virtual void Validate(T entity, Accion accion)
		{
			if (this.properties_validate != null)
			{
				Type t = typeof(T);
				PropertyInfo[] p = t.GetPropertiesInfo(this.properties_validate);

				foreach (var propery in p)
				{
					object value = propery.GetValue(entity, null);

					if (value != null)
						propery.SetValue(entity, value.ToString(), null);
				}
			}
		}

	}
}
