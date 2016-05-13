namespace NHibernate.Cfg.MappingSchema
{
	public partial interface ICollectionSqlsMapping
	{
		HbmLoader SqlLoader { get; }
		HbmCustomSQL SqlInsert { get; }
		HbmCustomSQL SqlUpdate { get; }
		HbmCustomSQL SqlDelete { get; }
		HbmCustomSQL SqlDeleteAll { get; }
		string Subselect { get; }
	}
}