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
	/// <summary>
	/// Superclass for built-in mapping strategies. Implements functionalty common to both mapping
	/// strategies
	/// </summary>
	/// <remarks>
	/// May be considered an immutable view of the mapping object
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityPersister : IOuterJoinLoadable, IQueryable, IClassMetadata, IUniqueKeyLoadable, ISqlLoadable, ILazyPropertyInitializer, IPostInsertIdentityPersister, ILockable
	{
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

		public async Task<object> GetCurrentVersionAsync(object id, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Getting version: " + MessageHelper.InfoString(this, id, Factory));
			}

			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					IDbCommand st = session.Batcher.PrepareQueryCommand(CommandType.Text, VersionSelectString, IdentifierType.SqlTypes(Factory));
					IDataReader rs = null;
					try
					{
						IdentifierType.NullSafeSet(st, id, 0, session);
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						if (!rs.Read())
						{
							return null;
						}

						if (!IsVersioned)
						{
							return this;
						}

						return VersionType.NullSafeGet(rs, VersionColumnName, session, null);
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

		private async Task ProcessGeneratedPropertiesWithGeneratedSqlAsync(object id, object entity, object[] state, ISessionImplementor session, SqlString selectionSQL, ValueInclusion[] generationInclusions)
		{
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					IDbCommand cmd = session.Batcher.PrepareQueryCommand(CommandType.Text, selectionSQL, IdentifierType.SqlTypes(Factory));
					IDataReader rs = null;
					try
					{
						IdentifierType.NullSafeSet(cmd, id, 0, session);
						rs = await (session.Batcher.ExecuteReaderAsync(cmd));
						if (!rs.Read())
						{
							throw new HibernateException("Unable to locate row for retrieval of generated properties: " + MessageHelper.InfoString(this, id, Factory));
						}

						for (int i = 0; i < PropertySpan; i++)
						{
							if (generationInclusions[i] != ValueInclusion.None)
							{
								object hydratedState = PropertyTypes[i].Hydrate(rs, GetPropertyAliases(string.Empty, i), session, entity);
								state[i] = PropertyTypes[i].ResolveIdentifier(hydratedState, session, entity);
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

		public async Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session)
		{
			if (!HasUpdateGeneratedProperties)
			{
				throw new AssertionFailure("no update-generated properties");
			}

			session.Batcher.ExecuteBatch(); //force immediate execution of the update
			if (loaderName == null)
			{
				await (ProcessGeneratedPropertiesWithGeneratedSqlAsync(id, entity, state, session, sqlUpdateGeneratedValuesSelectString, PropertyUpdateGenerationInclusions));
			}
			else
			{
				// Remove entity from first-level cache to ensure that loader fetches fresh data from database.
				// The loader will ensure that the same entity is added back to the first-level cache.
				session.PersistenceContext.RemoveEntity(session.GenerateEntityKey(id, this));
				ProcessGeneratedPropertiesWithLoader(id, entity, session);
			}
		}

		public async Task ProcessInsertGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session)
		{
			if (!HasInsertGeneratedProperties)
			{
				throw new AssertionFailure("no insert-generated properties");
			}

			session.Batcher.ExecuteBatch(); //force immediate execution of the insert
			if (loaderName == null)
			{
				await (ProcessGeneratedPropertiesWithGeneratedSqlAsync(id, entity, state, session, sqlInsertGeneratedValuesSelectString, PropertyInsertGenerationInclusions));
			}
			else
			{
				ProcessGeneratedPropertiesWithLoader(id, entity, session);
				// The loader has added the entity to the first-level cache. We must remove
				// the entity from the first-level cache to avoid problems in the Save or SaveOrUpdate
				// event listeners, which don't expect the entity to already be present in the 
				// first-level cache.
				session.PersistenceContext.RemoveEntity(session.GenerateEntityKey(id, this));
			}
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
					IDbCommand st = session.Batcher.PrepareCommand(CommandType.Text, SQLSnapshotSelectString, IdentifierType.SqlTypes(factory));
					IDataReader rs = null;
					try
					{
						IdentifierType.NullSafeSet(st, id, 0, session);
						rs = await (session.Batcher.ExecuteReaderAsync(st));
						if (!rs.Read())
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
								values[i] = types[i].Hydrate(rs, GetPropertyAliases(string.Empty, i), session, null); //null owner ok??
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
					IDbCommand ps = session.Batcher.PrepareCommand(CommandType.Text, sql, IdentifierType.SqlTypes(factory));
					IDataReader rs = null;
					try
					{
						IdentifierType.NullSafeSet(ps, id, 0, session);
						rs = await (session.Batcher.ExecuteReaderAsync(ps));
						//if there is no resulting row, return null
						if (!rs.Read())
						{
							return null;
						}

						for (int i = 0; i < naturalIdPropertyCount; i++)
						{
							snapshot[i] = extractionTypes[i].Hydrate(rs, GetPropertyAliases(string.Empty, naturalIdPropertyIndexes[i]), session, null);
							if (extractionTypes[i].IsEntityType)
							{
								snapshot[i] = extractionTypes[i].ResolveIdentifier(snapshot[i], session, null);
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

		public async Task<object[]> HydrateAsync(IDataReader rs, object id, object obj, ILoadable rootLoadable, string[][] suffixedPropertyColumns, bool allProperties, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Hydrating entity: " + MessageHelper.InfoString(this, id, Factory));
			}

			AbstractEntityPersister rootPersister = (AbstractEntityPersister)rootLoadable;
			bool hasDeferred = rootPersister.HasSequentialSelect;
			IDbCommand sequentialSelect = null;
			IDataReader sequentialResultSet = null;
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
							sequentialSelect = session.Batcher.PrepareCommand(CommandType.Text, sql, IdentifierType.SqlTypes(factory));
							rootPersister.IdentifierType.NullSafeSet(sequentialSelect, id, 0, session);
							sequentialResultSet = await (session.Batcher.ExecuteReaderAsync(sequentialSelect));
							if (!sequentialResultSet.Read())
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
								IDataReader propertyResultSet = propertyIsDeferred ? sequentialResultSet : rs;
								string[] cols = propertyIsDeferred ? propertyColumnAliases[i] : suffixedPropertyColumns[i];
								values[i] = types[i].Hydrate(propertyResultSet, cols, session, obj);
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

		public virtual async Task LockAsync(object id, object version, object obj, LockMode lockMode, ISessionImplementor session)
		{
			await (GetLocker(lockMode).LockAsync(id, version, obj, session));
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
					IDbCommand ps = null;
					IDataReader rs = null;
					try
					{
						SqlString lazySelect = SQLLazySelectString;
						if (lazySelect != null)
						{
							// null sql means that the only lazy properties
							// are shared PK one-to-one associations which are
							// handled differently in the Type#nullSafeGet code...
							ps = session.Batcher.PrepareCommand(CommandType.Text, lazySelect, IdentifierType.SqlTypes(Factory));
							IdentifierType.NullSafeSet(ps, id, 0, session);
							rs = await (session.Batcher.ExecuteReaderAsync(ps));
							rs.Read();
						}

						object[] snapshot = entry.LoadedState;
						for (int j = 0; j < lazyPropertyNames.Length; j++)
						{
							object propValue = lazyPropertyTypes[j].NullSafeGet(rs, lazyPropertyColumnAliases[j], session, entity);
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
	}
}