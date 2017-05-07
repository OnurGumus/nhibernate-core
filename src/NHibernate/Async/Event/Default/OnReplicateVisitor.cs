﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHibernate.Event.Default
{
	using System.Threading.Tasks;
	using System;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class OnReplicateVisitor : ReattachVisitor
	{

		internal override Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			try
			{
				if (collection == CollectionType.UnfetchedCollection)
				{
					return Task.FromResult<object>(null);
				}

				IEventSource session = Session;
				ICollectionPersister persister = session.Factory.GetCollectionPersister(type.Role);
				if (isUpdate)
				{
					RemoveCollection(persister, ExtractCollectionKeyFromOwner(persister), session);
				}

				IPersistentCollection wrapper = collection as IPersistentCollection;
				if (wrapper != null)
				{
					wrapper.SetCurrentSession(session);
					if (wrapper.WasInitialized)
					{
						session.PersistenceContext.AddNewCollection(persister, wrapper);
					}
					else
					{
						ReattachCollection(wrapper, type);
					}
				}
				else
				{
				// otherwise a null or brand new collection
				// this will also (inefficiently) handle arrays, which
				// have no snapshot, so we can't do any better
				//processArrayOrNewCollection(collection, type);
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