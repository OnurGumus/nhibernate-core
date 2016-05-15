#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1898
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task TypeOfParametersShouldBeSetCorrectlyAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var entity = new DomainClass{Id = 1, Data = "some oldValue data"};
					await (session.SaveAsync(entity));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.GetNamedQuery("replaceQuery").SetString("old", "oldValue").SetString("new", "newValue").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}

				using (ITransaction tx = session.BeginTransaction())
				{
					var entity = await (session.GetAsync<DomainClass>(1));
					Assert.AreEqual("some newValue data", entity.Data);
					await (session.DeleteAsync(entity));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
