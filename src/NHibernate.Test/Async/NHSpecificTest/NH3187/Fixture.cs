#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3187
{
	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var station = new Station();
					await (session.SaveAsync(station));
					await (session.SaveAsync(new Policeman{Name = "2Bob", Station = station, IsDeleted = true}));
					await (session.SaveAsync(new Policeman{Name = "1Sally", Station = station}));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public void CanGetCountOfPolicemen()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var station = session.Query<Station>().Single();
					var policemen = station.Policemen;
					Assert.AreEqual(1, station.Policemen.Count());
					foreach (var policeman in policemen)
					{
						Assert.NotNull(policeman);
					}
				}
		}
	}
}
#endif
