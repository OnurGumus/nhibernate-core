namespace NHibernate.Mapping
{
	/// <summary>
	/// Any mapping with an outer-join attribute
	/// </summary>
	public partial interface IFetchable
	{
		FetchMode FetchMode { get; set; }
		bool IsLazy { get; set; }
	}
}