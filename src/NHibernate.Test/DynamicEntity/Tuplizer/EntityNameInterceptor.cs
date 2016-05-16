namespace NHibernate.Test.DynamicEntity.Tuplizer
{
	public partial class EntityNameInterceptor : EmptyInterceptor
	{
		public override string GetEntityName(object entity)
		{
			string entityName = ProxyHelper.ExtractEntityName(entity) ?? base.GetEntityName(entity);
			return entityName;
		}
	}
}