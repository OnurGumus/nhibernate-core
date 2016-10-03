#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractPersistentCollection : IPersistentCollection
	{
		/// <summary>
		/// Called by any read-only method of the collection interface
		/// </summary>
		public virtual Task ReadAsync()
		{
			return InitializeAsync(false);
		}

		/// <summary> Called by the <tt>Count</tt> property</summary>
		protected virtual async Task<bool> ReadSizeAsync()
		{
			if (!initialized)
			{
				if (cachedSize != -1 && !HasQueuedOperations)
				{
					return true;
				}
				else
				{
					ThrowLazyInitializationExceptionIfNotConnected();
					CollectionEntry entry = session.PersistenceContext.GetCollectionEntry(this);
					ICollectionPersister persister = entry.LoadedPersister;
					if (persister.IsExtraLazy)
					{
						if (HasQueuedOperations)
						{
							await (session.FlushAsync());
						}

						cachedSize = await (persister.GetSizeAsync(entry.LoadedKey, session));
						return true;
					}
				}
			}

			await (ReadAsync());
			return false;
		}

		protected virtual async Task<bool ? > ReadIndexExistenceAsync(object index)
		{
			if (!initialized)
			{
				ThrowLazyInitializationExceptionIfNotConnected();
				CollectionEntry entry = session.PersistenceContext.GetCollectionEntry(this);
				ICollectionPersister persister = entry.LoadedPersister;
				if (persister.IsExtraLazy)
				{
					if (HasQueuedOperations)
					{
						await (session.FlushAsync());
					}

					return await (persister.IndexExistsAsync(entry.LoadedKey, index, session));
				}
			}

			await (ReadAsync());
			return null;
		}

		protected virtual async Task<bool ? > ReadElementExistenceAsync(object element)
		{
			if (!initialized)
			{
				ThrowLazyInitializationExceptionIfNotConnected();
				CollectionEntry entry = session.PersistenceContext.GetCollectionEntry(this);
				ICollectionPersister persister = entry.LoadedPersister;
				if (persister.IsExtraLazy)
				{
					if (HasQueuedOperations)
					{
						await (session.FlushAsync());
					}

					return await (persister.ElementExistsAsync(entry.LoadedKey, element, session));
				}
			}

			await (ReadAsync());
			return null;
		}

		protected virtual async Task<object> ReadElementByIndexAsync(object index)
		{
			if (!initialized)
			{
				ThrowLazyInitializationExceptionIfNotConnected();
				CollectionEntry entry = session.PersistenceContext.GetCollectionEntry(this);
				ICollectionPersister persister = entry.LoadedPersister;
				if (persister.IsExtraLazy)
				{
					if (HasQueuedOperations)
					{
						await (session.FlushAsync());
					}

					var elementByIndex = await (persister.GetElementByIndexAsync(entry.LoadedKey, index, session, owner));
					return persister.NotFoundObject == elementByIndex ? NotFound : elementByIndex;
				}
			}

			await (ReadAsync());
			return Unknown;
		}

		/// <summary>
		/// Called by any writer method of the collection interface
		/// </summary>
		protected virtual async Task WriteAsync()
		{
			await (InitializeAsync(true));
			Dirty();
		}

		/// <summary>
		/// Initialize the collection, if possible, wrapping any exceptions
		/// in a runtime exception
		/// </summary>
		/// <param name = "writing">currently obsolete</param>
		/// <exception cref = "LazyInitializationException">if we cannot initialize</exception>
		protected virtual async Task InitializeAsync(bool writing)
		{
			if (!initialized)
			{
				if (initializing)
				{
					throw new LazyInitializationException("illegal access to loading collection");
				}

				ThrowLazyInitializationExceptionIfNotConnected();
				await (session.InitializeCollectionAsync(this, writing));
			}
		}

		/// <summary>
		/// To be called internally by the session, forcing
		/// immediate initialization.
		/// </summary>
		/// <remarks>
		/// This method is similar to <see cref = "Initialize"/>, except that different exceptions are thrown.
		/// </remarks>
		public virtual async Task ForceInitializationAsync()
		{
			if (!initialized)
			{
				if (initializing)
				{
					throw new AssertionFailure("force initialize loading collection");
				}

				if (session == null)
				{
					throw new HibernateException("collection is not associated with any session");
				}

				if (!session.IsConnected)
				{
					throw new HibernateException("disconnected session");
				}

				await (session.InitializeCollectionAsync(this, false));
			}
		}

		public async Task<ICollection> GetQueuedOrphansAsync(string entityName)
		{
			if (HasQueuedOperations)
			{
				List<object> additions = new List<object>(operationQueue.Count);
				List<object> removals = new List<object>(operationQueue.Count);
				for (int i = 0; i < operationQueue.Count; i++)
				{
					IDelayedOperation op = operationQueue[i];
					if (op.AddedInstance != null)
					{
						additions.Add(op.AddedInstance);
					}

					if (op.Orphan != null)
					{
						removals.Add(op.Orphan);
					}
				}

				return await (GetOrphansAsync(removals, additions, entityName, session));
			}

			return CollectionHelper.EmptyCollection;
		}

		/// <summary>
		/// Called before inserting rows, to ensure that any surrogate keys are fully generated
		/// </summary>
		/// <param name = "persister"></param>
		public virtual Task PreInsertAsync(ICollectionPersister persister)
		{
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Get all "orphaned" elements
		/// </summary>
		public abstract Task<ICollection> GetOrphansAsync(object snapshot, string entityName);
		/// <summary> 
		/// Given a collection of entity instances that used to
		/// belong to the collection, and a collection of instances
		/// that currently belong, return a collection of orphans
		/// </summary>
		protected virtual async Task<ICollection> GetOrphansAsync(ICollection oldElements, ICollection currentElements, string entityName, ISessionImplementor session)
		{
			// short-circuit(s)
			if (currentElements.Count == 0)
			{
				// no new elements, the old list contains only Orphans
				return oldElements;
			}

			if (oldElements.Count == 0)
			{
				// no old elements, so no Orphans neither
				return oldElements;
			}

			IType idType = session.Factory.GetEntityPersister(entityName).IdentifierType;
			// create the collection holding the orphans
			List<object> res = new List<object>();
			// collect EntityIdentifier(s) of the *current* elements - add them into a HashSet for fast access
			var currentIds = new HashSet<TypedValue>();
			foreach (object current in currentElements)
			{
				if (current != null && await (ForeignKeys.IsNotTransientAsync(entityName, current, null, session)))
				{
					object currentId = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, current, session));
					currentIds.Add(new TypedValue(idType, currentId, session.EntityMode));
				}
			}

			// iterate over the *old* list
			foreach (object old in oldElements)
			{
				object oldId = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, old, session));
				if (!currentIds.Contains(new TypedValue(idType, oldId, session.EntityMode)))
				{
					res.Add(old);
				}
			}

			return res;
		}

		public async Task IdentityRemoveAsync(IList list, object obj, string entityName, ISessionImplementor session)
		{
			if (obj != null && await (ForeignKeys.IsNotTransientAsync(entityName, obj, null, session)))
			{
				IType idType = session.Factory.GetEntityPersister(entityName).IdentifierType;
				object idOfCurrent = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, obj, session));
				List<object> toRemove = new List<object>(list.Count);
				foreach (object current in list)
				{
					if (current == null)
					{
						continue;
					}

					object idOfOld = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, current, session));
					if (idType.IsEqual(idOfCurrent, idOfOld, session.EntityMode, session.Factory))
					{
						toRemove.Add(current);
					}
				}

				foreach (object ro in toRemove)
				{
					list.Remove(ro);
				}
			}
		}

		/// <summary>
		/// Disassemble the collection, ready for the cache
		/// </summary>
		/// <param name = "persister"></param>
		/// <returns></returns>
		public abstract Task<object> DisassembleAsync(ICollectionPersister persister);
		/// <summary>
		/// Get all the elements that need deleting
		/// </summary>
		public abstract Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula);
		public abstract Task<bool> EqualsSnapshotAsync(ICollectionPersister persister);
		/// <summary>
		/// Read the state of the collection from a disassembled cached value.
		/// </summary>
		/// <param name = "persister"></param>
		/// <param name = "disassembled"></param>
		/// <param name = "owner"></param>
		public abstract Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner);
		/// <summary>
		/// Do we need to update this element?
		/// </summary>
		/// <param name = "entry"></param>
		/// <param name = "i"></param>
		/// <param name = "elemType"></param>
		/// <returns></returns>
		public abstract Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType);
		/// <summary>
		/// Reads the row from the <see cref = "DbDataReader"/>.
		/// </summary>
		/// <param name = "reader">The DbDataReader that contains the value of the Identifier</param>
		/// <param name = "role">The persister for this Collection.</param>
		/// <param name = "descriptor">The descriptor providing result set column names</param>
		/// <param name = "owner">The owner of this Collection.</param>
		/// <returns>The object that was contained in the row.</returns>
		public abstract Task<object> ReadFromAsync(DbDataReader reader, ICollectionPersister role, ICollectionAliases descriptor, object owner);
		/// <summary>
		/// Do we need to insert this element?
		/// </summary>
		/// <param name = "entry"></param>
		/// <param name = "i"></param>
		/// <param name = "elemType"></param>
		/// <returns></returns>
		public abstract Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType);
	/*
		 * These were needed by Hibernate because Java's collections provide methods
		 * to get sublists, modify a collection with an iterator - all things that 
		 * Hibernate needs to be made aware of.  If .net changes their collection interfaces
		 * then we can readd these back in.
		 */
	}
}
#endif
