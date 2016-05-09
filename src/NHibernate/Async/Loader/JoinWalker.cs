using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Loader
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinWalker
	{
		private async Task AddAssociationToJoinTreeAsync(IAssociationType type, string[] aliasedLhsColumns, string alias, string path, int currentDepth, JoinType joinType)
		{
			IJoinable joinable = type.GetAssociatedJoinable(Factory);
			string subalias = GenerateTableAlias(associations.Count + 1, path, joinable);
			OuterJoinableAssociation assoc = new OuterJoinableAssociation(type, alias, aliasedLhsColumns, subalias, joinType, await (GetWithClauseAsync(path)), Factory, enabledFilters);
			assoc.ValidateJoin(path);
			AddAssociation(subalias, assoc);
			int nextDepth = currentDepth + 1;
			if (!joinable.IsCollection)
			{
				IOuterJoinLoadable pjl = joinable as IOuterJoinLoadable;
				if (pjl != null)
					await (WalkEntityTreeAsync(pjl, subalias, path, nextDepth));
			}
			else
			{
				IQueryableCollection qc = joinable as IQueryableCollection;
				if (qc != null)
					await (WalkCollectionTreeAsync(qc, subalias, path, nextDepth));
			}
		}

		private async Task AddAssociationToJoinTreeIfNecessaryAsync(IAssociationType type, string[] aliasedLhsColumns, string alias, string path, int currentDepth, JoinType joinType)
		{
			if (joinType >= JoinType.InnerJoin)
			{
				await (AddAssociationToJoinTreeAsync(type, aliasedLhsColumns, alias, path, currentDepth, joinType));
			}
		}

		protected async Task WalkComponentTreeAsync(IAbstractComponentType componentType, int begin, string alias, string path, int currentDepth, ILhsAssociationTypeSqlInfo associationTypeSQLInfo)
		{
			IType[] types = componentType.Subtypes;
			string[] propertyNames = componentType.PropertyNames;
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i].IsAssociationType)
				{
					var associationType = (IAssociationType)types[i];
					string[] aliasedLhsColumns = associationTypeSQLInfo.GetAliasedColumnNames(associationType, begin);
					string[] lhsColumns = associationTypeSQLInfo.GetColumnNames(associationType, begin);
					string lhsTable = associationTypeSQLInfo.GetTableName(associationType);
					string subpath = SubPath(path, propertyNames[i]);
					bool[] propertyNullability = componentType.PropertyNullability;
					JoinType joinType = GetJoinType(associationType, componentType.GetFetchMode(i), subpath, lhsTable, lhsColumns, propertyNullability == null || propertyNullability[i], currentDepth, componentType.GetCascadeStyle(i));
					await (AddAssociationToJoinTreeIfNecessaryAsync(associationType, aliasedLhsColumns, alias, subpath, currentDepth, joinType));
				}
				else if (types[i].IsComponentType)
				{
					string subpath = SubPath(path, propertyNames[i]);
					await (WalkComponentTreeAsync((IAbstractComponentType)types[i], begin, alias, subpath, currentDepth, associationTypeSQLInfo));
				}

				begin += types[i].GetColumnSpan(Factory);
			}
		}

		protected virtual async Task WalkEntityTreeAsync(IOuterJoinLoadable persister, string alias, string path, int currentDepth)
		{
			int n = persister.CountSubclassProperties();
			for (int i = 0; i < n; i++)
			{
				IType type = persister.GetSubclassPropertyType(i);
				ILhsAssociationTypeSqlInfo associationTypeSQLInfo = JoinHelper.GetLhsSqlInfo(alias, i, persister, Factory);
				if (type.IsAssociationType)
				{
					await (WalkEntityAssociationTreeAsync((IAssociationType)type, persister, i, alias, path, persister.IsSubclassPropertyNullable(i), currentDepth, associationTypeSQLInfo));
				}
				else if (type.IsComponentType)
				{
					await (WalkComponentTreeAsync((IAbstractComponentType)type, 0, alias, SubPath(path, persister.GetSubclassPropertyName(i)), currentDepth, associationTypeSQLInfo));
				}
			}
		}

		private async Task WalkCollectionTreeAsync(IQueryableCollection persister, string alias, string path, int currentDepth)
		{
			if (persister.IsOneToMany)
			{
				await (WalkEntityTreeAsync((IOuterJoinLoadable)persister.ElementPersister, alias, path, currentDepth));
			}
			else
			{
				IType type = persister.ElementType;
				if (type.IsAssociationType)
				{
					// a many-to-many
					// decrement currentDepth here to allow join across the association table
					// without exceeding MAX_FETCH_DEPTH (i.e. the "currentDepth - 1" bit)
					IAssociationType associationType = (IAssociationType)type;
					string[] aliasedLhsColumns = persister.GetElementColumnNames(alias);
					string[] lhsColumns = persister.ElementColumnNames;
					// if the current depth is 0, the root thing being loaded is the
					// many-to-many collection itself.  Here, it is alright to use
					// an inner join...
					bool useInnerJoin = currentDepth == 0;
					JoinType joinType = GetJoinType(associationType, persister.FetchMode, path, persister.TableName, lhsColumns, !useInnerJoin, currentDepth - 1, null);
					await (AddAssociationToJoinTreeIfNecessaryAsync(associationType, aliasedLhsColumns, alias, path, currentDepth - 1, joinType));
				}
				else if (type.IsComponentType)
				{
					await (WalkCompositeElementTreeAsync((IAbstractComponentType)type, persister.ElementColumnNames, persister, alias, path, currentDepth));
				}
			}
		}

		protected async Task WalkCollectionTreeAsync(IQueryableCollection persister, string alias)
		{
			await (WalkCollectionTreeAsync(persister, alias, string.Empty, 0));
		//TODO: when this is the entry point, we should use an INNER_JOIN for fetching the many-to-many elements!
		}

		protected async Task WalkEntityTreeAsync(IOuterJoinLoadable persister, string alias)
		{
			await (WalkEntityTreeAsync(persister, alias, string.Empty, 0));
		}

		private async Task WalkCompositeElementTreeAsync(IAbstractComponentType compositeType, string[] cols, IQueryableCollection persister, string alias, string path, int currentDepth)
		{
			IType[] types = compositeType.Subtypes;
			string[] propertyNames = compositeType.PropertyNames;
			int begin = 0;
			for (int i = 0; i < types.Length; i++)
			{
				int length = types[i].GetColumnSpan(factory);
				string[] lhsColumns = ArrayHelper.Slice(cols, begin, length);
				if (types[i].IsAssociationType)
				{
					IAssociationType associationType = types[i] as IAssociationType;
					// simple, because we can't have a one-to-one or collection
					// (or even a property-ref) in a composite element:
					string[] aliasedLhsColumns = StringHelper.Qualify(alias, lhsColumns);
					string subpath = SubPath(path, propertyNames[i]);
					bool[] propertyNullability = compositeType.PropertyNullability;
					JoinType joinType = GetJoinType(associationType, compositeType.GetFetchMode(i), subpath, persister.TableName, lhsColumns, propertyNullability == null || propertyNullability[i], currentDepth, compositeType.GetCascadeStyle(i));
					await (AddAssociationToJoinTreeIfNecessaryAsync(associationType, aliasedLhsColumns, alias, subpath, currentDepth, joinType));
				}
				else if (types[i].IsComponentType)
				{
					string subpath = SubPath(path, propertyNames[i]);
					await (WalkCompositeElementTreeAsync((IAbstractComponentType)types[i], lhsColumns, persister, alias, subpath, currentDepth));
				}

				begin += length;
			}
		}

		private async Task WalkEntityAssociationTreeAsync(IAssociationType associationType, IOuterJoinLoadable persister, int propertyNumber, string alias, string path, bool nullable, int currentDepth, ILhsAssociationTypeSqlInfo associationTypeSQLInfo)
		{
			string[] aliasedLhsColumns = associationTypeSQLInfo.GetAliasedColumnNames(associationType, 0);
			string[] lhsColumns = associationTypeSQLInfo.GetColumnNames(associationType, 0);
			string lhsTable = associationTypeSQLInfo.GetTableName(associationType);
			string subpath = SubPath(path, persister.GetSubclassPropertyName(propertyNumber));
			JoinType joinType = GetJoinType(associationType, persister.GetFetchMode(propertyNumber), subpath, lhsTable, lhsColumns, nullable, currentDepth, persister.GetCascadeStyle(propertyNumber));
			await (AddAssociationToJoinTreeIfNecessaryAsync(associationType, aliasedLhsColumns, alias, subpath, currentDepth, joinType));
		}
	}
}