#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2378
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var entity = new TestEntity();
					entity.Id = 1;
					entity.Name = "Test Entity";
					entity.TestPerson = new Person{Id = 1, Name = "TestUser"};
					await (session.SaveAsync(entity));
					var entity1 = new TestEntity();
					entity1.Id = 2;
					entity1.Name = "Test Entity";
					entity1.TestPerson = new Person{Id = 2, Name = "TestUser"};
					await (session.SaveAsync(entity1));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
