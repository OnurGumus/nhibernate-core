﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class UInt16Type : PrimitiveType, IDiscriminatorType, IVersionType
	{

		#region IVersionType Members

		public virtual Task<object> NextAsync(object current, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>((UInt16)current + 1);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		public virtual Task<object> SeedAsync(ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(1);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		#endregion
	}
}