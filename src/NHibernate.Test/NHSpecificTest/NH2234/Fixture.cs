using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2234
{
  public partial class SomethingLinq
	{
		public virtual string Name { get; set; }
		public virtual MyUsertype Relation { get; set; }
	}

	[TestFixture]
	public partial class Fixture: BugTestCase
	{
	  [Test]
	  public void CanQueryViaLinq()
	  {
		using (var s = OpenSession())
		{
		var qry = from item in s.Query<SomethingLinq>() where item.Relation == MyUserTypes.Value1 select item;

			qry.ToList();
			Assert.That(() => qry.ToList(), Throws.Nothing);
		}
	  }
	}
}
