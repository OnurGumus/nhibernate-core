﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Engine;

namespace NHibernate.Persister.Entity
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial interface IUniqueKeyLoadable
	{
		/// <summary>
		/// Load an instance of the persistent class, by a unique key other than the primary key.
		/// </summary>
		Task<object> LoadByUniqueKeyAsync(string propertyName, object uniqueKey, ISessionImplementor session);
	}
}