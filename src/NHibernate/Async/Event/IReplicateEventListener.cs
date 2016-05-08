using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of replicate events generated from a session.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IReplicateEventListener
	{
		Task OnReplicateAsync(ReplicateEvent @event);
	}
}