using System.Collections;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Type;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	/// <summary>
	/// Implementation of the <see cref = "IQuery"/> interface for collection filters.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionFilterImpl : QueryImpl
	{
		public override async Task<IEnumerable> EnumerableAsync()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return await (Session.EnumerableFilterAsync(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams)));
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return await (Session.EnumerableFilterAsync<T>(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams)));
		}

		public override async Task<IList> ListAsync()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return await (Session.ListFilterAsync(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams)));
		}

		public override async Task<IList<T>> ListAsync<T>()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return await (Session.ListFilterAsync<T>(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams)));
		}
	}
}