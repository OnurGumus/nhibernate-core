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
using System.Data.Common;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Impl;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	public abstract partial class CollectionType : AbstractType, IAssociationType
	{

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, new string[] { name }, session, owner);
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string[] name, ISessionImplementor session, object owner)
		{
			return ResolveIdentifierAsync(null, session, owner);
		}

		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			//we must use the "remembered" uk value, since it is 
			//not available from the EntityEntry during assembly
			if (cached == null)
			{
				return null;
			}
			else
			{
				object key = await (GetPersister(session).KeyType.AssembleAsync(cached, session, owner)).ConfigureAwait(false);
				return await (ResolveKeyAsync(key, session, owner)).ConfigureAwait(false);
			}
		}

		public override Task<object> HydrateAsync(DbDataReader rs, string[] name, ISessionImplementor session, object owner)
		{
			try
			{
				// can't just return null here, since that would
				// cause an owning component to become null
				return Task.FromResult<object>(NotNullCollection);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		public override Task<object> ResolveIdentifierAsync(object key, ISessionImplementor session, object owner)
		{
			try
			{
				return ResolveKeyAsync(GetKeyOfOwner(owner, session), session, owner);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		private async Task<object> ResolveKeyAsync(object key, ISessionImplementor session, object owner)
		{
			return key == null ? null : await (GetCollectionAsync(key, session, owner)).ConfigureAwait(false);
		}

		public async Task<object> GetCollectionAsync(object key, ISessionImplementor session, object owner)
		{
			ICollectionPersister persister = GetPersister(session);
			IPersistenceContext persistenceContext = session.PersistenceContext;

			// check if collection is currently being loaded
			IPersistentCollection collection = persistenceContext.LoadContexts.LocateLoadingCollection(persister, key);
			if (collection == null)
			{
				// check if it is already completely loaded, but unowned
				collection = persistenceContext.UseUnownedCollection(new CollectionKey(persister, key));
				if (collection == null)
				{
					// create a new collection wrapper, to be initialized later
					collection = Instantiate(session, persister, key);
					collection.Owner = owner;

					persistenceContext.AddUninitializedCollection(persister, collection, key);

					// some collections are not lazy:
					if (InitializeImmediately())
					{
						await (session.InitializeCollectionAsync(collection, false)).ConfigureAwait(false);
					}
					else if (!persister.IsLazy)
					{
						persistenceContext.AddNonLazyCollection(collection);
					}

					if (HasHolder())
					{
						session.PersistenceContext.AddCollectionHolder(collection);
					}
				}

				if (log.IsDebugEnabled)
				{
					log.Debug("Created collection wrapper: " + MessageHelper.CollectionInfoString(persister, collection, key, session));
				}
			}
			collection.Owner = owner;
			return collection.GetValue();
		}

		public override Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
		{
			throw new NotSupportedException("collection mappings may not form part of a property-ref");
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner,
									   IDictionary copyCache)
		{
			if (original == null)
			{
				return null;
			}

			if (!NHibernateUtil.IsInitialized(original))
			{
				return target;
			}

			object result = target == null || target == original
								? InstantiateResult(original)
								: target;

			//for arrays, replaceElements() may return a different reference, since
			//the array length might not match
			result = await (ReplaceElementsAsync(original, result, owner, copyCache, session)).ConfigureAwait(false);

			if (original == target)
			{
				//get the elements back into the target
				//TODO: this is a little inefficient, don't need to do a whole
				//	  deep replaceElements() call
				await (ReplaceElementsAsync(result, target, owner, copyCache, session)).ConfigureAwait(false);
				result = target;
			}

			return result;
		}

		public virtual async Task<object> ReplaceElementsAsync(object original, object target, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			var elemType = GetElementType(session.Factory);
			var targetPc = target as IPersistentCollection;
			var originalPc = original as IPersistentCollection;
			var iterOriginal = (IEnumerable)original;
			var clearTargetsDirtyFlag = ShouldTargetsDirtyFlagBeCleared(targetPc, originalPc, iterOriginal);

			// copy elements into newly empty target collection
			Clear(target);
			foreach (var obj in iterOriginal)
			{
				Add(target, await (elemType.ReplaceAsync(obj, null, session, owner, copyCache)).ConfigureAwait(false));
			}

			if(clearTargetsDirtyFlag)
			{
				targetPc.ClearDirty();
			}

			return target;
		}

		#region Methods added in NH

		#endregion
	}
}
