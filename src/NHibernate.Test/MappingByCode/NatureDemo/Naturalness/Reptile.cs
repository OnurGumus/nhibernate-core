namespace NHibernate.Test.MappingByCode.NatureDemo.Naturalness
{
	public partial class Reptile : Animal
	{
		public virtual float BodyTemperature { get; set; }
	}

	public partial class Lizard : Reptile {}
}