using NHibernate.Engine;

namespace NHibernate.Cache.Entry
{
	public partial interface ICacheEntryStructure
	{
		object Structure(object item);
		object Destructure(object map, ISessionFactoryImplementor factory);
	}
}