namespace NHibernate.Dialect.Schema
{
	public partial interface IForeignKeyMetadata
	{
		string Name { get; }

		void AddColumn(IColumnMetadata column);

		IColumnMetadata[] Columns { get; }
	}
}