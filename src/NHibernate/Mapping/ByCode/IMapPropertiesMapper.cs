namespace NHibernate.Mapping.ByCode
{
	public partial interface IMapPropertiesMapper : ICollectionPropertiesMapper {}

	public partial interface IMapPropertiesMapper<TEntity, TKey, TElement> : ICollectionPropertiesMapper<TEntity, TElement>
	{}
}