#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Persister.Collection
{
	/// <summary>
	/// Collection persister for collections of values and many-to-many associations.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicCollectionPersister : AbstractCollectionPersister
	{
		protected override async Task<int> DoUpdateRowsAsync(object id, IPersistentCollection collection, ISessionImplementor session)
		{
			if (ArrayHelper.IsAllFalse(elementColumnIsSettable))
				return 0;
			try
			{
				DbCommand st = null;
				IExpectation expectation = Expectations.AppropriateExpectation(UpdateCheckStyle);
				//bool callable = UpdateCallable;
				bool useBatch = expectation.CanBeBatched;
				IEnumerable entries = collection.Entries(this);
				int i = 0;
				int count = 0;
				foreach (object entry in entries)
				{
					if (await (collection.NeedsUpdatingAsync(entry, i, ElementType)))
					{
						int offset = 0;
						if (useBatch)
						{
							if (st == null)
							{
								st = session.Batcher.PrepareBatchCommand(SqlUpdateRowString.CommandType, SqlUpdateRowString.Text, SqlUpdateRowString.ParameterTypes);
							}
						}
						else
						{
							st = session.Batcher.PrepareCommand(SqlUpdateRowString.CommandType, SqlUpdateRowString.Text, SqlUpdateRowString.ParameterTypes);
						}

						try
						{
							//offset += expectation.Prepare(st, Factory.ConnectionProvider.Driver);
							int loc = await (WriteElementAsync(st, collection.GetElement(entry), offset, session));
							if (hasIdentifier)
							{
								await (WriteIdentifierAsync(st, collection.GetIdentifier(entry, i), loc, session));
							}
							else
							{
								loc = await (WriteKeyAsync(st, id, loc, session));
								if (HasIndex && !indexContainsFormula)
								{
									await (WriteIndexToWhereAsync(st, collection.GetIndex(entry, i, this), loc, session));
								}
								else
								{
									await (WriteElementToWhereAsync(st, collection.GetSnapshotElement(entry, i), loc, session));
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

						count++;
					}

					i++;
				}

				return count;
			}
			catch (DbException sqle)
			{
				throw ADOExceptionHelper.Convert(SQLExceptionConverter, sqle, "could not update collection rows: " + await (MessageHelper.CollectionInfoStringAsync(this, collection, id, session)), SqlUpdateRowString.Text);
			}
		}
	}
}
#endif
