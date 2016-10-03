#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Dialect.Lock;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Id.Insert;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Loader.Entity;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Properties;
using NHibernate.SqlCommand;
using NHibernate.Tuple;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using NHibernate.Util;
using Array = System.Array;
using Property = NHibernate.Mapping.Property;
using NHibernate.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace NHibernate.Persister.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityPersister : IOuterJoinLoadable, IQueryable, IClassMetadata, IUniqueKeyLoadable, ISqlLoadable, ILazyPropertyInitializer, IPostInsertIdentityPersister, ILockable
	{
		public virtual async Task<object> InitializeLazyPropertyAsync(string fieldName, object entity, ISessionImplementor session)
		{
			object id = session.GetContextEntityIdentifier(entity);
			EntityEntry entry = session.PersistenceContext.GetEntry(entity);
			if (entry == null)
				throw new HibernateException("entity is not associated with the session: " + id);
			if (log.IsDebugEnabled)
			{
				log.Debug(string.Format("initializing lazy properties of: {0}, field access: {1}", MessageHelper.InfoString(this, id, Factory), fieldName));
			}

			if (HasCache)
			{
				CacheKey cacheKey = session.GenerateCacheKey(id, IdentifierType, EntityName);
				object ce = Cache.Get(cacheKey, session.Timestamp);
				if (ce != null)
				{
					CacheEntry cacheEntry = (CacheEntry)CacheEntryStructure.Destructure(ce, factory);
					if (!cacheEntry.AreLazyPropertiesUnfetched)
					{
						//note early exit here:
						return await (InitializeLazyPropertiesFromCacheAsync(fieldName, entity, session, entry, cacheEntry));
					}
				}
			}

			return await (InitializeLazyPropertiesFromDatastoreAsync(fieldName, entity, session, id, entry));
		}

		private async Task<object> InitializeLazyPropertiesFromDatastoreAsync(string fieldName, object entity, ISessionImplementor session, object id, EntityEntry entry)
		{
			if (!HasLazyProperties)
				throw new AssertionFailure("no lazy properties");
			log.Debug("initializing lazy properties from datastore");
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					object result = null;
					DbCommand ps = null;
					DbDataReader rs = null;
					try
					{
						SqlString lazySelect = SQLLazySelectString;
						if (lazySelect != null)
						{
							// null sql means that the only lazy properties
							// are shared PK one-to-one associations which are
							// handled differently in the Type#nullSafeGet code...
							ps = await (session.Batcher.PrepareCommandAsync(CommandType.Text, lazySelect, IdentifierType.SqlTypes(Factory)));
							await (IdentifierType.NullSafeSetAsync(ps, id, 0, session));
							rs = await (session.Batcher.ExecuteReaderAsync(ps));
							await (rs.ReadAsync());
						}

						object[] snapshot = entry.LoadedState;
						for (int j = 0; j < lazyPropertyNames.Length; j++)
						{
							object propValue = await (lazyPropertyTypes[j].NullSafeGetAsync(rs, lazyPropertyColumnAliases[j], session, entity));
							if (InitializeLazyProperty(fieldName, entity, session, snapshot, j, propValue))
							{
								result = propValue;
							}
						}
					}
					finally
					{
						session.Batcher.CloseCommand(ps, rs);
					}

					log.Debug("done initializing lazy properties");
					return result;
				}
				catch (DbException sqle)
				{
					var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not initialize lazy properties: " + MessageHelper.InfoString(this, id, Factory), Sql = SQLLazySelectString.ToString(), EntityName = EntityName, EntityId = id};
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
				}
		}

		private async Task<object> InitializeLazyPropertiesFromCacheAsync(string fieldName, object entity, ISessionImplementor session, EntityEntry entry, CacheEntry cacheEntry)
		{
			log.Debug("initializing lazy properties from second-level cache");
			object result = null;
			object[] disassembledValues = cacheEntry.DisassembledState;
			object[] snapshot = entry.LoadedState;
			for (int j = 0; j < lazyPropertyNames.Length; j++)
			{
				object propValue = await (lazyPropertyTypes[j].AssembleAsync(disassembledValues[lazyPropertyNumbers[j]], session, entity));
				if (InitializeLazyProperty(fieldName, entity, session, snapshot, j, propValue))
				{
					result = propValue;
				}
			}

			log.Debug("done initializing lazy properties");
			return result;
		}

		public async Task<object[]> GetDatabaseSnapshotAsync(object id, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Getting current persistent state for: " + MessageHelper.InfoString(this, id, Factory));
			}

			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					DbCommand st = await (session.Batcher.PrepareCommandAsync(CommandType.Text, SQLSnapshotSelectString, IdentifierType.SqlTypes(factory)));
					DbDataReader rs = null;
					try
					{
						await (IdentifierType.NullSafeSetAsync(st, id, 0, session));
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						if (!await (rs.ReadAsync()))
						{
							//if there is no resulting row, return null
							return null;
						}

						//otherwise return the "hydrated" state (ie. associations are not resolved)
						IType[] types = PropertyTypes;
						object[] values = new object[types.Length];
						bool[] includeProperty = PropertyUpdateability;
						for (int i = 0; i < types.Length; i++)
						{
							if (includeProperty[i])
							{
								values[i] = await (types[i].HydrateAsync(rs, GetPropertyAliases(string.Empty, i), session, null)); //null owner ok??
							}
						}

						return values;
					}
					finally
					{
						session.Batcher.CloseCommand(st, rs);
					}
				}
				catch (DbException sqle)
				{
					var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not retrieve snapshot: " + MessageHelper.InfoString(this, id, Factory), Sql = SQLSnapshotSelectString.ToString(), EntityName = EntityName, EntityId = id};
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
				}
		}

		public async Task<object> ForceVersionIncrementAsync(object id, object currentVersion, ISessionImplementor session)
		{
			if (!IsVersioned)
				throw new AssertionFailure("cannot force version increment on non-versioned entity");
			if (IsVersionPropertyGenerated)
			{
				// the difficulty here is exactly what do we update in order to
				// force the version to be incremented in the db...
				throw new HibernateException("LockMode.Force is currently not supported for generated version properties");
			}

			object nextVersion = await (VersionType.NextAsync(currentVersion, session));
			if (log.IsDebugEnabled)
			{
				log.Debug("Forcing version increment [" + MessageHelper.InfoString(this, id, Factory) + "; " + VersionType.ToLoggableString(currentVersion, Factory) + " -> " + VersionType.ToLoggableString(nextVersion, Factory) + "]");
			}

			IExpectation expectation = Expectations.AppropriateExpectation(updateResultCheckStyles[0]);
			// todo : cache this sql...
			SqlCommandInfo versionIncrementCommand = GenerateVersionIncrementUpdateString();
			try
			{
				DbCommand st = await (session.Batcher.PrepareCommandAsync(versionIncrementCommand.CommandType, versionIncrementCommand.Text, versionIncrementCommand.ParameterTypes));
				try
				{
					await (VersionType.NullSafeSetAsync(st, nextVersion, 0, session));
					await (IdentifierType.NullSafeSetAsync(st, id, 1, session));
					await (VersionType.NullSafeSetAsync(st, currentVersion, 1 + IdentifierColumnSpan, session));
					Check(await (session.Batcher.ExecuteNonQueryAsync(st)), id, 0, expectation, st);
				}
				finally
				{
					session.Batcher.CloseCommand(st, null);
				}
			}
			catch (DbException sqle)
			{
				var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not retrieve version: " + MessageHelper.InfoString(this, id, Factory), Sql = VersionSelectString.ToString(), EntityName = EntityName, EntityId = id};
				throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
			}

			return nextVersion;
		}

		/// <summary>
		/// Retrieve the version number
		/// </summary>
		public async Task<object> GetCurrentVersionAsync(object id, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Getting version: " + MessageHelper.InfoString(this, id, Factory));
			}

			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					DbCommand st = session.Batcher.PrepareQueryCommand(CommandType.Text, VersionSelectString, IdentifierType.SqlTypes(Factory));
					DbDataReader rs = null;
					try
					{
						await (IdentifierType.NullSafeSetAsync(st, id, 0, session));
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						if (!await (rs.ReadAsync()))
						{
							return null;
						}

						if (!IsVersioned)
						{
							return this;
						}

						return await (VersionType.NullSafeGetAsync(rs, VersionColumnName, session, null));
					}
					finally
					{
						session.Batcher.CloseCommand(st, rs);
					}
				}
				catch (DbException sqle)
				{
					var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not retrieve version: " + MessageHelper.InfoString(this, id, Factory), Sql = VersionSelectString.ToString(), EntityName = EntityName, EntityId = id};
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
				}
		}

		public virtual async Task LockAsync(object id, object version, object obj, LockMode lockMode, ISessionImplementor session)
		{
			await (GetLocker(lockMode).LockAsync(id, version, obj, session));
		}

		public async Task<object> LoadByUniqueKeyAsync(string propertyName, object uniqueKey, ISessionImplementor session)
		{
			return await (GetAppropriateUniqueKeyLoader(propertyName, session.EnabledFilters).LoadByUniqueKeyAsync(session, uniqueKey));
		}

		protected Task<int> DehydrateAsync(object id, object[] fields, bool[] includeProperty, bool[][] includeColumns, int j, DbCommand st, ISessionImplementor session)
		{
			return DehydrateAsync(id, fields, null, includeProperty, includeColumns, j, st, session, 0);
		}

		/// <summary> Marshall the fields of a persistent instance to a prepared statement</summary>
		protected async Task<int> DehydrateAsync(object id, object[] fields, object rowId, bool[] includeProperty, bool[][] includeColumns, int table, DbCommand statement, ISessionImplementor session, int index)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Dehydrating entity: " + MessageHelper.InfoString(this, id, Factory));
			}

			// there's a pretty strong coupling between the order of the SQL parameter 
			// construction and the actual order of the parameter collection. 
			for (int i = 0; i < entityMetamodel.PropertySpan; i++)
			{
				if (includeProperty[i] && IsPropertyOfTable(i, table))
				{
					try
					{
						await (PropertyTypes[i].NullSafeSetAsync(statement, fields[i], index, includeColumns[i], session));
						index += ArrayHelper.CountTrue(includeColumns[i]); //TODO:  this is kinda slow...
					}
					catch (Exception ex)
					{
						throw new PropertyValueException("Error dehydrating property value for", EntityName, entityMetamodel.PropertyNames[i], ex);
					}
				}
			}

			if (rowId != null)
			{
				// TODO H3.2 : support to set the rowId
				// TransactionManager.manager.SetObject(ps, index, rowId);
				// index += 1;
				throw new NotImplementedException("support to set the rowId");
			}
			else if (id != null)
			{
				var property = GetIdentiferProperty(table);
				await (property.Type.NullSafeSetAsync(statement, id, index, session));
				index += property.Type.GetColumnSpan(factory);
			}

			return index;
		}

		/// <summary>
		/// Unmarshall the fields of a persistent instance from a result set,
		/// without resolving associations or collections
		/// </summary>
		public async Task<object[]> HydrateAsync(DbDataReader rs, object id, object obj, ILoadable rootLoadable, string[][] suffixedPropertyColumns, bool allProperties, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Hydrating entity: " + MessageHelper.InfoString(this, id, Factory));
			}

			AbstractEntityPersister rootPersister = (AbstractEntityPersister)rootLoadable;
			bool hasDeferred = rootPersister.HasSequentialSelect;
			DbCommand sequentialSelect = null;
			DbDataReader sequentialResultSet = null;
			bool sequentialSelectEmpty = false;
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					if (hasDeferred)
					{
						SqlString sql = rootPersister.GetSequentialSelect(EntityName);
						if (sql != null)
						{
							//TODO: I am not so sure about the exception handling in this bit!
							sequentialSelect = await (session.Batcher.PrepareCommandAsync(CommandType.Text, sql, IdentifierType.SqlTypes(factory)));
							await (rootPersister.IdentifierType.NullSafeSetAsync(sequentialSelect, id, 0, session));
							sequentialResultSet = await (session.Batcher.ExecuteReaderAsync(sequentialSelect));
							if (!await (sequentialResultSet.ReadAsync()))
							{
								// TODO: Deal with the "optional" attribute in the <join> mapping;
								// this code assumes that optional defaults to "true" because it
								// doesn't actually seem to work in the fetch="join" code
								//
								// Note that actual proper handling of optional-ality here is actually
								// more involved than this patch assumes.  Remember that we might have
								// multiple <join/> mappings associated with a single entity.  Really
								// a couple of things need to happen to properly handle optional here:
								//  1) First and foremost, when handling multiple <join/>s, we really
								//      should be using the entity root table as the driving table;
								//      another option here would be to choose some non-optional joined
								//      table to use as the driving table.  In all likelihood, just using
								//      the root table is much simplier
								//  2) Need to add the FK columns corresponding to each joined table
								//      to the generated select list; these would then be used when
								//      iterating the result set to determine whether all non-optional
								//      data is present
								// My initial thoughts on the best way to deal with this would be
								// to introduce a new SequentialSelect abstraction that actually gets
								// generated in the persisters (ok, SingleTable...) and utilized here.
								// It would encapsulated all this required optional-ality checking...
								sequentialSelectEmpty = true;
							}
						}
					}

					string[] propNames = PropertyNames;
					IType[] types = PropertyTypes;
					object[] values = new object[types.Length];
					bool[] laziness = PropertyLaziness;
					string[] propSubclassNames = SubclassPropertySubclassNameClosure;
					for (int i = 0; i < types.Length; i++)
					{
						if (!propertySelectable[i])
						{
							values[i] = BackrefPropertyAccessor.Unknown;
						}
						else if (allProperties || !laziness[i])
						{
							//decide which ResultSet to get the property value from:
							bool propertyIsDeferred = hasDeferred && rootPersister.IsSubclassPropertyDeferred(propNames[i], propSubclassNames[i]);
							if (propertyIsDeferred && sequentialSelectEmpty)
							{
								values[i] = null;
							}
							else
							{
								DbDataReader propertyResultSet = propertyIsDeferred ? sequentialResultSet : rs;
								string[] cols = propertyIsDeferred ? propertyColumnAliases[i] : suffixedPropertyColumns[i];
								values[i] = await (types[i].HydrateAsync(propertyResultSet, cols, session, obj));
							}
						}
						else
						{
							values[i] = LazyPropertyInitializer.UnfetchedProperty;
						}
					}

					if (sequentialResultSet != null)
					{
						sequentialResultSet.Close();
					}

					return values;
				}
				finally
				{
					if (sequentialSelect != null)
					{
						session.Batcher.CloseCommand(sequentialSelect, sequentialResultSet);
					}
				}
		}

		/// <summary>
		/// Perform an SQL INSERT, and then retrieve a generated identifier.
		/// </summary>
		/// <remarks>
		/// This form is used for PostInsertIdentifierGenerator-style ids (IDENTITY, select, etc).
		/// </remarks>
		protected async Task<object> InsertAsync(object[] fields, bool[] notNull, SqlCommandInfo sql, object obj, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Inserting entity: " + EntityName + " (native id)");
				if (IsVersioned)
				{
					log.Debug("Version: " + Versioning.GetVersion(fields, this));
				}
			}

			IBinder binder = new GeneratedIdentifierBinder(fields, notNull, session, obj, this);
			return await (identityDelegate.PerformInsertAsync(sql, session, binder));
		}

		/// <summary>
		/// Perform an SQL INSERT.
		/// </summary>
		/// <remarks>
		/// This for is used for all non-root tables as well as the root table
		/// in cases where the identifier value is known before the insert occurs.
		/// </remarks>
		protected async Task InsertAsync(object id, object[] fields, bool[] notNull, int j, SqlCommandInfo sql, object obj, ISessionImplementor session)
		{
			//check if the id comes from an alternate column
			object tableId = GetJoinTableId(j, fields) ?? id;
			if (IsInverseTable(j))
			{
				return;
			}

			//note: it is conceptually possible that a UserType could map null to
			//	  a non-null value, so the following is arguable:
			if (IsNullableTable(j) && IsAllNull(fields, j))
			{
				return;
			}

			if (log.IsDebugEnabled)
			{
				log.Debug("Inserting entity: " + MessageHelper.InfoString(this, tableId, Factory));
				if (j == 0 && IsVersioned)
				{
					log.Debug("Version: " + Versioning.GetVersion(fields, this));
				}
			}

			IExpectation expectation = Expectations.AppropriateExpectation(insertResultCheckStyles[j]);
			//bool callable = IsInsertCallable(j);
			// we can't batch joined inserts, *especially* not if it is an identity insert;
			// nor can we batch statements where the expectation is based on an output param
			bool useBatch = j == 0 && expectation.CanBeBatched;
			try
			{
				// Render the SQL query
				DbCommand insertCmd = useBatch ? await (session.Batcher.PrepareBatchCommandAsync(sql.CommandType, sql.Text, sql.ParameterTypes)) : await (session.Batcher.PrepareCommandAsync(sql.CommandType, sql.Text, sql.ParameterTypes));
				try
				{
					int index = 0;
					await (DehydrateAsync(tableId, fields, null, notNull, propertyColumnInsertable, j, insertCmd, session, index));
					if (useBatch)
					{
						await (session.Batcher.AddToBatchAsync(expectation));
					}
					else
					{
						expectation.VerifyOutcomeNonBatched(await (session.Batcher.ExecuteNonQueryAsync(insertCmd)), insertCmd);
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
						session.Batcher.CloseCommand(insertCmd, null);
					}
				}
			}
			catch (DbException sqle)
			{
				var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not insert: " + MessageHelper.InfoString(this, tableId), Sql = sql.ToString(), EntityName = EntityName, EntityId = tableId};
				throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
			}
		}

		/// <summary> Perform an SQL UPDATE or SQL INSERT</summary>
		protected internal virtual async Task UpdateOrInsertAsync(object id, object[] fields, object[] oldFields, object rowId, bool[] includeProperty, int j, object oldVersion, object obj, SqlCommandInfo sql, ISessionImplementor session)
		{
			if (!IsInverseTable(j))
			{
				//check if the id comes from an alternate column
				object tableId = GetJoinTableId(j, fields) ?? id;
				bool isRowToUpdate;
				if (IsNullableTable(j) && oldFields != null && IsAllNull(oldFields, j))
				{
					//don't bother trying to update, we know there is no row there yet
					isRowToUpdate = false;
				}
				else if (IsNullableTable(j) && IsAllNull(fields, j))
				{
					//if all fields are null, we might need to delete existing row
					isRowToUpdate = true;
					await (DeleteAsync(tableId, oldVersion, j, obj, SqlDeleteStrings[j], session, null));
				}
				else
				{
					//there is probably a row there, so try to update
					//if no rows were updated, we will find out
					isRowToUpdate = await (UpdateAsync(tableId, fields, oldFields, rowId, includeProperty, j, oldVersion, obj, sql, session));
				}

				if (!isRowToUpdate && !IsAllNull(fields, j))
				{
					await (InsertAsync(tableId, fields, PropertyInsertability, j, SqlInsertStrings[j], obj, session));
				}
			}
		}

		protected async Task<bool> UpdateAsync(object id, object[] fields, object[] oldFields, object rowId, bool[] includeProperty, int j, object oldVersion, object obj, SqlCommandInfo sql, ISessionImplementor session)
		{
			bool useVersion = j == 0 && IsVersioned;
			IExpectation expectation = Expectations.AppropriateExpectation(updateResultCheckStyles[j]);
			//bool callable = IsUpdateCallable(j);
			bool useBatch = j == 0 && expectation.CanBeBatched && IsBatchable; //note: updates to joined tables can't be batched...
			if (log.IsDebugEnabled)
			{
				log.Debug("Updating entity: " + MessageHelper.InfoString(this, id, Factory));
				if (useVersion)
				{
					log.Debug("Existing version: " + oldVersion + " -> New Version: " + fields[VersionProperty]);
				}
			}

			try
			{
				int index = 0;
				DbCommand statement = useBatch ? await (session.Batcher.PrepareBatchCommandAsync(sql.CommandType, sql.Text, sql.ParameterTypes)) : await (session.Batcher.PrepareCommandAsync(sql.CommandType, sql.Text, sql.ParameterTypes));
				try
				{
					//index += expectation.Prepare(statement, factory.ConnectionProvider.Driver);
					//Now write the values of fields onto the prepared statement
					index = await (DehydrateAsync(id, fields, rowId, includeProperty, propertyColumnUpdateable, j, statement, session, index));
					// Write any appropriate versioning conditional parameters
					if (useVersion && Versioning.OptimisticLock.Version == entityMetamodel.OptimisticLockMode)
					{
						if (CheckVersion(includeProperty))
							await (VersionType.NullSafeSetAsync(statement, oldVersion, index, session));
					}
					else if (entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version && oldFields != null)
					{
						bool[] versionability = PropertyVersionability;
						bool[] includeOldField = OptimisticLockMode == Versioning.OptimisticLock.All ? PropertyUpdateability : includeProperty;
						IType[] types = PropertyTypes;
						for (int i = 0; i < entityMetamodel.PropertySpan; i++)
						{
							bool include = includeOldField[i] && IsPropertyOfTable(i, j) && versionability[i];
							if (include)
							{
								bool[] settable = types[i].ToColumnNullness(oldFields[i], Factory);
								await (types[i].NullSafeSetAsync(statement, oldFields[i], index, settable, session));
								index += ArrayHelper.CountTrue(settable);
							}
						}
					}

					if (useBatch)
					{
						await (session.Batcher.AddToBatchAsync(expectation));
						return true;
					}
					else
					{
						return Check(await (session.Batcher.ExecuteNonQueryAsync(statement)), id, j, expectation, statement);
					}
				}
				catch (StaleStateException e)
				{
					if (useBatch)
					{
						session.Batcher.AbortBatch(e);
					}

					throw new StaleObjectStateException(EntityName, id);
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
						session.Batcher.CloseCommand(statement, null);
					}
				}
			}
			catch (DbException sqle)
			{
				var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not update: " + MessageHelper.InfoString(this, id, Factory), Sql = sql.Text.ToString(), EntityName = EntityName, EntityId = id};
				throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
			}
		}

		/// <summary>
		/// Perform an SQL DELETE
		/// </summary>
		public async Task DeleteAsync(object id, object version, int j, object obj, SqlCommandInfo sql, ISessionImplementor session, object[] loadedState)
		{
			//check if the id should come from another column
			object tableId = GetJoinTableId(j, obj, session.EntityMode) ?? id;
			if (IsInverseTable(j))
			{
				return;
			}

			// NH : Only use version if lock mode is Version
			bool useVersion = j == 0 && IsVersioned && Versioning.OptimisticLock.Version == entityMetamodel.OptimisticLockMode;
			//bool callable = IsDeleteCallable(j);
			IExpectation expectation = Expectations.AppropriateExpectation(deleteResultCheckStyles[j]);
			bool useBatch = j == 0 && expectation.CanBeBatched && IsBatchable;
			if (log.IsDebugEnabled)
			{
				log.Debug("Deleting entity: " + MessageHelper.InfoString(this, tableId, Factory));
				if (useVersion)
				{
					log.Debug("Version: " + version);
				}
			}

			if (IsTableCascadeDeleteEnabled(j))
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("delete handled by foreign key constraint: " + GetTableName(j));
				}

				return; //EARLY EXIT!
			}

			try
			{
				int index = 0;
				DbCommand statement;
				if (useBatch)
				{
					statement = await (session.Batcher.PrepareBatchCommandAsync(sql.CommandType, sql.Text, sql.ParameterTypes));
				}
				else
				{
					statement = await (session.Batcher.PrepareCommandAsync(sql.CommandType, sql.Text, sql.ParameterTypes));
				}

				try
				{
					//index += expectation.Prepare(statement, factory.ConnectionProvider.Driver);
					// Do the key. The key is immutable so we can use the _current_ object state - not necessarily
					// the state at the time the delete was issued
					var property = GetIdentiferProperty(j);
					await (property.Type.NullSafeSetAsync(statement, tableId, index, session));
					index += property.Type.GetColumnSpan(factory);
					// We should use the _current_ object state (ie. after any updates that occurred during flush)
					if (useVersion)
					{
						await (VersionType.NullSafeSetAsync(statement, version, index, session));
					}
					else if (entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version && loadedState != null)
					{
						bool[] versionability = PropertyVersionability;
						IType[] types = PropertyTypes;
						for (int i = 0; i < entityMetamodel.PropertySpan; i++)
						{
							if (IsPropertyOfTable(i, j) && versionability[i])
							{
								// this property belongs to the table and it is not specifically
								// excluded from optimistic locking by optimistic-lock="false"
								bool[] settable = types[i].ToColumnNullness(loadedState[i], Factory);
								await (types[i].NullSafeSetAsync(statement, loadedState[i], index, settable, session));
								index += ArrayHelper.CountTrue(settable);
							}
						}
					}

					if (useBatch)
					{
						await (session.Batcher.AddToBatchAsync(expectation));
					}
					else
					{
						Check(await (session.Batcher.ExecuteNonQueryAsync(statement)), tableId, j, expectation, statement);
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
						session.Batcher.CloseCommand(statement, null);
					}
				}
			}
			catch (DbException sqle)
			{
				var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not delete: " + MessageHelper.InfoString(this, tableId, Factory), Sql = sql.Text.ToString(), EntityName = EntityName, EntityId = tableId};
				throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
			}
		}

		public async Task UpdateAsync(object id, object[] fields, int[] dirtyFields, bool hasDirtyCollection, object[] oldFields, object oldVersion, object obj, object rowId, ISessionImplementor session)
		{
			//note: dirtyFields==null means we had no snapshot, and we couldn't get one using select-before-update
			//	  oldFields==null just means we had no snapshot to begin with (we might have used select-before-update to get the dirtyFields)
			bool[] tableUpdateNeeded = GetTableUpdateNeeded(dirtyFields, hasDirtyCollection);
			int span = TableSpan;
			bool[] propsToUpdate;
			SqlCommandInfo[] updateStrings;
			EntityEntry entry = session.PersistenceContext.GetEntry(obj);
			// Ensure that an immutable or non-modifiable entity is not being updated unless it is
			// in the process of being deleted.
			if (entry == null && !IsMutable)
				throw new InvalidOperationException("Updating immutable entity that is not in session yet!");
			if (entityMetamodel.IsDynamicUpdate && dirtyFields != null)
			{
				// For the case of dynamic-update="true", we need to generate the UPDATE SQL
				propsToUpdate = GetPropertiesToUpdate(dirtyFields, hasDirtyCollection);
				// don't need to check laziness (dirty checking algorithm handles that)
				updateStrings = new SqlCommandInfo[span];
				for (int j = 0; j < span; j++)
				{
					updateStrings[j] = tableUpdateNeeded[j] ? GenerateUpdateString(propsToUpdate, j, oldFields, j == 0 && rowId != null) : null;
				}
			}
			else if (!IsModifiableEntity(entry))
			{
				// We need to generate UPDATE SQL when a non-modifiable entity (e.g., read-only or immutable)
				// needs:
				// - to have references to transient entities set to null before being deleted
				// - to have version incremented do to a "dirty" association
				// If dirtyFields == null, then that means that there are no dirty properties to
				// to be updated; an empty array for the dirty fields needs to be passed to
				// getPropertiesToUpdate() instead of null.
				propsToUpdate = this.GetPropertiesToUpdate((dirtyFields == null ? ArrayHelper.EmptyIntArray : dirtyFields), hasDirtyCollection);
				// don't need to check laziness (dirty checking algorithm handles that)
				updateStrings = new SqlCommandInfo[span];
				for (int j = 0; j < span; j++)
				{
					updateStrings[j] = tableUpdateNeeded[j] ? GenerateUpdateString(propsToUpdate, j, oldFields, j == 0 && rowId != null) : null;
				}
			}
			else
			{
				// For the case of dynamic-update="false", or no snapshot, we use the static SQL
				updateStrings = GetUpdateStrings(rowId != null, HasUninitializedLazyProperties(obj, session.EntityMode));
				propsToUpdate = GetPropertyUpdateability(obj, session.EntityMode);
			}

			for (int j = 0; j < span; j++)
			{
				// Now update only the tables with dirty properties (and the table with the version number)
				if (tableUpdateNeeded[j])
				{
					await (UpdateOrInsertAsync(id, fields, oldFields, j == 0 ? rowId : null, propsToUpdate, j, oldVersion, obj, updateStrings[j], session));
				}
			}
		}

		public async Task<object> InsertAsync(object[] fields, object obj, ISessionImplementor session)
		{
			int span = TableSpan;
			object id;
			if (entityMetamodel.IsDynamicInsert)
			{
				// For the case of dynamic-insert="true", we need to generate the INSERT SQL
				bool[] notNull = GetPropertiesToInsert(fields);
				id = await (InsertAsync(fields, notNull, GenerateInsertString(true, notNull), obj, session));
				for (int j = 1; j < span; j++)
				{
					await (InsertAsync(id, fields, notNull, j, GenerateInsertString(notNull, j), obj, session));
				}
			}
			else
			{
				// For the case of dynamic-insert="false", use the static SQL
				id = await (InsertAsync(fields, PropertyInsertability, SQLIdentityInsertString, obj, session));
				for (int j = 1; j < span; j++)
				{
					await (InsertAsync(id, fields, PropertyInsertability, j, SqlInsertStrings[j], obj, session));
				}
			}

			return id;
		}

		public async Task InsertAsync(object id, object[] fields, object obj, ISessionImplementor session)
		{
			int span = TableSpan;
			if (entityMetamodel.IsDynamicInsert)
			{
				bool[] notNull = GetPropertiesToInsert(fields);
				// For the case of dynamic-insert="true", we need to generate the INSERT SQL
				for (int j = 0; j < span; j++)
				{
					await (InsertAsync(id, fields, notNull, j, GenerateInsertString(notNull, j), obj, session));
				}
			}
			else
			{
				// For the case of dynamic-insert="false", use the static SQL
				for (int j = 0; j < span; j++)
				{
					await (InsertAsync(id, fields, PropertyInsertability, j, SqlInsertStrings[j], obj, session));
				}
			}
		}

		public async Task DeleteAsync(object id, object version, object obj, ISessionImplementor session)
		{
			int span = TableSpan;
			bool isImpliedOptimisticLocking = !entityMetamodel.IsVersioned && entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version;
			object[] loadedState = null;
			if (isImpliedOptimisticLocking)
			{
				// need to treat this as if it where optimistic-lock="all" (dirty does *not* make sense);
				// first we need to locate the "loaded" state
				//
				// Note, it potentially could be a proxy, so perform the location the safe way...
				EntityKey key = session.GenerateEntityKey(id, this);
				object entity = session.PersistenceContext.GetEntity(key);
				if (entity != null)
				{
					EntityEntry entry = session.PersistenceContext.GetEntry(entity);
					loadedState = entry.LoadedState;
				}
			}

			SqlCommandInfo[] deleteStrings;
			if (isImpliedOptimisticLocking && loadedState != null)
			{
				// we need to utilize dynamic delete statements
				deleteStrings = GenerateSQLDeleteStrings(loadedState);
			}
			else
			{
				// otherwise, utilize the static delete statements
				deleteStrings = SqlDeleteStrings;
			}

			for (int j = span - 1; j >= 0; j--)
			{
				await (DeleteAsync(id, version, j, obj, deleteStrings[j], session, loadedState));
			}
		}

		/// <summary>
		/// Load an instance using the appropriate loader (as determined by <see cref = "GetAppropriateLoader"/>
		/// </summary>
		public async Task<object> LoadAsync(object id, object optionalObject, LockMode lockMode, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Fetching entity: " + MessageHelper.InfoString(this, id, Factory));
			}

			IUniqueEntityLoader loader = GetAppropriateLoader(lockMode, session);
			return await (loader.LoadAsync(id, optionalObject, session));
		}

		public virtual async Task<int[]> FindDirtyAsync(object[] currentState, object[] previousState, object entity, ISessionImplementor session)
		{
			int[] props = await (TypeHelper.FindDirtyAsync(entityMetamodel.Properties, currentState, previousState, propertyColumnUpdateable, HasUninitializedLazyProperties(entity, session.EntityMode), session));
			if (props == null)
			{
				return null;
			}
			else
			{
				LogDirtyProperties(props);
				return props;
			}
		}

		public virtual async Task<int[]> FindModifiedAsync(object[] old, object[] current, object entity, ISessionImplementor session)
		{
			int[] props = await (TypeHelper.FindModifiedAsync(entityMetamodel.Properties, current, old, propertyColumnUpdateable, HasUninitializedLazyProperties(entity, session.EntityMode), session));
			if (props == null)
			{
				return null;
			}
			else
			{
				LogDirtyProperties(props);
				return props;
			}
		}

		public virtual async Task<object[]> GetPropertyValuesToInsertAsync(object obj, IDictionary mergeMap, ISessionImplementor session)
		{
			return await (GetTuplizer(session.EntityMode).GetPropertyValuesToInsertAsync(obj, mergeMap, session));
		}

		public async Task ProcessInsertGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session)
		{
			if (!HasInsertGeneratedProperties)
			{
				throw new AssertionFailure("no insert-generated properties");
			}

			await (session.Batcher.ExecuteBatchAsync()); //force immediate execution of the insert
			if (loaderName == null)
			{
				await (ProcessGeneratedPropertiesWithGeneratedSqlAsync(id, entity, state, session, sqlInsertGeneratedValuesSelectString, PropertyInsertGenerationInclusions));
			}
			else
			{
				await (ProcessGeneratedPropertiesWithLoaderAsync(id, entity, session));
				// The loader has added the entity to the first-level cache. We must remove
				// the entity from the first-level cache to avoid problems in the Save or SaveOrUpdate
				// event listeners, which don't expect the entity to already be present in the 
				// first-level cache.
				session.PersistenceContext.RemoveEntity(session.GenerateEntityKey(id, this));
			}
		}

		public async Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session)
		{
			if (!HasUpdateGeneratedProperties)
			{
				throw new AssertionFailure("no update-generated properties");
			}

			await (session.Batcher.ExecuteBatchAsync()); //force immediate execution of the update
			if (loaderName == null)
			{
				await (ProcessGeneratedPropertiesWithGeneratedSqlAsync(id, entity, state, session, sqlUpdateGeneratedValuesSelectString, PropertyUpdateGenerationInclusions));
			}
			else
			{
				// Remove entity from first-level cache to ensure that loader fetches fresh data from database.
				// The loader will ensure that the same entity is added back to the first-level cache.
				session.PersistenceContext.RemoveEntity(session.GenerateEntityKey(id, this));
				await (ProcessGeneratedPropertiesWithLoaderAsync(id, entity, session));
			}
		}

		private async Task ProcessGeneratedPropertiesWithGeneratedSqlAsync(object id, object entity, object[] state, ISessionImplementor session, SqlString selectionSQL, ValueInclusion[] generationInclusions)
		{
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					DbCommand cmd = session.Batcher.PrepareQueryCommand(CommandType.Text, selectionSQL, IdentifierType.SqlTypes(Factory));
					DbDataReader rs = null;
					try
					{
						await (IdentifierType.NullSafeSetAsync(cmd, id, 0, session));
						rs = await (session.Batcher.ExecuteReaderAsync(cmd));
						if (!await (rs.ReadAsync()))
						{
							throw new HibernateException("Unable to locate row for retrieval of generated properties: " + MessageHelper.InfoString(this, id, Factory));
						}

						for (int i = 0; i < PropertySpan; i++)
						{
							if (generationInclusions[i] != ValueInclusion.None)
							{
								object hydratedState = await (PropertyTypes[i].HydrateAsync(rs, GetPropertyAliases(string.Empty, i), session, entity));
								state[i] = await (PropertyTypes[i].ResolveIdentifierAsync(hydratedState, session, entity));
								SetPropertyValue(entity, i, state[i], session.EntityMode);
							}
						}
					}
					finally
					{
						session.Batcher.CloseCommand(cmd, rs);
					}
				}
				catch (DbException sqle)
				{
					var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "unable to select generated column values", Sql = selectionSQL.ToString(), EntityName = EntityName, EntityId = id};
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
				}
		}

		private async Task ProcessGeneratedPropertiesWithLoaderAsync(object id, object entity, ISessionImplementor session)
		{
			var query = (AbstractQueryImpl)session.GetNamedQuery(loaderName);
			if (query.HasNamedParameters)
			{
				query.SetParameter(query.NamedParameters[0], id, this.IdentifierType);
			}
			else
			{
				query.SetParameter(0, id, this.IdentifierType);
			}

			query.SetOptionalId(id);
			query.SetOptionalEntityName(this.EntityName);
			query.SetOptionalObject(entity);
			query.SetFlushMode(FlushMode.Never);
			await (query.ListAsync());
		}

		public virtual async Task<object[]> GetNaturalIdentifierSnapshotAsync(object id, ISessionImplementor session)
		{
			if (!HasNaturalIdentifier)
			{
				throw new MappingException("persistent class did not define a natural-id : " + MessageHelper.InfoString(this));
			}

			if (log.IsDebugEnabled)
			{
				log.Debug("Getting current natural-id snapshot state for: " + MessageHelper.InfoString(this, id, Factory));
			}

			int[] naturalIdPropertyIndexes = NaturalIdentifierProperties;
			int naturalIdPropertyCount = naturalIdPropertyIndexes.Length;
			bool[] naturalIdMarkers = new bool[PropertySpan];
			IType[] extractionTypes = new IType[naturalIdPropertyCount];
			for (int i = 0; i < naturalIdPropertyCount; i++)
			{
				extractionTypes[i] = PropertyTypes[naturalIdPropertyIndexes[i]];
				naturalIdMarkers[naturalIdPropertyIndexes[i]] = true;
			}

			///////////////////////////////////////////////////////////////////////
			// TODO : look at perhaps caching this...
			SqlSelectBuilder select = new SqlSelectBuilder(Factory);
			if (Factory.Settings.IsCommentsEnabled)
			{
				select.SetComment("get current natural-id state " + EntityName);
			}

			select.SetSelectClause(ConcretePropertySelectFragmentSansLeadingComma(RootAlias, naturalIdMarkers));
			select.SetFromClause(FromTableFragment(RootAlias) + FromJoinFragment(RootAlias, true, false));
			string[] aliasedIdColumns = StringHelper.Qualify(RootAlias, IdentifierColumnNames);
			SqlString whereClause = new SqlString(SqlStringHelper.Join(new SqlString("=", Parameter.Placeholder, " and "), aliasedIdColumns), "=", Parameter.Placeholder, WhereJoinFragment(RootAlias, true, false));
			SqlString sql = select.SetOuterJoins(SqlString.Empty, SqlString.Empty).SetWhereClause(whereClause).ToStatementString();
			///////////////////////////////////////////////////////////////////////
			object[] snapshot = new object[naturalIdPropertyCount];
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					DbCommand ps = await (session.Batcher.PrepareCommandAsync(CommandType.Text, sql, IdentifierType.SqlTypes(factory)));
					DbDataReader rs = null;
					try
					{
						await (IdentifierType.NullSafeSetAsync(ps, id, 0, session));
						rs = await (session.Batcher.ExecuteReaderAsync(ps));
						//if there is no resulting row, return null
						if (!await (rs.ReadAsync()))
						{
							return null;
						}

						for (int i = 0; i < naturalIdPropertyCount; i++)
						{
							snapshot[i] = await (extractionTypes[i].HydrateAsync(rs, GetPropertyAliases(string.Empty, naturalIdPropertyIndexes[i]), session, null));
							if (extractionTypes[i].IsEntityType)
							{
								snapshot[i] = await (extractionTypes[i].ResolveIdentifierAsync(snapshot[i], session, null));
							}
						}

						return snapshot;
					}
					finally
					{
						session.Batcher.CloseCommand(ps, rs);
					}
				}
				catch (DbException sqle)
				{
					var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not retrieve snapshot: " + MessageHelper.InfoString(this, id, Factory), Sql = sql.ToString(), EntityName = EntityName, EntityId = id};
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, exceptionContext);
				}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class GeneratedIdentifierBinder : IBinder
		{
			public virtual Task BindValuesAsync(DbCommand ps)
			{
				return entityPersister.DehydrateAsync(null, fields, notNull, entityPersister.propertyColumnInsertable, 0, ps, session);
			}
		}
	}
}
#endif
