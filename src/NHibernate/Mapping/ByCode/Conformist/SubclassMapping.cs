using NHibernate.Mapping.ByCode.Impl;
using NHibernate.Mapping.ByCode.Impl.CustomizersImpl;

namespace NHibernate.Mapping.ByCode.Conformist
{
	public partial class SubclassMapping<T> : SubclassCustomizer<T> where T : class
	{
		public SubclassMapping() : base(new ExplicitDeclarationsHolder(), new CustomizersHolder()) { }
	}
}