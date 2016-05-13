namespace NHibernate.Mapping.ByCode
{
	public partial interface IFilterMapper
	{
		void Condition(string sqlCondition);
	}
}