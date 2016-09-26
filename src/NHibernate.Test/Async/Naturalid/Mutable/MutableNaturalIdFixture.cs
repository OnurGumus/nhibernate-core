#if NET_4_5
using System;
using System.Collections;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.Naturalid.Mutable
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MutableNaturalIdFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"Naturalid.Mutable.User.hbm.xml"};
			}
		}

		protected override void Configure(Configuration configuration)
		{
			cfg.SetProperty(Environment.UseSecondLevelCache, "true");
			cfg.SetProperty(Environment.UseQueryCache, "true");
			cfg.SetProperty(Environment.GenerateStatistics, "true");
		}

		[Test]
		public async Task ReattachmentNaturalIdCheckAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			User u = new User("gavin", "hb", "secret");
			await (s.PersistAsync(u));
			await (s.Transaction.CommitAsync());
			s.Close();
			FieldInfo name = u.GetType().GetField("name", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
			name.SetValue(u, "Gavin");
			s = OpenSession();
			s.BeginTransaction();
			try
			{
				await (s.UpdateAsync(u));
				await (s.Transaction.CommitAsync());
			}
			catch (HibernateException)
			{
				s.Transaction.Rollback();
			}
			catch (Exception)
			{
				s.Transaction.Rollback();
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			s.BeginTransaction();
			await (s.DeleteAsync(u));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task NonexistentNaturalIdCacheAsync()
		{
			sessions.Statistics.Clear();
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			object nullUser = await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "gavin").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			Assert.That(nullUser, Is.Null);
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCachePutCount);
			s = OpenSession();
			t = s.BeginTransaction();
			User u = new User("gavin", "hb", "secret");
			await (s.PersistAsync(u));
			await (t.CommitAsync());
			s.Close();
			sessions.Statistics.Clear();
			s = OpenSession();
			t = s.BeginTransaction();
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "gavin").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCachePutCount);
			sessions.Statistics.Clear();
			s = OpenSession();
			t = s.BeginTransaction();
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "gavin").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			await (s.DeleteAsync(u));
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(0, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCacheHitCount);
			sessions.Statistics.Clear();
			s = OpenSession();
			t = s.BeginTransaction();
			nullUser = await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "gavin").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			Assert.That(nullUser, Is.Null);
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCachePutCount);
		}

		[Test]
		public async Task NaturalIdCacheAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			User u = new User("gavin", "hb", "secret");
			await (s.PersistAsync(u));
			await (t.CommitAsync());
			s.Close();
			sessions.Statistics.Clear();
			s = OpenSession();
			t = s.BeginTransaction();
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "gavin").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			await (t.CommitAsync());
			s.Close();
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCachePutCount);
			s = OpenSession();
			t = s.BeginTransaction();
			User v = new User("xam", "hb", "foobar");
			await (s.PersistAsync(v));
			await (t.CommitAsync());
			s.Close();
			sessions.Statistics.Clear();
			s = OpenSession();
			t = s.BeginTransaction();
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "xam").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(0, sessions.Statistics.QueryCacheHitCount);
			u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.NaturalId().Set("name", "gavin").Set("org", "hb")).SetCacheable(true).UniqueResultAsync());
			Assert.That(u, Is.Not.Null);
			Assert.AreEqual(1, sessions.Statistics.QueryExecutionCount);
			Assert.AreEqual(1, sessions.Statistics.QueryCacheHitCount);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync("from User"));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task QueryingAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			User u = new User("emmanuel", "hb", "bh");
			await (s.PersistAsync(u));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			u = (User)await (s.CreateQuery("from User u where u.name = :name").SetParameter("name", "emmanuel").UniqueResultAsync());
			Assert.AreEqual("emmanuel", u.Name);
			await (s.DeleteAsync(u));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
