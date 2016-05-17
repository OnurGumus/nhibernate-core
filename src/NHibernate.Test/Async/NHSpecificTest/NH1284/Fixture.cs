#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1284
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task EmptyValueTypeComponentAsync()
		{
			Person jimmy;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("Jimmy Hendrix");
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					jimmy = (Person)s.Get(typeof (Person), "Jimmy Hendrix");
					await (tx.CommitAsync());
				}

			Assert.IsFalse(jimmy.Address.HasValue);
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
