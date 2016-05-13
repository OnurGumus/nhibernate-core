namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IRelationship
	{
		string EntityName { get; }
		string Class { get; }
		HbmNotFoundMode NotFoundMode { get; }
	}
}