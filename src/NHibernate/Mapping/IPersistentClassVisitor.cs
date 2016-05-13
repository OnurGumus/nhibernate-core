namespace NHibernate.Mapping
{
	public partial interface IPersistentClassVisitor
	{
		object Accept(PersistentClass clazz);
	}

	public partial interface IPersistentClassVisitor<T> where T : PersistentClass
	{
		object Accept(T clazz);
	}
}