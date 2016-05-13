using System;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IJoinedSubclassAttributesMapper : IEntityAttributesMapper, IEntitySqlsMapper
	{
		void Table(string tableName);
		void Catalog(string catalogName);
		void Schema(string schemaName);
		void Key(Action<IKeyMapper> keyMapping);
		void Extends(System.Type baseType);
		void SchemaAction(SchemaAction action);
		void Filter(string filterName, Action<IFilterMapper> filterMapping);
		void Abstract(bool isAbstract);
	}

	public partial interface IJoinedSubclassMapper : IJoinedSubclassAttributesMapper, IPropertyContainerMapper {}

	public partial interface IJoinedSubclassAttributesMapper<TEntity> : IEntityAttributesMapper, IEntitySqlsMapper where TEntity : class
	{
		void Abstract(bool isAbstract);
		void Extends(System.Type baseType);
		void Table(string tableName);
		void Catalog(string catalogName);
		void Schema(string schemaName);
		void Key(Action<IKeyMapper<TEntity>> keyMapping);
		void SchemaAction(SchemaAction action);
        void Filter(string filterName, Action<IFilterMapper> filterMapping);
	}

	public partial interface IJoinedSubclassMapper<TEntity> : IJoinedSubclassAttributesMapper<TEntity>, IPropertyContainerMapper<TEntity> where TEntity : class {}
}