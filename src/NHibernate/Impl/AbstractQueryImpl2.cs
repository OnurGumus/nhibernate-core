using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using System.Threading.Tasks;
using System;

namespace NHibernate.Impl
{
	public abstract class AbstractQueryImpl2 : AbstractQueryImpl
	{
		private readonly Dictionary<string, LockMode> _lockModes = new Dictionary<string, LockMode>(2);

		protected internal override IDictionary<string, LockMode> LockModes
		{
			get { return _lockModes; }
		}

		protected AbstractQueryImpl2(string queryString, FlushMode flushMode, ISessionImplementor session, ParameterMetadata parameterMetadata)
			: base(queryString, flushMode, session, parameterMetadata)
		{
		}

		public override IQuery SetLockMode(string alias, LockMode lockMode)
		{
			_lockModes[alias] = lockMode;
			return this;
		}
		public override int ExecuteUpdate()
		{
			return this.ExecuteUpdateAsync(false).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		public override Task<int> ExecuteUpdateAsync()
		{
			return this.ExecuteUpdateAsync(true);
		}
		public override async Task<int> ExecuteUpdateAsync(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.ExecuteUpdateAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams), async).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}

		public override IEnumerable Enumerable()
		{
			return EnumerableAsync(false).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override Task<IEnumerable> EnumerableAsync()
		{
			return EnumerableAsync(true);
		}

		public override async Task<IEnumerable> EnumerableAsync(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.EnumerableAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams), async).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}

		public override IEnumerable<T> Enumerable<T>()
		{
			return EnumerableAsync<T>(false).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			return EnumerableAsync<T>(true);
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.EnumerableAsync<T>(ExpandParameters(namedParams), GetQueryParameters(namedParams), async).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}

		public override IList List()
		{
			return this.ListAsync(false).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		public override Task<IList> ListAsync()
		{
			return this.ListAsync(true);
		}
		public override async Task<IList> ListAsync(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.ListAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams), async).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}
		public override void List(IList results)
		{
			this.ListAsync(results, false).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		public override Task ListAsync(IList results)
		{
			return this.ListAsync(results, true);
		}
		public override async Task ListAsync(IList results, bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				await Session.ListAsync(ExpandParameters(namedParams), GetQueryParameters(namedParams), results, async).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}

		public override IList<T> List<T>()
		{
			return this.ListAsync<T>(false).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		public override Task<IList<T>> ListAsync<T>()
		{
			return this.ListAsync<T>(true);
		}
		public async override Task<IList<T>> ListAsync<T>(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.ListAsync<T>(ExpandParameters(namedParams), GetQueryParameters(namedParams), async).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}
		/// <summary> 
		/// Warning: adds new parameters to the argument by side-effect, as well as mutating the query expression tree!
		/// </summary>
		protected abstract IQueryExpression ExpandParameters(IDictionary<string, TypedValue> namedParamsCopy);

		protected internal override IEnumerable<ITranslator> GetTranslators(ISessionImplementor sessionImplementor, QueryParameters queryParameters)
		{
			// NOTE: updates queryParameters.NamedParameters as (desired) side effect
			var queryExpression = ExpandParameters(queryParameters.NamedParameters);

			return sessionImplementor.GetQueries(queryExpression, false)
									 .Select(queryTranslator => new HqlTranslatorWrapper(queryTranslator));
		}
	}
}