#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1693
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task without_filterAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var q1 = "from Invoice i where i.Mode='a' and i.Category=:cat and not exists (from Invoice i2 where i2.Mode='a' and i2.Category=:cat and i2.Num=i.Num+1)";
					var list = session.CreateQuery(q1).SetParameter("cat", 10).List<Invoice>();
					Assert.That(list.Count, Is.EqualTo(2));
					Assert.That(list[0].Num == 2 && list[0].Mode == "a");
					Assert.That(list[1].Num == 4 && list[1].Mode == "a");
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task with_filterAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					session.EnableFilter("modeFilter").SetParameter("currentMode", "a");
					var q1 = "from Invoice i where i.Category=:cat and not exists (from Invoice i2 where i2.Category=:cat and i2.Num=i.Num+1)";
					var list = session.CreateQuery(q1).SetParameter("cat", 10).List<Invoice>();
					Assert.That(list.Count, Is.EqualTo(2));
					Assert.That(list[0].Num == 2 && list[0].Mode == "a");
					Assert.That(list[1].Num == 4 && list[1].Mode == "a");
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
