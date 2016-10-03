#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.DomainModel
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomPersister : IEntityPersister
	{
		public Task<int[]> FindDirtyAsync(object[] currentState, object[] previousState, object entity, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<int[]>(FindDirty(currentState, previousState, entity, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int[]>(ex);
			}
		}

		public Task<int[]> FindModifiedAsync(object[] old, object[] current, object entity, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<int[]>(FindModified(old, current, entity, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int[]>(ex);
			}
		}

		public Task<object[]> GetNaturalIdentifierSnapshotAsync(object id, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object[]>(GetNaturalIdentifierSnapshot(id, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object[]>(ex);
			}
		}

		public async Task<object> LoadAsync(object id, object optionalObject, LockMode lockMode, ISessionImplementor session)
		{
			// fails when optional object is supplied
			Custom clone = null;
			Custom obj = (Custom)Instances[id];
			if (obj != null)
			{
				clone = (Custom)obj.Clone();
				TwoPhaseLoad.AddUninitializedEntity(session.GenerateEntityKey(id, this), clone, this, LockMode.None, false, session);
				TwoPhaseLoad.PostHydrate(this, id, new String[]{obj.Name}, null, clone, LockMode.None, false, session);
				await (TwoPhaseLoad.InitializeEntityAsync(clone, false, session, new PreLoadEvent((IEventSource)session), new PostLoadEvent((IEventSource)session)));
			}

			return clone;
		}

		public Task LockAsync(object id, object version, object obj, LockMode lockMode, ISessionImplementor session)
		{
			try
			{
				Lock(id, version, obj, lockMode, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task InsertAsync(object id, object[] fields, object obj, ISessionImplementor session)
		{
			try
			{
				Insert(id, fields, obj, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> InsertAsync(object[] fields, object obj, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Insert(fields, obj, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task DeleteAsync(object id, object version, object obj, ISessionImplementor session)
		{
			try
			{
				Delete(id, version, obj, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task UpdateAsync(object id, object[] fields, int[] dirtyFields, bool hasDirtyCollection, object[] oldFields, object oldVersion, object obj, object rowId, ISessionImplementor session)
		{
			try
			{
				Update(id, fields, dirtyFields, hasDirtyCollection, oldFields, oldVersion, obj, rowId, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object[]> GetDatabaseSnapshotAsync(object id, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object[]>(GetDatabaseSnapshot(id, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object[]>(ex);
			}
		}

		public Task<object> GetCurrentVersionAsync(object id, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(GetCurrentVersion(id, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> ForceVersionIncrementAsync(object id, object currentVersion, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(ForceVersionIncrement(id, currentVersion, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object[]> GetPropertyValuesToInsertAsync(object obj, IDictionary mergeMap, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object[]>(GetPropertyValuesToInsert(obj, mergeMap, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object[]>(ex);
			}
		}

		public Task ProcessInsertGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		}

		public Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
