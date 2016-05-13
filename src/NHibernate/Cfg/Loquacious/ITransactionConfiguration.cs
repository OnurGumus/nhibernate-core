using NHibernate.Transaction;
namespace NHibernate.Cfg.Loquacious
{
	public partial interface ITransactionConfiguration
	{
		IDbIntegrationConfiguration Through<TFactory>() where TFactory : ITransactionFactory;
	}
}