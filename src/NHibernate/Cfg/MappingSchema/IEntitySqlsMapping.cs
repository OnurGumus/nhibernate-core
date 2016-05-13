namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IEntitySqlsMapping
	{
		HbmLoader SqlLoader { get; }
		HbmCustomSQL SqlInsert { get; }
		HbmCustomSQL SqlUpdate { get; }
		HbmCustomSQL SqlDelete { get; }
		string Subselect { get; }
	}
}