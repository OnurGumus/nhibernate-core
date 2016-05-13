using System;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IListPropertiesMapper : ICollectionPropertiesMapper
	{
		void Index(Action<IListIndexMapper> listIndexMapping);
	}

	public partial interface IListPropertiesMapper<TEntity, TElement> : ICollectionPropertiesMapper<TEntity, TElement>
	{
		void Index(Action<IListIndexMapper> listIndexMapping);
	}
}