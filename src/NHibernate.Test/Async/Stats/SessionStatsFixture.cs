﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using NHibernate.Stat;
using NUnit.Framework;

namespace NHibernate.Test.Stats
{
	using Criterion;
	using System.Threading.Tasks;
	using System.Threading;

	[TestFixture]
	public class SessionStatsFixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new string[] { "Stats.Continent2.hbm.xml" }; }
		}

		private static async Task<Continent> FillDbAsync(ISession s, CancellationToken cancellationToken = default(CancellationToken))
		{
			Continent europe = new Continent();
			europe.Name="Europe";
			Country france = new Country();
			france.Name="France";
			europe.Countries= new HashSet<Country>();
			europe.Countries.Add(france);
			await (s.SaveAsync(france, cancellationToken));
			await (s.SaveAsync(europe, cancellationToken));
			return europe;
		}

		private static async Task CleanDbAsync(ISession s, CancellationToken cancellationToken = default(CancellationToken))
		{
			await (s.DeleteAsync("from Country", cancellationToken));
			await (s.DeleteAsync("from Continent", cancellationToken));
		}

		[Test]
		public async Task Can_use_cached_query_that_return_no_resultsAsync()
		{
			Assert.IsTrue(sessions.Settings.IsQueryCacheEnabled);

			using(ISession s = OpenSession())
			{
				IList list = await (s.CreateCriteria(typeof (Country))
					.Add(Restrictions.Eq("Name", "Narnia"))
					.SetCacheable(true)
					.ListAsync());

				Assert.AreEqual(0, list.Count);
			}

			using (ISession s = OpenSession())
			{
				IList list = await (s.CreateCriteria(typeof(Country))
					.Add(Restrictions.Eq("Name", "Narnia"))
					.SetCacheable(true)
					.ListAsync());

				Assert.AreEqual(0, list.Count);
			}
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
			await (NHibernateUtil.InitializeAsync(europe.Countries));
			IEnumerator itr = europe.Countries.GetEnumerator();
			itr.MoveNext();
			await (NHibernateUtil.InitializeAsync(itr.Current));
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
