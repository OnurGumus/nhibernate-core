using System;
using System.Linq.Expressions;
using System.Collections;

namespace NHibernate.Cfg.Loquacious
{
	public partial interface IEntityCollectionCacheConfigurationProperties
	{
		EntityCacheUsage Strategy { get; set; }
		string RegionName { get; set; }
	}

	public partial interface IEntityCacheConfigurationProperties<TEntity> where TEntity: class
	{
		EntityCacheUsage? Strategy { get; set; }
		string RegionName { get; set; }

		void Collection<TCollection>(Expression<Func<TEntity, TCollection>> collectionProperty, Action<IEntityCollectionCacheConfigurationProperties> collectionCacheConfiguration)
			where TCollection : IEnumerable;
	}
}