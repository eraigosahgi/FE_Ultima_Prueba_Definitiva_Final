using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using Microsoft.VisualBasic.FileIO;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	/// <summary>
	/// Repositorio.Importar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{

		/// <summary>
		/// agrega o actualiza los objetos obtenidos de un archivo cvs
		/// </summary>
		public virtual string ImportFromCvs(string path, string key_column, string external_key_column, char separator)
		{
			Type t = typeof(T);
			PropertyInfo key_property = t.GetPropertyInfo(key_column);

			int inserted = 0, edited = 0, total = 0;

			if (key_property != null)
			{
				ParameterExpression e = Expression.Parameter(t, "e");
				MemberExpression m = Expression.MakeMemberAccess(e, key_property);
				ConstantExpression c;
				BinaryExpression b;
				Expression<Func<T, bool>> lambda;
				object value;

				PropertyInfo[] keys_properties = t.GetPropertiesInfo(this.context.FindPrimaryKey<T>());
				List<T> entities = this.GetExcelData(path, separator, external_key_column);

				foreach (T entity in entities)
				{
					this.Close();

					value = key_property.GetValue(entity, null);

					c = Expression.Constant(value, value.GetType());
					b = Expression.Equal(m, c);

					lambda = Expression.Lambda<Func<T, bool>>(b, e);

					T attachedEntity = FindFirstBy(lambda);

					if (attachedEntity != null)
					{
						foreach (var key in keys_properties)
						{
							key.SetValue(entity, key.GetValue(attachedEntity, null), null);
						}

						this.Validate(entity, Accion.actualizacion);
						this.OnEditing(entity, attachedEntity);

						var attachedEntry = this.context.Entry(attachedEntity);
						attachedEntry.CurrentValues.SetValues(entity);

						this.context.SaveChanges();
						this.OnEdited(entity, attachedEntity);

						edited++;
					}
					else
					{
						Add(entity);
						inserted++;
					}
				}
			}

			total = inserted + edited;

			return string.Format(RecursoMensajes.ImportResult, inserted, edited, total);
		}

		/// <summary>
		/// evento antes de importar una propiedad externa
		/// </summary>
		public virtual void OnImportingExternalProperty(T entity, string external_value)
		{
		}

		/// <summary>
		/// obtiene una lista de objetos con los datos del excel
		/// </summary>
		protected virtual List<T> GetExcelData(string path, char separator, string external_key_column)
		{
			List<T> entities = new List<T>();

			if (Archivo.ValidarExistencia(path))
			{
				Type t = typeof(T);
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(t);

				List<string[]> data_rows = GetExcelValues(path, separator);
				List<string> column_names = data_rows[0].ToList();
				data_rows.RemoveAt(0);

				T entity;
				PropertyInfo property;
				PropertyDescriptor prop;
				string[] row;
				string column_name = string.Empty;

				for (int j = 0; j < data_rows.Count; j++)
				{
					row = data_rows[j];

					entity = (T)Activator.CreateInstance(t);

					object value;
					string value_data = string.Empty;

					for (int i = 0; i < column_names.Count; i++)
					{
						column_name = column_names[i].Replace("\"", "").Trim();
						value_data = row[i].Replace("\"", "").Trim();

						property = t.GetProperty(column_name);

						if (property != null && !column_name.Equals(external_key_column))
						{
							prop = props[property.Name];

							try
							{
								value = GetExportValue(property, prop, column_name, j, value_data);
								property.SetValue(entity, value, null);
							}
							catch (Exception exec)
							{
								throw new InvalidCastException(string.Format(RecursoMensajes.CellArgumentError, value_data, j, column_name, prop.PropertyType), exec);
							}
						}
						else if (column_name.Equals(external_key_column))
							this.OnImportingExternalProperty(entity, value_data);
						else
							throw new ArgumentException(string.Format(RecursoMensajes.ColumnNullError, column_name, t));
					}
					entities.Add(entity);
				}
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.InvalidPath, path));

			return entities;
		}

		protected static List<string[]> GetExcelValues(string path, char separator)
		{
			List<string[]> data_rows = new List<string[]>();

			using (TextFieldParser field_parser = new TextFieldParser(path))
			{
				field_parser.TextFieldType = FieldType.Delimited;

				field_parser.Delimiters = new string[] { separator.ToString() };

				field_parser.HasFieldsEnclosedInQuotes = true;


				// Read and process the fields
				while (!field_parser.EndOfData)
				{
					// Parse the line just read into the array
					data_rows.Add(field_parser.ReadFields());
				}
			}
			return data_rows;
		}


		/// <summary>
		/// obtiene un valor de un string basado en la propiedad de un objeto
		/// </summary>
		protected static object GetExportValue(PropertyInfo property, PropertyDescriptor prop, string column_name, int row_index, string value_data)
		{
			object value;

			if (property.PropertyType == typeof(DateTime))
			{
				DateTime fecha = Fecha.GetFecha();

				if (DateTime.TryParseExact(value_data, Fecha.formato_fecha_hora_archivo, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
					value = fecha;
				else
					throw new ArgumentException(RecursoMensajes.ValueFormatError);
			}
			else if (property.PropertyType == typeof(bool))
			{
				bool bool_value = false;
				string bool_data = value_data.ToLower();

				value = false;

				if (bool_data.Equals("si"))
					value = true;
				else if (bool_data.Equals("no"))
					value = false;
				else if (bool_data.Equals("1"))
					value = true;
				else if (bool_data.Equals("0"))
					value = false;
				else
				{
					if (Boolean.TryParse(bool_data, out bool_value))
						value = bool_value;
					else
						throw new ArgumentException(string.Format(RecursoMensajes.ValueFormatError, value_data, row_index, column_name));
				}
			}
			else if (property.PropertyType == typeof(decimal))
			{
				decimal decimal_value = 0;

				if (Decimal.TryParse(value_data, NumberStyles.Any, CultureInfo.CreateSpecificCulture("es"), out decimal_value))
					value = decimal_value;
				else
					throw new ArgumentException(string.Format(RecursoMensajes.ValueFormatError, value_data, row_index, column_name));
			}
			else
				value = prop.Converter.ConvertFromInvariantString(value_data);

			return value;
		}

		/// <summary>
		/// obtiene un valor de un string basado en la propiedad de un objeto
		/// </summary>
		protected static T GetExportValue<T>(string column_name, int row_index, string value_data)
		{
			Type type = typeof(T);
			object value;

			if (type == typeof(DateTime))
			{
				DateTime fecha = Fecha.GetFecha();

				if (DateTime.TryParseExact(value_data, Fecha.formato_fecha_hora_archivo, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
					value = fecha;
				else
					throw new ArgumentException(RecursoMensajes.ValueFormatError);
			}
			else if (type == typeof(bool))
			{
				bool bool_value = false;
				string bool_data = value_data.ToLower();

				value = false;

				if (bool_data.Equals("si"))
					value = true;
				else if (bool_data.Equals("no"))
					value = false;
				else if (bool_data.Equals("1"))
					value = true;
				else if (bool_data.Equals("0"))
					value = false;
				else
				{
					if (Boolean.TryParse(bool_data, out bool_value))
						value = bool_value;
					else
						throw new ArgumentException(string.Format(RecursoMensajes.ValueFormatError, value_data, row_index, column_name));
				}
			}
			else if (type == typeof(decimal))
			{
				decimal decimal_value = 0;

				if (Decimal.TryParse(value_data, NumberStyles.Any, CultureInfo.CreateSpecificCulture("es"), out decimal_value))
					value = decimal_value;
				else
					throw new ArgumentException(string.Format(RecursoMensajes.ValueFormatError, value_data, row_index, column_name));
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ValueFormatError, value_data, row_index, column_name, type));

			return (T)value;
		}
	}
}
