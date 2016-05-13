namespace NHibernate.Mapping.ByCode
{
	public partial interface IDynamicComponentAttributesMapper : IEntityPropertyMapper
	{
		void Update(bool consideredInUpdateQuery);
		void Insert(bool consideredInInsertQuery);
		void Unique(bool unique);
	}

	public partial interface IDynamicComponentMapper : IDynamicComponentAttributesMapper, IPropertyContainerMapper { }

	public partial interface IDynamicComponentAttributesMapper<TComponent> : IEntityPropertyMapper
	{
		void Update(bool consideredInUpdateQuery);
		void Insert(bool consideredInInsertQuery);
		void Unique(bool unique);
	}

	public partial interface IDynamicComponentMapper<TComponent> : IDynamicComponentAttributesMapper<TComponent>, IPropertyContainerMapper<TComponent>
	{ }

}