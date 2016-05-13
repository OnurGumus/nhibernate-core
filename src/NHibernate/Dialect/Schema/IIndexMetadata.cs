namespace NHibernate.Dialect.Schema
{
	public partial interface IIndexMetadata
	{
		string Name { get; }

		void AddColumn(IColumnMetadata column);

		IColumnMetadata[] Columns { get; }
	}
}