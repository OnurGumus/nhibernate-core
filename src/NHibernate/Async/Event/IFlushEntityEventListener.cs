using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IFlushEntityEventListener
	{
		Task OnFlushEntityAsync(FlushEntityEvent @event);
	}
}