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
using System.Linq;
using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Util;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class GenericIdentifierBagType<T> : CollectionType
	{

		public override async Task<object> ReplaceElementsAsync(
			object original, 			object target, 			object owner, 			IDictionary copyCache, 			ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var elemType = GetElementType(session.Factory);
			var targetPc = target as PersistentIdentifierBag<T>;
			var originalPc = original as IPersistentCollection;
			var iterOriginal = (IList<T>)original;
			var clearTargetsDirtyFlag = false;
			var clearTarget = true;

			if (targetPc != null)
			{
				if (originalPc == null)
				{
					if (!targetPc.IsDirty && AreCollectionElementsEqual(iterOriginal, targetPc))
					{
						clearTargetsDirtyFlag = true;
						clearTarget = false;
					}
				}
				else
				{
					if (!originalPc.IsDirty)
					{
						clearTargetsDirtyFlag = true;
					}
				}
			}

			if (clearTarget)
			{
				Clear(target);
				foreach (var obj in iterOriginal)
				{
					Add(target, await (elemType.ReplaceAsync(obj, null, session, owner, copyCache, cancellationToken)).ConfigureAwait(false));
				}
			}
			else
			{
				var originalLookup = iterOriginal.ToLookup(e => e);
				for (var i = 0; i < targetPc.Count; i++)
				{
					var currTarget = targetPc[i];
					var orgToUse = originalLookup[currTarget].First();
					targetPc[i] = (T)await (elemType.ReplaceAsync(orgToUse, null, session, owner, copyCache, cancellationToken)).ConfigureAwait(false);
				}
			}

			if (clearTargetsDirtyFlag)
			{
				targetPc.ClearDirty();
			}

			return target;
		}
	}
}
