using System;
namespace NHibernate.Mapping.ByCode
{
	public partial interface IIdBagPropertiesMapper : ICollectionPropertiesMapper
	{
		void Id(Action<ICollectionIdMapper> idMapping);
	}

	public partial interface IIdBagPropertiesMapper<TEntity, TElement> : ICollectionPropertiesMapper<TEntity, TElement>
	{
		void Id(Action<ICollectionIdMapper> idMapping);		
	}
}