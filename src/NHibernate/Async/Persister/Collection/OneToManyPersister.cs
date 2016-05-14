#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Loader.Entity;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Persister.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OneToManyPersister : AbstractCollectionPersister
	{
		protected override async Task<int> DoUpdateRowsAsync(object id, IPersistentCollection collection, ISessionImplementor session)
		{
			// we finish all the "removes" first to take care of possible unique 
			// constraints and so that we can take better advantage of batching
			try
			{
				const int offset = 0;
				int count = 0;
				if (RowDeleteEnabled)
				{
					IExpectation deleteExpectation = Expectations.AppropriateExpectation(DeleteCheckStyle);
					bool useBatch = deleteExpectation.CanBeBatched;
					SqlCommandInfo sql = SqlDeleteRowString;
					// update removed rows fks to null
					int i = 0;
					IEnumerable entries = collection.Entries(this);
					foreach (object entry in entries)
					{
						if (await (collection.NeedsUpdatingAsync(entry, i, ElementType)))
						{
							DbCommand st = null;
							// will still be issued when it used to be null
							if (useBatch)
							{
								st = session.Batcher.PrepareBatchCommand(SqlDeleteRowString.CommandType, sql.Text, SqlDeleteRowString.ParameterTypes);
							}
							else
							{
								st = session.Batcher.PrepareCommand(SqlDeleteRowString.CommandType, sql.Text, SqlDeleteRowString.ParameterTypes);
							}

							try
							{
								int loc = await (WriteKeyAsync(st, id, offset, session));
								await (WriteElementToWhereAsync(st, collection.GetSnapshotElement(entry, i), loc, session));
								if (useBatch)
								{
									session.Batcher.AddToBatch(deleteExpectation);
								}
								else
								{
									deleteExpectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
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
								if (!useBatch && st != null)
								{
									session.Batcher.CloseCommand(st, null);
								}
							}

							count++;
						}

						i++;
					}
				}

				if (RowInsertEnabled)
				{
					IExpectation insertExpectation = Expectations.AppropriateExpectation(InsertCheckStyle);
					//bool callable = InsertCallable;
					bool useBatch = insertExpectation.CanBeBatched;
					SqlCommandInfo sql = SqlInsertRowString;
					// now update all changed or added rows fks
					int i = 0;
					IEnumerable entries = collection.Entries(this);
					foreach (object entry in entries)
					{
						if (await (collection.NeedsUpdatingAsync(entry, i, ElementType)))
						{
							DbCommand st = null;
							if (useBatch)
							{
								st = session.Batcher.PrepareBatchCommand(SqlInsertRowString.CommandType, sql.Text, SqlInsertRowString.ParameterTypes);
							}
							else
							{
								st = session.Batcher.PrepareCommand(SqlInsertRowString.CommandType, sql.Text, SqlInsertRowString.ParameterTypes);
							}

							try
							{
								//offset += insertExpectation.Prepare(st, Factory.ConnectionProvider.Driver);
								int loc = await (WriteKeyAsync(st, id, offset, session));
								if (HasIndex && !indexContainsFormula)
								{
									loc = await (WriteIndexToWhereAsync(st, collection.GetIndex(entry, i, this), loc, session));
								}

								await (WriteElementToWhereAsync(st, collection.GetElement(entry), loc, session));
								if (useBatch)
								{
									session.Batcher.AddToBatch(insertExpectation);
								}
								else
								{
									insertExpectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(st), st);
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
								if (!useBatch && st != null)
								{
									session.Batcher.CloseCommand(st, null);
								}
							}

							count++;
						}

						i++;
					}
				}

				return count;
			}
			catch (DbException sqle)
			{
				throw ADOExceptionHelper.Convert(SQLExceptionConverter, sqle, "could not update collection rows: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)));
			}
		}

		public override async Task<object> GetElementByIndexAsync(object key, object index, ISessionImplementor session, object owner)
		{
			return await (new CollectionElementLoader(this, Factory, session.EnabledFilters).LoadElementAsync(session, key, IncrementIndexByBase(index))) ?? NotFoundObject;
		}
	}
}
#endif
