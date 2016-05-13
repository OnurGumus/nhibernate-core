namespace NHibernate.Mapping.ByCode
{
	public partial interface IOneToManyMapper
	{
		void Class(System.Type entityType);
		void EntityName(string entityName);
		void NotFound(NotFoundMode mode);
	}
}