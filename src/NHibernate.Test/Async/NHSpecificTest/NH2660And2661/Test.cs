#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2660And2661
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				DomainClass entity = new DomainClass{Id = 1, Data = DateTime.Parse("10:00")};
				await (session.SaveAsync(entity));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				await (session.CreateQuery("delete from DomainClass").ExecuteUpdateAsync());
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2008Dialect;
		}

		protected override void Configure(Configuration configuration)
		{
			// to be sure we are using the new drive
			base.Configure(configuration);
			configuration.DataBaseIntegration(x => x.Driver<Sql2008ClientDriver>());
		}

		[Test]
		public async Task ShouldBeAbleToQueryEntityAsync()
		{
			using (ISession session = OpenSession())
			{
				var query = session.CreateQuery(@"from DomainClass entity where Data = :data");
				query.SetParameter("data", DateTime.Parse("10:00"), NHibernateUtil.Time);
				Assert.That(async () => await (query.ListAsync()), Throws.Nothing);
			}
		}
	}
}
#endif
