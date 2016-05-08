using System;
using System.Collections;
using NHibernate.Action;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using Status = NHibernate.Engine.Status;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractSaveEventListener : AbstractReassociateEventListener
	{
		protected virtual async Task<object> SaveWithGeneratedIdAsync(object entity, string entityName, object anything, IEventSource source, bool requiresImmediateIdAccess)
		{
			IEntityPersister persister = source.GetEntityPersister(entityName, entity);
			object generatedId = await (persister.IdentifierGenerator.GenerateAsync(source, entity));
			if (generatedId == null)
			{
				throw new IdentifierGenerationException("null id generated for:" + entity.GetType());
			}
			else if (generatedId == IdentifierGeneratorFactory.ShortCircuitIndicator)
			{
				return source.GetIdentifier(entity);
			}
			else if (generatedId == IdentifierGeneratorFactory.PostInsertIndicator)
			{
				return PerformSave(entity, null, persister, true, anything, source, requiresImmediateIdAccess);
			}
			else
			{
				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("generated identifier: {0}, using strategy: {1}", persister.IdentifierType.ToLoggableString(generatedId, source.Factory), persister.IdentifierGenerator.GetType().FullName));
				}

				return PerformSave(entity, generatedId, persister, false, anything, source, true);
			}
		}
	}
}