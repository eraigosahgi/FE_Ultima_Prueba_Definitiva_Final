using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Properties;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	public static class ExtType
	{
		/// <summary>
		/// obtiene informacion de la propiedad enviada como texto
		/// </summary>
		public static PropertyInfo GetPropertyInfo(this Type t, string key_property_name)
		{
			if (string.IsNullOrWhiteSpace(key_property_name))
				return null;

			PropertyInfo p = t.GetProperty(key_property_name);

			if (p != null)
				return p;
			else
				throw new ArgumentException(string.Format(RecursoMensajes.PropertyNullError, key_property_name, t));
		}

		/// <summary>
		/// obtiene informacion de la propiedad enviada como texto
		/// </summary>
		public static PropertyInfo GetPropertyInfo(this Type t, string key_property_name, BindingFlags binding_flags)
		{
			if (string.IsNullOrWhiteSpace(key_property_name))
				return null;

			PropertyInfo p = t.GetProperty(key_property_name, binding_flags);

			if (p != null)
				return p;
			else
				throw new ArgumentException(string.Format(RecursoMensajes.PropertyNullError, key_property_name, t));
		}

		/// <summary>
		/// obtiene informacion de las propiedaes enviadas como texto
		/// </summary>
		public static PropertyInfo[] GetPropertiesInfo(this Type t, IEnumerable<string> key_properties_names)
		{
			if (key_properties_names == null)
				return null;

			PropertyInfo[] p = t.GetProperties().Where(p_ => key_properties_names.Contains(p_.Name)).ToArray();

			if (p != null)
				return p;
			else
				throw new ArgumentException(string.Format(RecursoMensajes.PropertyNullError, key_properties_names, t));
		}


		/// <summary>
		/// determina si un tipo permite o no null
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsNullableType(this Type type)
		{
			if (type != null)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
					return true;
				else
					return false;
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "type", type));
		}

		/// <summary>
		/// obtiene el tipo base del null
		/// </summary>
		public static Type GetNullableBaseType(this Type nullable_type)
		{
			if (nullable_type != null)
			{
				if (nullable_type.IsGenericType && nullable_type.GetGenericTypeDefinition() == typeof(Nullable<>))
					return nullable_type.GetGenericArguments()[0];
				else
					return nullable_type;
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "nullable_type", nullable_type));
		}

		/// <summary>
		/// obtiene el tipo de datos null
		/// </summary>
		public static Type GetNullableType(this Type type)
		{
			if (type != null)
			{
				if (type.IsValueType && !type.IsNullableType())
					return typeof(Nullable<>).MakeGenericType(type);
				else
					return type;
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "type", type));
		}
	}
}