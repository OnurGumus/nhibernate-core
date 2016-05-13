namespace NHibernate.Mapping.ByCode
{
	public partial interface IEntityPropertyMapper : IAccessorPropertyMapper
	{
		void OptimisticLock(bool takeInConsiderationForOptimisticLock);
	}
}