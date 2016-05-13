using NHibernate.Bytecode;
namespace NHibernate.Cfg.Loquacious
{
	public partial interface ICollectionFactoryConfiguration
	{
		IFluentSessionFactoryConfiguration Through<TCollecionsFactory>() where TCollecionsFactory : ICollectionTypeFactory;
	}
}