﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.Naturalid.Immutable
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class ImmutableNaturalIdFixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new string[] {"Naturalid.Immutable.User.hbm.xml"}; }
		}

		protected override void Configure(Configuration configuration)
		{
			cfg.SetProperty(Environment.UseSecondLevelCache, "true");
			cfg.SetProperty(Environment.UseQueryCache, "true");
			cfg.SetProperty(Environment.GenerateStatistics, "true");
		}

		[Test]
		public async Task UpdateAsync()
		{
			// prepare some test data...
			User user;
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				user = new User();
				user.UserName = "steve";
				user.Email = "steve@hibernate.org";
				user.Password = "brewhaha";
				await (session.SaveAsync(user, CancellationToken.None));
				await (session.Transaction.CommitAsync(CancellationToken.None));
			}
			// 'user' is now a detached entity, so lets change a property and reattch...
			user.Password = "homebrew";
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				await (session.UpdateAsync(user, CancellationToken.None));
				await (session.Transaction.CommitAsync(CancellationToken.None));
			}

			// clean up
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				await (session.DeleteAsync(user, CancellationToken.None));
				await (session.Transaction.CommitAsync(CancellationToken.None));
			}
		}

		[Test]
		public async Task NaturalIdCheckAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();

			User u = new User("steve", "superSecret");
			await (s.PersistAsync(u, CancellationToken.None));
			u.UserName = "Steve";
			try
			{
				await (s.FlushAsync(CancellationToken.None));
				Assert.Fail();
			}
			catch (HibernateException) {}
			u.UserName = "steve";
			await (s.DeleteAsync(u, CancellationToken.None));
			await (t.CommitAsync(CancellationToken.None));
			s.Close();
		}

		[Test]
		public async Task NaturalIdCacheAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			User u = new User("steve", "superSecret");
			await (s.PersistAsync(u, CancellationToken.None));
			await (s.Transaction.CommitAsync(CancellationToken.None));
			s.Close();

			sessions.Statistics.Clear();

			s = OpenSession();
			s.BeginTransaction();
			u =
				(User)
				await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("UserName", "steve")).SetCacheable(true).
					UniqueResultAsync(CancellationToken.None));
			Assert.That(u, Is.Not.Null);
			await (s.Transaction.CommitAsync(CancellationToken.None));
			s.Close();

			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCachePutCount);

			s = OpenSession();
			s.BeginTransaction();
			User v = new User("gavin", "supsup");
			await (s.PersistAsync(v, CancellationToken.None));
			await (s.Transaction.CommitAsync(CancellationToken.None));
			s.Close();

			sessions.Statistics.Clear();

			s = OpenSession();
			s.BeginTransaction();
			u =
				(User)
				await (s.CreateCriteria(typeof(User)).Add(Restrictions.NaturalId().Set("UserName", "steve")).SetCacheable(true).
					UniqueResultAsync(CancellationToken.None));
			Assert.That(u, Is.Not.Null);
			Assert.AreEqual(0, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCacheHitCount);
			u =
				(User)
				await (s.CreateCriteria(typeof(User)).Add(Restrictions.NaturalId().Set("UserName", "steve")).SetCacheable(true).
					UniqueResultAsync(CancellationToken.None));
			Assert.That(u, Is.Not.Null);
			Assert.AreEqual(0, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(2, sessions.Statistics.QueryCacheHitCount);
			await (s.Transaction.CommitAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();
			s.BeginTransaction();
			await (s.DeleteAsync("from User", CancellationToken.None));
			await (s.Transaction.CommitAsync(CancellationToken.None));
			s.Close();
		}
	}
}