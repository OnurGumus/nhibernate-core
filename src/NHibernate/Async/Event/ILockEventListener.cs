#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILockEventListener
	{
		/// <summary>Handle the given lock event. </summary>
		/// <param name = "event">The lock event to be handled. </param>
		Task OnLockAsync(LockEvent @event);
	}
}
#endif
