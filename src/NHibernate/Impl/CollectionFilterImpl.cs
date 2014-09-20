using System.Collections;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Type;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace NHibernate.Impl
{
	/// <summary>
	/// Implementation of the <see cref="IQuery"/> interface for collection filters.
	/// </summary>
	public class CollectionFilterImpl : QueryImpl
	{
		private readonly object collection;

		public CollectionFilterImpl(string queryString, object collection, ISessionImplementor session, ParameterMetadata parameterMetadata)
			: base(queryString, session, parameterMetadata)
		{
			this.collection = collection;
		}

		public override IEnumerable Enumerable()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return Session.EnumerableFilter(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
		}

		public override IEnumerable<T> Enumerable<T>()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return Session.EnumerableFilter<T>(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
		}
		public override IList List()
		{
			try
			{
				return this.ListAsync().Result;
			}
			catch (AggregateException e)
			{
				throw e.InnerException;
			}
		}
		public override IList<T> List<T>()
		{
			try
			{
				return this.ListAsync<T>().Result;
			}
			catch (AggregateException e)
			{
				throw e.InnerException;
			}
		}
		public override async Task<IList> ListAsync()
		{


			await Task.Yield();

			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return Session.ListFilter(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
		}

		public override async Task<IList<T>> ListAsync<T>()
		{

			await Task.Yield();

			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			return Session.ListFilter<T>(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
		}

		public override IType[] TypeArray()
		{
			IList<IType> typeList = Types;
			int size = typeList.Count;
			IType[] result = new IType[size + 1];
			for (int i = 0; i < size; i++)
			{
				result[i + 1] = typeList[i];
			}
			return result;
		}

		public override object[] ValueArray()
		{
			IList valueList = Values;
			int size = valueList.Count;
			object[] result = new object[size + 1];
			for (int i = 0; i < size; i++)
			{
				result[i + 1] = valueList[i];
			}
			return result;
		}
	}
}
