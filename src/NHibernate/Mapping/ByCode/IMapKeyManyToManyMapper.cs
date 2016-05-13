namespace NHibernate.Mapping.ByCode
{
	public partial interface IMapKeyManyToManyMapper : IColumnsMapper
	{
		void ForeignKey(string foreignKeyName);
		void Formula(string formula);
	}
}