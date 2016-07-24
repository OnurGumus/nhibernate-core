#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1495
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CreateTestAsync()
		{
			object id;
			using (ISession session = OpenSession())
			{
				var person = new Person{Name = "Nelo"};
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.SaveAsync(person));
					await (trans.CommitAsync());
				}

				id = person.Id;
			}

			using (ISession session = OpenSession())
			{
				var person = (IPerson)await (session.LoadAsync(typeof (Person), id)); //to work with the proxy
				Assert.IsNotNull(person);
				Assert.AreEqual("Nelo", person.Name);
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.DeleteAsync(person));
					await (trans.CommitAsync());
				}
			}
		}
	}
}
#endif
