#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlQueryImpl : AbstractQueryImpl, ISQLQuery
	{
		public override async Task<IList> ListAsync()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
			QueryParameters qp = GetQueryParameters(namedParams);
			Before();
			try
			{
				return await (Session.ListAsync(spec, qp));
			}
			finally
			{
				After();
			}
		}

		public override async Task ListAsync(IList results)
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
			QueryParameters qp = GetQueryParameters(namedParams);
			Before();
			try
			{
				await (Session.ListAsync(spec, qp, results));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IList<T>> ListAsync<T>()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
			QueryParameters qp = GetQueryParameters(namedParams);
			Before();
			try
			{
				return await (Session.ListAsync<T>(spec, qp));
			}
			finally
			{
				After();
			}
		}

		public override Task<IEnumerable> EnumerableAsync()
		{
			try
			{
				return Task.FromResult<IEnumerable>(Enumerable());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable>(ex);
			}
		}

		public override Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			try
			{
				return Task.FromResult<IEnumerable<T>>(Enumerable<T>());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable<T>>(ex);
			}
		}

		public override async Task<int> ExecuteUpdateAsync()
		{
			IDictionary<string, TypedValue> namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.ExecuteNativeUpdateAsync(GenerateQuerySpecification(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}

		protected internal override Task<IEnumerable<ITranslator>> GetTranslatorsAsync(ISessionImplementor sessionImplementor, QueryParameters queryParameters)
		{
			try
			{
				return Task.FromResult<IEnumerable<ITranslator>>(GetTranslators(sessionImplementor, queryParameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable<ITranslator>>(ex);
			}
		}
	}
}
#endif
