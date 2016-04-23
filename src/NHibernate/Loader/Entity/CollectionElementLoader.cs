using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;
using NHibernate.Driver;

namespace NHibernate.Loader.Entity
{
	public class CollectionElementLoader : OuterJoinLoader
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (CollectionElementLoader));

		private readonly IOuterJoinLoadable persister;
		private readonly IType keyType;
		private readonly IType indexType;
		private readonly string entityName;
		private IParameterSpecification[] parametersSpecifications;

		public CollectionElementLoader(IQueryableCollection collectionPersister, ISessionFactoryImplementor factory,
		                               IDictionary<string, IFilter> enabledFilters) : base(factory, enabledFilters)
		{
			keyType = collectionPersister.KeyType;
			indexType = collectionPersister.IndexType;
			persister = (IOuterJoinLoadable) collectionPersister.ElementPersister;
			entityName = persister.EntityName;

			JoinWalker walker =
				new EntityJoinWalker(persister,
				                     ArrayHelper.Join(collectionPersister.KeyColumnNames, collectionPersister.IndexColumnNames), 1,
				                     LockMode.None, factory, enabledFilters);
			InitFromWalker(walker);

			PostInstantiate();

			log.Debug("Static select for entity " + entityName + ": " + SqlString);
		}

		private IEnumerable<IParameterSpecification> CreateParameterSpecificationsAndAssignBackTrack(IEnumerable<Parameter> sqlPatameters)
		{
			var specifications = new IParameterSpecification[]
			                     {
			                     	new PositionalParameterSpecification(1, 0, 0) {ExpectedType = keyType},
			                     	new PositionalParameterSpecification(1, 0, 1) {ExpectedType = indexType},
			                     };
			Parameter[] parameters = sqlPatameters.ToArray();
			int sqlParameterPos = 0;
			IEnumerable<string> paramTrackers = specifications.SelectMany(specification => specification.GetIdsForBackTrack(Factory));
			foreach (string paramTracker in paramTrackers)
			{
				parameters[sqlParameterPos++].BackTrack = paramTracker;
			}
			return specifications;
		}

		protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
		{
			return parametersSpecifications ?? (parametersSpecifications = CreateParameterSpecificationsAndAssignBackTrack(SqlString.GetParameters()).ToArray());
		}

		protected override bool IsSingleRowLoader
		{
			get { return true; }
		}

		public virtual async Task<object> LoadElement(ISessionImplementor session, object key, object index)
		{
			IList list = await LoadEntity(session, key, index, keyType, indexType, persister).ConfigureAwait(false);

			if (list.Count == 1)
			{
				return list[0];
			}
			else if (list.Count == 0)
			{
				return null;
			}
			else
			{
				if (CollectionOwners != null)
				{
					return list[0];
				}
				else
				{
					throw new HibernateException("More than one row was found");
				}
			}
		}

		protected override Task<object> GetResultColumnOrRow(object[] row, IResultTransformer transformer, IDataReaderEx rs,
		                                               ISessionImplementor session)
		{
			return Task.FromResult(row[row.Length - 1]);
		}
	}
}