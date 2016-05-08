using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Cfg;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Id.Insert;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using Array = NHibernate.Mapping.Array;
using System.Threading.Tasks;

namespace NHibernate.Persister.Collection
{
	/// <summary>
	/// Summary description for AbstractCollectionPersister.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractCollectionPersister : ICollectionMetadata, ISqlLoadableCollection, IPostInsertIdentityPersister
	{
		public async Task<int> GetSizeAsync(object key, ISessionImplementor session)
		{
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					if (session.EnabledFilters.Count > 0)
					{
					}

					IDbCommand st = session.Batcher.PrepareCommand(CommandType.Text, GenerateSelectSizeString(session), KeyType.SqlTypes(factory));
					IDataReader rs = null;
					try
					{
						KeyType.NullSafeSet(st, key, 0, session);
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						return rs.Read() ? Convert.ToInt32(rs.GetValue(0)) - baseIndex : 0;
					}
					finally
					{
						session.Batcher.CloseCommand(st, rs);
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, sqle, "could not retrieve collection size: " + MessageHelper.CollectionInfoString(this, key, Factory), GenerateSelectSizeString(session));
				}
		}

		protected async Task<object> PerformInsertAsync(object ownerId, IPersistentCollection collection, object entry, int index, ISessionImplementor session)
		{
			IBinder binder = new GeneratedIdentifierBinder(ownerId, collection, entry, index, session, this);
			return await (identityDelegate.PerformInsertAsync(SqlInsertRowString, session, binder));
		}

		public virtual async Task<object> GetElementByIndexAsync(object key, object index, ISessionImplementor session, object owner)
		{
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					List<SqlType> sqlTl = new List<SqlType>(KeyType.SqlTypes(factory));
					sqlTl.AddRange(IndexType.SqlTypes(factory));
					IDbCommand st = session.Batcher.PrepareCommand(CommandType.Text, sqlSelectRowByIndexString, sqlTl.ToArray());
					IDataReader rs = null;
					try
					{
						KeyType.NullSafeSet(st, key, 0, session);
						IndexType.NullSafeSet(st, IncrementIndexByBase(index), keyColumnNames.Length, session);
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						try
						{
							if (rs.Read())
							{
								return ElementType.NullSafeGet(rs, elementColumnAliases, session, owner);
							}
							else
							{
								return NotFoundObject;
							}
						}
						finally
						{
							rs.Close();
						}
					}
					finally
					{
						session.Batcher.CloseCommand(st, rs);
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, sqle, "could not read row: " + MessageHelper.CollectionInfoString(this, key, Factory), GenerateSelectSizeString(session));
				}
		}

		private async Task<bool> ExistsAsync(object key, object indexOrElement, IType indexOrElementType, SqlString sql, ISessionImplementor session)
		{
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					List<SqlType> sqlTl = new List<SqlType>(KeyType.SqlTypes(factory));
					sqlTl.AddRange(indexOrElementType.SqlTypes(factory));
					IDbCommand st = session.Batcher.PrepareCommand(CommandType.Text, sql, sqlTl.ToArray());
					IDataReader rs = null;
					try
					{
						KeyType.NullSafeSet(st, key, 0, session);
						indexOrElementType.NullSafeSet(st, indexOrElement, keyColumnNames.Length, session);
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						try
						{
							return rs.Read();
						}
						finally
						{
							rs.Close();
						}
					}
					catch (TransientObjectException)
					{
						return false;
					}
					finally
					{
						session.Batcher.CloseCommand(st, rs);
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, sqle, "could not check row existence: " + MessageHelper.CollectionInfoString(this, key, Factory), GenerateSelectSizeString(session));
				}
		}
	}
}