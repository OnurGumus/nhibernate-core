using NHibernate.Engine;

namespace NHibernate.AdoNet
{
	public partial class MySqlClientBatchingBatcherFactory : IBatcherFactory
	{
		public virtual IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
		{
			return new MySqlClientBatchingBatcher(connectionManager, interceptor);
		}
	}
}