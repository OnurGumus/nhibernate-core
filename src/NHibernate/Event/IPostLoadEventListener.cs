namespace NHibernate.Event
{
	/// <summary>
	/// Occurs after an an entity instance is fully loaded.
	/// </summary>
	public partial interface IPostLoadEventListener
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="event"></param>
		void OnPostLoad(PostLoadEvent @event);
	}
}