using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Impl;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System.Threading.Tasks;
using System.Reflection;

namespace NHibernate.Linq
{
	public static class LinqExtensionMethods
	{
		
		public static IQueryable<T> Query<T>(this ISession session)
		{
			return new NhQueryable<T>(session.GetSessionImplementation());
		}

		public static IQueryable<T> Query<T>(this IStatelessSession session)
		{
			return new NhQueryable<T>(session.GetSessionImplementation());
		}

	    /// <summary>Determines whether a sequence contains any elements.</summary>
        /// <returns>true if the source sequence contains any elements; otherwise, false.</returns>
        /// <param name="source">A sequence to check for being empty.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source" /> is null.</exception>
        public static async Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
            {
              throw new ArgumentNullException("source");
            }
			INhQueryProvider provider = (INhQueryProvider)source.Provider;

			var typeArray = new System.Type[] { typeof(TSource) };
            MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeArray);
            Expression[] expression = new Expression[] { source.Expression };
            return await provider.ExecuteAsync<bool>(Expression.Call(null, methodInfo, expression));
        }

        /// <summary>Determines whether any element of a sequence satisfies a condition.</summary>
        /// <returns>true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
        /// <param name="source">A sequence whose elements to test for a condition.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///  <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static async Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			INhQueryProvider provider = (INhQueryProvider)source.Provider;
			MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
			Expression[] expression = new Expression[] { source.Expression, Expression.Quote(predicate) };
			return await provider.ExecuteAsync<bool>(Expression.Call(null, methodInfo, expression));
		}
		 /// <summary>Returns the number of elements in a sequence.</summary>
        /// <returns>The number of elements in the input sequence.</returns>
        /// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> that contains the elements to be counted.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static async Task<int> CountAsync<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
            {
               throw new ArgumentNullException("source");
            }
            INhQueryProvider provider = (INhQueryProvider)source.Provider;
            MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
            Expression[] expression = new Expression[] { source.Expression };
            return await provider.ExecuteAsync<int>(Expression.Call(null, methodInfo, expression));
        }

        /// <summary>Returns the number of elements in the specified sequence that satisfies a condition.</summary>
        /// <returns>The number of elements in the sequence that satisfies the condition in the predicate function.</returns>
        /// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> that contains the elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
		public static async Task<int> CountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			INhQueryProvider provider = (INhQueryProvider)source.Provider;
			MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
			Expression[] expression = new Expression[] { source.Expression, Expression.Quote(predicate) };
			return  await provider.ExecuteAsync<int>(Expression.Call(null, methodInfo, expression));
		}


		/// <summary>Returns the first element of a sequence.</summary>
		/// <returns>The first element in <paramref name="source" />.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
		public static async Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			INhQueryProvider provider = (INhQueryProvider)source.Provider;
			MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
			Expression[] expression = new Expression[] { source.Expression };
			return await provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the first element of a sequence that satisfies a specified condition.</summary>
		/// <returns>The first element in <paramref name="source" /> that passes the test in <paramref name="predicate" />.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
		public static  async Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			INhQueryProvider provider = (INhQueryProvider)source.Provider;
			MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
			Expression[] expression = new Expression[] { source.Expression, Expression.Quote(predicate) };
			return await provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
		/// <returns>default(<paramref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
		/// <param name="source">The <see cref="T:System.Linq.IQueryable`1" /> to return the first element of.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source" /> is null.</exception>
		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			INhQueryProvider provider = (INhQueryProvider)source.Provider;
			MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
			Expression[] expression = new Expression[] { source.Expression };
			return await provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		/// <summary>Returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found.</summary>
		/// <returns>default(<paramref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
		/// <param name="source">An <see cref="T:System.Linq.IQueryable`1" /> to return an element from.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
		public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			INhQueryProvider provider = (INhQueryProvider)source.Provider;
			MethodInfo methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new System.Type[] { typeof(TSource) });
			Expression[] expression = new Expression[] { source.Expression, Expression.Quote(predicate) };
			return await provider.ExecuteAsync<TSource>(Expression.Call(null, methodInfo, expression));
		}

		public static IQueryable<T> Cacheable<T>(this IQueryable<T> query)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => Cacheable<object>(null)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression);

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

		public static IQueryable<T> SetLockMode<T>(this IQueryable<T> query, LockMode lockMode)
		{
			var method = ReflectionHelper.GetMethodDefinition(() => SetLockMode<object>(null, LockMode.Read)).MakeGenericMethod(typeof(T));

			var callExpression = Expression.Call(method, query.Expression, Expression.Constant(lockMode));

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



		public static async Task<IEnumerable<T>> ToListAsync<T>(this IQueryable<T> query)
		{
			var nhQueryable = query as QueryableBase<T>;
			if (nhQueryable == null)
				throw new NotSupportedException("Query needs to be of type QueryableBase<T>");

			var provider = (INhQueryProvider)nhQueryable.Provider;
			var result = await provider.ExecuteAsync<IEnumerable<T>>(nhQueryable.Expression);
			return (IEnumerable<T>)result;
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
