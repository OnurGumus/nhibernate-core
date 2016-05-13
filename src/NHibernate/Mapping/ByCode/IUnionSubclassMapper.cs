namespace NHibernate.Mapping.ByCode
{
	public partial interface IUnionSubclassAttributesMapper : IEntityAttributesMapper, IEntitySqlsMapper
	{
		void Table(string tableName);
		void Catalog(string catalogName);
		void Schema(string schemaName);
		void Extends(System.Type baseType);
		void Abstract(bool isAbstract);
	}

	public partial interface IUnionSubclassMapper : IUnionSubclassAttributesMapper, IPropertyContainerMapper {}

	public partial interface IUnionSubclassAttributesMapper<TEntity> : IEntityAttributesMapper, IEntitySqlsMapper where TEntity : class
	{
		void Table(string tableName);
		void Catalog(string catalogName);
		void Schema(string schemaName);
		void Extends(System.Type baseType);
		void Abstract(bool isAbstract);
	}

	public partial interface IUnionSubclassMapper<TEntity> : IUnionSubclassAttributesMapper<TEntity>, IPropertyContainerMapper<TEntity> where TEntity : class {}
}