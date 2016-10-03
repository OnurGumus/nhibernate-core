#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IDirtyCheckEventListener
	{
		/// <summary>Handle the given dirty-check event. </summary>
		/// <param name = "event">The dirty-check event to be handled. </param>
		Task OnDirtyCheckAsync(DirtyCheckEvent @event);
	}
}
#endif
