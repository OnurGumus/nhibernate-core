#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1868
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					cat = new Category{ValidUntil = DateTime.Now};
					await (session.SaveAsync(cat));
					package = new Package{ValidUntil = DateTime.Now};
					await (session.SaveAsync(package));
					await (tx.CommitAsync());
				}
			}
		}

		private Category cat;
		private Package package;
		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Category"));
					await (session.DeleteAsync("from Package"));
					await (tx.CommitAsync());
				}
			}

			await (base.OnTearDownAsync());
		}

		public async Task ExecuteQueryAsync(Action<ISession> sessionModifier)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					sessionModifier(session);
					await (session.RefreshAsync(cat));
					await (session.RefreshAsync(package));
					await ((await ((await (session.CreateQuery(@"
                    select 
                        inv
                    from 
                        Invoice inv
                        , Package p
                    where
                        p = :package
                        and inv.Category = :cat
                        and inv.ValidUntil > :now
                        and inv.Package = :package 
                    ").SetEntityAsync("cat", cat))).SetEntityAsync("package", package))).SetDateTime("now", DateTime.Now).UniqueResultAsync<Invoice>());
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task BugAsync()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterOnOffOnAsync()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s =>
			{
			}

			)));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterQueryTwiceAsync()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterQuery3Async()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}
	}
}
#endif
