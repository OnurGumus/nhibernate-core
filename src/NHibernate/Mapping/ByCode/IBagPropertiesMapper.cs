namespace NHibernate.Mapping.ByCode
{
	public partial interface IBagPropertiesMapper : ICollectionPropertiesMapper {}

	public partial interface IBagPropertiesMapper<TEntity, TElement> : ICollectionPropertiesMapper<TEntity, TElement>
	{}
}