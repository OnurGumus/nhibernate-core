using NHibernate.Mapping.ByCode.Impl;
using NHibernate.Mapping.ByCode.Impl.CustomizersImpl;

namespace NHibernate.Mapping.ByCode.Conformist
{
	public partial class JoinedSubclassMapping<T> : JoinedSubclassCustomizer<T> where T : class
	{
		public JoinedSubclassMapping() : base(new ExplicitDeclarationsHolder(), new CustomizersHolder()) { }
	}
}