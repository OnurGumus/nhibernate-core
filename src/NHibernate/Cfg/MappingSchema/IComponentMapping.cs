namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IComponentMapping : IPropertiesContainerMapping
	{
		string Class { get; }
		HbmParent Parent { get; }
		string EmbeddedNode { get; }
		string Name { get; }
	}
}