namespace NHibernate.Mapping.ByCode
{
	public partial interface ISetPropertiesMapper : ICollectionPropertiesMapper {}

	public partial interface ISetPropertiesMapper<TEntity, TElement> : ICollectionPropertiesMapper<TEntity, TElement>
	{}
}