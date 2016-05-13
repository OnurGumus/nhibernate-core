using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary> Defines the contract for handling of session dirty-check events.</summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IDirtyCheckEventListener
	{
		/// <summary>Handle the given dirty-check event. </summary>
		/// <param name = "event">The dirty-check event to be handled. </param>
		Task OnDirtyCheckAsync(DirtyCheckEvent @event);
	}
}