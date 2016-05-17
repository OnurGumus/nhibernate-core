#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Stat;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Stats
{
	using Criterion;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SessionStatsFixture : TestCase
	{
		private static async Task<Continent> FillDbAsync(ISession s)
		{
			Continent europe = new Continent();
			europe.Name = "Europe";
			Country france = new Country();
			france.Name = "France";
			europe.Countries = new HashSet<Country>();
			europe.Countries.Add(france);
			await (s.SaveAsync(france));
			await (s.SaveAsync(europe));
			return europe;
		}

		private static async Task CleanDbAsync(ISession s)
		{
			await (s.DeleteAsync("from Country"));
			await (s.DeleteAsync("from Continent"));
		}

		[Test]
		public async Task SessionStatisticsAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			IStatistics stats = sessions.Statistics;
			stats.Clear();
			bool isStats = stats.IsStatisticsEnabled;
			stats.IsStatisticsEnabled = true;
			Continent europe = await (FillDbAsync(s));
			await (tx.CommitAsync());
			s.Clear();
			tx = s.BeginTransaction();
			ISessionStatistics sessionStats = s.Statistics;
			Assert.AreEqual(0, sessionStats.EntityKeys.Count);
			Assert.AreEqual(0, sessionStats.EntityCount);
			Assert.AreEqual(0, sessionStats.CollectionKeys.Count);
			Assert.AreEqual(0, sessionStats.CollectionCount);
			europe = await (s.GetAsync<Continent>(europe.Id));
			NHibernateUtil.Initialize(europe.Countries);
			IEnumerator itr = europe.Countries.GetEnumerator();
			itr.MoveNext();
			NHibernateUtil.Initialize(itr.Current);
			Assert.AreEqual(2, sessionStats.EntityKeys.Count);
			Assert.AreEqual(2, sessionStats.EntityCount);
			Assert.AreEqual(1, sessionStats.CollectionKeys.Count);
			Assert.AreEqual(1, sessionStats.CollectionCount);
			await (CleanDbAsync(s));
			await (tx.CommitAsync());
			s.Close();
			stats.IsStatisticsEnabled = isStats;
		}
	}
}
#endif
