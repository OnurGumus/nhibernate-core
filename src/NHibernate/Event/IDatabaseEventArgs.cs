namespace NHibernate.Event
{
	public partial interface IDatabaseEventArgs
	{
		/// <summary> 
		/// Returns the session event source for this event.  
		/// This is the underlying session from which this event was generated.
		/// </summary>
		IEventSource Session { get; }
	}
}
