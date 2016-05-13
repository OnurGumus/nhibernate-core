namespace NHibernate.Mapping
{
	public partial interface IValueVisitor
	{
		object Accept(IValue visited);
	}

	public partial interface IValueVisitor<T> : IValueVisitor where T: IValue 
	{
		object Accept(T visited);
	}
}