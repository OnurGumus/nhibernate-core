#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1775
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BitwiseOperationsShouldBeSupportedAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					// &
					IList<DTO> result = s.CreateQuery("select new DTO(m.Id, concat(m.FirstName, ' ', m.LastName)) from Member m where (m.Roles & :roles) = :roles").SetInt32("roles", 1).List<DTO>();
					Assert.AreEqual(2, result.Count);
					Assert.IsTrue((result[0].Name == "Bob One" && result[1].Name == "Bob OneAndFour") || (result[0].Name == "Bob OneAndFour" && result[1].Name == "Bob One"));
					// |
					result = s.CreateQuery("select new DTO(m.Id, concat(m.FirstName, ' ', m.LastName)) from Member m where (m.Roles & (:firstRole | :secondRole)) = (:firstRole | :secondRole)").SetInt32("firstRole", 1).SetInt32("secondRole", 4).List<DTO>();
					Assert.AreEqual(1, result.Count);
					Assert.AreEqual("Bob OneAndFour", result[0].Name);
					// !
					result = s.CreateQuery("select new DTO(m.Id, concat(m.FirstName, ' ', m.LastName)) from Member m where (m.Roles & (!(!:roles))) = :roles").SetInt32("roles", 1).List<DTO>();
					Assert.AreEqual(2, result.Count);
					Assert.IsTrue((result[0].Name == "Bob One" && result[1].Name == "Bob OneAndFour") || (result[0].Name == "Bob OneAndFour" && result[1].Name == "Bob One"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
