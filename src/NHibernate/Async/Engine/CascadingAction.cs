using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	/// <summary>
	/// A session action that may be cascaded from parent entity to its children
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class CascadingAction
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class MergeCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to merge: " + entityName);
				}

				await (session.MergeAsync(entityName, child, (IDictionary)anything));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class EvictCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to evict: " + entityName);
				}

				await (session.EvictAsync(child));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class ReplicateCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to replicate: " + entityName);
				}

				await (session.ReplicateAsync(entityName, child, (ReplicationMode)anything));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class DeleteCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to delete: " + entityName);
				}

				await (session.DeleteAsync(entityName, child, isCascadeDeleteEnabled, (ISet<object>)anything));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class PersistCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to persist: " + entityName);
				}

				await (session.PersistAsync(entityName, child, (IDictionary)anything));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class PersistOnFlushCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to persistOnFlush: " + entityName);
				}

				await (session.PersistOnFlushAsync(entityName, child, (IDictionary)anything));
			}

			public override async Task NoCascadeAsync(IEventSource session, object child, object parent, IEntityPersister persister, int propertyIndex)
			{
				if (child == null)
				{
					return;
				}

				IType type = persister.PropertyTypes[propertyIndex];
				if (type.IsEntityType)
				{
					string childEntityName = ((EntityType)type).GetAssociatedEntityName(session.Factory);
					if (!IsInManagedState(child, session) && !(child.IsProxy()) && await (ForeignKeys.IsTransientAsync(childEntityName, child, null, session)))
					{
						string parentEntiytName = persister.EntityName;
						string propertyName = persister.PropertyNames[propertyIndex];
						throw new TransientObjectException(string.Format("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave: {0}.{1} -> {2}", parentEntiytName, propertyName, childEntityName));
					}
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class LockCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to lock: " + entityName);
				}

				await (session.LockAsync(entityName, child, LockMode.None));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class SaveUpdateCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to saveOrUpdate: " + entityName);
				}

				await (session.SaveOrUpdateAsync(entityName, child));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class RefreshCascadingAction : CascadingAction
		{
			public override async Task CascadeAsync(IEventSource session, object child, string entityName, object anything, bool isCascadeDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("cascading to refresh: " + entityName);
				}

				await (session.RefreshAsync(child, (IDictionary)anything));
			}
		}
	}
}