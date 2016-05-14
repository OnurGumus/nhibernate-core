#if NET_4_5
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
		public async Task InitializeAsync(object key, ISessionImplementor session)
		{
			await (GetAppropriateInitializer(key, session).InitializeAsync(key, session));
		}

		/// <summary>
		/// Reads the Element from the IDataReader.  The IDataReader will probably only contain
		/// the id of the Element.
		/// </summary>
		/// <remarks>See ReadElementIdentifier for an explanation of why this method will be depreciated.</remarks>
		public Task<object> ReadElementAsync(IDataReader rs, object owner, string[] aliases, ISessionImplementor session)
		{
			return ElementType.NullSafeGetAsync(rs, aliases, session, owner);
		}

		public async Task<object> ReadIndexAsync(IDataReader rs, string[] aliases, ISessionImplementor session)
		{
			object index = await (IndexType.NullSafeGetAsync(rs, aliases, session, null));
			if (index == null)
			{
				throw new HibernateException("null index column for collection: " + role);
			}

			index = DecrementIndexByBase(index);
			return index;
		}

		public async Task<object> ReadIdentifierAsync(IDataReader rs, string alias, ISessionImplementor session)
		{
			object id = await (IdentifierType.NullSafeGetAsync(rs, alias, session, null));
			if (id == null)
			{
				throw new HibernateException("null identifier column for collection: " + role);
			}

			return id;
		}

		public Task<object> ReadKeyAsync(IDataReader dr, string[] aliases, ISessionImplementor session)
		{
			return KeyType.NullSafeGetAsync(dr, aliases, session, null);
		}

		protected async Task<int> WriteKeyAsync(IDbCommand st, object id, int i, ISessionImplementor session)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id", "Null key for collection: " + role);
			}

			await (KeyType.NullSafeSetAsync(st, id, i, session));
			return i + keyColumnAliases.Length;
		}

		protected async Task<int> WriteElementAsync(IDbCommand st, object elt, int i, ISessionImplementor session)
		{
			await (ElementType.NullSafeSetAsync(st, elt, i, elementColumnIsSettable, session));
			return i + ArrayHelper.CountTrue(elementColumnIsSettable);
		}

		protected async Task<int> WriteIndexAsync(IDbCommand st, object idx, int i, ISessionImplementor session)
		{
			await (IndexType.NullSafeSetAsync(st, IncrementIndexByBase(idx), i, indexColumnIsSettable, session));
			return i + ArrayHelper.CountTrue(indexColumnIsSettable);
		}

		protected async Task<int> WriteElementToWhereAsync(IDbCommand st, object elt, int i, ISessionImplementor session)
		{
			if (elementIsPureFormula)
			{
				throw new AssertionFailure("cannot use a formula-based element in the where condition");
			}

			await (ElementType.NullSafeSetAsync(st, elt, i, elementColumnIsInPrimaryKey, session));
			return i + elementColumnAliases.Length;
		}

		protected async Task<int> WriteIndexToWhereAsync(IDbCommand st, object index, int i, ISessionImplementor session)
		{
			if (indexContainsFormula)
			{
				throw new AssertionFailure("cannot use a formula-based index in the where condition");
			}

			await (IndexType.NullSafeSetAsync(st, IncrementIndexByBase(index), i, session));
			return i + indexColumnAliases.Length;
		}

		protected async Task<int> WriteIdentifierAsync(IDbCommand st, object idx, int i, ISessionImplementor session)
		{
			await (IdentifierType.NullSafeSetAsync(st, idx, i, session));
			return i + 1;
		}

		public async Task RemoveAsync(object id, ISessionImplementor session)
		{
			if (!isInverse && RowDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Deleting collection: " + await (MessageHelper.CollectionInfoStringAsync(this, id, Factory)));
				}

				// Remove all the old entries
				try
				{
					int offset = 0;
					IExpectation expectation = Expectations.AppropriateExpectation(DeleteAllCheckStyle);
					//bool callable = DeleteAllCallable;
					bool useBatch = expectation.CanBeBatched;
					IDbCommand st = useBatch ? session.Batcher.PrepareBatchCommand(SqlDeleteString.CommandType, SqlDeleteString.Text, SqlDeleteString.ParameterTypes) : session.Batcher.PrepareCommand(SqlDeleteString.CommandType, SqlDeleteString.Text, SqlDeleteString.ParameterTypes);
					try
					{
						await (WriteKeyAsync(st, id, offset, session));
						if (useBatch)
						{
							session.Batcher.AddToBatch(expectation);
						}
						else
						{
							expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
						}
					}
					catch (Exception e)
					{
						if (useBatch)
						{
							session.Batcher.AbortBatch(e);
						}

						throw;
					}
					finally
					{
						if (!useBatch)
						{
							session.Batcher.CloseCommand(st, null);
						}
					}

					if (log.IsDebugEnabled)
					{
						log.Debug("done deleting collection");
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(sqlExceptionConverter, sqle, "could not delete collection: " + MessageHelper.CollectionInfoString(this, id));
				}
			}
		}

		public async Task RecreateAsync(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse && RowInsertEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Inserting collection: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
				}

				try
				{
					IExpectation expectation = null;
					bool useBatch = false;
					int i = 0;
					int count = 0;
					// create all the new entries
					IEnumerator entries = collection.Entries(this).GetEnumerator();
					while (entries.MoveNext())
					{
						// Init, if we're on the first element.
						if (count == 0)
						{
							expectation = Expectations.AppropriateExpectation(insertCheckStyle);
							await (collection.PreInsertAsync(this));
							//bool callable = InsertCallable;
							useBatch = expectation.CanBeBatched;
						}

						object entry = entries.Current;
						if (collection.EntryExists(entry, i))
						{
							object entryId;
							if (!IsIdentifierAssignedByInsert)
							{
								// NH Different implementation: write once
								entryId = await (PerformInsertAsync(id, collection, expectation, entry, i, useBatch, false, session));
							}
							else
							{
								entryId = await (PerformInsertAsync(id, collection, entry, i, session));
							}

							collection.AfterRowInsert(this, entry, i, entryId);
							count++;
						}

						i++;
					}

					if (log.IsDebugEnabled)
					{
						if (count > 0)
							log.Debug(string.Format("done inserting collection: {0} rows inserted", count));
						else
							log.Debug("collection was empty");
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(sqlExceptionConverter, sqle, "could not insert collection: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
				}
			}
		}

		public async Task DeleteRowsAsync(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse && RowDeleteEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Deleting rows of collection: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
				}

				bool deleteByIndex = !IsOneToMany && hasIndex && !indexContainsFormula;
				try
				{
					// delete all the deleted entries
					IEnumerator deletes = (await (collection.GetDeletesAsync(this, !deleteByIndex))).GetEnumerator();
					if (deletes.MoveNext())
					{
						deletes.Reset();
						int offset = 0;
						int count = 0;
						while (deletes.MoveNext())
						{
							IDbCommand st;
							IExpectation expectation = Expectations.AppropriateExpectation(deleteCheckStyle);
							//bool callable = DeleteCallable;
							bool useBatch = expectation.CanBeBatched;
							if (useBatch)
							{
								st = session.Batcher.PrepareBatchCommand(SqlDeleteRowString.CommandType, SqlDeleteRowString.Text, SqlDeleteRowString.ParameterTypes);
							}
							else
							{
								st = session.Batcher.PrepareCommand(SqlDeleteRowString.CommandType, SqlDeleteRowString.Text, SqlDeleteRowString.ParameterTypes);
							}

							try
							{
								object entry = deletes.Current;
								int loc = offset;
								if (hasIdentifier)
								{
									await (WriteIdentifierAsync(st, entry, loc, session));
								}
								else
								{
									loc = await (WriteKeyAsync(st, id, loc, session));
									if (deleteByIndex)
									{
										await (WriteIndexToWhereAsync(st, entry, loc, session));
									}
									else
									{
										await (WriteElementToWhereAsync(st, entry, loc, session));
									}
								}

								if (useBatch)
								{
									session.Batcher.AddToBatch(expectation);
								}
								else
								{
									expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
								}

								count++;
							}
							catch (Exception e)
							{
								if (useBatch)
								{
									session.Batcher.AbortBatch(e);
								}

								throw;
							}
							finally
							{
								if (!useBatch)
								{
									session.Batcher.CloseCommand(st, null);
								}
							}
						}

						if (log.IsDebugEnabled)
						{
							log.Debug("done deleting collection rows: " + count + " deleted");
						}
					}
					else
					{
						if (log.IsDebugEnabled)
						{
							log.Debug("no rows to delete");
						}
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(sqlExceptionConverter, sqle, "could not delete collection rows: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
				}
			}
		}

		public async Task InsertRowsAsync(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse && RowInsertEnabled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Inserting rows of collection: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
				}

				try
				{
					// insert all the new entries
					await (collection.PreInsertAsync(this));
					IExpectation expectation = Expectations.AppropriateExpectation(insertCheckStyle);
					//bool callable = InsertCallable;
					bool useBatch = expectation.CanBeBatched;
					int i = 0;
					int count = 0;
					IEnumerable entries = collection.Entries(this);
					foreach (object entry in entries)
					{
						if (await (collection.NeedsInsertingAsync(entry, i, elementType)))
						{
							object entryId;
							if (!IsIdentifierAssignedByInsert)
							{
								// NH Different implementation: write once
								entryId = await (PerformInsertAsync(id, collection, expectation, entry, i, useBatch, false, session));
							}
							else
							{
								entryId = await (PerformInsertAsync(id, collection, entry, i, session));
							}

							collection.AfterRowInsert(this, entry, i, entryId);
							count++;
						}

						i++;
					}

					if (log.IsDebugEnabled)
					{
						log.Debug(string.Format("done inserting rows: {0} inserted", count));
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(sqlExceptionConverter, sqle, "could not insert collection rows: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
				}
			}
		}

		public async Task UpdateRowsAsync(IPersistentCollection collection, object id, ISessionImplementor session)
		{
			if (!isInverse && collection.RowUpdatePossible)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("Updating rows of collection: {0}#{1}", role, id));
				}

				// update all the modified entries
				int count = await (DoUpdateRowsAsync(id, collection, session));
				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("done updating rows: {0} updated", count));
				}
			}
		}

		protected abstract Task<int> DoUpdateRowsAsync(object key, IPersistentCollection collection, ISessionImplementor session);
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
						await (KeyType.NullSafeSetAsync(st, key, 0, session));
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
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, sqle, "could not retrieve collection size: " + await (MessageHelper.CollectionInfoStringAsync(this, key, Factory)), GenerateSelectSizeString(session));
				}
		}

		public async Task<bool> IndexExistsAsync(object key, object index, ISessionImplementor session)
		{
			return await (ExistsAsync(key, IncrementIndexByBase(index), IndexType, sqlDetectRowByIndexString, session));
		}

		public Task<bool> ElementExistsAsync(object key, object element, ISessionImplementor session)
		{
			return ExistsAsync(key, element, ElementType, sqlDetectRowByElementString, session);
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
						await (KeyType.NullSafeSetAsync(st, key, 0, session));
						await (indexOrElementType.NullSafeSetAsync(st, indexOrElement, keyColumnNames.Length, session));
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
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, sqle, "could not check row existence: " + await (MessageHelper.CollectionInfoStringAsync(this, key, Factory)), GenerateSelectSizeString(session));
				}
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
						await (KeyType.NullSafeSetAsync(st, key, 0, session));
						await (IndexType.NullSafeSetAsync(st, IncrementIndexByBase(index), keyColumnNames.Length, session));
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						try
						{
							if (rs.Read())
							{
								return await (ElementType.NullSafeGetAsync(rs, elementColumnAliases, session, owner));
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
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, sqle, "could not read row: " + await (MessageHelper.CollectionInfoStringAsync(this, key, Factory)), GenerateSelectSizeString(session));
				}
		}

		protected async Task<object> PerformInsertAsync(object ownerId, IPersistentCollection collection, IExpectation expectation, object entry, int index, bool useBatch, bool callable, ISessionImplementor session)
		{
			object entryId = null;
			int offset = 0;
			IDbCommand st = useBatch ? session.Batcher.PrepareBatchCommand(SqlInsertRowString.CommandType, SqlInsertRowString.Text, SqlInsertRowString.ParameterTypes) : session.Batcher.PrepareCommand(SqlInsertRowString.CommandType, SqlInsertRowString.Text, SqlInsertRowString.ParameterTypes);
			try
			{
				//offset += expectation.Prepare(st, factory.ConnectionProvider.Driver);
				offset = await (WriteKeyAsync(st, ownerId, offset, session));
				if (hasIdentifier)
				{
					entryId = collection.GetIdentifier(entry, index);
					offset = await (WriteIdentifierAsync(st, entryId, offset, session));
				}

				if (hasIndex)
				{
					offset = await (WriteIndexAsync(st, collection.GetIndex(entry, index, this), offset, session));
				}

				await (WriteElementAsync(st, collection.GetElement(entry), offset, session));
				if (useBatch)
				{
					session.Batcher.AddToBatch(expectation);
				}
				else
				{
					expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
				}
			}
			catch (Exception e)
			{
				if (useBatch)
				{
					session.Batcher.AbortBatch(e);
				}

				throw;
			}
			finally
			{
				if (!useBatch)
				{
					session.Batcher.CloseCommand(st, null);
				}
			}

			return entryId;
		}

		/// <summary>
		/// Perform an SQL INSERT, and then retrieve a generated identifier.
		/// </summary>
		/// <returns> the id of the collection entry </returns>
		/// <remarks>
		/// This form is used for PostInsertIdentifierGenerator-style ids (IDENTITY, select, etc).
		/// </remarks>
		protected async Task<object> PerformInsertAsync(object ownerId, IPersistentCollection collection, object entry, int index, ISessionImplementor session)
		{
			IBinder binder = new GeneratedIdentifierBinder(ownerId, collection, entry, index, session, this);
			return await (identityDelegate.PerformInsertAsync(SqlInsertRowString, session, binder));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		protected partial class GeneratedIdentifierBinder : IBinder
		{
			public async Task BindValuesAsync(IDbCommand cm)
			{
				int offset = 0;
				offset = await (persister.WriteKeyAsync(cm, ownerId, offset, session));
				if (persister.HasIndex)
				{
					offset = await (persister.WriteIndexAsync(cm, collection.GetIndex(entry, index, persister), offset, session));
				}

				await (persister.WriteElementAsync(cm, collection.GetElement(entry), offset, session));
			}
		}
	}
}
#endif
