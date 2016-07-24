#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1693
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from Invoice"));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.SaveAsync(new Invoice{Mode = "a", Num = 1, Category = 10}));
					await (session.SaveAsync(new Invoice{Mode = "a", Num = 2, Category = 10}));
					await (session.SaveAsync(new Invoice{Mode = "a", Num = 3, Category = 20}));
					await (session.SaveAsync(new Invoice{Mode = "a", Num = 4, Category = 10}));
					await (session.SaveAsync(new Invoice{Mode = "b", Num = 2, Category = 10}));
					await (session.SaveAsync(new Invoice{Mode = "b", Num = 3, Category = 10}));
					await (session.SaveAsync(new Invoice{Mode = "b", Num = 5, Category = 10}));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task without_filterAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var q1 = "from Invoice i where i.Mode='a' and i.Category=:cat and not exists (from Invoice i2 where i2.Mode='a' and i2.Category=:cat and i2.Num=i.Num+1)";
					var list = await (session.CreateQuery(q1).SetParameter("cat", 10).ListAsync<Invoice>());
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
					var list = await (session.CreateQuery(q1).SetParameter("cat", 10).ListAsync<Invoice>());
					Assert.That(list.Count, Is.EqualTo(2));
					Assert.That(list[0].Num == 2 && list[0].Mode == "a");
					Assert.That(list[1].Num == 4 && list[1].Mode == "a");
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
