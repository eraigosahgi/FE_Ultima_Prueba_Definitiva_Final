using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	public static class ModelExt
	{
		public static ObjectSet<T> FindObjectSet<T>(this IObjectContextAdapter context) where T : class
		{
			return context.ObjectContext.CreateObjectSet<T>();
		}

		public static T Detach<T>(this DbContext context, T entity) where T : class
		{
			if (entity != null)
				context.FindObjectSet<T>().Detach(entity);

			return entity;
		}

		public static IEnumerable<string> FindPrimaryKey<T>(this DbContext context) where T : class
		{
			EntityType elementType = FindObjectSet<T>(context).EntitySet.ElementType;
			return elementType.KeyMembers.Select(p => p.Name);
		}

		public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
		{
			if (includes != null)
				query = includes.Aggregate(query, (current, include) => current.Include(include));

			return query;
		}

		/// <summary>
		/// Función para ordenar los datos de una consulta DBML 
		/// </summary>
		public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string sortExpression) //where TEntity : class
		{
			if (!string.IsNullOrWhiteSpace(sortExpression))
			{
				string[] sortOptions = sortExpression.Split(' ');

				string campo = "";
				string direccion = "";

				if (sortOptions.Length == 2)
				{
					campo = sortOptions[0];
					direccion = sortOptions[1];

					var type = typeof(TEntity);

					string methodName = "";

					if (direccion.Equals("ASC"))
						methodName = "OrderBy";
					else
						methodName = "OrderByDescending";

					MethodCallExpression resultExp = null;

					if (!campo.Contains("."))
					{
						PropertyInfo property = type.GetProperty(campo);

						var parametro = Expression.Parameter(type, "p");
						var propertyAccess = Expression.MakeMemberAccess(parametro, property);
						var orderByExp = Expression.Lambda(propertyAccess, parametro);

						resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
					}
					else
					{
						Type relationType = type.GetProperty(campo.Split('.')[0]).PropertyType;

						PropertyInfo relationProperty = type.GetProperty(campo.Split('.')[0]);

						PropertyInfo relationProperty2 = relationType.GetProperty(campo.Split('.')[1]);

						var parameter = Expression.Parameter(type, "p");

						var propertyAccess = Expression.MakeMemberAccess(parameter, relationProperty);

						var propertyAccess2 = Expression.MakeMemberAccess(propertyAccess, relationProperty2);

						var orderByExp = Expression.Lambda(propertyAccess2, parameter);

						resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, relationProperty2.PropertyType }, source.Expression, Expression.Quote(orderByExp));
					}
					return source.Provider.CreateQuery<TEntity>(resultExp);
				}
				else
					return source;
			}
			else
				return source;
		}

		/// <summary>
		/// funcion para obtener resultados unicos de conjuntos de datos
		/// </summary>
		/// <typeparam name="TSource">tipo del conjunto de datos</typeparam>
		/// <typeparam name="TKey">tipo de objeto, resultado de la comparacion el el distinc</typeparam>
		/// <param name="source">conjunto de datos</param>
		/// <param name="keySelector">funcion selector para el distinc</param>
		/// <param name="comparer">comparar para el distinc</param>
		/// <returns>conjuntode datos con distinct</returns>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
			foreach (TSource element in source)
			{
				if (knownKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}
	}
}