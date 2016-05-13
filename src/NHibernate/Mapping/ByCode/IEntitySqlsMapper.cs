namespace NHibernate.Mapping.ByCode
{
	public partial interface IEntitySqlsMapper
	{
		void Loader(string namedQueryReference);
		void SqlInsert(string sql);
		void SqlUpdate(string sql);
		void SqlDelete(string sql);
		void Subselect(string sql);
	}
}