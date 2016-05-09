using NHibernate.Cfg.MappingSchema;

namespace NHibernate.Mapping.ByCode
{
	public abstract partial class CacheInclude
	{
		public static CacheInclude All = new AllCacheInclude();
		public static CacheInclude NonLazy = new NonLazyCacheInclude();

		internal abstract HbmCacheInclude ToHbm();

		#region Nested type: AllCacheInclude

		public partial class AllCacheInclude : CacheInclude
		{
			internal override HbmCacheInclude ToHbm()
			{
				return HbmCacheInclude.All;
			}
		}

		#endregion

		#region Nested type: NonLazyCacheInclude

		public partial class NonLazyCacheInclude : CacheInclude
		{
			internal override HbmCacheInclude ToHbm()
			{
				return HbmCacheInclude.NonLazy;
			}
		}

		#endregion
	}
}