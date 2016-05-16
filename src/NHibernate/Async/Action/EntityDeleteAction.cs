#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class EntityDeleteAction : EntityAction
	{
		public override async Task ExecuteAsync()
		{
			object id = Id;
			IEntityPersister persister = Persister;
			ISessionImplementor session = Session;
			object instance = Instance;
			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			bool veto = PreDelete();
			object tmpVersion = version;
			if (persister.IsVersionPropertyGenerated)
			{
				// we need to grab the version value from the entity, otherwise
				// we have issues with generated-version entities that may have
				// multiple actions queued during the same flush
				tmpVersion = persister.GetVersion(instance, session.EntityMode);
			}

			CacheKey ck;
			if (persister.HasCache)
			{
				ck = session.GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
				sLock = persister.Cache.Lock(ck, version);
			}
			else
			{
				ck = null;
			}

			if (!isCascadeDeleteEnabled && !veto)
			{
				await (persister.DeleteAsync(id, tmpVersion, instance, session));
			}

			//postDelete:
			// After actually deleting a row, record the fact that the instance no longer 
			// exists on the database (needed for identity-column key generation), and
			// remove it from the session cache
			IPersistenceContext persistenceContext = session.PersistenceContext;
			EntityEntry entry = persistenceContext.RemoveEntry(instance);
			if (entry == null)
			{
				throw new AssertionFailure("Possible nonthreadsafe access to session");
			}

			entry.PostDelete();
			EntityKey key = session.GenerateEntityKey(entry.Id, entry.Persister);
			persistenceContext.RemoveEntity(key);
			persistenceContext.RemoveProxy(key);
			if (persister.HasCache)
				persister.Cache.Evict(ck);
			PostDelete();
			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.DeleteEntity(Persister.EntityName, stopwatch.Elapsed);
			}
		}
	}
}
#endif
