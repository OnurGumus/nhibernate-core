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
using System.Reflection;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Tuple;
using NHibernate.Tuple.Component;
using NHibernate.Util;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class ComponentType : AbstractType, IAbstractComponentType
	{

		public override async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return await (ResolveIdentifierAsync(await (HydrateAsync(rs, names, session, owner)).ConfigureAwait(false), session, owner)).ConfigureAwait(false);
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, new string[] {name}, session, owner);
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner,
									   IDictionary copiedAlready)
		{
			if (original == null)
				return null;

			object result = target ?? Instantiate(owner, session);

			object[] values = await (TypeHelper.ReplaceAsync(GetPropertyValues(original), GetPropertyValues(result), propertyTypes, session, owner, copiedAlready)).ConfigureAwait(false);

			SetPropertyValues(result, values);
			return result;
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			if (original == null)
				return null;

			object result = target ?? Instantiate(owner, session);

			object[] values = await (TypeHelper.ReplaceAsync(GetPropertyValues(original), GetPropertyValues(result), propertyTypes, session, owner, copyCache, foreignKeyDirection)).ConfigureAwait(false);

			SetPropertyValues(result, values);
			return result;
		}

		public override async Task<object> AssembleAsync(object obj, ISessionImplementor session, object owner)
		{
			if (obj == null)
			{
				return null;
			}
			else
			{
				object[] values = (object[]) obj;
				object[] assembled = new object[values.Length];
				for (int i = 0; i < propertyTypes.Length; i++)
				{
					assembled[i] = await (propertyTypes[i].AssembleAsync(values[i], session, owner)).ConfigureAwait(false);
				}
				object result = Instantiate(owner, session);
				SetPropertyValues(result, assembled);
				return result;
			}
		}

		public override async Task<object> HydrateAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			int begin = 0;
			bool notNull = false;
			object[] values = new object[propertySpan];
			for (int i = 0; i < propertySpan; i++)
			{
				int length = propertyTypes[i].GetColumnSpan(session.Factory);
				string[] range = ArrayHelper.Slice(names, begin, length); //cache this
				object val = await (propertyTypes[i].HydrateAsync(rs, range, session, owner)).ConfigureAwait(false);
				if (val == null)
				{
					if (isKey)
					{
						return null; //different nullability rules for pk/fk
					}
				}
				else
				{
					notNull = true;
				}
				values[i] = val;
				begin += length;
			}

			if (ReturnedClass.IsValueType)
				return values;
			else
				return notNull ? values : null;
		}

		public override async Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
		{
			if (value != null)
			{
				object result = Instantiate(owner, session);
				object[] values = (object[])value;
				object[] resolvedValues = new object[values.Length]; //only really need new array during semiresolve!
				for (int i = 0; i < values.Length; i++)
				{
					resolvedValues[i] = await (propertyTypes[i].ResolveIdentifierAsync(values[i], session, owner)).ConfigureAwait(false);
				}
				SetPropertyValues(result, resolvedValues);
				return result;
			}
			else
			{
				return null;
			}
		}

		public override Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
		{
			//note that this implementation is kinda broken
			//for components with many-to-one associations
			return ResolveIdentifierAsync(value, session, owner);
		}
	}
}