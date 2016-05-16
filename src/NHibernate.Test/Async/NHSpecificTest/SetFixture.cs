#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	internal partial class CollectionPersisterStub : ICollectionPersister
	{
		public Task InsertRowsAsync(IPersistentCollection collection, object key, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// TODO:  Add CollectionPersisterStub.InsertRows implementation
		}

		public Task UpdateRowsAsync(IPersistentCollection collection, object key, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// TODO:  Add CollectionPersisterStub.UpdateRows implementation
		}

		public Task DeleteRowsAsync(IPersistentCollection collection, object key, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// TODO:  Add CollectionPersisterStub.DeleteRows implementation
		}

		public Task RecreateAsync(IPersistentCollection collection, object key, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// TODO:  Add CollectionPersisterStub.Recreate implementation
		}

		public Task RemoveAsync(object id, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// TODO:  Add CollectionPersisterStub.Remove implementation
		}

		public Task<object> ReadElementAsync(DbDataReader rs, object owner, string[] aliases, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(ReadElement(rs, owner, aliases, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> ReadIndexAsync(DbDataReader rs, string[] aliases, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(ReadIndex(rs, aliases, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task InitializeAsync(object key, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		// TODO:  Add CollectionPersisterStub.Initialize implementation
		}

		public Task<object> ReadKeyAsync(DbDataReader rs, string[] aliases, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(ReadKey(rs, aliases, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> ReadIdentifierAsync(DbDataReader rs, string alias, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(ReadIdentifier(rs, alias, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<int> GetSizeAsync(object key, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<int>(GetSize(key, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int>(ex);
			}
		}

		public Task<bool> IndexExistsAsync(object key, object index, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<bool>(IndexExists(key, index, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public Task<bool> ElementExistsAsync(object key, object element, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<bool>(ElementExists(key, element, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public Task<object> GetElementByIndexAsync(object key, object index, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(GetElementByIndex(key, index, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
