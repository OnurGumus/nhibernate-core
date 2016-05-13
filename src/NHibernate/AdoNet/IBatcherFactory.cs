using NHibernate.Engine;

namespace NHibernate.AdoNet
{
	/// <summary> Factory for <see cref="IBatcher"/> instances.</summary>
	public partial interface IBatcherFactory
	{
		IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor);
	}
}