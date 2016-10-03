#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1775
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is Oracle8iDialect);
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new Member{FirstName = "Bob", LastName = "One", Roles = 1}));
					await (s.SaveAsync(new Member{FirstName = "Bob", LastName = "Two", Roles = 2}));
					await (s.SaveAsync(new Member{FirstName = "Bob", LastName = "Four", Roles = 4}));
					await (s.SaveAsync(new Member{FirstName = "Bob", LastName = "OneAndFour", Roles = 5}));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task BitwiseOperationsShouldBeSupportedAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					// &
					IList<DTO> result = await (s.CreateQuery("select new DTO(m.Id, concat(m.FirstName, ' ', m.LastName)) from Member m where (m.Roles & :roles) = :roles").SetInt32("roles", 1).ListAsync<DTO>());
					Assert.AreEqual(2, result.Count);
					Assert.IsTrue((result[0].Name == "Bob One" && result[1].Name == "Bob OneAndFour") || (result[0].Name == "Bob OneAndFour" && result[1].Name == "Bob One"));
					// |
					result = await (s.CreateQuery("select new DTO(m.Id, concat(m.FirstName, ' ', m.LastName)) from Member m where (m.Roles & (:firstRole | :secondRole)) = (:firstRole | :secondRole)").SetInt32("firstRole", 1).SetInt32("secondRole", 4).ListAsync<DTO>());
					Assert.AreEqual(1, result.Count);
					Assert.AreEqual("Bob OneAndFour", result[0].Name);
					// !
					result = await (s.CreateQuery("select new DTO(m.Id, concat(m.FirstName, ' ', m.LastName)) from Member m where (m.Roles & (!(!:roles))) = :roles").SetInt32("roles", 1).ListAsync<DTO>());
					Assert.AreEqual(2, result.Count);
					Assert.IsTrue((result[0].Name == "Bob One" && result[1].Name == "Bob OneAndFour") || (result[0].Name == "Bob OneAndFour" && result[1].Name == "Bob One"));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Member"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
