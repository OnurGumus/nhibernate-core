﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHibernate.Event.Default
{
	using System.Threading.Tasks;
	using System;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class OnLockVisitor : ReattachVisitor
	{

		internal override Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			try
			{
				ISessionImplementor session = Session;
				ICollectionPersister persister = session.Factory.GetCollectionPersister(type.Role);
				if (collection == null)
				{
				//do nothing
				}
				else
				{
					IPersistentCollection persistentCollection = collection as IPersistentCollection;
					if (persistentCollection != null)
					{
						if (persistentCollection.SetCurrentSession(session))
						{
							if (IsOwnerUnchanged(persistentCollection, persister, ExtractCollectionKeyFromOwner(persister)))
							{
								// a "detached" collection that originally belonged to the same entity
								if (persistentCollection.IsDirty)
								{
									return Task.FromException<object>(new HibernateException("reassociated object has dirty collection: " + persistentCollection.Role));
								}

								ReattachCollection(persistentCollection, type);
							}
							else
							{
								// a "detached" collection that belonged to a different entity
								return Task.FromException<object>(new HibernateException("reassociated object has dirty collection reference: " + persistentCollection.Role));
							}
						}
						else
						{
							// a collection loaded in the current session
							// can not possibly be the collection belonging
							// to the entity passed to update()
							return Task.FromException<object>(new HibernateException("reassociated object has dirty collection reference: " + persistentCollection.Role));
						}
					}
					else
					{
						// brand new collection
						//TODO: or an array!! we can't lock objects with arrays now??
						return Task.FromException<object>(new HibernateException("reassociated object has dirty collection reference (or an array)"));
					}
				}

				return Task.FromResult<object>(null);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}
	}
}
