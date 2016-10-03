#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3057
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var a = new AClass{Id = 1};
					await (session.SaveAsync(a));
					var b = new BClass{Id = 2, A = a, InheritedProperty = "B2"};
					await (session.SaveAsync(b));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task CollectionQueryOnJoinedSubclassInheritedPropertyHqlAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var entities = await (session.CreateQuery("from AClass a where exists (from a.Bs b where b.InheritedProperty = 'B2')").ListAsync<AClass>());
					Assert.AreEqual(1, entities.Count);
					Assert.AreEqual(1, entities[0].Id);
				}
		}
	}
}
#endif
