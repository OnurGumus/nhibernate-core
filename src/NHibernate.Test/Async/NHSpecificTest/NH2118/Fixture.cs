#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2118
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = sessions.OpenStatelessSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.InsertAsync(new Person{FirstName = "Bart", LastName = "Simpson"}));
					await (s.InsertAsync(new Person{FirstName = "Homer", LastName = "Simpson"}));
					await (s.InsertAsync(new Person{FirstName = "Apu", LastName = "Nahasapeemapetilon"}));
					await (s.InsertAsync(new Person{FirstName = "Montgomery ", LastName = "Burns"}));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public void CanGroupByWithoutSelect()
		{
			using (var s = sessions.OpenSession())
				using (s.BeginTransaction())
				{
					var groups = s.Query<Person>().GroupBy(p => p.LastName).ToList();
					Assert.AreEqual(3, groups.Count);
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var s = sessions.OpenStatelessSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from Person").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
