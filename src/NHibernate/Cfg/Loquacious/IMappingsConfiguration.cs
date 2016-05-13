namespace NHibernate.Cfg.Loquacious
{
	public partial interface IMappingsConfiguration
	{
		IMappingsConfiguration UsingDefaultCatalog(string defaultCatalogName);
		IFluentSessionFactoryConfiguration UsingDefaultSchema(string defaultSchemaName);
	}

	public partial interface IMappingsConfigurationProperties
	{
		string DefaultCatalog { set; }
		string DefaultSchema { set; }
	}
}