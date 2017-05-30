﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Loader.Entity;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Util;

namespace NHibernate.Persister.Collection
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class OneToManyPersister : AbstractCollectionPersister
	{

		protected override async Task<int> DoUpdateRowsAsync(object id, IPersistentCollection collection, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
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
						if (collection.NeedsUpdating(entry, i, ElementType))
						{
							DbCommand st = null;
							// will still be issued when it used to be null
							if (useBatch)
							{
								st = await (session.Batcher.PrepareBatchCommandAsync(SqlDeleteRowString.CommandType, sql.Text,
																		 SqlDeleteRowString.ParameterTypes, cancellationToken)).ConfigureAwait(false);
							}
							else
							{
								st = await (session.Batcher.PrepareCommandAsync(SqlDeleteRowString.CommandType, sql.Text,
																	SqlDeleteRowString.ParameterTypes, cancellationToken)).ConfigureAwait(false);
							}

							try
							{
								int loc = WriteKey(st, id, offset, session);
								WriteElementToWhere(st, collection.GetSnapshotElement(entry, i), loc, session);
								if (useBatch)
								{
									await (session.Batcher.AddToBatchAsync(deleteExpectation, cancellationToken)).ConfigureAwait(false);
								}
								else
								{
									deleteExpectation.VerifyOutcomeNonBatched(await (session.Batcher.ExecuteNonQueryAsync(st, cancellationToken)).ConfigureAwait(false), st);
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
						if (collection.NeedsUpdating(entry, i, ElementType))
						{
							DbCommand st = null;
							if (useBatch)
							{
								st = await (session.Batcher.PrepareBatchCommandAsync(SqlInsertRowString.CommandType, sql.Text,
																		 SqlInsertRowString.ParameterTypes, cancellationToken)).ConfigureAwait(false);
							}
							else
							{
								st = await (session.Batcher.PrepareCommandAsync(SqlInsertRowString.CommandType, sql.Text,
																	SqlInsertRowString.ParameterTypes, cancellationToken)).ConfigureAwait(false);
							}

							try
							{
								//offset += insertExpectation.Prepare(st, Factory.ConnectionProvider.Driver);
								int loc = WriteKey(st, id, offset, session);
								if (HasIndex && !indexContainsFormula)
								{
									loc = WriteIndexToWhere(st, collection.GetIndex(entry, i, this), loc, session);
								}
								WriteElementToWhere(st, collection.GetElement(entry), loc, session);
								if (useBatch)
								{
									await (session.Batcher.AddToBatchAsync(insertExpectation, cancellationToken)).ConfigureAwait(false);
								}
								else
								{
									insertExpectation.VerifyOutcomeNonBatched(await (session.Batcher.ExecuteNonQueryAsync(st, cancellationToken)).ConfigureAwait(false), st);
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
				throw ADOExceptionHelper.Convert(SQLExceptionConverter, sqle, "could not update collection rows: " + MessageHelper.CollectionInfoString(this, collection, id, session));
			}
		}

		#region NH Specific

		#endregion
	}
}