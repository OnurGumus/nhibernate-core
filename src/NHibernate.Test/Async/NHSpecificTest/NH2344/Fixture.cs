#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2344
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CoalesceShouldWorkAsync()
		{
			int personId;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var p1 = new Person{Name = "inserted name"};
					var p2 = new Person{Name = null};
					await (s.SaveAsync(p1));
					await (s.SaveAsync(p2));
					personId = p2.Id;
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					var person = s.Query<Person>().Where(p => (p.Name ?? "e") == "e").First();
					Assert.AreEqual(personId, person.Id);
				}
		}
	}
}
#endif
