namespace NHibernate.Cfg
{
	public partial interface IHibernateConfiguration
	{
		string ByteCodeProviderType { get; }
		bool UseReflectionOptimizer { get; }
		ISessionFactoryConfiguration SessionFactory { get; }
	}
}
