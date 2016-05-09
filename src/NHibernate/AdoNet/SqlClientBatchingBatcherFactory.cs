using NHibernate.Engine;

namespace NHibernate.AdoNet
{
	public partial class SqlClientBatchingBatcherFactory : IBatcherFactory
	{
		public virtual IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
		{
			return new SqlClientBatchingBatcher(connectionManager, interceptor);
		}
	}
}