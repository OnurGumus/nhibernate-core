#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Criteria.Lambda
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleIntegrationFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Criteria.Lambda.Mappings.hbm.xml"};
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person{Name = "test person 1", Age = 20}));
					await (s.SaveAsync(new Person{Name = "test person 2", Age = 30}));
					await (s.SaveAsync(new Person{Name = "test person 3", Age = 40}));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from Child").ExecuteUpdateAsync());
					await (s.CreateQuery("update Person p set p.Father = null").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from Person").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from JoinedChild").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from Parent").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestQueryOverAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					Person personAlias = null;
					var actual = await (s.QueryOver<Person>(() => personAlias).Where(() => personAlias.Name == "test person 2").And(() => personAlias.Age == 30).ListAsync());
					Assert.That(actual.Count, Is.EqualTo(1));
				}
		}

		[Test]
		public async Task TestQueryOverAliasAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					Person personAlias = null;
					var actual = await (s.QueryOver<Person>(() => personAlias).Where(() => personAlias.Name == "test person 2").And(() => personAlias.Age == 30).ListAsync());
					Assert.That(actual.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
