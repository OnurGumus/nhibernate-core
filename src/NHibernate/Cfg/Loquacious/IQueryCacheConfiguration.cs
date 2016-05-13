using NHibernate.Cache;

namespace NHibernate.Cfg.Loquacious
{
	public partial interface IQueryCacheConfiguration
	{
		ICacheConfiguration Through<TFactory>() where TFactory : IQueryCache;
	}
}