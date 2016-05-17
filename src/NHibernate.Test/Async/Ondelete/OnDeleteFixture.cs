#if NET_4_5
using System.Collections;
using NHibernate.Stat;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Ondelete
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OnDeleteFixture : TestCase
	{
		[Test]
		public async Task JoinedSubclassAsync()
		{
			IStatistics statistics = sessions.Statistics;
			statistics.Clear();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Salesperson mark = new Salesperson();
			mark.Name = "Mark";
			mark.Title = "internal sales";
			mark.Sex = 'M';
			mark.Address.address = "buckhead";
			mark.Address.zip = "30305";
			mark.Address.country = "USA";
			Person joe = new Person();
			joe.Name = "Joe";
			joe.Address.address = "San Francisco";
			joe.Address.zip = "XXXXX";
			joe.Address.country = "USA";
			joe.Sex = 'M';
			joe.Salesperson = mark;
			mark.Customers.Add(joe);
			await (s.SaveAsync(mark));
			await (t.CommitAsync());
			Assert.AreEqual(2, statistics.EntityInsertCount);
			Assert.IsTrue(5 >= statistics.PrepareStatementCount);
			statistics.Clear();
			t = s.BeginTransaction();
			await (s.DeleteAsync(mark));
			await (t.CommitAsync());
			Assert.AreEqual(2, statistics.EntityDeleteCount);
			Assert.AreEqual(1, statistics.PrepareStatementCount);
			t = s.BeginTransaction();
			IList names = s.CreateQuery("select p.name from Person p").List();
			Assert.AreEqual(0, names.Count);
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
