﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Runtime.CompilerServices;
using NHibernate.Engine;

namespace NHibernate.Id
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class CounterGenerator : IIdentifierGenerator
	{

		public Task<object> GenerateAsync(ISessionImplementor cache, object obj)
		{
			try
			{
				return Task.FromResult<object>(unchecked ((DateTime.Now.Ticks << 16) + Count));
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}
	}
}