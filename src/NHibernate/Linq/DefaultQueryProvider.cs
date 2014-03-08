using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Linq
{
	public interface INhQueryProvider : IQueryProvider
	{
		object ExecuteFuture(Expression expression);
		Task<TResult> ExecuteAsync<TResult>(Expression expression);
		void SetResultTransformerAndAdditionalCriteria(IQuery query, NhLinqExpression nhExpression, IDictionary<string, Tuple<object, IType>> parameters);
	}

	public class DefaultQueryProvider : INhQueryProvider
	{
		private static readonly MethodInfo CreateQueryMethodDefinition = ReflectionHelper.GetMethodDefinition((DefaultQueryProvider p) => p.CreateQuery<object>(null));

		private readonly WeakReference _session;

		public DefaultQueryProvider(ISessionImplementor session)
		{
			_session = new WeakReference(session, true);
		}

		protected virtual ISessionImplementor Session
		{
			get { return _session.Target as ISessionImplementor; }
		}

		public virtual async Task<object> Execute(Expression expression, bool async)
		{
			IQuery query;
			NhLinqExpression nhQuery;
			NhLinqExpression nhLinqExpression = PrepareQuery(expression, out query, out nhQuery);

			return await ExecuteQuery(nhLinqExpression, query, nhQuery, async);
		}
		public virtual object Execute(Expression expression)
		{
			IQuery query;
			NhLinqExpression nhQuery;
			NhLinqExpression nhLinqExpression = PrepareQuery(expression, out query, out nhQuery);

			try
			{
				return ExecuteQuery(nhLinqExpression, query, nhQuery, false).Result;
			}
			catch (AggregateException e)
			{
				throw e.InnerException;
			}
		}
		public TResult Execute<TResult>(Expression expression)
		{
			try
			{
				return (TResult)Execute(expression, false).Result;
			}
			catch (AggregateException e)
			{
				throw e.InnerException;
			}
		}

		public async Task<TResult> ExecuteAsync<TResult>(Expression expression)
		{
			return (TResult)await Execute(expression, true);
		}
		public virtual IQueryable CreateQuery(Expression expression)
		{
			MethodInfo m = CreateQueryMethodDefinition.MakeGenericMethod(expression.Type.GetGenericArguments()[0]);

			return (IQueryable)m.Invoke(this, new object[] { expression });
		}

		public virtual IQueryable<T> CreateQuery<T>(Expression expression)
		{
			return new NhQueryable<T>(this, expression);
		}

		public virtual object ExecuteFuture(Expression expression)
		{
			IQuery query;
			NhLinqExpression nhQuery;
			NhLinqExpression nhLinqExpression = PrepareQuery(expression, out query, out nhQuery);
			return ExecuteFutureQuery(nhLinqExpression, query, nhQuery);
		}

		protected NhLinqExpression PrepareQuery(Expression expression, out IQuery query, out NhLinqExpression nhQuery)
		{
			var nhLinqExpression = new NhLinqExpression(expression, Session.Factory);

			query = Session.CreateQuery(nhLinqExpression);

			nhQuery = (NhLinqExpression)((ExpressionQueryImpl)query).QueryExpression;

			SetParameters(query, nhLinqExpression.ParameterValuesByName);
			SetResultTransformerAndAdditionalCriteria(query, nhQuery, nhLinqExpression.ParameterValuesByName);
			return nhLinqExpression;
		}

		protected virtual object ExecuteFutureQuery(NhLinqExpression nhLinqExpression, IQuery query, NhLinqExpression nhQuery)
		{
			MethodInfo method;
			if (nhLinqExpression.ReturnType == NhLinqExpressionReturnType.Sequence)
			{
				method = typeof(IQuery).GetMethod("Future").MakeGenericMethod(nhQuery.Type);
			}
			else
			{
				method = typeof(IQuery).GetMethod("FutureValue").MakeGenericMethod(nhQuery.Type);
			}

			object result = method.Invoke(query, new object[0]);

			if (nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer != null)
			{
				((IDelayedValue)result).ExecuteOnEval = nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer;
			}

			return result;
		}

		protected virtual async Task<object> ExecuteQuery(NhLinqExpression nhLinqExpression, IQuery query, NhLinqExpression nhQuery, bool async)
		{
			IList results = await query.ListAsync();

			if (nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer != null)
			{
				try
				{
					return nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer.DynamicInvoke(results.AsQueryable());
				}
				catch (TargetInvocationException e)
				{
					throw e.InnerException;
				}
			}

			if (nhLinqExpression.ReturnType == NhLinqExpressionReturnType.Sequence)
			{
				return results.AsQueryable();
			}

			return results[0];
		}

		private static void SetParameters(IQuery query, IDictionary<string, Tuple<object, IType>> parameters)
		{
			foreach (var parameterName in query.NamedParameters)
			{
				var param = parameters[parameterName];

				if (param.Item1 == null)
				{
					if (typeof(IEnumerable).IsAssignableFrom(param.Item2.ReturnedClass) &&
						param.Item2.ReturnedClass != typeof(string))
					{
						query.SetParameterList(parameterName, null, param.Item2);
					}
					else
					{
						query.SetParameter(parameterName, null, param.Item2);
					}
				}
				else
				{
					if (param.Item1 is IEnumerable && !(param.Item1 is string))
					{
						query.SetParameterList(parameterName, (IEnumerable)param.Item1);
					}
					else if (param.Item2 != null)
					{
						query.SetParameter(parameterName, param.Item1, param.Item2);
					}
					else
					{
						query.SetParameter(parameterName, param.Item1);
					}
				}
			}
		}

		public void SetResultTransformerAndAdditionalCriteria(IQuery query, NhLinqExpression nhExpression, IDictionary<string, Tuple<object, IType>> parameters)
		{
			query.SetResultTransformer(nhExpression.ExpressionToHqlTranslationResults.ResultTransformer);

			foreach (var criteria in nhExpression.ExpressionToHqlTranslationResults.AdditionalCriteria)
			{
				criteria(query, parameters);
			}
		}
	}
}