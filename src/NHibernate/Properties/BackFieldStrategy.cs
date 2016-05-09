namespace NHibernate.Properties
{
	public partial class BackFieldStrategy : IFieldNamingStrategy
	{
		#region Implementation of IFieldNamingStrategy

		public string GetFieldName(string propertyName)
		{
			return string.Concat("<",propertyName.Trim(),">k__BackingField");
		}

		#endregion
	}
}