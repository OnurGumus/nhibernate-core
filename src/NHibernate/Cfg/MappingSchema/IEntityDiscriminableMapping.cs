namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IEntityDiscriminableMapping
	{
		string DiscriminatorValue { get; }
	}
}