
using System.Threading.Tasks;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Wrap collections in a Hibernate collection wrapper.
	/// </summary>
	public class WrapVisitor : ProxyVisitor
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(WrapVisitor));
		private bool substitute = false;

		public WrapVisitor(IEventSource session) : base(session) { }

		internal bool SubstitutionRequired
		{
			get { return substitute; }
		}

		internal override async Task Process(object obj, IEntityPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;
			object[] values = persister.GetPropertyValues(obj, entityMode);
			IType[] types = persister.PropertyTypes;
			await ProcessEntityPropertyValues(values, types).ConfigureAwait(false);
			if (SubstitutionRequired)
			{
				persister.SetPropertyValues(obj, values, entityMode);
			}
		}

		internal override Task<object> ProcessCollection(object collection, CollectionType collectionType)
		{
			IPersistentCollection coll = collection as IPersistentCollection;
			if (coll != null)
			{
				ISessionImplementor session = Session;
				if (coll.SetCurrentSession(session))
				{
					ReattachCollection(coll, collectionType);
				}
				return Task.FromResult<object>(null);
			}
			else
			{
				return Task.FromResult(ProcessArrayOrNewCollection(collection, collectionType));
			}
		}

		private object ProcessArrayOrNewCollection(object collection, CollectionType collectionType)
		{
			if (collection == null)
			{
				//do nothing
				return null;
			}

			ISessionImplementor session = Session;

			ICollectionPersister persister = session.Factory.GetCollectionPersister(collectionType.Role);
			
			IPersistenceContext persistenceContext = session.PersistenceContext;
			//TODO: move into collection type, so we can use polymorphism!

			if (collectionType.HasHolder(session.EntityMode))
			{
				if (collection == CollectionType.UnfetchedCollection)
					return null;

				IPersistentCollection ah = persistenceContext.GetCollectionHolder(collection);
				if (ah == null)
				{
					ah = collectionType.Wrap(session, collection);
					persistenceContext.AddNewCollection(persister, ah);
					persistenceContext.AddCollectionHolder(ah);
				}
				return null;
			}
			else
			{
				IPersistentCollection persistentCollection = collectionType.Wrap(session, collection);
				persistenceContext.AddNewCollection(persister, persistentCollection);

				if (log.IsDebugEnabled)
					log.Debug("Wrapped collection in role: " + collectionType.Role);

				return persistentCollection; //Force a substitution!
			}
		}

		internal override async Task ProcessValue(int i, object[] values, IType[] types)
		{
			object result = await ProcessValue(values[i], types[i]).ConfigureAwait(false);
			if (result != null)
			{
				substitute = true;
				values[i] = result;
			}
		}

		internal override async Task<object> ProcessComponent(object component, IAbstractComponentType componentType)
		{
			if (component != null)
			{
				object[] values = await componentType.GetPropertyValues(component, Session).ConfigureAwait(false);
				IType[] types = componentType.Subtypes;
				bool substituteComponent = false;
				for (int i = 0; i < types.Length; i++)
				{
					object result = await ProcessValue(values[i], types[i]).ConfigureAwait(false);
					if (result != null)
					{
						values[i] = result;
						substituteComponent = true;
					}
				}
				if (substituteComponent)
				{
					componentType.SetPropertyValues(component, values, Session.EntityMode);
				}
			}

			return null;
		}
	}
}
