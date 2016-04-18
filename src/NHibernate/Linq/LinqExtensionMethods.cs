using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Impl;
using NHibernate.Type;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System.Threading.Tasks;
using System.Reflection;

namespace NHibernate.Linq
{
	public static class LinqExtensionMethods
	{
		private static readonly Dictionary<string, MethodInfo> cachableQueryableMethods;

		static LinqExtensionMethods()
		{
			var methods = typeof(Queryable).GetMethods().ToLookup(o => o.Name, o => o);

			cachableQueryableMethods = new Dictionary<string, MethodInfo>
			{
				{"Count", methods["Count"].Single(o => o.GetParameters().Length == 1)},
				{"CountParam", methods["Count"].Single(o => o.GetParameters().Length == 2)},

				{"LongCount", methods["LongCount"].Single(o => o.GetParameters().Length == 1)},
				{"LongCountParam", methods["LongCount"].Single(o => o.GetParameters().Length == 2)},

				{"Any", methods["Any"].Single(o => o.GetParameters().Length == 1)},
				{"AnyParam", methods["Any"].Single(o => o.GetParameters().Length == 2)},

				{"First", methods["First"].Single(o => o.GetParameters().Length == 1)},
				{"FirstParam", methods["First"].Single(o => o.GetParameters().Length == 2)},

				{"FirstOrDefault", methods["FirstOrDefault"].Single(o => o.GetParameters().Length == 1)},
				{"FirstOrDefaultParam", methods["FirstOrDefault"].Single(o => o.GetParameters().Length == 2)},

				{"Single", methods["Single"].Single(o => o.GetParameters().Length == 1)},
				{"SingleParam", methods["Single"].Single(o => o.GetParameters().Length == 2)},

				{"SingleOrDefault", methods["SingleOrDefault"].Single(o => o.GetParameters().Length == 1)},
				{"SingleOrDefaultParam", methods["SingleOrDefault"].Single(o => o.GetParameters().Length == 2)},

				{"Min", methods["Min"].Single(o => o.GetParameters().Length == 1)},
				{"MinParam", methods["Min"].Single(o => o.GetParameters().Length == 2)},

				{"Max", methods["Max"].Single(o => o.GetParameters().Length == 1)},
				{"MaxParam", methods["Max"].Single(o => o.GetParameters().Length == 2)},

				{"SumInt", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(int))},
				{"SumInt?", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(int?))},
				{"SumLong", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(long))},
				{"SumLong?", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(long?))},
				{"SumFloat", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(float))},
				{"SumFloat?", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(float?))},
				{"SumDouble", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(double))},
				{"SumDouble?", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(double?))},
				{"SumDecimal", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(decimal))},
				{"SumDecimal?", methods["Sum"].Single(o => o.GetParameters().Length == 1 && o.ReturnType == typeof(decimal?))},
				{"SumIntParam", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(int))},
				{"SumInt?Param", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(int?))},
				{"SumLongParam", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(long))},
				{"SumLong?Param", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(long?))},
				{"SumFloatParam", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(float))},
				{"SumFloat?Param", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(float?))},
				{"SumDoubleParam", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(double))},
				{"SumDouble?Param", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(double?))},
				{"SumDecimalParam", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(decimal))},
				{"SumDecimal?Param", methods["Sum"].Single(o => o.GetParameters().Length == 2 && o.ReturnType == typeof(decimal?))},

				{"AverageInt", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(int))},
				{"AverageInt?", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(int?))},
				{"AverageLong", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(long))},
				{"AverageLong?", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(long?))},
				{"AverageFloat", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(float))},
				{"AverageFloat?", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(float?))},
				{"AverageDouble", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(double))},
				{"AverageDouble?", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(double?))},
				{"AverageDecimal", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(decimal))},
				{"AverageDecimal?", methods["Average"].Single(o => o.GetParameters().Length == 1 && o.GetParameters()[0].ParameterType.GetGenericArguments()[0] == typeof(decimal?))},
				{"AverageIntParam", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(int))},
				{"AverageInt?Param", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(int?))},
				{"AverageLongParam", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(long))},
				{"AverageLong?Param", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(long?))},
				{"AverageFloatParam", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(float))},
				{"AverageFloat?Param", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(float?))},
				{"AverageDoubleParam", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(double))},
				{"AverageDouble?Param", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(double?))},
				{"AverageDecimalParam", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(decimal))},
				{"AverageDecimal?Param", methods["Average"].Single(o => o.GetParameters().Length == 2 && o.GetParameters()[1].ParameterType.GetGenericArguments()[0].GetGenericArguments()[1] == typeof(decimal?))},
			};
		}


		
		public static IQueryable<T> Query<T>(this ISession session)
		{
			return new NhQueryable<T>(session.GetSessionImplementation());
		}

		public static IQueryable<T> Query<T>(this IStatelessSession session)
		{
			return new NhQueryable<T>(session.GetSessionImplementation());
		}

		#region AnyAsync

		/// <summary>Determines whether a sequence contains any elements.</summary>
		/// <returns>true if the source sequence contains any elements; otherwise, false.</returns>
		/// <param name="source">A sequence to check for being empty.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["Any"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<bool>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Determines whether any element of a sequence satisfies a condition.</summary>
		/// <returns>true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
		/// <param name="source">A sequence whose elements to test for a condition.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AnyParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<bool>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region CountAsync

		/// <summary>Returns the number of elements in a sequence.</summary>
		/// <returns>The number of elements in the input sequence.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> that contains the elements to be counted.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["Count"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<int>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the number of elements in the specified sequence that satisfies a condition.</summary>
		/// <returns>The number of elements in the sequence that satisfies the condition in the predicate function.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> that contains the elements to be counted.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["CountParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<int>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region SumAsync

		/// <summary>
		/// Computes the sum of a sequence of <see cref="T:System.Int32"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Int32"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
		public static Task<int> SumAsync(this IQueryable<int> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumInt"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<int>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of nullable <see cref="T:System.Int32"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int32"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
		public static Task<int?> SumAsync(this IQueryable<int?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumInt?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<int?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of <see cref="T:System.Int64"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Int64"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
		public static Task<long> SumAsync(this IQueryable<long> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumLong"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<long>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of nullable <see cref="T:System.Int64"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int64"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
		public static Task<long?> SumAsync(this IQueryable<long?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumLong?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<long?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of <see cref="T:System.Single"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Single"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Single.MaxValue"/>.</exception>
		public static Task<float> SumAsync(this IQueryable<float> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumFloat"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<float>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of nullable <see cref="T:System.Single"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Single"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Single.MaxValue"/>.</exception>
		public static Task<float?> SumAsync(this IQueryable<float?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumFloat?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<float?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of <see cref="T:System.Double"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Double"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Double.MaxValue"/>.</exception>
		public static Task<double> SumAsync(this IQueryable<double> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDouble"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of nullable <see cref="T:System.Double"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Double"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Double.MaxValue"/>.</exception>
		public static Task<double?> SumAsync(this IQueryable<double?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDouble?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of <see cref="T:System.Decimal"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Decimal"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
		public static Task<decimal> SumAsync(this IQueryable<decimal> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDecimal"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<decimal>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of a sequence of nullable <see cref="T:System.Decimal"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the values in the sequence.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Decimal"/> values to calculate the sum of.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
		public static Task<decimal?> SumAsync(this IQueryable<decimal?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDecimal?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<decimal?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
		public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumIntParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<int>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of nullable <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int32.MaxValue"/>.</exception>
		public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumInt?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<int?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
		public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumLongParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<long>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of nullable <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Int64.MaxValue"/>.</exception>
		public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumLong?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<long?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Single.MaxValue"/>.</exception>
		public static Task<float> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumFloatParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<float>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of nullable <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Single.MaxValue"/>.</exception>
		public static Task<float?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumFloat?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<float?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Double.MaxValue"/>.</exception>
		public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDoubleParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of nullable <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Double.MaxValue"/>.</exception>
		public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDouble?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
		public static Task<decimal> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDecimalParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<decimal>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the sum of the sequence of nullable <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The sum of the projected values.
		/// </returns>
		/// <param name="source">A sequence of values of type <paramref name="TSource"/>.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.OverflowException">The sum is larger than <see cref="F:System.Decimal.MaxValue"/>.</exception>
		public static Task<decimal?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SumDecimal?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<decimal?>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region Average

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Int32"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Int32"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<double> AverageAsync(this IQueryable<int> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageInt"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Int32"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the source sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int32"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<double?> AverageAsync(this IQueryable<int?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageInt?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Int64"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Int64"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<double> AverageAsync(this IQueryable<long> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageLong"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Int64"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the source sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Int64"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<double?> AverageAsync(this IQueryable<long?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageLong?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Single"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Single"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<float> AverageAsync(this IQueryable<float> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageFloat"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<float>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Single"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the source sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Single"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<float?> AverageAsync(this IQueryable<float?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageFloat?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<float?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Double"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Double"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<double> AverageAsync(this IQueryable<double> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDouble"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Double"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the source sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Double"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<double?> AverageAsync(this IQueryable<double?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDouble?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Decimal"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of <see cref="T:System.Decimal"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<decimal> AverageAsync(this IQueryable<decimal> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDecimal"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<decimal>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Decimal"/> values.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the source sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of nullable <see cref="T:System.Decimal"/> values to calculate the average of.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<decimal?> AverageAsync(this IQueryable<decimal?> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDecimal?"];
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<decimal?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageIntParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Int32"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageInt?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageLongParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Int64"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageLong?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}
		
		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<float> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageFloatParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<float>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Single"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<float?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageFloat?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<float?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDoubleParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Double"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDouble?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<double?>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
		public static Task<decimal> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDecimalParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<decimal>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Computes the average of a sequence of nullable <see cref="T:System.Decimal"/> values that is obtained by invoking a projection function on each element of the input sequence.
		/// </summary>
		/// 
		/// <returns>
		/// The average of the sequence of values, or null if the <paramref name="source"/> sequence is empty or contains only null values.
		/// </returns>
		/// <param name="source">A sequence of values to calculate the average of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<decimal?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["AverageDecimal?Param"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<decimal?>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region MinAsync

		/// <summary>
		/// Returns the minimum value of a generic <see cref="T:System.Linq.IQueryable`1"/>.
		/// </summary>
		/// 
		/// <returns>
		/// The minimum value in the sequence.
		/// </returns>
		/// <param name="source">A sequence of values to determine the minimum of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<TSource> MinAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["Min"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Invokes a projection function on each element of a generic <see cref="T:System.Linq.IQueryable`1"/> and returns the minimum resulting value.
		/// </summary>
		/// 
		/// <returns>
		/// The minimum value in the sequence.
		/// </returns>
		/// <param name="source">A sequence of values to determine the minimum of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by the function represented by <paramref name="selector"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<TResult> MinAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["MinParam"].MakeGenericMethod(typeof(TSource), typeof(TResult));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<TResult>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region MaxAsync

		/// <summary>
		/// Returns the maximum value in a generic <see cref="T:System.Linq.IQueryable`1"/>.
		/// </summary>
		/// 
		/// <returns>
		/// The maximum value in the sequence.
		/// </returns>
		/// <param name="source">A sequence of values to determine the maximum of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Task<TSource> MaxAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["Max"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>
		/// Invokes a projection function on each element of a generic <see cref="T:System.Linq.IQueryable`1"/> and returns the maximum resulting value.
		/// </summary>
		/// 
		/// <returns>
		/// The maximum value in the sequence.
		/// </returns>
		/// <param name="source">A sequence of values to determine the maximum of.</param>
		/// <param name="selector">A projection function to apply to each element.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by the function represented by <paramref name="selector"/>.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
		public static Task<TResult> MaxAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["MaxParam"].MakeGenericMethod(typeof(TSource), typeof(TResult));
			var expression = new[] { source.Expression, Expression.Quote(selector) };
			return provider.ExecuteAsync<TResult>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region LongCountAsync

		/// <summary>Returns the number of elements in a sequence.</summary>
		/// <returns>The number of elements in the input sequence.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> that contains the elements to be counted.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["LongCount"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<long>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the number of elements in the specified sequence that satisfies a condition.</summary>
		/// <returns>The number of elements in the sequence that satisfies the condition in the predicate function.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> that contains the elements to be counted.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		/// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int64.MaxValue" />.</exception>
		public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["LongCountParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<long>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region FirstAsync

		/// <summary>Returns the first element of a sequence.</summary>
		/// <returns>The first element in <paramref name="source" />.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
		public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["First"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the first element of a sequence that satisfies a specified condition.</summary>
		/// <returns>The first element in <paramref name="source" /> that passes the test in <paramref name="predicate" />.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
		public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["FirstParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region SingleAsync

		/// <summary>Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.</summary>
		/// <returns>The single element in <paramref name="source" />.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
		public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["Single"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.</summary>
		/// <returns>The single element in <paramref name="source" /> that passes the test in <paramref name="predicate" />.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
		public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SingleParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region SingleOrDefaultAsync

		/// <summary>Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.</summary>
		/// <returns>default(<paramref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the single element in <paramref name="source" />.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> to return the single element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SingleOrDefault"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.</summary>
		/// <returns>default(<paramref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the single element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["SingleOrDefaultParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		#region FirstOrDefaultAsync

		/// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
		/// <returns>default(<paramref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
		public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["FirstOrDefault"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found.</summary>
		/// <returns>default(<paramref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			var provider = (INhQueryProvider)source.Provider;
			var methodInfo = cachableQueryableMethods["FirstOrDefaultParam"].MakeGenericMethod(typeof(TSource));
			var expression = new[] { source.Expression, Expression.Quote(predicate) };
			return provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		#endregion

		

		public static IQueryable<T> Cacheable<T>(this IQueryable<T> query)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => Cacheable<object>(null)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression);

			return new NhQueryable<T>(query.Provider, callExpression);
		}
		public static IQueryable<T> SetLockMode<T>(this IQueryable<T> query, LockMode lockMode)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => SetLockMode<object>(null, LockMode.Read)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression, Expression.Constant(lockMode));

			return new NhQueryable<T>(query.Provider, callExpression);
		}

		public static IQueryable<T> CacheMode<T>(this IQueryable<T> query, CacheMode cacheMode)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => CacheMode<object>(null, NHibernate.CacheMode.Normal)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression, Expression.Constant(cacheMode));

			return new NhQueryable<T>(query.Provider, callExpression);
		}

		public static IQueryable<T> CacheRegion<T>(this IQueryable<T> query, string region)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => CacheRegion<object>(null, null)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression, Expression.Constant(region));

			return new NhQueryable<T>(query.Provider, callExpression);
		}

		public static IQueryable<T> Timeout<T>(this IQueryable<T> query, int timeout)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => Timeout<object>(null, 0)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression, Expression.Constant(timeout));

			return new NhQueryable<T>(query.Provider, callExpression);
		}


		public static IEnumerable<T> ToFuture<T>(this IQueryable<T> query)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)nhQueryable.Provider;
			var future = provider.ExecuteFuture(nhQueryable.Expression);
			return (IEnumerable<T>)future;
		}

		public static IAsyncEnumerable<T> ToFutureAsync<T>(this IQueryable<T> query)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)nhQueryable.Provider;
			return (IAsyncEnumerable<T>)provider.ExecuteFuture(nhQueryable.Expression, true);
		}

		public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)nhQueryable.Provider;
			var result = await provider.ExecuteAsync<IEnumerable<T>>(nhQueryable.Expression).ConfigureAwait(false);
			return result.ToList();
		}

		public static IFutureValue<T> ToFutureValue<T>(this IQueryable<T> query)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)nhQueryable.Provider;
			var future = provider.ExecuteFuture(nhQueryable.Expression);
			if (future is IEnumerable<T>)
			{
				return new FutureValue<T>(() => ((IEnumerable<T>)future));
			}

			return (IFutureValue<T>)future;
		}

		public static IFutureValueAsync<T> ToFutureValueAsync<T>(this IQueryable<T> query)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)nhQueryable.Provider;
			var future = provider.ExecuteFuture(nhQueryable.Expression, true);
			if (future is IAsyncEnumerable<T>)
			{
				return new FutureValueAsync<T>(async () => await ((IAsyncEnumerable<T>) future).ToList().ConfigureAwait(false));
			}

			return (FutureValueAsync<T>)future;
		}

		public static T MappedAs<T>(this T parameter, IType type)
		{
			throw new InvalidOperationException("The method should be used inside Linq to indicate a type of a parameter");
		}

		public static IFutureValue<TResult> ToFutureValue<T, TResult>(this IQueryable<T> query, Expression<Func<IQueryable<T>, TResult>> selector)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)query.Provider;

			var expression = ReplacingExpressionTreeVisitor.Replace(selector.Parameters.Single(),
																	query.Expression,
																	selector.Body);

			return (IFutureValue<TResult>)provider.ExecuteFuture(expression);
		}
	}
}
