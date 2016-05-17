#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2490
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BadSqlFromJoinLogicErrorAsync()
		{
			try
			{
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						Derived item1 = new Derived()
						{ShortContent = "Short", ShortContent2 = "Short2", LongContent = "LongLongLongLongLong", LongContent2 = "LongLongLongLongLong2", };
						await (s.SaveAsync(item1));
						await (t.CommitAsync());
					}

				// this is the real meat of the test
				// for most edifying results, run this with show_sql enabled
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						var q = s.CreateQuery("from Base");
						Assert.That(() => q.List(), Throws.Nothing);
					}
			}
			finally
			{
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						await (s.DeleteAsync("from Derived"));
						await (t.CommitAsync());
					}
			}
		}
	}
}
#endif
