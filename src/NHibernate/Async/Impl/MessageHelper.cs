using System;
using System.Text;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	/// <summary>
	/// Helper methods for rendering log messages and exception messages
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class MessageHelper
	{
		private static async Task AddIdToCollectionInfoStringAsync(ICollectionPersister persister, object id, ISessionFactoryImplementor factory, StringBuilder s)
		{
			// Need to use the identifier type of the collection owner
			// since the incoming is value is actually the owner's id.
			// Using the collection's key type causes problems with
			// property-ref keys.
			// Also need to check that the expected identifier type matches
			// the given ID.  Due to property-ref keys, the collection key
			// may not be the owner key.
			IType ownerIdentifierType = persister.OwnerEntityPersister.IdentifierType;
			if (id.GetType().IsAssignableFrom(ownerIdentifierType.ReturnedClass))
			{
				s.Append(await (ownerIdentifierType.ToLoggableStringAsync(id, factory)));
			}
			else
			{
				// TODO: This is a crappy backup if a property-ref is used.
				// If the reference is an object w/o toString(), this isn't going to work.
				s.Append(id.ToString());
			}
		}

		internal static async Task<string> CollectionInfoStringAsync(ICollectionPersister persister, object id, ISessionFactoryImplementor factory)
		{
			StringBuilder s = new StringBuilder();
			s.Append("[");
			if (persister == null)
			{
				s.Append("<unreferenced>");
			}
			else
			{
				s.Append(persister.Role);
				s.Append("#");
				if (id == null)
				{
					s.Append("<null>");
				}
				else
				{
					await (AddIdToCollectionInfoStringAsync(persister, id, factory, s));
				}
			}

			s.Append("]");
			return s.ToString();
		}

		public static async Task<string> InfoStringAsync(ICollectionPersister persister, object id, ISessionFactoryImplementor factory)
		{
			return await (CollectionInfoStringAsync(persister, id, factory));
		}

		internal static async Task<String> CollectionInfoStringAsync(ICollectionPersister persister, IPersistentCollection collection, object collectionKey, ISessionImplementor session)
		{
			StringBuilder s = new StringBuilder();
			s.Append("[");
			if (persister == null)
			{
				s.Append("<unreferenced>");
			}
			else
			{
				s.Append(persister.Role);
				s.Append("#");
				IType ownerIdentifierType = persister.OwnerEntityPersister.IdentifierType;
				object ownerKey;
				// TODO: Is it redundant to attempt to use the collectionKey,
				// or is always using the owner id sufficient?
				if (collectionKey.GetType().IsAssignableFrom(ownerIdentifierType.ReturnedClass))
				{
					ownerKey = collectionKey;
				}
				else
				{
					ownerKey = session.PersistenceContext.GetEntry(collection.Owner).Id;
				}

				s.Append(await (ownerIdentifierType.ToLoggableStringAsync(ownerKey, session.Factory)));
			}

			s.Append("]");
			return s.ToString();
		}

		public static async Task<string> InfoStringAsync(IEntityPersister persister, object id, IType identifierType, ISessionFactoryImplementor factory)
		{
			StringBuilder s = new StringBuilder();
			s.Append('[');
			if (persister == null)
			{
				s.Append("<null Class>");
			}
			else
			{
				s.Append(persister.EntityName);
			}

			s.Append('#');
			if (id == null)
			{
				s.Append("<null>");
			}
			else
			{
				s.Append(await (identifierType.ToLoggableStringAsync(id, factory)));
			}

			s.Append(']');
			return s.ToString();
		}

		public static async Task<string> InfoStringAsync(IEntityPersister persister, object[] ids, ISessionFactoryImplementor factory)
		{
			StringBuilder s = new StringBuilder();
			s.Append('[');
			if (persister == null)
			{
				s.Append("<null IEntityPersister>");
			}
			else
			{
				s.Append(persister.EntityName).Append("#<");
				for (int i = 0; i < ids.Length; i++)
				{
					s.Append(await (persister.IdentifierType.ToLoggableStringAsync(ids[i], factory)));
					if (i < ids.Length - 1)
					{
						s.Append(StringHelper.CommaSpace);
					}

					s.Append('>');
				}
			}

			s.Append(']');
			return s.ToString();
		}
	}
}