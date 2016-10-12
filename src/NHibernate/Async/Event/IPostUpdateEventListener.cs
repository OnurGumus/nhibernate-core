#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPostUpdateEventListener
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name = "event"></param>
		Task OnPostUpdateAsync(PostUpdateEvent @event);
	}
}
#endif
