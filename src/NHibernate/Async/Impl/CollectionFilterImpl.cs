﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Type;
using System.Collections.Generic;

namespace NHibernate.Impl
{
	using System.Threading.Tasks;
	using System;
	/// <summary>
	/// Implementation of the <see cref="IQuery"/> interface for collection filters.
	/// </summary>
	public partial class CollectionFilterImpl : QueryImpl
	{

		public override Task<IEnumerable> EnumerableAsync()
		{
			try
			{
				VerifyParameters();
				IDictionary<string, TypedValue> namedParams = NamedParams;
				return Session.EnumerableFilterAsync(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
			}
			catch (Exception ex)
			{
				return Task.FromException<IEnumerable>(ex);
			}
		}

		public override Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			try
			{
				VerifyParameters();
				IDictionary<string, TypedValue> namedParams = NamedParams;
				return Session.EnumerableFilterAsync<T>(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
			}
			catch (Exception ex)
			{
				return Task.FromException<IEnumerable<T>>(ex);
			}
		}

		public override Task<IList> ListAsync()
		{
			try
			{
				VerifyParameters();
				IDictionary<string, TypedValue> namedParams = NamedParams;
				return Session.ListFilterAsync(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
			}
			catch (Exception ex)
			{
				return Task.FromException<IList>(ex);
			}
		}

		public override Task<IList<T>> ListAsync<T>()
		{
			try
			{
				VerifyParameters();
				IDictionary<string, TypedValue> namedParams = NamedParams;
				return Session.ListFilterAsync<T>(collection, ExpandParameterLists(namedParams), GetQueryParameters(namedParams));
			}
			catch (Exception ex)
			{
				return Task.FromException<IList<T>>(ex);
			}
		}
	}
}
