using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace LibreriaGlobalHGInet.Funciones
{
	public static class Enumeracion
	{
		public static T ParseToEnum<T>(object value) where T : struct, IConvertible
		{
			Type type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException(string.Format("{0} debe ser una enumeración.", type.Name));

			if (Enum.IsDefined(type, value))
				return (T)Enum.Parse(type, value.ToString(), true);
			else
				return default(T);
		}

		public static string ParseFromEnum<T>(this T _enum) where T : struct, IConvertible
		{
			Type type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException(string.Format("{0} debe ser una enumeración.", type.Name));

			return Enum.GetName(type, _enum);
		}

		public static IEnumerable<T> GetAllItems<T>() where T : struct
		{
			Type type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException(string.Format("{0} debe ser una enumeración.", type.Name));

			foreach (object item in Enum.GetValues(type))
			{
				yield return (T)item;
			}
		}

		public static T GetEnumObjectByValue<T>(int valueId)
		{
			Type type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException(string.Format("{0} debe ser una enumeración.", type.Name));

			return (T)Enum.ToObject(typeof(T), valueId);
		}

		public static ListItem[] GetCharListItems<T>() where T : struct, IConvertible
		{
			List<ListItem> items = new List<ListItem>();

			Type type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException(string.Format("{0} debe ser una enumeración.", type.Name));

			foreach (string name in Enum.GetNames(type))
			{
				char value = (char)(int)(Enum.Parse(typeof(T), name));
				items.Add(new ListItem { Text = name, Value = value.ToString() });
			}

			return items.ToArray();
		}

		public static string GetDescription(object value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}

		public static string GetAmbiente(object value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			AmbientValueAttribute[] attributes =
				(AmbientValueAttribute[])fi.GetCustomAttributes(typeof(AmbientValueAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Value.ToString();
			else
				return value.ToString();
		}

		/// <summary>
		/// Obtiene una enumeración con la descripción
		/// </summary>
		/// <typeparam name="T">tipo de enumeración</typeparam>
		/// <param name="description">descripción</param>
		/// <returns>enumeración</returns>
		public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException(RecursoMensajes.ObjectNotExist, description);
        }

		public static T GetValueFromAmbiente<T>(string description)
		{
			var type = typeof(T);
			if (!type.IsEnum) throw new InvalidOperationException();
			foreach (var field in type.GetFields())
			{
				var attribute = Attribute.GetCustomAttribute(field,
					typeof(AmbientValueAttribute)) as AmbientValueAttribute;
				if (attribute != null)
				{
					if (attribute.Value.ToString() == description)
						return (T)field.GetValue(null);
				}
				else
				{
					if (field.Name == description)
						return (T)field.GetValue(null);
				}
			}
			throw new ArgumentException(RecursoMensajes.ObjectNotExist, description);
		}

	}
}
