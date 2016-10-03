#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2439
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH2439FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Cfg.Environment.ShowSql, "true");
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
			{
				var organisation = new Organisation();
				var trainingComponent = new TrainingComponent{Code = "123", Title = "title"};
				var scope = new RtoScope{Nrt = trainingComponent, Rto = organisation, StartDate = DateTime.Today.AddDays(-100)};
				await (session.SaveAsync(organisation));
				await (session.SaveAsync(trainingComponent));
				await (session.SaveAsync(scope));
				var searchResult = new OrganisationSearchResult{Organisation = organisation};
				await (session.SaveAsync(searchResult));
				await (session.FlushAsync());
			}

			await (base.OnSetUpAsync());
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from OrganisationSearchResult"));
				await (session.DeleteAsync("from RtoScope"));
				await (session.DeleteAsync("from Organisation"));
				await (session.DeleteAsync("from TrainingComponent"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
