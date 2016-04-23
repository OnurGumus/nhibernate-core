using System;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Defines the default lock event listeners used by hibernate to lock entities
	/// in response to generated lock events. 
	/// </summary>
	[Serializable]
	public class DefaultLockEventListener : AbstractLockUpgradeEventListener, ILockEventListener
	{
		/// <summary>Handle the given lock event. </summary>
		/// <param name="event">The lock event to be handled.</param>
		public virtual async Task OnLock(LockEvent @event)
		{
			if (@event.Entity == null)
			{
				throw new NullReferenceException("attempted to lock null");
			}

			if (@event.LockMode == LockMode.Write)
			{
				throw new HibernateException("Invalid lock mode for lock()");
			}

			ISessionImplementor source = @event.Session;

			if (@event.LockMode == LockMode.None && source.PersistenceContext.ReassociateIfUninitializedProxy(@event.Entity))
			{
				// NH-specific: shortcut for uninitialized proxies - reassociate
				// without initialization
				return;
			}

			object entity = source.PersistenceContext.UnproxyAndReassociate(@event.Entity);
			//TODO: if object was an uninitialized proxy, this is inefficient,resulting in two SQL selects

			EntityEntry entry = source.PersistenceContext.GetEntry(entity);
			if (entry == null)
			{
				IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
				object id = persister.GetIdentifier(entity, source.EntityMode);
				if (!await ForeignKeys.IsNotTransient(@event.EntityName, entity, false, source).ConfigureAwait(false))
				{
					throw new TransientObjectException("cannot lock an unsaved transient instance: " + persister.EntityName);
				}

				entry = await Reassociate(@event, entity, id, persister);

				await CascadeOnLock(@event, persister, entity).ConfigureAwait(false);
			}

			await UpgradeLock(entity, entry, @event.LockMode, source).ConfigureAwait(false);
		}

		private async Task CascadeOnLock(LockEvent @event, IEntityPersister persister, object entity)
		{
			IEventSource source = @event.Session;
			source.PersistenceContext.IncrementCascadeLevel();
			try
			{
				await new Cascade(CascadingAction.Lock, CascadePoint.AfterLock, source).CascadeOn(persister, entity, @event.LockMode).ConfigureAwait(false);
			}
			finally
			{
				source.PersistenceContext.DecrementCascadeLevel();
			}
		}
	}
}
