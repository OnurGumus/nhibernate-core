#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2390
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestAsync()
		{
			var rowsUpdated = 0;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					rowsUpdated = await (s.CreateQuery("UPDATE VERSIONED Class1 c SET c.Property1 = :value1, c.Property2 = :value2, c.Property3 = :value3, c.Property4 = :value4, c.Property5 = :value5").SetParameter("value1", 1).SetParameter("value2", 2).SetParameter("value3", 3).SetParameter("value4", 4).SetParameter("value5", 5).ExecuteUpdateAsync());
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var class1 = (Class1)(await (s.CreateQuery("FROM Class1").UniqueResultAsync()));
					Assert.That(rowsUpdated, Is.EqualTo(1), "UPDATE did not alter the expected number of rows");
					Assert.That(class1.Property1, Is.EqualTo(1), "UPDATE did not alter Property1");
					Assert.That(class1.Property2, Is.EqualTo(2), "UPDATE did not alter Property2");
					Assert.That(class1.Property3, Is.EqualTo(3), "UPDATE did not alter Property3");
					Assert.That(class1.Property4, Is.EqualTo(4), "UPDATE did not alter Property4");
					Assert.That(class1.Property5, Is.EqualTo(5), "UPDATE did not alter Property5");
					Assert.That(class1.Version, Is.EqualTo(2), "UPDATE did not increment the version");
				}
		}
	}
}
#endif
