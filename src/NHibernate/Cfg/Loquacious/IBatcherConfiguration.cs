using NHibernate.AdoNet;
namespace NHibernate.Cfg.Loquacious
{
	public partial interface IBatcherConfiguration
	{
		IBatcherConfiguration Through<TBatcher>() where TBatcher : IBatcherFactory;
		IDbIntegrationConfiguration Each(short batchSize);
		IBatcherConfiguration OrderingInserts();
		IBatcherConfiguration DisablingInsertsOrdering();
	}
}