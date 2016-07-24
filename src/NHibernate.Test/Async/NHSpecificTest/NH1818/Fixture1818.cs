#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1818
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture1818Async : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
			{
				await (session.SaveAsync(new DomainClass{Id = 1}));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object"));
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect as PostgreSQL82Dialect != null;
		}

		[Test]
		[Description("Test HQL query on a property mapped with a formula.")]
		public async Task ComputedPropertyShouldRetrieveDataCorrectlyAsync()
		{
			using (var session = OpenSession())
			{
				var obj = await (session.CreateQuery("from DomainClass dc where dc.AlwaysTrue").UniqueResultAsync<DomainClass>());
				Assert.IsNotNull(obj);
			}
		}
	}
}
#endif
