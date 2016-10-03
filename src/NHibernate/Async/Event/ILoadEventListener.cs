#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILoadEventListener
	{
		/// <summary> 
		/// Handle the given load event. 
		/// </summary>
		/// <param name = "event">The load event to be handled. </param>
		/// <param name = "loadType"></param>
		/// <returns> The result (i.e., the loaded entity). </returns>
		Task OnLoadAsync(LoadEvent @event, LoadType loadType);
	}
}
#endif
