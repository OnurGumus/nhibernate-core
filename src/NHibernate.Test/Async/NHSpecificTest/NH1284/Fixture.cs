#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1284
{
	[TestFixture, Ignore("Not supported yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
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
					jimmy = (Person)await (s.GetAsync(typeof (Person), "Jimmy Hendrix"));
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
