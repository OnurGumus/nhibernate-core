namespace NHibernate.Mapping.ByCode
{
	public enum Accessor
	{
		Property,
		Field,
		NoSetter,
		ReadOnly,
		None
	}

	public partial interface IAccessorPropertyMapper
	{
		void Access(Accessor accessor);
		void Access(System.Type accessorType);
	}
}