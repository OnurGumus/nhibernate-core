using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractQueryImpl2 : AbstractQueryImpl
	{
		public override async Task<int> ExecuteUpdateAsync()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.ExecuteUpdateAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IEnumerable> EnumerableAsync()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.EnumerableAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.EnumerableAsync<T>(ExpandParameters(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IList> ListAsync()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.ListAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}

		public override async Task ListAsync(IList results)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				await (Session.ListAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams), results));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IList<T>> ListAsync<T>()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.ListAsync<T>(ExpandParameters(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}

		protected internal override async Task<IEnumerable<ITranslator>> GetTranslatorsAsync(ISessionImplementor sessionImplementor, QueryParameters queryParameters)
		{
			// NOTE: updates queryParameters.NamedParameters as (desired) side effect
			var queryExpression = ExpandParameters(queryParameters.NamedParameters);
			return (await (sessionImplementor.GetQueriesAsync(queryExpression, false))).Select(queryTranslator => new HqlTranslatorWrapper(queryTranslator));
		}
	}
}