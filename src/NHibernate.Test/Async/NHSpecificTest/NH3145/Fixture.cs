#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3145
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task QueryWithLazyBaseClassShouldNotThrowNoPersisterForErrorAsync()
		{
			try
			{
				using (var s = OpenSession())
					using (var t = s.BeginTransaction())
					{
						var item1 = new Derived{LongContent = "LongLongLongLongLong"};
						var root = new Root{Base = item1};
						await (s.SaveAsync(item1));
						await (s.SaveAsync(root));
						await (t.CommitAsync());
					}

				// This will succeed if either:
				// a) we do not initialize root.Base
				// or
				// b) Base.LongContent is made non-lazy (remove lazy properties)
				using (var s = OpenSession())
					using (var t = s.BeginTransaction())
					{
						var root = await (s.CreateQuery("from Root").UniqueResultAsync<Root>());
						await (NHibernateUtil.InitializeAsync(root.Base));
						var q = await (s.CreateQuery("from Derived d where d = ?").SetEntityAsync(0, root.Base));
						await (q.ListAsync());
					}
			}
			finally
			{
				using (var s = OpenSession())
					using (var t = s.BeginTransaction())
					{
						await (s.DeleteAsync("from Root"));
						await (s.DeleteAsync("from Derived"));
						await (t.CommitAsync());
					}
			}
		}
	}
}
#endif
