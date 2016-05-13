namespace NHibernate.Cfg.Loquacious
{
	public partial interface IDbSchemaIntegrationConfiguration
	{
		IDbIntegrationConfiguration Recreating();
		IDbIntegrationConfiguration Creating();
		IDbIntegrationConfiguration Updating();
		IDbIntegrationConfiguration Validating();
	}
}