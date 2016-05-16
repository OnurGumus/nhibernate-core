namespace NHibernate.Test.Generatedkeys.ByTrigger
{
	public partial class MyEntity
	{
		public virtual int Id { get; protected set; }

		public virtual string Name { get; set; }
	}
}