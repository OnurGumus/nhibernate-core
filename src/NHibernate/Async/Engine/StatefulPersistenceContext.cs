#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using NHibernate.Collection;
using NHibernate.Engine.Loading;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatefulPersistenceContext : IPersistenceContext, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Get the current state of the entity as known to the underlying
		/// database, or null if there is no corresponding row
		/// </summary>
		public async Task<object[]> GetDatabaseSnapshotAsync(object id, IEntityPersister persister)
		{
			EntityKey key = session.GenerateEntityKey(id, persister);
			object cached;
			if (entitySnapshotsByKey.TryGetValue(key, out cached))
			{
				return cached == NoRow ? null : (object[])cached;
			}
			else
			{
				object[] snapshot = await (persister.GetDatabaseSnapshotAsync(id, session));
				entitySnapshotsByKey[key] = snapshot ?? NoRow;
				return snapshot;
			}
		}

		/// <summary>
		/// Get the values of the natural id fields as known to the underlying
		/// database, or null if the entity has no natural id or there is no
		/// corresponding row.
		/// </summary>
		public async Task<object[]> GetNaturalIdSnapshotAsync(object id, IEntityPersister persister)
		{
			if (!persister.HasNaturalIdentifier)
			{
				return null;
			}

			// if the natural-id is marked as non-mutable, it is not retrieved during a
			// normal database-snapshot operation...
			int[] props = persister.NaturalIdentifierProperties;
			bool[] updateable = persister.PropertyUpdateability;
			bool allNatualIdPropsAreUpdateable = true;
			for (int i = 0; i < props.Length; i++)
			{
				if (!updateable[props[i]])
				{
					allNatualIdPropsAreUpdateable = false;
					break;
				}
			}

			if (allNatualIdPropsAreUpdateable)
			{
				// do this when all the properties are updateable since there is
				// a certain likelihood that the information will already be
				// snapshot-cached.
				object[] entitySnapshot = await (GetDatabaseSnapshotAsync(id, persister));
				if (entitySnapshot == NoRow)
				{
					return null;
				}

				object[] naturalIdSnapshot = new object[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					naturalIdSnapshot[i] = entitySnapshot[props[i]];
				}

				return naturalIdSnapshot;
			}
			else
			{
				return await (persister.GetNaturalIdentifierSnapshotAsync(id, session));
			}
		}

		/// <summary>
		/// Get the entity instance underlying the given proxy, throwing
		/// an exception if the proxy is uninitialized. If the given object
		/// is not a proxy, simply return the argument.
		/// </summary>
		public async Task<object> UnproxyAsync(object maybeProxy)
		{
			// TODO H3.2 Not ported
			//ElementWrapper wrapper = maybeProxy as ElementWrapper;
			//if (wrapper != null)
			//{
			//  maybeProxy = wrapper.Element;
			//}
			if (maybeProxy.IsProxy())
			{
				INHibernateProxy proxy = maybeProxy as INHibernateProxy;
				ILazyInitializer li = proxy.HibernateLazyInitializer;
				if (li.IsUninitialized)
					throw new PersistentObjectException("object was an uninitialized proxy for " + li.PersistentClass.FullName);
				return await (li.GetImplementationAsync()); // unwrap the object
			}
			else
			{
				return maybeProxy;
			}
		}

		/// <summary>
		/// Possibly unproxy the given reference and reassociate it with the current session.
		/// </summary>
		/// <param name = "maybeProxy">The reference to be unproxied if it currently represents a proxy. </param>
		/// <returns> The unproxied instance. </returns>
		public async Task<object> UnproxyAndReassociateAsync(object maybeProxy)
		{
			// TODO H3.2 Not ported
			//ElementWrapper wrapper = maybeProxy as ElementWrapper;
			//if (wrapper != null)
			//{
			//  maybeProxy = wrapper.Element;
			//}
			if (maybeProxy.IsProxy())
			{
				var proxy = maybeProxy as INHibernateProxy;
				ILazyInitializer li = proxy.HibernateLazyInitializer;
				ReassociateProxy(li, proxy);
				return await (li.GetImplementationAsync()); //initialize + unwrap the object
			}

			return maybeProxy;
		}

		/// <summary>
		/// Force initialization of all non-lazy collections encountered during
		/// the current two-phase load (actually, this is a no-op, unless this
		/// is the "outermost" load)
		/// </summary>
		public async Task InitializeNonLazyCollectionsAsync()
		{
			if (loadCounter == 0)
			{
				log.Debug("initializing non-lazy collections");
				//do this work only at the very highest level of the load
				loadCounter++; //don't let this method be called recursively
				try
				{
					while (nonlazyCollections.Count > 0)
					{
						//note that each iteration of the loop may add new elements
						IPersistentCollection tempObject = nonlazyCollections[nonlazyCollections.Count - 1];
						nonlazyCollections.RemoveAt(nonlazyCollections.Count - 1);
						await (tempObject.ForceInitializationAsync());
					}
				}
				finally
				{
					loadCounter--;
					ClearNullProperties();
				}
			}
		}

		/// <summary>
		/// Search the persistence context for an owner for the child object,
		/// given a collection role
		/// </summary>
		public async Task<object> GetOwnerIdAsync(string entityName, string propertyName, object childEntity, IDictionary mergeMap)
		{
			string collectionRole = entityName + '.' + propertyName;
			IEntityPersister persister = session.Factory.GetEntityPersister(entityName);
			ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(collectionRole);
			object parent = parentsByChild[childEntity];
			if (parent != null)
			{
				var entityEntry = (EntityEntry)entityEntries[parent];
				//there maybe more than one parent, filter by type
				if (persister.IsSubclassEntityName(entityEntry.EntityName) && await (IsFoundInParentAsync(propertyName, childEntity, persister, collectionPersister, parent)))
				{
					return GetEntry(parent).Id;
				}

				parentsByChild.Remove(childEntity); // remove wrong entry
			}

			// iterate all the entities currently associated with the persistence context.
			foreach (DictionaryEntry entry in entityEntries)
			{
				var entityEntry = (EntityEntry)entry.Value;
				// does this entity entry pertain to the entity persister in which we are interested (owner)?
				if (persister.IsSubclassEntityName(entityEntry.EntityName))
				{
					object entityEntryInstance = entry.Key;
					//check if the managed object is the parent
					bool found = await (IsFoundInParentAsync(propertyName, childEntity, persister, collectionPersister, entityEntryInstance));
					if (!found && mergeMap != null)
					{
						//check if the detached object being merged is the parent
						object unmergedInstance = mergeMap[entityEntryInstance];
						object unmergedChild = mergeMap[childEntity];
						if (unmergedInstance != null && unmergedChild != null)
						{
							found = await (IsFoundInParentAsync(propertyName, unmergedChild, persister, collectionPersister, unmergedInstance));
						}
					}

					if (found)
					{
						return entityEntry.Id;
					}
				}
			}

			// if we get here, it is possible that we have a proxy 'in the way' of the merge map resolution...
			// 		NOTE: decided to put this here rather than in the above loop as I was nervous about the performance
			//		of the loop-in-loop especially considering this is far more likely the 'edge case'
			if (mergeMap != null)
			{
				foreach (DictionaryEntry mergeMapEntry in mergeMap)
				{
					var proxy = mergeMapEntry.Key as INHibernateProxy;
					if (proxy != null)
					{
						if (persister.IsSubclassEntityName(proxy.HibernateLazyInitializer.EntityName))
						{
							bool found = await (IsFoundInParentAsync(propertyName, childEntity, persister, collectionPersister, mergeMap[proxy]));
							if (!found)
							{
								found = await (IsFoundInParentAsync(propertyName, mergeMap[childEntity], persister, collectionPersister, mergeMap[proxy]));
							}

							if (found)
							{
								return proxy.HibernateLazyInitializer.Identifier;
							}
						}
					}
				}
			}

			return null;
		}

		private async Task<bool> IsFoundInParentAsync(string property, object childEntity, IEntityPersister persister, ICollectionPersister collectionPersister, object potentialParent)
		{
			object collection = persister.GetPropertyValue(potentialParent, property, session.EntityMode);
			return collection != null && NHibernateUtil.IsInitialized(collection) && await (collectionPersister.CollectionType.ContainsAsync(collection, childEntity, session));
		}

		/// <inheritdoc/>
		public async Task SetReadOnlyAsync(object entityOrProxy, bool readOnly)
		{
			if (entityOrProxy == null)
				throw new ArgumentNullException("entityOrProxy");
			if (IsReadOnly(entityOrProxy) == readOnly)
				return;
			if (entityOrProxy is INHibernateProxy)
			{
				INHibernateProxy proxy = (INHibernateProxy)entityOrProxy;
				SetProxyReadOnly(proxy, readOnly);
				if (NHibernateUtil.IsInitialized(proxy))
				{
					SetEntityReadOnly(await (proxy.HibernateLazyInitializer.GetImplementationAsync()), readOnly);
				}
			}
			else
			{
				SetEntityReadOnly(entityOrProxy, readOnly);
				// PersistenceContext.proxyFor( entity ) returns entity if there is no proxy for that entity
				// so need to check the return value to be sure it is really a proxy
				object maybeProxy = this.Session.PersistenceContext.ProxyFor(entityOrProxy);
				if (maybeProxy is INHibernateProxy)
				{
					SetProxyReadOnly((INHibernateProxy)maybeProxy, readOnly);
				}
			}
		}
	}
}
#endif
