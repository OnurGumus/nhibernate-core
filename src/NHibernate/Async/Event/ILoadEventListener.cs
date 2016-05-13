using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of load events generated from a session. 
	/// </summary>
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