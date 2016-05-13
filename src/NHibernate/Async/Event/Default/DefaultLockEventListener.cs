using System;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultLockEventListener : AbstractLockUpgradeEventListener, ILockEventListener
	{
		/// <summary>Handle the given lock event. </summary>
		/// <param name = "event">The lock event to be handled.</param>
		public virtual async Task OnLockAsync(LockEvent @event)
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

			object entity = await (source.PersistenceContext.UnproxyAndReassociateAsync(@event.Entity));
			//TODO: if object was an uninitialized proxy, this is inefficient,resulting in two SQL selects
			EntityEntry entry = source.PersistenceContext.GetEntry(entity);
			if (entry == null)
			{
				IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
				object id = await (persister.GetIdentifierAsync(entity, source.EntityMode));
				if (!await (ForeignKeys.IsNotTransientAsync(@event.EntityName, entity, false, source)))
				{
					throw new TransientObjectException("cannot lock an unsaved transient instance: " + persister.EntityName);
				}

				entry = await (ReassociateAsync(@event, entity, id, persister));
				await (CascadeOnLockAsync(@event, persister, entity));
			}

			await (UpgradeLockAsync(entity, entry, @event.LockMode, source));
		}

		private async Task CascadeOnLockAsync(LockEvent @event, IEntityPersister persister, object entity)
		{
			IEventSource source = @event.Session;
			source.PersistenceContext.IncrementCascadeLevel();
			try
			{
				await (new Cascade(CascadingAction.Lock, CascadePoint.AfterLock, source).CascadeOnAsync(persister, entity, @event.LockMode));
			}
			finally
			{
				source.PersistenceContext.DecrementCascadeLevel();
			}
		}
	}
}