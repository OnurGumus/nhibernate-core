
namespace NHibernate.Test.MappingByCode.IntegrationTests.NH2728
{
	public partial class Dog : IAnimal
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int Walks { get; set; }
	}
}
