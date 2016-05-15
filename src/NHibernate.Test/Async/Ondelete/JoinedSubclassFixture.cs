#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Stat;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.Ondelete
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinedSubclassFixture : TestCase
	{
		[Test]
		public async Task JoinedSubclassCascadeAsync()
		{
			G g1 = new G("thing", "white", "10x10");
			F f1 = new F("thing2", "blue");
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(g1));
			await (s.SaveAsync(f1));
			await (t.CommitAsync());
			s.Close();
			IStatistics statistics = sessions.Statistics;
			statistics.Clear();
			s = OpenSession();
			t = s.BeginTransaction();
			IList<E> l = await (s.CreateQuery("from E").ListAsync<E>());
			statistics.Clear();
			await (s.DeleteAsync(l[0]));
			await (s.DeleteAsync(l[1]));
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(2, statistics.EntityDeleteCount);
			// In this case the batcher reuse the same command because have same SQL and same parametersTypes
			Assert.AreEqual(1, statistics.PrepareStatementCount);
		}
	}
}
#endif
