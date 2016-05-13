namespace NHibernate.Persister.Collection
{
	public partial interface ISqlLoadableCollection : IQueryableCollection
	{
		string[] GetCollectionPropertyColumnAliases(string propertyName, string str);
		string IdentifierColumnName { get; }
	}
}
