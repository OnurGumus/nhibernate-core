#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2913
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
			{
				for (int x = 0; x < 10; x++)
				{
					var ci = new CostItem()
					{Units = x};
					await (session.SaveAsync(ci));
				}

				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object"));
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect as MsSql2008Dialect != null;
		}

		[Test]
		public void QueryShouldReturnResults()
		{
			using (var session = OpenSession())
			{
				var excludedCostItems = (
					from ci in session.Query<CostItem>()where ci.Id == 1
					select ci);
				var items = (
					from ci in session.Query<CostItem>()where !excludedCostItems.Any(c => c.Id == ci.Id)select ci).ToArray();
				Assert.That(items.Length, Is.EqualTo(9));
			}
		}
	}
}
#endif
