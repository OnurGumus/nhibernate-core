using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using System.Threading.Tasks;

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
			return this.ExecuteUpdate(false).Result;
		}
		public override async Task<int> ExecuteUpdateAsync()
		{
			return await this.ExecuteUpdate(true);
		}
		public override async Task<int> ExecuteUpdate(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.ExecuteUpdate(ExpandParameters(namedParams), GetQueryParameters(namedParams),async);
			}
			finally
			{
				After();
			}
		}

		public override IEnumerable Enumerable()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return Session.Enumerable(ExpandParameters(namedParams), GetQueryParameters(namedParams));
			}
			finally
			{
				After();
			}
		}

		public override IEnumerable<T> Enumerable<T>()
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return Session.Enumerable<T>(ExpandParameters(namedParams), GetQueryParameters(namedParams));
			}
			finally
			{
				After();
			}
		}
		public override async Task<IList> ListAsync()
		{
			return await this.ListAsync(true);
		}
		public override async Task<IList> ListAsync(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.List(ExpandParameters(namedParams), GetQueryParameters(namedParams), async);
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
				await Session.List(ExpandParameters(namedParams), GetQueryParameters(namedParams), results, false);
			}
			finally
			{
				After();
			}
		}

		public override async Task<IList<T>> ListAsync<T>()
		{
			return await this.ListAsync<T>(true);
		}
		public  async Task<IList<T>> ListAsync<T>(bool async)
		{
			VerifyParameters();
			var namedParams = NamedParams;
			Before();
			try
			{
				return await Session.List<T>(ExpandParameters(namedParams), GetQueryParameters(namedParams), async);
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