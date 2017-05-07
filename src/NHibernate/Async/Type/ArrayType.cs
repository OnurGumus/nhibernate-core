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
using System.Data.Common;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Util;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class ArrayType : CollectionType
	{

		public override async Task<object> ReplaceElementsAsync(object original, object target, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			Array org = (Array) original;
			Array result = (Array)target;

			int length = org.Length;
			if (length != result.Length)
			{
				//note: this affects the return value!
				result = (Array) InstantiateResult(original);
			}

			IType elemType = GetElementType(session.Factory);
			for (int i = 0; i < length; i++)
			{
				result.SetValue(await (elemType.ReplaceAsync(org.GetValue(i), null, session, owner, copyCache)).ConfigureAwait(false), i);
			}

			return result;
		}
	}
}