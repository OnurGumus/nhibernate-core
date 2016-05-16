using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1605
{
	[TestFixture]
	public partial class Fixture
	{
		[Test]
		public void SupportTypedefInReturnScalarElements()
		{
			var cfg = new Configuration();
			Assembly assembly = Assembly.GetExecutingAssembly();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1605.Mappings.hbm.xml", assembly);
			using (cfg.BuildSessionFactory()) {}
		}
	}

	public enum CapitalCities
	{
		Amsterdam,
		Berlin,
		Cairo,
		Dublin
	}

	public partial class Country
	{
		public virtual CapitalCities CapitalCity { get; set; }
	}
}