#if NET_4_5
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OnUpdateVisitor : ReattachVisitor
	{
		internal override Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			try
			{
				return Task.FromResult<object>(ProcessCollection(collection, type));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
