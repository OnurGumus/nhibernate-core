namespace NHibernate.Test.NHSpecificTest.NH1969
{
	/// <summary>
	/// Author : Stephane Verlet
	/// </summary>
	public partial class EntityWithTypeProperty
	{
		public int Id { get; set; }

		public System.Type TypeValue { get; set; }
	}
}