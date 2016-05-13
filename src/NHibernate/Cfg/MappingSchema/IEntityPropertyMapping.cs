namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IEntityPropertyMapping: IDecoratable
	{
		string Name { get; }
		string Access { get; }
		bool OptimisticLock { get; }
		bool IsLazyProperty { get; }
	}
}