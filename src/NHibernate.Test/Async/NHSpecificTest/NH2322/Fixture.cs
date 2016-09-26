#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Event;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2322
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.FormatSql, "false");
			configuration.SetListener(ListenerType.PostUpdate, new PostUpdateEventListener());
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Person"));
				await (s.FlushAsync());
			}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task ShouldNotThrowWhenCommitingATransactionAsync()
		{
			int id;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var p = new Person{Name = "inserted name"};
					await (s.SaveAsync(p));
					id = p.Id;
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var p = await (s.GetAsync<Person>(id));
					p.Name = "changing the name...";
					Assert.That(async delegate ()
					{
						await (t.CommitAsync());
					}

					, Throws.Nothing);
				}
		}
	}
}
#endif
