using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Hql.Util;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Loader
{
	/// <summary>
	/// Abstract superclass of object loading (and querying) strategies.
	/// </summary>
	/// <remarks>
	/// <p>
	/// This class implements useful common functionality that concrete loaders would delegate to.
	/// It is not intended that this functionality would be directly accessed by client code (Hence,
	/// all methods of this class are declared <c>protected</c> or <c>private</c>.) This class relies heavily upon the
	/// <see cref = "ILoadable"/> interface, which is the contract between this class and 
	/// <see cref = "IEntityPersister"/>s that may be loaded by it.
	/// </p>
	/// <p>
	/// The present implementation is able to load any number of columns of entities and at most 
	/// one collection role per query.
	/// </p>
	/// </remarks>
	/// <seealso cref = "NHibernate.Persister.Entity.ILoadable"/>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class Loader
	{
		private async Task LoadFromResultSetAsync(IDataReader rs, int i, object obj, string instanceClass, EntityKey key, string rowIdAlias, LockMode lockMode, ILoadable rootPersister, ISessionImplementor session)
		{
			object id = key.Identifier;
			// Get the persister for the _subclass_
			ILoadable persister = (ILoadable)Factory.GetEntityPersister(instanceClass);
			if (Log.IsDebugEnabled)
			{
				Log.Debug("Initializing object from DataReader: " + MessageHelper.InfoString(persister, id));
			}

			bool eagerPropertyFetch = IsEagerPropertyFetchEnabled(i);
			// add temp entry so that the next step is circular-reference
			// safe - only needed because some types don't take proper
			// advantage of two-phase-load (esp. components)
			TwoPhaseLoad.AddUninitializedEntity(key, obj, persister, lockMode, !eagerPropertyFetch, session);
			// This is not very nice (and quite slow):
			string[][] cols = persister == rootPersister ? EntityAliases[i].SuffixedPropertyAliases : EntityAliases[i].GetSuffixedPropertyAliases(persister);
			object[] values = await (persister.HydrateAsync(rs, id, obj, rootPersister, cols, eagerPropertyFetch, session));
			object rowId = persister.HasRowId ? rs[rowIdAlias] : null;
			IAssociationType[] ownerAssociationTypes = OwnerAssociationTypes;
			if (ownerAssociationTypes != null && ownerAssociationTypes[i] != null)
			{
				string ukName = ownerAssociationTypes[i].RHSUniqueKeyPropertyName;
				if (ukName != null)
				{
					int index = ((IUniqueKeyLoadable)persister).GetPropertyIndex(ukName);
					IType type = persister.PropertyTypes[index];
					// polymorphism not really handled completely correctly,
					// perhaps...well, actually its ok, assuming that the
					// entity name used in the lookup is the same as the
					// the one used here, which it will be
					EntityUniqueKey euk = new EntityUniqueKey(rootPersister.EntityName, ukName, type.SemiResolve(values[index], session, obj), type, session.EntityMode, session.Factory);
					session.PersistenceContext.AddEntity(euk, obj);
				}
			}

			TwoPhaseLoad.PostHydrate(persister, id, values, rowId, obj, lockMode, !eagerPropertyFetch, session);
		}

		protected async Task<IDataReader> GetResultSetAsync(IDbCommand st, bool autoDiscoverTypes, bool callable, RowSelection selection, ISessionImplementor session)
		{
			IDataReader rs = null;
			try
			{
				Log.Info(st.CommandText);
				// TODO NH: Callable
				rs = await (session.Batcher.ExecuteReaderAsync(st));
				//NH: this is checked outside the WrapResultSet because we
				// want to avoid the syncronization overhead in the vast majority
				// of cases where IsWrapResultSetsEnabled is set to false
				if (session.Factory.Settings.IsWrapResultSetsEnabled)
					rs = WrapResultSet(rs);
				Dialect.Dialect dialect = session.Factory.Dialect;
				if (!dialect.SupportsLimitOffset || !UseLimit(selection, dialect))
				{
					Advance(rs, selection);
				}

				if (autoDiscoverTypes)
				{
					AutoDiscoverTypes(rs);
				}

				return rs;
			}
			catch (Exception sqle)
			{
				ADOExceptionReporter.LogExceptions(sqle);
				session.Batcher.CloseCommand(st, rs);
				throw;
			}
		}

		private async Task<IList> DoQueryAsync(ISessionImplementor session, QueryParameters queryParameters, bool returnProxies, IResultTransformer forcedResultTransformer)
		{
			using (new SessionIdLoggingContext(session.SessionId))
			{
				RowSelection selection = queryParameters.RowSelection;
				int maxRows = HasMaxRows(selection) ? selection.MaxRows : int.MaxValue;
				int entitySpan = EntityPersisters.Length;
				List<object> hydratedObjects = entitySpan == 0 ? null : new List<object>(entitySpan * 10);
				IDbCommand st = PrepareQueryCommand(queryParameters, false, session);
				IDataReader rs = await (GetResultSetAsync(st, queryParameters.HasAutoDiscoverScalarTypes, queryParameters.Callable, selection, session));
				// would be great to move all this below here into another method that could also be used
				// from the new scrolling stuff.
				//
				// Would need to change the way the max-row stuff is handled (i.e. behind an interface) so
				// that I could do the control breaking at the means to know when to stop
				LockMode[] lockModeArray = GetLockModes(queryParameters.LockModes);
				EntityKey optionalObjectKey = GetOptionalObjectKey(queryParameters, session);
				bool createSubselects = IsSubselectLoadingEnabled;
				List<EntityKey[]> subselectResultKeys = createSubselects ? new List<EntityKey[]>() : null;
				IList results = new List<object>();
				try
				{
					HandleEmptyCollections(queryParameters.CollectionKeys, rs, session);
					EntityKey[] keys = new EntityKey[entitySpan]; // we can reuse it each time
					if (Log.IsDebugEnabled)
					{
						Log.Debug("processing result set");
					}

					int count;
					for (count = 0; count < maxRows && rs.Read(); count++)
					{
						if (Log.IsDebugEnabled)
						{
							Log.Debug("result set row: " + count);
						}

						object result = GetRowFromResultSet(rs, session, queryParameters, lockModeArray, optionalObjectKey, hydratedObjects, keys, returnProxies, forcedResultTransformer);
						results.Add(result);
						if (createSubselects)
						{
							subselectResultKeys.Add(keys);
							keys = new EntityKey[entitySpan]; //can't reuse in this case
						}
					}

					if (Log.IsDebugEnabled)
					{
						Log.Debug(string.Format("done processing result set ({0} rows)", count));
					}
				}
				catch (Exception e)
				{
					e.Data["actual-sql-query"] = st.CommandText;
					throw;
				}
				finally
				{
					session.Batcher.CloseCommand(st, rs);
				}

				InitializeEntitiesAndCollections(hydratedObjects, rs, session, queryParameters.IsReadOnly(session));
				if (createSubselects)
				{
					CreateSubselects(subselectResultKeys, queryParameters, session);
				}

				return results;
			}
		}
	}
}