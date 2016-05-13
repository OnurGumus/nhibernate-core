namespace NHibernate
{
	public partial interface IFutureValue<T>
	{
		T Value { get; }
	}
}