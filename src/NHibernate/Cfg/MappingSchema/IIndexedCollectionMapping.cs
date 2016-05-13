namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IIndexedCollectionMapping
	{
		HbmListIndex ListIndex { get; }
		HbmIndex Index { get; }
	}
}