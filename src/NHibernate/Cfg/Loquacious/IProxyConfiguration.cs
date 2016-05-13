using NHibernate.Bytecode;
namespace NHibernate.Cfg.Loquacious
{
	public partial interface IProxyConfiguration
	{
		IProxyConfiguration DisableValidation();
		IFluentSessionFactoryConfiguration Through<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory;
	}

	public partial interface IProxyConfigurationProperties
	{
		bool Validation { set; }
		void ProxyFactoryFactory<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory;
	}
}