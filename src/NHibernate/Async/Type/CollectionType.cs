#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using System.Xml;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Collections.Generic;
using NHibernate.Impl;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class CollectionType : AbstractType, IAssociationType
	{
		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, new string[]{name}, session, owner);
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string[] name, ISessionImplementor session, object owner)
		{
			return ResolveIdentifierAsync(null, session, owner);
		}

		public override Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// NOOP
		}

		public override Task NullSafeSetAsync(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			//remember the uk value
			//This solution would allow us to eliminate the owner arg to disassemble(), but
			//what if the collection was null, and then later had elements added? seems unsafe
			//session.getPersistenceContext().getCollectionEntry( (PersistentCollection) value ).getKey();
			object key = GetKeyOfOwner(owner, session);
			if (key == null)
			{
				return null;
			}
			else
			{
				return await (GetPersister(session).KeyType.DisassembleAsync(key, session, owner));
			}
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
				object key = await (GetPersister(session).KeyType.AssembleAsync(cached, session, owner));
				return await (ResolveKeyAsync(key, session, owner));
			}
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
		{
			// collections don't dirty an unversioned parent entity
			// TODO: I don't like this implementation; it would be better if this was handled by SearchForDirtyCollections();
			return IsOwnerVersioned(session) && await (base.IsDirtyAsync(old, current, session));
		}

		public override Task<object> HydrateAsync(DbDataReader rs, string[] name, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Hydrate(rs, name, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<object> ResolveIdentifierAsync(object key, ISessionImplementor session, object owner)
		{
			return await (ResolveKeyAsync(GetKeyOfOwner(owner, session), session, owner));
		}

		private Task<object> ResolveKeyAsync(object key, ISessionImplementor session, object owner)
		{
			return key == null ? Task.FromResult<object>(null) : GetCollectionAsync(key, session, owner);
		}

		public async Task<object> GetCollectionAsync(object key, ISessionImplementor session, object owner)
		{
			ICollectionPersister persister = GetPersister(session);
			IPersistenceContext persistenceContext = session.PersistenceContext;
			EntityMode entityMode = session.EntityMode;
			if (entityMode == EntityMode.Xml && !isEmbeddedInXML)
			{
				return UnfetchedCollection;
			}

			// check if collection is currently being loaded
			IPersistentCollection collection = persistenceContext.LoadContexts.LocateLoadingCollection(persister, key);
			if (collection == null)
			{
				// check if it is already completely loaded, but unowned
				collection = persistenceContext.UseUnownedCollection(new CollectionKey(persister, key, entityMode));
				if (collection == null)
				{
					// create a new collection wrapper, to be initialized later
					collection = Instantiate(session, persister, key);
					collection.Owner = owner;
					persistenceContext.AddUninitializedCollection(persister, collection, key);
					// some collections are not lazy:
					if (InitializeImmediately(entityMode))
					{
						await (session.InitializeCollectionAsync(collection, false));
					}
					else if (!persister.IsLazy)
					{
						persistenceContext.AddNonLazyCollection(collection);
					}

					if (HasHolder(entityMode))
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
			try
			{
				return Task.FromResult<object>(SemiResolve(value, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache)
		{
			if (original == null)
			{
				return null;
			}

			if (!NHibernateUtil.IsInitialized(original))
			{
				return target;
			}

			object result = target == null || target == original ? InstantiateResult(original) : target;
			//for arrays, replaceElements() may return a different reference, since
			//the array length might not match
			result = await (ReplaceElementsAsync(original, result, owner, copyCache, session));
			if (original == target)
			{
				await (ReplaceElementsAsync(result, target, owner, copyCache, session));
				result = target;
			}

			return result;
		}

		public virtual async Task<object> ReplaceElementsAsync(object original, object target, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			// TODO: does not work for EntityMode.DOM4J yet!
			object result = target;
			Clear(result);
			// copy elements into newly empty target collection
			IType elemType = GetElementType(session.Factory);
			IEnumerable iter = (IEnumerable)original;
			foreach (object obj in iter)
			{
				Add(result, await (elemType.ReplaceAsync(obj, null, session, owner, copyCache)));
			}

			// if the original is a PersistentCollection, and that original
			// was not flagged as dirty, then reset the target's dirty flag
			// here after the copy operation.
			// One thing to be careful of here is a "bare" original collection
			// in which case we should never ever ever reset the dirty flag
			// on the target because we simply do not know...
			IPersistentCollection originalPc = original as IPersistentCollection;
			IPersistentCollection resultPc = result as IPersistentCollection;
			if (originalPc != null && resultPc != null)
			{
				if (!originalPc.IsDirty)
					resultPc.ClearDirty();
			}

			return result;
		}

		public override Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return IsDirtyAsync(old, current, session);
		}

		public override Task<bool> IsModifiedAsync(object oldHydratedState, object currentState, bool[] checkable, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<bool>(IsModified(oldHydratedState, currentState, checkable, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public virtual async Task<bool> ContainsAsync(object collection, object childObject, ISessionImplementor session)
		{
			// we do not have to worry about queued additions to uninitialized
			// collections, since they can only occur for inverse collections!
			IEnumerable elems = GetElementsIterator(collection, session);
			foreach (object elem in elems)
			{
				object element = elem;
				// worrying about proxies is perhaps a little bit of overkill here...
				if (element.IsProxy())
				{
					INHibernateProxy proxy = element as INHibernateProxy;
					ILazyInitializer li = proxy.HibernateLazyInitializer;
					if (!li.IsUninitialized)
						element = await (li.GetImplementationAsync());
				}

				if (element == childObject)
					return true;
			}

			return false;
		}
	}
}
#endif
